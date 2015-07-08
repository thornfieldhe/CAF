
namespace CAF.ObjectBusiness
{
    public class PropertyChanged
    {
        public bool IsPropertyChanged { get; set; }

        public string PropertyName { get; set; }
    }

    public class Property<T>
    {
        public string PropertyName { get; private set; }
        private T OldValue;
        private T NewValue;

        public Property(string name) { this.PropertyName = name; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isInit">是否是初始化</param>
        /// <returns></returns>
        public bool SetValue(T value, bool isInit = false)
        {
            if (isInit)
            {
                this.OldValue = value;
                this.NewValue = value;
                return true;
            }
            else
            {
                if ((this.OldValue == null && value == null) || (this.OldValue != null && this.OldValue.Equals(value)))
                {
                    return false;
                }
                else
                {
                    this.NewValue = value;
                    return true;
                }
            }

        }
        public T GetValue() { return this.NewValue; }
    }
}
