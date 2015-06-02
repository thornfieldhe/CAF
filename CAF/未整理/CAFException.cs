using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF
{
    public enum CAFException
    {
        /// <summary>
        /// 数据库异常
        /// </summary>
        DbException,

        /// <summary>
        /// 类库异常
        /// </summary>
        LibException,

        /// <summary>
        /// 界面异常
        /// </summary>
        UIException,

        /// <summary>
        /// 未指定异常
        /// </summary>
        UnhandledException,
    }
}
