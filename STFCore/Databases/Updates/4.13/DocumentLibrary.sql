USE [TestDocumentLibrary]
GO

---------------------------------------------------------------------------------------------------------
-- bmyers -- Modifications to columns to enable unicode and be more consistent with other databases

-- Must drop keys so that columns can be modified
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument] DROP CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentExtension]') AND name = N'PK_TestDocumentExtension')
ALTER TABLE [dbo].[TestDocumentExtension] DROP CONSTRAINT [PK_TestDocumentExtension] WITH ( ONLINE = OFF )
GO

-- Change columns
ALTER TABLE DocumentSet ALTER COLUMN Name nvarchar(255) NOT NULL
ALTER TABLE DocumentSet ALTER COLUMN SetType nvarchar(50) NOT NULL
GO

ALTER TABLE TestDocument ALTER COLUMN Extension nvarchar(10) NOT NULL
ALTER TABLE TestDocument ALTER COLUMN FileName nvarchar(255) NOT NULL
ALTER TABLE TestDocument ALTER COLUMN FileType nvarchar(50) NULL
ALTER TABLE TestDocument ALTER COLUMN ColorMode varchar(10) NOT NULL
ALTER TABLE TestDocument ALTER COLUMN Orientation varchar(10) NOT NULL
ALTER TABLE TestDocument ALTER COLUMN Submitter nvarchar(50) NOT NULL
ALTER TABLE TestDocument ALTER COLUMN AuthorType nvarchar(50) NULL
ALTER TABLE TestDocument ALTER COLUMN Application nvarchar(50) NULL
ALTER TABLE TestDocument ALTER COLUMN AppVersion nvarchar(50) NULL
ALTER TABLE TestDocument ALTER COLUMN DefectId nvarchar(50) NULL
ALTER TABLE TestDocument ALTER COLUMN Tag nvarchar(255) NULL
GO

ALTER TABLE TestDocumentExtension ALTER COLUMN Extension nvarchar(10) NOT NULL
GO

-- Restore keys

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentExtension]') AND name = N'PK_TestDocumentExtension')
ALTER TABLE [dbo].[TestDocumentExtension] ADD  CONSTRAINT [PK_TestDocumentExtension] PRIMARY KEY CLUSTERED 
(
	[Extension] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument]  WITH CHECK ADD  CONSTRAINT [FK_TestDocument_TestDocumentExtension] FOREIGN KEY([Extension])
REFERENCES [dbo].[TestDocumentExtension] ([Extension])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument] CHECK CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO

-- Drop obsolete column
ALTER TABLE TestDocument DROP COLUMN [Version]
---------------------------------------------------------------------------------------------------------
