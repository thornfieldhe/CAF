using System;
using System.Collections.Generic;
using System.Web;

namespace CAF
{
    public class GenericContex
    {
        class NameBasedDictionary : Dictionary<string, object> { }

        [ThreadStatic]
        private static NameBasedDictionary threadCache;

        private static readonly bool isWeb = CheckWhetherIsWeb();

        private const string ContexKey = "corsicasoft.context.web";

        public GenericContex()
        {
            if (isWeb && (HttpContext.Current.Items[ContexKey] == null))
            {
                HttpContext.Current.Items[ContexKey] = new NameBasedDictionary();
            }
        }

        public object this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }
                NameBasedDictionary cache = GetCache();
                if (cache.Count <= 0)
                {
                    return null;
                }
                object result;
                if (cache.TryGetValue(name, out result))
                {
                    return result;
                }
                else
                    return null;
            }
            set
            {
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                NameBasedDictionary cache = GetCache();
                object temp;
                if (cache.TryGetValue(name, out temp))
                {
                    cache[name] = value;
                }
                else
                {
                    cache.Add(name, value);
                }
            }
        }

        private static NameBasedDictionary GetCache()
        {
            NameBasedDictionary cache;
            if (isWeb)
            {
                cache = (NameBasedDictionary)HttpContext.Current.Items[ContexKey];
            }
            else
            {
                cache = threadCache;
            }
            if (cache == null)
            {
                cache = new NameBasedDictionary();
            }
            if (isWeb)
            {
                HttpContext.Current.Items[ContexKey] = cache;
            }
            else
            {
                threadCache = cache;
            }
            return cache;
        }

        public static bool CheckWhetherIsWeb()
        {
            bool result = false;
            AppDomain domain = AppDomain.CurrentDomain;
            try
            {
                if (domain.ShadowCopyFiles)
                {
                    result = (HttpContext.Current.GetType() != null);
                }
            }
            catch (SystemException)
            {
            }
            return result;
        }
    }
}