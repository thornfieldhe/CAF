using System;

namespace CAF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CAF.Utility;

    public sealed partial class User
    {

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
        public Guid Organize_Id { get; set; }

        #region 1:n子对象允许remove时配置为可空对象

        /// <summary>
        /// 组织架构Id
        /// </summary>
        // [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        //public Guid? Organize_Id { get; set; }

        #endregion

        /// <summary>
        /// 组织架构
        /// </summary>
        public Organize Organize { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不允许为空")]
        [StringLength(50, ErrorMessage = "电子邮件长度不能超过50")]
        public string Email { get; set; }

        public List<Role> Roles { get; set; }

        public List<Post> Posts { get; set; }

        public UserSetting UserSetting { get; set; }

        public Description Description { get; set; }

        #endregion

        #region 扩展方法

        public static List<Directory> GetDirectories(Guid uderId)
        {
            var contex = ContextWapper.Instance.Context;
            return
                contex.Set<User>()
                    .Where(i => i.Id == uderId)
                    .SelectMany(r => r.Roles.SelectMany(d => d.DirectoryRoles.Select(p => p.Directory))).ToList();
        }

        #endregion

        #region 重载

        protected override void PreInsert()
        {
            this.Abb = this.Name.GetChineseSpell();
            this.Pass = Encrypt.DesEncrypt(this.Pass);
            base.PreInsert();
        }

        protected override void PreUpdate()
        {
            var currentValue = this.DbContex.Entry(this).CurrentValues.GetValue<Guid>("Name");
            var orignialValue = this.DbContex.Entry(this).OriginalValues.GetValue<Guid>("Name");
            if (currentValue != orignialValue)
            {
                this.Abb = this.Name.GetChineseSpell();
            }
            base.PreUpdate();
        }

        #endregion
    }
}
