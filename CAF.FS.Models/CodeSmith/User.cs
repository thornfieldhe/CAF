using System;
using System.Collections.Generic;

namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : EFEntity<User>
    {
        #region 构造函数

        public User(Guid id) : base(id) { }
        public User() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不允许为空")]
        [StringLength(20, ErrorMessage = "登录名长度不能超过20")]
        public string LoginName { get; set; }

        /// <summary>
        /// 用户简称
        /// </summary>
        [Required(ErrorMessage = "用户简称不允许为空")]
        [StringLength(20, ErrorMessage = "用户简称长度不能超过20")]
        public string Abb { get; protected set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户姓名不允许为空")]
        [StringLength(20, ErrorMessage = "用户姓名长度不能超过20")]
        public string Name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "用户密码不允许为空")]
        [StringLength(50, ErrorMessage = "用户密码长度不能超过50")]
        public string Pass { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(30, ErrorMessage = "电话长度不能超过30")]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 组织架构Id
        /// </summary>
        [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        public Guid OrganizeId { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        [ForeignKey("OrganizeId")]
        public virtual Organize Organize { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不允许为空")]
        [StringLength(50, ErrorMessage = "电子邮件长度不能超过50")]
        public string Email { get; set; }

        #endregion

        public virtual List<Role> Roles { get; set; }

        public virtual List<Post> Posts { get; set; }
        public virtual UserSetting UserSetting { get; set; }

        public override void Validate()
        {
            foreach (var item in this.Posts)
            {
                item.Validate();
            }
            foreach (var item in this.Roles)
            {
                item.Validate();
            }
            base.Validate();
        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("LoginName:" + this.LoginName);
            this.AddDescription("Abb:" + this.Abb);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("Pass:" + this.Pass);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("OrganizeId:" + this.OrganizeId);
            this.AddDescription("Email:" + this.Email);
        }
    }
}
