﻿
namespace CAF.Models
{

    public partial class ErrorLog : EFEntity<ErrorLog>
    {

        #region 覆写基类方法

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

        #endregion

    }
}