
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/******  Object:  Schema [Reports]  ******/
USE [EnterpriseTestODS]
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Reports')
EXEC sys.sp_executesql N'CREATE SCHEMA [Reports]'
GO

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[ActivityExecution]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecution](
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[ResourceMetadataId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityName] [nvarchar](255) NULL,
	[ActivityType] [varchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[HostName] [nvarchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Status] [varchar](20) NOT NULL,
	[ResultMessage] [nvarchar](1024) NULL,
	[ResourceInstanceId] [nvarchar](50) NULL,
	[ResultCategory] [nvarchar](1024) NULL,
 CONSTRAINT [PK_ActivityExecution] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionAssetUsage]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionAssetUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionAssetUsage](
	[ActivityExecutionAssetUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[AssetId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ActivityExecutionAssetUsage] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionAssetUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionDetail]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionDetail](
	[ActivityExecutionDetailId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[Label] [nvarchar](255) NULL,
	[Message] [nvarchar](1024) NULL,
	[DetailDateTime] [datetime] NULL,
 CONSTRAINT [PK_ActivityExecutionDetail] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionDocumentUsage]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDocumentUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionDocumentUsage](
	[ActivityExecutionDocumentUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[DocumentName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ActivityExecutionDocumentUsage] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionDocumentUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionPerformance]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionPerformance](
	[ActivityExecutionPerformanceId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[EventLabel] [varchar](255) NULL,
	[EventIndex] [int] NULL,
	[EventDateTime] [datetime] NULL,
 CONSTRAINT [Pk_ActivityExecutionPerformance] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionPerformanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionRetries]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionRetries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionRetries](
	[ActivityExecutionRetriesId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[Status] [varchar](20) NULL,
	[ResultMessage] [nvarchar](1024) NULL,
	[ResultCategory] [nvarchar](1024) NULL,
	[RetryStartDateTime] [datetime] NULL,
 CONSTRAINT [PK_ActivityExecutionRetries] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionRetriesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ActivityExecutionServerUsage]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionServerUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityExecutionServerUsage](
	[ActivityExecutionServerUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[ServerName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ActivityExecutionServerUsage] PRIMARY KEY CLUSTERED 
(
	[ActivityExecutionServerUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ConnectorJobInput]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConnectorJobInput]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConnectorJobInput](
	[ConnectorJobInputId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[DeviceId] [nvarchar](50) NULL,
	[AppName] [nvarchar](50) NULL,
	[JobType] [nvarchar](50) NULL,
	[LoginID] [nvarchar](50) NULL,
	[FilePath] [nvarchar](255) NULL,
	[FilePrefix] [nvarchar](50) NULL,
	[OptionsData] [nvarchar](max) NULL,
	[PageCount] [int] NULL,
	[JobEndStatus] [nvarchar](50) NULL,
 CONSTRAINT [PK_ConnectorJobInput] PRIMARY KEY CLUSTERED 
(
	[ConnectorJobInputId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DeviceEvent]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceEvent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceEvent](
	[DeviceEventId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[AssetId] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EventDateTime] [datetime] NULL,
	[EventType] [nvarchar](20) NULL,
	[EventCode] [nvarchar](20) NULL,
	[EventDescription] [nvarchar](1024) NULL,
 CONSTRAINT [PK_DeviceEvent] PRIMARY KEY CLUSTERED 
(
	[DeviceEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DeviceMemoryCount]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemoryCount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceMemoryCount](
	[DeviceMemoryCountId] [uniqueidentifier] NOT NULL,
	[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	[CategoryName] [varchar](255) NOT NULL,
	[DataLabel] [varchar](255) NOT NULL,
	[DataValue] [bigint] NOT NULL,
 CONSTRAINT [PK_DeviceMemoryCount] PRIMARY KEY CLUSTERED 
(
	[DeviceMemoryCountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DeviceMemorySnapshot]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemorySnapshot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceMemorySnapshot](
	[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[DeviceId] [nvarchar](50) NOT NULL,
	[SnapshotLabel] [nvarchar](50) NULL,
	[UsageCount] [int] NULL,
	[SnapshotDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_DeviceMemorySnapshot] PRIMARY KEY CLUSTERED 
(
	[DeviceMemorySnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DeviceMemoryXml]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemoryXml]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceMemoryXml](
	[DeviceMemoryXmlId] [uniqueidentifier] NOT NULL,
	[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	[MemoryXml] [xml] NULL,
 CONSTRAINT [PK_DeviceMemoryXML] PRIMARY KEY CLUSTERED 
(
	[DeviceMemoryXmlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendJobInput]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendJobInput](
	[DigitalSendJobInputId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[FilePrefix] [nvarchar](255) NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Sender] [nvarchar](50) NULL,
	[DeviceId] [nvarchar](50) NULL,
	[ScanType] [nvarchar](50) NULL,
	[JobEndStatus] [nvarchar](50) NULL,
	[PageCount] [smallint] NULL,
	[DestinationCount] [smallint] NULL,
	[Ocr] [bit] NULL,
 CONSTRAINT [PK_DigitalSendJobInput] PRIMARY KEY CLUSTERED 
(
	[DigitalSendJobInputId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendJobNotification]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobNotification]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendJobNotification](
	[DigitalSendJobNotificationId] [uniqueidentifier] NOT NULL,
	[FilePrefix] [nvarchar](255) NULL,
	[SessionId] [varchar](50) NOT NULL,
	[NotificationDestination] [nvarchar](255) NULL,
	[NotificationReceivedDateTime] [datetime] NULL,
	[NotificationResult] [nvarchar](50) NULL,
 CONSTRAINT [PK_DigitalSendJobNotification] PRIMARY KEY CLUSTERED 
(
	[DigitalSendJobNotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendJobOutput]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobOutput]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendJobOutput](
	[DigitalSendJobOutputId] [uniqueidentifier] NOT NULL,
	[FilePrefix] [nvarchar](255) NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FileName] [nvarchar](255) NULL,
	[FileLocation] [nvarchar](255) NULL,
	[FileSentDateTime] [datetime] NULL,
	[FileReceivedDateTime] [datetime] NULL,
	[FileSizeBytes] [bigint] NULL,
	[PageCount] [smallint] NULL,
	[ErrorMessage] [nvarchar](1024) NULL,
 CONSTRAINT [PK_DigitalSendJobOutput] PRIMARY KEY CLUSTERED 
(
	[DigitalSendJobOutputId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendJobTempFile]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobTempFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendJobTempFile](
	[DigitalSendJobTempFileId] [uniqueidentifier] NOT NULL,
	[DigitalSendJobId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FirstFileInDateTime] [datetime] NULL,
	[RenameDateTime] [datetime] NULL,
	[LastFileOutDateTime] [datetime] NULL,
	[TotalFiles] [int] NULL,
	[TotalBytes] [bigint] NULL,
 CONSTRAINT [PK_DigitalSendJobTempFile] PRIMARY KEY CLUSTERED 
(
	[DigitalSendJobTempFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendServerJob]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendServerJob]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendServerJob](
	[DigitalSendServerJobId] [uniqueidentifier] NOT NULL,
	[DigitalSendJobId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[JobType] [nvarchar](50) NULL,
	[CompletionDateTime] [datetime] NULL,
	[CompletionStatus] [nvarchar](50) NULL,
	[FileType] [nvarchar](50) NULL,
	[FileName] [nvarchar](255) NULL,
	[FileSizeBytes] [bigint] NULL,
	[ScannedPages] [smallint] NULL,
	[DeviceModel] [nvarchar](50) NULL,
	[DssVersion] [nvarchar](50) NULL,
	[ProcessedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_DigitalSendServerJob] PRIMARY KEY CLUSTERED 
(
	[DigitalSendServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DigitalSendTempSnapshot]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendTempSnapshot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DigitalSendTempSnapshot](
	[DigitalSendTempSnapshotId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[HostName] [nvarchar](50) NULL,
	[SnapshotDateTime] [datetime] NULL,
	[TotalBytes] [bigint] NULL,
	[TotalFiles] [bigint] NULL,
	[TotalJobs] [bigint] NULL,
 CONSTRAINT [PK_DigitalSendTempSnapshot] PRIMARY KEY CLUSTERED 
(
	[DigitalSendTempSnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DirectorySnapshot]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DirectorySnapshot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DirectorySnapshot](
	[DirectorySnapshotId] [uniqueidentifier] NOT NULL,
	[SnapshotDateTime] [datetime] NULL,
	[HostName] [nvarchar](50) NULL,
	[DirectoryName] [nvarchar](1024) NULL,
	[TotalFiles] [int] NULL,
	[TotalBytes] [bigint] NULL,
 CONSTRAINT [PK_DirectorySnapshot] PRIMARY KEY CLUSTERED 
(
	[DirectorySnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[EPrintServerJob]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EPrintServerJob]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EPrintServerJob](
	[EPrintServerJobId] [bigint] NOT NULL,
	[EPrintJobId] [int] NULL,
	[PrintJobClientId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[EmailAccount] [nvarchar](50) NULL,
	[PrinterName] [nvarchar](50) NULL,
	[JobName] [nvarchar](255) NULL,
	[JobFolderId] [nvarchar](255) NULL,
	[EmailReceivedDateTime] [datetime] NULL,
	[JobStartDateTime] [datetime] NULL,
	[LastStatusDateTime] [datetime] NULL,
	[JobStatus] [nvarchar](50) NULL,
	[EPrintTransactionId] [int] NULL,
	[TransactionStatus] [nvarchar](50) NULL,
 CONSTRAINT [PK_EPrintServerJob] PRIMARY KEY CLUSTERED 
(
	[EPrintServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[JetAdvantageLinkDeviceMemoryCount]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemoryCount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JetAdvantageLinkDeviceMemoryCount](
	[JetAdvantageLinkDeviceMemoryCountId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[JetAdvantageLinkMemorySnapshotId] [uniqueidentifier] NULL,
	[CategoryName] [nvarchar](50) NULL,
	[DataLabel] [nvarchar](255) NULL,
	[DataValue] [bigint] NULL,
 CONSTRAINT [PK_JetAdvantageLinkDeviceMemoryCount] PRIMARY KEY CLUSTERED 
(
	[JetAdvantageLinkDeviceMemoryCountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[JetAdvantageLinkDeviceMemorySnapshot]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemorySnapshot]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JetAdvantageLinkDeviceMemorySnapshot](
	[JetAdvantageLinkDeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[DeviceId] [nvarchar](50) NULL,
	[SnapshotLabel] [nvarchar](255) NULL,
	[UsageCount] [int] NULL,
	[SnapshotDateTime] [datetime] NULL,
 CONSTRAINT [PK_JetAdvantageLinkDeviceMemorySnapshot] PRIMARY KEY CLUSTERED 
(
	[JetAdvantageLinkDeviceMemorySnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[JetAdvantagePullPrintJobRetrieval]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JetAdvantagePullPrintJobRetrieval]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JetAdvantagePullPrintJobRetrieval](
	[JetAdvantagePullPrintJobRetrievalId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[JetAdvantageLoginId] [nvarchar](50) NULL,
	[DeviceId] [nvarchar](50) NULL,
	[SolutionType] [nvarchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [nvarchar](50) NULL,
	[ErrorMessage] [nvarchar](1024) NULL,
 CONSTRAINT [PK_JetAdvantagePullPrintJobRetrieval] PRIMARY KEY CLUSTERED 
(
	[JetAdvantagePullPrintJobRetrievalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[JetAdvantageUpload]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JetAdvantageUpload]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JetAdvantageUpload](
	[JetAdvantageUploadId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[LoginId] [nvarchar](50) NULL,
	[DestinationUrl] [nvarchar](255) NULL,
	[FileName] [nvarchar](255) NULL,
	[FileType] [nvarchar](50) NULL,
	[FileSentDateTime] [datetime] NULL,
	[FileSizeSentBytes] [bigint] NULL,
	[FileReceivedDateTime] [datetime] NULL,
	[FileSizeReceivedBytes] [bigint] NULL,
	[CompletionDateTime] [datetime] NULL,
	[CompletionStatus] [nvarchar](50) NULL,
 CONSTRAINT [PK_JetAdvantageUpload] PRIMARY KEY CLUSTERED 
(
	[JetAdvantageUploadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PhysicalDeviceJob]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhysicalDeviceJob]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhysicalDeviceJob](
	[PhysicalDeviceJobId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[DeviceId] [nvarchar](50) NULL,
	[JobId] [nvarchar](50) NULL,
	[JobName] [nvarchar](255) NULL,
	[JobApplicationName] [nvarchar](255) NULL,
	[JobCategory] [nvarchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [nvarchar](50) NULL,
	[MonitorStartDateTime] [datetime] NULL,
	[MonitorEndDateTime] [datetime] NULL,
 CONSTRAINT [PK_PhysicalDeviceJob] PRIMARY KEY CLUSTERED 
(
	[PhysicalDeviceJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintJobClient]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintJobClient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintJobClient](
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FileName] [nvarchar](255) NULL,
	[FileSizeBytes] [bigint] NULL,
	[ClientOS] [nvarchar](50) NULL,
	[PrintQueue] [nvarchar](255) NULL,
	[PrintType] [nvarchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[PrintStartDateTime] [datetime] NULL,
 CONSTRAINT [PK_PrintJobClient] PRIMARY KEY CLUSTERED 
(
	[PrintJobClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintServerJob]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintServerJob]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintServerJob](
	[PrintServerJobId] [uniqueidentifier] NOT NULL,
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[PrintServer] [nvarchar](50) NULL,
	[PrintServerOS] [nvarchar](50) NULL,
	[PrintQueue] [nvarchar](255) NULL,
	[PrintDriver] [nvarchar](255) NULL,
	[DataType] [varchar](20) NULL,
	[SubmittedDateTime] [datetime] NULL,
	[SpoolStartDateTime] [datetime] NULL,
	[SpoolEndDateTime] [datetime] NULL,
	[PrintStartDateTime] [datetime] NULL,
	[PrintEndDateTime] [datetime] NULL,
	[JobTotalPages] [smallint] NULL,
	[JobTotalBytes] [bigint] NULL,
	[PrintedPages] [smallint] NULL,
	[PrintedBytes] [bigint] NULL,
	[RenderOnClient] [bit] NULL,
	[ColorMode] [varchar](10) NULL,
	[Copies] [smallint] NULL,
	[NumberUp] [smallint] NULL,
	[Duplex] [varchar](20) NULL,
	[EndStatus] [varchar](255) NULL,
 CONSTRAINT [PK_PrintServerJob] PRIMARY KEY CLUSTERED 
(
	[PrintServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ProductDetails]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductDetails](
	[Product] [varchar](50) NOT NULL,
	[Model] [varchar](255) NULL,
	[Platform] [varchar](50) NULL,
	[Group] [varchar](50) NULL,
	[Color] [bit] NULL,
	[MaxMediaSize] [char](5) NULL,
	[Function] [varchar](10) NULL,
	[DeviceColor]  AS (case [Color] when (1) then 'Color' when (0) then 'Mono' else '' end),
 CONSTRAINT [PK_ProductDetails] PRIMARY KEY CLUSTERED 
(
	[Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PullPrintJobRetrieval]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PullPrintJobRetrieval]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PullPrintJobRetrieval](
	[PullPrintJobRetrievalId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[DeviceId] [nvarchar](50) NULL,
	[SolutionType] [nvarchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [nvarchar](50) NULL,
	[InitialJobCount] [smallint] NULL,
	[FinalJobCount] [smallint] NULL,
	[NumberOfCopies] [smallint] NULL,
 CONSTRAINT [PK_PullPrintJobRetrieval] PRIMARY KEY CLUSTERED 
(
	[PullPrintJobRetrievalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionDevice]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionDevice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionDevice](
	[SessionDeviceId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[DeviceId] [nvarchar](50) NOT NULL,
	[ProductName] [nvarchar](255) NULL,
	[DeviceName] [nvarchar](255) NULL,
	[FirmwareRevision] [nvarchar](50) NULL,
	[FirmwareDatecode] [nvarchar](20) NULL,
	[FirmwareType] [nvarchar](50) NULL,
	[ModelNumber] [nvarchar](50) NULL,
	[NetworkCardModel] [nvarchar](50) NULL,
	[NetworkInterfaceVersion] [nvarchar](255) NULL,
	[IpAddress] [nvarchar](50) NULL,
	[FirmwareBundleVersion] [nvarchar](50) NULL,
 CONSTRAINT [PK_SessionDevice] PRIMARY KEY CLUSTERED 
(
	[SessionDeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionDocument]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionDocument]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionDocument](
	[SessionDocumentId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](255) NULL,
	[Extension] [nvarchar](10) NULL,
	[FileType] [nvarchar](50) NULL,
	[FileSizeKilobytes] [bigint] NULL,
	[Pages] [smallint] NULL,
	[ColorMode] [varchar](10) NULL,
	[Orientation] [varchar](10) NULL,
	[DefectId] [nvarchar](50) NULL,
	[Tag] [nvarchar](255) NULL,
 CONSTRAINT [PK_SessionDocument] PRIMARY KEY CLUSTERED 
(
	[SessionDocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionProductAssoc]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionProductAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionProductAssoc](
	[SessionId] [varchar](50) NOT NULL,
	[EnterpriseTestAssociatedProductId] [uniqueidentifier] NOT NULL,
	[Version] [nvarchar](1024) NULL,
	[Name] [nvarchar](50) NULL,
	[Vendor] [nvarchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SessionProductAssoc] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[EnterpriseTestAssociatedProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionScenario]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionScenario]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionScenario](
	[SessionId] [varchar](50) NOT NULL,
	[ConfigurationData] [xml] NOT NULL,
	[RunOrder] [tinyint] NOT NULL,
	[ScenarioStart] [datetime] NULL,
	[ScenarioEnd] [datetime] NULL,
 CONSTRAINT [PK_SessionScenario] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[RunOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionServer]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionServer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionServer](
	[SessionServerId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ServerId] [uniqueidentifier] NOT NULL,
	[HostName] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[OperatingSystem] [nvarchar](50) NULL,
	[Architecture] [varchar](10) NULL,
	[Processors] [smallint] NULL,
	[Cores] [smallint] NULL,
	[Memory] [int] NULL,
 CONSTRAINT [PK_SessionServer] PRIMARY KEY CLUSTERED 
(
	[SessionServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SessionSummary]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionSummary](
	[SessionId] [varchar](50) NOT NULL,
	[SessionName] [nvarchar](255) NULL,
	[Owner] [nvarchar](50) NULL,
	[Dispatcher] [nvarchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Status] [varchar](20) NULL,
	[Tags] [nvarchar](255) NULL,
	[STFVersion] [nvarchar](50) NULL,
	[Type] [nvarchar](255) NULL,
	[Cycle] [nvarchar](255) NULL,
	[Reference] [nvarchar](255) NULL,
	[Notes] [nvarchar](max) NULL,
	[ShutdownState] [varchar](20) NULL,
	[ProjectedEndDateTime] [datetime] NULL,
	[ExpirationDateTime] [datetime] NULL,
	[ShutdownUser] [nvarchar](50) NULL,
	[ShutdownDateTime] [datetime] NULL,
 CONSTRAINT [PK_SessionSummary] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[TriageData]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TriageData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TriageData](
	[TriageDataId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[ControlIds] [nvarchar](max) NULL,
	[ControlPanelImage] [varbinary](max) NULL,
	[Reason] [nvarchar](max) NULL,
	[Thumbnail] [varbinary](max) NULL,
	[TriageDateTime] [datetime] NULL,
	[DeviceWarnings] [nvarchar](max) NULL,
	[UIDumpData] [nvarchar](max) NULL,
 CONSTRAINT [PK_TriageData] PRIMARY KEY CLUSTERED 
(
	[TriageDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[TriageDataJetAdvantageLink]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TriageDataJetAdvantageLink]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TriageDataJetAdvantageLink](
	[TriageDataJetAdvantageLinkId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[UIDumpData] [nvarchar](max) NULL,
 CONSTRAINT [PK_TriageDataJetAdvantageLink] PRIMARY KEY CLUSTERED 
(
	[TriageDataJetAdvantageLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualPrinterJob]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualPrinterJob]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualPrinterJob](
	[VirtualPrinterJobId] [uniqueidentifier] NOT NULL,
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[PjlJobName] [nvarchar](255) NULL,
	[PjlLanguage] [nvarchar](50) NULL,
	[FirstByteReceivedDateTime] [datetime] NULL,
	[LastByteReceivedDateTime] [datetime] NULL,
	[BytesReceived] [bigint] NULL,
 CONSTRAINT [PK_VirtualPrinterJob] PRIMARY KEY CLUSTERED 
(
	[VirtualPrinterJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceInstanceStatus]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceInstanceStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceInstanceStatus](
	[VirtualResourceStatusId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Index] [int] NULL,
	[TransitionTo] [nvarchar](50) NULL,
	[TransitionActive] [bit] NULL,
	[Caller] [nvarchar](50) NULL,
	[ResourceInstanceId] [nchar](50) NULL,
 CONSTRAINT [PK_VirtualResourceInstanceStatus] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/*****************************************************************************
 * INDEXES                                                                   *
 *****************************************************************************/

/******  Object:  Index [Idx_ActivityExecution_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecution]') AND name = N'Idx_ActivityExecution_SessionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_SessionId] ON [dbo].[ActivityExecution]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityName],
	[ActivityType],
	[UserName],
	[HostName],
	[StartDateTime],
	[EndDateTime],
	[Status],
	[ResultMessage],
	[ResultCategory]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecution_SessionId_ActivityExecutionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecution]') AND name = N'Idx_ActivityExecution_SessionId_ActivityExecutionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_SessionId_ActivityExecutionId] ON [dbo].[ActivityExecution]
(
	[SessionId] ASC,
	[ActivityExecutionId] ASC
)
INCLUDE ( 	[ActivityName],
	[ActivityType],
	[EndDateTime],
	[HostName],
	[ResultMessage],
	[StartDateTime],
	[Status],
	[UserName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecution_StartDateTime]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecution]') AND name = N'Idx_ActivityExecution_StartDateTime')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_StartDateTime] ON [dbo].[ActivityExecution]
(
	[StartDateTime] ASC
)
INCLUDE ( 	[SessionId],
	[ActivityType],
	[EndDateTime],
	[Status]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionAssetUsage_ActivityExecutionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionAssetUsage]') AND name = N'Idx_ActivityExecutionAssetUsage_ActivityExecutionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionAssetUsage_ActivityExecutionId] ON [dbo].[ActivityExecutionAssetUsage]
(
	[ActivityExecutionId] ASC
)
INCLUDE ( 	[SessionId],
	[AssetId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionAssetUsage_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionAssetUsage]') AND name = N'Idx_ActivityExecutionAssetUsage_SessionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionAssetUsage_SessionId] ON [dbo].[ActivityExecutionAssetUsage]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[AssetId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionDetail_ActivityExecutionId_Label]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDetail]') AND name = N'Idx_ActivityExecutionDetail_ActivityExecutionId_Label')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionDetail_ActivityExecutionId_Label] ON [dbo].[ActivityExecutionDetail]
(
	[ActivityExecutionId] ASC,
	[Label] ASC
)
INCLUDE ( 	[Message]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionDetail_SessionId_Label]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDetail]') AND name = N'Idx_ActivityExecutionDetail_SessionId_Label')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionDetail_SessionId_Label] ON [dbo].[ActivityExecutionDetail]
(
	[SessionId] ASC,
	[Label] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[Message]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionPerformance_ActivityExecutionId_EventLabel]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]') AND name = N'Idx_ActivityExecutionPerformance_ActivityExecutionId_EventLabel')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_ActivityExecutionId_EventLabel] ON [dbo].[ActivityExecutionPerformance]
(
	[ActivityExecutionId] ASC,
	[EventLabel] ASC
)
INCLUDE ( 	[SessionId],
	[EventIndex],
	[EventDateTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionPerformance_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]') AND name = N'Idx_ActivityExecutionPerformance_SessionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_SessionId] ON [dbo].[ActivityExecutionPerformance]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[EventLabel],
	[EventIndex],
	[EventDateTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionPerformance_SessionId_ActivityExecutionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]') AND name = N'Idx_ActivityExecutionPerformance_SessionId_ActivityExecutionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionPerformance_SessionId_ActivityExecutionId] ON [dbo].[ActivityExecutionPerformance]
(
	[SessionId] ASC,
	[ActivityExecutionId] ASC
)
INCLUDE ( 	[EventLabel],
	[EventDateTime],
	[EventIndex]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionRetries_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionRetries]') AND name = N'Idx_ActivityExecutionRetries_SessionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionRetries_SessionId] ON [dbo].[ActivityExecutionRetries]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_ActivityExecutionServerUsage_ActivityExecutionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecutionServerUsage]') AND name = N'Idx_ActivityExecutionServerUsage_ActivityExecutionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecutionServerUsage_ActivityExecutionId] ON [dbo].[ActivityExecutionServerUsage]
(
	[ActivityExecutionId] ASC
)
INCLUDE ( 	[ServerName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DeviceMemoryCount_CategoryName_DataLabel]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemoryCount]') AND name = N'Idx_DeviceMemoryCount_CategoryName_DataLabel')
CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCount_CategoryName_DataLabel] ON [dbo].[DeviceMemoryCount]
(
	[DataLabel] ASC,
	[CategoryName] ASC
)
INCLUDE ( 	[DeviceMemorySnapshotId],
	[DataValue]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DeviceMemoryCount_DeviceMemorySnapshotId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemoryCount]') AND name = N'Idx_DeviceMemoryCount_DeviceMemorySnapshotId')
CREATE NONCLUSTERED INDEX [Idx_DeviceMemoryCount_DeviceMemorySnapshotId] ON [dbo].[DeviceMemoryCount]
(
	[DeviceMemorySnapshotId] ASC
)
INCLUDE ( 	[CategoryName],
	[DataValue]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DeviceMemorySnapshot_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemorySnapshot]') AND name = N'Idx_DeviceMemorySnapshot_SessionId')
CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshot_SessionId] ON [dbo].[DeviceMemorySnapshot]
(
	[SessionId] ASC
)
INCLUDE ( 	[DeviceId],
	[SnapshotLabel],
	[SnapshotDateTime],
	[UsageCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DeviceMemorySnapshot_SessionId_DeviceId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemorySnapshot]') AND name = N'Idx_DeviceMemorySnapshot_SessionId_DeviceId')
CREATE NONCLUSTERED INDEX [Idx_DeviceMemorySnapshot_SessionId_DeviceId] ON [dbo].[DeviceMemorySnapshot]
(
	[SessionId] ASC,
	[DeviceId] ASC
)
INCLUDE ( 	[SnapshotLabel],
	[UsageCount],
	[SnapshotDateTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DigitalSendJobInput_ActivityExecutionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]') AND name = N'Idx_DigitalSendJobInput_ActivityExecutionId')
CREATE NONCLUSTERED INDEX [Idx_DigitalSendJobInput_ActivityExecutionId] ON [dbo].[DigitalSendJobInput]
(
	[ActivityExecutionId] ASC
)
INCLUDE ( 	[FilePrefix],
	[Ocr]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_DigitalSendJobOutput_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSendJobOutput]') AND name = N'Idx_DigitalSendJobOutput_SessionId')
CREATE NONCLUSTERED INDEX [Idx_DigitalSendJobOutput_SessionId] ON [dbo].[DigitalSendJobOutput]
(
	[SessionId] ASC
)
INCLUDE ( 	[ErrorMessage],
	[FileLocation],
	[FileName],
	[FilePrefix],
	[FileReceivedDateTime],
	[FileSentDateTime],
	[FileSizeBytes],
	[PageCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_PhysicalDeviceJob_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhysicalDeviceJob]') AND name = N'Idx_PhysicalDeviceJob_SessionId')
CREATE NONCLUSTERED INDEX [Idx_PhysicalDeviceJob_SessionId] ON [dbo].[PhysicalDeviceJob]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[JobEndStatus]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_PrintJobClient_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintJobClient]') AND name = N'Idx_PrintJobClient_SessionId')
CREATE NONCLUSTERED INDEX [Idx_PrintJobClient_SessionId] ON [dbo].[PrintJobClient]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[FileName],
	[FileSizeBytes],
	[ClientOS],
	[PrintQueue],
	[JobStartDateTime],
	[JobEndDateTime],
	[PrintStartDateTime]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_PrintServerJob_PrintJobClientId]  ******/
USE [EnterpriseTestODS]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintServerJob]') AND name = N'Idx_PrintServerJob_PrintJobClientId')
CREATE NONCLUSTERED INDEX [Idx_PrintServerJob_PrintJobClientId] ON [dbo].[PrintServerJob]
(
	[PrintJobClientId] ASC
)
INCLUDE ( 	[PrintStartDateTime],
	[PrintedPages]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_PullPrintJobRetrieval_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PullPrintJobRetrieval]') AND name = N'Idx_PullPrintJobRetrieval_SessionId')
CREATE NONCLUSTERED INDEX [Idx_PullPrintJobRetrieval_SessionId] ON [dbo].[PullPrintJobRetrieval]
(
	[SessionId] ASC
)
INCLUDE ( 	[ActivityExecutionId],
	[SolutionType],
	[JobStartDateTime],
	[JobEndDateTime],
	[JobEndStatus],
	[InitialJobCount],
	[FinalJobCount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_SessionDevice_SessionId_DeviceId_ProductName]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SessionDevice]') AND name = N'Idx_SessionDevice_SessionId_DeviceId_ProductName')
CREATE NONCLUSTERED INDEX [Idx_SessionDevice_SessionId_DeviceId_ProductName] ON [dbo].[SessionDevice]
(
	[SessionId] ASC,
	[DeviceId] ASC,
	[ProductName] ASC
)
INCLUDE ( 	[FirmwareRevision],
	[IpAddress]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/******  Object:  Index [Idx_SessionDocument_SessionId]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SessionDocument]') AND name = N'Idx_SessionDocument_SessionId')
CREATE NONCLUSTERED INDEX [Idx_SessionDocument_SessionId] ON [dbo].[SessionDocument]
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/******  Object:  Foreign Key [FK_VirtualResourceInstanceStatus_SessionSummary]  ******/
USE [EnterpriseTestODS]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceInstanceStatus_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceInstanceStatus]'))
ALTER TABLE [dbo].[VirtualResourceInstanceStatus]  WITH NOCHECK ADD  CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceInstanceStatus_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceInstanceStatus]'))
ALTER TABLE [dbo].[VirtualResourceInstanceStatus] CHECK CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary]
GO

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/******  Object:  StoredProcedure [dbo].[sel_AuthenticationTimesCnts]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_AuthenticationTimesCnts]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sel_AuthenticationTimesCnts] AS' 
END
GO

-- =============================================
-- Author:		Don Anderson
-- Create date: 10/26/2018
-- Description:	Generates a listing of eager and lazy authentication times
-- =============================================
ALTER PROCEDURE [dbo].[sel_AuthenticationTimesCnts] 
	-- Add the parameters for the stored procedure here
	@startDateTime datetime = null 
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if object_id('tempdb..##tempTable') is not null
	drop table ##tempTable


create table ##tempTable
(
	ID int identity (1,1) primary key,
	ProductName varchar(50),
	FirmwareRevision varchar(50),
	FirmwareDatecode varchar(50),
	SolutionAuthType nvarchar(2048),
	EagerAuth decimal(10,3),
	EagerAuthMin decimal(10,3),
	EagerAuthMax decimal(10,3),
	EagerAuthCount int,
	LazyAuth decimal(10,3) null,
	LazyAuthMin decimal(10,3) null,
	LazyAuthMax decimal(10,3) null,
	LazyAuthCount int null,
	TotalAuthCount integer null
)
insert into ##tempTable(ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, EagerAuth, EagerAuthMin, EagerAuthMax, EagerAuthCount)
	select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, EagerAuth, EagerAuthMin, EagerAuthMax, EagerAuthCount from
	(
		select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, 
		format(round(CAST(AVG(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuth, 
		format(round(CAST(MAX(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuthMax, 
		format(round(CAST(MIN(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuthMin, 
		count(SolutionAuthType) as EagerAuthCount  
		from 
		(
			select StartDateTime, ProductName, FirmwareRevision, FirmwareDatecode, [DeviceButtonPress_Sign In], [EnterCredentialsEnd], [AuthenticationEnd], SolutionAuthType, 
			AuthTime  = 
				(CASE
					When SolutionAuthType NOT LIKE 'Proximity Card%' 
						then DATEDIFF(MILLISECOND, [EnterCredentialsEnd],[AuthenticationEnd])
					Else DATEDIFF(MILLISECOND, [DeviceButtonPress_Sign In],[AuthenticationEnd])
					END
				)
				from 
				(
					select * from 
					(
						select StartDateTime, sd.ProductName, sd.FirmwareRevision, sd.FirmwareDatecode, aeau.AssetId, EventLabel, EventDateTime,
						SolutionAuthType = STUFF((
							Select  ' - '  + [Message] 
								From ActivityExecutionDetail as aed2 with (nolock) 
								Where aed2.Label in ('AuthType', 'AppButtonPress') and
									  ActivityExecutionId = aed.ActivityExecutionId
								Order by [Message]
								FOR XML PATH('')), 1, 3, '')
						from ActivityExecutionPerformance aep
							inner join ActivityExecutionAssetUsage aeau on aep.ActivityExecutionId = aeau.ActivityExecutionId 
							inner join SessionDevice sd on aep.SessionId = sd.SessionId and aeau.AssetId = sd.DeviceId
							inner join ActivityExecution ae on aep.ActivityExecutionId = ae.ActivityExecutionId
							inner join ActivityExecutionDetail aed on aep.ActivityExecutionId = aed.ActivityExecutionId
								where	EventLabel in ('DeviceButtonPress_Sign In', 'EnterCredentialsBegin', 'EnterCredentialsEnd', 'AuthenticationEnd') and										
										aed.Label in ('AuthType', 'AppButtonPress') AND
										ae.StartDateTime > @startDateTime 
								
			
					) as scr 

					PIVOT
					(
						max(EventDateTime) for EventLabel in ([DeviceButtonPress_Sign In],[EnterCredentialsBegin], [EnterCredentialsEnd], [AuthenticationEnd]) 
					) as p

				) as d


		) as x where x.[DeviceButtonPress_Sign In] is not null
		group by  ProductName, FirmwareDatecode, FirmwareRevision, SolutionAuthType

	) as da
	where da.EagerAuth is NOT null 
	AND da.FirmwareRevision is NOT null

if object_id('tempdb..##tempTableLazy') is not null
	drop table ##tempTableLazy


create table ##tempTableLazy
(
	ID int identity (1,1) primary key,
	ProductName varchar(50),
	FirmwareRevision varchar(50),
	FirmwareDatecode varchar(50),
	SolutionAuthType nvarchar(2048),
	LazyAuth decimal(10,3),
	LazyAuthMin decimal(10,3),
	LazyAuthMax decimal(10,3),
	LazyAuthCount int
)



insert into ##tempTableLazy
select * from
(
	select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, 
	format(round(CAST(AVG(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuth, 
	format(round(CAST(Min(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuthMin, 
	format(round(CAST(Max(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuthMax, 
	count(SolutionAuthType) as LazyAuthCount  
	from 
	(
		select StartDateTime, ProductName, FirmwareRevision, FirmwareDatecode, [EventLabelLazy], [EnterCredentialsEnd], [AuthenticationEnd], SolutionAuthType, 
				DATEDIFF(MILLISECOND, [EnterCredentialsEnd],[AuthenticationEnd]) as AuthTime from
		(
			select * from 
			(	
				select StartDateTime, sd.ProductName, sd.FirmwareRevision, sd.FirmwareDatecode, EventDateTime, 
				SolutionAuthType= STUFF((
							Select ' - '  + Message   
								From ActivityExecutionDetail as aed2 with(nolock) 
								Where aed2.Label in ( 'AppButtonPress', 'AuthType') and
									  ActivityExecutionId = aed.ActivityExecutionId
									  Order by Message
										FOR XML PATH('')), 1, 3, '')
				,
				case
					when (EventLabel like 'AppButtonPress_%' AND EventIndex = 2) Then 'EventLabelLazy'
					when EventLabel = 'DeviceButtonPress_Sign In' then NULL
					else EventLabel
					end as EventLabel					 
				from ActivityExecutionPerformance aep
					inner join ActivityExecutionAssetUsage aeau on aep.ActivityExecutionId = aeau.ActivityExecutionId 
					inner join SessionDevice sd on aep.SessionId = sd.SessionId and aeau.AssetId = sd.DeviceId
					inner join ActivityExecution ae on aep.ActivityExecutionId = ae.ActivityExecutionId
					inner join ActivityExecutionDetail aed on aep.ActivityExecutionId = aed.ActivityExecutionId
						where (EventLabel = 'EnterCredentialsEnd' or EventLabel = 'AuthenticationEnd'  OR (EventLabel like 'AppButtonPress_%' AND EventIndex = 2) ) 
						and aed.Label in ('AuthType', 'AppButtonPress')
						and ae.StartDateTime > @startDateTime	

			) as SRC

			PIVOT
			(
				max(EventDateTime) for EventLabel in ([EventLabelLazy], [EnterCredentialsEnd], [AuthenticationEnd]) 
			) as p

		) as d
	) as x
		where x.EventLabelLazy is not null
		group by  ProductName, FirmwareDatecode, FirmwareRevision, SolutionAuthType
) as lazy
where Lazy.LazyAuth is not null

update ##tempTable 
SET ##tempTable.LazyAuth = lazy.LazyAuth,
	##tempTable.LazyAuthMin = lazy.LazyAuthMin,
	##tempTable.LazyAuthMax = lazy.LazyAuthMax,
	##tempTable.LazyAuthCount = lazy.LazyAuthCount
from ##tempTable as eager
inner join ##tempTableLazy lazy  on eager.ProductName = lazy.ProductName
	where eager.FirmwareRevision = lazy.FirmwareRevision 
		and eager.FirmwareDatecode = lazy.FirmwareDatecode  
		and eager.SolutionAuthType = lazy.SolutionAuthType



insert INTO ##tempTable 
	(ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, LazyAuth, LazyAuthMin, LazyAuthMax, LazyAuthCount)
	select l.ProductName, l.FirmwareRevision, l.FirmwareDatecode, l.SolutionAuthType, l.LazyAuth, l.LazyAuthMin, l.LazyAuthMax, l.LazyAuthCount 
		FROM ##tempTableLazy as l
			Left join ##tempTable as e
			on e.FirmwareRevision = l.FirmwareRevision and e.FirmwareDatecode = l.FirmwareDatecode
				where e.ID is null
				order by l.ProductName, l.SolutionAuthType, l.FirmwareDatecode


update ##tempTable
set TotalAuthCount = isnull(EagerAuthCount, 0) + isnull(LazyAuthCount, 0),
	EagerAuth = ISNULL(EagerAuth, 0),
	EagerAuthMin = ISNULL(EagerAuthMin, 0),
	EagerAuthMax = ISNULL(EagerAuthMax, 0),
	EagerAuthCount = ISNULL(EagerAuthCount, 0),
	LazyAuth = ISNULL(LazyAuth, 0),
	LazyAuthMin = ISNULL(LazyAuthMin, 0),
	LazyAuthMax = ISNULL(LazyAuthMax, 0),
	LazyAuthCount = ISNULL(LazyAuthCount, 0)
		where ID = ID


select * from ##tempTable
where (EagerAuth > 0 OR LazyAuth > 0)
order by  ProductName, SolutionAuthType, FirmwareDatecode 

END
GO

/******  Object:  StoredProcedure [dbo].[sel_AuthenticationTimesEager]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_AuthenticationTimesEager]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sel_AuthenticationTimesEager] AS' 
END
GO
-- =============================================
-- Author:		Don Anderson
-- Create date: 01/25/2019
-- Description:	Shows the eager authentication times greater than the given date
-- =============================================
ALTER PROCEDURE [dbo].[sel_AuthenticationTimesEager] 
	-- Add the parameters for the stored procedure here
	@startDateTime datetime = null
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if object_id('tempdb..##tempTableEager') is not null
	drop table ##tempTableEager


create table ##tempTableEager
(
	ID int identity (1,1) primary key,
	ProductName varchar(50),
	FirmwareRevision varchar(50),
	FirmwareDatecode varchar(50),
	SolutionAuthType nvarchar(2048),
	EagerAuth decimal(10,3),
	EagerAuthMin decimal(10,3),
	EagerAuthMax decimal(10,3),
	EagerAuthCount int,
	SessionIds varchar(MAX)
)
insert into ##tempTableEager(ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, EagerAuth,EagerAuthMin, EagerAuthMax, EagerAuthCount)
	select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, EagerAuth, EagerAuthMin, EagerAuthMax, EagerAuthCount from
	(
		select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, 
		format(round(CAST(AVG(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuth, 
		format(round(CAST(MAX(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuthMax, 
		format(round(CAST(MIN(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as EagerAuthMin, 
		count(SolutionAuthType) as EagerAuthCount  
		from 
		(
			select StartDateTime, ProductName, FirmwareRevision, FirmwareDatecode, [DeviceButtonPress_Sign In], [EnterCredentialsEnd], [AuthenticationEnd], SolutionAuthType, 
			AuthTime  = 
				(CASE
					When SolutionAuthType NOT LIKE 'Proximity Card%' 
						then DATEDIFF(MILLISECOND, [EnterCredentialsEnd],[AuthenticationEnd])
					Else DATEDIFF(MILLISECOND, [DeviceButtonPress_Sign In],[AuthenticationEnd])
					END
				)
				from 
				(
					select * from 
					(
						select StartDateTime, sd.ProductName, sd.FirmwareRevision, sd.FirmwareDatecode, aeau.AssetId, EventLabel, EventDateTime,
						SolutionAuthType = STUFF((
							Select ' - '  + [Message]
								From ActivityExecutionDetail as aed2 with (nolock) 
								Where aed2.Label in ('AuthType', 'AppButtonPress')
									AND ActivityExecutionId = aed.ActivityExecutionId
								ORDER BY [Message]
								FOR XML PATH('')), 1, 3, '')
						from ActivityExecutionPerformance aep with (nolock)
							inner join ActivityExecutionAssetUsage aeau with (nolock) on aep.ActivityExecutionId = aeau.ActivityExecutionId 
							inner join SessionDevice sd with (nolock) on aep.SessionId = sd.SessionId and aeau.AssetId = sd.DeviceId
							inner join ActivityExecution ae with (nolock) on aep.ActivityExecutionId = ae.ActivityExecutionId
							inner join ActivityExecutionDetail aed with (nolock) on aep.ActivityExecutionId = aed.ActivityExecutionId
								where	EventLabel in ('DeviceButtonPress_Sign In', 'EnterCredentialsBegin', 'EnterCredentialsEnd', 'AuthenticationEnd') and										
										aed.Label in ('AuthType', 'AppButtonPress') AND
										ae.StartDateTime > @startDateTime 
								
			
					) as scr 

					PIVOT
					(
						max(EventDateTime) for EventLabel in ([DeviceButtonPress_Sign In],[EnterCredentialsBegin], [EnterCredentialsEnd], [AuthenticationEnd]) 
					) as p

				) as d


		) as x where x.[DeviceButtonPress_Sign In] is not null
		group by  ProductName, FirmwareDatecode, FirmwareRevision, SolutionAuthType

	) as da
	where da.EagerAuth is NOT null 
	AND da.FirmwareRevision is NOT null
	order by ProductName, FirmwareDatecode, SolutionAuthType

		update ##tempTableEager
	set SessionIds =  STUFF((
		select distinct ', ' + sd.SessionId 
		from SessionDevice sd with (nolock)
			inner join ActivityExecution ae with (nolock) on sd.SessionId = ae.SessionId
			inner join ActivityExecutionDetail aed with (nolock) on ae.SessionId = aed.SessionId
		where ae.StartDateTime > @startDateTime
			and ##tempTableEager.FirmwareRevision = sd.FirmwareRevision
			and aed.Label = 'AuthType'
			and aed.Message = ##tempTableEager.SolutionAuthType
		FOR XML PATH('')), 1,2, '')

	update ##tempTableEager
	set SessionIds =  STUFF((
		select distinct ', ' + sd.SessionId 
		from SessionDevice sd with (nolock)
			inner join ActivityExecution ae with (nolock) on sd.SessionId = ae.SessionId
			inner join ActivityExecutionDetail aed with (nolock) on ae.SessionId = aed.SessionId
		where ae.StartDateTime > @startDateTime
			and ##tempTableEager.FirmwareRevision = sd.FirmwareRevision
			and aed.Label = 'AuthType'
			and CHARINDEX(aed.Message, ##tempTableEager.SolutionAuthType) = 1 			
		FOR XML PATH('')), 1,2, '')
	where ##tempTableEager.SessionIds is Null


		update ##tempTableEager
	set SessionIds =  STUFF((
		select distinct ', ' + sd.SessionId 
		from SessionDevice sd with (nolock)
			inner join ActivityExecution ae with (nolock) on sd.SessionId = ae.SessionId
			inner join ActivityExecutionDetail aed with (nolock) on ae.SessionId = aed.SessionId
		where ae.StartDateTime > @startDateTime
			and ##tempTableEager.FirmwareRevision = sd.FirmwareRevision
			and aed.Label = 'AuthType'
			and CHARINDEX(aed.Message, ##tempTableEager.SolutionAuthType) > 1 			
		FOR XML PATH('')), 1,2, '')
	where ##tempTableEager.SessionIds is Null

	select * from ##tempTableEager

END
GO

/******  Object:  StoredProcedure [dbo].[sel_AuthenticationTimesLazy]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_AuthenticationTimesLazy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sel_AuthenticationTimesLazy] AS' 
END
GO
-- =============================================
-- Author:		Don Anderson
-- Create date: 01/29/2019
-- Description:	Calculates the Lazy authentication times
-- =============================================
ALTER PROCEDURE [dbo].[sel_AuthenticationTimesLazy] 
	-- Add the parameters for the stored procedure here
	@startDatetime datetime = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if object_id('tempdb..##tempTableLazy') is not null
	drop table ##tempTableLazy


create table ##tempTableLazy
(
	ID int identity (1,1) primary key,
	ProductName varchar(50),
	FirmwareRevision varchar(50),
	FirmwareDatecode varchar(50),
	SolutionAuthType varchar(2048),
	LazyAuth decimal(10,3),
	LazyAuthMin decimal(10,3),
	LazyAuthMax decimal(10,3),
	LazyAuthCount int,
	SessionIds Varchar(MAX)
)

insert into ##tempTableLazy(ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, LazyAuth, LazyAuthMin, LazyAuthMax, LazyAuthCount)
select * from
(	
	select ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, 
	format(round(CAST(AVG(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuth, 
	format(round(CAST(Min(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuthMin, 
	format(round(CAST(Max(AuthTime)  as decimal(10,2)) /1000, 2), 'N3') as LazyAuthMax, 
	count(SolutionAuthType) as LazyAuthCount  
	from 
	(
		select StartDateTime, ProductName, FirmwareRevision, FirmwareDatecode, [EventLabelLazy], [EnterCredentialsEnd], [AuthenticationEnd], SolutionAuthType, 
				DATEDIFF(MILLISECOND, [EnterCredentialsEnd],[AuthenticationEnd]) as AuthTime from
		(
			select * from 
			(	
				select StartDateTime, sd.ProductName, sd.FirmwareRevision, sd.FirmwareDatecode, EventDateTime, 
				SolutionAuthType= STUFF((
							Select ' - '  + [Message]
								From ActivityExecutionDetail as aed2 with(nolock) 
								Where aed2.Label in ('AuthType', 'AppButtonPress') and
									  ActivityExecutionId = aed.ActivityExecutionId
									  ORDER BY [Message]
										FOR XML PATH('')), 1, 3, '')
				,
				case
					when (EventLabel like 'AppButtonPress_%' AND EventIndex = 2) Then 'EventLabelLazy'
					when EventLabel = 'DeviceButtonPress_Sign In' then NULL
					else EventLabel
					end as EventLabel					 
				from ActivityExecutionPerformance aep with(nolock) 
					inner join ActivityExecutionAssetUsage aeau with(nolock)  on aep.ActivityExecutionId = aeau.ActivityExecutionId 
					inner join SessionDevice sd with(nolock) on aep.SessionId = sd.SessionId and aeau.AssetId = sd.DeviceId
					inner join ActivityExecution ae with(nolock)  on aep.ActivityExecutionId = ae.ActivityExecutionId
					inner join ActivityExecutionDetail aed with(nolock)  on aep.ActivityExecutionId = aed.ActivityExecutionId
						where (EventLabel = 'EnterCredentialsEnd' or EventLabel = 'AuthenticationEnd'  OR (EventLabel like 'AppButtonPress_%' AND EventIndex = 2) ) 
						and aed.Label in ('AuthType', 'AppButtonPress')
						and ae.StartDateTime > @startDateTime

			) as SRC

			PIVOT
			(
				max(EventDateTime) for EventLabel in ([EventLabelLazy], [EnterCredentialsEnd], [AuthenticationEnd]) 
			) as p

		) as d
	) as x
		where x.EventLabelLazy is not null
		group by  ProductName, FirmwareDatecode, FirmwareRevision, SolutionAuthType
) as Lazy
where Lazy.LazyAuth is not null


	update ##tempTableLazy
	set SessionIds =  STUFF
	(
		(
			select distinct ', ' + sd.SessionId from SessionDevice sd with(nolock)
				inner join ActivityExecution ae with(nolock) on sd.SessionId = ae.SessionId 
				inner join ActivityExecutionDetail aed with(nolock) on ae.SessionId = aed.SessionId
			where aed.DetailDateTime > @startDateTime
			    and ae.StartDateTime > @startDateTime
				and aed.Label in ('AuthType', 'AppButtonPress') 
				and ##tempTableLazy.FirmwareRevision = sd.FirmwareRevision
				and aed.Message = ##tempTableLazy.SolutionAuthType
				FOR XML PATH('')
		), 1,2, ''
	)

	update ##tempTableLazy
	set SessionIds =  STUFF
	(
		(
			select distinct ', ' + sd.SessionId from SessionDevice sd with(nolock)
				inner join ActivityExecution ae with(nolock) on sd.SessionId = ae.SessionId 
				inner join ActivityExecutionDetail aed with(nolock) on ae.SessionId = aed.SessionId
			where aed.DetailDateTime > @startDateTime
			    and ae.StartDateTime > @startDateTime
				and aed.Label in ('AuthType', 'AppButtonPress') 
				and ##tempTableLazy.FirmwareRevision = sd.FirmwareRevision
				and CHARINDEX(aed.Message, ##tempTableLazy.SolutionAuthType) = 1 
				FOR XML PATH('')
		), 1,2, ''
	)
	where ##tempTableLazy.SessionIds is Null

	update ##tempTableLazy
	set SessionIds =  STUFF
	(
		(
			select distinct ', ' + sd.SessionId from SessionDevice sd with(nolock)
				inner join ActivityExecution ae with(nolock) on sd.SessionId = ae.SessionId 
				inner join ActivityExecutionDetail aed with(nolock) on ae.SessionId = aed.SessionId
			where aed.DetailDateTime > @startDateTime
			    and ae.StartDateTime > @startDateTime
				and aed.Label in ('AuthType', 'AppButtonPress') 
				and ##tempTableLazy.FirmwareRevision = sd.FirmwareRevision
				and CHARINDEX(aed.Message, ##tempTableLazy.SolutionAuthType) > 1 
				FOR XML PATH('')
		), 1,2, ''
	)
	where ##tempTableLazy.SessionIds is Null

select * from ##tempTableLazy
END
GO

/******  Object:  StoredProcedure [Reports].[DigitalSendReport]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[DigitalSendReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [Reports].[DigitalSendReport] AS' 
END
GO
-- =============================================
-- Author:		report_designer
-- Create date: <1/28/2019>
-- Description:	Returns the data used in the Excel based DigitalSendReport
-- =============================================
ALTER PROCEDURE [Reports].[DigitalSendReport]
    @sessionId varchar(50)
AS
BEGIN
	DROP TABLE IF EXISTS #DSActivityPerformance
	
	SET NOCOUNT ON;
	SET ANSI_WARNINGS OFF

	-- Fill the #DSActivityPerformance temp table.
	SELECT	 paep.ActivityExecutionId
			,paep.AppButtonPress AS [App Button Press]
			,paep.PressDeviceSignIn
			,IIF(paep.ScanNumber = 1, Reports.TimeDiffInDays(paep.ActivityBegin, paep.ScanJobBegin), NULL) AS [Activity to Scan Start]
			,CASE
					WHEN AppButtonPress < AuthenticationBegin THEN Reports.TimeDiffInDays (paep.AppButtonPress, paep.AuthenticationBegin) /*  Lazy Auth */
					ELSE Reports.TimeDiffInDays(paep.PressDeviceSignIn, paep.AuthenticationBegin) /*Eager Auth */
			 END AS [Show Auth Screen]
			 ,CASE
					WHEN AppButtonPress < AuthenticationBegin THEN Reports.TimeDiffInDays(paep.EnterCredentialsEnd, paep.AppShown) /*  Lazy Auth */
					ELSE Reports.TimeDiffInDays(paep.AppButtonPress, paep.AppShown) /*Eager Auth */
			  END AS [Show Application]
			,Reports.TimeDiffInDays(paep.ScanJobBegin, paep.ScanJobEnd) AS [Scan Page Time]
			,CASE
					WHEN (paep.JobBuildBegin IS NULL AND paep.ScanNumber = 1)
					THEN	CASE 
								WHEN (paep.ImagePreviewBegin IS NOT NULL) THEN Reports.TimeDiffInDays(paep.ImagePreviewBegin, paep.ImagePreviewEnd)
								ELSE Reports.TimeDiffInDays(paep.ScanJobBegin, paep.ScanJobEnd)
							END
					ELSE Reports.TimeDiffInDays(paep.JobBuildBegin, paep.JobBuildEnd)
			 END AS [Total Scan Time]
			,(SELECT MIN(w) FROM (VALUES (paep.ProcessingJobBegin), (paep.SendingJobBegin)) AS value(w)) AS [ProcessSendingBegin]
			,Reports.TimeDiffInDays((SELECT MIN(w) FROM (VALUES (paep.ProcessingJobBegin), (paep.SendingJobBegin)) AS value(w)), (SELECT MAX(v) FROM (VALUES (paep.ProcessingJobEnd), (paep.SendingJobEnd)) AS value(v))) AS [Sending Time]
			,Reports.TimeDiffInDays(paep.AuthenticationBegin, paep.AuthenticationEnd) AS [Sign into App]
			,IIF(paep.ScanJobBegin IS NULL, NULL, paep.ScanNumber) AS [Scan Number]
			,IIF(paep.ScanJobBegin IS NULL, NULL, COUNT(paep.ScanJobBegin) OVER (PARTITION BY paep.[ActivityExecutionId])) AS [Scanned Pages]
			,IIF(paep.ScanJobBegin IS NULL, NULL, IIF(paep.ImagePreviewBegin IS NULL, 'NoPreview', 'Preview')) AS [Image Preview]
			,MIN(paep.ActivityBegin) OVER (partition by paep.ActivityExecutionId ORDER BY ScanNumber) AS ActivityBegin
			,MAX(paep.ActivityEnd) OVER (partition by paep.ActivityExecutionId ORDER BY ScanNumber) AS ActivityEnd
			,paed.AuthType
			,paed.SignOutType
	INTO #DSActivityPerformance
	FROM (   SELECT SessionId
					,ActivityExecutionId
					,CASE
						WHEN [EventLabel] = 'DeviceButtonPress_Sign In' THEN 'PressDeviceSignIn'
						WHEN LEFT(EventLabel, 15) = 'AppButtonPress_' 	THEN 'AppButtonPress'
						ELSE EventLabel
					 END AS EventLabel
					,EventDateTime
					,CASE
						WHEN EventLabel IN ('ScanJobBegin', 'ScanJobEnd', 'ImagePreviewBegin', 'ImagePreviewEnd') THEN DENSE_RANK() OVER ( PARTITION BY [ActivityExecutionId], [EventLabel] ORDER BY [EventIndex] )
						ELSE 1
					 END AS [ScanNumber]
			FROM ActivityExecutionPerformance WITH (NOLOCK)
			WHERE SessionId IN (@sessionId)) AS saep
	PIVOT(MAX(saep.EventDateTime) FOR saep.EventLabel IN (  PressDeviceSignIn
															,EnterCredentialsBegin
															,EnterCredentialsEnd
															,AppButtonPress
															,AppShown
															,AuthenticationBegin
															,AuthenticationEnd
															,ActivityBegin
															,ActivityEnd
															,ScanJobBegin
															,ScanJobEnd
															,ImagePreviewBegin
															,ImagePreviewEnd
															,JobBuildBegin
															,JobBuildEnd
															,ProcessingJobBegin
															,ProcessingJobEnd
															,SendingJobBegin
															,SendingJobEnd
															,DeviceSignOutBegin
															,DeviceSignOutEnd	)) AS paep
	LEFT JOIN ( --Second pivot. From ActivityExecutionDebug. All has to be done within the JOIN, so requires dual nested
				SELECT * FROM ( SELECT ActivityExecutionId
						,[Label]
						,[Message]
				FROM [ActivityExecutionDetail] WITH (NOLOCK)
				WHERE [SessionId] IN (@sessionId) AND [Label] IN ('AuthType', 'SignOutType')) saed
				PIVOT(MAX(saed.[Message]) FOR saed.[Label] IN ([AuthType], [SignOutType])) AS ipaep
	) AS paed ON paed.[ActivityExecutionId] = paep.[ActivityExecutionId]

	SELECT	 ISNULL(ISNULL(ae.SessionId, dsji.SessionId), dsjo.SessionId) AS SessionId
			,FLOOR((DATEDIFF(MINUTE, aefirst.StartDateTime, ae.StartDateTime)) / 60) + 1 AS DurHour
			,ISNULL(ISNULL(dsap.ActivityBegin, ae.StartDateTime), dsjo.FileSentDateTime) AS [Start Time]
			,ISNULL(dsjo.FileReceivedDateTime, ISNULL(dsap.ActivityEnd, ae.EndDateTime)) AS [End Time]
			,Reports.TimeDiffInDays(ISNULL(ISNULL(dsap.ActivityBegin, ae.StartDateTime), dsjo.FileSentDateTime), ISNULL(dsjo.FileReceivedDateTime, ISNULL(dsap.ActivityEnd, ae.EndDateTime))) AS [Activity Length]
			,ISNULL(dsji.Sender, ae.UserName) as [User Name]
			,ISNULL(dsji.DeviceId, aeau.AssetId) AS [Device Id]
			,sd.ProductName AS Product
			,ISNULL(sd.ProductName, '-') + ' ' + ISNULL(dsji.DeviceId, aeau.AssetId) AS [Device Designation]
			,sd.IpAddress AS [Device IP]
			,ae.ActivityName
			,ae.ActivityType AS [Solution Type]
			,Reports.ActivityGroup(ae.ActivityType) AS [Activity Group]
			,CASE
					WHEN dsjo.[Destination Number] > 1 THEN 'Destination'
					WHEN dsap.[Scan Number] > 1 THEN 'Scan'
					ELSE 'Primary'
			 END AS [Row Type]
			,dsap.AuthType AS [Auth Type]
			,dsap.SignOutType AS [Sign Out Type]
			,ae.Status AS [Update Type]
			,ae.ResultMessage AS [Error Message]
			,ae.ResultCategory AS [Error Group Message]
			,dsji.FilePrefix
			,CASE
					WHEN CHARINDEX('DFS', ae.ActivityName) > 0 THEN 'Folder DFS'
					WHEN ae.ActivityType = 'ScanToFolder' AND dsji.DestinationCount > 1 THEN 'Folder MULTI'
					WHEN dsji.ScanType IS NULL AND ae.ActivityType IN ('HPEC', 'LanFax', 'ScanToEmail', 'ScanToFolder', 'ScanToHpcr', 'ScanToWorkflow', 'AutoStore') THEN 'Not Reached'
					ELSE dsji.ScanType
			END AS [Scan Type]
			,CASE dsji.Ocr
					WHEN 1 THEN 'OCR'
					WHEN 0 THEN 'Non-OCR'
					ELSE NULL
			 END AS OCR
			,dsji.[PageCount] AS [Intended Pages]
			,dsji.DestinationCount AS [Destination Count]
			,dsji.JobEndStatus AS [Scan Job Status]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[App Button Press]) AS [App Button Press]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Activity to Scan Start]) AS [Activity to Scan Start]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Show Auth Screen]) AS [Show Auth Screen]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Show Application]) AS [Show Application]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Scan Page Time]) AS [Scan Page Time]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Total Scan Time]) AS [Total Scan Time]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Sending Time]) AS [Sending Time]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Sign into App]) AS [Sign into App]
			,iif(dsjo.[Destination Number] > 1, NULL, dsap.[Scan Number]) AS [Scan Number]
			,dsap.[Scanned Pages]
			,dsap.[Image Preview]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.FileSentDateTime) AS [File Sent Time]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.FileReceivedDateTime) AS [File Received Time]
			,iif(dsap.[Scan Number] > 1, NULL, Reports.TimeDiffInDays(dsjo.FileSentDateTime, dsjo.FileReceivedDateTime)) AS [File Arrival Duration]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.FileName) AS [Output File Name]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.FileSizeBytes) AS [Output File Size]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.FileLocation) AS [Output File Location]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.[Destination Number]) AS [Destination Number]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.[Destinations Count]) AS [Destinations Count]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.[PageCount]) AS [Destination Pages]
			,iif(dsap.[Scan Number] > 1, NULL, dsjo.[PageCount] - dsji.[PageCount]) AS [Page Delta]
			,CASE
					WHEN dsap.[Scan Number] > 1 THEN NULL
					WHEN dsji.FilePrefix IS NOT NULL THEN 
							CASE WHEN dsjo.[FileName] IS NOT NULL THEN 
										CASE WHEN (dsjo.[PageCount] - dsji.[PageCount]) != 0 THEN 
												CASE WHEN dsjo.ErrorMessage IS NULL THEN 'Page Delta'
													 ELSE 'FAIL'
										        END
											 WHEN (dsjo.[Destinations Count] != dsji.[DestinationCount]) THEN 'Missing Destinations'
											 ELSE dsjo.[MatchingAndGood]
										 END
							     ELSE CASE WHEN CHARINDEX('Complete', ae.STATUS) > 0 THEN 'Missing'
									  ELSE 'Not Received'
								      END
							END
					WHEN ae.ActivityType IN ('HPEC', 'LanFax', 'ScanToEmail', 'ScanToFolder', 'ScanToHpcr', 'ScanToWorkflow', 'AutoStore') THEN 'Not Scanned'
					WHEN dsjo.DigitalSendJobOutputId IS NOT NULL THEN 'Orphaned'
					ELSE ''
			 END AS [Validation Result]
			,dsjo.ErrorMessage AS [Output Error]
			,dssj.JobType AS [Job Type]
			,dssj.CompletionStatus AS [Completion Status]
			,dssj.CompletionDateTime AS [Completion Time]
			,dssj.FileSizeBytes AS [Server File Size]
			,dssj.FileType AS [File Type]
			,dssj.ScannedPages AS [Server Pages]
			,dssj.DssVersion AS [Server Version]
			,dssj.ProcessedBy AS [Processed By]
			,dssj.DeviceModel AS [Device Model]
			,CASE 
				WHEN dsap.ProcessSendingBegin > ( CASE WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
												       ELSE dsjo.FileSentDateTime
												       END	) THEN 0
				ELSE Reports.TimeDiffInDays(dsap.ProcessSendingBegin, CASE WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
																	  ELSE dsjo.FileSentDateTime 
																	  END) 
				END AS [Scan to File Sent]
			,Reports.TimeDiffInDays(dsap.ProcessSendingBegin, CASE WHEN dssj.CompletionDateTime > dsjo.FileReceivedDateTime THEN dssj.CompletionDateTime
														           ELSE dsjo.FileReceivedDateTime
												                   END) AS [Scan to Server Done]
			,Reports.TimeDiffInDays(dsap.ActivityBegin, CASE WHEN CHARINDEX('email', ae.ActivityType) > 0 THEN dsjo.FileReceivedDateTime
												             ELSE dsjo.FileSentDateTime
											                 END) AS [Total Time]
			,CASE WHEN dssj.CompletionDateTime < dsap.ActivityBegin THEN 'No'
				  ELSE 'Yes'
			 END AS [Valid Server Time]
			,CASE WHEN dssj.DeviceModel IS NULL	THEN 'No'
				  ELSE CASE WHEN CHARINDEX(RIGHT(pd.Model, 4), dssj.DeviceModel) > 0 THEN 'No'
						    ELSE 'Yes'
			                END
			      END AS [Duplicate?]
			,sd.FirmwareRevision AS [Firmware Version]
			,pd.Model         
			,pd.Platform
			,CASE 
				   WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
				   ELSE pd.[Group] + ' ' + pd.DeviceColor + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
			       END AS [Device Category]
			 ,pd.[Group]
			 ,pd.DeviceColor AS [Device Color]
			 ,pd.MaxMediaSize AS [Max Media Size]
	    	 ,CASE WHEN CHARINDEX('SIM', aeau.AssetId) > 0 THEN 'Yes'
				   ELSE 'No' 
		           END AS [SIM?]
             ,CAST(ae.ActivityExecutionId as nvarchar(40)) AS [ActivityEID]
FROM [ActivityExecution] ae  WITH (NOLOCK)
LEFT OUTER JOIN #DSActivityPerformance dsap ON ae.ActivityExecutionId = dsap.ActivityExecutionId
FULL JOIN dbo.DigitalSendJobInput dsji WITH (NOLOCK) ON dsji.SessionId = ae.SessionId AND dsji.ActivityExecutionId = ae.ActivityExecutionId
FULL OUTER JOIN (
	SELECT *
		,IIF(
			MAX(ErrorMessage) OVER (PARTITION BY _dsjo.[FileName]) IS NULL
			AND
			/* evaulates to total number of different FileSize items for a specific filename by swapping counting direction */
			(dense_rank() OVER (PARTITION BY _dsjo.[FileName] ORDER BY _dsjo.[FileSizeBytes]) + dense_rank() OVER (PARTITION BY _dsjo.[FileName] ORDER BY _dsjo.[FileSizeBytes] DESC)) = 2
			, 'Valid', 'Failed') AS [MatchingAndGood]
		,row_number() OVER (PARTITION BY _dsjo.[FileName] ORDER BY _dsjo.[FileLocation]) AS [Destination Number]
		,COUNT(*) OVER (PARTITION BY _dsjo.[FileName]) AS [Destinations Count]
	FROM DigitalSendJobOutput _dsjo WITH (NOLOCK) ) dsjo ON ae.SessionId = dsjo.SessionId AND dsji.FilePrefix = dsjo.FilePrefix
LEFT OUTER JOIN dbo.ActivityExecutionAssetUsage aeau WITH (NOLOCK) ON aeau.ActivityExecutionId = ae.ActivityExecutionId
LEFT OUTER JOIN dbo.SessionDevice sd WITH (NOLOCK) ON sd.SessionId = aeau.SessionId AND sd.DeviceId = aeau.AssetId
LEFT OUTER JOIN dbo.ProductDetails pd WITH (NOLOCK) ON pd.Product = sd.ProductName
LEFT OUTER JOIN dbo.DigitalSendServerJob dssj WITH (NOLOCK) ON ((dsjo.SessionId = dssj.SessionId OR aeau.SessionId = dssj.SessionId)
	AND (dsjo.[FileName] = dssj.[FileName])
	AND (
		dssj.JobType != 'SendFax'
		OR dssj.CompletionDateTime IS NULL
		OR dssj.CompletionDateTime > dsap.[App Button Press]
		)
	AND (
		dsjo.[PageCount] = dssj.ScannedPages
		OR dssj.ScannedPages IS NULL
		)
)
FULL JOIN (SELECT TOP 1 StartDateTime FROM [ActivityExecution] WITH (NOLOCK) WHERE SessionId IN (@sessionId) ORDER BY StartDateTime ASC) aefirst ON aefirst.StartDateTime <= ae.StartDateTime
WHERE ae.SessionId IN (@sessionId) AND (dsap.[Scan Number] = 1 OR dsjo.[Destination Number] = 1 OR dsap.[Scan Number] IS NULL or dsjo.[Destination Number] IS NULL)
ORDER BY ae.StartDateTime, ae.ActivityExecutionId, dsap.[Scan Number], dsjo.[Destination Number]

END
GO

/******  Object:  StoredProcedure [Reports].[GetBorgPerformanceData]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[GetBorgPerformanceData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [Reports].[GetBorgPerformanceData] AS' 
END
GO
-- ============================================================================
-- Author:      Daniel Ognomo Bakyono
-- Create date: 4/24/2019
-- Description: Get performance information for the Borg daskboard.
-- ============================================================================
ALTER PROCEDURE [Reports].[GetBorgPerformanceData]
    @firmwareCycles varchar(255),
    @productNames varchar(255)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE CurrentSession_Cursor CURSOR FOR
        SELECT DISTINCT ae.SessionId
        FROM ActivityExecution ae WITH (NOLOCK)
        JOIN PrintJobClient pjc WITH (NOLOCK) ON pjc.SessionId = ae.SessionId AND pjc.ActivityExecutionId = ae.ActivityExecutionId
        JOIN SessionDevice sd WITH (NOLOCK) ON sd.SessionId = ae.SessionId 
        WHERE ae.SessionId IN (SELECT SessionId
                               FROM SessionSummary WITH (NOLOCK)
                               WHERE Tags LIKE '%Official%' AND Cycle IN (SELECT value FROM STRING_SPLIT(@firmwareCycles, ',')) AND [Status] LIKE 'Complete' AND ShutdownState LIKE 'Complete'
                              ) 
              AND ae.ActivityType IN ('HPACPullPrinting', 'SafeComPullPrinting', 'EquitracPullPrinting', 'Blueprint', 'PaperCut')
              AND ae.[Status] IN ('Passed')
              AND pjc.[FileName] = '1pgFlyer4MB.docx'
              AND sd.ProductName IN (SELECT value FROM STRING_SPLIT(@productNames, ','));

    OPEN CurrentSession_Cursor;
    DECLARE @currentSession varchar(50);
    FETCH NEXT FROM CurrentSession_Cursor INTO @currentSession;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        --PRINT @currentSession;

        DROP TABLE IF EXISTS #PPResults
        DROP TABLE IF EXISTS #PPMedianStats

        DECLARE @noofstds int = 2;

        SELECT ae.SessionId
              ,sessinfo.[Session Num]
              ,sessinfo.[Solution Version]
              ,sessinfo.[Firmware Build] AS [Session Firmware]
              ,aeau.AssetId AS DeviceId
              ,CASE 
                   WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
                   ELSE sd.ProductName
               END AS Product
              ,sd.ShortName AS [Product Designation]
              ,sd.FirmwareBuild
              ,CASE ae.ActivityType
                   WHEN 'Printing' THEN 'Printing'
                   WHEN 'Authentication' THEN 'AuthOnly'
                   WHEN 'SafeComPullPrinting' THEN 'PullPrint'
                   WHEN 'HpacPullPrinting' THEN 'PullPrint'
                   WHEN 'EquitracPullPrinting' THEN 'PullPrint'
                   WHEN 'Blueprint' THEN 'PullPrint'
                   WHEN 'PaperCut' THEN 'PullPrint'
                   ELSE 'NonPullPrint'
               END AS [Activity Group]
              ,ae.ActivityType
              ,CASE
                   WHEN aep.AppButtonPress < aep.AuthenticationBegin
                       /* Normal, Start with button press */
                       THEN DATEDIFF(ms, aep.AppButtonPress, aep.AuthenticationBegin) / (1000.0 * 24 * 3600)
                   /* Wrong Config, start with Sign In button (was used in old runs)*/
                   ELSE DATEDIFF(ms, aep.PressDeviceSignIn, aep.AuthenticationBegin) / (1000.0 * 24 * 3600)
               END AS [Show Auth Screen]
              ,CASE
                   WHEN aep.AppButtonPress < aep.AuthenticationBegin
                       /* Normal, Start with button press */
                       THEN DATEDIFF(ms, aep.EnterCredentialsEnd, aep.DocumentListReady) / (1000.0 * 24 * 3600)
                   /* Wrong Config, start with Sign In button (was used in old runs)*/
                   ELSE DATEDIFF(ms, aep.AppButtonPress, aep.DocumentListReady) / (1000.0 * 24 * 3600)
               END AS [Application Ready]
               --,DATEDIFF(MILLISECOND, aep.EnterCredentialsEnd, aep.AuthenticationEnd) / 1000.0 / 3600 / 24 AS [Sign In to App]
               --,DATEDIFF(MILLISECOND, aep.AuthenticationEnd, aep.DocumentListReady) / 1000.0 / 3600 / 24 AS [Retrieve Doc List]
               --,DATEDIFF(MILLISECOND, aep.PrintBegin, aep.PrintEnd) / 1000.0 / 3600 / 24 AS [Pull and Print]
               ,DATEDIFF(MILLISECOND, aep.PrintBegin, IIF(aep.ProcessingJobEnd > aep.PullingJobFromServerEnd, aep.ProcessingJobEnd, aep.PullingJobFromServerEnd)) / 1000.0 / 3600 / 24 AS [Pull and Print]
               ,DATEDIFF(MILLISECOND, aep.DeviceSignOutBegin, aep.DeviceSignOutEnd) / 1000.0 / 3600 / 24 AS [Sign Out of App]
               ,CASE
                    WHEN aep.AppButtonPress IS NULL THEN DATEDIFF(MILLISECOND, aep.AuthenticationEnd, IIF(aep.DeviceSignOutEnd > aep.PrintEnd, aep.DeviceSignOutEnd, aep.PrintEnd)) / 1000.0 / 3600 / 24
                    ELSE DATEDIFF(MILLISECOND, aep.AppButtonPress, IIF(aep.DeviceSignOutEnd > aep.PrintEnd, aep.DeviceSignOutEnd, aep.PrintEnd)) / 1000.0 / 3600 / 24
                END AS [Total Pull Time]
               ,pd.[Platform]
               ,CASE 
                    WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
                    ELSE pd.[Group] + ' ' + CASE pd.Color
                                                WHEN 1 THEN 'Color'
                                                WHEN 0 THEN 'Mono'
                                                ELSE ''
                                            END + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
                END AS [Device Category]
               ,pd.[Group]
               ,CASE pd.Color
                    WHEN 1 THEN 'Color'
                    WHEN 0 THEN 'Mono'
                    ELSE ''
                END AS [Device Color]
               ,pd.MaxMediaSize AS [Max Media Size]
               ,pd.[Function]
               ,CAST(ae.ActivityExecutionId as nvarchar(40)) AS [Activity Execution ID]
        INTO #PPResults
        FROM ActivityExecution ae WITH (NOLOCK)
        JOIN (SELECT prodcom.SessionId
                    ,RANK() OVER (ORDER BY devfw.[Firmware Date] ASC, prodcom.[Version] ASC) AS [Session Num]
                    ,devfw.[Firmware Date]
                    ,devfw.[Firmware Build]
                    ,prodcom.[Name] AS Solution
                    ,prodcom.[Version] AS [Solution Version]
              FROM (SELECT ISNULL(spa.SessionId, aprod.SessionId) AS SessionId
                          ,ISNULL(spa.[Name], aprod.[Name]) AS [Name]
                          ,spa.[Version]
                    FROM SessionProductAssoc spa WITH (NOLOCK)
                    FULL JOIN (SELECT DISTINCT ae.SessionId
                                     ,CASE
                                          WHEN ae.ActivityType IN ('EquitracPullPrinting') THEN 'Equitrac'
                                          WHEN ae.ActivityType IN ('HPACPullPrinting') THEN 'HPAC'
                                          WHEN ae.ActivityType IN ('SafeComPullPrinting') THEN 'SafeCom'
                                          WHEN ae.ActivityType IN ('Blueprint', 'PaperCut') THEN ae.ActivityType
                                      END AS [Name]
                               FROM ActivityExecution ae WITH (NOLOCK)
                               WHERE ae.SessionId IN (@currentSession)
                                     AND ae.ActivityType IN ('HPACPullPrinting', 'SafeComPullPrinting', 'EquitracPullPrinting', 'Blueprint', 'PaperCut')
                                     AND ae.[Status] IN ('Passed', 'Failed')
                              ) aprod ON spa.SessionId = aprod.SessionId AND spa.[Name] = aprod.[Name]
                    WHERE spa.SessionId IN (@currentSession) OR spa.SessionId IS NULL
                   ) prodcom
              JOIN (SELECT SessionId
                          ,MAX(FirmwareDatecode) AS [Firmware Date]
                          ,SUBSTRING(MAX(FirmwareRevision), 0, CHARINDEX('_', MAX(FirmwareRevision))) AS [Firmware Build]
                    FROM SessionDevice WITH (NOLOCK)
                    WHERE SessionId IN (@currentSession)
                    GROUP BY [SessionId]
                   ) devfw ON devfw.SessionId = prodcom.SessionId
              WHERE prodcom.[Name] IN ('Equitrac', 'HPAC', 'SafeCom', 'Blueprint', 'PaperCut')
             ) sessinfo ON sessinfo.SessionId = ae.SessionId
        LEFT OUTER JOIN ActivityExecutionAssetUsage aeau WITH (NOLOCK) ON aeau.ActivityExecutionId = ae.ActivityExecutionId
        LEFT OUTER JOIN (SELECT SessionId
                               ,DeviceId
                               ,ProductName
                               ,IIF(CHARINDEX(' ', ProductName) > 0, SUBSTRING(ProductName, 0, CHARINDEX(' ', ProductName)), ProductName) AS ShortName
                               ,DeviceName
                               ,FirmwareRevision
                               ,IIF(CHARINDEX('_', FirmwareRevision) > 0, SUBSTRING(FirmwareRevision, 0, CHARINDEX('_', FirmwareRevision)), FirmwareRevision) AS FirmwareBuild
                               ,FirmwareDatecode
                               ,IpAddress
                         FROM SessionDevice WITH (NOLOCK)
                         WHERE SessionId IN (@currentSession)
                        ) sd ON sd.SessionId = aeau.SessionId
                                AND sd.DeviceId = aeau.AssetId
        LEFT OUTER JOIN ProductDetails pd WITH (NOLOCK) ON pd.Product = sd.ProductName
        LEFT OUTER JOIN (SELECT pvt.ActivityExecutionId
                               ,pvt.AppButtonPress
                               ,pvt.PressDeviceSignIn
                               ,pvt.AuthenticationBegin
                               ,pvt.AuthenticationEnd
                               ,pvt.EnterCredentialsEnd
                               ,pvt.DocumentListReady
                               ,pvt.PrintBegin
                               ,pvt.PrintEnd
                               ,pvt.PullingJobFromServerEnd
                               ,pvt.ProcessingJobEnd
                               ,pvt.DeviceSignOutBegin
                               ,pvt.DeviceSignOutEnd
                         FROM (SELECT aep.SessionId
                                     ,aep.ActivityExecutionId
                                     ,CASE 
                                          WHEN EventLabel = 'DeviceButtonPress_Sign In' THEN 'PressDeviceSignIn'
                                          WHEN LEFT(EventLabel, 15) = 'AppButtonPress_' THEN 'AppButtonPress'
                                          WHEN EventLabel = 'PrintAllBegin' OR EventLabel = 'PrintKeepBegin' OR EventLabel = 'PrintDeleteBegin' THEN 'PrintBegin'
                                          WHEN EventLabel = 'PrintAllEnd' OR EventLabel = 'PrintKeepEnd' OR EventLabel = 'PrintDeleteEnd' THEN 'PrintEnd'
                                          ELSE EventLabel
                                      END AS EventLabel
                                     ,EventDateTime
                               FROM ActivityExecutionPerformance aep WITH (NOLOCK)
                               JOIN PrintJobClient pjc WITH (NOLOCK) ON aep.SessionId = pjc.SessionId
                                    AND aep.ActivityExecutionId = pjc.ActivityExecutionId
                               WHERE aep.SessionId IN (@currentSession) AND pjc.FileName = '1pgFlyer4MB.docx'
                              ) AS p
                         PIVOT(MAX(p.EventDateTime) FOR p.EventLabel IN (AppButtonPress
                                                                        ,PressDeviceSignIn
                                                                        ,AuthenticationBegin
                                                                        ,AuthenticationEnd
                                                                        ,EnterCredentialsEnd
                                                                        ,DocumentListReady
                                                                        ,PrintBegin
                                                                        ,PrintEnd
                                                                        ,PullingJobFromServerEnd
                                                                        ,ProcessingJobEnd
                                                                        ,DeviceSignOutBegin
                                                                        ,DeviceSignOutEnd
                                                                        )
                              ) AS pvt
                        ) aep ON ae.ActivityExecutionId = aep.ActivityExecutionId
        LEFT JOIN PrintJobClient pjc WITH (NOLOCK) ON pjc.SessionId = ae.SessionId
                                                      AND pjc.ActivityExecutionId = ae.ActivityExecutionId
        WHERE ae.SessionId IN (@currentSession)
              AND ae.UserName != 'Jawa'
              AND ae.[Status] = 'Passed'
              AND ae.ActivityType IN ('EquitracPullPrinting','HPACPullPrinting','SafeComPullPrinting','Blueprint','PaperCut')
              AND (pjc.FileName = '1pgFlyer4MB.docx')
        ORDER BY ae.StartDateTime;

        IF (SELECT COUNT(*) FROM #PPResults) = 0
        GOTO SkipRow;

        SELECT DISTINCT SessionId
                       ,[Product Designation]
                       ,FirmwareBuild
                       ,AVG([Show Auth Screen]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Show Auth Screen Avg]
                       ,STDEV([Show Auth Screen]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Show Auth Screen StDev]
                       ,COUNT([Show Auth Screen]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Show Auth Screen Count]
                       ,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Show Auth Screen]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Show Auth Screen Median]

                       --,AVG([Sign In to App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign In to App Avg]
                       --,STDEV([Sign In to App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign In to App StDev]
                       --,COUNT([Sign In to App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign In to App Count]
                       --,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Sign In to App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign In to App Median]

                       --,AVG([Retrieve Doc List]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Retrieve Doc List Avg]
                       --,STDEV([Retrieve Doc List]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Retrieve Doc List StDev]
                       --,COUNT([Retrieve Doc List]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Retrieve Doc List Count]
                       --,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Retrieve Doc List]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Retrieve Doc List Median]

                       ,AVG([Application Ready]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Application Ready Avg]
                       ,STDEV([Application Ready]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Application Ready StDev]
                       ,COUNT([Application Ready]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Application Ready Count]
                       ,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Application Ready]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Application Ready Median]

                       ,AVG([Pull and Print]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Pull and Print Avg]
                       ,STDEV([Pull and Print]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Pull and Print StDev]
                       ,COUNT([Pull and Print]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Pull and Print Count]
                       ,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Pull and Print]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Pull and Print Median]

                       ,AVG([Sign Out of App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign Out of App Avg]
                       ,STDEV([Sign Out of App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign Out of App StDev]
                       ,COUNT([Sign Out of App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign Out of App Count]
                       ,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Sign Out of App]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Sign Out of App Median]

                       ,AVG([Total Pull Time]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Total Pull Time Avg]
                       ,STDEV([Total Pull Time]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Total Pull Time StDev]
                       ,COUNT([Total Pull Time]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Total Pull Time Count]
                       ,PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY [Total Pull Time]) OVER (PARTITION BY SessionId, [Product Designation], FirmwareBuild) AS [Total Pull Time Median]

        INTO #PPMedianStats
        FROM #PPResults MQA
        ORDER BY SessionId, [Product Designation];

        DECLARE @TResults Table (SessionId varchar(50),
                                 [Session Num] bigint,
                                 [Product Designation] nvarchar(255),
                                 FirmwareBuild nvarchar(50),
                                 [Solution Version] nvarchar(1020),
                                 [Device Category] nvarchar(73),
                                 [Show Auth Screen] numeric(25,14),
                                 [Show Auth Screen Count] numeric(25,14),
                                 [Application Ready] numeric(25,14),
                                 [Application Ready Count] numeric(25,14),
                                 [Pull and Print] numeric(25,14),
                                 [Pull and Print Count] numeric(25,14),
                                 [Sign Out of App] numeric(25,14),
                                 [Sign Out of App Count] numeric(25,14),
                                 [Total Pull Time] numeric(25,14),
                                 [Total Pull Time Count] numeric(25,14)
                                )

        INSERT INTO @TResults 

        --SELECT name, system_type_name, is_nullable
        --FROM sys.dm_exec_describe_first_result_set('select * from #PPResults', NULL, 0)
        SELECT ppr.SessionId
              ,ppr.[Session Num]
              ,ppms.[Product Designation]
              ,ppms.FirmwareBuild
              ,ppr.[Solution Version]
              ,ppr.[Device Category]

              ,AVG (CASE WHEN ABS(ppr.[Show Auth Screen] - ppms.[Show Auth Screen Median]) <= @noofstds * ppms.[Show Auth Screen StDev] THEN ppr.[Show Auth Screen] END) AS [Show Auth Screen]
              ,ppms.[Show Auth Screen Count]

              --,AVG (CASE WHEN ABS(ppr.[Sign In to App] - ppms.[Sign In to App Median]) <= @noofstds * ppms.[Sign In to App StDev] THEN ppr.[Sign In to App] END) AS [Sign In to App Weighted]
              --,ppms.[Sign In to App Count]

              --,AVG (CASE WHEN ABS(ppr.[Retrieve Doc List] - ppms.[Retrieve Doc List Median]) <= @noofstds * ppms.[Retrieve Doc List StDev] THEN ppr.[Retrieve Doc List] END) AS [Retrieve Doc List Weighted]
              --,ppms.[Retrieve Doc List Count]

              ,AVG (CASE WHEN ABS(ppr.[Application Ready] - ppms.[Application Ready Median]) <= @noofstds * ppms.[Application Ready StDev] THEN ppr.[Application Ready] END) AS [Application Ready]
              ,ppms.[Application Ready Count]

              ,AVG (CASE WHEN ABS(ppr.[Pull and Print] - ppms.[Pull and Print Median]) <= @noofstds * ppms.[Pull and Print StDev] THEN ppr.[Pull and Print] END) AS [Pull and Print]
              ,ppms.[Pull and Print Count]

              ,AVG (CASE WHEN ABS(ppr.[Sign Out of App] - ppms.[Sign Out of App Median]) <= @noofstds * ppms.[Sign Out of App StDev] THEN ppr.[Sign Out of App] END) AS [Sign Out of App]
              ,ppms.[Sign Out of App Count]

              ,AVG (CASE WHEN ABS(ppr.[Total Pull Time] - ppms.[Total Pull Time Median]) <= @noofstds * ppms.[Total Pull Time StDev] THEN ppr.[Total Pull Time] END) AS [Total Pull Time]
              ,ppms.[Total Pull Time Count]

        FROM #PPResults ppr
        JOIN #PPMedianStats ppms ON ppms.SessionId = ppr.SessionId AND ppms.[Product Designation] = ppr.[Product Designation] AND ppms.FirmwareBuild = ppr.FirmwareBuild
        GROUP BY
            ppr.SessionId, ppr.[Session Num], ppms.[Product Designation], ppms.FirmwareBuild, ppr.[Device Category], ppr.[Solution Version]
            , ppms.[Show Auth Screen Median], ppms.[Show Auth Screen StDev], ppms.[Show Auth Screen Count]
            --, ppms.[Sign In to App Median], ppms.[Sign In to App StDev], ppms.[Sign In to App Count]
            --, ppms.[Retrieve Doc List Median], ppms.[Retrieve Doc List StDev], ppms.[Retrieve Doc List Count]
            , ppms.[Application Ready Median], ppms.[Application Ready StDev], ppms.[Application Ready Count]
            , ppms.[Pull and Print Median], ppms.[Pull and Print StDev], ppms.[Pull and Print Count]
            , ppms.[Sign Out of App Median], ppms.[Sign Out of App StDev], ppms.[Sign Out of App Count]
            , ppms.[Total Pull Time Median], ppms.[Total Pull Time StDev], ppms.[Total Pull Time Count]
        ORDER BY ppr.[Session Num] ASC, ppms.[Product Designation] ASC;

SkipRow:

    FETCH NEXT FROM CurrentSession_Cursor INTO @currentSession;
    END

    CLOSE CurrentSession_Cursor;
    DEALLOCATE CurrentSession_Cursor;

    SELECT  min([StartDateTime]), 
	/*[@TResults].[SessionId]
	, [Session Num], 
			[Product Designation],FirmwareBuild, 
			[Solution Version], [Device Category], 
			[Show Auth Screen], [Show Auth Screen Count], 
			[Application Ready], [Application Ready Count], 
			[Pull and Print], [Pull and Print Count],
            [Sign Out of App], [Sign Out of App Count],
			[Total Pull Time], [Total Pull Time Count],*/
			[Cycle] 
			FROM @TResults JOIN [SessionSummary] on [@TResults].SessionId = [SessionSummary].SessionId WHERE [Product Designation] IN (SELECT value FROM STRING_SPLIT(@productNames, ',')) GROUP BY [Cycle]
			/*, [@TResults].[SessionId]
			, [Session Num], 
			[Product Designation],FirmwareBuild, 
			[Solution Version], [Device Category], 
			[Show Auth Screen], [Show Auth Screen Count], 
			[Application Ready], [Application Ready Count], 
			[Pull and Print], [Pull and Print Count],
            [Sign Out of App], [Sign Out of App Count],
			[Total Pull Time], [Total Pull Time Count]*/;
END
GO

/******  Object:  StoredProcedure [Reports].[PullPrintReport]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[PullPrintReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [Reports].[PullPrintReport] AS' 
END
GO
-- =============================================
-- Author:		report_designer
-- Create date: 12/12/2018
-- Description:	Returns the data used in the Excel based PullPrintReport.
-- =============================================
ALTER PROCEDURE [Reports].[PullPrintReport] 
    @sessionId varchar(50)
AS
BEGIN
    DROP TABLE IF EXISTS #PPActivityPerformance
    DROP TABLE IF EXISTS #PPPrintServerJob
    DROP TABLE IF EXISTS #PPActivityExecutionAssetUsage

    SET NOCOUNT ON
    SET ANSI_WARNINGS OFF

    -- Fill the #PPActivityPerformance temp table.
    SELECT paep.ActivityExecutionId
          ,paep.AppButtonPress
          ,paep.PressDeviceSignIn
          ,paep.AuthenticationBegin
          ,paep.AuthenticationEnd
		  ,paep.EnterCredentialsBegin
          ,paep.EnterCredentialsEnd
          ,paep.AppShown
          ,paep.PrintBegin
          ,paep.PrintEnd
          ,paep.ProcessingJobBegin
          ,paep.PullingJobFromServerEnd
          ,paep.ProcessingJobEnd
          ,paep.DeviceSignOutBegin
          ,paep.DeviceSignOutEnd
          ,paed.AuthType
          ,paed.SignOutType
    INTO #PPActivityPerformance
    FROM (SELECT SessionId
                ,ActivityExecutionId
                ,CASE
                     WHEN EventLabel = 'DeviceButtonPress_Sign In' THEN 'PressDeviceSignIn'
                     WHEN left(EventLabel, 15) = 'AppButtonPress_' THEN 'AppButtonPress'
                     WHEN EventLabel IN ('PrintAllBegin', 'PrintKeepBegin', 'PrintDeleteBegin') THEN 'PrintBegin'
                     WHEN EventLabel IN ('PrintAllEnd', 'PrintKeepEnd', 'PrintDeleteEnd') THEN 'PrintEnd'
                     ELSE EventLabel
                 END AS EventLabel
                ,EventDateTime
          FROM ActivityExecutionPerformance WITH (NOLOCK)
          WHERE SessionId IN (@sessionId)) AS saep
    PIVOT (MAX(saep.EventDateTime) FOR saep.EventLabel IN (AppButtonPress
                                                          ,PressDeviceSignIn
                                                          ,AuthenticationBegin
                                                          ,AuthenticationEnd
														  ,EnterCredentialsBegin
                                                          ,EnterCredentialsEnd
                                                          ,AppShown
                                                          ,PrintBegin
                                                          ,PrintEnd
                                                          ,ProcessingJobBegin
                                                          ,PullingJobFromServerEnd
                                                          ,ProcessingJobEnd
                                                          ,DeviceSignOutBegin
                                                          ,DeviceSignOutEnd)) AS paep
    LEFT JOIN (SELECT *
               FROM (SELECT ActivityExecutionId
                           ,Label
                           ,Message
                     FROM ActivityExecutionDetail WITH (NOLOCK)
                     WHERE SessionId IN (@sessionId) AND Label IN ('AuthType', 'SignOutType')) AS saed
               PIVOT (MAX(saed.Message) FOR saed.Label IN (AuthType, SignOutType)) AS ipaep) AS paed ON paed.ActivityExecutionID = paep.ActivityExecutionId

    -- Fill the #PPPrintServerJob temp table.
    SELECT s.PrintDriver
          ,SUM(s.PrintedBytes) AS PrintedBytes
          ,SUM(s.PrintedPages) AS PrintedPages
          ,MAX(s.PrintEndDateTime) AS PrintEndDateTime
          ,s.PrintJobClientId
          ,s.PrintServerOS
          ,MIN(s.PrintStartDateTime) AS PrintStartDateTime
          ,MAX(s.SpoolEndDateTime) AS SpoolEndDateTime
          ,MIN(s.SpoolStartDateTime) AS SpoolStartDateTime
    INTO #PPPrintServerJob
    FROM PrintServerJob s WITH (NOLOCK)
    Full JOIN PrintJobClient pc WITH (NOLOCK) ON pc.PrintJobClientId = s.PrintJobClientId
    WHERE pc.SessionId IN (@sessionId)
    GROUP BY s.PrintJobClientId, s.PrintServer, s.PrintServerOS, s.PrintQueue, s.PrintDriver, s.DataType

    -- Fill the #PPActivityExecutionAssetUsage temp table.
    SELECT ActivityExecutionId
          ,MIN(AssetId) AS AssetId
          ,SessionId
    INTO #PPActivityExecutionAssetUsage
    FROM ActivityExecutionAssetUsage WITH (NOLOCK)
    WHERE SessionId IN (@sessionId)
    GROUP BY ActivityExecutionId, SessionId

    -- Create indexes on the temp tables.
    CREATE NONCLUSTERED INDEX idx ON #PPPrintServerJob (PrintJobClientId)
    CREATE NONCLUSTERED INDEX idx ON #PPActivityPerformance (ActivityExecutionId)
    CREATE NONCLUSTERED INDEX idx ON #PPActivityExecutionAssetUsage (ActivityExecutionId)

    -- Perform the main work of the procedure.
    SELECT ae.SessionId
          ,FLOOR((DATEDIFF(SECOND, ss.StartDateTime, ae.StartDateTime) - 480) / 3600) + 1 AS [Dur Hour]
          ,ae.StartDateTime AS [Activity Start]
          ,ae.EndDateTime AS [Activity End]
          ,ae.UserName AS [User Name]
          ,aeau.AssetId AS DeviceId
          ,CASE 
               WHEN ae.ActivityType = 'HpacAuthentication' THEN 'SIM'
               WHEN aeau.AssetId LIKE 'VP%' THEN 'Virtual Print'
               ELSE sd.ProductName
           END AS Product
          ,ae.ActivityName AS [Activity Name]
          ,ISNULL(ae.ActivityType, ppjr.SolutionType) AS [Solution Type]
          ,Reports.ActivityGroup(ae.ActivityType) AS [Activity Group]
          ,REPLACE(ae.Status, 'Activity', '') AS [Update Type]
          ,ae.ResultMessage AS [Error Message]
          ,ae.ResultCategory AS [Error Group Message]
          ,ae.HostName AS [Client Name]
          ,Reports.ClientOS(pjc.ClientOS) AS [Client OS]
          ,pjc.FileName AS [File Name]
          ,pjc.FileSizeBytes AS [File Size]
          ,sdoc.Pages AS [File Pages]
          ,Reports.FileType(sdoc.Extension) AS [File Type]
          ,CASE 
               WHEN CHARINDEX('HP Universal', psj.PrintDriver) > 0 THEN ('HP UPD ' + RIGHT(psj.PrintDriver, LEN(psj.PrintDriver) - 22))
               ELSE CASE 
                        WHEN CHARINDEX('Safecom', pjc.PrintQueue) > 0 THEN pjc.PrintQueue
                        ELSE psj.PrintDriver
                    END
           END AS [Print Driver]
          ,Reports.PrintServerOS(psj.PrintServerOS) AS [Print Server OS]
          ,CASE pjc.JobEndDateTime
               WHEN '' THEN ''
               ELSE Reports.TimeDiffInDays(pjc.PrintStartDateTime, pjc.JobEndDateTime)
           END AS [Client Time]
          ,CASE psj.SpoolEndDateTime
               WHEN '' THEN ''
               ELSE Reports.TimeDiffInDays(psj.SpoolStartDateTime, psj.SpoolEndDateTime)
           END AS SpoolTime
          ,CASE psj.PrintEndDateTime
               WHEN '' THEN ''
               ELSE Reports.TimeDiffInDays(psj.PrintStartDateTime, psj.PrintEndDateTime)
           END AS RenderTime
          ,CASE psj.PrintEndDateTime
               WHEN '' THEN ''
               ELSE Reports.TimeDiffInDays(pjc.PrintStartDateTime, psj.PrintEndDateTime)
           END AS [Total Print Time]
          ,psj.PrintedPages AS [Printed Pages]
          ,pdj.JobEndStatus AS [Job End Status]
          ,ISNULL(aer.RetryCount, 0) AS [Retry Count]
          ,ppjr.InitialJobCount AS [Document List Count]
          ,IIF(aep.AppButtonPress IS NOT NULL, IIF(aep.PressDeviceSignIn IS NOT NULL, 'Eager', 'Lazy'), NULL) AS [Auth Urgency]
          ,aep.AuthType AS [Auth Type]
          ,aep.SignOutType AS [Sign Out Type]
          ,CASE
               /* Lazy Auth */
               WHEN aep.AppButtonPress < aep.AuthenticationBegin THEN Reports.TimeDiffInDays(aep.AppButtonPress, aep.AuthenticationBegin)
               /* Eager Auth */
               ELSE Reports.TimeDiffInDays(aep.PressDeviceSignIn, aep.AuthenticationBegin)
           END AS [Show Auth Screen]
          ,CASE
               WHEN aep.AuthType = 'Proximity Card' THEN Reports.TimeDiffInDays(aep.EnterCredentialsBegin, aep.AuthenticationEnd)
               ELSE Reports.TimeDiffInDays(aep.EnterCredentialsEnd, aep.AuthenticationEnd)
           END AS [Authentication]

          ,CASE 
               WHEN aep.EnterCredentialsEnd > aep.AppButtonPress THEN Reports.TimeDiffInDays(aep.AuthenticationEnd, aep.AppShown) /*Lazy*/
               ELSE Reports.TimeDiffInDays(aep.AppButtonPress, aep.AppShown) /*Eager*/
           END AS [Show App]

          ,Reports.TimeDiffInDays(aep.PrintBegin, IIF(aep.ProcessingJobEnd > aep.PullingJobFromServerEnd, aep.ProcessingJobEnd, aep.PullingJobFromServerEnd)) AS [Pull and Print]
          ,Reports.TimeDiffInDays(aep.PrintBegin, aep.ProcessingJobBegin) AS [Processing Job]
          ,CASE
               WHEN aep.AppButtonPress IS NULL THEN Reports.TimeDiffInDays(aep.AuthenticationEnd, IIF(aep.DeviceSignOutEnd > aep.PrintEnd, aep.DeviceSignOutEnd, aep.PrintEnd))
               ELSE Reports.TimeDiffInDays(aep.AppButtonPress, IIF(aep.DeviceSignOutEnd > aep.PrintEnd, aep.DeviceSignOutEnd, aep.PrintEnd))
           END AS [Total Pull Time]
          ,pd.Platform
          ,CASE 
               WHEN pd.[Function] = 'Scan' THEN (RTRIM(pd.MaxMediaSize) + ' Network Scanner')
               ELSE pd.[Group] + ' ' + pd.DeviceColor + ' ' + RTRIM(pd.MaxMediaSize) + ' ' + pd.[Function]
           END AS [Device Category]
          ,pd.[Group]
          ,pd.DeviceColor AS [Device Color]
          ,pd.MaxMediaSize AS [Max Media Size]
          ,pd.[Function]
          ,ae.ActivityExecutionId AS [Activity Execution ID]
    FROM ActivityExecution ae WITH (NOLOCK)
    JOIN (SELECT prodcom.SessionId
                ,RANK() OVER (ORDER BY devfw.FirmwareDate ASC, prodcom.Version ASC) AS SessionNum
                ,devfw.FirmwareDate
                ,devfw.FirmwareBuild
                ,prodcom.Name AS Solution
                ,prodcom.Version AS SolutionVersion
          FROM (SELECT ISNULL(spa.SessionID, aprod.SessionId) AS [SessionID]
                      ,ISNULL(spa.Name, aprod.Name) AS [Name]
                      ,spa.Version
                FROM SessionProductAssoc spa WITH (NOLOCK)
                FULL JOIN (SELECT DISTINCT ae.SessionId
                                 ,Reports.ActivityName(ae.ActivityType, null) AS [Name]
                           FROM ActivityExecution ae WITH (NOLOCK)
                           WHERE ae.SessionId IN (@sessionId) AND
                                 ae.ActivityType IN ('HPACPullPrinting', 'HPACSimulation', 'SafeComPullPrinting', 'EquitracPullPrinting', 'Blueprint', 'PaperCut', 'PaperCutAgentless', 'Celiveo', 'SafeQPullPrinting', 'CloudConnector','HPRoam') AND
                                 ae.Status IN ('Passed', 'Failed')
                          ) aprod ON spa.SessionId = aprod.SessionId AND spa.Name = aprod.Name
                WHERE spa.SessionId IN (@sessionId) OR spa.SessionId IS NULL
               ) prodcom
          JOIN (SELECT SessionId
                      ,MAX(FirmwareDatecode) AS FirmwareDate
                      ,SUBSTRING(MAX(FirmwareRevision), 0, CHARINDEX('_', MAX(FirmwareRevision))) AS FirmwareBuild
                FROM SessionDevice WITH (NOLOCK)
                WHERE SessionId IN (@sessionId)
                GROUP BY SessionId
               ) devfw ON devfw.SessionId = prodcom.SessionId
          WHERE prodcom.Name IN ('Equitrac', 'HPAC', 'SafeCom', 'Blueprint', 'PaperCut', 'Celiveo', 'SafeQ', 'HPRoam','JALink')
         ) sessinfo ON sessinfo.SessionId = ae.SessionId
    INNER JOIN SessionSummary ss WITH (NOLOCK) ON ss.SessionId = ae.SessionId
    LEFT OUTER JOIN #PPActivityPerformance aep ON ae.ActivityExecutionId = aep.ActivityExecutionId
    LEFT OUTER JOIN PrintJobClient pjc WITH (NOLOCK) ON pjc.SessionId = ae.SessionID AND ae.ActivityExecutionId = pjc.ActivityExecutionId
    LEFT OUTER JOIN #PPPrintServerJob psj ON pjc.PrintJobClientId = psj.PrintJobClientId
    LEFT OUTER JOIN #PPActivityExecutionAssetUsage aeau ON ae.ActivityExecutionId = aeau.ActivityExecutionId
    LEFT OUTER JOIN SessionDevice sd WITH (NOLOCK) ON sd.SessionId = ae.SessionId AND sd.DeviceId = aeau.AssetId
    LEFT OUTER JOIN ProductDetails pd WITH (NOLOCK) ON pd.Product = sd.ProductName
    LEFT OUTER JOIN (SELECT ActivityExecutionId
                           ,COUNT(*) AS RetryCount
                     FROM ActivityExecutionRetries WITH (NOLOCK)
                     WHERE SessionId IN (@sessionId)
                     GROUP BY ActivityExecutionId
                    ) aer ON aer.ActivityExecutionId = ae.ActivityExecutionId
    LEFT OUTER JOIN (SELECT ActivityExecutionId
                           ,COUNT(*) AS PPJRCount
                     FROM PullPrintJobRetrieval WITH (NOLOCK)
                     WHERE SessionId IN (@sessionId)
                     GROUP BY ActivityExecutionId
                    ) ppjrcnt ON ppjrcnt.ActivityExecutionId = ae.ActivityExecutionId
    LEFT OUTER JOIN (SELECT tp.ActivityExecutionId
                           ,tp.SolutionType
                           ,tp.InitialJobCount
                           ,tp.FinalJobCount
                           ,ROW_NUMBER() OVER (PARTITION BY tp.SessionId, tp.ActivityExecutionId ORDER BY tp.JobStartDateTime) AS RowNumber
                     FROM PullPrintJobRetrieval tp WITH (NOLOCK)
                     WHERE tp.SessionId IN (@sessionId)
                    ) ppjr ON ae.ActivityExecutionId = ppjr.ActivityExecutionId AND ppjr.RowNumber = ISNULL(ppjrcnt.PPJRCount, 1)
    LEFT OUTER JOIN PhysicalDeviceJob pdj WITH (NOLOCK) ON pdj.SessionId = ae.SessionId AND pdj.ActivityExecutionId = ae.ActivityExecutionId
    LEFT OUTER JOIN SessionDocument sdoc WITH (NOLOCK) ON sdoc.SessionId = ae.SessionId AND sdoc.FileName = pjc.FileName
    WHERE aeau.SessionId IN (@sessionId) AND ae.SessionId IN (@sessionId) and (ae.ActivityType not in ('Authentication'))
    ORDER BY ae.StartDateTime;
END
GO

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/******  Object:  Function [Reports].[ActivityGroup]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[ActivityGroup]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns activity group based on activity type.
-- =============================================
CREATE FUNCTION [Reports].[ActivityGroup] 
(
    @activityType varchar(50)
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @result varchar(50)

	SELECT @result = CASE @activityType
						 WHEN ''HPEC'' THEN ''DigitalSend'' 
						 WHEN ''LanFax'' THEN ''DigitalSend'' 
						 WHEN ''Fax'' THEN ''DigitalSend'' 
						 WHEN ''ScanToEmail'' THEN ''DigitalSend'' 
						 WHEN ''ScanToFolder'' THEN ''DigitalSend'' 
						 WHEN ''ScanToHpcr'' THEN ''DigitalSend'' 
						 WHEN ''ScanToWorkFlow'' THEN ''DigitalSend'' 
						 WHEN ''Autostore'' THEN ''DigitalSend'' 
                         WHEN ''Printing'' THEN ''Printing''
                         WHEN ''Authentication'' THEN ''Authentication''
                         WHEN ''SafeComPullPrinting'' THEN ''PullPrint''
                         WHEN ''HpacPullPrinting'' THEN ''PullPrint''
                         WHEN ''HpacSimulation'' THEN ''PullPrint''
                         WHEN ''EquitracPullPrinting'' THEN ''PullPrint''
                         WHEN ''Blueprint'' THEN ''PullPrint''
                         WHEN ''PaperCut'' THEN ''PullPrint''
						 WHEN ''PaperCutAgentless'' THEN ''PullPrint''
                         WHEN ''Celiveo'' THEN ''PullPrint''
                         WHEN ''SafeQPullPrinting'' THEN ''PullPrint''
						 WHEN ''CloudConnector'' THEN ''JALink''
						 WHEN ''LinkScanApps'' THEN ''JALink''
						 WHEN ''HpRoam'' THEN ''HPRoam''
						 WHEN ''CollectDeviceSystemInfo'' THEN ''Informational''
                         ELSE ''''
						 END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[ActivityName]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[ActivityName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns the activity name based on the activity type.
-- =============================================
CREATE FUNCTION [Reports].[ActivityName] 
(
    @activityType varchar(50),
    @serverName nvarchar(50) null
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @result varchar(50)

	SELECT @result = CASE @activityType
                         WHEN ''LanFax'' THEN IIF(@serverName IS NULL, ''Embedded DS'', ''DSS'')
                         WHEN ''ScanToEmail'' THEN IIF(@serverName IS NULL, ''Embedded DS'', ''DSS'')
                         WHEN ''ScanToFolder'' THEN IIF(@serverName IS NULL, ''Embedded DS'', ''DSS'')
                         WHEN ''ScanToWorkflow'' THEN IIF(@serverName IS NULL, ''Embedded DS'', ''DSS'')
                         WHEN ''ScanToUsb'' THEN IIF(@serverName IS NULL, ''Embedded DS'', ''DSS'')
                         WHEN ''Hpec'' THEN ''HPEC''
                         WHEN ''HpacPullPrinting'' THEN ''HPAC''
                         WHEN ''HpacSimulation'' THEN ''HPAC''
                         WHEN ''SafeComPullPrinting'' THEN ''SafeCom''
                         WHEN ''EquitracPullPrinting'' THEN ''Equitrac''
                         WHEN ''ScanToHpcr'' THEN ''HPCR''
                         WHEN ''Blueprint'' THEN ''Blueprint''
                         WHEN ''PaperCut'' THEN ''PaperCut''
						 WHEN ''PaperCutAgentless'' THEN ''PaperCut''
                         WHEN ''AutoStore'' THEN ''AutoStore''
                         WHEN ''Celiveo'' THEN ''Celiveo''
                         WHEN ''SafeQPullPrinting'' THEN ''SafeQ''
						 WHEN ''CloudConnector'' THEN ''JALink''
						 WHEN ''LinkScanApps'' THEN ''JALink''
						 WHEN ''HpRoam'' THEN ''HPRoam''
                     END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[ActivityVendor]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[ActivityVendor]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Return vendor name based on activity type.
-- =============================================
CREATE FUNCTION [Reports].[ActivityVendor] 
(
	@activityType varchar(50)
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @result varchar(50)

	SELECT @result = CASE
                         WHEN @activityType IN (''LanFax'', ''ScanToEmail'', ''ScanToFolder'', ''ScanToWorkflow'', ''Hpec'', ''ScanToUsb'') THEN ''HP''
                         WHEN @activityType IN (''HpacPullPrinting'') THEN ''LRS''
                         WHEN @activityType IN (''ScanToHpcr'') THEN ''Omtool''
                         WHEN @activityType IN (''SafeComPullPrinting'', ''EquitracPullPrinting'', ''AutoStore'') THEN ''Nuance''
                         WHEN @activityType IN (''Blueprint'') THEN ''Pharos''
                         WHEN @activityType IN (''PaperCut'', ''PaperCutAgentless'') THEN ''PaperCut''
                         WHEN @activityType IN (''Celiveo'') THEN ''Celiveo''
                         WHEN @activityType IN (''SafeQPullPrinting'') THEN ''YSoft''
						 WHEN @activityType IN (''CloudConnector'', ''LinkScanApps'')  THEN ''JALink''
						 WHEN @activityType IN (''HpRoam'') THEN ''HPRoam''
					 END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[ClientOS]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[ClientOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Converts the client operating system to a text value.
-- =============================================
CREATE FUNCTION [Reports].[ClientOS] 
(
	@clientOS nvarchar(50)
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @result nvarchar(50)

	SELECT @result = CASE SUBSTRING(@clientOS, CHARINDEX(''.'', @clientOS) - 1, 3)
                         WHEN ''10.'' THEN ''Windows 10''
                         WHEN ''6.3'' THEN ''Windows 8.1''
                         WHEN ''6.2'' THEN ''Windows 8''
                         WHEN ''6.1'' THEN ''Windows 7''
                         WHEN ''6.0'' THEN ''Windows Vista''
                         WHEN ''5.2'' THEN ''Windows XP Professional x64''
                         WHEN ''5.1'' THEN ''Windows XP''
                         ELSE @clientOS
                     END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[FileType]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[FileType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns file type based on the file''s extension.
-- =============================================
CREATE FUNCTION [Reports].[FileType] 
(
	@fileExtension nvarchar(10)
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @result nvarchar(50)

	SELECT @result = CASE (SUBSTRING(@fileExtension, 1, 3))
                         WHEN ''PDF'' THEN ''Acrobat''
                         WHEN ''PPT'' THEN ''PowerPoint''
                         WHEN ''DOC'' THEN ''Word''
                         WHEN ''XLS'' THEN ''Excel''
                         WHEN ''txt'' THEN ''NotePad''
                         ELSE ''''
                     END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[MemorySummary]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[MemorySummary]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns information about the memory used on devices in the specified session.
-- =============================================
CREATE FUNCTION [Reports].[MemorySummary] 
(
    @sessionId varchar(50)
)
RETURNS 
@returnTable TABLE 
(
	SnapshotDateTime datetime, 
	DeviceId nvarchar(50),
    ProductName nvarchar(255),
    SnapshotLabel nvarchar(50),
    CategoryName varchar(255),
    DataValue bigint,
    Reason varchar(10),
    [Percent Growth] float
)
AS
BEGIN
    ;WITH MemoryTable AS
    (
        SELECT dms.SnapshotDateTime
              ,dms.DeviceId
              ,sd.ProductName
              ,dms.SnapshotLabel
              ,DENSE_RANK() OVER (PARTITION BY dms.DeviceId ORDER BY dms.SnapshotDateTime) - 1 AS ProductIteration
              ,RANK() OVER (PARTITION BY dms.DeviceId, dmc.CategoryName ORDER BY dmc.DataValue DESC, dms.SnapshotDateTime ASC) AS MemRank
              ,REPLACE(dmc.CategoryName, ''JediMem_'', '''') AS CategoryName
              ,dmc.DataValue
              ,dms.DeviceMemorySnapshotId
        FROM SessionDevice sd WITH (NOLOCK)
        INNER JOIN DeviceMemorySnapshot dms WITH (NOLOCK) ON dms.SessionId = sd.SessionId AND dms.DeviceId = sd.DeviceId
        INNER JOIN DeviceMemoryCount dmc WITH (NOLOCK) ON dms.DeviceMemorySnapshotId = dmc.DeviceMemorySnapshotId
        where dms.SessionId IN (@sessionId) AND
              dmc.CategoryName IN (''JediMem_NonJediManaged'', ''JediMem_WebkitSmall'', ''JediMem_WebkitLarge'', ''JediMem_JavaScriptAligned'') AND
              dmc.DataLabel = ''HighWater''
    )
    INSERT INTO @returnTable (SnapshotDateTime, DeviceId, ProductName, SnapshotLabel, CategoryName, DataValue, Reason, [Percent Growth])
    SELECT mt.SnapshotDateTime
          ,mt.DeviceId
          ,mt.ProductName
          ,mt.SnapshotLabel
          ,mt.CategoryName
          ,mt.DataValue
          ,CASE
               WHEN mt.MemRank = 1 THEN ''peak''
               ELSE ''base''
           END AS Reason
          ,(CAST((mt.DataValue - mt2.BaseValue) AS FLOAT) / mt2.BaseValue) AS PercentGrowth
    FROM MemoryTable mt
    JOIN (SELECT DeviceId
                ,CategoryName
                ,DataValue AS BaseValue
          FROM MemoryTable
          WHERE ProductIteration = 2) AS mt2 ON mt.DeviceId = mt2.DeviceId AND mt.CategoryName = mt2.CategoryName
    WHERE mt.MemRank = 1
    ORDER BY mt.ProductName, mt.DeviceId, mt.CategoryName, Reason
	
	RETURN 
END
' 
END
GO

/******  Object:  Function [Reports].[PrintServerOS]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[PrintServerOS]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns the server operating system as a string based on the OS number.
-- =============================================
CREATE FUNCTION [Reports].[PrintServerOS] 
(
	@printServerOS nvarchar(50)
)
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @result nvarchar(50)

	SELECT @result = CASE SUBSTRING(@printServerOS, CHARINDEX(''.'', @printServerOS) - 2, 4)
                         WHEN ''10.0'' THEN ''Windows Server 2016''
                         WHEN '' 6.3'' THEN ''Windows Server 2012 R2''
                         WHEN '' 6.2'' THEN ''Windows Server 2012''
                         WHEN '' 6.1'' THEN ''Windows Server 2008 R2''
                         WHEN '' 6.0'' THEN ''Windows Server 2008''
                         WHEN '' 5.2'' THEN ''Windows Server 2003''
                         ELSE @printServerOS
                     END

	RETURN @result
END
' 
END
GO

/******  Object:  Function [Reports].[SessionInformation]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[SessionInformation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns basic information about a session.
-- =============================================
CREATE FUNCTION [Reports].[SessionInformation] 
(
	@sessionId varchar(50)
)
RETURNS 
@resultTable TABLE 
(
	SessionId varchar(50),
    Cycle nvarchar(255),
    [Session Name] nvarchar(255),
    [Owner] nvarchar(50),
    Dispatcher nvarchar(50),
    [Activities Start Time] datetime,
    [Activities End Time] datetime,
    [Activities Length] float,
    [Status] varchar(20),
    Tags nvarchar(255),
    [STF Version] nvarchar(50),
    [Type] nvarchar(255),
    Reference nvarchar(255),
    [Firmware Date] nvarchar(20),
    [Firmware Build] nvarchar(50),
    [Device Count] int,
    [Activity Count] int,
    Vendor varchar(50),
    Solution varchar(50),
    [Solution Version] nvarchar(1024)
)
AS
BEGIN
    INSERT INTO @resultTable (SessionId, Cycle, [Session Name], [Owner], Dispatcher, [Activities Start Time],
                              [Activities End Time], [Activities Length], [Status], Tags, [STF Version], [Type],
                              Reference, [Firmware Date], [Firmware Build], [Device Count], [Activity Count], Vendor,
                              Solution, [Solution Version])
    SELECT prodcom.SessionId
          ,ss.Cycle
          ,ss.SessionName
          ,ss.Owner
          ,ss.Dispatcher
          ,MIN(ae.StartDateTime) AS ActivitiesStartTime
          ,MAX(ae.EndDateTime) AS ActivitiesEndTime
          ,Reports.TimeDiffInDays(MIN(ae.StartDateTime), MAX(ae.EndDateTime)) AS ActivitiesLength
          ,ss.Status
          ,ISNULL(ss.Tags, '''') AS Tags
          ,ss.STFVersion
          ,ss.Type
          ,ss.Reference
          ,devfw.FirmwareDate
          ,devfw.FirmwareBuild
          ,COUNT(DISTINCT sd.DeviceId) AS DeviceCount
          ,COUNT(ae.ActivityExecutionId) AS ActivityCount
          ,prodcom.Vendor
          ,prodcom.Name as Solution
          ,prodcom.Version as SolutionVersion
    FROM (SELECT DISTINCT ISNULL(spa.SessionId, aprod.SessionId) AS SessionId
                ,ISNULL(spa.Name, aprod.Name) AS [Name]
                ,ISNULL(spa.Vendor, aprod.Vendor) AS Vendor
                ,spa.Version
          FROM SessionProductAssoc spa WITH (NOLOCK)
          fULL JOIN (SELECT DISTINCT ae.SessionId
                           ,aesu.ServerName
                           ,Reports.ActivityName(ae.ActivityType, aesu.ServerName) AS [Name]
                           ,Reports.ActivityVendor(ae.ActivityType) AS Vendor
                     FROM ActivityExecution ae WITH (NOLOCK)
                     LEFT OUTER JOIN ActivityExecutionServerUsage aesu WITH (NOLOCK) ON ae.ActivityExecutionId = aesu.ActivityExecutionId
                     WHERE ae.SessionId in (@sessionId) AND
                           ae.ActivityType NOT IN (''CollectDeviceSystemInfo'', ''Authentication'') AND
                           ae.Status IN (''Passed'', ''Failed'')) AS aprod ON spa.SessionId = aprod.SessionId AND spa.Name = aprod.Name
          WHERE spa.SessionId IN (@sessionId) OR spa.SessionId IS NULL) AS prodcom
    JOIN SessionSummary ss WITH (NOLOCK) ON prodcom.SessionId = ss.SessionId
    JOIN ActivityExecution ae WITH (NOLOCK) ON ae.SessionId = ss.SessionId
    JOIN (SELECT SessionId
                ,MAX(FirmwareDatecode) AS FirmwareDate
                ,SUBSTRING(MAX(FirmwareRevision), 0, CHARINDEX(''_'', MAX(FirmwareRevision))) AS FirmwareBuild
          FROM SessionDevice WITH (NOLOCK)
          WHERE SessionId in (@sessionId)
          GROUP BY SessionId) AS devfw ON devfw.SessionId = ss.SessionId
    LEFT JOIN SessionDevice sd WITH (NOLOCK) ON ss.SessionId = sd.SessionId
    WHERE prodcom.Name IN (''AutoStore'', ''Embedded DS'', ''Equitrac'', ''HPAC'', ''SafeCom'', ''Blueprint'', ''PaperCut'', ''SafeQ'', ''Celiveo'', ''HPRoam'', ''JALink'')
    GROUP BY prodcom.SessionId, ss.Cycle, ss.SessionName, devfw.FirmwareBuild, devfw.FirmwareDate, prodcom.Name,
             prodcom.Vendor, prodcom.Version, ss.Tags, ss.STFVersion, ss.Owner, ss.Status, ss.Type, ss.Reference, ss.Dispatcher

	RETURN 
END
' 
END
GO

/******  Object:  Function [Reports].[TimeDiffInDays]  ******/
USE [EnterpriseTestODS]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Reports].[TimeDiffInDays]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		report_designer
-- Create date: 12/11/2018
-- Description:	Returns the difference between two datetime values as a
--              fraction of a day.
-- =============================================
CREATE FUNCTION [Reports].[TimeDiffInDays] 
(
	@startDateTime datetime,
    @endDateTime datetime
)
RETURNS float
AS
BEGIN
	DECLARE @result float

	SELECT @result = DATEDIFF(MILLISECOND, @startDateTime, @endDateTime) / 86400000.0

	RETURN @result
END
' 
END
GO

/*****************************************************************************
 * VIEWS                                                                     *
 *****************************************************************************/

/*****************************************************************************
 * TRIGGERS                                                                  *
 *****************************************************************************/

/*****************************************************************************
 * USERS                                                                     *
 *****************************************************************************/

/******  Object:  User [dbscripter]  ******/
USE [EnterpriseTestODS]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'dbscripter')
CREATE USER [dbscripter] FOR LOGIN [dbscripter] WITH DEFAULT_SCHEMA=[dbo]
GO
