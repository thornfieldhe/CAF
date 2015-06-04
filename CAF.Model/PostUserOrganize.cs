using System.Linq;

namespace CAF.Model
{
    using System.ComponentModel.DataAnnotations;

    using CAF.Data;
    using System.Data;

    using CAF.Validations;

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
            this.AddValidationRule(new EmptyErrorValidateionRule("该岗位用户部门配置已存在！"));
            this.Validate();
        }

    }
}
