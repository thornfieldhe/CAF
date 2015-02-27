using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace CAF.Data
{
    public class SqlDataPortal
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public Database Db { get; private set; }

        public DbConnection Connection { get { return Db.CreateConnection(); } }

        public SqlDataPortal()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }

        public SqlDataPortal(string dbName)
        {
            this.Db = DatabaseFactory.CreateDatabase(dbName);
        }

        #region --- Connection Management ---

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        public void CloseReader(IDataReader reader)
        {
            reader.Close();
        }

        #endregion --- Connection Management ---

        #region --- ExecuteNonQuery ---

        //sqlstring
        public int ExecuteNonQueryBySql(string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return ExecuteNonQuery(cmd, cmdParameters);
        }

        public int ExecuteNonQueryBySql(DbTransaction transaction, string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return ExecuteNonQuery(transaction, cmd, cmdParameters);
        }

        //dbcommand
        public int ExecuteNonQuery(DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var query = 0;
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                query = Db.ExecuteNonQuery(cmd);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return query;
        }

        public int ExecuteNonQuery(DbTransaction transaction, DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var query = 0;
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                query = Db.ExecuteNonQuery(cmd, transaction);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return query;
        }

        //sp
        public int ExecuteNonQueryBySP(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");

            return Db.ExecuteNonQuery(transaction, spName, parameterValues);
        }

        public int ExecuteNonQueryBySP(string spName, params object[] parameterValues)
        {
            var query = 0;
            try
            {
                var cmd = Db.GetStoredProcCommand(spName);

                query = Db.ExecuteNonQuery(spName, parameterValues);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                //if (rethrow)
                //throw;
            }
            return query;
        }

        #endregion --- ExecuteNonQuery ---

        #region --- ExecuteScale ---

        //sqlstring
        public object ExecuteScaleBySql(string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return ExecuteScale(cmd, cmdParameters);
        }

        public object ExecuteScaleBySql(DbTransaction transaction, string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return ExecuteScale(transaction, cmd, cmdParameters);
        }

        //dbcommand
        public object ExecuteScale(DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var ob = new object();
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                ob = Db.ExecuteScalar(cmd);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ob;
        }

        public object ExecuteScale(DbTransaction transaction, DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var ob = new object();
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                ob = Db.ExecuteScalar(cmd, transaction);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ob;
        }

        //sp
        public object ExecuteScaleBySP(DbTransaction transaction, string spName, params object[] parameters)
        {
            var ob = new object();
            try
            {
                ob = Db.ExecuteScalar(transaction, spName, parameters);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ob;
        }

        public object ExecuteScaleBySP(string spName, params object[] paramers)
        {
            var ob = new object();
            try
            {
                ob = Db.ExecuteScalar(spName, paramers);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ob;
        }

        #endregion --- ExecuteScale ---

        #region --- GetDataSet ---

        //sqlstr
        public DataSet GetDataSetBySql(string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return GetDataSet(cmd, cmdParameters);
        }

        public DataSet GetDataSetBySql(DbTransaction transaction, string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return GetDataSet(transaction, cmd, cmdParameters);
        }

        //dbcommand
        public DataSet GetDataSet(DbTransaction transaction, DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var ds = new DataSet();
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                ds = Db.ExecuteDataSet(cmd, transaction);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ds;
        }

        public DataSet GetDataSet(DbCommand cmd, params SqlParameter[] cmdParameters)
        {
            var ds = new DataSet();
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                ds = Db.ExecuteDataSet(cmd);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ds;
        }

        //sp
        public DataSet GetDataSetBySP(string spName, params object[] parameterValues)
        {
            var ds = new DataSet();
            try
            {
                ds = Db.ExecuteDataSet(spName, parameterValues);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ds;
        }

        public DataSet GetDataSetBySP(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            var ds = new DataSet();
            try
            {
                ds = Db.ExecuteDataSet(transaction, spName, parameterValues);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return ds;
        }

        #endregion --- GetDataSet ---

        #region --- GetReader ---

        //sqlstr
        public IDataReader GetReaderBySql(string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return GetReader(cmd, cmdParameters);
        }

        public IDataReader GetReaderBySql(DbTransaction transaction, string sqlstr, params SqlParameter[] cmdParameters)
        {
            var cmd = Db.GetSqlStringCommand(sqlstr);
            return GetReader(transaction, cmd, cmdParameters);
        }

        //dbcommand
        public IDataReader GetReader(DbCommand cmd, SqlParameter[] cmdParameters)
        {
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                return Db.ExecuteReader(cmd);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return null;
        }

        public IDataReader GetReader(DbTransaction transaction, DbCommand cmd, SqlParameter[] cmdParameters)
        {
            try
            {
                if (cmdParameters != null)
                {
                    for (var i = 0; i < cmdParameters.Length; i++)
                    {
                        Db.AddInParameter(cmd, cmdParameters[i].ParameterName, cmdParameters[i].DbType, cmdParameters[i].Value);
                    }
                }
                return Db.ExecuteReader(cmd, transaction);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return null;
        }

        //sp
        public IDataReader GetReaderBySP(string spName, params object[] parameterValues)
        {
            try
            {
                var cmd = Db.GetStoredProcCommand(spName);
                return Db.ExecuteReader(spName, parameterValues);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return null;
        }

        public IDataReader GetReaderBySP(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            try
            {
                return Db.ExecuteReader(transaction, spName, parameterValues);
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return null;
        }

        #endregion --- GetReader ---

        public List<string> DiscoverParameters(string spName)
        {
            var cmd = Db.GetStoredProcCommand(spName);
            Db.DiscoverParameters(cmd);
            var parametersNames = new List<string>();
            foreach (DbParameter parameter in cmd.Parameters)
            {
                if (parameter.Direction == ParameterDirection.Input)
                {
                    parametersNames.Add(parameter.ParameterName.Replace("@", ""));
                }
            }
            return parametersNames;
        }

        #region --- LoadEntities ---

        public virtual List<T> LoadBySp<T>(string spName, params object[] parameters) where T : class
        {
            var reader = GetReaderBySP(spName, parameters);
            return Load<T>(reader);
        }

        public virtual List<T> LoadBySp<T>(DbTransaction transaction, string spName, params object[] parameters) where T : class
        {
            var reader = GetReaderBySP(spName, parameters);
            return Load<T>(reader);
        }

        public virtual List<T> LoadBySql<T>(string sqlstr, params SqlParameter[] parameters) where T : class
        {
            var reader = GetReaderBySql(sqlstr, parameters);
            return Load<T>(reader);
        }

        public virtual List<T> LoadBySql<T>(DbTransaction transaction, string sqlstr, params SqlParameter[] parameters) where T : class
        {
            var reader = GetReaderBySql(transaction, sqlstr, parameters);
            return Load<T>(reader);
        }

        public virtual List<T> LoadByCmd<T>(DbCommand cmd, params SqlParameter[] parameters) where T : class
        {
            var reader = GetReader(cmd, parameters);
            return Load<T>(reader);
        }

        public virtual List<T> LoadByCmd<T>(DbTransaction transaction, DbCommand cmd, params SqlParameter[] parameters) where T : class
        {
            var reader = GetReader(transaction, cmd, parameters);
            return Load<T>(reader);
        }

        /// <summary>
        /// 从Reader填写类的内容
        /// </summary>
        public virtual List<T> Load<T>(IDataReader reader) where T : class
        {
            List<string> set = null;
            var t = typeof(T);
            var entities = new List<T>();
            try
            {
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
                                    info.SetValue(entity, reader[attr.FieldName], BindingFlags.NonPublic | BindingFlags.Public, null, null, null);
                                    //info.SetValue(entity, GetValueInType(reader[attr.FieldName], info.PropertyType, info.Name), null);
                                }
                            }
                        }
                        entities.Add(entity);
                    }
                }
            }
            catch (Exception exception1)
            {
                var rethrow = ExceptionPolicy.HandleException(exception1, "DataBaseException Policy");
                if (rethrow)
                    throw;
            }
            return entities;
        }

        #endregion --- LoadEntities ---

        #region --- EntityExcute ---

        public virtual int ExecuteNonQuery<T>(string spName, TargetException entity)
        {
            return 1;
        }

        public virtual int ExecuteNonQuery<T>(DbTransaction transaction, string spName, TargetException entity)
        {
            return 1;
        }

        #endregion --- EntityExcute ---

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

        #endregion Nested type: SetField
    }
}