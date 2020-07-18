USE [AssetInventory]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING ASSETINVENTORY TABLES
-- Not all systems may have the AssetInventory table that is being updated.
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

-- kyoungman 9/24/2019 Create ExternalCredential table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalCredential]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ExternalCredential](
	[ExternalCredentialId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[ExternalCredentialType] [varchar](50) NOT NULL,
	[DomainUserName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ExternalCredential] PRIMARY KEY CLUSTERED 
(
	[ExternalCredentialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

