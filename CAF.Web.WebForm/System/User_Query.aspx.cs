using System;

namespace CAF.Web.WebForm
{
    using CAF;
    using CAF.Ext;
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;
    using global::System.Linq;

    public partial class User_Query : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("5CCA7546-79EB-4AAE-A7F9-90F9E660A3A6");
            base.OnLoad(e);
            this.grid.OnQuery += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindOrganizes(new Guid(), this.dropDeps, new Guid().ToString(), true);
            PageTools.BindDropdownList(Role.GetSimpleRoleList(), this.dropRoles, new Guid().ToString());
            PageTools.BindDropdownList(typeof(UserStatusEnum), this.dropStatus);
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("User_Edit.aspx", "新增");
        }

        protected override void Query() { this.grid_OnQuery(); }

        protected override void Delete() { this.grid.Delete<User>(); }


        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<Model.User>(e);
        }

        protected void btnLockRows_Click(object sender, EventArgs e) { this.Update((int)UserStatusEnum.Locked); }

        protected void btnUnLockRows_Click(object sender, EventArgs e) { this.Update((int)UserStatusEnum.Normal); }


        #region 自定义方法

        private void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var exp = new ExpConditions<ReadOnlyUser>();
      
            this.dropDeps.SelectedValue.IfIsNotNullOrEmpty(r => exp.AndWhere(u =>
                    u.Level.Contains(this.dropDeps.SelectedValue)));

            this.dropRoles.SelectedValue.ToGuid().IsEmptuy()
                .IfFalse(()=>exp.AndWhere(u => u.Roles.Contains(this.dropRoles.SelectedValue)));
       
            this.dropStatus.SelectedValue.IfIsNotNullOrEmpty(t =>
                exp.AndWhere(u => u.Status == int.Parse(this.dropStatus.SelectedValue)));

            this.txtName.Text.Trim()
                .IfIsNotNullOrEmpty(
                    t =>
                    exp.AndWhere(u =>
                        u.Name.Contains(this.txtName.Text.Trim()) || u.Abb.Contains(this.txtName.Text.Trim().ToUpper())));

            this.grid.BindDataSource(exp);
        }


        private void Update(int cmd)
        {
            try
            {
                var list = this.grid.SelectedRowIndexArray;
                if (list.Length == 0)
                {
                    Alert.ShowInTop("请选择用户！");
                }
                else
                {
                    foreach (var u in list.Select(i => Model.User.Get(new Guid(this.grid.Rows[i].DataKeys[0].ToString()))))
                    {
                        u.Status = cmd;
                        u.Save();
                        if (u.Errors.Count > 0)
                        {
                            Alert.ShowInTop(u.Errors[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
            this.grid_OnQuery();
        }

        #endregion


    }
}