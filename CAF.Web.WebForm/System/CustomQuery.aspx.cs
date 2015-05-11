using System;

namespace CAF.Web.WebForm.System
{
    using CAF.Model;

    using global::System.Data.SqlClient;

    public partial class CustomQuery : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("21024743-666b-4f3a-8bb1-3e5902386f66");
            base.OnLoad(e);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtSql.Text.Trim()))
                {
                    return;
                }
                using (var conn = SqlService.Instance.Connection)
                {
                    var cmd = new SqlCommand(this.txtSql.Text, conn);
                    var reader = cmd.ExecuteReader();
                    this.grid.DataSource = reader;
                    this.grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                this.CreateErrorLog(ex);
                this.lblError.Text = ex.Message;
            }

        }
    }
}