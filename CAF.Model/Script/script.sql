USE [CAF]
GO

/****** Object:  Table [dbo].[Sys_UserSettings]    Script Date: 2015/1/21 21:59:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_UserSettings](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[Note] [nvarchar](500) NULL,
	[Settings] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Sys_UserSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sys_UserSettings] ADD  CONSTRAINT [DF_Sys_UserSettings_Status]  DEFAULT ((1)) FOR [Status]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_UserSettings', @level2type=N'COLUMN',@level2name=N'Settings'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_UserSettings', @level2type=N'COLUMN',@level2name=N'UserId'
GO

USE [CAF]
GO

/****** Object:  Table [dbo].[Sys_Users]    Script Date: 2015/1/21 21:59:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL CONSTRAINT [DF_Sys_Users_Status]  DEFAULT ((0)),
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Sys_Users_CreatedDate]  DEFAULT (getdate()),
	[ChangedDate] [datetime] NOT NULL CONSTRAINT [DF_Sys_Users_ChangedDate]  DEFAULT (getdate()),
	[Note] [nvarchar](500) NULL,
	[LoginName] [nvarchar](20) NOT NULL,
	[Abb] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Pass] [nvarchar](50) NOT NULL,
	[PhoneNum] [nvarchar](30) NULL,
	[OrganizeId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sys_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'Note'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'LoginName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户简称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'Abb'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'Pass'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'PhoneNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组织架构Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'OrganizeId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电子邮件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Users', @level2type=N'COLUMN',@level2name=N'Email'
GO

USE [CAF]
GO

/****** Object:  Table [dbo].[Sys_Role]    Script Date: 2015/1/21 21:59:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[Note] [nvarchar](500) NULL,
	[Name] [nvarchar](20) NULL,
	[ParentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Sys_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sys_Role] ADD  CONSTRAINT [DF_Sys_Role_Status]  DEFAULT ((0)) FOR [Status]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Role', @level2type=N'COLUMN',@level2name=N'Note'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Role', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Role', @level2type=N'COLUMN',@level2name=N'ParentId'
GO



USE [CAF]
GO

/****** Object:  Table [dbo].[Sys_R_User_Role]    Script Date: 2015/1/21 21:59:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_R_User_Role](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_R_User_Role] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [CAF]
GO

/****** Object:  Table [dbo].[Sys_Organize]    Script Date: 2015/1/21 22:00:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_Organize](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL CONSTRAINT [DF_Sys_Organize_Status]  DEFAULT ((0)),
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[Note] [nvarchar](500) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Sort] [int] NOT NULL CONSTRAINT [DF_Sys_Organize_Sort]  DEFAULT ((0)),
	[Level] [nvarchar](20) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sys_Organize] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'Note'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父部门Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'ParentId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'Sort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门层级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'Level'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Organize', @level2type=N'COLUMN',@level2name=N'Code'
GO

USE [CAF]
GO

/****** Object:  View [dbo].[V_Users]    Script Date: 2015/1/21 22:00:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[V_Users]
AS
SELECT   t1.Id, t1.CreatedDate, t1.ChangedDate, t1.Note, t1.LoginName, t1.Name, t1.PhoneNum, t1.OrganizeId, t1.Email, 
                t2.Name AS OrganizeName,t2.[Level], dbo.F_GetUserRoles(t1.Id)  AS Roles,t1.Status,t1.Abb
FROM      dbo.Sys_Users AS t1 INNER JOIN
                dbo.Sys_Organize AS t2 ON t1.OrganizeId = t2.Id
WHERE   (t1.Status <> - 1)


GO




CREATE  FUNCTION [dbo].[F_GetUserRoles](@UserId uniqueidentifier)
RETURNS VARCHAR(300)
AS
BEGIN
DECLARE @Roles varchar(300)
SET @Roles = ''
-- 通过递归 SELECT 连接指定列存储到临时变量中
SELECT @Roles = CONVERT(VARCHAR(300), DicValue)+ ',' + @Roles FROM dbo.Sys_User_Role WHERE  UserId=@UserId
-- 去掉尾巴的 , (逗号)
IF LEN(@Roles) > 0
SET @Roles = LEFT(@Roles, LEN(@Roles) - 1)
--PRINT @Roles
RETURN @Roles
END
