﻿namespace CAF.Models
{
    using System;

    public partial class Organize : EFEntity<Organize>
    {
        #region 覆写基类方法

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

        protected override void Init() { }

        protected override void AddDescriptions()
        {
            base.AddDescriptions();
            this.AddDescription("Name:" + this.Name);
            this.AddDescription("Code:" + this.Code);
            this.AddDescription("ParentId:" + this.ParentId);
            this.AddDescription("Level:" + this.Level);
            this.AddDescription("Sort:" + this.Sort);
        }

        #endregion


        #region 构造函数

        public Organize(Guid id) : base(id) { }

        public Organize() : this(Guid.NewGuid()) { }

        #endregion

    }
}