using System.ComponentModel.DataAnnotations;

namespace CAF {
    using System;

    using CAF.Utility;

    /// <summary>
    /// 验证特性扩展
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 获取验证特性的错误消息
        /// </summary>
        /// <param name="attribute">验证特性</param>
        public static string GetErrorMessage( this ValidationAttribute attribute ) {
            if ( attribute == null )
                return String.Empty;
            return attribute.ErrorMessage.IsEmpty() == false ? attribute.ErrorMessage 
                : ResourceHelper.GetString( attribute.ErrorMessageResourceType.FullName, attribute.ErrorMessageResourceName, attribute.ErrorMessageResourceType.Assembly );
        }
    }
}
