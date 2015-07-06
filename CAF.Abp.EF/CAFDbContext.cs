
namespace CAF.Abp.EF
{
    using CAF.Abp.Core.Authorization;
    using CAF.Abp.Core.Business;
    using CAF.Abp.Core.MultiTenancy;
    using CAF.Abp.Core.Users;
    using global::Abp.Zero.EntityFramework;
    using System.Data.Common;
    using System.Data.Entity;

    public class CAFDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        public virtual IDbSet<Test1> Test1s { get; set; }
        public virtual IDbSet<Test2> Test2s { get; set; }

        public CAFDbContext() : base("Default") { }
        public CAFDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public CAFDbContext(DbConnection connection) : base(connection, true) { }

    }
}
