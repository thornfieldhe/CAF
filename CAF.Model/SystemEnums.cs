
namespace CAF.Model
{
    using System.ComponentModel;

    public enum HideStatusEnum
    {
        [Description("显示")]
        Show = 0,
        [Description("隐藏")]
        Hide = 1,
    }

    public enum IsSystemRoleEnum
    {
        System = 1,
        UnSystem = 0,
    }

    public enum UserStatusEnum
    {
        [Description("正常")]
        Normal = 1,
        [Description("系统")]
        System = 2,
        [Description("锁定")]
        Locked = 4,

    }


    public enum RightStatusEnum
    {
        [Description("读")]
        Read = 1,
        [Description("写")]
        Write = 2,
    }


    public enum ExcuteResultEnum
    {
        [Description("成功")]
        Success,
        [Description("失败")]
        Fail
    }

    public enum SMSStatusEnum
    {
        [Description("待发送")]
        UnSended = 0,
        [Description("已发送")]
        Sended = 1,
        [Description("发送失败")]
        Fail = -1,
    }


}
