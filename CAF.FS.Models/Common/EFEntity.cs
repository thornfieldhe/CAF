namespace CAF.FSModels
{
    using EntityFramework.Caching;
    using EntityFramework.Extensions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class EFEntity<K> : BaseEntity<K>, IEntityBase where K : EFEntity<K>, IEntityBase, new()
    {
        [NotMapped]
        internal Context DbContex { get; set; }

        #region 构造函数

        protected EFEntity(Guid id) : base(id) { this.InitContex(); }

        protected EFEntity() : this(Guid.NewGuid()) { }

        #endregion

        #region 实例方法

        protected void InitContex()
        {
            this.DbContex = ContextWapper.Instance.Context;
        }

        public override int Create() { return this.Create(this.DbContex); }

        public override int Save() { return this.Save(this.DbContex); }

        public override int Delete() { return this.Delete(this.DbContex); }

        internal List<K> Query(Expression<Func<K, bool>> func, bool useCache = false)
        {
            var query = this.DbContex.Set<K>().Where(func);
            return this.PostQuery(query, useCache);
        }


        internal List<K> Query(bool useCache = false)
        {
            var query = this.DbContex.Set<K>();
            return this.PostQuery(query, useCache);
        }

        internal K QuerySingle(Expression<Func<K, bool>> func)
        {
            var query = this.DbContex.Set<K>().Where(func);
            return this.PostQuerySingle(query);
        }


        internal virtual int Create(Context context)
        {
            this.PreInsert(context);
            this.Validate();
            this.Insert(context);
            this.PostInsert(context);
            return context.SaveChanges();
        }

        internal virtual int Save(Context context)
        {
            this.PreUpdate(context);
            this.Validate();
            this.Update(context);
            this.PostUpdate(context);
            return context.SaveChanges();
        }

        internal virtual int Delete(Context context)
        {
            this.PreRemove(context);
            this.Remove(context);
            this.PostRemove(context);
            return context.SaveChanges();
        }

        protected virtual void Update(Context context) { this.ChangedDate = DateTime.Now; }

        protected virtual void Insert(Context context) { context.Set<K>().Add(this as K); }

        protected virtual void Remove(Context context)
        {
            this.Status = -1;
            this.ChangedDate = DateTime.Now;
        }

        protected virtual void PreInsert(Context context) { }

        protected virtual void PreUpdate(Context context) { this.ChangedDate = DateTime.Now; }

        protected virtual int PreRemove(Context context) { return 0; }

        protected virtual int PostUpdate(Context context) { return 0; }

        protected virtual int PostRemove(Context context) { return 0; }

        protected virtual int PostInsert(Context context) { return 0; }

        public virtual List<K> PostQuery(IQueryable<K> query, bool useCache = false)
        {
            var items = new List<K>();
            items = useCache ? query.FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromSeconds(60))).ToList() : query.ToList();

            items.ForEach(i => i.DbContex = this.DbContex);
            return items;
        }

        public virtual K PostQuerySingle(IQueryable<K> query)
        {
            var item = query.FirstOrDefault();
            item.IfNotNull(i => i.DbContex = this.DbContex);
            return item;
        }

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