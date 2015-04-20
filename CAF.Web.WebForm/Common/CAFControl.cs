
namespace CAF.Web.WebForm.CAFControl
{
    using CAF.Model;
    using CAF.Web.WebForm.Common;
    using FineUI;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    #region CAFPanel

    public class CAFPanel : Panel
    {
        public CAFPanel()
        {
            this.BodyPadding = "5px 5px 5px 5px";
            this.ShowBorder = false;
            this.ShowHeader = false;
        }
    }

    public class MainPanel : CAFPanel
    {
        public MainPanel()
        {
            this.BoxConfigAlign = BoxLayoutAlign.Stretch;
            this.BoxConfigPosition = BoxLayoutPosition.Start;
        }
    }

    public class SubmitPanel : CAFPanel
    {
        public SubmitPanel() { this.CssClass = "submitpanel"; }
    }

    #endregion


    #region CAFForm

    public class CAFForm : Form
    {
        public CAFForm()
        {
            this.BoxConfigAlign = BoxLayoutAlign.Stretch;
            this.BoxConfigPosition = BoxLayoutPosition.Start;
            this.BodyPadding = "5px 5px 5px 5px";
            this.ShowBorder = false;
            this.ShowHeader = false;
        }
    }

    public class SubmitForm : CAFForm
    {
        public SubmitForm()
        {

        }

        public void Create(IBusinessBase business)
        {
            if (!this.PreCreate(business))
            {
                return;
            }
            var created = this.OnCreate(business);
            if (created)
            {
                this.PostCreate(business);
            }
        }
        public void Update(IBusinessBase business)
        {
            if (!this.PreUpdate(business))
            {
                return;
            }
            var updated = this.OnUpdate(business);
            if (updated)
            {
                this.PostUpdate(business);
            }
        }
        public void Delete(IBusinessBase business)
        {
            if (!this.PreDelete(business))
            {
                return;
            }
            var delete = this.OnDelete(business);
            if (delete)
            {
                this.PostDelete(business);
            }
        }


        public bool PreCreate(IBusinessBase business) { return this.OnPreCreated == null || this.OnPreCreated(business); }

        public void PostCreate(IBusinessBase business) { if (this.OnPostCreated != null) { this.OnPostCreated(business); } }

        public bool PreDelete(IBusinessBase business) { return this.OnPreDelete == null || this.OnPreDelete(business); }

        public void PostDelete(IBusinessBase business) { if (this.OnPostDelete != null) { this.OnPostDelete(business); } }

        public bool PreUpdate(IBusinessBase business) { return this.OnPreUpdated == null || this.OnPreUpdated(business); }

        public void PostUpdate(IBusinessBase business) { if (this.OnPostUpdated != null) { this.OnPostUpdated(business); } }

        public bool OnCreate(IBusinessBase business)
        {
            PageTools.BindModel(this, business);
            business.Create();
            Alert.ShowInTop(business.Errors.Count > 0 ? business.Errors[0] : Resource.System_Message_AddSuccess);
            return business.Errors.Count == 0;
        }

        public bool OnDelete(IBusinessBase business)
        {
            PageTools.BindModel(this, business);
            business.Delete();
            Alert.ShowInTop(Resource.System_Message_DeleteSuccess);
            return true;
        }

