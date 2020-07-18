
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[ActivityExecution]  ******/
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
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
USE [ScalableTestDataLog]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExecution]') AND name = N'Idx_ActivityExecution_SessionId')
CREATE NONCLUSTERED INDEX [Idx_ActivityExecution_SessionId] ON [dbo].[ActivityExecution]
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/******  Object:  Foreign Key [FK_ActivityExecution_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecution_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecution]'))
ALTER TABLE [dbo].[ActivityExecution]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecution_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecution_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecution]'))
ALTER TABLE [dbo].[ActivityExecution] CHECK CONSTRAINT [FK_ActivityExecution_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionAssetUsage_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionAssetUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionAssetUsage]'))
ALTER TABLE [dbo].[ActivityExecutionAssetUsage]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionAssetUsage_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionAssetUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionAssetUsage]'))
ALTER TABLE [dbo].[ActivityExecutionAssetUsage] CHECK CONSTRAINT [FK_ActivityExecutionAssetUsage_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionDetail_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionDetail_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDetail]'))
ALTER TABLE [dbo].[ActivityExecutionDetail]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionDetail_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionDetail_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDetail]'))
ALTER TABLE [dbo].[ActivityExecutionDetail] CHECK CONSTRAINT [FK_ActivityExecutionDetail_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionDocumentUsage_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionDocumentUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDocumentUsage]'))
ALTER TABLE [dbo].[ActivityExecutionDocumentUsage]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionDocumentUsage_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionDocumentUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionDocumentUsage]'))
ALTER TABLE [dbo].[ActivityExecutionDocumentUsage] CHECK CONSTRAINT [FK_ActivityExecutionDocumentUsage_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionPerformance_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionPerformance_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]'))
ALTER TABLE [dbo].[ActivityExecutionPerformance]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionPerformance_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionPerformance]'))
ALTER TABLE [dbo].[ActivityExecutionPerformance] CHECK CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionRetries_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionRetries_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionRetries]'))
ALTER TABLE [dbo].[ActivityExecutionRetries]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionRetries_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionRetries_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionRetries]'))
ALTER TABLE [dbo].[ActivityExecutionRetries] CHECK CONSTRAINT [FK_ActivityExecutionRetries_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ActivityExecutionServerUsage_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionServerUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionServerUsage]'))
ALTER TABLE [dbo].[ActivityExecutionServerUsage]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionServerUsage_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActivityExecutionServerUsage_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActivityExecutionServerUsage]'))
ALTER TABLE [dbo].[ActivityExecutionServerUsage] CHECK CONSTRAINT [FK_ActivityExecutionServerUsage_SessionSummary]
GO

/******  Object:  Foreign Key [FK_ConnectorJobInput_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ConnectorJobInput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ConnectorJobInput]'))
ALTER TABLE [dbo].[ConnectorJobInput]  WITH CHECK ADD  CONSTRAINT [FK_ConnectorJobInput_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ConnectorJobInput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ConnectorJobInput]'))
ALTER TABLE [dbo].[ConnectorJobInput] CHECK CONSTRAINT [FK_ConnectorJobInput_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DeviceEvent_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceEvent_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceEvent]'))
ALTER TABLE [dbo].[DeviceEvent]  WITH CHECK ADD  CONSTRAINT [FK_DeviceEvent_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceEvent_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceEvent]'))
ALTER TABLE [dbo].[DeviceEvent] CHECK CONSTRAINT [FK_DeviceEvent_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DeviceMemoryCount_DeviceMemorySnapshot]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemoryCount_DeviceMemorySnapshot]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemoryCount]'))
ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemoryCount_DeviceMemorySnapshot]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemoryCount]'))
ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot]
GO

/******  Object:  Foreign Key [FK_DeviceMemorySnapshot_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemorySnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemorySnapshot]'))
ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemorySnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemorySnapshot]'))
ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [FK_DeviceMemorySnapshot_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DeviceMemoryXml_DeviceMemorySnapshot]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemoryXml_DeviceMemorySnapshot]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemoryXml]'))
ALTER TABLE [dbo].[DeviceMemoryXml]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryXml_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceMemoryXml_DeviceMemorySnapshot]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceMemoryXml]'))
ALTER TABLE [dbo].[DeviceMemoryXml] CHECK CONSTRAINT [FK_DeviceMemoryXml_DeviceMemorySnapshot]
GO

/******  Object:  Foreign Key [FK_DigitalSendJobInput_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobInput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]'))
ALTER TABLE [dbo].[DigitalSendJobInput]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobInput_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobInput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobInput]'))
ALTER TABLE [dbo].[DigitalSendJobInput] CHECK CONSTRAINT [FK_DigitalSendJobInput_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DigitalSendJobNotification_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobNotification_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobNotification]'))
ALTER TABLE [dbo].[DigitalSendJobNotification]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobNotification_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobNotification_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobNotification]'))
ALTER TABLE [dbo].[DigitalSendJobNotification] CHECK CONSTRAINT [FK_DigitalSendJobNotification_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DigitalSendJobOutput_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobOutput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobOutput]'))
ALTER TABLE [dbo].[DigitalSendJobOutput]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobOutput_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobOutput_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobOutput]'))
ALTER TABLE [dbo].[DigitalSendJobOutput] CHECK CONSTRAINT [FK_DigitalSendJobOutput_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DigitalSendJobTempFile_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobTempFile_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobTempFile]'))
ALTER TABLE [dbo].[DigitalSendJobTempFile]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobTempFile_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendJobTempFile_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendJobTempFile]'))
ALTER TABLE [dbo].[DigitalSendJobTempFile] CHECK CONSTRAINT [FK_DigitalSendJobTempFile_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DigitalSendServerJob_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendServerJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendServerJob]'))
ALTER TABLE [dbo].[DigitalSendServerJob]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendServerJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendServerJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendServerJob]'))
ALTER TABLE [dbo].[DigitalSendServerJob] CHECK CONSTRAINT [FK_DigitalSendServerJob_SessionSummary]
GO

/******  Object:  Foreign Key [FK_DigitalSendTempSnapshot_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendTempSnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendTempSnapshot]'))
ALTER TABLE [dbo].[DigitalSendTempSnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendTempSnapshot_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DigitalSendTempSnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[DigitalSendTempSnapshot]'))
ALTER TABLE [dbo].[DigitalSendTempSnapshot] CHECK CONSTRAINT [FK_DigitalSendTempSnapshot_SessionSummary]
GO

/******  Object:  Foreign Key [FK_EPrintServerJob_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EPrintServerJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[EPrintServerJob]'))
ALTER TABLE [dbo].[EPrintServerJob]  WITH CHECK ADD  CONSTRAINT [FK_EPrintServerJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EPrintServerJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[EPrintServerJob]'))
ALTER TABLE [dbo].[EPrintServerJob] CHECK CONSTRAINT [FK_EPrintServerJob_SessionSummary]
GO

/******  Object:  Foreign Key [FK_JetAdvantageLinkDeviceMemoryCount_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageLinkDeviceMemoryCount_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemoryCount]'))
ALTER TABLE [dbo].[JetAdvantageLinkDeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantageLinkDeviceMemoryCount_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageLinkDeviceMemoryCount_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemoryCount]'))
ALTER TABLE [dbo].[JetAdvantageLinkDeviceMemoryCount] CHECK CONSTRAINT [FK_JetAdvantageLinkDeviceMemoryCount_SessionSummary]
GO

/******  Object:  Foreign Key [FK_JetAdvantageLinkDeviceMemorySnapshot_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageLinkDeviceMemorySnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemorySnapshot]'))
ALTER TABLE [dbo].[JetAdvantageLinkDeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantageLinkDeviceMemorySnapshot_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageLinkDeviceMemorySnapshot_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageLinkDeviceMemorySnapshot]'))
ALTER TABLE [dbo].[JetAdvantageLinkDeviceMemorySnapshot] CHECK CONSTRAINT [FK_JetAdvantageLinkDeviceMemorySnapshot_SessionSummary]
GO

/******  Object:  Foreign Key [FK_JetAdvantagePullPrintJobRetrieval_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantagePullPrintJobRetrieval_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantagePullPrintJobRetrieval]'))
ALTER TABLE [dbo].[JetAdvantagePullPrintJobRetrieval]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantagePullPrintJobRetrieval_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantagePullPrintJobRetrieval_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantagePullPrintJobRetrieval]'))
ALTER TABLE [dbo].[JetAdvantagePullPrintJobRetrieval] CHECK CONSTRAINT [FK_JetAdvantagePullPrintJobRetrieval_SessionSummary]
GO

/******  Object:  Foreign Key [FK_JetAdvantageUpload_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageUpload_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageUpload]'))
ALTER TABLE [dbo].[JetAdvantageUpload]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantageUpload_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JetAdvantageUpload_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[JetAdvantageUpload]'))
ALTER TABLE [dbo].[JetAdvantageUpload] CHECK CONSTRAINT [FK_JetAdvantageUpload_SessionSummary]
GO

/******  Object:  Foreign Key [FK_PhysicalDeviceJob_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PhysicalDeviceJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhysicalDeviceJob]'))
ALTER TABLE [dbo].[PhysicalDeviceJob]  WITH CHECK ADD  CONSTRAINT [FK_PhysicalDeviceJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PhysicalDeviceJob_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhysicalDeviceJob]'))
ALTER TABLE [dbo].[PhysicalDeviceJob] CHECK CONSTRAINT [FK_PhysicalDeviceJob_SessionSummary]
GO

/******  Object:  Foreign Key [FK_PrintJobClient_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintJobClient_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintJobClient]'))
ALTER TABLE [dbo].[PrintJobClient]  WITH CHECK ADD  CONSTRAINT [FK_PrintJobClient_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintJobClient_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintJobClient]'))
ALTER TABLE [dbo].[PrintJobClient] CHECK CONSTRAINT [FK_PrintJobClient_SessionSummary]
GO

/******  Object:  Foreign Key [FK_PrintServerJob_PrintJobClient]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintServerJob_PrintJobClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintServerJob]'))
ALTER TABLE [dbo].[PrintServerJob]  WITH CHECK ADD  CONSTRAINT [FK_PrintServerJob_PrintJobClient] FOREIGN KEY([PrintJobClientId])
REFERENCES [dbo].[PrintJobClient] ([PrintJobClientId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintServerJob_PrintJobClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintServerJob]'))
ALTER TABLE [dbo].[PrintServerJob] CHECK CONSTRAINT [FK_PrintServerJob_PrintJobClient]
GO

/******  Object:  Foreign Key [FK_PullPrintJobRetrieval_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PullPrintJobRetrieval_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PullPrintJobRetrieval]'))
ALTER TABLE [dbo].[PullPrintJobRetrieval]  WITH CHECK ADD  CONSTRAINT [FK_PullPrintJobRetrieval_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PullPrintJobRetrieval_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[PullPrintJobRetrieval]'))
ALTER TABLE [dbo].[PullPrintJobRetrieval] CHECK CONSTRAINT [FK_PullPrintJobRetrieval_SessionSummary]
GO

/******  Object:  Foreign Key [FK_SessionDevice_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionDevice_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionDevice]'))
ALTER TABLE [dbo].[SessionDevice]  WITH CHECK ADD  CONSTRAINT [FK_SessionDevice_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionDevice_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionDevice]'))
ALTER TABLE [dbo].[SessionDevice] CHECK CONSTRAINT [FK_SessionDevice_SessionSummary]
GO

/******  Object:  Foreign Key [FK_SessionDocument_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionDocument_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionDocument]'))
ALTER TABLE [dbo].[SessionDocument]  WITH CHECK ADD  CONSTRAINT [FK_SessionDocument_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionDocument_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionDocument]'))
ALTER TABLE [dbo].[SessionDocument] CHECK CONSTRAINT [FK_SessionDocument_SessionSummary]
GO

/******  Object:  Foreign Key [FK_SessionProductAssoc_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionProductAssoc_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionProductAssoc]'))
ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionProductAssoc_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionProductAssoc]'))
ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_SessionSummary]
GO

/******  Object:  Foreign Key [FK_SessionConfiguration_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionConfiguration_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionScenario]'))
ALTER TABLE [dbo].[SessionScenario]  WITH CHECK ADD  CONSTRAINT [FK_SessionConfiguration_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionConfiguration_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionScenario]'))
ALTER TABLE [dbo].[SessionScenario] CHECK CONSTRAINT [FK_SessionConfiguration_SessionSummary]
GO

/******  Object:  Foreign Key [FK_SessionServer_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionServer_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionServer]'))
ALTER TABLE [dbo].[SessionServer]  WITH CHECK ADD  CONSTRAINT [FK_SessionServer_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SessionServer_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[SessionServer]'))
ALTER TABLE [dbo].[SessionServer] CHECK CONSTRAINT [FK_SessionServer_SessionSummary]
GO

/******  Object:  Foreign Key [FK_TriageData_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TriageData_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[TriageData]'))
ALTER TABLE [dbo].[TriageData]  WITH CHECK ADD  CONSTRAINT [FK_TriageData_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TriageData_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[TriageData]'))
ALTER TABLE [dbo].[TriageData] CHECK CONSTRAINT [FK_TriageData_SessionSummary]
GO

/******  Object:  Foreign Key [FK_TriageDataJetAdvantageLink_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TriageDataJetAdvantageLink_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[TriageDataJetAdvantageLink]'))
ALTER TABLE [dbo].[TriageDataJetAdvantageLink]  WITH CHECK ADD  CONSTRAINT [FK_TriageDataJetAdvantageLink_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TriageDataJetAdvantageLink_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[TriageDataJetAdvantageLink]'))
ALTER TABLE [dbo].[TriageDataJetAdvantageLink] CHECK CONSTRAINT [FK_TriageDataJetAdvantageLink_SessionSummary]
GO

/******  Object:  Foreign Key [FK_VirtualPrinterJob_PrintJobClient]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualPrinterJob_PrintJobClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualPrinterJob]'))
ALTER TABLE [dbo].[VirtualPrinterJob]  WITH CHECK ADD  CONSTRAINT [FK_VirtualPrinterJob_PrintJobClient] FOREIGN KEY([PrintJobClientId])
REFERENCES [dbo].[PrintJobClient] ([PrintJobClientId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualPrinterJob_PrintJobClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualPrinterJob]'))
ALTER TABLE [dbo].[VirtualPrinterJob] CHECK CONSTRAINT [FK_VirtualPrinterJob_PrintJobClient]
GO

/******  Object:  Foreign Key [FK_VirtualResourceInstanceStatus_SessionSummary]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceInstanceStatus_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceInstanceStatus]'))
ALTER TABLE [dbo].[VirtualResourceInstanceStatus]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceInstanceStatus_SessionSummary]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceInstanceStatus]'))
ALTER TABLE [dbo].[VirtualResourceInstanceStatus] CHECK CONSTRAINT [FK_VirtualResourceInstanceStatus_SessionSummary]
GO

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/******  Object:  StoredProcedure [dbo].[del_SessionData]  ******/
USE [ScalableTestDataLog]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[del_SessionData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[del_SessionData] AS' 
END
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 1/24/2013
-- Description:	Deletes all session data for the given session id.
-- Updated:		
--	 6/4/2014 - Added removal of TraceLog data
--  10/3/2014 - Revise TraceLog data cleanup
--  6/15/2017 - Walk through a few of the larger tables and delete 10,000 records
--              at a time to reduce the possibility of filling up the tran log.
--   9/1/2017 - Removed TraceLog table
--  12/5/2018 - Adjusted for new db replication process.
-- =============================================
ALTER PROCEDURE [dbo].[del_SessionData] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

    -- DeviceMemoryXml
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[DeviceMemoryXml]'))
    BEGIN
        SELECT TOP 1 * FROM DeviceMemoryXml
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM DeviceMemoryXml
            WHERE EXISTS (SELECT *
                          FROM DeviceMemorySnapshot dms
                          WHERE dms.DeviceMemorySnapshotId = DeviceMemoryXml.DeviceMemorySnapshotId AND dms.SessionId = @sessionId)
        END
    END

    -- DirectorySnapshot
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[DirectorySnapshot]'))
    BEGIN
        SELECT TOP 1 * FROM DirectorySnapshot
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (10000) FROM DirectorySnapshot
            WHERE SnapshotDateTime < DATEADD(DAY, -90, DirectorySnapshot.SnapshotDateTime)
        END
    END

    -- TriageData
    IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'[dbo].[TriageData]'))
    BEGIN
        SELECT TOP 1 * FROM TriageData
        WHILE (@@ROWCOUNT > 0)
        BEGIN
            DELETE TOP (1000) FROM TriageData
            WHERE SessionId = @sessionId
        END
    END
