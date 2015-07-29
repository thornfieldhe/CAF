namespace CAF.Models.Migrations
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LoginName = c.String(nullable: false, maxLength: 20),
                        Abb = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 20),
                        Pass = c.String(nullable: false, maxLength: 50),
                        PhoneNum = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 50),
                        Description_Name = c.String(nullable: false, maxLength: 20),
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
                "dbo.UserNotes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Desc = c.String(nullable: false, maxLength: 20),
                        User_Id = c.Guid(),
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
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                        User_Id = c.Guid(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
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
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(),
                        Note = c.String(),
                        From = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangedDate = c.DateTime(nullable: false),
                        Version = c.Binary(),
                        Note = c.String(),
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
            DropForeignKey("dbo.UserSettings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserNotes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.R_RoleUsers", "User_Id", "dbo.Roles");
            DropForeignKey("dbo.R_RoleUsers", "Role_Id", "dbo.Users");
            DropForeignKey("dbo.R_PostUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.R_PostUsers", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Test2", new[] { "Id" });
            DropIndex("dbo.Test1", new[] { "Id" });
            DropIndex("dbo.R_RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.R_RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.R_PostUsers", new[] { "User_Id" });
            DropIndex("dbo.R_PostUsers", new[] { "Post_Id" });
            DropIndex("dbo.UserSettings", new[] { "User_Id" });
            DropIndex("dbo.UserNotes", new[] { "User_Id" });
            DropTable("dbo.Test2");
            DropTable("dbo.Test1");
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
            DropTable("dbo.UserNotes",
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
            DropTable("dbo.Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "globalFilter_Status", "EntityFramework.Filters.FilterDefinition" },
                });
        }
    }
}
