USE [EnterpriseTest]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING ENTERPRISETEST TABLES
-- Not all systems may have the EnterpriseTest table that is being updated.
-- This is especially true with the STB database.  So, if you are altering the
-- database schema then please wrap the SQL commands with the appropriate check
-- below.  This will help to ensure that an error does not occur when the SQL
-- script is ran.
--
-- * Check if table exists.
-- IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.<YOUR_TABLE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR TABLE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a table does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a view exists.
-- IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID('dbo.<YOUR_VIEW_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR VIEW ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a view does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a stored procedure exists.
-- IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.<YOUR_PROCEDURE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR PROCEDURE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a stored procedure does not exist, change the 'IF'
--       statement to
--          IF NOT EXISTS ( ... )
--
-- * Check if a column exists in a specific table.
-- IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.<TABLE_NAME_HERE>') and name = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/


-- bmyers 7/3/19 - Create new CategoryValue and CategoryValueParent tables
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryValue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryValue](
	[CategoryValueId] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_CategoryValue] PRIMARY KEY CLUSTERED 
(
	[CategoryValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryValueParent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryValueParent](
	[CategoryValueId] [int] NOT NULL,
	[ParentCategoryValueId] [int] NOT NULL,
 CONSTRAINT [PK_CategoryValueParent] PRIMARY KEY CLUSTERED 
(
	[CategoryValueId] ASC,
	[ParentCategoryValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryValue_CategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryValueParent]'))
ALTER TABLE [dbo].[CategoryValueParent]  WITH CHECK ADD  CONSTRAINT [FK_CategoryValue_CategoryId] FOREIGN KEY([CategoryValueId])
REFERENCES [dbo].[CategoryValue] ([CategoryValueId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryValue_CategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryValueParent]'))
ALTER TABLE [dbo].[CategoryValueParent] CHECK CONSTRAINT [FK_CategoryValue_CategoryId]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryValue_ParentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryValueParent]'))
ALTER TABLE [dbo].[CategoryValueParent]  WITH CHECK ADD  CONSTRAINT [FK_CategoryValue_ParentId] FOREIGN KEY([ParentCategoryValueId])
REFERENCES [dbo].[CategoryValue] ([CategoryValueId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryValue_ParentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryValueParent]'))
ALTER TABLE [dbo].[CategoryValueParent] CHECK CONSTRAINT [FK_CategoryValue_ParentId]
GO



-- bmyers 7/3/19 - Populate CategoryValue/Parent tables from ResourceWindowsCategory/Parent tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategory]') AND type in (N'U'))
BEGIN
	DELETE FROM CategoryValueParent
	DELETE FROM CategoryValue

	SET IDENTITY_INSERT CategoryValue ON
	INSERT INTO CategoryValue (CategoryValueId, Category, Value) SELECT CategoryId, CategoryType, Name FROM ResourceWindowsCategory
	INSERT INTO CategoryValueParent (CategoryValueId, ParentCategoryValueId) SELECT CategoryId, ParentCategoryId FROM ResourceWindowsCategoryParent
	SET IDENTITY_INSERT CategoryValue OFF
END
GO



-- bmyers 7/3/19 - Drop old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]') AND type in (N'U'))
DROP TABLE [dbo].[ResourceWindowsCategoryParent]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategory]') AND type in (N'U'))
DROP TABLE [dbo].[ResourceWindowsCategory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValueGroup]') AND type in (N'U'))
DROP TABLE [dbo].[ValueGroup]
GO




-- bmyers 7/5/19 - Rename columns and primary keys in SoftwareInstaller and SoftwareInstallerPackage
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[SoftwareInstaller]') and name = 'InstallerId')
EXEC sp_rename '[SoftwareInstaller].[InstallerId]', 'SoftwareInstallerId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[SoftwareInstallerPackage]') and name = 'PackageId')
EXEC sp_rename '[SoftwareInstallerPackage].[PackageId]', 'SoftwareInstallerPackageId'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstaller]') AND name = N'PK_Installer')
EXEC sp_rename 'PK_Installer', 'PK_SoftwareInstaller'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackage]') AND name = N'PK_InstallerPackage')
EXEC sp_rename 'PK_InstallerPackage', 'PK_SoftwareInstallerPackage'
GO



