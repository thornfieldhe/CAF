namespace CAF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public partial class User : EFEntity<User>
    {
        #region 覆写基类方法

        public override void Validate()
        {
            this.Posts.IfNotNull(r =>
                {
                    foreach (var item in this.Posts)
                    {
                        item.Validate();
                    }
                });
            this.Roles.IfNotNull(r =>
                {
                    foreach (var item in this.Roles)
                    {
                        item.Validate();
                    }
                });

            base.Validate();
        }

        protected override List<User> PreQuery(IQueryable<User> query, bool useCache = false)
        {
            query = query.Include(i => i.Roles)
                .Include(i => i.Posts)
                .Include(i => i.UserSetting).Include(i => i.UserNotes);
            return base.PreQuery(query, useCache);
        }

        protected override User PreQuerySingle(IQueryable<User> query)
        {
            query = query.Include(i => i.Roles)
                .Include(i => i.Posts)
                .Include(i => i.UserSetting).Include(i => i.UserNotes);
            return base.PreQuerySingle(query);

        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("LoginName:" + this.LoginName);
            this.AddDescription("Abb:" + this.Abb);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("Pass:" + this.Pass);
            this.AddDescription("PhoneNum:" + this.PhoneNum);
            this.AddDescription("Email:" + this.Email);
        }

        #endregion

        #region 构造函数

        public User(Guid id) : base(id) { }

        public User() : this(Guid.NewGuid()) { }

        #endregion
    }
}