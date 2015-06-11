using System;

namespace FS.Configs
{
    using CAF.FS.Configs;
    using CAF.FS.Utils;

    /// <summary> 系统配置文件 </summary>
    public class SystemConfigs : AbsConfigs<SystemConfig> { }

    /// <summary> 系统配置文件 </summary>
    [Serializable]
    public class SystemConfig
    {
        /// <summary> 根据自己的需要来设置 </summary>
        public bool DeBug = true;
        /// <summary> 开启记录数据库执行过程 </summary>
        public bool IsWriteDbLog = true;
    }
}