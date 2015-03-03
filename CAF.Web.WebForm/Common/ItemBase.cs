
namespace CAF.Web.WebForm
{
    public abstract class ItemBase : BasePage
    {
        protected override string PostDelete()
        {
            Initialization();
            return "";
        }

        protected override string PostUpdate()
        {
            Initialization();
            return "";
        }

        protected override string PostAdd()
        {
            Initialization();
            return "";
        }
    }
}