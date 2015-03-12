
namespace CAF.Web.WebForm
{
    using FineUI;

    public abstract class EditableBase 
    {
        protected  string PostDelete()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }

        protected  string PostUpdate()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }

        protected  string PostAdd()
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            return "";
        }
    }
}