
namespace CAF.Web.WebForm
{
    using FineUI;

    public abstract class EditableBase : ItemBase
    {
        protected override void PostDelete()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void PostUpdate()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected override void PostAdd()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}