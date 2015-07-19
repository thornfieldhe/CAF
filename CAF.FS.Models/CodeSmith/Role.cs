
namespace CAF.Models
{
    using System;

    public partial class Role : EFEntity<Role>
    {
        #region 构造函数

        public Role(Guid id) : base(id) { }
        public Role() : this(Guid.NewGuid()) { }

        #endregion

        #region 覆写基类方法

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }

        #endregion

    }
}
