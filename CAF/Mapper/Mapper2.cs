using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF
{
    using CAF.Data;
    using System.Data;
    using System.Reflection;

    /// <summary>
    /// 自定义映射
    /// 包括字典到实体
    /// 包括实体到DataTable
    /// IDataReader到Entity
    /// </summary>
    public static class Mapper2
    {


        public static TDestination Map<TDestination>(IDictionary<string, string> source, TDestination target) where TDestination : class
        {
            var k = typeof(TDestination);
            if (source == null)
            {
                return null;
            }
            foreach (var item in source.Where(item => k.GetProperty(item.Key) != null && k.GetProperty(item.Key).CanWrite))
            {
                k.GetProperty(item.Key).SetValue(target, GetType(k.GetProperty(item.Key), item.Value), BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
            }
            return target;
        }

        public static DataTable Map<TSource>(IEnumerable<TSource> sourcelist) where TSource : class
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

        /// <summary>
        /// 从Reader填写类的内容
        /// </summary>
        public static List<TDestination> Map<TDestination>(IDataReader reader) where TDestination : class
        {
            List<string> set = null;
            var t = typeof(TDestination);
            var entities = new List<TDestination>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = Activator.CreateInstance<TDestination>();
                    foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        // Auto load fields which has a DbFieldAttribute
                        var attr =
                            Attribute.GetCustomAttribute(info, typeof(DbPropertyAttribute)) as DbPropertyAttribute;
                        if (attr == null)
                        {
                            continue;
                        }
                        if (set == null)
                        {
                            var schema = reader.GetSchemaTable();
                            set = new List<string>(schema.Rows.Count);
                            set.AddRange(from DataRow row in schema.Rows let prop = schema.Columns["ColumnName"] select row[prop].ToString().ToLower());
                        }
                        if (set.Contains(attr.FieldName.ToLower()) && !Convert.IsDBNull(reader[attr.FieldName]))
                        {
                            info.SetValue(entity, GetValueInType(reader[attr.FieldName], info.PropertyType, info.Name), null);
                        }
                    }
                    entities.Add(entity);
                }
            }
            return entities;
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
                    switch (val.ToLower())
                    {
                        case "true":
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


        private static object GetValueInType(object value, Type type, string fieldName)
        {
            try
            {
                return Convert.ChangeType(value, type);
            }
            catch (FormatException ex)
            {
                var message =
                    string.Format(
                        "System.FormatException: Input ({0}) was not in a correct format of type ({1}) when loading field ({2})",
                        value.ToString(), type.ToString(), fieldName);

                throw new Exception(message, ex);
            }
        }

    }
}
