
namespace CAF.Models
{
    using EntityFramework.Filters;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    public class BaseMap<T> : EntityTypeConfiguration<T> where T : class,IEntityBase
    {
        public BaseMap()
        {
            this.Property(i => i.Version).IsRowVersion();
            this.Property(i => i.CreatedDate).IsRequired();
            this.Property(i => i.ChangedDate).IsRequired();
            this.Filter("Status", i => i.Condition(l => l.Status != -1));

        }
    }

    public class RoleMap : BaseMap<Role>
    {
        public RoleMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
        }
    }


    public class ErrorLogMap : BaseMap<ErrorLog>
    {
        public ErrorLogMap()
            : base()
        {
            this.Property(i => i.UserName).IsRequired().HasMaxLength(20);
            this.Property(i => i.PageCode).IsRequired();
            this.Property(i => i.Ip).IsRequired().HasMaxLength(20);
            this.Property(i => i.Page).HasMaxLength(200);
            this.Property(i => i.Message).IsRequired().HasMaxLength(200);
            this.Property(i => i.Details).IsRequired();
        }
    }

    public class InfoLogMap : BaseMap<InfoLog>
    {
        public InfoLogMap()
            : base()
        {
            this.Property(i => i.UserName).IsRequired().HasMaxLength(20);
            this.Property(i => i.Action).IsRequired().HasMaxLength(200);
            this.Property(i => i.Page).HasMaxLength(200);
        }
    }

    public class PostMap : BaseMap<Post>
    {
        public PostMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(50);
            this.HasMany(i => i.Users).WithMany(l => l.Posts).Map(m =>
            {
                m.ToTable("R_PostUsers");
                m.MapLeftKey("Post_Id");
                m.MapRightKey("User_Id");
            });
        }
    }

    public class UserMap : BaseMap<User>
    {
        public UserMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
            this.Property(i => i.Abb).IsRequired().HasMaxLength(20);
            this.Property(i => i.LoginName).IsRequired().HasMaxLength(20);
            this.Property(i => i.Pass).IsRequired().HasMaxLength(50);
            this.Property(i => i.PhoneNum).IsRequired().HasMaxLength(30);
            this.Property(i => i.Email).IsRequired().HasMaxLength(50);
            this.HasMany(i => i.Roles).WithMany(l => l.Users).Map(m =>
            {
                m.ToTable("R_RoleUsers");
                m.MapLeftKey("Role_Id");
                m.MapRightKey("User_Id");

            });
            this.HasMany(i => i.UserNotes).WithOptional(i=>i.User).HasForeignKey(i => i.User_Id);

        }
    }
    public class UserSettingMap : BaseMap<UserSetting>
    {
        public UserSettingMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
            this.HasRequired(i => i.User).WithOptional(l => l.UserSetting).Map(m => { m.MapKey("User_Id"); });
        }
    }

    public class UserNoteMap : BaseMap<UserNote>
    {
        public UserNoteMap()
            : base()
        {
            this.Property(i => i.Desc).IsRequired().HasMaxLength(20);
            //            this.HasRequired(i => i.User).WithMany(l => l.UserNotes).HasForeignKey(i=>i.User_Id);
        }
    }

    public class DescriptionMap : ComplexTypeConfiguration<Description>
    {
        public DescriptionMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
        }
    }

}
