namespace CAF.FSModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Organize : EFEntity<Organize>
    {
        public override void Validate()
        {
            this.Users.IfNotNull(r =>
                {
                    foreach (var item in this.Users)
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

        protected override void Init() { this.Level = "aa"; }

        protected override void PreInsert(Context context)
        {
            this.Level = "xxx";
            base.PreInsert(context);
        }

        protected override void PreUpdate(Context context)
        {
            this.Level = "xxx";
            base.PreInsert(context);
        }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("Code:" + this.Code);
            this.AddDescription("ParentId:" + this.ParentId);
            this.AddDescription("Level:" + this.Level);
            this.AddDescription("Sort:" + this.Sort);
        }

        #region 构造函数

        public Organize(Guid id) : base(id) { }

        public Organize() : this(Guid.NewGuid()) { }

        #endregion

    }
}