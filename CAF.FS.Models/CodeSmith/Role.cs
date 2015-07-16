
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Role : EFEntity<Role>
    {
        #region 构造函数

        public Role(Guid id) : base(id) { }
        public Role() : this(Guid.NewGuid()) { }

        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }
}
