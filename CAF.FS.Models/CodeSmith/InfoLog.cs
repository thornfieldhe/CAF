
namespace CAF.Models
{
    using System;

    public partial class InfoLog : EFEntity<InfoLog>
    {
        #region 构造函数
        public InfoLog() : this(Guid.NewGuid())
        {
        }
        public InfoLog(Guid id) : base(id) { }


        #endregion

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
