using System;
using System.Linq;
namespace CAF.Web.WebForm
{
    using CAF.Model;
    using CAF.Web.WebForm.CAFControl;
    using CAF.Web.WebForm.Common;
    using FineUI;


    public partial class Directory_Edit : BasePage
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("f66d4ee2-8c93-47bd-83bf-550cab2025da");
            }
            base.OnLoad(e);
            submitForm.OnPostCreated += submitForm_OnPostExcute;
            submitForm.OnPostDelete += submitForm_OnPostExcute;
            submitForm.OnPostUpdated += submitForm_OnPostExcute;

        }

        private void submitForm_OnPostExcute(IBusinessBase business)
        {
            Initialization();
        }

        protected override void Bind()
        {
            base.Bind();
            PageHelper.BindDirectories(txtId.Text.ToGuid(), dropParentId, Guid.Empty.ToString());
            BindTree();
            txtId.Readonly = false;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Directory.Get(txtId.Text.ToGuid());
            submitForm.Delete(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Directory.Get(txtId.Text.ToGuid());
            submitForm.Update(item);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new Directory();
            submitForm.Create(item);
        }


        #region tree

        protected void treeDirs_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            var dir = Directory.Get(new Guid(e.Node.NodeID));
            PageTools.BindControls(submitForm, dir);
            PageHelper.BindDirectories(txtId.Text.ToGuid(), dropParentId, dir.ParentId.ToString());
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            txtId.Readonly = true;
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

        private void ResolveSubTree(Guid id, TreeNode treeNode)
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
                               NodeID = item.Id.ToString()
                           };
            parent.Add(node);
            return node;
        }

        #endregion tree
    }
}