USE [AssetInventory]
GO
----------------------------------------------------------------------------
--tmong 8/11/2016: Adding to AssetInventory Tables for Robot and Badgebox
/****** Object:  Table [dbo].[Robot]    Script Date: 8/11/2016 12:11:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Robot')
BEGIN
	CREATE TABLE [dbo].[Robot](
		[RobotId] [uniqueidentifier] NOT NULL,
		[PrinterId] [varchar](50) NULL,
		[Address] [varchar](50) NOT NULL,
		[Description] [varchar](255) NOT NULL,
	 CONSTRAINT [PK_Robot] PRIMARY KEY CLUSTERED 
	(
		[RobotId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO

SET ANSI_PADDING OFF
GO

----------------------------------------------------------------------------
/****** Object:  Table [dbo].[BadgeBox]    Script Date: 8/11/2016 12:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'BadgeBox')
BEGIN
	CREATE TABLE [dbo].[BadgeBox](
		[BadgeBoxId] [uniqueidentifier] NOT NULL,
		[PrinterId] [varchar](50) NULL,
		[Description] [varchar](50) NULL,
		[IPAddress] [varchar](50) NULL,
	 CONSTRAINT [PK_BadgeBox] PRIMARY KEY CLUSTERED 
	(
		[BadgeBoxId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO

SET ANSI_PADDING OFF
GO
----------------------------------------------------------------------------
/****** Object:  Table [dbo].[Badge]    Script Date: 8/11/2016 12:12:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Badge')
BEGIN
	CREATE TABLE [dbo].[Badge](
		[BadgeId] [uniqueidentifier] NOT NULL,
		[BadgeBoxId] [uniqueidentifier] NULL,
		[Username] [varchar](50) NOT NULL,
		[Index] [tinyint] NOT NULL,
		[Description] [varchar](50) NULL,
	 CONSTRAINT [PK_Badge] PRIMARY KEY CLUSTERED 
	(
		[BadgeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO

SET ANSI_PADDING OFF
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Badge')
BEGIN
	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_Badge_BadgeBox')
	BEGIN
		ALTER TABLE [dbo].[Badge]  WITH CHECK ADD  CONSTRAINT [FK_Badge_BadgeBox] FOREIGN KEY([BadgeBoxId])
		REFERENCES [dbo].[BadgeBox] ([BadgeBoxId])

		ALTER TABLE [dbo].[Badge] CHECK CONSTRAINT [FK_Badge_BadgeBox]
	END
END
GO

----------------------------------------------------------------------------
-- gparham 9/15/2016: Adding a column to the FrameworkServer table
/****** Object:  Table [dbo].[Badge]    Script Date: 8/11/2016 12:12:01 PM ******/
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].FrameworkServer') AND name = 'StfServiceVersion')
BEGIN
	ALTER TABLE [dbo].[FrameworkServer] ADD StfServiceVersion VARCHAR(50) NULL
END
GO

--kyoungman 11/3/2016: Disable the foreign key constraint on Badge table, allowing nulls to be inserted.
ALTER TABLE Badge NOCHECK CONSTRAINT FK_Badge_BadgeBox