END
GO

/******  Object:  StoredProcedure [dbo].[sel_AuthenticationTimesEager]  ******/
USE [ScalableTestDataLog]
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

if object_id('tempdb..##tempTable') is not null
	drop table ##tempTable


create table ##tempTable
(
	ID int identity (1,1) primary key,
	ProductName varchar(50),
	FirmwareRevision varchar(50),
	FirmwareDatecode varchar(50),
	SolutionAuthType varchar(50),
	EagerAuth decimal(10,3),
	EagerAuthMin decimal(10,3),
	EagerAuthMax decimal(10,3),
	EagerAuthCount int,
	SessionIds varchar(MAX)
)
insert into ##tempTable(ProductName, FirmwareRevision, FirmwareDatecode, SolutionAuthType, EagerAuth,EagerAuthMin, EagerAuthMax, EagerAuthCount)
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
	order by ProductName, FirmwareDatecode, SolutionAuthType

	update ##tempTable
	set SessionIds =  STUFF((
		select distinct ', ' + sd.SessionId 
		from SessionDevice sd with (nolock)
			inner join ActivityExecution ae with (nolock) on sd.SessionId = ae.SessionId
			inner join ActivityExecutionDetail aed with (nolock) on ae.SessionId = aed.SessionId
		where ae.StartDateTime > @startDateTime
			and ##tempTable.FirmwareRevision = sd.FirmwareRevision
			and CHARINDEX(aed.Message, ##tempTable.SolutionAuthType) > 0
		FOR XML PATH('')), 1,2, '')

	select * from ##tempTable

END
GO

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/*****************************************************************************
 * VIEWS                                                                     *
 *****************************************************************************/

/*****************************************************************************
 * TRIGGERS                                                                  *
 *****************************************************************************/

/******  Object:  Trigger [SchemaChange_LogInTable_DDL]  ******/
USE [ScalableTestDataLog]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'SchemaChange_LogInTable_DDL')
EXECUTE dbo.sp_executesql N'


