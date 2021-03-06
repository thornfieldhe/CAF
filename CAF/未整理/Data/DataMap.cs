﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CAF.Data
{
    using System.Linq;

    public static class DataMap
    {
        public static K Map<T, K>(T source)
            where T : class
            where K : class
        {
            var t = typeof(T);
            var k = typeof(K);
            //K target = Activator.CreateInstance<K>();
            if (source == null)
            {
                return null;
            }
            var target = (K)Activator.CreateInstance(k, true);
            foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(info => k.GetProperty(info.Name) != null && k.GetProperty(info.Name).CanWrite && info.CanWrite && info.Name != "Item")) {
                k.GetProperty(info.Name).SetValue(target, info.GetValue(source, null), BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
                //            myFieldInfo.SetValue(myObject, "New value", BindingFlags.Public, null, null);
            }
            return target;
        }

        public static K Map<T, K>(T source, K target)
            where T : class
            where K : class
        {
            var t = typeof(T);
            var k = typeof(K);
            if (source == null)
            {
                return null;
            }
            if (target==null)
            {
                target = (K)Activator.CreateInstance(k, true);
            }
            foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.NonPublic).Where(info => k.GetProperty(info.Name) != null && k.GetProperty(info.Name).CanWrite && info.CanWrite && info.Name != "Item")) {
                k.GetProperty(info.Name).SetValue(target, info.GetValue(source, null), BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
                //            myFieldInfo.SetValue(myObject, "New value", BindingFlags.Public, null, null);
            }
            return target;
        }

        public static List<K> Map<T, K>(List<T> sources)
            where T : class
            where K : class
        {
            var targets = new List<K>();
            if (sources == null)
            {
                return targets;
            }
            foreach (var source in sources)
            {
                var t = typeof(T);
                var k = typeof(K);
                //K target = Activator.CreateInstance<K>();
                var target = (K)Activator.CreateInstance(k, true);
                foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(info => k.GetProperty(info.Name) != null && k.GetProperty(info.Name).CanWrite && info.Name != "Item")) {
                    k.GetProperty(info.Name).SetValue(target, info.GetValue(source, null), BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
                }
                targets.Add(target);
            }
            return targets;
        }

        public static K Map<K>(IDictionary<string, string> source, K target) where K : class
        {
            var k = typeof(K);
            if (source == null)
            {
                return null;
            }
            foreach (var item in source.Where(item => k.GetProperty(item.Key) != null && k.GetProperty(item.Key).CanWrite)) {
                k.GetProperty(item.Key).SetValue(target, GetType(k.GetProperty(item.Key), item.Value), BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
            }
            return target;
        }

        public static DataTable Map<T>(IEnumerable<T> sourcelist)
        {
            var dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (sourcelist == null)
                return dtReturn;

            foreach (var rec in sourcelist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                             == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static object GetType(PropertyInfo Info, string val)
        {
            if (val == null)
            {
                return null;
            }
            else
            {
                val = val.Trim();
                if (Info.PropertyType == typeof(string))
                {
                    return val;
                }
                else if (Info.PropertyType == typeof(int) || Info.PropertyType == typeof(int?))
                {
                    switch (val.ToLower()) { case "true":
                            val = "1";
                            break;
                        case "false":
                            val = "0";
                            break;
                    }
                    var temp = 0;
                    int.TryParse(val, out temp);
                    return temp;
                }
                else if (Info.PropertyType == typeof(long) || Info.PropertyType == typeof(long?))
                {
                    long temp = 0;
                    long.TryParse(val, out temp);
                    return temp;
                }
                else if (Info.PropertyType == typeof(decimal) || Info.PropertyType == typeof(decimal?))
                {
                    var temp = 0m;
                    decimal.TryParse(val, out temp);
                    return temp;
                }
                else if (Info.PropertyType == typeof(DateTime) || Info.PropertyType == typeof(DateTime?))
                {
                    var temp = new DateTime(2000, 1, 1);
                    if (DateTime.TryParse(val, out temp))
                    {
                        return temp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (Info.PropertyType == typeof(double) || Info.PropertyType == typeof(double?))
                {
                    var temp = 0.0d;
                    double.TryParse(val, out temp);
                    return temp;
                }
                else if (Info.PropertyType == typeof(bool))
                {
                    return Convert.ToBoolean(val);
                }
                else if (Info.PropertyType == typeof(Guid) || Info.PropertyType == typeof(Guid?))
                {
                    Guid temp;
                    if (Guid.TryParse(val, out temp))
                    {
                        return temp;
                    }
                    return null;
                }
                return val;
            }
        }

        public static string GetStr(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }
    }
}