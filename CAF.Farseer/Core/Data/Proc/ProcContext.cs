using CAF.FS.Core.Infrastructure;
using System.ComponentModel;
using System.Data;

namespace CAF.FS.Core.Data.Proc
{

    /// <summary>
    /// 多张存储过程上下文
    /// </summary>
    public class ProcContext : BaseContext
    {
        /// <summary>
        /// 使用DB特性设置数据库信息
        /// </summary>
        protected ProcContext()
        {
            this.InstanceProperty();
        }

        /// <summary>
        /// 通过数据库配置，连接数据库
        /// </summary>
        /// <param name="dbIndex">数据库选项</param>
        protected ProcContext(int dbIndex) : base(dbIndex) { this.InstanceProperty(); }

        /// <summary>
        /// 通过自定义数据链接符，连接数据库
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="commandTimeout">SQL执行超时时间</param>
        protected ProcContext(string connectionString, DataBaseType dbType = DataBaseType.SqlServer, int commandTimeout = 30) : base(connectionString, dbType, commandTimeout) { this.InstanceProperty(); }

        /// <summary>
        /// true:启用合并执行命令、并延迟加载
        /// </summary>
        protected internal bool IsMergeCommand { get; set; }

        /// <summary>
        /// 队列管理
        /// </summary>
        protected internal ProcQueueManger QueueManger { get; private set; }

        /// <summary>
        /// 保存修改
        /// IsMergeCommand=true时：只提交一次SQL到数据库
        /// </summary>
        /// <param name="isOlation">默认启用事务操作</param>
        public int SaveChanges(bool isOlation = true)
        {
            // 开启或关闭事务
            if (isOlation) { this.QueueManger.DataBase.OpenTran(IsolationLevel.Serializable); }
            else { this.QueueManger.DataBase.CloseTran(); }

            var result = this.QueueManger.Commit();
            // 如果开启了事务，则关闭
            if (isOlation)
            {
                this.QueueManger.DataBase.Commit();
                this.QueueManger.DataBase.CloseTran();
                this.QueueManger.DataBase.Close(true);
            }
            return result;
        }

        /// <summary>
        /// 实例化子类中，所有Set属性
        /// </summary>
        private void InstanceProperty()
        {
            this.IsMergeCommand = true;
            this.QueueManger = new ProcQueueManger(this.DataBase, this.ContextMap);
            this.InstanceProperty(this, "ProcSet`1");
        }



        /// <summary>
        /// 动态返回TableSet类型
        /// </summary>
        /// <param name="propertyName">当有多个相同类型TEntity时，须使用propertyName来寻找唯一</param>
        /// <typeparam name="TEntity"></typeparam>
        public ProcSet<TEntity> Set<TEntity>(string propertyName = null) where TEntity : class, new()
        {
            var pInfo = this.GetSetPropertyInfo<TEntity>(typeof(ProcSet<TEntity>), propertyName);
            // 找到存在的属性后，返回属性
            return new ProcSet<TEntity>(this, pInfo);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(bool disposing)
        {
            //释放托管资源
            if (disposing)
            {
                this.QueueManger.Dispose();
            }
        }
    }
}
