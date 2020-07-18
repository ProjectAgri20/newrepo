USE [ScalableTestDataLog]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING DATALOG TABLES
-- Not all systems may have the DataLog table that is being updated.  This is
-- especially true with the STB database.  So, if you are altering the database
-- schema then please wrap the SQL commands with the appropriate check below.
-- This will help to ensure that an error does not occur when the SQL script
-- is ran.
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
-- * Check if a column exists in a specific table. (Same syntax works for views also)
-- IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '<TABLE_NAME_HERE>' and COLUMN_NAME = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/
/* dwa, adding two new columns to the table PullPrintJobRetrieval for tracking pull print documents */
 IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.PullPrintJobRetrieval'))
 BEGIN
	IF COL_LENGTH('PullPrintJobRetrieval', 'InitialJobCount') is NULL
	BEGIN
		ALTER TABLE PullPrintJobRetrieval ADD InitialJobCount int NULL;
	END


	IF COL_LENGTH('PullPrintJobRetrieval', 'FinalJobCount') is NULL
	BEGIN
		ALTER TABLE PullPrintJobRetrieval ADD FinalJobCount int NULL;
	END
 END

-- 2017 Mar 09 Max Tuinstra
-- Improve the way solutions and other "products" are associated with scenarios/sessions

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO


IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.SessionProductAssoc'))
BEGIN
	-- This table originally existed in EnterpriseTest, but it is more appropriate for DataLog since it contains session-specific info.
	-- Version column is for solution version or version of whatever product
	CREATE TABLE [dbo].[SessionProductAssoc](
		[SessionId] [varchar](50) NOT NULL,
		[EnterpriseTestAssociatedProductId] [uniqueidentifier] NOT NULL,
		[Version] [nvarchar](450) NULL,  -- Later we will index this column.  The maximum index key length is 900 bytes (450 * 2).  If this column gets any wider, some inserts will fail.
		CONSTRAINT [PK_SessionProductAssoc] PRIMARY KEY CLUSTERED 
	(
		[SessionId] ASC,
		[EnterpriseTestAssociatedProductId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	SET ANSI_PADDING OFF

	ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON DELETE CASCADE

	-- Instruct SQL Server to verify data affected by constraint so that constraint is trusted for query optimization.  http://sqlblog.com/blogs/hugo_kornelis/archive/2007/03/29/can-you-trust-your-constraints.aspx
	ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_SessionSummary]

	CREATE NONCLUSTERED INDEX IDX_SessionProductAssoc_Version
	ON [dbo].[SessionProductAssoc] (Version)
END
