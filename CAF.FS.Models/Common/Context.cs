
namespace CAF.FSModels
{
    using EntityFramework.Filters;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Interception;

    public class Context : DbContext
    {

        public Context() : base("CAFConnectionString") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DbInterception.Add(new FilterInterceptor());

            modelBuilder.Configurations.Add(new DirectoryMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new DirectoryRoleMap());
            modelBuilder.Configurations.Add(new ErrorLogMap());
            modelBuilder.Configurations.Add(new InfoLogMap());
            modelBuilder.Configurations.Add(new OrganizeMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new UserMap());
            //TPT继承类和基类分多表显示
            //            modelBuilder.Entity<Test>().Map(r => r.ToTable("t"));
            //            modelBuilder.Entity<Test1>().Map(r => r.ToTable("t1"));
            //            modelBuilder.Entity<Test2>().Map(r => r.ToTable("t2"));
            //TPH
            modelBuilder.Entity<Test>().Map(r => { r.Requires("From").HasValue("t"); });
            modelBuilder.Entity<Test1>().Map(r => { r.Requires("From").HasValue("t1"); });
            modelBuilder.Entity<Test2>().Map(r => { r.Requires("From").HasValue("t2"); });
            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// 上下文包装类用于封装Contex
    /// </summary>
    public class ContextWapper : SingletonBase<ContextWapper>
    {
        public Context Context
        {
            get
            {
                var context = new Context();
                context.EnableFilter("Status");
                return context;
            }
        }
    }
}
