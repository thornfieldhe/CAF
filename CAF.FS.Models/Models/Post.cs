
namespace CAF.Models
{
    using CAF.Utility;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public partial class Post
    {
        #region 公共属性

        /// <summary>
        /// 岗位名称
        /// </summary>
        [Required(ErrorMessage = "岗位名称不允许为空")]
        [StringLength(50, ErrorMessage = "岗位名称长度不能超过50")]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 获取简单角色列表
        /// </summary>
        /// <returns></returns>
        public static List<Kuple<Guid, string>> GetSimpleRoleList()
        {
            return ContextWapper.Instance.Context.Set<Post>().Select(r => new Kuple<Guid, string> { Key = r.Id, Value = r.Name }).ToList();

        }
        #endregion

    }
}
