
using CAF.FS.Core.Data;
namespace CAF.FS.Core.Infrastructure
{
    using CAF.FS.Extends;
    using CAF.FS.Mapping.Context;
    using CAF.FS.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class DbReadSet<TSet, TEntity>
        where TSet : DbReadSet<TSet, TEntity>
        where TEntity : class, new()
    {
        protected Queue Queue { get { return this.QueueManger.CreateQueue(this.Name, this.Map); } }
        /// <summary>
        /// 表名/视图名/存储过程名
        /// </summary>
        protected string Name;
        /// <summary>
        /// 实体类映射
        /// </summary>
        protected readonly FieldMap Map;
        /// <summary>
        /// 保存字段映射的信息
        /// </summary>
        protected SetState SetState;
        protected abstract BaseQueueManger QueueManger { get; }
        //private List<TEntity> _lstCurrentCache;

        public DbReadSet()
        {
            this.Map = typeof(TEntity);
        }

        #region 条件筛选器
        /// <summary>
        ///     字段选择器
        /// </summary>
        /// <param name="select">字段选择器</param>
        public virtual TSet Select<T>(Expression<Func<TEntity, T>> select)
        {
            this.Queue.AddSelect(select);
            return (TSet)this;
        }

        /// <summary>
        ///     查询条件
        /// </summary>
        /// <param name="where">查询条件</param>
        public virtual TSet Where(Expression<Func<TEntity, bool>> where)
        {
            this.Queue.AddWhere(where);
            return (TSet)this;
        }

        /// <summary> 自动生成o.ID == ID </summary>
        /// <param name="value">值</param>
        /// <param name="memberName">默认为主键ID属性（非数据库字段名称）</param>
        public virtual TSet Where(int value, string memberName = null)
        {
            memberName = string.IsNullOrWhiteSpace(memberName) ? this.Map.PrimaryState.Key.Name : "ID";
            this.Where(ConvertHelper.CreateBinaryExpression<TEntity>(value, memberName: memberName));
            return (TSet)this;
        }

        /// <summary> 自动生成lstIDs.Contains(o.ID) </summary>
        /// <param name="lstvValues"></param>
        /// <param name="memberName">默认为主键ID属性（非数据库字段名称）</param>
        public virtual TSet Where(List<int> lstvValues, string memberName = null)
        {
            memberName = string.IsNullOrWhiteSpace(memberName) ? this.Map.PrimaryState.Key.Name : "ID";
            this.Where(ConvertHelper.CreateContainsBinaryExpression<TEntity>(lstvValues, memberName: memberName));
            return (TSet)this;
        }

        /// <summary>
        /// 倒序查询
        /// </summary>
        /// <typeparam name="TKey">实体类属性类型</typeparam>
        /// <param name="desc">字段选择器</param>
        public virtual TSet Desc<TKey>(Expression<Func<TEntity, TKey>> desc)
        {
            this.Queue.AddOrderBy(desc, false);
            return (TSet)this;
        }

        /// <summary>
        /// 正序查询
        /// </summary>
        /// <typeparam name="TKey">实体类属性类型</typeparam>
        /// <param name="asc">字段选择器</param>
        public virtual TSet Asc<TKey>(Expression<Func<TEntity, TKey>> asc)
        {
            this.Queue.AddOrderBy(asc, true);
            return (TSet)this;
        }
        #endregion

        #region ToTable
        /// <summary> 查询多条记录（不支持延迟加载） </summary>
        /// <param name="top">限制显示的数量</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        /// <param name="isRand">返回当前条件下随机的数据</param>
        public virtual DataTable ToTable(int top = 0, bool isDistinct = false, bool isRand = false)
        {
            DataTable dt = null;
            this.QueueManger.Append(this.Name, this.Map, (queryQueue) => dt = queryQueue.SqlBuilder.ToList(top, isDistinct, isRand).ExecuteQuery.ToTable(), true);
            return dt;
        }

        /// <summary> 查询多条记录（不支持延迟加载） </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        /// <returns></returns>
        public virtual DataTable ToTable(int pageSize, int pageIndex, bool isDistinct = false)
        {
            #region 计算总页数
            if (pageIndex < 1) { pageIndex = 1; }
            if (pageSize < 0) { pageSize = 20; }
            #endregion

            return this.Queue.SqlBuilder.ToList(pageSize, pageIndex, isDistinct).ExecuteQuery.ToTable();
        }

        /// <summary> 查询多条记录（不支持延迟加载） </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="recordCount">总记录数量</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        public virtual DataTable ToTable(int pageSize, int pageIndex, out int recordCount, bool isDistinct = false)
        {
            var queue = this.Queue;
            recordCount = this.Count();
            this.Queue.Copy(queue);

            #region 计算总页数
            var allCurrentPage = 1;

            if (pageIndex < 1) { pageIndex = 1; }
            if (pageSize < 0) { pageSize = 0; }
            if (pageSize != 0)
            {
                allCurrentPage = (recordCount / pageSize);
                allCurrentPage = ((recordCount % pageSize) != 0 ? allCurrentPage + 1 : allCurrentPage);
                allCurrentPage = (allCurrentPage == 0 ? 1 : allCurrentPage);
            }
            if (pageIndex > allCurrentPage) { pageIndex = allCurrentPage; }
            #endregion

            return this.ToTable(pageSize, pageIndex, isDistinct);
        }
        #endregion

        #region ToList
        /// <summary> 查询多条记录（不支持延迟加载） </summary>
        /// <param name="top">限制显示的数量</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        /// <param name="isRand">返回当前条件下随机的数据</param>
        public virtual List<TEntity> ToList(int top = 0, bool isDistinct = false, bool isRand = false)
        {
            return this.ToTable(top, isDistinct, isRand).ToList<TEntity>();
        }

        /// <summary>
        /// 查询多条记录（不支持延迟加载）
        /// </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        /// <returns></returns>
        public virtual List<TEntity> ToList(int pageSize, int pageIndex, bool isDistinct = false)
        {
            return this.ToTable(pageSize, pageIndex, isDistinct).ToList<TEntity>();
        }

        /// <summary>
        /// 查询多条记录（不支持延迟加载）
        /// </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="recordCount">总记录数量</param>
        /// <param name="isDistinct">返回当前条件下非重复数据</param>
        public virtual List<TEntity> ToList(int pageSize, int pageIndex, out int recordCount, bool isDistinct = false)
        {
            return this.ToTable(pageSize, pageIndex, out recordCount, isDistinct).ToList<TEntity>();
        }
        #endregion

        #region ToSelectList
        /// <summary>
        ///     返回筛选后的列表
        /// </summary>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="T">实体类的属性</typeparam>
        /// <param name="select">字段选择器</param>
        public virtual List<T> ToSelectList<T>(Expression<Func<TEntity, T>> select)
        {
            return this.ToSelectList(0, select);
        }

        /// <summary>
        ///     返回筛选后的列表
        /// </summary>
        /// <param name="top">限制显示的数量</param>
        /// <param name="select">字段选择器</param>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="T">实体类的属性</typeparam>
        public virtual List<T> ToSelectList<T>(int top, Expression<Func<TEntity, T>> select)
        {
            return this.Select(select).ToList(top).Select(select.Compile()).ToList();
        }

        /// <summary>
        ///     返回筛选后的列表
        /// </summary>
        /// <param name="select">字段选择器</param>
        /// <param name="lstIDs">o => IDs.Contains(o.ID)</param>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="T">实体类的属性</typeparam>
        public virtual List<T> ToSelectList<T>(List<T> lstIDs, Expression<Func<TEntity, T>> select)
        {
            this.Where(ConvertHelper.CreateContainsBinaryExpression<TEntity>(lstIDs));
            return this.ToSelectList(select);
        }

        /// <summary>
        ///     返回筛选后的列表
        /// </summary>
        /// <param name="select">字段选择器</param>
        /// <param name="lstIDs">o => IDs.Contains(o.ID)</param>
        /// <param name="top">限制显示的数量</param>
        /// <typeparam name="TEntity">实体类</typeparam>
        /// <typeparam name="T">实体类的属性</typeparam>
        public virtual List<T> ToSelectList<T>(List<T> lstIDs, int top, Expression<Func<TEntity, T>> select)
        {
            this.Where(ConvertHelper.CreateContainsBinaryExpression<TEntity>(lstIDs));
            return this.ToSelectList(top, select);
        }
        #endregion

        #region ToEntity
        /// <summary>
        /// 查询单条记录（不支持延迟加载）
        /// </summary>
        public virtual TEntity ToEntity()
        {
            return this.Queue.SqlBuilder.ToEntity().ExecuteQuery.ToEntity<TEntity>();
        }

        /// <summary>
        ///     获取单条记录
        /// </summary>
        /// <typeparam name="T">ID</typeparam>
        /// <param name="ID">条件，等同于：o=>o.ID.Equals(ID) 的操作</param>
        public virtual TEntity ToEntity<T>(T ID)
        {
            this.Where(ConvertHelper.CreateBinaryExpression<TEntity>(ID));
            return this.ToEntity();
        }
        #endregion

        #region Count

        /// <summary>
        /// 查询数量（不支持延迟加载）
        /// </summary>
        public virtual int Count(bool isDistinct = false, bool isRand = false)
        {
            return this.Queue.SqlBuilder.Count().ExecuteQuery.ToValue<int>();
        }

        /// <summary>
        ///     获取数量
        /// </summary>
        /// <typeparam name="T">ID</typeparam>
        /// <param name="ID">条件，等同于：o=>o.ID.Equals(ID) 的操作</param>
        public virtual int Count<T>(T ID)
        {
            this.Where(ConvertHelper.CreateBinaryExpression<TEntity>(ID));
            return this.Count();
        }

        /// <summary>
        ///     获取数量
        /// </summary>
        /// <typeparam name="T">ID</typeparam>
        /// <param name="lstIDs">条件，等同于：o=> IDs.Contains(o.ID) 的操作</param>
        public virtual int Count<T>(List<T> lstIDs)
        {
            this.Where(ConvertHelper.CreateContainsBinaryExpression<TEntity>(lstIDs));
            return this.Count();
        }

        #endregion

        #region IsHaving

        /// <summary>
        /// 查询数据是否存在（不支持延迟加载）
        /// </summary>
        public virtual bool IsHaving()
        {
            return this.Count() > 0;
        }

        /// <summary>
        ///     判断是否存在记录
        /// </summary>
        /// <typeparam name="T">ID</typeparam>
        /// <param name="ID">条件，等同于：o=>o.ID == ID 的操作</param>
        public virtual bool IsHaving<T>(T ID)
        {
            this.Where(ConvertHelper.CreateBinaryExpression<TEntity>(ID));
            return this.IsHaving();
        }

        /// <summary>
        ///     判断是否存在记录
        /// </summary>
        /// <typeparam name="T">ID</typeparam>
        /// <param name="lstIDs">条件，等同于：o=> IDs.Contains(o.ID) 的操作</param>
        public virtual bool IsHaving<T>(List<T> lstIDs)
        {
            this.Where(ConvertHelper.CreateContainsBinaryExpression<TEntity>(lstIDs));
            return this.IsHaving();
        }

        #endregion

        #region GetValue

        /// <summary>
        /// 查询单个值（不支持延迟加载）
        /// </summary>
        public virtual T GetValue<T>(Expression<Func<TEntity, T>> fieldName, T defValue = default(T))
        {
            if (fieldName == null) { throw new ArgumentNullException("fieldName", "查询Value操作时，fieldName参数不能为空！"); }
            this.Select(fieldName);

            return this.Queue.SqlBuilder.GetValue().ExecuteQuery.ToValue(defValue);
        }

        /// <summary>
        ///     获取数量
        /// </summary>
        /// <typeparam name="T1">ID</typeparam>
        /// <typeparam name="T2">字段类型</typeparam>
        /// <param name="ID">条件，等同于：o=>o.ID.Equals(ID) 的操作</param>
        /// <param name="fieldName">筛选字段</param>
        /// <param name="defValue">不存在时默认值</param>
        public virtual T2 GetValue<T1, T2>(T1 ID, Expression<Func<TEntity, T2>> fieldName, T2 defValue = default(T2))
        {
            this.Where(ConvertHelper.CreateBinaryExpression<TEntity>(ID));
            return this.GetValue(fieldName, defValue);
        }

        #endregion

        #region 聚合
        /// <summary>
        /// 累计和（不支持延迟加载）
        /// </summary>
        public virtual T Sum<T>(Expression<Func<TEntity, T>> fieldName, T defValue = default(T))
        {
            if (fieldName == null) { throw new ArgumentNullException("fieldName", "查询Sum操作时，fieldName参数不能为空！"); }
            this.Select(fieldName);

            return this.Queue.SqlBuilder.Sum().ExecuteQuery.ToValue(defValue);
        }

        /// <summary>
        /// 查询最大数（不支持延迟加载）
        /// </summary>
        public virtual T Max<T>(Expression<Func<TEntity, T>> fieldName, T defValue = default(T))
        {
            if (fieldName == null) { throw new ArgumentNullException("fieldName", "查询Max操作时，fieldName参数不能为空！"); }
            this.Select(fieldName);

            return this.Queue.SqlBuilder.Max().ExecuteQuery.ToValue(defValue);
        }
        /// <summary>
        /// 查询最小数（不支持延迟加载）
        /// </summary>
        public virtual T Min<T>(Expression<Func<TEntity, T>> fieldName, T defValue = default(T))
        {
            if (fieldName == null) { throw new ArgumentNullException("fieldName", "查询Min操作时，fieldName参数不能为空！"); }
            this.Select(fieldName);

            return this.Queue.SqlBuilder.Min().ExecuteQuery.ToValue(defValue);
        }

        #endregion
    }
}
