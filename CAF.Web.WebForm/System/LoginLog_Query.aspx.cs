using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;


    public partial class LoginLog_Query : BasePage
    {

        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            base.Bind();
            this.grid_OnQuery();
        }

        protected void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var exp = new ExpConditions<LoginLog>();
            this.txtName.Text.Trim().IfIsNotNullOrEmpty(r=> exp.AndWhere(ex => ex.UserName == r.Trim()));
            this.dateFrom.SelectedDate.HasValue.IfTrue(() => exp.AndWhere(ex => ex.CreatedDate >= this.dateFrom.Text.ToDate()));
            this.dateTo.SelectedDate.HasValue.IfTrue(() => exp.AndWhere(ex => ex.CreatedDate >= this.dateTo.Text.ToDate().AddDays(1)));
            this.grid.BindDataSource(exp);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("07c50982-a935-4ae3-874e-58847b16608d");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Query()
        {
            this.grid_OnQuery();
        }
        #endregion
    }
}
