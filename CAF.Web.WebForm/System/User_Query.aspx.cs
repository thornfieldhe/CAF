using System;

namespace CAF.Web.WebForm
{
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
            this.btnQuery.Click += this.grid_OnQuery;
            this.winEdit.Close += this.grid_OnQuery;
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindDirectories(new Guid(), this.dropDeps, new Guid().ToString(), true);
            PageTools.BindDropdownList(Role.GetSimpleRoleList(), this.dropRoles, new Guid().ToString());
            PageTools.BindDropdownList(typeof(UserStatusEnum), this.dropStatus);
            this.grid_OnQuery();
            this.btnNew.OnClientClick = this.winEdit.GetShowReference("User_Edit.aspx", "新增");
        }


        protected void btnDeleteRows_Click(object sender, EventArgs e)
        {
            this.grid.Delete<User>();
        }
        
        protected void gridRowCommand(object sender, GridCommandEventArgs e)
        {
            this.grid.Excute<Model.User>(e);
        }

        protected void btnLockRows_Click(object sender, EventArgs e) { this.Update((int)UserStatusEnum.Locked); }

        protected void btnUnLockRows_Click(object sender, EventArgs e) { this.Update((int)UserStatusEnum.Normal); }


        #region 自定义方法

        private void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var strWhere = " 1=1";
            var userCriteria = new ReadOnlyUser();
            if (this.dropDeps.SelectedValue != "")
            {
                strWhere += " And Level Like '{0}%'";
                userCriteria.Level = this.dropDeps.SelectedValue;
            }
            if (new Guid(this.dropRoles.SelectedValue) != new Guid())
            {
                strWhere += " And Roles Like '%'+@RoleId+'%'";
                userCriteria.Roles = this.dropRoles.SelectedValue;
            }
            if (this.dropStatus.SelectedValue != "")
            {
                strWhere += "And Status = @Status";
                userCriteria.Status = this.dropStatus.SelectedValue.ToInt();
            }
            if (this.txtName.Text.Trim() != "")
            {
                strWhere += " And(Name Like '%'+@Name+'%' OR Abb Like '%'+@Abb+'%')";
                userCriteria.Name = this.txtName.Text.Trim();
                userCriteria.Abb = this.txtName.Text.Trim().ToUpper();
            }
            this.grid.BindDataSource(userCriteria, strWhere);
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