using System;

namespace CAF.FSModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class Directory : EFEntity<Directory>
    {
        #region 构造函数

        public Directory(Guid id) : base(id) { }
        public Directory() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

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

        #endregion

        #region 扩展方法

        protected override Directory PreQuerySingle(IQueryable<Directory> query)
        {
            query = query.Include(i => i.Children).Include(i => i.DirectoryRoles);
            return base.PreQuerySingle(query);
        }

        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("Url:" + this.Url);
            this.AddDescription("ParentId:" + this.ParentId);
            this.AddDescription("Level:" + this.Level);
            this.AddDescription("Sort:" + this.Sort);
        }

    }


}
