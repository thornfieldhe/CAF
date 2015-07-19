
namespace CAF.Models
{
    using System;


    public partial class LoginLog : EFEntity<LoginLog>
    {
        #region 构造函数

        public LoginLog(Guid id) : base(id) { }
        public LoginLog() : this(Guid.NewGuid()) { }

        #endregion

        #region 覆写基类方法

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("UserName:" + this.UserName);
            this.AddDescription("Ip:" + this.Ip);
        }

        #endregion
    }
}
