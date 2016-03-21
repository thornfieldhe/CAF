using System;
using System.Collections.Generic;

namespace CAF
{
    using CAF.Validations;

    /// <summary>
    /// The base entity.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    public abstract partial class BaseBusiness<T> : IBusinessBase
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

        protected BaseBusiness(Guid id)
        {
            this.Id = id;
            this.Status = 0;
            this.CreatedDate = DateTime.Now;
            this.ChangedDate = DateTime.Now;

            this._rules = new List<IValidationRule>();
            this._handler = TypeCreater.IocBuildUp<IValidationHandler>();
        }

        protected BaseBusiness() : this(Guid.NewGuid()) { }

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