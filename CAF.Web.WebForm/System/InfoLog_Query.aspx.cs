using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using FineUI;

    public partial class InfoLog_Query : BasePage
    {
    
        #region 系统生成
        
        protected override void Bind()
        {
            //绑定查询条件

            base.Bind();
            this.grid_OnQuery();
        }
        
        protected void grid_OnQuery(object sender=null, EventArgs e=null)
        {
            var criteria = new InfoLog();
            var where = "1=1";
            this.txtName.Text.Trim().IfIsNotNullOrEmpty(c =>
            {
                where += " And UserName=@UserName";
                criteria.UserName = this.txtName.Text.Trim();
            }
                    );
            this.dateFrom.SelectedDate.HasValue.IfIsTrue(() =>
            {
                where += " And CreatedDate>=@Action";
                criteria.Action = this.dateFrom.Text;
            });
            this.dateTo.SelectedDate.HasValue.IfIsTrue(() =>
            {
                where += " And CreatedDate<@Page";
                criteria.Page = this.dateTo.SelectedDate.Value.AddDays(1).ToShortDateString();
            });
            this.grid.BindDataSource(criteria, where: where);
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
