using Abp.MultiTenancy;

using CAF.Abp.Core.Users;
namespace CAF.Abp.Core.MultiTenancy
{
    public class Tenant : AbpTenant<Tenant,User>
    {
        public Tenant() { }

        public Tenant(string tenancyName, string name) : base(tenancyName, name) { }
    }
}
