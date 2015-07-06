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
    using global::Abp.Authorization.Users;
    using global::Abp.Domain.Repositories;

    public class RoleStore : AbpRoleStore<Tenant, Role, User>
    {
        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                roleRepository,
                userRoleRepository,
                rolePermissionSettingRepository)
        {
        }
    }
}
