
namespace CAF.Abp.Core.Users
{
    using System;

    using CAF.Abp.Core.Authorization;
    using CAF.Abp.Core.MultiTenancy;

    using global::Abp.Authorization.Users;
    using global::Abp.Domain.Repositories;
    using global::Abp.Domain.Uow;
    using global::Abp.Runtime.Session;

    public class UserStore : AbpUserStore<Tenant, Role, User>
    {
        public UserStore(
            IRepository<User,long> userRepository,
            IRepository<UserLogin,long> userLoginRepository,
            IRepository<UserRole,long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting,long> userPermissionSettionRepository,
            IAbpSession session,
            IUnitOfWorkManager unitOfWorkManager)
            : base(userRepository,
            userLoginRepository,
            userRoleRepository,
            roleRepository,
            userPermissionSettionRepository,
            session,
            unitOfWorkManager){}
    }
}
