using System;
using System.Collections.Generic;
using System.Linq;
namespace CAF
{
    [Serializable()]
    public class BusinessBase
    {
        public bool IsNew { get; protected set; }
        //protected bool IsSelfDirty{ get; protected set; }
        public bool IsDirty { get; protected set; }
        //protected bool IsSelfValid{ get; protected set; }
        public bool IsValid
        {
            get
            {
                return Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(this).IsValid;
            }
        }
        //protected bool IsSavable{ get; protected set; }
        public bool IsDeleted { get; protected set; }

        #region 存储初始状态

        protected bool SIsNew = false;
        protected bool SIsDirty = false;
        protected bool SIsDeleted = false;

        #endregion

        public Dictionary<string, string> UnValidResults
        {
            get
            {
                return Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(this).ToDictionary(e => e.Key, e => e.Message);
            }
        }

        protected Guid id;
        public Guid Id
        {
            get { return id; }
            protected set
            {
                id = value;
            }
        }

        public virtual void MakeNew()
        {
            IsNew = true;
            IsDirty = true;
            IsDeleted = false;
        }

        public virtual void MakeClean()
        {
            IsNew = false;
            IsDirty = false;
            IsDeleted = false;
        }

        public virtual void MakeDelete()
        {
            IsDeleted = true;
        }

        public virtual void MakeDirty()
        {
            IsDirty = true;
        }

        public BusinessBase()
        {
            this.Id = Guid.NewGuid();
            SetInitializationState();
        }

        protected virtual void SetInitializationState()
        {
            SIsNew = IsNew;
            SIsDirty = IsDirty;
            SIsDeleted = IsDeleted;
        }

        protected virtual void ReSetInitializationState()
        {
            //IsNew = SIsNew;
            //IsDirty = SIsDirty;
            //IsDeleted = SIsDeleted;
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}