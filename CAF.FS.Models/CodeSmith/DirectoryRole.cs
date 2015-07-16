using System;

namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DirectoryRole : EFEntity<DirectoryRole>
    {
        #region 公共属性

        /// <summary>
        /// 角色Id
        /// </summary>
        [GuidRequired(ErrorMessage = "角色不允许为空")]
        public Guid Role_Id { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// 目录Id
        /// </summary>
        [GuidRequired(ErrorMessage = "目录不允许为空")]
        public Guid Directory_Id { get; set; }

        /// <summary>
        /// 目录
        /// </summary>
        [ForeignKey("DirectoryId")]
        public Directory Directory { get; set; }

        #endregion
    }
}