-- bmyers 7/5/19 - Create new software installer association tables
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SoftwareInstallerPackageItem](
	[SoftwareInstallerPackageId] [uniqueidentifier] NOT NULL,
	[SoftwareInstallerId] [uniqueidentifier] NOT NULL,
	[InstallOrder] [int] NOT NULL,
 CONSTRAINT [PK_SoftwareInstallerPackageItem] PRIMARY KEY CLUSTERED 
(
	[SoftwareInstallerPackageId] ASC,
	[SoftwareInstallerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageItem_SoftwareInstaller]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageItem]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageItem]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageItem_SoftwareInstaller] FOREIGN KEY([SoftwareInstallerId])
REFERENCES [dbo].[SoftwareInstaller] ([SoftwareInstallerId])
ON UPDATE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageItem_SoftwareInstaller]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageItem]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageItem] CHECK CONSTRAINT [FK_SoftwareInstallerPackageItem_SoftwareInstaller]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageItem_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageItem]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageItem]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageItem_SoftwareInstallerPackage] FOREIGN KEY([SoftwareInstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([SoftwareInstallerPackageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageItem_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageItem]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageItem] CHECK CONSTRAINT [FK_SoftwareInstallerPackageItem_SoftwareInstallerPackage]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageMetadataTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SoftwareInstallerPackageMetadataTypeAssoc](
	[SoftwareInstallerPackageId] [uniqueidentifier] NOT NULL,
	[MetadataTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SoftwareInstallerPackageMetadataTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[SoftwareInstallerPackageId] ASC,
	[MetadataTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageMetadataTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageMetadataTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageMetadataTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageMetadataTypeAssoc] CHECK CONSTRAINT [FK_SoftwareInstallerPackageMetadataTypeAssoc_MetadataType]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageMetadataTypeAssoc_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageMetadataTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageMetadataTypeAssoc_SoftwareInstallerPackage] FOREIGN KEY([SoftwareInstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([SoftwareInstallerPackageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageMetadataTypeAssoc_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageMetadataTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageMetadataTypeAssoc] CHECK CONSTRAINT [FK_SoftwareInstallerPackageMetadataTypeAssoc_SoftwareInstallerPackage]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageResourceTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SoftwareInstallerPackageResourceTypeAssoc](
	[SoftwareInstallerPackageId] [uniqueidentifier] NOT NULL,
	[ResourceTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SoftwareInstallerPackageResourceTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[SoftwareInstallerPackageId] ASC,
	[ResourceTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageResourceTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageResourceTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageResourceTypeAssoc_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageResourceTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageResourceTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageResourceTypeAssoc] CHECK CONSTRAINT [FK_SoftwareInstallerPackageResourceTypeAssoc_ResourceType]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageResourceTypeAssoc_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageResourceTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerPackageResourceTypeAssoc_SoftwareInstallerPackage] FOREIGN KEY([SoftwareInstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([SoftwareInstallerPackageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerPackageResourceTypeAssoc_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackageResourceTypeAssoc]'))
ALTER TABLE [dbo].[SoftwareInstallerPackageResourceTypeAssoc] CHECK CONSTRAINT [FK_SoftwareInstallerPackageResourceTypeAssoc_SoftwareInstallerPackage]
GO


-- bmyers 7/5/19 - Populate new software installer association tables from the old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]') AND type in (N'U'))
BEGIN
	DELETE FROM SoftwareInstallerPackageItem
	INSERT INTO SoftwareInstallerPackageItem (SoftwareInstallerPackageId, SoftwareInstallerId, InstallOrder) SELECT PackageId, InstallerId, InstallOrderNumber FROM SoftwareInstallerSetting
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]') AND type in (N'U'))
BEGIN
	DELETE FROM SoftwareInstallerPackageMetadataTypeAssoc
	INSERT INTO SoftwareInstallerPackageMetadataTypeAssoc (SoftwareInstallerPackageId, MetadataTypeName) SELECT InstallerPackageId, MetadataType FROM MetadataInstallerPackageAssoc
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]') AND type in (N'U'))
BEGIN
	DELETE FROM SoftwareInstallerPackageResourceTypeAssoc
	INSERT INTO SoftwareInstallerPackageResourceTypeAssoc (SoftwareInstallerPackageId, ResourceTypeName) SELECT InstallerPackageId, ResourceType FROM ResourceInstallerPackageAssoc
END
GO



-- bmyers 7/5/19 - Drop old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]') AND type in (N'U'))
DROP TABLE [dbo].[SoftwareInstallerSetting]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[MetadataInstallerPackageAssoc]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[ResourceInstallerPackageAssoc]
GO




-- bmyers 7/9/19 - Rename columns and primary keys in UserGroup and UserGroupAssoc
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[UserGroup]') and name = 'GroupName')
EXEC sp_rename '[UserGroup].[GroupName]', 'Name'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[UserGroupAssoc]') and name = 'GroupName')
EXEC sp_rename '[UserGroupAssoc].[GroupName]', 'UserGroupName'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UserGroup]') AND name = N'PK_OperatorGroup')
EXEC sp_rename 'PK_OperatorGroup', 'PK_UserGroup'
GO



-- bmyers 7/9/19 - Create new user group association tables
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGroupFrameworkClientAssociation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGroupFrameworkClientAssociation](
	[UserGroupName] [nvarchar](50) NOT NULL,
	[FrameworkClientHostName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserGroupFrameworkClientAssociation] PRIMARY KEY CLUSTERED 
(
	[UserGroupName] ASC,
	[FrameworkClientHostName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupFrameworkClientAssociation_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupFrameworkClientAssociation]'))
ALTER TABLE [dbo].[UserGroupFrameworkClientAssociation]  WITH CHECK ADD  CONSTRAINT [FK_UserGroupFrameworkClientAssociation_UserGroup] FOREIGN KEY([UserGroupName])
REFERENCES [dbo].[UserGroup] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupFrameworkClientAssociation_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupFrameworkClientAssociation]'))
ALTER TABLE [dbo].[UserGroupFrameworkClientAssociation] CHECK CONSTRAINT [FK_UserGroupFrameworkClientAssociation_UserGroup]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioUserGroupAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EnterpriseScenarioUserGroupAssoc](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[UserGroupName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EnterpriseScenarioUserGroupAssoc] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC,
	[UserGroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioUserGroupAssoc_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioUserGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioUserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioUserGroupAssoc_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioUserGroupAssoc_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioUserGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioUserGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioUserGroupAssoc_EnterpriseScenario]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioUserGroupAssoc_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioUserGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioUserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioUserGroupAssoc_UserGroup] FOREIGN KEY([UserGroupName])
REFERENCES [dbo].[UserGroup] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioUserGroupAssoc_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioUserGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioUserGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioUserGroupAssoc_UserGroup]
GO




-- bmyers 7/9/19 - Populate new user group association tables from the old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachineGroupAssoc]') AND type in (N'U'))
BEGIN
	DELETE FROM UserGroupFrameworkClientAssociation
	INSERT INTO UserGroupFrameworkClientAssociation (UserGroupName, FrameworkClientHostName) SELECT GroupName, MachineName FROM VirtualMachineGroupAssoc
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]') AND type in (N'U'))
BEGIN
	DELETE FROM EnterpriseScenarioUserGroupAssoc
	INSERT INTO EnterpriseScenarioUserGroupAssoc (EnterpriseScenarioId, UserGroupName) SELECT ScenarioId, GroupName FROM EnterpriseScenarioGroupAssoc
