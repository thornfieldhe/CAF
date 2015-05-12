using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;

    using global::System.Dynamic;

    public partial class ErrorLog_Query : BasePage
    {

        #region 系统生成

        protected override void Bind()
        {
            //绑定查询条件

            base.Bind();
            this.grid_OnQuery();
        }

        protected override void Query() { this.grid_OnQuery(); }

        protected void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var where = "1=1";
            dynamic criteria = new
                                   {
                                       UserName = this.txtName.Text.Trim(),
                                       CreatedDateFrom = this.dateFrom.Text.ToDate(),
                                       CreatedDateTo = this.dateTo.Text.ToDate().AddDays(1).ToShortDateString()
                                   };
            this.txtName.Text.Trim().IfIsNotNullOrEmpty(c =>where += " And UserName=@UserName");
            this.dateFrom.SelectedDate.HasValue.IfIsTrue(()=> where += " And CreatedDate>=@CreatedDateFrom");
            this.dateTo.SelectedDate.HasValue.IfIsTrue(() =>where += " And CreatedDate<@CreatedDateTo");
            this.grid.BindDataSource<ErrorLog>(criteria, where: where);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("514d089c-c034-4ae7-8e6f-9156ba000566");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }



        #endregion
    }
}
