namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class User : EFEntity<User>
    {
        public virtual List<Role> Roles { get; set; }

        public virtual List<Post> Posts { get; set; }

        public virtual UserSetting UserSetting { get; set; }
        public Description Description { get; set; }

        public override void Validate()
        {
            this.Posts.IfNotNull(r =>
                {
                    foreach (var item in this.Posts)
                    {
                        item.Validate();
                    }
                });
            this.Roles.IfNotNull(r =>
                {
                    foreach (var item in this.Roles)
                    {
                        item.Validate();
                    }
                });

            base.Validate();
        }


        protected override List<User> PreQuery(IQueryable<User> query, bool useCache = false)
        {
            query = query.Include(i => i.Organize).Include(i => i.Posts).Include(i => i.Roles);
            return base.PreQuery(query, useCache);
        }


        protected override User PreQuerySingle(IQueryable<User> query)
        {
            query = query.Include(i => i.Organize).Include(i => i.Posts).Include(i => i.Roles);
            return base.PreQuerySingle(query);
        }

        protected override void Init() { this.Abb = "aa"; }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("LoginName:" + this.LoginName);
            this.AddDescription("Abb:" + this.Abb);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("Pass:" + this.Pass);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("OrganizeId:" + this.Organize_Id);
            this.AddDescription("Email:" + this.Email);
        }

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
        //        [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        public Guid Organize_Id { get; set; }

        /// <summary>
        /// 组织架构Id
        /// </summary>
        //        [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        //public Guid? Organize_Id { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public virtual Organize Organize { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不允许为空")]
        [StringLength(50, ErrorMessage = "电子邮件长度不能超过50")]
        public string Email { get; set; }

        #endregion
    }
}