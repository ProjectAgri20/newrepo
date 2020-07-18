
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[TestDocument]  ******/
USE [TestDocumentLibrary]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestDocument]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TestDocument](
	[TestDocumentId] [uniqueidentifier] NOT NULL,
	[Extension] [nvarchar](10) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FileType] [nvarchar](50) NULL,
	[FileSize] [bigint] NOT NULL,
	[Pages] [int] NOT NULL,
	[ColorMode] [varchar](10) NOT NULL,
	[Orientation] [varchar](10) NOT NULL,
	[Author] [nvarchar](50) NULL,
	[Vertical] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[Submitter] [nvarchar](50) NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[AuthorType] [nvarchar](50) NULL,
	[Application] [nvarchar](50) NULL,
	[AppVersion] [nvarchar](50) NULL,
	[DefectId] [nvarchar](50) NULL,
	[Tag] [nvarchar](255) NULL,
 CONSTRAINT [PK_TestDocument] PRIMARY KEY NONCLUSTERED 
(
	[TestDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocument]') AND name = N'IDX_TestDocument_FileName')
CREATE UNIQUE CLUSTERED INDEX [IDX_TestDocument_FileName] ON [dbo].[TestDocument]
(
	[FileName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Table [dbo].[TestDocumentExtension]  ******/
USE [TestDocumentLibrary]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentExtension]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TestDocumentExtension](
	[Extension] [nvarchar](10) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[ContentType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TestDocumentExtension] PRIMARY KEY CLUSTERED 
(
	[Extension] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[TestDocumentSet]  ******/
USE [TestDocumentLibrary]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TestDocumentSet](
	[TestDocumentSetId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SetType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TestDocumentSet] PRIMARY KEY CLUSTERED 
(
	[TestDocumentSetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[TestDocumentSetItem]  ******/
USE [TestDocumentLibrary]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TestDocumentSetItem](
	[TestDocumentSetId] [int] NOT NULL,
	[TestDocumentId] [uniqueidentifier] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_TestDocumentSetItem] PRIMARY KEY CLUSTERED 
(
	[TestDocumentSetId] ASC,
	[TestDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/*****************************************************************************
 * INDEXES                                                                   *
 *****************************************************************************/

/******  Object:  Index [PK_TestDocument]  ******/
USE [TestDocumentLibrary]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TestDocument]') AND name = N'PK_TestDocument')
ALTER TABLE [dbo].[TestDocument] ADD  CONSTRAINT [PK_TestDocument] PRIMARY KEY NONCLUSTERED 
(
	[TestDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/******  Object:  Foreign Key [FK_TestDocument_TestDocumentExtension]  ******/
USE [TestDocumentLibrary]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument]  WITH CHECK ADD  CONSTRAINT [FK_TestDocument_TestDocumentExtension] FOREIGN KEY([Extension])
REFERENCES [dbo].[TestDocumentExtension] ([Extension])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocument_TestDocumentExtension]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocument]'))
ALTER TABLE [dbo].[TestDocument] CHECK CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO

/******  Object:  Foreign Key [FK_TestDocumentSetItem_TestDocument]  ******/
USE [TestDocumentLibrary]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocument]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_TestDocumentSetItem_TestDocument] FOREIGN KEY([TestDocumentId])
REFERENCES [dbo].[TestDocument] ([TestDocumentId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocument]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] CHECK CONSTRAINT [FK_TestDocumentSetItem_TestDocument]
GO

/******  Object:  Foreign Key [FK_TestDocumentSetItem_TestDocumentSet]  ******/
USE [TestDocumentLibrary]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocumentSet]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_TestDocumentSetItem_TestDocumentSet] FOREIGN KEY([TestDocumentSetId])
REFERENCES [dbo].[TestDocumentSet] ([TestDocumentSetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TestDocumentSetItem_TestDocumentSet]') AND parent_object_id = OBJECT_ID(N'[dbo].[TestDocumentSetItem]'))
ALTER TABLE [dbo].[TestDocumentSetItem] CHECK CONSTRAINT [FK_TestDocumentSetItem_TestDocumentSet]
GO

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/*****************************************************************************
 * VIEWS                                                                     *
 *****************************************************************************/

/*****************************************************************************
 * TRIGGERS                                                                  *
 *****************************************************************************/

/*****************************************************************************
 * USERS                                                                     *
 *****************************************************************************/

/******  Object:  User [document_admin]  ******/
USE [TestDocumentLibrary]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'document_admin')
CREATE USER [document_admin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [document_admin]
GO
