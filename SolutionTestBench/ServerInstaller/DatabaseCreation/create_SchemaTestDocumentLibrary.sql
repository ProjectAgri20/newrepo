USE [TestDocumentLibrary]
GO
/****** Object:  Table [dbo].[DocumentSet]    Script Date: 9/24/2015 4:04:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentSet](
	[SetId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[SetType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DocumentSet] PRIMARY KEY CLUSTERED 
(
	[SetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocumentSetItem]    Script Date: 9/24/2015 4:04:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentSetItem](
	[SetId] [int] NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_DocumentSetItem_SortOrder]  DEFAULT ((0)),
 CONSTRAINT [PK_DocumentSetAssoc] PRIMARY KEY CLUSTERED 
(
	[SetId] ASC,
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ItemQueryDomain]    Script Date: 9/24/2015 4:04:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemQueryDomain](
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](255) NOT NULL,
 CONSTRAINT [PK_ItemQueryDomain] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestDocument]    Script Date: 9/24/2015 4:04:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestDocument](
	[DocumentId] [uniqueidentifier] NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
	[FileType] [nvarchar](50) NOT NULL,
	[FileSize] [bigint] NOT NULL CONSTRAINT [DF_TestDocument_Size]  DEFAULT ((0)),
	[Pages] [int] NOT NULL CONSTRAINT [DF_TestDocument_Pages]  DEFAULT ((0)),
	[ColorMode] [nvarchar](50) NULL,
	[Orientation] [nvarchar](50) NULL,
	[Author] [nvarchar](50) NULL,
	[Vertical] [nvarchar](50) NULL,
	[Notes] [nvarchar](255) NULL,
	[Submitter] [varchar](50) NOT NULL,
	[SubmitDate] [datetime] NOT NULL,
	[AuthorType] [varchar](50) NULL,
	[Application] [varchar](50) NULL,
	[AppVersion] [varchar](50) NULL,
	[DefectId] [varchar](50) NULL,
	[Tag] [varchar](255) NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_TestDocument_DocumentId] PRIMARY KEY NONCLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestDocumentExtension]    Script Date: 9/24/2015 4:04:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestDocumentExtension](
	[Extension] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[ContentType] [varchar](50) NOT NULL DEFAULT ('application/octet-stream'),
 CONSTRAINT [PK_TestDocumentExtension] PRIMARY KEY CLUSTERED 
(
	[Extension] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[DocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_DocumentSetAssoc_DocumentSet] FOREIGN KEY([SetId])
REFERENCES [dbo].[DocumentSet] ([SetId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocumentSetItem] CHECK CONSTRAINT [FK_DocumentSetAssoc_DocumentSet]
GO
ALTER TABLE [dbo].[DocumentSetItem]  WITH CHECK ADD  CONSTRAINT [FK_DocumentSetAssoc_TestDocument] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[TestDocument] ([DocumentId])
GO
ALTER TABLE [dbo].[DocumentSetItem] CHECK CONSTRAINT [FK_DocumentSetAssoc_TestDocument]
GO
ALTER TABLE [dbo].[TestDocument]  WITH CHECK ADD  CONSTRAINT [FK_TestDocument_TestDocumentExtension] FOREIGN KEY([Extension])
REFERENCES [dbo].[TestDocumentExtension] ([Extension])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TestDocument] CHECK CONSTRAINT [FK_TestDocument_TestDocumentExtension]
GO
