using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.Data
{
    /// <summary>
    /// 用于Microsoft.Finance.BusinessEntity.BaseBusinessEntity.Load(),
    /// 自动从reader中获取需要的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbPropertyAttribute : Attribute
    {
        private string fieldName;

        public DbPropertyAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public string FieldName
        {
            get
            {
                return fieldName;
            }
        }
    }
}
