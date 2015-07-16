namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Organize : EFEntity<Organize>
    {
        public virtual List<User> Users { get; set; }

        public virtual List<Role> Roles { get; set; }

        public override void Validate()
        {
            this.Users.IfNotNull(r =>
                {
                    foreach (var item in this.Users)
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

        protected override void Init() { this.Level = "aa"; }

        protected override void PreInsert(Context context)
        {
            this.Level = "xxx";
            base.PreInsert(context);
        }

        protected override void PreUpdate(Context context)
        {
            this.Level = "xxx";
            base.PreInsert(context);
        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("Code:" + this.Code);
            this.AddDescription("ParentId:" + this.ParentId);
            this.AddDescription("Level:" + this.Level);
            this.AddDescription("Sort:" + this.Sort);
        }

        #region 构造函数

        public Organize(Guid id) : base(id) { }

        public Organize() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage = "部门名称不允许为空")]
        [StringLength(50, ErrorMessage = "部门名称长度不能超过50")]
        public string Name { get; set; }

        /// <summary>
        /// 父部门Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父部门
        /// </summary>
        public virtual Organize Parent { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        public virtual List<Organize> Children { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不允许为空")]
        public int Sort { get; set; }

        /// <summary>
        /// 部门层级
        /// </summary>
        [Required(ErrorMessage = "部门层级不允许为空")]
        [StringLength(20, ErrorMessage = "部门层级长度不能超过20")]
        public string Level { get; protected set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "编码不允许为空")]
        [StringLength(50, ErrorMessage = "编码长度不能超过50")]
        public string Code { get; set; }

        #endregion
    }
}