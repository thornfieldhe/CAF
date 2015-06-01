
namespace CAF.Utility
{
    using CAF.Utility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 字符串操作类-工具方法
    /// </summary>
    public  partial class Str
    {
        #region 流水号生成

        /// <summary>
        /// 获取全局唯一值
        /// </summary>
        public static string Unique() { return Guid.NewGuid().ToString().Replace("-", ""); }

        /// <summary>
        /// 创建一个32位流水号
        /// </summary>
        public static string GenerateCode() { return Encrypt.Md5By32(Guid.NewGuid().ToString()).ToLower(); }

        /// <summary>
        /// 创建一个16位流水号
        /// </summary>
        public static string GenerateCodeBy16() { return Encrypt.Md5By16(Guid.NewGuid().ToString()).ToLower(); }

        #endregion



    }
}
