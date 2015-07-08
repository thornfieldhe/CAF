
namespace CAF.FSModels
{

    using FS.Core.Infrastructure;

    public class FarseerCollection<TCollection, TMember> : CollectionBase<TCollection, TMember>
        where TCollection : CollectionBase<TCollection, TMember>, new()
        where TMember : FarseerEntity<TMember, TCollection>, IEntityBase, IEntityStatus, IBusinessBase, new()
    {

        public event OnSaveHandler OnSaved;

        #region 数据库操作方法

        public int Save()
        {
            using (var contex = new Context())
            {
                return this.Save(contex);
            }
        }

        internal int Save(Context contex)
        {
            if (!this.IsDirty)
            {
                return 0;
            }
            var i = 0;
            i += this.PreSubmit(contex);
            i += this.Submit(contex);
            i += this.PostSubmit(contex);
            if (this.OnSaved != null)
            {
                i += this.OnSaved(contex);
            }
            return i;
        }

        protected virtual int PreSubmit(Context contex)
        {
            return 0;
        }

        protected virtual int Submit(Context contex)
        {
            if (!this.IsDirty)
            {
                return 0;
            }
            var rows = 0;
            this._items.ForEach(
                member =>
                {
                    member.Validate();
                    rows += member.SubmitChange(contex);
                });

            return rows;
        }
        protected virtual int PostSubmit(Context contex)
        {
            return 0;
        }

        #endregion

        public FarseerCollection(bool isReadOnly)
            : base(isReadOnly)
        {
        }

        protected FarseerCollection() { }
    }

    public delegate int OnSaveHandler(Context contex);        //属性改变事件，用于通知列表，修改状态为Dity

}