create trigger [SchemaChange_LogInTable_DDL] on database for CREATE_TABLE, ALTER_TABLE, DROP_TABLE as
    declare @eventInfo XML

    set @eventInfo = EVENTDATA()

    insert into DBAdmin.dbo.DBSchema_Change_Log (EventTime, LoginName, UserName, DatabaseName, SchemaName, ObjectName, ObjectType, DDLCommand)
    values (
        REPLACE(CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/PostTime)'')), ''T'', '' ''),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/LoginName)'')),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/UserName)'')),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/DatabaseName)'')),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/SchemaName)'')),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/ObjectName)'')),
        CONVERT(varchar(50), @eventInfo.query(''data(/EVENT_INSTANCE/ObjectType)'')),
        CONVERT(varchar(MAX), @eventInfo.query(''data(/EVENT_INSTANCE/TSQLCommand/CommandText)''))
    )
'
GO
ENABLE TRIGGER [SchemaChange_LogInTable_DDL] ON DATABASE
GO

/*****************************************************************************
 * USERS                                                                     *
 *****************************************************************************/

/******  Object:  User [enterprise_data]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'enterprise_data')
CREATE USER [enterprise_data] FOR LOGIN [enterprise_data] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [enterprise_data]
GO

/******  Object:  User [ETL\jawa]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ETL\jawa')
CREATE USER [ETL\jawa] FOR LOGIN [ETL\jawa] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ETL\jawa]
GO

/******  Object:  User [ETL\odsrepl]  ******/
USE [ScalableTestDataLog]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ETL\odsrepl')
CREATE USER [ETL\odsrepl] FOR LOGIN [ETL\odsrepl] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ETL\odsrepl]
GO
