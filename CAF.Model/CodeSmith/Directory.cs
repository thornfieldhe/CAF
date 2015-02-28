﻿using System;
using System.Linq;

namespace CAF.Model.Model
{
    using CAF.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    [Serializable]
    public partial class Directory : BaseEntity<Directory>
    {
        public Directory()
        {
            base.MarkNew();
        }


        #region 公共属性

        private string _name = String.Empty;
        private string _url = String.Empty;
        private Guid? _parentId = Guid.Empty;
        private string _level = String.Empty;
        private int _sort;

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不允许为空")]
        [StringLength(50, ErrorMessage = "名称长度不能超过50")]
        public string Name
        {
            get { return _name; }
            set { SetProperty("Name", ref _name, value); }
        }

        /// <summary>
        /// Url地址
        /// </summary>
        [StringLength(100, ErrorMessage = "Url地址长度不能超过100")]
        public string Url
        {
            get { return _url; }
            set { SetProperty("Url", ref _url, value); }
        }

        /// <summary>
        /// 父目录
        /// </summary>
        public Guid? ParentId
        {
            get { return _parentId; }
            set { SetProperty("ParentId", ref _parentId, value); }
        }

        /// <summary>
        /// 父目录
        /// </summary>
        public Directory Parent
        {
            get
            {
                return !ParentId.HasValue ? null : Directory.Get(ParentId.Value);
            }
        }


        /// <summary>
        /// 层级
        /// </summary>
        [StringLength(20, ErrorMessage = "层级长度不能超过20")]
        public string Level
        {
            get { return _level; }
            set { SetProperty("Level", ref _level, value); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不允许为空")]
        public int Sort
        {
            get { return _sort; }
            set { SetProperty("Sort", ref _sort, value); }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_Directory WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_Directory WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_Directory SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_Directory WHERE Id = @Id";
        const string QUERY_INSERT = "INSERT INTO Sys_Directory (Id, Name, Url, ParentId, Level, Sort, Note, Status, CreatedDate, ChangedDate) VALUES (@Id, @Name, @Url, @ParentId, @Level, @Sort, @Note, @Status, @CreatedDate, @ChangedDate)";
        const string QUERY_UPDATE = "UPDATE Sys_Directory SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static Directory Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<Directory>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<Directory>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                return item;
            }
        }

        public static DirectoryList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory>(QUERY_GETAll, null).ToList();
                var list = new DirectoryList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                list.MarkOld();
                return list;
            }
        }


        /// <summary>
        /// 直接删除
        /// </summary>
        /// <returns></returns>
        public static int Delete(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Execute(QUERY_DELETE, new { Id = id });
            }
        }

        public static bool Exists(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(QUERY_EXISTS, new { Id = id }).Single() >= 1;
            }
        }

        #endregion


        internal override int Delete(IDbConnection conn, IDbTransaction transaction)
        {
            base.MarkDelete();
            return conn.Execute(QUERY_DELETE, new { Id = Id }, transaction, null, null);
        }

        internal override int Update(IDbConnection conn, IDbTransaction transaction)
        {
            if (!IsDirty)
            {
                return _changedRows;
            }
            _updateParameters += ", ChangedDate = GetDate()";
            _updateParameters += ", Status = @Status";
            var query = String.Format(QUERY_UPDATE, _updateParameters.TrimStart(','));
            _changedRows += conn.Execute(query, this, transaction, null, null);
            return _changedRows;
        }

        internal override int Insert(IDbConnection conn, IDbTransaction transaction)
        {
            _changedRows += conn.Execute(QUERY_INSERT, this, transaction, null, null);
            return _changedRows;
        }

        #region 私有方法

        #endregion

    }

    [Serializable]
    public class DirectoryList : CollectionBase<DirectoryList, Directory>
    {
        public DirectoryList() { }

        protected const string tableName = "Sys_Directory";

        public static DirectoryList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<Directory>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new DirectoryList();
                foreach (var item in items)
                {
                    item.MarkOld();
                    list.Add(item);
                }
                return list;
            }
        }

        public static int QueryCount(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single();
            }
        }

        public static bool Exists(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(string.Format(COUNT, tableName, query), dynamicObj).Single() > 0;
            }
        }
    }
}

