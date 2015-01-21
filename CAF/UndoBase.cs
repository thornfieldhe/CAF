using CAF.Data;
using System;
using System.Collections.Generic;

namespace CAF
{

    [Serializable()]
    public class UndoBase<T> : BusinessBase, IUndoBase where T : class
    {
        [NonSerialized]
        protected Dictionary<int, T> dic = new Dictionary<int, T>();
        [NonSerialized]
        protected int dicCount = 0;
        [NonSerialized]
        protected int currentlyCount = 0;
        [NonSerialized]
        protected bool allowSave = true;

        public virtual int Save()
        {
            if (allowSave)
            {
                dicCount++;
                currentlyCount = dicCount;
                dic.Add(dicCount, (T)ObjectCloner.DeepCopy<T>(this as T));
                MakeDirty();
                return currentlyCount;
            }
            return 0;
        }

        public bool UndoAble { get { return currentlyCount > 1; } }
        public bool RedoAble { get { return currentlyCount < dic.Count; } }

        public virtual void Undo()
        {
            if (dic.Count > 0 && currentlyCount > 1)
            {
                currentlyCount--;
                allowSave = false;
                DataMap.Map<T, T>(dic[currentlyCount], this as T);
                allowSave = true;
            }
            if (currentlyCount <= 1)
            {
                ReSetInitializationState();
            }
        }

        public virtual void Redo()
        {
            if (dic.Count > 0 && currentlyCount < dicCount)
            {
                currentlyCount++;
                allowSave = false;
                DataMap.Map<T, T>(dic[currentlyCount], this as T);
                allowSave = true;
            }
            if (currentlyCount <= 1)
            {
                ReSetInitializationState();
            }
        }

        public virtual void Submit()
        {
            dic.Clear();
            currentlyCount = 0;
            dicCount = 0;
        }

        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged(BusinessChangedEventArgs eventArgs)
        {
            IsDirty = true;
            Save();
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new BusinessChangedEventArgs());
            }
        }

        public override void MakeDelete()
        {
            base.MakeDelete();
            OnPropertyChanged(new BusinessChangedEventArgs());
        }
    }
}