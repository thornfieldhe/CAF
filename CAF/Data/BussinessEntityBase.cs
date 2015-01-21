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
            Type t = typeof(T);
            List<T> entities = new List<T>();

            using (reader)
            {
                while (reader.Read())
                {
                    T entity = Activator.CreateInstance<T>();
                    foreach (PropertyInfo info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        // Auto load fields which has a DbFieldAttribute
                        DbPropertyAttribute attr =
                            Attribute.GetCustomAttribute(info, typeof(DbPropertyAttribute)) as DbPropertyAttribute;
                        if (attr != null)
                        {
                            if (set == null)
                            {
                                DataTable schema = reader.GetSchemaTable();
                                set = new List<string>(schema.Rows.Count);

                                foreach (DataRow row in schema.Rows)
                                {
                                    DataColumn prop = schema.Columns["ColumnName"];
                                    string colName = row[prop].ToString().ToLower();
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
                string message =
                    string.Format(
                        "System.FormatException: Input ({0}) was not in a correct format of type ({1}) when loading field ({2})",
                        value.ToString(), type.ToString(), fieldName);

                throw new Exception(message, ex);
            }
        }

        private SqlParameter[] CreateParameters<T>(T entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            Type t = entity.GetType();
            foreach (PropertyInfo info in t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                DbPropertyAttribute attr =
                    Attribute.GetCustomAttribute(info, typeof(DbPropertyAttribute)) as DbPropertyAttribute;
                if (attr != null)
                {
                    SqlParameter parameter = new SqlParameter(attr.FieldName, info.GetValue(entity, null));
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