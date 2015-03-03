using System;
using System.Linq;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.CAFControl;
    using CAF.Web.WebForm.Common;
    using FineUI;


    public partial class Directory_Edit : ItemBase
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("f66d4ee2-8c93-47bd-83bf-550cab2025da");
            }
            base.OnLoad(e);
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindDirectories(txtId.Text.ToGuid(), dropParentId);
            BindTree();
            txtId.Readonly = false;
        }

        protected override string Delete()
        {
            if (txtId.Text.ToGuid() == Guid.Empty)
            {
                return "未选择目录或目录编码格式不正确！";
            }
            Directory.Delete(new Guid(txtId.Text));
            return "";
        }

        protected override string Update()
        {
            if (txtId.Text.ToGuid() == Guid.Empty)
            {
                return "未选择目录或目录编码格式不正确！";
            }
            var dir = Directory.Get(new Guid(txtId.Text));
            PageTools.BindModel(Page, dir);
            dir.Save();
            return dir.Errors.Count == 0 ? "" : dir.Errors[0];
        }

        protected override string Add()
        {

            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                return "目录编码格式不正确！";
            }
            var dir = new Directory();
            PageTools.BindModel(submitForm, dir);
            dir.Create();
            return dir.Errors.Count == 0 ? "" : dir.Errors[0];
        }



        #region tree

        protected void treeDirs_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            var dir = Directory.Get(new Guid(e.Node.NodeID));
            txtId.Readonly = true;
            PageHelper.BindDirectories(txtId.Text.ToGuid(), dropParentId);
            PageTools.BindControls(submitForm, dir);

        }

        private void BindTree()
        {
            var dirs = Directory.GetAll();
            foreach (var item in dirs)
            {
                if (item.ParentId != new Guid())
                {
                    continue;
                }
                var node = CreateNode(item, treeDirs.Nodes);
                ResolveSubTree(item.Id, node);
            }
        }

        private void ResolveSubTree(Guid id, CAFTreeNode treeNode)
        {
            var dirs = Directory.GetAllByParentId(id).OrderBy(d => d.Sort).ToList();
            if (dirs.Count <= 0)
            {
                return;
            }
            treeNode.Expanded = true;
            foreach (var item in dirs)
            {
                var node = CreateNode(item, treeNode.Nodes);
                ResolveSubTree(item.Id, node);
            }
        }

        private CAFTreeNode CreateNode(Directory item, TreeNodeCollection parent)
        {
            var node = new CAFTreeNode
                           {
                               Text =
                                   string.Format("{0}<span style='color: #FF0000'>{1}</span>",
                                       item.Name,
                                       item.Status == (int)HideStatusEnum.Hide ? "[隐藏]" : ""),
                               NodeID = item.Id.ToString(),

                           };
            parent.Add(node);
            return node;
        }

        #endregion tree
    }
}