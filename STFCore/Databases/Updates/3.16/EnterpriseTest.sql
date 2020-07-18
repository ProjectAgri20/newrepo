USE [EnterpriseTest]
GO

------------------------------------------------------------------------------------------------------------------------

-- 11/2/2015 - Gary Parham - Drop a stored procedure that is no longer needed.

/****** Object:  StoredProcedure [dbo].[sel_SessionsWithCounts]    Script Date: 11/2/2015 10:21:25 AM ******/
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_SCHEMA = 'dbo' and ROUTINE_NAME = 'sel_SessionsWithCounts')
BEGIN
	DROP PROCEDURE [dbo].[sel_SessionsWithCounts]
END
GO

------------------------------------------------------------------------------------------------------------------------

-- 11/4/2015 - BJ Myers - Add tables for storing asset and document selection data.
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'VirtualResourceMetadataAssetUsage')
BEGIN
	/****** Object:  Table [dbo].[VirtualResourceMetadataAssetUsage]    Script Date: 11/4/2015 3:40:12 PM ******/
	CREATE TABLE [dbo].[VirtualResourceMetadataAssetUsage](
		[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
		[AssetSelectionData] [xml] NOT NULL,
	 CONSTRAINT [PK_VirtualResourceMetadataAssetUsage] PRIMARY KEY CLUSTERED 
	(
		[VirtualResourceMetadataId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[VirtualResourceMetadataAssetUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
	REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[VirtualResourceMetadataAssetUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata]
END
GO


/****** Object:  Table [dbo].[VirtualResourceMetadataDocumentUsage]    Script Date: 11/4/2015 3:41:02 PM ******/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'VirtualResourceMetadataDocumentUsage')
BEGIN
	CREATE TABLE [dbo].[VirtualResourceMetadataDocumentUsage](
		[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
		[DocumentSelectionData] [xml] NOT NULL,
	 CONSTRAINT [PK_VirtualResourceMetadataDocumentUsage] PRIMARY KEY CLUSTERED 
	(
		[VirtualResourceMetadataId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[VirtualResourceMetadataDocumentUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
	REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[VirtualResourceMetadataDocumentUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata]
END
GO

------------------------------------------------------------------------------------------------------------------------
-- 11/9/2015 - BJ Myers - Add column for storing metadata version.
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'VirtualResourceMetadata' and COLUMN_NAME = 'MetadataVersion')
BEGIN
	ALTER TABLE VirtualResourceMetadata ADD MetadataVersion varchar(50) NULL
END
GO

------------------------------------------------------------------------------------------------------------------------
-- 11/12/2015 - Cambron - Adding new PluginSetting table.

/****** Object:  Table [dbo].[PluginSetting]    Script Date: 11/12/2015 5:38:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PluginSetting](
	[Plugin] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_PluginSetting] PRIMARY KEY CLUSTERED 
(
	[Plugin] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
