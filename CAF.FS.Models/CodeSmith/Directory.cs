using System;

namespace CAF.FSModels
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public partial class Directory : EFEntity<Directory>
    {
        #region 构造函数

        public Directory(Guid id) : base(id) { }
        public Directory() : this(Guid.NewGuid()) { }

        #endregion


        #region 覆写基类方法

        protected override Directory PreQuerySingle(IQueryable<Directory> query)
        {
            query = query.Include(i => i.Children).Include(i => i.DirectoryRoles);
            return base.PreQuerySingle(query);
        }

        protected override List<Directory> PreQuery(IQueryable<Directory> query, bool useCache = false)
        {
            query = query.Include(i => i.Children).Include(i => i.DirectoryRoles);
            return base.PreQuery(query, useCache);
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
