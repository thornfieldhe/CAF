
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Role
    {

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20, ErrorMessage = "角色名称长度不能超过20")]
        public string Name { get; set; }

        public virtual List<DirectoryRole> DirectoryRoles { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Organize> Organizes { get; set; }

    }
}
