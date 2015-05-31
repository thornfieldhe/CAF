using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CAF.Data
{
    /// <summary>
    /// 数据映射基类
    /// </summary>
    [Serializable]
    public abstract class EntityMapBase
    {
        /// <summary>
        /// 初始化默认值
        /// </summary>
        protected virtual void AssignDefault()
        {
            // virtual method or abstract method?
        }

        /// <summary>
        /// 从Reader填写类的内容
        /// </summary>
        public virtual List<T> Load<T>(IDataReader reader) where T : class
        {
            List<string> set = null;
            var t = typeof(T);
            var entities = new List<T>();

            using (reader)
            {
                while (reader.Read())
                {
                    var entity = Activator.CreateInstance<T>();
                    foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        // Auto load fields which has a DbFieldAttribute
                        var attr =
                            Attribute.GetCustomAttribute(info, typeof(DbPropertyAttribute)) as DbPropertyAttribute;
                        if (attr != null)
                        {
                            if (set == null)
                            {
                                var schema = reader.GetSchemaTable();
                                set = new List<string>(schema.Rows.Count);

                                foreach (DataRow row in schema.Rows)
                                {
                                    var prop = schema.Columns["ColumnName"];
                                    var colName = row[prop].ToString().ToLower();
                                    set.Add(colName);
                                }
                            }
                            if (set.Contains(attr.FieldName.ToLower()) && !Convert.IsDBNull(reader[attr.FieldName]))
                            {
                                info.SetValue(entity, GetValueInType(reader[attr.FieldName], info.PropertyType, info.Name), null);
                            }
                        }
                    }
                    entities.Add(entity);
                }
            }
            return entities;
        }

        protected void LoadField(IDataReader reader, string fieldName, SetField setField)
        {
            //!Convert.IsDBNull(reader["file"]))用于判断数据库字段是否为空
            if (!Convert.IsDBNull(reader[fieldName]))
            {
                setField(reader[fieldName]);
            }
        }

        private object GetValueInType(object value, Type type, string fieldName)
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

        private SqlParameter[] CreateParameters<T>(T entity)
        {
            var parameters = new List<SqlParameter>();
            var t = entity.GetType();
            foreach (var info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var attr =
                    Attribute.GetCustomAttribute(info, typeof(DbPropertyAttribute)) as DbPropertyAttribute;
                if (attr != null)
                {
                    var parameter = new SqlParameter(attr.FieldName, info.GetValue(entity, null));
                    parameters.Add(parameter);
                }
            }
            return parameters.ToArray();
        }

        #region Nested type: SetField

        protected delegate void SetField(object value);

        #endregion
    }
}