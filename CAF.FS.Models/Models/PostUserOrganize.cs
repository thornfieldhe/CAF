using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class PostUserOrganize
    {
        #region 公共属性

        /// <summary>
        /// 岗位Id
        /// </summary>
        [GuidRequired(ErrorMessage = "岗位不允许为空")]
        public Guid PostId { get; set; }

        
        /// <summary>
        /// 岗位
        /// </summary>
        public virtual Post Post { get; set; }


        /// <summary>
        /// 用户Id
        /// </summary>
        [GuidRequired(ErrorMessage = "用户不允许为空")]
        public Guid UserId { get; set; }

        
        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        [GuidRequired(ErrorMessage = "部门不允许为空")]
        public Guid OrganizeId { get; set; }


        /// <summary>
        /// 部门
        /// </summary>
        public virtual Organize Organize { get; set; }

        #endregion
    }
}
