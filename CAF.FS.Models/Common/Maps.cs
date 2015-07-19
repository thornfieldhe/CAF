
namespace CAF.Models
{
    using EntityFramework.Filters;
    using System.Data.Entity.ModelConfiguration;

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

    public class DirectoryMap : BaseMap<Directory>
    {
        public DirectoryMap()
            : base()
        {

            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
            this.Property(i => i.Level).IsRequired();
            this.Property(i => i.Sort).IsRequired();
            this.HasOptional(i => i.Parent).WithMany(i => i.Children).HasForeignKey(p => p.ParentId);
        }
    }

    public class RoleMap : BaseMap<Role>
    {
        public RoleMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
            this.HasMany(i => i.Organizes).WithMany(l => l.Roles).Map(m =>
            {
                m.ToTable("R_RoleOrganizes");
                m.MapLeftKey("Role_Id");
                m.MapRightKey("Organize_Id");
            });
        }
    }

    public class DirectoryRoleMap : BaseMap<DirectoryRole>
    {
        public DirectoryRoleMap()
            : base()
        {
            this.Property(i => i.Role_Id).IsRequired();
            this.Property(i => i.Directory_Id).IsRequired();
            this.HasRequired(i => i.Role).WithMany(r => r.DirectoryRoles).HasForeignKey(i => i.Role_Id);
            this.HasRequired(i => i.Directory).WithMany(r => r.DirectoryRoles).HasForeignKey(i => i.Directory_Id);
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

    public class OrganizeMap : BaseMap<Organize>
    {
        public OrganizeMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(50);
            this.Property(i => i.Sort).IsRequired();
            this.Property(i => i.Level).IsRequired().HasMaxLength(20);
            this.Property(i => i.Code).IsRequired().HasMaxLength(20);
            this.HasOptional(i => i.Parent).WithMany(i => i.Children).HasForeignKey(p => p.ParentId);
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
            this.HasRequired(i => i.Organize).WithMany(l => l.Users).HasForeignKey(i => i.Organize_Id);
            //this.HasOptional(i => i.Organize).WithMany(l => l.Users).HasForeignKey(i=>i.Organize_Id);
        }
    }
    public class UserSettingMap : BaseMap<UserSetting>
    {
        public UserSettingMap()
            : base()
        {
            this.Property(i => i.Name).IsRequired().HasMaxLength(20);
            this.HasRequired(i => i.User).WithOptional(l => l.UserSetting);
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

    public class PostUserOrganizeMap : ComplexTypeConfiguration<PostUserOrganize>
    {
        public PostUserOrganizeMap()
            : base()
        {
            this.Property(i => i.UserId).IsRequired();
            this.Property(i => i.PostId).IsRequired();
            this.Property(i => i.OrganizeId).IsRequired();
        }
    }
}
