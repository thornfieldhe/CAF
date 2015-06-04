using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Validations
{
    /// <summary>
    /// 验证工厂
    /// </summary>
    public class ValidationFactory : SingletonBase<ValidationFactory>
    {
        /// <summary>
        /// 创建验证操作
        /// </summary>
        public IValidation Validation { get; private set; }
        public ValidationFactory()
        {
            
        }
    }
}
