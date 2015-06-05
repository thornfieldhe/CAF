using System.Text;

namespace CAF.ObjectBusiness
{
    /// <summary>
    /// 要输出实体的状态
    /// </summary>
    public abstract class StatusDescription
    {
        /// <summary>
        /// 描述
        /// </summary>
        private StringBuilder _description;

        /// <summary>
        /// 输出对象状态
        /// </summary>
        public override string ToString()
        {
            this._description = new StringBuilder();
            this.AddDescriptions();
            return this._description.ToString().TrimEnd().TrimEnd(',');
        }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected abstract void AddDescriptions();

        /// <summary>
        /// 添加描述
        /// </summary>
        protected void AddDescription(string description)
        {
            if (description.IsEmpty())
                return;
            this._description.Append(description);
        }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected void AddDescription<T>(string name, T value)
        {
            if (value.ToStr().IsEmpty())
                return;
            this._description.AppendFormat("{0}:{1},", name, value);
        }
    }
}
