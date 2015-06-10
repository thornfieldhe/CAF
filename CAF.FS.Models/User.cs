using System;

namespace CAF.FS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User : DomainBase<User>, IEntityBase
    {
        public User()
        {
            base.MarkNew();
        }


        #region 公共属性

        private string _loginName = String.Empty;
        private string _abb = String.Empty;
        private string _name = String.Empty;
        private string _pass = String.Empty;
        private string _phoneNum = String.Empty;
        private Guid _organizeId = Guid.Empty;
        private string _email = String.Empty;

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不允许为空")]
        [StringLength(20, ErrorMessage = "登录名长度不能超过20")]
        public string LoginName
        {
            get { return this._loginName; }
            set { this.SetProperty("LoginName", ref this._loginName, value); }
        }

        /// <summary>
        /// 用户简称
        /// </summary>
        [Required(ErrorMessage = "用户简称不允许为空")]
        [StringLength(20, ErrorMessage = "用户简称长度不能超过20")]
        public string Abb
        {
            get { return this._abb; }
            set { this.SetProperty("Abb", ref this._abb, value); }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户姓名不允许为空")]
        [StringLength(20, ErrorMessage = "用户姓名长度不能超过20")]
        public string Name
        {
            get { return this._name; }
            set { this.SetProperty("Name", ref this._name, value); }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "用户密码不允许为空")]
        [StringLength(50, ErrorMessage = "用户密码长度不能超过50")]
        public string Pass
        {
            get { return this._pass; }
            set { this.SetProperty("Pass", ref this._pass, value); }
        }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(30, ErrorMessage = "电话长度不能超过30")]
        public string PhoneNum
        {
            get { return this._phoneNum; }
            set { this.SetProperty("PhoneNum", ref this._phoneNum, value); }
        }

        /// <summary>
        /// 组织架构Id
        /// </summary>
        [GuidRequired(ErrorMessage = "组织架构不允许为空")]
        public Guid OrganizeId
        {
            get { return this._organizeId; }
            set { this.SetProperty("OrganizeId", ref this._organizeId, value); }
        }


        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required(ErrorMessage = "电子邮件不允许为空")]
        [StringLength(50, ErrorMessage = "电子邮件长度不能超过50")]
        public string Email
        {
            get { return this._email; }
            set { this.SetProperty("Email", ref this._email, value); }
        }


        #endregion

        protected override void AddDescriptions()
        {
            this.AddDescription("Id:" + this.Id + ",");
            this.AddDescription("Status:" + this.Status + ",");
            this.AddDescription("CreatedDate:" + this.CreatedDate + ",");
            this.AddDescription("ChangedDate:" + this.ChangedDate + ",");
            this.AddDescription("Note:" + this.Note + ",");
            this.AddDescription("LoginName:" + this.LoginName + ",");
            this.AddDescription("Abb:" + this.Abb + ",");
            this.AddDescription("Name:" + this.Name + ",");
            this.AddDescription("Pass:" + this.Pass + ",");
            this.AddDescription("PhoneNum:" + this.PhoneNum + ",");
            this.AddDescription("OrganizeId:" + this.OrganizeId + ",");
            this.AddDescription("Email:" + this.Email + ",");
        }
    }
}
