using System;
using System.Collections;
using System.Collections.Generic;

namespace CAF
{
    using CAF.Model;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// 集合业务对象
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    [Serializable]
    public class CollectionBase<TCollection, TMember> : IEnumerable<TMember>, IList<TMember>, IEnumerable

        where TCollection : CollectionBase<TCollection, TMember>
        where TMember : BaseEntity<TMember>
    {
        protected List<TMember> _items;

        public CollectionBase()
        {
            _items = new List<TMember>();
        }

        internal CollectionBase(List<TMember> items)
        {
            _items = items;
        }

        #region 基本状态
        internal bool _isNew = false;
        internal bool _isDirty = false;

        internal bool IsNew { get { return _isNew; } set { _isNew = value; } }

        internal bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }


        public delegate int OnSaveHandler(IDbConnection conn, IDbTransaction transaction);        //属性改变事件，用于通知列表，修改状态为Dity
        public event OnSaveHandler OnSaved;
        public delegate void OnDirtyHandler();//更新当前Dirty属性同时更新订阅的父对象的Dity属性
        public event OnDirtyHandler OnMarkDirty;
        public delegate void OnInsertHandler(TMember member);//插入集合对象时执行父对象的PostInsert方法
        public event OnInsertHandler OnInsert;

        internal virtual void MarkNew()
        {
            _isNew = true;
            MarkDirty();
        }


        internal virtual void MarkOld()
        {
            _isNew = false;
            MarkClean();
        }

        internal virtual void MarkClean()
        {
            _isDirty = false;
        }

        internal virtual void MarkDirty()
        {
            _isDirty = true;
            if (OnMarkDirty != null)
            {
                OnMarkDirty();
            }
        }

        #endregion

        #region 表达式

        protected const string QUERY = "SELECT * FROM {0} Where Status!=-1 {1} ";
        protected const string COUNT = "SELECT COUNT(*) AS COUNT FROM {0} Where Status!=-1 {1} ";


        #endregion

        /// <summary>
        /// 添加一个成员岛集合
        /// </summary>
        /// <param name="member">成员实例</param>
        public virtual void Add(TMember member)
        {
            _items.Add(member);
            if (OnInsert!=null)
            {
                OnInsert(member);
            }
            member.OnPropertyChange += MarkDirty;
            MarkDirty();
        }

        /// <summary>
        /// 合并一个业务集合
        /// AddRange方法用于合并两个同类型的集合为一个集合
        /// </summary>
        /// <param name="container">被合并的集合</param>
        public virtual void AddRange(TCollection container)
        {
            CollectionBase<TCollection, TMember> collection = container as CollectionBase<TCollection, TMember>;
            if (collection == null || (collection.Count <= 0))
            {
                return;
            }
            foreach (TMember member in collection)
            {
                _items.Add(member);
                member.OnPropertyChange += MarkDirty;
            }
            MarkDirty();
        }

        /// <summary>
        /// 添加成员列表到集合
        /// </summary>
        /// <param name="members">成员列表</param>
        public virtual void AddRange(TMember[] members)
        {
            if (members != null && members.Length > 0)
            {
                foreach (TMember member in members)
                {
                    _items.Add(member);
                    member.OnPropertyChange += MarkDirty;
                }
                MarkDirty();
            }
        }

        /// <summary>
        /// 添加成员列表到集合
        /// </summary>
        /// <param name="members">成员列表</param>
        public virtual void AddRange(List<TMember> members)
        {
            foreach (TMember member in members)
            {
                _items.Add(member);
                member.OnPropertyChange += MarkDirty;
            }
            MarkDirty();
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
            MarkDirty();
            member.MarkDelete();
            return true;
        }

        /// <summary>
        /// 清除集合内所有成员
        /// </summary>
        public virtual void Clear()
        {
            _items.ForEach(
                member =>
                {
                    member.MarkDelete();
                });
            MarkDirty();
        }

        public void Insert(int index, TMember member)
        {
            _items.Insert(index, member);
            member.OnPropertyChange += MarkDirty;
            MarkDirty();
        }

        public void RemoveAt(int index)
        {
            var member = _items[index];
            if (member == null)
            {
                return;
            }
            member.MarkDelete();
            MarkDirty();
        }

        public void CopyTo(TMember[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 集合成员列表
        /// </summary>
        public TMember[] Members
        {
            get { return _items.ToArray(); }
            set { _items = value.ToList(); }
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
                var normalItems = _items.Where(member => !member.IsDelete).ToList();
                return normalItems[index];
            }
            set
            {
                _items[index] = value;
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
            return _items.Where(item => !item.IsDelete).Contains(member);
        }

        /// <summary>
        /// 实现IEnumerable接口
        /// </summary>
        /// <returns></returns>
        IEnumerator<TMember> IEnumerable<TMember>.GetEnumerator()
        {
            return _items.Where(item => !item.IsDelete).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Where(item => !item.IsDelete).GetEnumerator();
        }

        public int IndexOf(TMember member)
        {
            return _items.IndexOf(member);
        }


        #region 数据库操作方法

        public int Save()
        {
            int i = 0;
            if (IsDirty)
            {
                using (IDbConnection conn = SqlService.Instance.Connection)
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        i = SaveChanges(conn, transaction);
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
            int i = 0;
            if (IsDirty)
            {
                i += PreSubmit(conn, transaction);
                i += Submit(conn, transaction);
                i += PostSubmit(conn, transaction);
                if (OnSaved != null)
                {
                    i += OnSaved(conn, transaction);
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
            int rows = 0;
            if (_isDirty)
            {
                bool isValid = true;
                this._items.ForEach(
                    member =>
                    {
                        if (!member.IsValid)
                        {
                            isValid = false;
                        }
                    });
                if (isValid)
                {
                    _items.ForEach(
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

    }
}