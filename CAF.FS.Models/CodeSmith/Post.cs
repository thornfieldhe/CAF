
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Post : EFEntity<Post>
    {
        #region 构造函数

        public Post(Guid id) : base(id) { }
        public Post() : this(Guid.NewGuid()) { }

        #endregion

        #region 公共属性

        /// <summary>
        /// 岗位名称
        /// </summary>
        [Required(ErrorMessage = "岗位名称不允许为空")]
        [StringLength(50, ErrorMessage = "岗位名称长度不能超过50")]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }
        public override void Validate()
        {
            this.Users.IfNotNull(r =>
            {
                foreach (var item in this.Users)
                {
                    item.Validate();
                }
            });
            
            base.Validate();
        }

        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }
}
