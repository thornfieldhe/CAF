
namespace CAF.Model
{
    using System.ComponentModel;

    public partial class User
    {
    }

    public enum UserStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("")]
        Nomal = 1,

        /// <summary>
        /// 锁定
        /// </summary>
        [Description("")]
        Locked = 2,

        /// <summary>
        /// 系统
        /// </summary>
        [Description("")]
        System = 4
    }
}
