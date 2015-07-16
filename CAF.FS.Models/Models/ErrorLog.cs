
namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class ErrorLog
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不允许为空")]
        [StringLength(20, ErrorMessage = "用户名长度不能超过20")]
        public string UserName { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [Required(ErrorMessage = "错误代码不允许为空")]
        public int PageCode { get; set; }

        /// <summary>
        /// 错误页
        /// </summary>
        [StringLength(200, ErrorMessage = "错误页长度不能超过200")]
        public string Page { get; set; }

        [Required(ErrorMessage = "Ip不允许为空")]
        [StringLength(20, ErrorMessage = "Ip长度不能超过20")]
        public string Ip { get; set; }

        /// <summary>
        /// 错误
        /// </summary>
        [Required(ErrorMessage = "错误不允许为空")]
        [StringLength(200, ErrorMessage = "错误长度不能超过200")]
        public string Message { get; set; }

        /// <summary>
        /// 详细错误
        /// </summary>
        [Required(ErrorMessage = "详细错误不允许为空")]
        public string Details { get; set; }

    }
}
