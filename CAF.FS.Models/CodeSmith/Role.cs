
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role : EFEntity<Role>
    {
        #region 构造函数

        public Role(Guid id) : base(id) { }
        public Role() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20, ErrorMessage = "角色名称长度不能超过20")]
        public string Name { get; set; }

        #endregion

        public virtual List<DirectoryRole> DirectoryRoles { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Organize> Organizes { get; set; }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }
}
