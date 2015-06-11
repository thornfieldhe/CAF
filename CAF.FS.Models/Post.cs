using System;

namespace CAF.FS.Models
{
    using CAF.Farseer;
    using System.ComponentModel.DataAnnotations;

    public class Post : DomainBase<Post>, IEntityBase
    {
        public Post()
        {
            base.MarkNew();
        }


        #region 公共属性

        private string _name = String.Empty;


        /// <summary>
        /// 岗位名称
        /// </summary>
        [Required(ErrorMessage = "岗位名称不允许为空")]
        [StringLength(50, ErrorMessage = "岗位名称长度不能超过50")]
        public string Name
        {
            get { return this._name; }
            set { this.SetProperty("Name", ref this._name, value); }
        }

        protected override void AddDescriptions()
        {
            this.AddDescription("Id:" + this.Id + ",");
            this.AddDescription("Status:" + this.Status + ",");
            this.AddDescription("CreatedDate:" + this.CreatedDate + ",");
            this.AddDescription("ChangedDate:" + this.ChangedDate + ",");
            this.AddDescription("Note:" + this.Note + ",");
            this.AddDescription("Name:" + this.Name + ",");
            this.AddDescription("Version:" + this.Version + ",");
        }

        #endregion
    }
}
