
namespace CAF.Web.WebForm
{
    public abstract class ItemBase : BasePage
    {
        protected override void PostDelete()
        {
            Initialization();
        }

        protected override void PostUpdate()
        {
            Initialization();
        }

        protected override void PostAdd()
        {
            Initialization();
        }
    }
}