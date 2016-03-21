
namespace CAF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public partial class UserNote
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [Required(ErrorMessage = "配置名称不允许为空")]
        [StringLength(50, ErrorMessage = "配置名称长度不能超过50")]
        public string Desc { get; set; }
        /// <summary>
        ///用户Id
        /// </summary>
        public Guid? User_Id { get; set; }

        #region 1:n子对象允许remove时配置为可空对象

        /// <summary>
        /// 角色Id
        /// </summary>
        // [GuidRequired(ErrorMessage = "角色不允许为空")]
        //public Guid? Role_Id { get; set; }
        #endregion

        public User User { get; set; }
    }
}
