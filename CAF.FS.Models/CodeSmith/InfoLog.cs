
namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;


    public partial class InfoLog : EFEntity<InfoLog>
    {

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("UserName:" + this.UserName);
            this.AddDescription("Action:" + this.Action);
            this.AddDescription("Page:" + this.Page);
        }
    }
}
