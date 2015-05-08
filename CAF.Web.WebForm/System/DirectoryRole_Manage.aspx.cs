using System;

namespace CAF.Web.WebForm.System
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;

    using FineUI;

    using global::System.Linq;

    using Newtonsoft.Json.Linq;

    public partial class DirectoryRole_Manage : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("b0e74279-e2bb-47f0-96c4-7a6c92c4e7a4");
            this.grid.OnQuery += this.grid_OnQuery;
            base.OnLoad(e);
        }

        void grid_OnQuery(object sender = null, EventArgs e = null)
        {
            var criteria = new ReadOnlyDirectoryRole { DirectoryId = this.dropDirs.SelectedValue.ToGuid() };
            this.grid.BindDataSource(criteria, where: " DirectoryId=@DirectoryId");
        }

        protected override void Bind()
        {
            base.Bind();
            this.BindScripts();
            PageHelper.BindDirectories(Guid.Empty, this.dropDirs, Guid.Empty.ToString(), false);
            PageHelper.BindRoles(this.dropRoles, Guid.Empty.ToString());
            PageTools.BindDropdownList(typeof(RightStatusEnum), this.dropStatus);
            this.grid_OnQuery();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var directoryId = this.dropDirs.SelectedValue.ToGuid();

            //删除
            var deletedDict = this.grid.GetDeletedList();
            foreach (var rowIndex in deletedDict)
            {
                var id = this.grid.DataKeys[rowIndex][0].ToString().ToGuid();
                Directory_Role.Delete(id);
            }

            //更新
            var modifiedDict = this.grid.GetModifiedDict();
            foreach (var rowIndex in modifiedDict.Keys)
            {
                var id = this.grid.DataKeys[rowIndex][0].ToString().ToGuid();
                var d = Directory_Role.Get(id);
                if (modifiedDict[rowIndex].ContainsKey("RoleId"))
                {
                    var roleId = modifiedDict[rowIndex].SingleOrDefault(r => r.Key == "RoleId").Value.ToString().ToGuid();
                    if (!Directory_Role.Exists(directoryId, roleId))
                    {
                        if (modifiedDict[rowIndex].ContainsKey("Status"))
                        {
                            d = new Directory_Role
                                    {
                                        DirectoryId = directoryId,
                                        RoleId = roleId,
                                        Status =
                                            modifiedDict[rowIndex].SingleOrDefault(r => r.Key == "Status")
                                            .Value.ToString()
                                            .ToInt()
                                    };
                            d.Create();
                            Directory_Role.Delete(id);
                        }
                        else
                        {
                            var status = d.Status;
                            d = new Directory_Role
                            {
                                DirectoryId = directoryId,
                                RoleId = roleId,
                                Status = status
                            };
                            d.Create();
                            Directory_Role.Delete(id);
                        }
                    }
                    else
                    {
                        d = Directory_Role.Get(directoryId, roleId);
                        if (!modifiedDict[rowIndex].ContainsKey("Status"))
                        {
                            var status = this.grid.Rows[rowIndex].Values[2].ToString().ToInt();
                            d.Save();
                            Directory_Role.Delete(id);
                        }
                        else
                        {
                            d.Status =
                                modifiedDict[rowIndex].SingleOrDefault(r => r.Key == "Status")
                                    .Value.ToString()
                                    .ToInt();
                            d.Save();
                            Directory_Role.Delete(id);
                        }
                    }
                }
                else
                {
                    d.Status = modifiedDict[rowIndex].SingleOrDefault(r => r.Key == "Status").Value.ToString().ToInt();
                    d.Save();
                }
            }

            //新增
            var newAddedList = this.grid.GetNewAddedList();
            for (var i = newAddedList.Count - 1; i >= 0; i--)
            {
                var roleId = newAddedList[i].Single(r => r.Key == "RoleId").Value.ToString().ToGuid();
                var status = newAddedList[i].Single(r => r.Key == "Status").Value.ToString().ToInt();
                if (Directory_Role.Exists(directoryId, roleId))
                {
                    var d = Directory_Role.Get(directoryId, roleId);
                    d.Status = status;
                    d.Save();
                }
                else
                {
                    var d = new Directory_Role
                                {
                                    RoleId = roleId,
                                    Status = status,
                                    DirectoryId = this.dropDirs.SelectedValue.ToGuid()
                                };
                    d.Create();
                }
            }
            this.grid_OnQuery();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var item = Organize.Get(this.Id);
            this.grid_OnQuery();

        }

        protected void grid_PreDataBound(object sender, EventArgs e)
        {
            // 设置LinkButtonField的点击客户端事件
            var deleteField = this.grid.FindColumn("Delete") as LinkButtonField;
            deleteField.OnClientClick = this.GetDeleteScript();

        }
        protected void BindScripts()
        {
            // 删除选中单元格的客户端脚本
            var deleteScript = this.GetDeleteScript();

            var defaultObj = new JObject
                                 {
                                     { "Status", 1 } ,
                                     {"Delete", String.Format("<a href=\"javascript:;\" onclick=\"{0}\"><img src=\"{1}\"/></a>", deleteScript, IconHelper.GetResolvedIconUrl(Icon.Delete))}
                                 };

            // 第一行新增一条数据
            this.btnAdd.OnClientClick = this.grid.GetAddNewRecordReference(defaultObj, true);
            // 重置表格
            this.btnReset.OnClientClick = this.grid.GetRejectChangesReference();

        }



        // 删除选中行的脚本
        private string GetDeleteScript()
        {
            return Confirm.GetShowReference("删除选中行？", String.Empty, MessageBoxIcon.Question, grid.GetDeleteSelectedReference(), String.Empty);
        }


    }
}