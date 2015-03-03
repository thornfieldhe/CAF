using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;
    using System.Linq;


    public partial class User_Query : ListBase
    {
        protected override void OnLoad(EventArgs e)
        {
            pageId = new Guid("5CCA7546-79EB-4AAE-A7F9-90F9E660A3A6");
            base.OnLoad(e);
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindDirectories(new Guid(), dropDeps);
            PageTools.BindDropdownList(Role.GetSimpleRoleList(), dropRoles);
            PageTools.BindDropdownList(typeof(UserStatusEnum), dropStatus);
            Query();
        }

        protected override void BindScripts()
        {
            base.BindScripts();
            btnNew.OnClientClick = winEdit.GetShowReference("User_Edit.aspx", "新增");
        }

        protected override void ExcuteQuery()
        {
            var strWhere = " 1=1";
            var userCriteria = new ReadOnlyUser();
            if (dropDeps.SelectedValue != "")
            {
                strWhere += " And Level Like '{0}%'";
                userCriteria.Level = dropDeps.SelectedValue;
            }
            if (new Guid(dropRoles.SelectedValue) != new Guid())
            {
                strWhere += " And Roles Like '%'+@RoleId+'%'";
                userCriteria.Roles = dropRoles.SelectedValue;
            }
            if (dropStatus.SelectedValue != "")
            {
                strWhere += "And Status = @Status";
                userCriteria.Status = dropStatus.SelectedValue.ToInt();
            }
            if (txtName.Text.Trim() != "")
            {
                strWhere += " And( UserName Like '%'+@Name+'%' OR Abb Like '%'+@Name+'%')";
                userCriteria.Name = txtName.Text.Trim();
                userCriteria.Abb = txtName.Text.Trim().ToUpper();
            }
            grid.BindDataSource("Name", userCriteria, strWhere);
        }

        protected string Edit(object id, object name)
        {
            return winEdit.GetShowReference("User_Edit.aspx?Id=" + id, "编辑 - " + name);
        }

        protected string EditRoleDirs(object id, object name)
        {
            return winEditRoleDirs.GetShowReference("User_Edit.aspx?Id=" + id, "编辑 - " + name);
        }

        protected void btnLockRows_Click(object sender, EventArgs e)
        {
            Update((int)UserStatusEnum.Locked);
        }

        protected void btnUnLockRows_Click(object sender, EventArgs e)
        {
            Update((int)UserStatusEnum.Normal);
        }

        protected void btnDeleteRows_Click(object sender, EventArgs e)
        {
            Update(-1);
        }

        protected void winEdit_Close(object sender, WindowCloseEventArgs e)
        {
            Query();
        }

        private void Update(int cmd)
        {
            try
            {
                var list = grid.SelectedRowIndexArray;
                if (list.Length == 0)
                {
                    Alert.ShowInTop("请选择用户！");
                }
                else
                {
                    foreach (var u in list.Select(i => Model.User.Get(new Guid(grid.Rows[i].DataKeys[0].ToString()))))
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
            Query();
        }

    }
}