
namespace CAF.Abp.Core.Users
{
    using CAF.Abp.Core.Authorization;
    using CAF.Abp.Core.MultiTenancy;

    using global::Abp.Authorization;
    using global::Abp.Authorization.Roles;
    using global::Abp.Authorization.Users;
    using global::Abp.Configuration;
    using global::Abp.Configuration.Startup;
    using global::Abp.Dependency;
    using global::Abp.Domain.Repositories;
    using global::Abp.Domain.Uow;
    using global::Abp.Zero.Configuration;

    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(
             UserStore store,
             RoleManager roleManager,
             IRepository<Tenant> tenantRepository,
             IMultiTenancyConfig multiTenancyConfig,
             IPermissionManager permissionManager,
             IUnitOfWorkManager unitOfWorkManager,
             ISettingManager settingManager,
             IUserManagementConfig userManagementConfig,
             IIocResolver iocResolver)
            : base(
                store,
                roleManager,
                tenantRepository,
                multiTenancyConfig,
                permissionManager,
                unitOfWorkManager,
                settingManager,
                userManagementConfig,
                iocResolver)
        {
        }
    }
}
