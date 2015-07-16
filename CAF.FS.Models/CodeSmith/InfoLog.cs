
namespace CAF.FSModels
{
    using System.ComponentModel.DataAnnotations;


    public class InfoLog : EFEntity<InfoLog>
    {
        #region 公共属性

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不允许为空")]
        [StringLength(20, ErrorMessage = "用户名长度不能超过20")]
        public string UserName { get; set; }


        /// <summary>
        /// 操作页
        /// </summary>
        [StringLength(200, ErrorMessage = "错误页长度不能超过200")]
        public string Page { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        [Required(ErrorMessage = "活动不允许为空")]
        [StringLength(200, ErrorMessage = "活动长度不能超过200")]
        public string Action { get; set; }

        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("UserName:" + this.UserName);
            this.AddDescription("Action:" + this.Action);
            this.AddDescription("Page:" + this.Page);
        }
    }
}
