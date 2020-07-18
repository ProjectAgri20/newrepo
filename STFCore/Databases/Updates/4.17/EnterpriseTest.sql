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

-- bmyers 3/6/19 Remove ConfigurationObjectHistory table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationObjectHistory]') AND type in (N'U'))
DROP TABLE [dbo].[ConfigurationObjectHistory]
GO


-- bmyers 3/29/19 Create new ResourceTypeFrameworkClientPlatformAssociation table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation](
	[FrameworkClientPlatformId] [nvarchar](50) NOT NULL,
	[ResourceTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceTypeFrameworkClientPlatformAssociation] PRIMARY KEY CLUSTERED 
(
	[FrameworkClientPlatformId] ASC,
	[ResourceTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]'))
ALTER TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]'))
ALTER TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation] CHECK CONSTRAINT [FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]
GO


-- bmyers 3/29/19 Populate ResourceTypeFrameworkClientPlatformAssociation table from old table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceTypePlatformAssoc]') AND type in (N'U'))
INSERT INTO ResourceTypeFrameworkClientPlatformAssociation (FrameworkClientPlatformId, ResourceTypeName)
SELECT PlatformId, Name
FROM ResourceTypePlatformAssoc
GO


-- bmyers 3/29/19 Drop old tables
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceTypePlatformAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[ResourceTypePlatformAssoc]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachinePlatformAssoc]') AND type in (N'U'))
DROP TABLE [dbo].[VirtualMachinePlatformAssoc]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachinePlatform]') AND type in (N'U'))
DROP TABLE [dbo].[VirtualMachinePlatform]
GO
