using System;

namespace CAF.FSModels
{
    using CAF.ObjectBusiness;
    using System.ComponentModel.DataAnnotations;
    [Serializable]
    public partial class Directory : FarseerEntity<Directory, DirectoryList>
    {
        #region 构造函数

        public Directory(Guid id) : base(id) { }
        public Directory() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        protected Property<string> NameProperty = new Property<string>("Name");
        protected Property<string> UrlProperty = new Property<string>("Url");
        protected Property<Guid?> ParentIdProperty = new Property<Guid?>("ParentId");
        protected Property<string> LevelProperty = new Property<string>("Level");
        protected Property<int> SortProperty = new Property<int>("Sort");

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不允许为空")]
        [StringLength(50, ErrorMessage = "名称长度不能超过50")]
        public string Name
        {
            get { return this.NameProperty.GetValue(); }
            set { this.SetProperty(ref this.NameProperty, value); }
        }

        /// <summary>
        /// Url地址
        /// </summary>
        [StringLength(100, ErrorMessage = "Url地址长度不能超过100")]
        public string Url
        {
            get { return this.UrlProperty.GetValue(); }
            set { this.SetProperty(ref this.UrlProperty, value); }
        }

        /// <summary>
        /// 父目录
        /// </summary>
        public Guid? ParentId
        {
            get { return this.ParentIdProperty.GetValue(); }
            set { this.SetProperty(ref this.ParentIdProperty, value); }
        }

        /// <summary>
        /// 父目录
        /// </summary>
        public Directory Parent
        {
            get
            {
                return !this.ParentId.HasValue ? null : Directory.Get(this.ParentId.Value);
            }
        }

        /// <summary>
        /// 层级
        /// </summary>
        [StringLength(20, ErrorMessage = "层级长度不能超过20")]
        public string Level
        {
            get { return this.LevelProperty.GetValue(); }
            set { this.SetProperty(ref this.LevelProperty, value); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不允许为空")]
        public int Sort
        {
            get { return this.SortProperty.GetValue(); }
            set { this.SetProperty(ref this.SortProperty, value); }
        }

        #endregion

        #region 静态方法



        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name + ",");
            this.AddDescription("Url:" + this.Url + ",");
            this.AddDescription("ParentId:" + this.ParentId + ",");
            this.AddDescription("Level:" + this.Level + ",");
            this.AddDescription("Sort:" + this.Sort);
        }

    }

    [Serializable]
    public class DirectoryList : FarseerCollection<DirectoryList, Directory>
    {
        public DirectoryList() { }
    }
}
