
namespace CAF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using CAF.Utility;

    public partial class Role
    {

        #region 公共属性

        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(20, ErrorMessage = "角色名称长度不能超过20")]
        public string Name { get; set; }

        public virtual List<DirectoryRole> DirectoryRoles { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<Organize> Organizes { get; set; }

        #endregion


        #region 扩展方法

        /// <summary>
        /// 获取简单角色列表
        /// </summary>
        /// <returns></returns>
        public static List<Kuple<Guid, string>> GetSimpleRoleList()
        {
            return ContextWapper.Instance.Context.Set<Role>().Select(r => new Kuple<Guid, string> { Key = r.Id, Value = r.Name }).ToList();

        }
        #endregion
    }
}
