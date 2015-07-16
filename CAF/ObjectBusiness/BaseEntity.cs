using System;
using System.Collections.Generic;

namespace CAF
{
    using CAF.Validations;

    [Serializable]
    public abstract partial class BaseEntity<T>
    {


        #region 基本属性

        public Guid Id { get; protected set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; protected set; }

        public DateTime ChangedDate { get; protected set; }

        public byte[] Version { get;protected set; }


        public string Note { get; set; }


        #endregion


        #region 构造函数

        protected BaseEntity(Guid id)
        {
            this.Id = id;
            this.Status = 0;
            this.CreatedDate = DateTime.Now;
            this.ChangedDate = DateTime.Now;

            this._rules = new List<IValidationRule>();
            this._handler = TypeCreater.IocBuildUp<IValidationHandler>();
        }

        protected BaseEntity() : this(Guid.NewGuid()) { }

        #endregion


        protected override void AddDescriptions()
        {
            this.AddDescription("Id:" + this.Id);
            this.AddDescription("Status:" + this.Status);
            this.AddDescription("Note:" + this.Note);
            this.AddDescription("CreatedDate:" + this.CreatedDate);
            this.AddDescription("ChangedDate:" + this.ChangedDate);

        }
    }
}