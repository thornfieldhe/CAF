using System;

namespace CAF.FSModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public partial class Directory : EFEntity<Directory>
    {


        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不允许为空")]
        [StringLength(50, ErrorMessage = "名称长度不能超过50")]
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        [StringLength(100, ErrorMessage = "Url地址长度不能超过100")]
        public string Url { get; set; }

        /// <summary>
        /// 父目录
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父目录
        /// </summary>
        public virtual Directory Parent { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public virtual List<Directory> Children { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不允许为空")]
        public int Sort { get; set; }

        public virtual List<DirectoryRole> DirectoryRoles { get; set; }



    }


}
