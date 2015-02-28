using System;

namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;


    public partial class Directory_Edit : ItemBase
    {
        protected override void OnLoad(EventArgs e)
        {
            pageId = new Guid("7DDFE30B-AC76-4062-9153-5D4C0E0300C6");
            base.OnLoad(e);
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindDirs(new Guid(), dropParentId);
            BindTree();
        }

        protected override string Delete()
        {
            if (txtId.Text == "")
            {
                return "请选择目录！";
            }
            Directory.Delete(new Guid(txtId.Text));
            return "";
        }

        protected override string Update()
        {
            if (txtId.Text != "")
            {
                var dir = Directory.Get(new Guid(txtId.Text));
                PageTools.BindModel(this.Page, dir);
                if (dir.ParentId == dir.Id)
                {
                    return "父目录不能等于自身！";
                }
                var rowNum = dir.Save();
                return rowNum > 0 ? "" : dir.Errors[0];
            }
            else
            {
                return "请选择目录！";
            }
        }

        protected override string Add()
        {
            txtId.Text = "";
            var dir = new Directory();
            PageTools.BindModel(this.submitForm, dir);
            var rowNum = dir.Create();
            return rowNum > 0 ? "" : dir.Errors[0];
        }

        protected override void PostDelete()
        {
            PageHelper.BindDirs(new Guid(), dropParentId);
            Initialization();
        }

        protected override void PostUpdate()
        {
            PageHelper.BindDirs(new Guid(), dropParentId);
            Initialization();
        }

        protected override void PostAdd()
        {
            PageHelper.BindDirs(new Guid(), dropParentId);
            Initialization();
        }

        #region tree

        protected void treeDirs_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            var dir = Directory.Get(new Guid(e.Node.NodeID));
            PageTools.BindControls(this.submitForm, dir);
        }

        private void BindTree()
        {
            var dirs = Directory.GetAll();
            foreach (var item in dirs)
            {
                if (item.ParentId == new Guid())
                {
                    var node = CreateNode(item, treeDirs.Nodes);
                    ResolveSubTree(item, node);
                }
            }
        }

        private void ResolveSubTree(Directory dir, FineUI.TreeNode treeNode)
        {
            var dirs = Directory.GetAllByParentId(dir.Id);
            if (dirs.Count > 0)
            {
                treeNode.Expanded = true;
                foreach (Directory item in dirs)
                {
                    var node = CreateNode(item, treeNode.Nodes);
                    ResolveSubTree(item, node);
                }
            }
        }

        private TreeNode CreateNode(Directory item, TreeNodeCollection parent)
        {
            var node = new TreeNode
                           {
                               Text =
                                   string.Format("{0}<span style='color: #FF0000'>{1}</span>",
                                       item.Name.ToString(),
                                       item.Status == (int)HideStatusEnum.Hide ? "[隐藏]" : ""),
                               NodeID = item.Id.ToString(),
                               Expanded = true
                           };
            parent.Add(node);
            return node;
        }

        #endregion tree
    }
}