using Abp.Authorization.Users;
namespace CAF.Abp.Core.Users
{
    using CAF.Abp.Core.MultiTenancy;

    public class User : AbpUser<Tenant, User>
    {
        public override string ToString() { return string.Format("[User {0}]{1}", base.Id, base.UserName); }
    }
}
