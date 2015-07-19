
namespace CAF.Models
{


    public partial class InfoLog : EFEntity<InfoLog>
    {

        #region 覆写基类方法

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("UserName:" + this.UserName);
            this.AddDescription("Action:" + this.Action);
            this.AddDescription("Page:" + this.Page);
        }

        #endregion

    }
}
