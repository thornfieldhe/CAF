
namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class ErrorLog : EFEntity<ErrorLog>
    {

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("UserName:" + this.UserName);
            this.AddDescription("PageCode:" + this.PageCode);
            this.AddDescription("Page:" + this.Page);
            this.AddDescription("Ip:" + this.Ip);
            this.AddDescription("Message:" + this.Message);
            this.AddDescription("Details:" + this.Details);
        }
    }
}
