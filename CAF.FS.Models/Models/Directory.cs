using System;

namespace CAF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public partial class Directory : EFEntity<Directory>
    {

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

        /// <summary>
        /// 层级编号支持每层2位，共100项
        /// 如：01,0203,010311
        /// </summary>
        /// <returns></returns>
        private string GetMaxLevel()
        {
            //获取当前层级最大编号

            var maxLevel = this.DbContex.Set<Directory>().Where(i => i.ParentId == this.ParentId).Max(i => i.Level);

            if (string.IsNullOrWhiteSpace(maxLevel))//当前层级没有项目
            {
                //获取父对象编号
                var parent = this.DbContex.Set<Directory>().SingleOrDefault(i => i.Id == this.ParentId);
                if (parent == null)//父层级不存在,即为第一条数据
                {
                    return "01";
                }
                else
                {
                    return parent.Level + "01";//父级存在，即为父级下第一条数据
                }
            }
            else
            {
                if (maxLevel.TrimStart('0').Length % 2 == 0 || maxLevel.ToInt() == 9)//当前层级最大编号>=9,层级编号直接+1
                {
                    return (maxLevel.ToInt() + 1).ToString();
                }
                else
                {
                    return (maxLevel.ToInt() + 1).ToString().PadLeft(maxLevel.Length, '0');
                }
            }
        }

        /// <summary>
        /// 获取目录，非自身及子目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SortLevelItem> GetOtherDirectories(Guid id)
        {
            var contex = ContextWapper.Instance.Context;
            var item = contex.Set<Directory>().Find(id);
            var level = item.IsNull() ? "00" : item.Level;
            return
                contex.Set<Directory>()
                    .Where(i => !i.Level.StartsWith(level))
                    .Select(
                        i =>
                        new SortLevelItem { Id = i.Id, Level = i.Level, Sort = i.Level.Length / 2 - 1, Name = i.Name })
                        .OrderBy(i=>i.Level).ToList();
        }

        #endregion


        #region 重载

        protected override void PreInsert() { this.Level = this.GetMaxLevel(); }

        protected override void PreUpdate()
        {
            var currentValue = this.DbContex.Entry(this).CurrentValues.GetValue<Guid>("ParentId");
            var orignialValue = this.DbContex.Entry(this).OriginalValues.GetValue<Guid>("ParentId");
            if (currentValue == orignialValue)
            {
                return;
            }
            this.Level = this.GetMaxLevel();
            this.Children.ForEach(
                r => r.Level = this.Level + r.Level.Substring(this.Level.Length, r.Level.Length - this.Level.Length));
        }
    }

    #endregion



}
