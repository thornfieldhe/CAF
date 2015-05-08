
namespace CAF.Model
{
    using CAF.Data;
    using CAF.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public partial class User
    {
        protected override void PreInsert(IDbConnection conn, IDbTransaction transaction)
        {
            this.Abb = this.Name.GetChineseSpell();
            this.Pass = Password.DesEncrypt(this.Pass);
        }

        protected override void PreUpdate(IDbConnection conn, IDbTransaction transaction)
        {
            this._updateParameters.Contains("Name").IfIsTrue(() => this.Abb = this.Name.GetChineseSpell());
            this._updateParameters.Contains("Pass").IfIsTrue(() => this.Pass = Password.DesEncrypt(this.Pass));
        }

        public static User Get(string name, string password)
        {
            const string query = "SELECT Top 1 * FROM Sys_Users WHERE LoginName = @LoginName And Pass=@Pass";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<User>(query, new { LoginName = name.Trim(), Pass = password.Trim() }).FirstOrDefault();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
                item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                return item;
            }
        }

        public static User Get(string name)
        {
            const string query = "SELECT Top 1 * FROM Sys_Users WHERE Name = @Name Or LoginName=@Name";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                var item = conn.Query<User>(query, new { Name = name.Trim() }).FirstOrDefault();
                if (item == null)
                {
                    return null;
                }
                item.MarkOld();
                item._userSettingInitalizer = new Lazy<UserSetting>(() => InitUserSetting(item), isThreadSafe: true);
                item._roleListInitalizer = new Lazy<RoleList>(() => InitRoles(item), isThreadSafe: true);
                return item;
            }
        }

        public static bool Exists(string name)
        {
            const string query = "SELECT Count(*) FROM Sys_Users WHERE Name Like '%'+@Name+'%' Or Abb Like '%'+@Abb+'%'";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(query, new { Name = name.Trim(), Abb = name.Trim().ToLower() }).Single() >= 1;
            }
        }

        public static List<KeyValueItem<Guid,string>> GetDirectories(Guid userId)
        {
            const string query = @"select distinct t1.Id as UserId,t5.Id as [Key],t5.Level as Value from Sys_Users t1 inner join Sys_R_User_Role t2 on t1.Id=t2.UserId
                                inner join Sys_Roles t3 on t2.RoleId=t3.Id
                                inner join Sys_RE_Directory_Role t4 on t3.Id=t4.RoleId
                                inner join Sys_Directories t5 on t4.DirectoryId=t5.Id
                                where t1.Status!=-1 and t2.Status!=-1 and t3.Status!=-1 and t4.Status!=-1 and t5.Status=0 
                                and t1.Id=@Id";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<KeyValueItem<Guid, string>>(query, new { Id = userId }).ToList();
            }
        }
    }

    public partial class ReadOnlyUser
    {
        public string StatusName
        {
            get
            {
                return RichEnumContent.GetDescription<UserStatusEnum>(this.Status);
            }
        }
    }

}
