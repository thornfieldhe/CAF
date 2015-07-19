
namespace CAF.Models
{
    using System.ComponentModel.DataAnnotations;


    public partial class LoginLog
    {
        #region 公共属性


        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不允许为空")]
        [StringLength(20, ErrorMessage = "用户名长度不能超过20")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ip不允许为空")]
        [StringLength(20, ErrorMessage = "Ip长度不能超过20")]
        public string Ip { get; set; }

        #endregion

    }
}
