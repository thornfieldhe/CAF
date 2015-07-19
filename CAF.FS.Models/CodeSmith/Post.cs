
namespace CAF.Models
{
    using System;


    public partial class Post : EFEntity<Post>
    {
        #region 构造函数

        public Post(Guid id) : base(id) { }
        public Post() : this(Guid.NewGuid()) { }

        #endregion

        #region 覆写基类方法

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }

        #endregion

    }
}
