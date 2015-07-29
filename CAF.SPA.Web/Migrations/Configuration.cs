namespace CAF.SPA.Web.Migrations
{
    using CAF.SPA.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.ContextKey = "CAF.SPA.Web.Models.ApplicationDbContext";
        }
    }


    public class AccountDbRecreatedInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        public const string SA_USERID = "76edf148-3e31-4e9e-8cf8-f17d3c96f05f";
        public const string SA_USERNAME = "13980509508";
        public const string SA_EMAIL = "thornfield_he@126.com";
        public const string SA_PWD = "waterlily";
        public const string ADMINS_ROLENAME = "Admins";
        public const string USERS_ROLENAME = "Users";
        public const string SELLERS_ROLENAME = "Sellers";

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            const string fullName = "系统管理员";
            var roleNames = new string[] { ADMINS_ROLENAME, USERS_ROLENAME, SELLERS_ROLENAME };

            //Create Role Admin if it does not exist
            using (var roleManager = ApplicationRoleManager.CreateForEF(context))
            {
                if ((from item in roleNames
                     let role = roleManager.FindByName(item)
                     where role == null
                     select new IdentityRole(item)
                         into role
                         select roleManager.Create(role)).Any(roleresult => !roleresult.Succeeded))
                {
                    throw new Exception("初始化系统失败！");
                }

                using (var userManager = ApplicationUserManager.CreateForEF(context))
                {
                    var user = userManager.FindByName(SA_USERNAME);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            Id = SA_USERID,
                            UserName = SA_USERNAME,
                            Email = SA_EMAIL,
                            EmailConfirmed = false,
                            FullName = fullName,
                            TwoFactorEnabled = true,
                        };
                        userManager.Create(user, SA_PWD);
                        userManager.SetLockoutEnabled(user.Id, true);
                    }

                    // Add user admin to Role Admin if not already added
                    var rolesForUser = userManager.GetRoles(user.Id);
                    if (!rolesForUser.Contains(ADMINS_ROLENAME))
                    {
                        userManager.AddToRole(user.Id, ADMINS_ROLENAME);
                    }
                }
            }
        }
    }
}
