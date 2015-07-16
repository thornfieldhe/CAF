
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Post 
    {
        /// <summary>
        /// 岗位名称
        /// </summary>
        [Required(ErrorMessage = "岗位名称不允许为空")]
        [StringLength(50, ErrorMessage = "岗位名称长度不能超过50")]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
