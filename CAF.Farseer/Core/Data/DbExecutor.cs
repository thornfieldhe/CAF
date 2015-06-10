using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace CAF.FS.Core.Data
{
    /// <summary>
    ///     数据库操作
    /// </summary>
    public class DbExecutor : IDisposable
    {
        /// <summary>
        ///     数据库执行时间，单位秒
        /// </summary>
        private readonly int _commandTimeout;

        /// <summary>
        ///     连接字符串
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        ///     数据类型
        /// </summary>
        public readonly DataBaseType DataType;

        /// <summary>
        ///     数据提供者
        /// </summary>
        public DbProviderFactory Factory;

        /// <summary>
        ///     是否开启事务
        /// </summary>
        public bool IsTransaction;

        /// <summary>
        ///     Sql执行对像
        /// </summary>
        private DbCommand comm;

        /// <summary>
        ///     数据库连接对像
        /// </summary>
        private DbConnection conn;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandTimeout">数据库执行时间，单位秒</param>
        /// <param name="tranLevel">开启事务等级</param>
        public DbExecutor(string connectionString, DataBaseType dbType = DataBaseType.SqlServer, int commandTimeout = 30, IsolationLevel tranLevel = IsolationLevel.Unspecified)
        {
            this._connectionString = connectionString;
            this._commandTimeout = commandTimeout;
            this.DataType = dbType;

            this.OpenTran(tranLevel);
        }


        /// <summary>
        ///     开启事务。
        /// </summary>
        /// <param name="tranLevel">事务方式</param>
        public void OpenTran(IsolationLevel tranLevel)
        {
            if (tranLevel != IsolationLevel.Unspecified)
            {
                this.Open();
                this.comm.Transaction = this.conn.BeginTransaction(tranLevel);
                this.IsTransaction = true;
            }
        }

        /// <summary>
        ///     关闭事务。
        /// </summary>
        public void CloseTran()
        {
            if (this.IsTransaction && this.comm != null && this.comm.Transaction != null) { this.comm.Transaction.Dispose(); }
            this.IsTransaction = false;
        }

        /// <summary>
        ///     打开数据库连接
        /// </summary>
        public void Open()
        {
            if (this.conn == null)
            {
                this.Factory = DbFactory.CreateDbProviderFactory(this.DataType);
                this.comm = this.Factory.CreateCommand();
                this.comm.CommandTimeout = this._commandTimeout;

                this.conn = this.Factory.CreateConnection();
                this.conn.ConnectionString = this._connectionString;
                this.comm.Connection = this.conn;
            }
            if (this.conn.State == ConnectionState.Closed)
            {
                this.conn.Open();
                this.comm.Parameters.Clear();
            }
        }

        /// <summary>
        ///     关闭数据库连接
        /// </summary>
        public void Close(bool dispose)
        {
            if (this.comm != null)
            {
                this.comm.Parameters.Clear();
            }
            if ((dispose || this.comm.Transaction == null) && this.conn != null && this.conn.State != ConnectionState.Closed)
            {
                this.comm.Dispose();
                this.comm = null;
                this.conn.Close();
                this.conn.Dispose();
                this.conn = null;
            }
        }

        /// <summary>
        ///     提交事务
        ///     如果未开启事务则会引发异常
        /// </summary>
        public void Commit()
        {
            if (this.comm.Transaction == null)
            {
                throw new Exception("未开启事务");
            }
            this.comm.Transaction.Commit();
        }

        /// <summary>
        ///     返回第一行第一列数据
        /// </summary>
        /// <param name="cmdType">执行方式</param>
        /// <param name="cmdText">SQL或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText)) { return null; }
            try
            {
                this.Open();
                this.comm.CommandType = cmdType;
                this.comm.CommandText = cmdText;
                if (parameters != null) { this.comm.Parameters.AddRange(parameters); }

                return this.comm.ExecuteScalar();
            }
            finally
            {
                this.Close(false);
            }
        }

        /// <summary>
        ///     返回受影响的行数
        /// </summary>
        /// <param name="cmdType">执行方式</param>
        /// <param name="cmdText">SQL或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText)) { return 0; }
            try
            {
                this.Open();
                this.comm.CommandType = cmdType;
                this.comm.CommandText = cmdText;
                if (parameters != null) { this.comm.Parameters.AddRange(parameters); }

                return this.comm.ExecuteNonQuery();
            }
            finally
            {
                this.Close(false);
            }
        }

        /// <summary>
        ///     返回数据(IDataReader)
        /// </summary>
        /// <param name="cmdType">执行方式</param>
        /// <param name="cmdText">SQL或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public IDataReader GetReader(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText)) { return null; }
            try
            {
                this.Open();
                this.comm.CommandType = cmdType;
                this.comm.CommandText = cmdText;
                if (parameters != null) { this.comm.Parameters.AddRange(parameters); }

                return this.IsTransaction ? this.comm.ExecuteReader() : this.comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
            } // Close();
        }

        /// <summary>
        ///     返回数据(DataSet)
        /// </summary>
        /// <param name="cmdType">执行方式</param>
        /// <param name="cmdText">SQL或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:检查 SQL 查询是否存在安全漏洞")]
        public DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText)) { return new DataSet(); }
            try
            {
                this.Open();
                this.comm.CommandType = cmdType;
                this.comm.CommandText = cmdText;
                if (parameters != null) { this.comm.Parameters.AddRange(parameters); }
                var ada = this.Factory.CreateDataAdapter();
                ada.SelectCommand = this.comm;
                var ds = new DataSet();
                ada.Fill(ds);
                return ds;
            }
            finally
            {
                this.Close(false);
            }
        }

        /// <summary>
        ///     返回数据(DataTable)
        /// </summary>
        /// <param name="cmdType">执行方式</param>
        /// <param name="cmdText">SQL或者存储过程名称</param>
        /// <param name="parameters">参数</param>
        public DataTable GetDataTable(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            var ds = this.GetDataSet(cmdType, cmdText, parameters);
            return ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0];
        }

        /// <summary>
        /// 指量操作数据（仅支付Sql Server)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据</param>
        public void ExecuteSqlBulkCopy(string tableName, DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) { return; }

            try
            {
                this.Open();
                using (var bulkCopy = new SqlBulkCopy((SqlConnection)this.conn, SqlBulkCopyOptions.Default, (SqlTransaction)this.comm.Transaction))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    bulkCopy.BulkCopyTimeout = 3600;
                    bulkCopy.WriteToServer(dt);
                }
            }
            finally { this.Close(false); }
        }

        protected virtual void Dispose(bool disposing)
        {
            //释放托管资源
            if (disposing) { this.Close(true); }
        }

        /// <summary>
        ///     注销
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary> 数据库类型 </summary>
    public enum DataBaseType
    {
        /// <summary> SqlServer数据库 </summary>
        [Display(Name = "System.Data.SqlClient")]
        SqlServer,
        /// <summary> Access数据库 </summary>
        [Display(Name = "System.Data.OleDb")]
        OleDb,
        /// <summary> MySql数据库 </summary>
        [Display(Name = "MySql.Data.MySqlClient")]
        MySql,
        /// <summary> SQLite </summary>
        [Display(Name = "System.Data.SQLite")]
        SQLite,
        /// <summary> Oracle </summary>
        [Display(Name = "System.Data.OracleClient")]
        Oracle,
    }

    /// <summary> 字段类型 </summary>
    public enum FieldType
    {
        /// <summary> 整型 </summary>
        [Display(Name = "Int")]
        Int,
        /// <summary> 布尔型 </summary>
        [Display(Name = "Bit")]
        Bit,
        /// <summary> 可变字符串 </summary>
        [Display(Name = "Varchar")]
        Varchar,
        /// <summary> 可变字符串（双字节） </summary>
        [Display(Name = "Nvarchar")]
        Nvarchar,
        /// <summary> 不可变字符串 </summary>
        [Display(Name = "Char")]
        Char,
        /// <summary> 不可变字符串（双字节） </summary>
        [Display(Name = "NChar")]
        NChar,
        /// <summary> 不可变文本 </summary>
        [Display(Name = "Text")]
        Text,
        /// <summary> 不可变文本 </summary>
        [Display(Name = "Ntext")]
        Ntext,
        /// <summary> 日期 </summary>
        [Display(Name = "DateTime")]
        DateTime,
        /// <summary> 短整型 </summary>
        [Display(Name = "Smallint")]
        Smallint,
        /// <summary> 浮点 </summary>
        [Display(Name = "Float")]
        Float,
    }
}