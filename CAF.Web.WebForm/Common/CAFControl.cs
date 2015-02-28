﻿
namespace CAF.Web.WebForm.CAFControl
{
    using CAF.Model;

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
            this.Icon = Icon.Accept;
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
            this.Icon = Icon.Accept;
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
            this.Icon = Icon.Accept;
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
            this.Icon = Icon.Accept;
            this.Text = Resource.System_Action_Save;
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
            this.Icon = Icon.PageGo;
            this.Text = Resource.System_Action_Export;
        }
    }
    public class InportButton : CAFButton
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

