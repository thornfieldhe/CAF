﻿namespace CAF.FS.Core.Data.Table
{

    /// <summary>
    /// 多张表带静态实例化的上下文
    /// </summary>
    /// <typeparam name="TPo"></typeparam>
    public class TableContext<TPo> : TableContext where TPo : TableContext<TPo>, new()
    {
        /// <summary>
        /// 使用DB特性设置数据库信息
        /// </summary>
        protected TableContext() { }

        /// <summary>
        /// 通过数据库配置，连接数据库
        /// </summary>
        /// <param name="dbIndex">数据库选项</param>
        protected TableContext(int dbIndex) : base(dbIndex) { }

        /// <summary>
        /// 通过自定义数据链接符，连接数据库
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="commandTimeout">SQL执行超时时间</param>
        protected TableContext(string connectionString, DataBaseType dbType = DataBaseType.SqlServer, int commandTimeout = 30) : base(connectionString, dbType, commandTimeout) { }

        /// <summary>
        /// 静态实例
        /// </summary>
        public static TPo Data { get { return new TPo { IsMergeCommand = false }; } }
    }
}
