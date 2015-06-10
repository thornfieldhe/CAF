using System;
using System.Collections;
using System.Collections.Generic;

namespace CAF
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 集合业务对象
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    [Serializable]
    public abstract class CollectionBase<TCollection, TMember> : IList<TMember>, ITableName, IBaseStatus, ICollectionBase<TCollection, TMember>
        , IDisposable,INotifyCollectionChanged
        where TCollection : CollectionBase<TCollection, TMember>
        where TMember : BaseEntity<TMember>
    {
        protected List<TMember> _items;

        public CollectionBase()
        {
            this._items = new List<TMember>();
        }

        internal CollectionBase(List<TMember> items)
        {
            this._items = items;
        }

        [NonSerialized]
        protected IDbConnection _connection;
        public IDbConnection Connection { get { return this._connection; } set { this._connection = value; } }


        #region 基本状态
        internal bool _isNew = false;
        internal bool _isDirty = false;

        internal bool IsNew { get { return this._isNew; } set { this._isNew = value; } }

        public bool IsClean
        {
            get { throw new NotImplementedException(); }
        }

        bool IBaseStatus.IsDirty
        {
            get { return this.IsDirty; }
            set { this.IsDirty = value; }
        }

        internal bool IsDirty { get { return this._isDirty; } set { this._isDirty = value; } }

        internal bool _isChangeRelationship;
        /// <summary>
        /// true：只更新关系
        /// false：标记删除
        /// </summary>
        public bool IsChangeRelationship
        {
            get { return this._isChangeRelationship; }
            set
            {
                this._isChangeRelationship = value;
                this._isChangeRelationship.IfTrue(() => this._items.ForEach(i => i.IsChangeRelationship = true));
            }
        }

        public delegate int OnSaveHandler(IDbConnection conn, IDbTransaction transaction);        //属性改变事件，用于通知列表，修改状态为Dity
        public event OnSaveHandler OnSaved;
        public delegate void OnDirtyHandler();//更新当前Dirty属性同时更新订阅的父对象的Dity属性
        public event OnDirtyHandler OnMarkDirty;
        public delegate void OnInsertHandler(TMember member);//插入集合对象时执行父对象的PostInsert方法
        public event OnInsertHandler OnInsert;

        public void MarkDirty()
        {
            this._isDirty = true;
            if (this.OnMarkDirty != null)
            {
                this.OnMarkDirty();
            }
        }

        public virtual void MarkNew()
        {
            this._isNew = true;
            this.MarkDirty();
        }


        public virtual void MarkOld()
        {
            this._isNew = false;
            this.MarkClean();
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void MarkClean()
        {
            this._isDirty = false;
        }

        public virtual void OnItemChanged(object sender,PropertyChangedEventArgs args)
        {
            this._isDirty = true;
            if (this.OnMarkDirty != null)
            {
                this.OnMarkDirty();
            }
        }

        #endregion

        #region 表达式

        protected const string QUERY = "SELECT * FROM {0} Where Status!=-1 AND {1} ";
        protected const string COUNT = "SELECT COUNT(*) AS COUNT FROM {0} Where Status!=-1 AND {1} ";


        #endregion

        /// <summary>
        /// 添加一个成员岛集合
        /// </summary>
        /// <param name="member">成员实例</param>
        public virtual void Add(TMember member)
        {
            member.IsChangeRelationship = this.IsChangeRelationship;
            this._items.Add(member);
            if (this.OnInsert != null)
            {
                this.OnInsert(member);
            }
            member.PropertyChanged += this.OnItemChanged;
            this.MarkDirty();
        }

        /// <summary>
        /// 合并一个业务集合
        /// AddRange方法用于合并两个同类型的集合为一个集合
        /// </summary>
        /// <param name="container">被合并的集合</param>
        public virtual void AddRange(TCollection container)
        {
            var collection = container as CollectionBase<TCollection, TMember>;
            if (collection == null || (collection.Count <= 0))
            {
                return;
            }
            foreach (var member in collection)
            {
                member.IsChangeRelationship = this.IsChangeRelationship;
                this.Add(member);
                member.PropertyChanged += this.OnItemChanged;
            }
            this.MarkDirty();
        }

        /// <summary>
        /// 添加成员列表到集合
        /// </summary>
        /// <param name="members">成员列表</param>
        public virtual void AddRange(TMember[] members)
        {
            if (members != null && members.Length > 0)
            {
                foreach (var member in members)
                {
                    member.IsChangeRelationship = this.IsChangeRelationship;
                    this.Add(member);
                    member.PropertyChanged += this.OnItemChanged;
                }
                this.MarkDirty();
            }
        }

        /// <summary>
        /// 添加成员列表到集合
        /// </summary>
        /// <param name="members">成员列表</param>
        public virtual void AddRange(List<TMember> members)
        {
            foreach (var member in members)
            {
                member.IsChangeRelationship = this.IsChangeRelationship;
                this.Add(member);
                member.PropertyChanged += this.OnItemChanged;
            }
            this.MarkDirty();
        }

        /// <summary>
        /// 移出一个成员
        /// </summary>
        /// <param name="member">成员实例</param>
        public virtual bool Remove(TMember member)
        {
            if (!this._items.Contains(member))
            {
                return false;
            }
            this.MarkDirty();
            member.MarkDelete();
            return true;
        }

        public int Add(object value)
        {
            var member = value as TMember;
            this.Add(member);
            return this.Members.Length - 1;
        }

        public bool Contains(object value)
        {
            var member = value as TMember;
            return this.Members.Contains(member);
        }

        /// <summary>
        /// 清除集合内所有成员
        /// </summary>
        public virtual void Clear()
        {
            this._items.ForEach(
                member =>
                {
                    member.MarkDelete();
                });
            this.MarkDirty();
        }

        public int IndexOf(object value)
        {
            var member = value as TMember;
            return Array.IndexOf(this.Members, member);
        }

        public void Insert(int index, object value)
        {
            var member = value as TMember;
            this.Insert(index,member);
        }

        public void Remove(object value)
        {
            var member = value as TMember;
            this.Remove(member); 
            
        }

        public void Insert(int index, TMember member)
        {
            this._items.Insert(index, member);
            member.PropertyChanged += this.OnItemChanged;
            this.MarkDirty();
        }

        public void RemoveAt(int index)
        {
            var member = this._items[index];
            if (member == null)
            {
                return;
            }
            member.MarkDelete();
            this.MarkDirty();
        }

        public void CopyTo(TMember[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 集合成员列表
        /// </summary>
        public TMember[] Members
        {
            get { return this._items.ToArray(); }
            set { this._items = value.ToList(); }
        }

        /// <summary>
        /// 获取指定序号成员
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TMember this[int index]
        {
            get
            {
                var normalItems = this._items.Where(member => !member.IsDelete).ToList();
                return normalItems[index];
            }
            set
            {
                this._items[index] = value;
            }
        }


        /// <summary>
        /// 集合内成员数量
        /// </summary>
        public int Count
        {
            get { return this._items.Count(member => !member.IsDelete); }
        }


        public bool IsReadOnly { get; private set; }


        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool Contains(TMember member)
        {
            return this._items.Where(item => !item.IsDelete).Contains(member);
        }

        /// <summary>
        /// 实现IEnumerable接口
        /// </summary>
        /// <returns></returns>
        IEnumerator<TMember> IEnumerable<TMember>.GetEnumerator()
        {
            return this._items.Where(item => !item.IsDelete).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.Where(item => !item.IsDelete).GetEnumerator();
        }

        public int IndexOf(TMember member)
        {
            return this._items.IndexOf(member);
        }

        public string TableName { get; protected set; }
        #region 数据库操作方法

        public int Save()
        {
            var i = 0;
            if (this.IsDirty)
            {
                using (var conn = this.Connection)
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        i = this.SaveChanges(conn, transaction);
                        transaction.Commit();
                        return i;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return i;
        }

        public int SaveChanges(IDbConnection conn, IDbTransaction transaction)
        {
            var i = 0;
            if (this.IsDirty)
            {
                i += this.PreSubmit(conn, transaction);
                i += this.Submit(conn, transaction);
                i += this.PostSubmit(conn, transaction);
                if (this.OnSaved != null)
                {
                    i += this.OnSaved(conn, transaction);
                }
            }
            return i;
        }

        protected virtual int PreSubmit(IDbConnection conn, IDbTransaction transaction)
        {
            return 0;
        }

        protected virtual int Submit(IDbConnection conn, IDbTransaction transaction)
        {
            var rows = 0;
            if (this._isDirty)
            {
                var isValid = true;
                this._items.ForEach(
                    member =>
                    {
                        member.Validate();
                        isValid = false;
                    });
                if (isValid)
                {
                    this._items.ForEach(
                        member =>
                        {
                            rows += member.SaveChange(conn, transaction);
                        });
                }
            }
            return rows;
        }
        protected virtual int PostSubmit(IDbConnection conn, IDbTransaction transaction)
        {
            return 0;
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            for (var i = 0; i < this.Count; i++)
            {
                this[i] = default(TMember);
            }
            this.Clear();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}