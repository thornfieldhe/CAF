
namespace CAF.Web.WebForm
{
    using FineUI;

    public abstract class EditableBase : ItemBase
    {
        protected override string PostDelete()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }

        protected override string PostUpdate()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }

        protected override string PostAdd()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }
    }
}