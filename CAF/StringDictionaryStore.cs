using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAF.Core;

namespace CAF
{
    /// <summary>
    /// 基于字符串的字典
    /// </summary>
    public class StringDictionaryStore : IDictionaryStore
    {
        public void Refersh() { }

        public virtual IDictionary<string, string> Data { get; set; }

        public void Find(DictionaryContext contex)
        {
            if (Data == null)
            {
                throw new NullReferenceException("Data");
            }
            string value;
            switch (contex.Operator)
            {
                case 'F':
                    if (!Data.TryGetValue(contex.Key, out value))
                    {
                        contex.Value = string.Empty;
                    }
                    else
                    {
                        contex.Value = value;
                    }
                    break;
                case 'T':
                    value = contex.Value;
                    foreach (string key in Data.Keys)
                    {
                        if (string.Equals(Data[key], value))
                        {
                            contex.Key = key;
                            return;
                        }
                        else
                        {
                            contex.Key = string.Empty;
                        }
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }

    /// <summary>
    /// 字典上下文
    /// </summary>
    public class DictionaryContext
    {
        public string Key;
        public string Value;

        /// <summary>
        /// 'T'(to) 根据Value获得Key
        /// 'F'(from)根据Key获得Value
        /// </summary>
        public char Operator;
    }
}
