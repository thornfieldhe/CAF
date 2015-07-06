using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Abp.Core.Authorization
{
    using CAF.Abp.Core.MultiTenancy;
    using CAF.Abp.Core.Users;

    using global::Abp.Authorization.Roles;

    public class Role:AbpRole<Tenant,User>
    {
        public Role(){}

        public Role(int? tenantId,string name,string displayName):base(tenantId,name,displayName)
        {}
    }
}
