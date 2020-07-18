USE [ScalableTestDataLog]
GO

-- Cambron, this is added to support dashboard reporting for both STF and STB.
-- CHECK THE TABLE BEFORE RUNNING.  THIS MAY ALREADY BE ADDED.
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ActivityExecution' and COLUMN_NAME = 'ResourceInstanceId')
BEGIN
	ALTER TABLE ActivityExecution ADD [ResourceInstanceId] [nvarchar](50) NOT NULL CONSTRAINT [ResourceInstanceIdConstraint]  DEFAULT ('')
END
GO

------------------------------------------------------------------------------------------------------------------------

-- 11/2/2015 - Gary Parham - Drop a stored procedure that is no longer needed.

/****** Object:  StoredProcedure [dbo].[sel_SessionCounts]    Script Date: 11/2/2015 10:21:42 AM ******/
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_SCHEMA = 'dbo' and ROUTINE_NAME = 'sel_SessionCounts')
BEGIN
	DROP PROCEDURE [dbo].[sel_SessionCounts]
END
GO

/* dwanderson 11/06/2015  These three tables are added to support storing device information and Memory status of Jedi devices. */
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'SessionDevice')
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	SET ANSI_PADDING ON
	CREATE TABLE [dbo].[SessionDevice](
		[SessionDeviceId] [uniqueidentifier] NOT NULL,
		[SessionId] [varchar](50) NOT NULL,
		[DeviceId] [varchar](50) NOT NULL,
		[ProductName] [varchar](100) NOT NULL,
		[DeviceName] [varchar](255) NOT NULL,
		[FirmwareRevision] [varchar](50) NOT NULL,
		[FirmwareDatecode] [varchar](10) NOT NULL,
		[FirmwareType] [varchar](50) NOT NULL,
		[ModelNumber] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_SessionDevice] PRIMARY KEY CLUSTERED 
	(
		[SessionDeviceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	SET ANSI_PADDING OFF
	ALTER TABLE [dbo].[SessionDevice]  WITH CHECK ADD  CONSTRAINT [FK_SessionDevice_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[SessionDevice] CHECK CONSTRAINT [FK_SessionDevice_SessionSummary]
END
GO

/****** Object:  Table [dbo].[DeviceMemorySnapshot]    Script Date: 11/12/2015 9:22:33 AM ******/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'DeviceMemorySnapshot')
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	SET ANSI_PADDING ON
	CREATE TABLE [dbo].[DeviceMemorySnapshot](
		[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
		[SessionId] [varchar](50) NOT NULL,
		[DeviceId] [varchar](50) NOT NULL,
		[SnapshotLabel] [varchar](50) NULL,
		[UsageCount] [int] NULL,
		[SnapshotDateTime] [datetime] NOT NULL,
	 CONSTRAINT [PK_DeviceMemorySnapshot] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemorySnapshotId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	SET ANSI_PADDING OFF

	ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[SessionSummary] ([SessionId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary]
END
GO

/****** Object:  Table [dbo].[DeviceMemoryCount]    Script Date: 11/6/2015 12:29:29 PM ******/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'DeviceMemoryCount')
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	SET ANSI_PADDING ON
	CREATE TABLE [dbo].[DeviceMemoryCount](
		[DeviceMemoryCountId] [uniqueidentifier] NOT NULL,
		[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
		[CategoryName] [varchar](255) NOT NULL,
		[DataLabel] [varchar](100) NOT NULL,
		[DataValue] [bigint] NOT NULL,
	 CONSTRAINT [PK_DeviceMemoryCount] PRIMARY KEY CLUSTERED 
	(
		[DeviceMemoryCountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	SET ANSI_PADDING OFF
	ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
	REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot]
END
GO

------------------------------------------------------------------------------------------------------------------------

-- 11/16/2015 - BJ Myers - Add Label column to activity execution
ALTER TABLE ActivityExecution ADD Label nvarchar(1024) NULL
GO

