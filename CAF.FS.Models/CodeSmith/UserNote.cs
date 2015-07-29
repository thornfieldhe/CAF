using System;
using System.Collections.Generic;
using System.Linq;

namespace CAF.Models
{
    using System.Data.Entity;

    public partial class UserNote : EFEntity<UserNote>
    {
        #region 构造函数

        public UserNote(Guid id) : base(id) { }
        public UserNote() : this(Guid.NewGuid()) { }

        #endregion

        #region 覆写基类方法

        public override void Validate()
        {
            this.User.IfNotNull(r => this.User.Validate());
            base.Validate();
        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Desc);
        }

        protected override List<UserNote> PreQuery(IQueryable<UserNote> query, bool useCache = false)
        {
            query = query.Include(i => i.User);
            return base.PreQuery(query, useCache);
        }

        protected override UserNote PreQuerySingle(IQueryable<UserNote> query)
        {
            query = query.Include(i => i.User);
            return base.PreQuerySingle(query);
        }

        #endregion

    }
}