END
GO




-- bmyers 7/9/19 - Drop old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachineGroupAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[VirtualMachineGroupAssoc]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[EnterpriseScenarioGroupAssoc]
GO




-- bmyers 7/16/19 - Create new metadata type association tables
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceTypeMetadataTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceTypeMetadataTypeAssoc](
	[ResourceTypeName] [varchar](50) NOT NULL,
	[MetadataTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceTypeMetadataTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[ResourceTypeName] ASC,
	[MetadataTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeMetadataTypeAssoc]'))
ALTER TABLE [dbo].[ResourceTypeMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypeMetadataTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeMetadataTypeAssoc]'))
ALTER TABLE [dbo].[ResourceTypeMetadataTypeAssoc] CHECK CONSTRAINT [FK_ResourceTypeMetadataTypeAssoc_MetadataType]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeMetadataTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeMetadataTypeAssoc]'))
ALTER TABLE [dbo].[ResourceTypeMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypeMetadataTypeAssoc_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeMetadataTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeMetadataTypeAssoc]'))
ALTER TABLE [dbo].[ResourceTypeMetadataTypeAssoc] CHECK CONSTRAINT [FK_ResourceTypeMetadataTypeAssoc_ResourceType]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssociatedProductMetadataTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssociatedProductMetadataTypeAssoc](
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
	[MetadataTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AssociatedProductMetadataTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[AssociatedProductId] ASC,
	[MetadataTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductMetadataTypeAssoc_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductMetadataTypeAssoc]'))
ALTER TABLE [dbo].[AssociatedProductMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductMetadataTypeAssoc_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductMetadataTypeAssoc_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductMetadataTypeAssoc]'))
ALTER TABLE [dbo].[AssociatedProductMetadataTypeAssoc] CHECK CONSTRAINT [FK_AssociatedProductMetadataTypeAssoc_AssociatedProduct]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductMetadataTypeAssoc]'))
ALTER TABLE [dbo].[AssociatedProductMetadataTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductMetadataTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductMetadataTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductMetadataTypeAssoc]'))
ALTER TABLE [dbo].[AssociatedProductMetadataTypeAssoc] CHECK CONSTRAINT [FK_AssociatedProductMetadataTypeAssoc_MetadataType]
GO



-- bmyers 7/16/19 - Populate new metadata type association tables from the old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]') AND type in (N'U'))
BEGIN
	DELETE FROM ResourceTypeMetadataTypeAssoc
	INSERT INTO ResourceTypeMetadataTypeAssoc (ResourceTypeName, MetadataTypeName) SELECT ResourceTypeName, MetadataTypeName FROM MetadataTypeResourceTypeAssoc
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]') AND type in (N'U'))
BEGIN
	DELETE FROM AssociatedProductMetadataTypeAssoc
	INSERT INTO AssociatedProductMetadataTypeAssoc (AssociatedProductId, MetadataTypeName) SELECT AssociatedProductId, MetadataType FROM ProductPluginAssociation
