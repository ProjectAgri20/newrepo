USE [AssetInventory]
GO
----------------------------------------------------------------------------
--kyoungman 8/23/2017 - Add DART Board data to Asset Inventory
/****** Object:  Table [dbo].[DartBoard]    Script Date: 8/23/2017 11:55:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DartBoard')
BEGIN
    CREATE TABLE [dbo].[DartBoard](
	    [DartBoardId] [varchar](50) NOT NULL,
	    [Address] [varchar](50) NOT NULL,
	    [FwVersion] [varchar](50) NULL,
	    [PrinterId] [varchar](50) NULL,
     CONSTRAINT [PK_DartBoard] PRIMARY KEY CLUSTERED 
    (
	    [DartBoardId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO
----------------------------------------------------------------------------
--kyoungman 8/24/2017 - Move ServerSettings from EnterpriseTest.SystemSettings to new table in AssetInventory
/****** Object:  Table [dbo].[ServerSetting]    Script Date: 8/24/2017 3:12:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ServerSetting')
BEGIN
    CREATE TABLE [dbo].[ServerSetting](
	    [ServerId] [uniqueidentifier] NOT NULL,
	    [Name] [varchar](50) NOT NULL,
	    [Value] [varchar](max) NOT NULL,
	    [Description] [varchar](255) NULL,
     CONSTRAINT [PK_ServerSetting] PRIMARY KEY CLUSTERED 
    (
	    [ServerId] ASC,
	    [Name] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

    SET ANSI_PADDING OFF

    ALTER TABLE [dbo].[ServerSetting]  WITH CHECK ADD  CONSTRAINT [FK_ServerSetting_FrameworkServer] FOREIGN KEY([ServerId])
    REFERENCES [dbo].[FrameworkServer] ([ServerId])

    ALTER TABLE [dbo].[ServerSetting] CHECK CONSTRAINT [FK_ServerSetting_FrameworkServer]
END
GO
