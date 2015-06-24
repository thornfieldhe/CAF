using System;

namespace CAF.Data
{
    /// <summary>
    /// 用于Microsoft.Finance.BusinessEntity.BaseBusinessEntity.Load(),
    /// 自动从reader中获取需要的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbPropertyAttribute : Attribute
    {
        public DbPropertyAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; private set; }
    }
}
