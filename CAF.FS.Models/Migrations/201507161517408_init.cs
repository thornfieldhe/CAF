namespace CAF.FSModels.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Directories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Url = c.String(maxLength: 100),
                        ParentId = c.Guid(),
                        Level = c.String(nullable: false),
                        Sort = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Directories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.DirectoryRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Role_Id = c.Guid(nullable: false),
                        Directory_Id = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Directories", t => t.Directory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Directory_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Guid(),
                        Sort = c.Int(nullable: false),
                        Level = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 20),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizes", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description_Name = c.String(nullable: false, maxLength: 20),
                        LoginName = c.String(nullable: false, maxLength: 20),
                        Abb = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 20),
                        Pass = c.String(nullable: false, maxLength: 50),
                        PhoneNum = c.String(nullable: false, maxLength: 30),
                        Organize_Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizes", t => t.Organize_Id, cascadeDelete: true)
                .Index(t => t.Organize_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        PageCode = c.Int(nullable: false),
                        Page = c.String(maxLength: 200),
                        Ip = c.String(nullable: false, maxLength: 20),
                        Message = c.String(nullable: false, maxLength: 200),
                        Details = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InfoLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        Page = c.String(maxLength: 200),
                        Action = c.String(nullable: false, maxLength: 200),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Note = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        From = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.R_PostUsers",
                c => new
                    {
                        Post_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Id, t.User_Id })
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.R_RoleUsers",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Users", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.R_RoleOrganizes",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        Organize_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Organize_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Organizes", t => t.Organize_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Organize_Id);
            
            CreateTable(
                "dbo.Test1",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Test", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Test2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Class = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Test", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Test2", "Id", "dbo.Test");
            DropForeignKey("dbo.Test1", "Id", "dbo.Test");
            DropForeignKey("dbo.Directories", "ParentId", "dbo.Directories");
            DropForeignKey("dbo.DirectoryRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.R_RoleOrganizes", "Organize_Id", "dbo.Organizes");
            DropForeignKey("dbo.R_RoleOrganizes", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserSettings", "Id", "dbo.Users");
            DropForeignKey("dbo.R_RoleUsers", "User_Id", "dbo.Roles");
            DropForeignKey("dbo.R_RoleUsers", "Role_Id", "dbo.Users");
            DropForeignKey("dbo.R_PostUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.R_PostUsers", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Users", "Organize_Id", "dbo.Organizes");
            DropForeignKey("dbo.Organizes", "ParentId", "dbo.Organizes");
            DropForeignKey("dbo.DirectoryRoles", "Directory_Id", "dbo.Directories");
            DropIndex("dbo.Test2", new[] { "Id" });
            DropIndex("dbo.Test1", new[] { "Id" });
            DropIndex("dbo.R_RoleOrganizes", new[] { "Organize_Id" });
            DropIndex("dbo.R_RoleOrganizes", new[] { "Role_Id" });
            DropIndex("dbo.R_RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.R_RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.R_PostUsers", new[] { "User_Id" });
            DropIndex("dbo.R_PostUsers", new[] { "Post_Id" });
            DropIndex("dbo.UserSettings", new[] { "Id" });
            DropIndex("dbo.Users", new[] { "Organize_Id" });
            DropIndex("dbo.Organizes", new[] { "ParentId" });
            DropIndex("dbo.DirectoryRoles", new[] { "Directory_Id" });
            DropIndex("dbo.DirectoryRoles", new[] { "Role_Id" });
            DropIndex("dbo.Directories", new[] { "ParentId" });
            DropTable("dbo.Test2");
            DropTable("dbo.Test1");
            DropTable("dbo.R_RoleOrganizes");
            DropTable("dbo.R_RoleUsers");
            DropTable("dbo.R_PostUsers");
            DropTable("dbo.Test");
            DropTable("dbo.Messages");
            DropTable("dbo.InfoLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.ErrorLogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.UserSettings",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.Posts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.Organizes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.DirectoryRoles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
            DropTable("dbo.Directories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
        }
    }
}
