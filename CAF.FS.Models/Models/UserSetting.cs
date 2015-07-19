
namespace CAF.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class UserSetting
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [Required(ErrorMessage = "配置名称不允许为空")]
        [StringLength(50, ErrorMessage = "配置名称长度不能超过50")]
        public string Name { get; set; }

        public virtual User User { get; set; }
    }
}