END
GO



-- bmyers 7/16/19 - Drop old tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[MetadataTypeResourceTypeAssoc]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]') AND type in (N'U'))
DROP TABLE [dbo].[ProductPluginAssociation]
GO




-- bmyers 7/18/19 - Various renames
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScenarioSession]') AND type in (N'U'))
EXEC sp_rename '[ScenarioSession]', 'EnterpriseScenarioSession'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioSession]') AND name = N'PK_ScenarioSession')
EXEC sp_rename 'PK_ScenarioSession', 'PK_EnterpriseScenarioSession'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ResourceType]') AND name = N'PK_VirtualResourceType')
EXEC sp_rename 'PK_VirtualResourceType', 'PK_ResourceType'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataRetrySetting]') AND name = N'PK_VirtualResourceMetadataRetrySetting_1')
EXEC sp_rename 'PK_VirtualResourceMetadataRetrySetting_1', 'PK_VirtualResourceMetadataRetrySetting'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[VirtualResourceMetadataRetrySetting]') and name = 'SettingId')
EXEC sp_rename '[VirtualResourceMetadataRetrySetting].[SettingId]', 'VirtualResourceMetadataRetrySettingId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[CitrixWorker]') and name = 'DBWorkerRunMode')
EXEC sp_rename '[CitrixWorker].[DBWorkerRunMode]', 'CitrixExecutionMode'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[CitrixWorker]') and name = 'ServerHostname')
EXEC sp_rename '[CitrixWorker].[ServerHostname]', 'ServerHostName'
GO



-- bmyers 7/18/19 - Drop default value constraints
DECLARE @dropConstraintsSql NVARCHAR(MAX) = N'';

SELECT @dropConstraintsSql += N'
ALTER TABLE [' + OBJECT_NAME(parent_object_id) + '] DROP CONSTRAINT ' + name + ';'
FROM sys.default_constraints;

--PRINT @dropConstraintsSql;
EXEC sp_executesql @dropConstraintsSql;
GO



