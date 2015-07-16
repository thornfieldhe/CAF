
namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Post : EFEntity<Post>
    {
        #region 构造函数

        public Post(Guid id) : base(id) { }
        public Post() : this(Guid.NewGuid()) { }

        #endregion

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
        }
    }
}
