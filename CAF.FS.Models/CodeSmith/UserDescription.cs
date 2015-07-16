
namespace CAF.FSModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Description 
    {
 
        #region 公共属性

        /// <summary>
        /// 配置名称
        /// </summary>
        [Required(ErrorMessage = "配置名称不允许为空")]
        [StringLength(50, ErrorMessage = "配置名称长度不能超过50")]
        public string Name { get; set; }

        #endregion

    }
}
