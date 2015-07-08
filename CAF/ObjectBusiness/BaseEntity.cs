using System;
using System.Collections.Generic;

namespace CAF
{
    using CAF.ObjectBusiness;
    using CAF.Validations;
    using FS.Mapping.Context.Attribute;
    using System.Linq;

    [Serializable]
    public abstract partial class BaseEntity<T>
    {

        protected int _changedRows = 0;//影响行


        //属性改变事件，用于通知列表，修改状态为Dity
        public event PropertyChangeHandler OnPropertyChanged;

        #region 基本属性

        protected Property<int> StatusProperty = new Property<int>("Status");
        protected Property<string> NoteProperty = new Property<string>("Note");

        [Field(UpdateStatusType = StatusType.ReadCondition)]
        public Guid Id { get; protected set; }

        public int Status
        {
            get { return this.StatusProperty.GetValue(); }
            set { this.SetProperty(ref this.StatusProperty, value); }
        }

        [Field(UpdateStatusType = StatusType.ReadCondition)]
        public DateTime CreatedDate { get; protected set; }

        public DateTime ChangedDate { get; protected set; }


        public string Note
        {
            get { return this.NoteProperty.GetValue(); }
            set { this.SetProperty(ref this.NoteProperty, value); }
        }

        /// <summary>
        /// 版本号(乐观锁)
        /// </summary>
        [Field(UpdateStatusType = StatusType.ReadCondition)]
        public byte[] Version { get; protected set; }

        #endregion


        #region 构造函数

        protected BaseEntity(Guid id)
        {
            this.Id = id;
            this.StatusProperty.SetValue(0, true);
            this.CreatedDate = DateTime.Now;
            this.ChangedDate = DateTime.Now;


            this.IsChangeRelationship = false; //默认进行标识删除

            this._rules = new List<IValidationRule>();
            this._handler = TypeCreater.IocBuildUp<IValidationHandler>();
            this.MarkNew();

        }

        protected BaseEntity() : this(Guid.NewGuid()) { }

        #endregion


        protected bool SetProperty<K>(ref Property<K> property, K newValue)
        {
            var name = property.PropertyName;
            var proterty = this.PropertyChangedList.FirstOrDefault(r => r.PropertyName == name);
            var result = property.SetValue(newValue);
            if (!result)
            {
                proterty.IfNotNull(r =>
                    this.PropertyChangedList.Remove(proterty));
                return false;
            }
            this.MarkDirty();

            proterty.IfNull(() =>
                    this.PropertyChangedList.Add(new PropertyChanged
                    {
                        IsPropertyChanged = true,
                        PropertyName = name
                    }));
            if (this.OnPropertyChanged != null)
            {
                this.OnPropertyChanged();
            }
            return true;
        }

        protected readonly List<PropertyChanged> PropertyChangedList = new List<PropertyChanged>();

        /// <summary>
        /// true：只更新关系
        /// false：标记删除
        /// </summary>
        [Field(IsMap = false)]
        public bool IsChangeRelationship { get; set; }

        /// <summary>
        /// 插入或更新中过滤不需要的字段，
        /// properties：A,B,C
        /// </summary>
        /// <param name="properties"></param>
        public void SkipProperties(string properties)
        {
            var propertyList = properties.Split(',');
            this.PropertyChangedList.RemoveAll(r => propertyList.Contains(r.PropertyName));
        }

        public abstract int Create();

        public abstract int Save();

        public abstract int Delete();

        public abstract int SubmitChange();

        protected override void AddDescriptions()
        {
            this.AddDescription("Id:" + this.Id + ",");
            this.AddDescription("Status:" + this.Status + ",");
            this.AddDescription("Note:" + this.Note + ",");
            this.AddDescription("CreatedDate:" + this.CreatedDate + ",");
            this.AddDescription("ChangedDate:" + this.ChangedDate + ",");
            this.AddDescription("Version:" + this.Version + ",");
        }
    }
}