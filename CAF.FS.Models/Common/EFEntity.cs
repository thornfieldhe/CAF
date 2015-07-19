namespace CAF.Models
{
    using EntityFramework.Caching;
    using EntityFramework.Extensions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class EFEntity<K> : BaseEntity<K>, IDbAction where K : EFEntity<K>, IEntityBase, new()
    {
        [NotMapped]
        internal Context DbContex { get; set; }

        #region 构造函数

        protected EFEntity(Guid id)
            : base(id)
        {
            this.DbContex = ContextWapper.Instance.Context;
            this.Init();
        }

        protected EFEntity() : this(Guid.NewGuid()) { }

        #endregion

        #region 实例方法


        public int Create()
        {
            this.PreInsert();
            this.Validate();
            this.Insert();
            this.PostInsert();
            return this.DbContex.SaveChanges();
        }

        public int Save()
        {
            this.PreUpdate();
            this.Validate();
            this.Update();
            this.PostUpdate();
            return this.DbContex.SaveChanges();
        }

        public int Delete()
        {
            this.PreRemove();
            this.Remove();
            this.PostRemove();
            return this.DbContex.SaveChanges();
        }

        public K Find(Guid id)
        {
            return this.QuerySingle(i => i.Id == id);
        }

        #region 模块内部方法

        internal List<K> Query(Expression<Func<K, bool>> func, bool useCache = false)
        {
            var query = this.DbContex.Set<K>().Where(func);
            var items = this.PreQuery(query, useCache);
            return this.PostQuery(items);
        }

        internal List<K> Query(bool useCache = false)
        {
            var query = this.DbContex.Set<K>();
            var items = this.PreQuery(query, useCache);
            return this.PostQuery(items);
        }

        internal K QuerySingle(Expression<Func<K, bool>> func)
        {
            var query = this.DbContex.Set<K>().Where(func);
            var item = this.PreQuerySingle(query);
            return this.PostQuerySingle(item);
        }

        #endregion


        #region 继承方法

        protected virtual void Init() { }

        protected virtual void Update() { this.ChangedDate = DateTime.Now; }

        protected virtual void Insert() { this.DbContex.Set<K>().Add(this as K); }

        protected virtual void Remove()
        {
            this.Status = -1;
            this.ChangedDate = DateTime.Now;
        }

        protected virtual void PreInsert() { }

        protected virtual void PreUpdate() { this.ChangedDate = DateTime.Now; }

        protected virtual List<K> PreQuery(IQueryable<K> query, bool useCache = false)
        {
            var items = useCache
                            ? query.FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromSeconds(60))).ToList()
                            : query.ToList();
            return this.PostQuery(items);
        }

        protected virtual K PreQuerySingle(IQueryable<K> query)
        {
            var item = query.FirstOrDefault();
            return item;
        }

        protected virtual int PreRemove() { return 0; }

        protected virtual int PostUpdate() { return 0; }

        protected virtual int PostRemove() { return 0; }

        protected virtual int PostInsert() { return 0; }
  

        protected virtual List<K> PostQuery(List<K> items)
        {
            items.ForEach(i => i.DbContex = this.DbContex);
            return items;
        }

        protected virtual K PostQuerySingle(K item)
        {
            item.IfNotNull(i => i.DbContex = this.DbContex);
            return item;
        }

        #endregion

        #endregion

        #region 静态方法

        public static K Get(Guid id)
        {
            return new K().QuerySingle(i => i.Id == id);
        }

        public static List<K> GetAll(bool useCache = false)
        {
            return new K().Query(useCache);
        }


        public static List<K> Get(Expression<Func<K, bool>> func, bool useCache = false)
        {
            return new K().Query(func, useCache);
        }


        public static List<K> Pages<T>(ref Pager pager, Func<K, bool> whereFunc, Func<K, T> orderByFunc, bool isAsc)
        {
            var context = ContextWapper.Instance.Context;
            pager.Total = context.Set<K>().Where(whereFunc).Count();
            List<K> result;
            if (isAsc)
            {
                result =
                    context.Set<K>()
                        .Where(whereFunc)
                        .OrderBy(orderByFunc)
                        .Skip(pager.PageSize * (pager.PageIndex - 1))
                        .Take(pager.PageIndex)
                        .ToList();
            }
            else
            {
                result =
                    context.Set<K>()
                        .Where(whereFunc)
                        .OrderByDescending(orderByFunc)
                        .Skip(pager.PageSize * (pager.PageIndex - 1))
                        .Take(pager.PageIndex)
                        .ToList();
            }
            return result;
        }

        public static K GetById(Guid id)
        {
            var context = ContextWapper.Instance.Context;
            var item = context.Set<K>().Find(id);
            item.DbContex = context;
            return item;
        }

        public static bool Exist(Expression<Func<K, bool>> func)
        {
            return ContextWapper.Instance.Context.Set<K>().Any(func);
        }

        public static int Count(Expression<Func<K, bool>> func)
        {
            return ContextWapper.Instance.Context.Set<K>().Count(func);
        }

        #endregion
    }

    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}