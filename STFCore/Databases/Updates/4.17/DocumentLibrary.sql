USE [TestDocumentLibrary]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING TESTDOCUMENTLIBRARY TABLES
-- Not all systems may have the TestDocumentLibrary table that is being
-- updated.  This is especially true with the STB database.  So, if you are
-- altering the database schema then please wrap the SQL commands with the
-- appropriate check below.  This will help to ensure that an error does not
-- occur when the SQL script is ran.
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

-- bmyers 3/15/19 Rename tables
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentSet]') AND type in (N'U'))
EXEC sp_rename 'DocumentSet', 'TestDocumentSet'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentSetItem]') AND type in (N'U'))
EXEC sp_rename 'DocumentSetItem', 'TestDocumentSetItem'
GO


-- bmyers 3/15/19 Rename columns
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSet]') AND name = 'SetId')
EXEC sp_rename 'TestDocumentSet.SetId', 'TestDocumentSetId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]') AND name = 'DocumentId')
EXEC sp_rename 'TestDocumentSetItem.DocumentId', 'TestDocumentId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]') AND name = 'SetId')
EXEC sp_rename 'TestDocumentSetItem.SetId', 'TestDocumentSetId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TestDocument]') AND name = 'DocumentId')
EXEC sp_rename 'TestDocument.DocumentId', 'TestDocumentId'
GO


-- bmyers 3/15/19 Rename primary keys
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocument]') AND name = 'PK_TestDocument_DocumentId')
EXEC sp_rename 'PK_TestDocument_DocumentId', 'PK_TestDocument'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSet]') AND name = 'PK_DocumentSet')
EXEC sp_rename 'PK_DocumentSet', 'PK_TestDocumentSet'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]') AND name = 'PK_DocumentSetAssoc')
EXEC sp_rename 'PK_DocumentSetAssoc', 'PK_TestDocumentSetItem'
GO


-- bmyers 3/15/19 Rename/modify foreign keys
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument] DROP CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO

ALTER TABLE [dbo].[TestDocument]  WITH CHECK ADD  CONSTRAINT [FK_TestDocument_TestDocumentExtension] FOREIGN KEY([Extension])
REFERENCES [dbo].[TestDocumentExtension] ([Extension])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[TestDocument] CHECK CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DocumentSetAssoc_DocumentSet]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] DROP CONSTRAINT [FK_DocumentSetAssoc_DocumentSet]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocumentSet]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] DROP CONSTRAINT [FK_TestDocumentSetItem_TestDocumentSet]
GO

ALTER TABLE [dbo].[TestDocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_TestDocumentSetItem_TestDocumentSet] FOREIGN KEY([TestDocumentSetId])
REFERENCES [dbo].[TestDocumentSet] ([TestDocumentSetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TestDocumentSetItem] CHECK CONSTRAINT [FK_TestDocumentSetItem_TestDocumentSet]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DocumentSetAssoc_TestDocument]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] DROP CONSTRAINT [FK_DocumentSetAssoc_TestDocument]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocument]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] DROP CONSTRAINT [FK_TestDocumentSetItem_TestDocument]
GO

ALTER TABLE [dbo].[TestDocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_TestDocumentSetItem_TestDocument] FOREIGN KEY([TestDocumentId])
REFERENCES [dbo].[TestDocument] ([TestDocumentId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[TestDocumentSetItem] CHECK CONSTRAINT [FK_TestDocumentSetItem_TestDocument]
GO


-- bmyers 3/15/19 Drop default value constraints
DECLARE @dropConstraintsSql NVARCHAR(MAX) = N'';

SELECT @dropConstraintsSql += N'
ALTER TABLE ' + OBJECT_NAME(parent_object_id) + ' DROP CONSTRAINT ' + name + ';'
FROM sys.default_constraints;

--PRINT @dropConstraintsSql;
EXEC sp_executesql @dropConstraintsSql;
GO


-- bmyers 3/15/19 Drop obsolete ItemQueryDomain table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemQueryDomain]') AND type in (N'U'))
DROP TABLE [dbo].[ItemQueryDomain]
GO


