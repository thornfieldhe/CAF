
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
        [Description("删除")]
        Delete = -1,
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

    public enum SmsStatusEnum
    {
        [Description("待发送")]
        UnSended = 0,
        [Description("已发送")]
        Sended = 1,
        [Description("发送失败")]
        Fail = -1,
    }

    public enum LoginStatusEnum
    {
        [Description("登出")]
        LoginOut = 0,
        [Description("登陆")]
        Login = 1,
    }

    /// <summary>
    /// 工作流状态
    /// </summary>
    public enum WorkflowState
    {
        [Description("进度中")]
        InAudit = 0,
        [Description("通过")]
        Passed = 1,
        [Description("撤销、回退")]
        Revoked = 2,
        [Description("其他")]
        Other = 5
    }

    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActivityType
    {
        [Description("初始化活动")]
        INITIAL = 0,
        [Description("常规交互活动")]
        INTERACTION = 1,
        [Description("与汇聚活动")]
        AND_MERGE = 2,
        [Description("或汇聚活动")]
        OR_MERGE = 3,
        [Description("终结活动")]
        COMPLETION = 4,
        [Description("单人审批活动")]
        AUTOMATION = 5
    }
}
