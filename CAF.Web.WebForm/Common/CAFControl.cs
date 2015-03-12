
namespace CAF.Web.WebForm.CAFControl
{
    using System;

    using CAF.Model;
    using CAF.Web.WebForm.Common;

    using FineUI;

    #region CAFPanel

    public class CAFPanel : Panel
    {
        public CAFPanel()
        {
            base.BodyPadding = "5px 5px 5px 5px";
            base.ShowBorder = false;
            base.ShowHeader = false;
        }
    }

    public class MainPanel : CAFPanel
    {
        public MainPanel()
        {
            base.BoxConfigAlign = BoxLayoutAlign.Stretch;
            base.BoxConfigPosition = BoxLayoutPosition.Start;
        }
    }

    public class SubmitPanel : CAFPanel
    {
        public SubmitPanel() { base.CssClass = "submitpanel"; }
    }

    #endregion


    #region CAFForm

    public class CAFForm : Form
    {
        public CAFForm()
        {
            base.BoxConfigAlign = BoxLayoutAlign.Stretch;
            base.BoxConfigPosition = BoxLayoutPosition.Start;
            base.BodyPadding = "5px 5px 5px 5px";
            base.ShowBorder = false;
            base.ShowHeader = false;
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
            var created = OnCreate(business);
            if (created)
            {
                PostCreate(business);
            }
        }
        public void Update(IBusinessBase business)
        {
            if (!this.PreUpdate(business))
            {
                return;
            }
            var updated = OnUpdate(business);
            if (updated)
            {
                PostUpdate(business);
            }
        }
        public void Delete(IBusinessBase business)
        {
            if (!this.PreDelete(business))
            {
                return;
            }
            var updated = OnDelete(business);
            if (updated)
            {
                PostDelete(business);
            }
        }


        public bool PreCreate(IBusinessBase business) { return OnPreCreated == null || OnPreCreated(business); }

        public void PostCreate(IBusinessBase business) { if (OnPostCreated != null) { OnPostCreated(business); } }

        public bool PreDelete(IBusinessBase business) { return OnPreDelete == null || OnPreDelete(business); }

        public void PostDelete(IBusinessBase business) { if (OnPostDelete != null) { OnPostDelete(business); } }

        public bool PreUpdate(IBusinessBase business) { return OnPreUpdated == null || OnPreUpdated(business); }

        public void PostUpdate(IBusinessBase business) { if (OnPostUpdated != null) { OnPostUpdated(business); } }

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
            PageTools.BindModel(this, business);
            business.Save();
            Alert.ShowInTop(business.Errors.Count > 0 ? business.Errors[0] : Resource.System_Message_UpdateSuccess);
            return business.Errors.Count == 0;
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
            base.Columns.Add(new RowNumberField() { EnablePagingNumber = true });
        }

        public delegate void QueryHandler();
        public event QueryHandler OnQuery;

        protected override void OnPageIndexChange(GridPageEventArgs e)
        {
            base.PageIndex = e.NewPageIndex;
            if (OnQuery != null)
            {
                OnQuery();
            }
            base.OnPageIndexChange(e);
        }

        protected override void OnSort(GridSortEventArgs e)
        {
            base.SortField=e.SortField;
            base.SortDirection = e.SortDirection;
            if (OnQuery != null)
            {
                OnQuery();
            }
            base.OnSort(e);
        }

        protected override void OnPreDataBound(EventArgs e)
        {
            var deleteField = base.FindColumn("Delete") as LinkButtonField;
            if (deleteField != null)
            {
                deleteField.OnClientClick = Confirm.GetShowReference(Resource.System_Message_ConfirmDelete,
                    Resource.System_Action_Delete, MessageBoxIcon.Question, string.Empty, string.Empty);
            }
            base.OnPreDataBound(e);
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="where"></param>
        public void BindDataSource<T>(T criteria, string where) where T : ReadOnlyBase
        {
            var result = ReadOnlyCollectionBase<T>
                .Query(SortField, PageSize, criteria, where, PageIndex,SortDirection);
            RecordCount = result.TotalCount;
            DataSource = result.Result;
            DataBind();
        }
    }

    public class CAFTree : Tree
    {
        public CAFTree()
        {
            base.EnableArrows = false;
            base.ShowHeader = false;
            base.EnableLines = true;
            base.ShowBorder = false;
        }

    }


    public class CAFTreeNode : TreeNode
    {
        public CAFTreeNode()
        {
            base.Expanded = true;
            base.EnableClickEvent = true;
        }
    }

    public class CAFWindow : FineUI.Window
    {
        public CAFWindow()
        {
            base.Hidden = true;
            base.EnableIFrame = true;
            base.CloseAction = CloseAction.HidePostBack;
            base.IFrameUrl = "about:blank";
            base.Target = Target.Top;
            base.IsModal = true;
            base.CloseAction = CloseAction.HidePostBack;
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
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ConfirmText = Resource.System_Message_ConfirmAdd;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ValidateForms = new[] { "submitForm" };
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
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
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
            this.ValidateForms = new[] { "submitForm" };
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
}

