using System.ComponentModel;

namespace CAF.Tests.Samples {
    using CAF.Utility;

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType {
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal = 1,
        /// <summary>
        /// 错误
        /// </summary>
        [OrderBy(5)]
        [Description( "错误" )]
        Error = 2,
        /// <summary>
        /// 警告
        /// </summary>
        [Description( "警告" )]
        [OrderBy(6)]
        Warning = 3,
        /// <summary>
        /// 信息
        /// </summary>
        [Description( "信息" )]
        Information = 4,
        /// <summary>
        /// 调试
        /// </summary>
        [OrderBy( 3 )]
        [Description( "调试" )]
        Debug = 5
    }
}
