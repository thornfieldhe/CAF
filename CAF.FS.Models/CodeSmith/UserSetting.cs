
namespace CAF.FSModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserSetting : EFEntity<UserSetting>
    {
        #region 构造函数

        public UserSetting(Guid id) : base(id) { }
        public UserSetting() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        /// <summary>
        /// 配置名称
        /// </summary>
        [Required(ErrorMessage = "配置名称不允许为空")]
        [StringLength(50, ErrorMessage = "配置名称长度不能超过50")]
        public string Name { get; set; }

        #endregion

        public virtual User User { get; set; }
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
