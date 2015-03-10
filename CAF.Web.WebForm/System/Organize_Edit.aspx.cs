using System;

namespace CAF.Web.WebForm
{
    using CAF.Ext;
    using CAF.Model;
    using CAF.Web.WebForm.CAFControl;
    using CAF.Web.WebForm.Common;
    using FineUI;

    public partial class Organize_Edit : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                pageId = new Guid("7405F6D8-3D7A-48E8-BC47-1169CE40AC4E");
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
            PageHelper.BindOrganizes(txtId.Text.ToGuid(), dropParentId, txtId.Text);
            BindTree();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var item = new Organize();
            submitForm.Create(item);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var item = Organize.Get(txtId.Text.ToGuid());
            submitForm.Update(item);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var item = Organize.Get(txtId.Text.ToGuid());
            submitForm.Delete(item);
        }

        #region tree

        protected void treeDeps_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            var item = Organize.Get(new Guid(e.Node.NodeID));
            PageTools.BindControls(this.submitForm, item);
            PageHelper.BindOrganizes(txtId.Text.ToGuid(), dropParentId, item.ParentId.Value.ToString());
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private void BindTree()
        {
            var list = Organize.GetAll();
            foreach (var item in list)
            {
                if (!item.ParentId.IsNullOrEmptuy())
                {
                    continue;
                }
                var node = CreateNode(item, treeDeps.Nodes);
                ResolveSubTree(item, node);
            }
        }

        private CAFTreeNode CreateNode(Organize item, TreeNodeCollection parent)
        {
            var node = new CAFTreeNode
                           {
                               Text = string.Format("{0}[{1}]", item.Name.ToString(), item.Code),
                               NodeID = item.Id.ToString(),
                               Expanded = true
                           };
            parent.Add(node);
            return node;
        }

        private void ResolveSubTree(Organize node, TreeNode treeNode)
        {
            var list = Organize.GetAllByParentId(node.Id);
            if (list.Count <= 0)
            {
                return;
            }
            treeNode.Expanded = true;
            foreach (var item in list)
            {
                var newNode = CreateNode(item, treeNode.Nodes);
                ResolveSubTree(item, newNode);
            }
        }

        #endregion tree
    }
}