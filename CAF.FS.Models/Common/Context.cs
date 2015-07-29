
namespace CAF.Models
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
            modelBuilder.Configurations.Add(new DescriptionMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new ErrorLogMap());
            modelBuilder.Configurations.Add(new InfoLogMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserSettingMap());
            modelBuilder.Configurations.Add(new UserNoteMap());
            //TPH基类和继承类显示在同一张表
            modelBuilder.Entity<Message>().Map(r => { r.Requires("From").HasValue("message"); });
            modelBuilder.Entity<Message1>().Map(r => { r.Requires("From").HasValue("message1"); });
            modelBuilder.Entity<Message2>().Map(r => { r.Requires("From").HasValue("message2"); });
            //TPT继承类和基类分多表显示
            modelBuilder.Entity<Test>().Map(r => r.ToTable("Test"));
            modelBuilder.Entity<Test1>().Map(r => r.ToTable("Test1"));
            modelBuilder.Entity<Test2>().Map(r => r.ToTable("Test2"));
            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// 上下文包装类用于封装Contex
    /// </summary>
    internal class ContextWapper : SingletonBase<ContextWapper>
    {
        public Context Context
        {
            get
            {
                var context = new Context();
                context.EnableFilter("Status");
                context.EnableFilter("XXX");
                return context;
            }
        }
    }
}
