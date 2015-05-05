using System.Linq;

namespace CAF.Model
{
    using CAF.Data;
    using System.Data;

    public partial class PostUserOrganize
    {
        protected override void PreInsert(IDbConnection conn, IDbTransaction transaction)
        {
            this.PreChange(conn, transaction);
        }

        protected override void PreUpdate(IDbConnection conn, IDbTransaction transaction)
        {
            this.PreChange(conn, transaction);
        }

        private void PreChange(IDbConnection conn, IDbTransaction transaction)
        {
            const string EXISTS = "SELECT Count(*) FROM Sys_PostUserOrganizes WHERE UserId = @UserId AND PostId = @PostId AND OrganizeId = @OrganizeId AND Status!=-1";

            var exist = conn.Query<int>(EXISTS, new { UserId = this.UserId, PostId = this.PostId, OrganizeId = this.OrganizeId }, transaction).Single() > 0;
            if (!exist)
            {
                return;
            }
            this.IsValid = false;
            this.Errors.Add("该岗位用户部门配置已存在！");
        }
    }
}
