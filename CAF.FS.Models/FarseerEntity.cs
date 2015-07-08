
namespace CAF.FSModels
{
    using FS.Core.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public abstract class FarseerEntity<T, K> : BaseEntity<FarseerEntity<T, K>>,
        IEntityBase
        where T : FarseerEntity<T, K>, IEntityBase, IEntityStatus, IBusinessBase, new()
        where K : CollectionBase<K, T>, new()
    {
        #region 构造函数

        protected FarseerEntity(Guid id):base(id){} 
        protected FarseerEntity():this(Guid.NewGuid()){} 

        #endregion

        #region 实例方法

        public override int Create()
        {
            using (var contex = new Context())
            {
                return this.Create(contex);
            }
        }

        public override int Save()
        {
            using (var contex = new Context())
            {
                return this.Save(contex);
            }
        }

        public override int Delete()
        {
            using (var contex = new Context())
            {
                return this.Delete(contex);
            }
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        public override int SubmitChange()
        {
            this._changedRows = 0;
            using (var contex = new Context())
            {
                this._changedRows += this.SubmitChange(contex);
                return this._changedRows;
            }
        }

        internal virtual int Create(Context contex)
        {
            this._changedRows = 0;
            this._changedRows += this.PreInsert(contex);
            this.Validate();
            this._changedRows += this.Insert(contex);
            this._changedRows += this.PostInsert(contex);
            contex.SaveChanges();
            this.MarkClean();
            return this._changedRows;
        }

        internal virtual int Save(Context contex)
        {
            if (!this.IsDirty)
            {
                return this._changedRows;
            }
            this._changedRows = 0;
            this._changedRows += this.PreUpdate(contex);
            this.Validate();
            this._changedRows += this.Update(contex);
            this._changedRows += this.PostUpdate(contex);
            contex.SaveChanges();
            this.MarkClean();
            return this._changedRows;
        }

        internal virtual int Delete(Context contex)
        {
            this._changedRows = 0;
            this._changedRows += this.PreRemove(contex);
            this._changedRows += this.Remove(contex);
            this._changedRows += this.PostRemove(contex);
            contex.SaveChanges();
            this.MarkDelete();
            return this._changedRows;
        }

        /// <summary>
        /// 适用于子元素更新
        /// 和不需要知道元素状态的更新
        /// </summary>
        /// <returns></returns>
        internal virtual int SubmitChange(Context contex)
        {
            if (this.IsDelete && !this.IsChangeRelationship)
            {
                return this.Delete(contex);
            }
            else if (this.IsNew)
            {
                return this.Insert(contex);
            }
            else if (this.IsDirty)
            {
                return this.Update(contex);
            }
            return this._changedRows;
        }


        protected virtual int Update(Context contex)
        {
            contex.TableSet<T>().Update(this as T);
            return 0;
        }

        protected virtual int Insert(Context contex)
        {
            contex.TableSet<T>().Insert(this as T);
            return 0;
        }

        protected virtual int Remove(Context contex)
        {
            contex.TableSet<T>().Delete(this as T);
            return 0;
        }

        protected virtual int PreInsert(Context contex) { return 0; }

        protected virtual int PreUpdate(Context contex)
        {
            this.ChangedDate = DateTime.Now;
            return 0;
        }

        protected virtual int PreRemove(Context contex) { return 0; }

        protected virtual int PostGet(Context contex) { return 0; }
        protected virtual int PostUpdate(Context contex) { return 0; }

        protected virtual int PostRemove(Context contex) { return 0; }

        protected virtual int PostInsert(Context contex) { return 0; }

        #endregion

        #region 静态方法

        public static T Get(Guid id)
        {
            using (var contex = new Context())
            {
                var item = contex.TableSet<T>().Where(t => t.Id == id).ToEntity();
                if (item == null)
                {
                    return null;
                }
                item.PostGet(contex);
                return item;
            }
        }
        public static K GetAll()
        {
            using (var contex = new Context())
            {
                var items = contex.TableSet<T>().ToList();
                return GetList(items, contex);
            }
        }
        public static K Get(Expression<Func<T, bool>> func)
        {
            using (var contex = new Context())
            {
                var items = contex.TableSet<T>().Where(func).ToList();
                return GetList(items, contex);
            }
        }

        public static bool Exist(Expression<Func<T, bool>> func) { return Context.Data.TableSet<T>().Where(func).IsHaving(); }
        public static void Delete(Expression<Func<T, bool>> func) { Context.Data.TableSet<T>().Where(func).Delete(); }
        public static int Count(Expression<Func<T, bool>> func) { return Context.Data.TableSet<T>().Where(func).Count(); }

        private static K GetList(List<T> items, Context contex)
        {
            var result = new K();
            if (items.Count <= 0)
            {
                return result;
            }
            items.ForEach(i => i.PostGet(contex));
            result.AddRange(items);
            return result;
        }

        #endregion
    }
}
