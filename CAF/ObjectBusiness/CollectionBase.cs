using System;
using System.Collections;
using System.Collections.Generic;

namespace CAF
{
    using FS.Core.Infrastructure;
    using System.Linq;


    /// <summary>
    /// 集合业务对象
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    [Serializable]
    public abstract class CollectionBase<TCollection, TMember> : IList<TMember>, IBaseStatus, ICollectionBase<TCollection, TMember>
        , IDisposable
        where TCollection : CollectionBase<TCollection, TMember>
        where TMember : class, IEntityStatus, IBusinessBase, IEntityBase
    {
        protected List<TMember> _items;

        #region 构造函数

        protected CollectionBase()
        {
            this.IsReadOnly = false;
            this._items = new List<TMember>();
        }

        protected CollectionBase(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
            this._items = new List<TMember>();
        }

        internal CollectionBase(List<TMember> items, bool isReadOnly)
        {
            this._items = items;
            this.IsReadOnly = isReadOnly;
        }

        #endregion

        #region 基本状态

        internal bool _isNew = false;
        internal bool _isDirty = false;

        public bool IsNew { get { return this._isNew; } set { this._isNew = value; } }


        public bool IsClean
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDirty { get { return this._isDirty; } set { this._isDirty = value; } }


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


        public virtual void MarkClean()
        {
            this._isNew = false;
            this._isDirty = false;
        }

        #endregion

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


        public delegate void OnDirtyHandler();//更新当前Dirty属性同时更新订阅的父对象的Dity属性
        public event OnDirtyHandler OnMarkDirty;


        /// <summary>
        /// 添加一个成员岛集合
        /// </summary>
        /// <param name="member">成员实例</param>
        public virtual void Add(TMember member)
        {
            member.IsChangeRelationship = this.IsChangeRelationship;
            this._items.Add(member);
            member.OnPropertyChanged += this.MarkDirty;
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
                member.OnPropertyChanged += this.MarkDirty;
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
                    member.OnPropertyChanged += this.MarkDirty;
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
                member.OnPropertyChanged += this.MarkDirty;
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
            if (member == null)
            {
                return this.Members.Length;
            }
            this.Add(member);
            member.OnPropertyChanged += this.MarkDirty;
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
            this.Insert(index, member);
        }

        public void Remove(object value)
        {
            var member = value as TMember;
            this.Remove(member);

        }

        public void Insert(int index, TMember member)
        {
            this._items.Insert(index, member);
            member.OnPropertyChanged += this.MarkDirty;
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

        public bool IsReadOnly { get; protected set; }

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

    }
}