        public bool OnUpdate(IBusinessBase business)
        {
            try
            {
                PageTools.BindModel(this, business);
                business.Save();
                Alert.ShowInTop(business.Errors.Count > 0 ? business.Errors[0] : Resource.System_Message_UpdateSuccess);
                return business.Errors.Count == 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void LoadEntity(IBusinessBase business)
        {
            PageTools.BindControls(this, business);
        }

        public delegate bool PerExcuteHandler(IBusinessBase business);
        public delegate void PostExcuteHandler(IBusinessBase business);
        public event PerExcuteHandler OnPreCreated;
        public event PostExcuteHandler OnPostCreated;
        public event PerExcuteHandler OnPreDelete;
        public event PostExcuteHandler OnPostDelete;
        public event PerExcuteHandler OnPreUpdated;
        public event PostExcuteHandler OnPostUpdated;

    }

    public class WindowForm : SubmitForm
    {

    }
    #endregion


    public class CAFGrid : Grid
    {

        public CAFGrid()
        {
            this.DataKeyNames = new string[] { "Id" };
            this.EnableCheckBoxSelect = true;
            this.SortDirection = "ASC";
            this.AllowSorting = true;
            this.AllowPaging = true;
            this.PageSize = 35;
            this.IsDatabasePaging = true;
            this.EnableFrame = true;
            this.AutoScroll = true;
            base.EnableCollapse = true;
            base.ShowBorder = true;
            base.ShowHeader = true;
            base.ShowPagingMessage = true;
            base.Columns.Add(new RowNumberField() { EnablePagingNumber = true });
        }

        public delegate void QueryHandler(object sender = null, EventArgs e = null);
        public event QueryHandler OnQuery;

        protected override void OnPageIndexChange(GridPageEventArgs e)
        {
            this.PageIndex = e.NewPageIndex;
            if (this.OnQuery != null)
            {
                this.OnQuery();
            }
            base.OnPageIndexChange(e);
        }

        protected override void OnSort(GridSortEventArgs e)
        {
            this.SortField = e.SortField;
            this.SortDirection = e.SortDirection;
            if (this.OnQuery != null)
            {
                this.OnQuery();
            }
            base.OnSort(e);
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="where"></param>
        public void BindDataSource<T>(T criteria, string where = " 1=1") where T : ReadOnlyBase
        {
            ReadOnlyCollectionBase<T>.Connection = SqlService.Instance.Connection;
            var result = ReadOnlyCollectionBase<T>
                .Query(this.SortField, this.PageSize, criteria, where, this.PageIndex, this.SortDirection);
            this.RecordCount = result.TotalCount;
            this.DataSource = result.Result;
            this.DataBind();
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="items"></param>
        public void BindDataSource<T>(IList<T> items)
        {
            this.RecordCount = items.Count;
            this.DataSource = items;
            this.DataBind();
        }

        public void Delete<T>() where T : IBusinessBase
        {
            try
            {
                var list = this.SelectedRowIndexArray;
                if (list.Length == 0)
                {
                    Alert.ShowInTop("请选择删除项！");
                }
                else
                {
                    foreach (var id in list.Select(i => (this.Rows[i].DataKeys[0].ToString().ToGuid())))
                    {
                        var item = (T)Activator.CreateInstance(typeof(T), true);
                        item.Id = id;
                        item.Delete();
                    }
                    if (this.OnQuery != null)
                    {
                        this.OnQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.ShowInTop(ex.Message);
            }
        }

        public void Excute<T>(GridCommandEventArgs e) where T : IBusinessBase
        {
            switch (e.CommandName)
            {
                case "Delete":
                    var id = this.Rows[e.RowIndex].DataKeys[0].ToString().ToGuid();
                    var item = (T)Activator.CreateInstance(typeof(T), true);
                    item.Id = id;
                    item.Delete();
                    break;
            }
            if (this.OnQuery != null)
            {
                this.OnQuery();
            }

        }
    }

    public class CAFTree : Tree
    {
        public CAFTree()
        {
            this.EnableArrows = false;
            base.ShowHeader = false;
            this.EnableLines = true;
            base.ShowBorder = false;
        }

    }


    public class CAFTreeNode : TreeNode
    {
        public CAFTreeNode()
        {
            this.Expanded = true;
            this.EnableClickEvent = true;
        }
    }

    public class CAFWindow : Window
    {
        public CAFWindow()
        {
            base.Hidden = true;
            base.EnableIFrame = true;
            this.EnableResize = false;
            this.CloseAction = CloseAction.HidePostBack;
            base.IFrameUrl = "about:blank";
            this.Target = Target.Top;
            this.IsModal = true;
            this.CloseAction = CloseAction.HidePostBack;
        }
    }

    #region CAFButton

    public class CAFButton : Button { }
    public class AddButton : CAFButton
    {
        public AddButton()
        {
            this.Icon = Icon.Add;
            this.Text = Resource.System_Action_Add;
            this.ConfirmText = Resource.System_Message_ConfirmAdd;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }

    }
    public class NewButton : CAFButton
    {
        public NewButton()
        {
            this.Icon = Icon.Add;
            this.Text = Resource.System_Action_Add;
        }
    }
    public class DeleteButton : CAFButton
    {
        public DeleteButton()
        {
            this.Icon = Icon.Delete;
            this.Text = Resource.System_Action_Delete;
            this.ConfirmText = Resource.System_Message_ConfirmDelete;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ConfirmIcon = MessageBoxIcon.Question;
        }
    }
    public class QueryButton : CAFButton
    {
        public QueryButton()
        {
            this.Text = Resource.System_Action_Query;
            this.Icon = Icon.Magnifier;
        }
    }
    public class SubmitButton : CAFButton
    {
        public SubmitButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Submit;
            this.ConfirmText = Resource.System_Message_ConfirmSubmit;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class EditButton : CAFButton
    {
        public EditButton()
        {
            this.Icon = Icon.BulletEdit;
            this.Text = Resource.System_Action_Edit;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class SaveButton : CAFButton
    {
        public SaveButton()
        {
            this.Icon = Icon.Disk;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class UpdateButton : CAFButton
    {
        public UpdateButton()
        {
            this.Icon = Icon.Disk;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class ResetButton : CAFButton
    {
        public ResetButton()
        {
            this.Icon = Icon.Reload;
            this.Text = Resource.System_Action_Reset;
            this.ConfirmText = Resource.System_Message_ConfirmReset;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
        }
    }
    public class CloseButton : CAFButton
    {
        public CloseButton()
        {
            this.Icon = Icon.SystemClose;
            this.Text = Resource.System_Action_Close;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
        }
    }
    public class ExportButton : CAFButton
    {
        public ExportButton()
        {
            this.Icon = Icon.DiskDownload;
            this.Text = Resource.System_Action_Export;
        }
    }
    public class InportButton : CAFButton
    {
        public InportButton()
        {
            this.Icon = Icon.DiskUpload;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    #endregion

    #region CAFLinkButtonField

    public class DeleteLinkButtonField : LinkButtonField
    {
        public DeleteLinkButtonField()
        {
            this.Icon = Icon.Delete;
            this.ConfirmText = Resource.System_Message_ConfirmDelete;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.Width = 60;
            this.CommandName = "Delete";
            this.ConfirmTarget = Target.Top;
            this.HeaderText = Resource.System_Action_Delete;
            this.ColumnID = "Delete";
        }
    }

    public class EditWindowField : WindowField
    {
        public EditWindowField()
        {
            this.Icon = Icon.Pencil;
            this.Width = 60;
            this.HeaderText = Resource.System_Action_Edit;
            this.ColumnID = "Edit";
        }
    }
    #endregion
}

