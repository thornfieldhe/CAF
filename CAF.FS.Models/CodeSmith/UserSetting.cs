
namespace CAF.FSModels
{
    using System;

    public partial class UserSetting : EFEntity<UserSetting>
    {
        #region 构造函数

        public UserSetting(Guid id) : base(id) { }
        public UserSetting() : this(Guid.NewGuid()) { }

        #endregion

        public override void Validate()
        {
            this.User.IfNotNull(r => this.User.Validate());
            base.Validate();
        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }
}
