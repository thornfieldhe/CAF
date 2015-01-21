using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.Core
{
    /// <summary>
    /// 显示名称枚举属性
    /// 用于枚举类型
    /// <example>    
    /// public enum State
    /// {
    ///    [DisplayName("打开")]
    ///     Open = 0,
    ///     [DisplayName("关闭")]
    ///     Close = 1,
    ///     [DisplayName("活动")]
    ///     Activie = 2,
    /// }
    /// </example>
    /// </summary>
    public class DisplayNameAttribute : Attribute
    {
        private string displayName;

        public DisplayNameAttribute(string displayName)
        {
            this.displayName = displayName;
        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }
    }
}
