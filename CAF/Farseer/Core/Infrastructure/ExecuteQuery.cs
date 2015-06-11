using CAF.FS.Core.Data;
using CAF.FS.Extends;
using CAF.FS.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace CAF.FS.Core.Infrastructure
{
    /// <summary>
    /// 将SQL发送到数据库
    /// </summary>
    public class ExecuteQuery
    {
        protected readonly Queue Queue;

        public ExecuteQuery(Queue queue)
        {
            this.Queue = queue;
        }

        /// <summary>
        /// 将OutPut参数赋值到实体
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="entity">实体类</param>
        private void SetParamToEntity<TEntity>(TEntity entity) where TEntity : class,new()
        {
            if (entity == null) { return; }
            var map = CacheManger.GetFieldMap(typeof(TEntity));
            foreach (var kic in map.MapList.Where(o => o.Value.FieldAtt.IsOutParam))
            {
                kic.Key.SetValue(entity, ConvertHelper.ConvertType(this.Queue.Param.Find(o => o.ParameterName == this.Queue.QueueManger.DbProvider.ParamsPrefix + kic.Value.FieldAtt.Name).Value, kic.Key.PropertyType), null);
            }
        }
        /// <summary>
        /// 存储过程创建SQL 输入、输出参数化
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="entity">实体类</param>
        private List<DbParameter> CreateParam<TEntity>(TEntity entity) where TEntity : class,new()
        {
            this.Queue.Param = new List<DbParameter>();
            if (entity == null) { return this.Queue.Param; }

            foreach (var kic in this.Queue.FieldMap.MapList.Where(o => o.Value.FieldAtt.IsInParam || o.Value.FieldAtt.IsOutParam))
            {
                var obj = kic.Key.GetValue(entity, null);
                this.Queue.Param.Add(this.Queue.QueueManger.DbProvider.CreateDbParam(kic.Value.FieldAtt.Name, obj, kic.Key.PropertyType, kic.Value.FieldAtt.IsOutParam));
            }
            return this.Queue.Param;
        }


        /// <summary>
        /// 返回影响行数
        /// </summary>
        public virtual int Execute()
        {
            var param = this.Queue.Param == null ? null : this.Queue.Param.ToArray();
            var result = this.Queue.Sql.Length < 1 ? 0 : this.Queue.QueueManger.DataBase.ExecuteNonQuery(CommandType.Text, this.Queue.Sql.ToString(), param);

            this.Queue.QueueManger.Clear();
            return result;
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        public virtual DataTable ToTable()
        {
            var param = this.Queue.Param == null ? null : this.Queue.Param.ToArray();
            var table = this.Queue.QueueManger.DataBase.GetDataTable(CommandType.Text, this.Queue.Sql.ToString(), param);
            this.Queue.QueueManger.Clear();
            return table;
        }
        /// <summary>
        /// 返回单条数据
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        public virtual TEntity ToEntity<TEntity>() where TEntity : class, new()
        {
            var param = this.Queue.Param == null ? null : this.Queue.Param.ToArray();
            TEntity t;
            using (var reader = this.Queue.QueueManger.DataBase.GetReader(CommandType.Text, this.Queue.Sql.ToString(), param))
            {
                t = reader.ToInfo<TEntity>();
            }
            this.Queue.QueueManger.DataBase.Close(false);

            this.Queue.QueueManger.Clear();
            return t;
        }
        /// <summary>
        /// 查询单个字段值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="defValue">默认值</param>
        public virtual T ToValue<T>(T defValue = default(T))
        {
            var param = this.Queue.Param == null ? null : this.Queue.Param.ToArray();
            var value = this.Queue.QueueManger.DataBase.ExecuteScalar(CommandType.Text, this.Queue.Sql.ToString(), param);
            var t = value.ConvertType(defValue);
            this.Queue.QueueManger.Clear();
            return t;
        }
        /// <summary>
        /// 返回影响行数
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="entity">实体类</param>
        public virtual int Execute<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var param = this.CreateParam(entity).ToArray();
            var result = this.Queue.QueueManger.DataBase.ExecuteNonQuery(CommandType.StoredProcedure, this.Queue.Name, param);
            this.SetParamToEntity(entity);

            this.Queue.QueueManger.Clear();
            return result;
        }
        /// <summary>
        /// 返回列表
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="entity">实体类</param>
        public virtual List<TEntity> ToList<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var param = this.CreateParam(entity).ToArray();
            List<TEntity> lst;
            using (var reader = this.Queue.QueueManger.DataBase.GetReader(CommandType.StoredProcedure, this.Queue.Name, param))
            {
                lst = reader.ToList<TEntity>();
            }
            this.Queue.QueueManger.DataBase.Close(false);
            this.SetParamToEntity(entity);
            this.Queue.QueueManger.Clear();
            return lst;
        }
        /// <summary>
        /// 单条记录
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <param name="entity">实体类</param>
        public virtual TEntity ToEntity<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var param = this.CreateParam(entity).ToArray();
            TEntity t;
            using (var reader = this.Queue.QueueManger.DataBase.GetReader(CommandType.StoredProcedure, this.Queue.Name, param))
            {
                t = reader.ToInfo<TEntity>();
            }
            this.Queue.QueueManger.DataBase.Close(false);

            this.SetParamToEntity(entity);
            this.Queue.QueueManger.Clear();
            return t;
        }
        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="entity">实体类</param>
        /// <param name="defValue"></param>
        public virtual T ToValue<TEntity, T>(TEntity entity, T defValue = default(T)) where TEntity : class, new()
        {
            var param = this.CreateParam(entity).ToArray();
            var value = this.Queue.QueueManger.DataBase.ExecuteScalar(CommandType.StoredProcedure, this.Queue.Name, param);
            var t = value.ConvertType(defValue);

            this.SetParamToEntity(entity);
            this.Queue.QueueManger.Clear();
            return t;
        }
    }
}
