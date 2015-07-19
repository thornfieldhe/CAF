using System;

namespace CAF.Web.WebForm.System
{

    using FineUI;

    using global::System.Data.SqlClient;

    public partial class ExcuteSql : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("fa4f7a1b-a8e0-47a8-b333-9c131c3c8128");
            base.OnLoad(e);
        }

        protected override void Bind()
        {
            this.btnSave.ConfirmText = "确认执行？";
            base.Bind();
        }

        protected override void Save()
        {
            if (string.IsNullOrWhiteSpace(this.txtSqlString.Text.Trim()))
            {
                Alert.Show("请输入SQL语句！");
            }
            else
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this.txtSqlString.Text.Trim()))
                    {
                        return;
                    }
                    using (var conn = SqlService.Instance.Connection)
                    {
                        var cmd = new SqlCommand(this.txtSqlString.Text, conn);
                        this.lblResult.Text = this.chkIsExcute.Checked ? cmd.ExecuteScalar().ToString() : cmd.ExecuteNonQuery().ToString();
                    }
                }
                catch (Exception ex)
                {
                    this.CreateErrorLog(ex);
                    Alert.Show(ex.Message);
                }
            }
        }
    }
}