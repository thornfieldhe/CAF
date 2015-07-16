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
                        RoleId = c.Guid(nullable: false),
                        DirectoryId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Directories", t => t.DirectoryId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.DirectoryId);
            
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
                        Organize_Id = c.Guid(),
                        User_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizes", t => t.Organize_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Organize_Id)
                .Index(t => t.User_Id);
            
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
                        LoginName = c.String(nullable: false, maxLength: 20),
                        Abb = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 20),
                        Pass = c.String(nullable: false, maxLength: 50),
                        PhoneNum = c.String(nullable: false, maxLength: 30),
                        OrganizeId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Organizes", t => t.OrganizeId, cascadeDelete: true)
                .Index(t => t.OrganizeId);
            
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
                "dbo.Tests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Age = c.Int(),
                        Class = c.String(),
                        From = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostUsers",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PostUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PostUsers", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Users", "OrganizeId", "dbo.Organizes");
            DropForeignKey("dbo.Roles", "Organize_Id", "dbo.Organizes");
            DropForeignKey("dbo.Organizes", "ParentId", "dbo.Organizes");
            DropForeignKey("dbo.Directories", "ParentId", "dbo.Directories");
            DropForeignKey("dbo.DirectoryRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.DirectoryRoles", "DirectoryId", "dbo.Directories");
            DropIndex("dbo.PostUsers", new[] { "User_Id" });
            DropIndex("dbo.PostUsers", new[] { "Post_Id" });
            DropIndex("dbo.Users", new[] { "OrganizeId" });
            DropIndex("dbo.Organizes", new[] { "ParentId" });
            DropIndex("dbo.Roles", new[] { "User_Id" });
            DropIndex("dbo.Roles", new[] { "Organize_Id" });
            DropIndex("dbo.DirectoryRoles", new[] { "DirectoryId" });
            DropIndex("dbo.DirectoryRoles", new[] { "RoleId" });
            DropIndex("dbo.Directories", new[] { "ParentId" });
            DropTable("dbo.PostUsers");
            DropTable("dbo.Tests");
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
