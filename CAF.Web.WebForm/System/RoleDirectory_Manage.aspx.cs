using FineUI;
using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.CAFControl;
    using CAF.Web.WebForm.Common;

    using global::System.Linq;
    using global::System.Text.RegularExpressions;

    public partial class RoleDirectory_Manage : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            this.pageId = new Guid("e30e54fe-cd12-4731-bdbd-1b870d4adf39");
            base.OnLoad(e);
        }




        protected void dropRolesId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindRoleToTreeNode(this.treeDirs.Nodes);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dropRoles.SelectedValue.ToGuid() == Guid.Empty)
            {
                Alert.ShowInTop("请选择目录！");
            }
            else
            {
                var dics = Directory_Role.GetAllByRoleId(this.dropRoles.SelectedValue.ToGuid());
                this.GetSelectdTreeNode(this.treeDirs.Nodes, ref dics);
                dics.Save();
                this.Initialization();
            }

        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindRoles(this.dropRoles, null);
            PageHelper.BindRoles(this.radioStatus, typeof(RightStatusEnum));
            this.BindTree();
        }


        private void GetSelectdTreeNode(TreeNodeCollection nodes, ref Directory_RoleList dics)
        {
            foreach (var item in nodes)
            {
                if (item.Checked)
                {
                    var selected = dics.FirstOrDefault(d => d.DirectoryId == item.NodeID.ToGuid());
                    if (selected == null)
                    {
                        var dir = new Directory_Role
                        {
                            DirectoryId = item.NodeID.ToGuid(),
                            RoleId = this.dropRoles.SelectedValue.ToGuid(),
                            Status = this.radioStatus.SelectedValue.ToInt()
                        };
                        dics.Add(dir);
                    }
                    else
                    {
                        selected.Status = this.radioStatus.SelectedValue.ToInt();
                    }
                }
                this.GetSelectdTreeNode(item.Nodes, ref dics);
            }
        }


        #region tree

        private void BindRoleToTreeNode(TreeNodeCollection parent)
        {
            foreach (var item in parent)
            {
                var dic = Directory_Role.Get(item.NodeID.ToGuid(), this.dropRoles.SelectedValue.ToGuid());
                var text = "";
                if (dic != null)
                {
                    switch (RichEnumContent.GetEnumFromFlagsEnum<RightStatusEnum>(dic.Status))
                    {
                        case RightStatusEnum.Read:
                            text = "[读]";
                            break;
                        case RightStatusEnum.Write:
                            text = "[写]";
                            break;
                    }
                    item.Checked = true;
                }
                var reg = new Regex("(<(.*)>)");
                item.Text = string.Format("{0}<span style='color: #FF0000'>{1}</span>", reg.Replace(item.Text, ""), text);
                item.Checked = false;
                this.BindRoleToTreeNode(item.Nodes);
            }
        }

        private void BindTree()
        {
            var dirs = Directory.GetAll();
            this.treeDirs.Items.Clear();
            foreach (var item in dirs)
            {
                if (item.ParentId != Guid.Empty)
                {
                    continue;
                }
                var node = this.CreateNode(item, this.treeDirs.Nodes);
                this.ResolveSubTree(item, node);
            }
        }

        private CAFTreeNode CreateNode(Directory item, TreeNodeCollection parent)
        {
            var node = new CAFTreeNode();
            var dic = Directory_Role.Get(item.Id, this.dropRoles.SelectedValue.ToGuid());
            var text = "";
            if (dic != null)
            {
                switch (RichEnumContent.GetEnumFromFlagsEnum<RightStatusEnum>(dic.Status))
                {
                    case RightStatusEnum.Read:
                        text = "[读]";
                        break;
                    case RightStatusEnum.Write:
                        text = "[写]";
                        break;
                }
                node.Checked = true;
            }
            node.Text = string.Format("{0}<span style='color: #FF0000'>{1}</span>", item.Name.ToString(), text);
            node.NodeID = item.Id.ToString();
            node.Expanded = true;

            node.EnableCheckBox = true;
            parent.Add(node);
            return node;
        }

        private void ResolveSubTree(Directory dir, CAFTreeNode treeNode)
        {
            var dirs = Directory.GetAllByParentId(dir.Id);
            if (dirs.Count <= 0)
            {
                return;
            }
            treeNode.Expanded = true;
            foreach (var item in dirs)
            {
                var node = this.CreateNode(item, treeNode.Nodes);
                this.ResolveSubTree(item, node);
            }
        }

        protected void treeDirs_NodeCheck(object sender, FineUI.TreeCheckEventArgs e)
        {
            if (e.Checked)
            {
                this.treeDirs.CheckAllNodes(e.Node.Nodes);
            }
            else
            {
                this.treeDirs.UncheckAllNodes(e.Node.Nodes);
            }
        }

        #endregion tree
    }
}