-- bmyers 7/18/19 - Drop unused columns
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[EnterpriseScenario]') and name = 'Deleted')
ALTER TABLE EnterpriseScenario DROP COLUMN Deleted
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[VirtualResource]') and name = 'Deleted')
ALTER TABLE VirtualResource DROP COLUMN Deleted
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[VirtualResourceMetadata]') and name = 'Deleted')
ALTER TABLE VirtualResourceMetadata DROP COLUMN Deleted
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[ResourceType]') and name = 'Platform')
ALTER TABLE ResourceType DROP COLUMN Platform
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.[ResourceType]') and name = 'PluginEnabled')
ALTER TABLE ResourceType DROP COLUMN PluginEnabled
GO



-- bmyers 7/18/19 - Drop unused user-defined functions
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionIdsMatchingPatterns]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SessionIdsMatchingPatterns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionIdsMatchingPatterns_Remove]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[SessionIdsMatchingPatterns_Remove]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValueListToContainedPatterns]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ValueListToContainedPatterns]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValueListToContainedPatterns_Remove]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ValueListToContainedPatterns_Remove]
GO



-- bmyers 7/22/19 - Modify foreign keys to enable cascading update/delete
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AdminWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[AdminWorker]'))
ALTER TABLE [dbo].[AdminWorker] DROP CONSTRAINT [FK_AdminWorker_VirtualResource]
GO

ALTER TABLE [dbo].[AdminWorker]  WITH CHECK ADD  CONSTRAINT [FK_AdminWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AdminWorker] CHECK CONSTRAINT [FK_AdminWorker_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CitrixWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[CitrixWorker]'))
ALTER TABLE [dbo].[CitrixWorker] DROP CONSTRAINT [FK_CitrixWorker_VirtualResource]
GO

ALTER TABLE [dbo].[CitrixWorker]  WITH CHECK ADD  CONSTRAINT [FK_CitrixWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CitrixWorker] CHECK CONSTRAINT [FK_CitrixWorker_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventLogCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventLogCollector]'))
ALTER TABLE [dbo].[EventLogCollector] DROP CONSTRAINT [FK_EventLogCollector_VirtualResource]
GO

ALTER TABLE [dbo].[EventLogCollector]  WITH CHECK ADD  CONSTRAINT [FK_EventLogCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EventLogCollector] CHECK CONSTRAINT [FK_EventLogCollector_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoadTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoadTester]'))
ALTER TABLE [dbo].[LoadTester] DROP CONSTRAINT [FK_LoadTester_VirtualResource]
GO

ALTER TABLE [dbo].[LoadTester]  WITH CHECK ADD  CONSTRAINT [FK_LoadTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LoadTester] CHECK CONSTRAINT [FK_LoadTester_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OfficeWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OfficeWorker]'))
ALTER TABLE [dbo].[OfficeWorker] DROP CONSTRAINT [FK_OfficeWorker_VirtualResource]
GO

ALTER TABLE [dbo].[OfficeWorker]  WITH CHECK ADD  CONSTRAINT [FK_OfficeWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OfficeWorker] CHECK CONSTRAINT [FK_OfficeWorker_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PerfMonCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[PerfMonCollector]'))
ALTER TABLE [dbo].[PerfMonCollector] DROP CONSTRAINT [FK_PerfMonCollector_VirtualResource]
GO

ALTER TABLE [dbo].[PerfMonCollector]  WITH CHECK ADD  CONSTRAINT [FK_PerfMonCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PerfMonCollector] CHECK CONSTRAINT [FK_PerfMonCollector_VirtualResource]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SolutionTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[SolutionTester]'))
ALTER TABLE [dbo].[SolutionTester] DROP CONSTRAINT [FK_SolutionTester_VirtualResource]
GO

ALTER TABLE [dbo].[SolutionTester]  WITH CHECK ADD  CONSTRAINT [FK_SolutionTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionTester] CHECK CONSTRAINT [FK_SolutionTester_VirtualResource]
GO



-- bmyers 7/22/19 - Create foreign key
DELETE FROM EnterpriseScenarioSession WHERE EnterpriseScenarioId NOT IN (SELECT EnterpriseScenarioId FROM EnterpriseScenario)
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioSession_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioSession]'))
ALTER TABLE [dbo].[EnterpriseScenarioSession] DROP CONSTRAINT [FK_EnterpriseScenarioSession_EnterpriseScenario]
GO

ALTER TABLE [dbo].[EnterpriseScenarioSession]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioSession_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EnterpriseScenarioSession] CHECK CONSTRAINT [FK_EnterpriseScenarioSession_EnterpriseScenario]
GO

