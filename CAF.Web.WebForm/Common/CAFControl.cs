
namespace CAF.Web.WebForm.CAFControl
{
    using CAF.Model;

    using FineUI;

    public class Container : CollapsablePanel
    {
        public Container()
        {
            base.BodyPadding = "5px";
            base.ShowBorder = false;
            base.ShowHeader = false;
        }
    }

    public class MainPanel : Container
    {
        public MainPanel()
        {
            base.BoxConfigAlign = BoxLayoutAlign.Stretch;
            base.BoxConfigPosition = BoxLayoutPosition.Start;
            base.BodyPadding = "5px 5px 5px 5px";
        }
    }

    public class SubmitPanel : Container
    {
        public SubmitPanel()
        {
            base.CssClass = "submitpanel";
        }
    }

    public class SubmitForm : Container
    {
        public SubmitForm()
        {
        }
    }

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
        }
    }

    #region Button
    public class AddButton : Button
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

    public class NewButton : Button
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

    public class DeleteButton : Button
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
    public class QueryButton : Button
    {
        public QueryButton()
        {
            this.Text = Resource.System_Action_Query;
            this.Icon = Icon.Magnifier;
        }
    }
    public class SubmitButton : Button
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
    public class EditButton : Button
    {
        public EditButton()
        {
            this.Icon = Icon.BulletEdit;
            this.Text = Resource.System_Action_Edit;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class SaveButton : Button
    {
        public SaveButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class UpdateButton : Button
    {
        public UpdateButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class ResetButton : Button
    {
        public ResetButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class CloseButton : Button
    {
        public CloseButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    public class ExportButton : Button
    {
        public ExportButton()
        {
            this.Icon = Icon.PageGo;
            this.Text = Resource.System_Action_Export;
        }
    }
    public class InportButton : Button
    {
        public InportButton()
        {
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
            this.ConfirmText = Resource.System_Message_ConfirmSave;
            this.ConfirmIcon = MessageBoxIcon.Question;
            this.ConfirmTitle = Resource.System_Info_Hint;
            this.ValidateForms = new[] { "submitForm" };
        }
    }
    #endregion
}

