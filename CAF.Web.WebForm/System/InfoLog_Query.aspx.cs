using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;


    public partial class InfoLog_Query : BasePage
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
            var exp = new ExpConditions<InfoLog>();
            this.txtName.Text.Trim().IfIsNotNullOrEmpty(c => exp.AndWhere(ex => ex.UserName == this.txtName.Text.Trim()));
            this.dateFrom.SelectedDate.HasValue.IfTrue(() => exp.AndWhere(ex => ex.CreatedDate >= this.dateFrom.Text.ToDate()));
            this.dateTo.SelectedDate.HasValue.IfTrue(() => exp.AndWhere(ex => ex.CreatedDate >= this.dateTo.Text.ToDate().AddDays(1)));
            this.grid.BindDataSource(exp);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("5e01fb62-f73b-4a5b-822c-b24f695c7e1e");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
        }

        protected override void Query()
        {
            this.grid_OnQuery();
        }



        #endregion
    }
}
