using System;
using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using CAF.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    [Serializable]
    public partial class ErrorLog : BaseEntity<ErrorLog>
    {
        public ErrorLog()
        {
            base.MarkNew();
        }


        #region 公共属性

        private DateTime _logTime;
        private string _userName = String.Empty;
        private int _pageCode;
        private string _page = String.Empty;
        private string _ip = String.Empty;
        private string _message = String.Empty;
        private string _details = String.Empty;

        /// <summary>
        /// 日志时间
        /// </summary>
        [Required(ErrorMessage = "日志时间不允许为空")]
        [DateTimeRequired(ErrorMessage = "日志时间不允许为空")]
        public DateTime LogTime
        {
            get { return _logTime; }
            set { SetProperty("LogTime", ref _logTime, value); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不允许为空")]
        [StringLength(20, ErrorMessage = "用户名长度不能超过20")]
        public string UserName
        {
            get { return _userName; }
            set { SetProperty("UserName", ref _userName, value); }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        [Required(ErrorMessage = "错误代码不允许为空")]
        public int PageCode
        {
            get { return _pageCode; }
            set { SetProperty("PageCode", ref _pageCode, value); }
        }

        /// <summary>
        /// 错误页
        /// </summary>
        [StringLength(50, ErrorMessage = "错误页长度不能超过50")]
        public string Page
        {
            get { return _page; }
            set { SetProperty("Page", ref _page, value); }
        }

        [Required(ErrorMessage = "Ip不允许为空")]
        [StringLength(20, ErrorMessage = "Ip长度不能超过20")]
        public string Ip
        {
            get { return _ip; }
            set { SetProperty("Ip", ref _ip, value); }
        }

        /// <summary>
        /// 错误
        /// </summary>
        [Required(ErrorMessage = "错误不允许为空")]
        [StringLength(200, ErrorMessage = "错误长度不能超过200")]
        public string Message
        {
            get { return _message; }
            set { SetProperty("Message", ref _message, value); }
        }

        /// <summary>
        /// 详细错误
        /// </summary>
        [Required(ErrorMessage = "详细错误不允许为空")]
        [StringLength(16, ErrorMessage = "详细错误长度不能超过16")]
        public string Details
        {
            get { return _details; }
            set { SetProperty("Details", ref _details, value); }
        }


        #endregion

        #region 常量定义

        const string QUERY_GETBYID = "SELECT Top 1 * FROM Sys_ErrorLog WHERE Id = @Id  AND Status!=-1";
        const string QUERY_GETAll = "SELECT * FROM Sys_ErrorLog WHERE  Status!=-1";
        const string QUERY_DELETE = "UPDATE Sys_ErrorLog SET Status=-1 WHERE Id = @Id AND  Status!=-1";
        const string QUERY_EXISTS = "SELECT Count(*) FROM Sys_ErrorLog WHERE Id = @Id";
        const string QUERY_INSERT = "INSERT INTO Sys_ErrorLog (Id, LogTime, UserName, PageCode, Page, Ip, Message, Details) VALUES (@Id, @LogTime, @UserName, @PageCode, @Page, @Ip, @Message, @Details)";
        const string QUERY_UPDATE = "UPDATE Sys_ErrorLog SET {0} WHERE  Id = @Id";

        #endregion

        #region 静态方法

        public static ErrorLog Get(Guid id)
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<ErrorLog>(QUERY_GETBYID, new { Id = id }).SingleOrDefault<ErrorLog>();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                return item;
            }
        }

        public static ErrorLogList GetAll()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<ErrorLog>(QUERY_GETAll, null).ToList();
                var list = new ErrorLogList();
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
    public class ErrorLogList : CollectionBase<ErrorLogList, ErrorLog>
    {
        public ErrorLogList() { }

        protected const string tableName = "Sys_ErrorLog";

        public static ErrorLogList Query(Object dynamicObj, string query = " 1=1")
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var items = conn.Query<ErrorLog>(string.Format(QUERY, tableName, query), dynamicObj).ToList();

                var list = new ErrorLogList();
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


