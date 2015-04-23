
namespace CAF.Model
{
    using CAF.Data;
    using System.Data;
    using System.Linq;

    public partial class User
    {
        public static bool Exists(string name)
        {
            const string query = "SELECT Count(*) FROM Sys_Users WHERE Name Like '%'+@Name+'%' Or Abb Like '%'+@Abb+'%'";

            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                return conn.Query<int>(query, new { Name = name.Trim(), Abb = name.Trim().ToLower() }).Single() >= 1;
            }
        }

        protected override void PreInsert(IDbConnection conn, IDbTransaction transaction)
        {
            this.Abb = this.Name.GetChineseSpell();
        }

        protected override void PreUpdate(IDbConnection conn, IDbTransaction transaction)
        {
            this._updateParameters.Contains("Name").IfIsTrue(() => this.Abb = this.Name.GetChineseSpell());
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
