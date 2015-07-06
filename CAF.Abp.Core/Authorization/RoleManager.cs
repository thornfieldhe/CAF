using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Abp.Core.Authorization
{
    using CAF.Abp.Core.MultiTenancy;
    using CAF.Abp.Core.Users;

    using global::Abp.Authorization;
    using global::Abp.Authorization.Roles;
    using global::Abp.Domain.Uow;
    using global::Abp.Zero.Configuration;

    public class RoleManager : AbpRoleManager<Tenant, Role, User>
    {
        public RoleManager(
            RoleStore store,
            IPermissionManager permissionManager,
            IRoleManagementConfig roleManagementConfig,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                store,
                permissionManager,
                roleManagementConfig,
                unitOfWorkManager)
        {
        }
    }
}
