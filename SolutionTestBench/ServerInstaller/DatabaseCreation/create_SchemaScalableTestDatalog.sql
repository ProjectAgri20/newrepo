USE [ScalableTestDataLog]
GO
/****** Object:  Schema [Reports]    Script Date: 9/24/2015 3:56:33 PM ******/
CREATE SCHEMA [Reports]
GO
/****** Object:  Table [dbo].[ActivityExecution]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExecution](
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[ResourceMetadataId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ResourceInstanceId] [nvarchar](50) NOT NULL,
	[ActivityName] [nvarchar](255) NULL,
	[ActivityType] [varchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[UpdateType] [varchar](50) NOT NULL,
	[RetryCount] [int] NULL,
	[ErrorMessage] [varchar](1024) NULL,
 CONSTRAINT [PK_ActivityExecution] PRIMARY KEY NONCLUSTERED 
(
	[ActivityExecutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ActivityExecutionDeviceUsage]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExecutionDeviceUsage](
	[ActivityExecutionDeviceUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[DeviceId] [varchar](50) NOT NULL,
	[Product] [varchar](50) NULL,
	[FirmwareVersion] [varchar](50) NULL,
 CONSTRAINT [PK_ActivityExecutionDeviceUsage] PRIMARY KEY NONCLUSTERED 
(
	[ActivityExecutionDeviceUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ActivityExecutionPacing]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExecutionPacing](
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [varchar](50) NULL,
	[Name] [varchar](255) NULL,
	[Index] [int] NULL,
	[TotalDurationSec] [real] NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[ActivityRunTimeSec] [real] NULL,
	[ElapsedTimeSec] [real] NULL,
	[RemainingTimeSec] [real] NULL,
	[TotalRunTimeSec] [real] NULL,
	[RemainingActivitySets] [real] NULL,
	[RemainingPacingSec] [real] NULL,
	[RemainingPacingSlots] [int] NULL,
	[AppliedDelaySec] [real] NULL,
	[EstimatedEndDateTime] [datetime] NULL,
 CONSTRAINT [PK_ActivityExecutionPacing] PRIMARY KEY NONCLUSTERED 
(
	[ActivityExecutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ActivityExecutionPerformance]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExecutionPerformance](
	[ActivityExecutionPerformanceId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[EventLabel] [varchar](50) NULL,
	[EventIndex] [int] NULL,
	[EventDateTime] [datetime] NULL,
 CONSTRAINT [Pk_ActivityExecutionPerformance] PRIMARY KEY NONCLUSTERED 
(
	[ActivityExecutionPerformanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ActivityExecutionServerUsage]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExecutionServerUsage](
	[ActivityExecutionServerUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[ServerName] [varchar](100) NULL,
 CONSTRAINT [PK_ActivityExecutionServerUsage] PRIMARY KEY NONCLUSTERED 
(
	[ActivityExecutionServerUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DemoActivityData]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DemoActivityData](
	[DemoActivityDataId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ThreadId] [int] NULL,
	[Name] [varchar](1024) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
 CONSTRAINT [PK_DemoActivityData] PRIMARY KEY NONCLUSTERED 
(
	[DemoActivityDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DeviceEvent]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeviceEvent](
	[DeviceEventId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[AssetId] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EventDateTime] [datetime] NULL,
	[EventType] [varchar](50) NULL,
	[EventCode] [varchar](50) NULL,
	[EventDescription] [varchar](50) NULL,
 CONSTRAINT [PK_DeviceEvent] PRIMARY KEY NONCLUSTERED 
(
	[DeviceEventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendJobInput]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendJobInput](
	[DigitalSendJobInputId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[FilePrefix] [varchar](255) NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Sender] [varchar](50) NULL,
	[DeviceId] [varchar](50) NULL,
	[ScanType] [varchar](50) NULL,
	[ActivityStartDateTime] [datetime] NULL,
	[ScanStartDateTime] [datetime] NULL,
	[ScanEndDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [varchar](50) NULL,
	[PageCount] [int] NULL,
	[DestinationCount] [int] NOT NULL,
	[Ocr] [bit] NULL,
 CONSTRAINT [PK_DigitalSendJobInput] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendJobInputId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendJobNotification]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendJobNotification](
	[DigitalSendJobNotificationId] [uniqueidentifier] NOT NULL,
	[FilePrefix] [varchar](255) NULL,
	[SessionId] [varchar](50) NOT NULL,
	[NotificationDestination] [varchar](255) NULL,
	[NotificationReceivedDateTime] [datetime] NULL,
	[NotificationResult] [varchar](50) NULL,
 CONSTRAINT [PK_DigitalSendJobNotification] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendJobNotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendJobOutput]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendJobOutput](
	[DigitalSendJobOutputId] [uniqueidentifier] NOT NULL,
	[FilePrefix] [varchar](255) NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FileName] [varchar](255) NULL,
	[FileLocation] [varchar](255) NULL,
	[FileSentDateTime] [datetime] NULL,
	[FileReceivedDateTime] [datetime] NULL,
	[FileSizeBytes] [bigint] NULL,
	[PageCount] [int] NULL,
	[ErrorMessage] [varchar](1024) NULL,
 CONSTRAINT [PK_DigitalSendJobOutput] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendJobOutputId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendJobTempFile]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendJobTempFile](
	[DigitalSendJobTempFileId] [uniqueidentifier] NOT NULL,
	[DigitalSendJobId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FirstFileInDateTime] [datetime] NULL,
	[RenameDateTime] [datetime] NULL,
	[LastFileOutDateTime] [datetime] NULL,
	[TotalFiles] [int] NULL,
	[TotalBytes] [int] NULL,
 CONSTRAINT [PK_DigitalSendJobTempFile] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendJobTempFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendServerJob]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendServerJob](
	[DigitalSendServerJobId] [uniqueidentifier] NOT NULL,
	[DigitalSendJobId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[JobType] [varchar](50) NULL,
	[CompletionDateTime] [datetime] NULL,
	[CompletionStatus] [varchar](50) NULL,
	[FileType] [varchar](50) NULL,
	[FileName] [varchar](255) NULL,
	[FileSizeBytes] [int] NULL,
	[ScannedPages] [int] NULL,
	[DeviceModel] [varchar](50) NULL,
	[DssVersion] [varchar](50) NULL,
	[ProcessedBy] [varchar](50) NULL,
 CONSTRAINT [PK_DigitalSendServerJob] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DigitalSendTempSnapshot]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendTempSnapshot](
	[DigitalSendTempSnapshotId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[HostName] [varchar](50) NULL,
	[SnapshotDateTime] [datetime] NULL,
	[TotalBytes] [bigint] NULL,
	[TotalFiles] [bigint] NULL,
	[TotalJobs] [bigint] NULL,
 CONSTRAINT [PK_DigitalSendTempSnapshot] PRIMARY KEY NONCLUSTERED 
(
	[DigitalSendTempSnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DirectorySnapshot]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DirectorySnapshot](
	[DirectorySnapshotId] [uniqueidentifier] NOT NULL,
	[SnapshotDateTime] [datetime] NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[DirectoryName] [varchar](max) NOT NULL,
	[TotalFiles] [int] NOT NULL,
	[TotalBytes] [bigint] NOT NULL,
 CONSTRAINT [PK_DirectorySnapshot] PRIMARY KEY NONCLUSTERED 
(
	[DirectorySnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[EPrintServerJob]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EPrintServerJob](
	[EPrintServerJobId] [bigint] NOT NULL,
	[EPrintJobId] [int] NULL,
	[PrintJobClientId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[EmailAccount] [varchar](50) NULL,
	[PrinterName] [varchar](50) NULL,
	[JobName] [varchar](255) NULL,
	[JobFolderId] [varchar](255) NULL,
	[EmailReceivedDateTime] [datetime] NULL,
	[JobStartDateTime] [datetime] NULL,
	[LastStatusDateTime] [datetime] NULL,
	[JobStatus] [varchar](50) NULL,
	[EPrintTransactionId] [int] NULL,
	[TransactionStatus] [varchar](50) NULL,
 CONSTRAINT [PK_EPrintServerJob] PRIMARY KEY NONCLUSTERED 
(
	[EPrintServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[JetAdvantagePullPrintJobRetrieval]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[JetAdvantagePullPrintJobRetrieval](
	[JetAdvantagePullPrintJobRetrievalId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [varchar](50) NULL,
	[JetAdvantageLoginId] [varchar](50) NULL,
	[DeviceId] [varchar](50) NULL,
	[SolutionType] [varchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [varchar](50) NULL,
	[ErrorMessage] [varchar](1024) NULL,
 CONSTRAINT [PK_JetAdvantagePullPrintJobRetrieval] PRIMARY KEY NONCLUSTERED 
(
	[JetAdvantagePullPrintJobRetrievalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[JetAdvantageUpload]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[JetAdvantageUpload](
	[JetAdvantageUploadId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[LoginId] [varchar](50) NULL,
	[DestinationUrl] [varchar](50) NULL,
	[FileName] [varchar](255) NULL,
	[FileType] [varchar](50) NULL,
	[FileSentDateTime] [datetime] NULL,
	[FileSizeSentBytes] [bigint] NULL,
	[FileReceivedDateTime] [datetime] NULL,
	[FileSizeReceivedBytes] [bigint] NULL,
	[CompletionDateTime] [datetime] NULL,
	[CompletionStatus] [varchar](50) NULL,
 CONSTRAINT [PK_JetAdvantageUpload] PRIMARY KEY NONCLUSTERED 
(
	[JetAdvantageUploadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[PhysicalDeviceJob]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhysicalDeviceJob](
	[PhysicalDeviceJobId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[DeviceId] [varchar](50) NULL,
	[JobId] [varchar](50) NULL,
	[JobName] [varchar](255) NULL,
	[JobApplicationName] [varchar](255) NULL,
	[JobCategory] [varchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [varchar](50) NULL,
	[MonitorStartDateTime] [datetime] NULL,
	[MonitorEndDateTime] [datetime] NULL,
 CONSTRAINT [PK_PhysicalDeviceJob] PRIMARY KEY NONCLUSTERED 
(
	[PhysicalDeviceJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[PinPrintJobRetrieval]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PinPrintJobRetrieval](
	[PinPrintJobRetrievalId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Sender] [varchar](50) NULL,
	[DocumentName] [varchar](50) NULL,
	[FileName] [varchar](50) NULL,
	[DeviceId] [varchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [varchar](50) NULL,
 CONSTRAINT [PK_PinPrintJobRetrieval] PRIMARY KEY NONCLUSTERED 
(
	[PinPrintJobRetrievalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[PrintJobClient]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintJobClient](
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[FileName] [varchar](255) NULL,
	[FileSizeBytes] [int] NULL,
	[DevicePlatform] [varchar](10) NULL,
	[ClientOS] [varchar](50) NULL,
	[PrintQueue] [varchar](1024) NULL,
	[PrintType] [varchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[PrintStartDateTime] [datetime] NULL,
 CONSTRAINT [PK_PrintJobClient] PRIMARY KEY NONCLUSTERED 
(
	[PrintJobClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[PrintServerJob]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintServerJob](
	[PrintServerJobId] [uniqueidentifier] NOT NULL,
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[PrintServer] [varchar](100) NULL,
	[PrintServerOS] [varchar](50) NULL,
	[PrintQueue] [varchar](1024) NULL,
	[PrintDriver] [varchar](100) NULL,
	[DataType] [varchar](50) NULL,
	[SubmittedDateTime] [datetime] NULL,
	[SpoolStartDateTime] [datetime] NULL,
	[SpoolEndDateTime] [datetime] NULL,
	[PrintStartDateTime] [datetime] NULL,
	[PrintEndDateTime] [datetime] NULL,
	[JobTotalPages] [int] NULL,
	[JobTotalBytes] [bigint] NULL,
	[PrintedPages] [int] NULL,
	[PrintedBytes] [bigint] NULL,
	[RenderOnClient] [bit] NULL,
	[ColorMode] [varchar](10) NULL,
	[Copies] [int] NULL,
	[NumberUp] [int] NULL,
	[Duplex] [varchar](20) NULL,
 CONSTRAINT [PK_PrintServerJob] PRIMARY KEY NONCLUSTERED 
(
	[PrintServerJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ProductDetails]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductDetails](
	[Product] [varchar](50) NOT NULL,
	[Model] [varchar](255) NULL,
	[Platform] [varchar](50) NULL,
	[Group] [varchar](50) NULL,
	[Color] [bit] NULL,
	[MaxMediaSize] [char](5) NULL,
	[Function] [varchar](10) NULL,
 CONSTRAINT [PK_ProductDetails] PRIMARY KEY CLUSTERED 
(
	[Product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[PullPrintJobRetrieval]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PullPrintJobRetrieval](
	[PullPrintJobRetrievalId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [varchar](50) NULL,
	[DeviceId] [varchar](50) NULL,
	[SolutionType] [varchar](50) NULL,
	[JobStartDateTime] [datetime] NULL,
	[JobEndDateTime] [datetime] NULL,
	[JobEndStatus] [varchar](50) NULL,
 CONSTRAINT [PK_PullPrintJobRetrieval] PRIMARY KEY NONCLUSTERED 
(
	[PullPrintJobRetrievalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[RemoteQueueInstall]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RemoteQueueInstall](
	[RemoteQueueInstallId] [uniqueidentifier] NOT NULL,
	[ResourceMetadataId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[User] [varchar](50) NULL,
	[Queue] [varchar](512) NULL,
	[WaitOnLockDateTime] [datetime] NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
 CONSTRAINT [PK_RemoteQueueInstall] PRIMARY KEY NONCLUSTERED 
(
	[RemoteQueueInstallId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[SessionConfiguration]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionConfiguration](
	[SessionId] [varchar](50) NOT NULL,
	[ConfigurationData] [xml] NULL,
 CONSTRAINT [PK_SessionConfiguration] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[SessionSummary]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionSummary](
	[SessionId] [varchar](50) NOT NULL,
	[SessionName] [nvarchar](255) NULL,
	[Owner] [varchar](50) NULL,
	[Dispatcher] [varchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_SessionSummary] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[StepExecution]    Script Date: 11/4/2015 3:21:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StepExecution](
	[StepExecutionId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[StepName] [varchar](50) NULL,
	[TaskName] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[TaskExecutionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_StepExecution] PRIMARY KEY NONCLUSTERED 
(
	[StepExecutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[StepExecution]  WITH CHECK ADD  CONSTRAINT [FK_StepExecution_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StepExecution] CHECK CONSTRAINT [FK_StepExecution_SessionSummary]
GO

/****** Object:  Table [dbo].[STFAssetUtilization]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[STFAssetUtilization](
	[SessionId] [varchar](50) NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[AssetType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_STFAssetUtilization] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[STFClientUtilization]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[STFClientUtilization](
	[SessionId] [varchar](50) NOT NULL,
	[ClientId] [varchar](50) NOT NULL,
	[VirtualResourceType] [varchar](50) NOT NULL,
	[ResourceCount] [int] NOT NULL,
 CONSTRAINT [PK_STFClientUtilization] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[STFSessionUtilization]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[STFSessionUtilization](
	[SessionId] [varchar](50) NOT NULL,
	[Owner] [varchar](50) NULL,
	[Dispatcher] [varchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
 CONSTRAINT [PK_STFSessionUtilization] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[TaskExecution]    Script Date: 11/4/2015 3:19:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskExecution](
	[TaskExecutionId] [uniqueidentifier] NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NULL,
	[SessionId] [varchar](50) NOT NULL,
	[TaskName] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[ExecutionPath] [varchar](50) NULL,
 CONSTRAINT [PK_TaskExecution] PRIMARY KEY NONCLUSTERED 
(
	[TaskExecutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TaskExecution]  WITH CHECK ADD  CONSTRAINT [FK_TaskExecution_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaskExecution] CHECK CONSTRAINT [FK_TaskExecution_SessionSummary]
GO

/****** Object:  Table [dbo].[TestDocumentReference]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestDocumentReference](
	[TestDocumentReferenceId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[FileType] [nvarchar](50) NOT NULL,
	[FileSizeKilobytes] [bigint] NOT NULL,
	[Pages] [int] NOT NULL,
	[ColorMode] [nvarchar](50) NULL,
	[Orientation] [nvarchar](50) NULL,
	[DefectId] [varchar](50) NULL,
	[Tag] [varchar](255) NULL,
 CONSTRAINT [PK_TestDocumentReference] PRIMARY KEY CLUSTERED 
(
	[TestDocumentReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestDocumentUsage]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestDocumentUsage](
	[TestDocumentUsageId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[ActivityExecutionId] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TestDocumentUsage] PRIMARY KEY CLUSTERED 
(
	[TestDocumentUsageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TraceLog]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TraceLog](
	[TraceLogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL,
	[SessionId] [varchar](50) NULL,
	[Dispatcher] [varchar](50) NULL,
	[ContextName] [varchar](50) NULL,
	[Environment] [varchar](20) NULL,
	[HostName] [varchar](50) NULL,
	[UserName] [varchar](50) NULL,
	[SessionMapElementId] [varchar](50) NULL,
 CONSTRAINT [PK_TraceLog] PRIMARY KEY CLUSTERED 
(
	[TraceLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualPrinterJob]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualPrinterJob](
	[VirtualPrinterJobId] [uniqueidentifier] NOT NULL,
	[PrintJobClientId] [uniqueidentifier] NOT NULL,
	[PjlJobName] [varchar](255) NULL,
	[PjlLanguage] [varchar](50) NULL,
	[FirstByteReceivedDateTime] [datetime] NULL,
	[LastByteReceivedDateTime] [datetime] NULL,
	[BytesReceived] [bigint] NULL,
 CONSTRAINT [PK_VirtualPrinterJob] PRIMARY KEY NONCLUSTERED 
(
	[VirtualPrinterJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/***************** Create Primary/Foreign Key References and any Constraints *****************/
GO
ALTER TABLE [dbo].[ActivityExecution]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecution_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExecution] CHECK CONSTRAINT [FK_ActivityExecution_SessionSummary]
GO
ALTER TABLE [dbo].[ActivityExecutionDeviceUsage]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionDeviceUsage_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExecutionDeviceUsage] CHECK CONSTRAINT [FK_ActivityExecutionDeviceUsage_SessionSummary]
GO
ALTER TABLE [dbo].[ActivityExecutionPacing]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionPacing_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExecutionPacing] CHECK CONSTRAINT [FK_ActivityExecutionPacing_SessionSummary]
GO
ALTER TABLE [dbo].[ActivityExecutionPerformance]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExecutionPerformance] CHECK CONSTRAINT [FK_ActivityExecutionPerformance_SessionSummary]
GO
ALTER TABLE [dbo].[ActivityExecutionServerUsage]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExecutionServerUsage_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExecutionServerUsage] CHECK CONSTRAINT [FK_ActivityExecutionServerUsage_SessionSummary]
GO
ALTER TABLE [dbo].[DemoActivityData]  WITH CHECK ADD  CONSTRAINT [FK_DemoActivityData_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DemoActivityData] CHECK CONSTRAINT [FK_DemoActivityData_SessionSummary]
GO
ALTER TABLE [dbo].[DeviceEvent]  WITH CHECK ADD  CONSTRAINT [FK_DeviceEvent_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DeviceEvent] CHECK CONSTRAINT [FK_DeviceEvent_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendJobInput]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobInput_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendJobInput] CHECK CONSTRAINT [FK_DigitalSendJobInput_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendJobNotification]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobNotification_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendJobNotification] CHECK CONSTRAINT [FK_DigitalSendJobNotification_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendJobOutput]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobOutput_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendJobOutput] CHECK CONSTRAINT [FK_DigitalSendJobOutput_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendJobTempFile]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendJobTempFile_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendJobTempFile] CHECK CONSTRAINT [FK_DigitalSendJobTempFile_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendServerJob]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendServerJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendServerJob] CHECK CONSTRAINT [FK_DigitalSendServerJob_SessionSummary]
GO
ALTER TABLE [dbo].[DigitalSendTempSnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DigitalSendTempSnapshot_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DigitalSendTempSnapshot] CHECK CONSTRAINT [FK_DigitalSendTempSnapshot_SessionSummary]
GO
ALTER TABLE [dbo].[EPrintServerJob]  WITH CHECK ADD  CONSTRAINT [FK_EPrintServerJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EPrintServerJob] CHECK CONSTRAINT [FK_EPrintServerJob_SessionSummary]
GO
ALTER TABLE [dbo].[JetAdvantagePullPrintJobRetrieval]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantagePullPrintJobRetrieval_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JetAdvantagePullPrintJobRetrieval] CHECK CONSTRAINT [FK_JetAdvantagePullPrintJobRetrieval_SessionSummary]
GO
ALTER TABLE [dbo].[JetAdvantageUpload]  WITH CHECK ADD  CONSTRAINT [FK_JetAdvantageUpload_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JetAdvantageUpload] CHECK CONSTRAINT [FK_JetAdvantageUpload_SessionSummary]
GO
ALTER TABLE [dbo].[PhysicalDeviceJob]  WITH CHECK ADD  CONSTRAINT [FK_PhysicalDeviceJob_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhysicalDeviceJob] CHECK CONSTRAINT [FK_PhysicalDeviceJob_SessionSummary]
GO
ALTER TABLE [dbo].[PinPrintJobRetrieval]  WITH CHECK ADD  CONSTRAINT [FK_PinPrintJobRetrieval_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PinPrintJobRetrieval] CHECK CONSTRAINT [FK_PinPrintJobRetrieval_SessionSummary]
GO
ALTER TABLE [dbo].[PrintJobClient]  WITH CHECK ADD  CONSTRAINT [FK_PrintJobClient_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintJobClient] CHECK CONSTRAINT [FK_PrintJobClient_SessionSummary]
GO
ALTER TABLE [dbo].[PrintServerJob]  WITH CHECK ADD  CONSTRAINT [FK_PrintServerJob_PrintJobClient] FOREIGN KEY([PrintJobClientId])
REFERENCES [dbo].[PrintJobClient] ([PrintJobClientId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintServerJob] CHECK CONSTRAINT [FK_PrintServerJob_PrintJobClient]
GO
ALTER TABLE [dbo].[PullPrintJobRetrieval]  WITH CHECK ADD  CONSTRAINT [FK_PullPrintJobRetrieval_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PullPrintJobRetrieval] CHECK CONSTRAINT [FK_PullPrintJobRetrieval_SessionSummary]
GO
ALTER TABLE [dbo].[RemoteQueueInstall]  WITH CHECK ADD  CONSTRAINT [FK_RemoteQueueInstall_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RemoteQueueInstall] CHECK CONSTRAINT [FK_RemoteQueueInstall_SessionSummary]
GO
ALTER TABLE [dbo].[SessionConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_SessionConfiguration_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionConfiguration] CHECK CONSTRAINT [FK_SessionConfiguration_SessionSummary]
GO
ALTER TABLE [dbo].[STFAssetUtilization]  WITH CHECK ADD  CONSTRAINT [FK_STFAssetUtilization_STFSessionUtilization] FOREIGN KEY([SessionId])
REFERENCES [dbo].[STFSessionUtilization] ([SessionId])
GO
ALTER TABLE [dbo].[STFAssetUtilization] CHECK CONSTRAINT [FK_STFAssetUtilization_STFSessionUtilization]
GO
ALTER TABLE [dbo].[STFClientUtilization]  WITH CHECK ADD  CONSTRAINT [FK_STFClientUtilization_STFSessionUtilization] FOREIGN KEY([SessionId])
REFERENCES [dbo].[STFSessionUtilization] ([SessionId])
GO
ALTER TABLE [dbo].[STFClientUtilization] CHECK CONSTRAINT [FK_STFClientUtilization_STFSessionUtilization]
GO
ALTER TABLE [dbo].[VirtualPrinterJob]  WITH CHECK ADD  CONSTRAINT [FK_VirtualPrinterJob_PrintJobClient] FOREIGN KEY([PrintJobClientId])
REFERENCES [dbo].[PrintJobClient] ([PrintJobClientId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualPrinterJob] CHECK CONSTRAINT [FK_VirtualPrinterJob_PrintJobClient]
GO
/****** Object:  StoredProcedure [dbo].[del_ExpiredTraceLogData]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		John Kennedy
-- Create date: 10/3/14
-- Description:	Deletes expired trace log data
-- =============================================
-- 10/28/14  Add nolock, waitfor delay, and revise query to lessen impact
-- =============================================
CREATE PROCEDURE [dbo].[del_ExpiredTraceLogData]
AS
BEGIN
    PRINT 'DO NOTHING - Disabled Tracelog purging for now...';
END

GO
/****** Object:  StoredProcedure [dbo].[del_ExpiredTraceLogData_Experimental]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		John Kennedy
-- Create date: 10/3/14
-- Description:	Deletes expired trace log data
-- =============================================
-- 10/28/14  Add nolock, waitfor delay, and revise query to lessen impact
-- 07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[del_ExpiredTraceLogData_Experimental]
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @remainder int
	DECLARE @message varchar(255)
	DECLARE @totalCount int
	DECLARE @DELETE_LIMIT int = 10000
	DECLARE @DELETION_CAP int = 2000000
	DECLARE @UPDATE_STATS_LIMIT int = 500000
	DECLARE @updateStatsCount int = 0

	-- The amount of trace log data being deleted can get very large and fill up the tran log
	-- so break up into more manageable chunks (10,000 rows)

	-- first seed the rowcount so that it will attempt the first deletion
	SET @remainder = 1
	SET @totalCount = 0
	
	PRINT convert(nvarchar(MAX), GETDATE(), 21) + ': Start deleting for expired sessions...';
	-- keep deleting in chunks until we're through
	WHILE (@remainder > 0 and @totalCount < @DELETION_CAP) BEGIN
		BEGIN TRANSACTION deleteTracelog
			-- Remove all trace logs that belong to expired (non-existent) sessions
			DELETE TOP(@DELETE_LIMIT) TraceLog
			FROM TraceLog tl
			where
				tl.SessionId != '(NULL)'
			and tl.SessionId not in (select SessionId from SessionSummary)

			SET @remainder = @@ROWCOUNT
			SET @totalCount = @totalCount + @remainder
		COMMIT TRANSACTION deleteTracelog

		SET @message =  convert(nvarchar(MAX), GETDATE(), 21) + ': Deleted ' + CAST(@totalCount as varchar) + ' rows so far...'
		RAISERROR (@message, 0, 1) WITH NOWAIT

		-- Wait before doing next chunk to allow for other processes to proceed
		WAITFOR DELAY '00:00:05'

		-- Update statistics after a significant number of deletions so that query plans are not out of whack
		--SET @updateStatsCount = @updateStatsCount + @remainder
		--IF (@updateStatsCount >= @UPDATE_STATS_LIMIT)
		--  BEGIN
		--	RAISERROR ('Updating statistics', 0, 1) WITH NOWAIT
		--    UPDATE STATISTICS dbo.TraceLog IX_SessionId
		--	SET @updateStatsCount = 0
		--  END

	CONTINUE END
	PRINT convert(nvarchar(MAX), GETDATE(), 21) + ': Finished deleting for expired sessions';

	-- do the same for rows that do not associate with a session but are past date
	PRINT 'Starting on session-less, out of date rows...';
	SET @remainder = 1
	WHILE (@remainder > 0 and @totalCount < @DELETION_CAP) BEGIN
		BEGIN TRANSACTION deleteTracelog
			-- Remove all trace logs that belong to expired (non-existent) sessions
			DELETE TOP(@DELETE_LIMIT) TraceLog
			FROM TraceLog tl
			WHERE
				(tl.SessionId = '(NULL)' or tl.SessionId is null)
			AND (LogDateTime < DATEADD(week, -2, convert(datetime, GETDATE())))

			SET @remainder = @@ROWCOUNT
			SET @totalCount = @totalCount + @remainder
		COMMIT TRANSACTION deleteTracelog

		SET @message =  convert(nvarchar(MAX), GETDATE(), 21) + ': Deleted ' + CAST(@totalCount as varchar) + ' rows so far...'
		RAISERROR (@message, 0, 1) WITH NOWAIT
		-- Wait before doing next chunk to allow for other processes to proceed
		WAITFOR DELAY '00:00:05'
	CONTINUE END
	PRINT convert(nvarchar(MAX), GETDATE(), 21) + ': Finished deleting for out of date rows';
	PRINT convert(nvarchar(MAX), GETDATE(), 21) + ': Total rows deleted=' + CAST(@totalCount as varchar);	
END

GO
/****** Object:  StoredProcedure [dbo].[del_SessionData]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 1/24/2013
-- Description:	Deletes all session data for the given session id.
-- Updated:		
--	 6/4/2014 - Added removal of TraceLog data
--  10/3/2014 - Revise TraceLog data cleanup 
-- =============================================
CREATE PROCEDURE [dbo].[del_SessionData] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SessionSummary WHERE SessionId = @sessionId

	-- Delete the TraceLog data related to the session
	-- The amount of trace log data being deleted can get very large and fill up the tran log
	-- so break up into more manageable chunks (10,000 rows)

	-- first seed the rowcount so that it will attempt the first deletion
	select top 1 * from TraceLog 
	-- keep deleting in chunks until we're through
	WHILE (@@ROWCOUNT > 0) BEGIN
		BEGIN TRANSACTION deleteTracelog
			DELETE TOP(10000) FROM TraceLog WHERE SessionId = @sessionId
		COMMIT TRANSACTION deleteTracelog
	CONTINUE END

	EXEC dbo.del_ExpiredTraceLogData	
END

GO
/****** Object:  StoredProcedure [dbo].[ins_TraceLog]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		jlkennedy
-- Create date: 5/8/14
-- Description:	Inserts entries into the TraceLog table
-- Updated:	
-- 07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[ins_TraceLog] (
	@log_date datetime
	,@thread varchar(255)
	,@log_level varchar(50)
	,@logger varchar(255) 
	,@message varchar(4000) = NULL
	,@exception varchar(2000) = NULL
	,@contextName varchar(50) = NULL
	,@dispatcher varchar(50) = NULL
	,@sessionId varchar(50) = NULL
	,@environment varchar(20) = NULL
	,@hostName varchar(50) = NULL
	,@userName varchar(50) = NULL
	,@mapElementId varchar(50) = NULL
	)
AS
BEGIN
	INSERT INTO [dbo].TraceLog (
		[LogDateTime],[Thread],[Level],[Logger],[Message],[Exception],[ContextName],[Dispatcher],[SessionId],[Environment],[HostName],[UserName],[SessionMapElementId]) 
        VALUES 
		(@log_date, @thread, @log_level, @logger, @message, @exception, @contextName, @dispatcher, @sessionId, @environment, @hostName, @userName, @mapElementId)
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorsPerMinute]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker error messages per minute
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorsPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ErrorMessage as ErrorMessage,
		owa.UpdateType,
		COUNT(owa.ErrorMessage) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
	(
		SELECT 
			(CASE
				WHEN CHARINDEX(':', i.ErrorMessage) = 0 THEN i.ErrorMessage
				ELSE LEFT(i.ErrorMessage, CHARINDEX(':', ErrorMessage) - 1)
			END) as ErrorMessage,
			i.UpdateType,
			COUNT(i.ErrorMessage) AS InnerCount,
			i.EndDateTime
		FROM
			ActivityExecution i
		WHERE
			i.SessionId = @sessionId
			and i.ErrorMessage != ''
			and (@time = 0 or (@time > 0 and i.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))			
		GROUP BY
			i.ErrorMessage,
			i.UpdateType,
			i.EndDateTime
	) owa

	GROUP BY
		owa.ErrorMessage,
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotals]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker errors grouped by error message.
-- Edited:		11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotals]
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		i.ErrorMessage,
		i.UpdateType,
		SUM(i.InnerCount) as Count
	FROM 
	(
		SELECT 
			(CASE
				WHEN CHARINDEX(':', owa.ErrorMessage) = 0 THEN owa.ErrorMessage
				ELSE LEFT(owa.ErrorMessage, CHARINDEX(':', ErrorMessage) - 1)
			END) as ErrorMessage,
			owa.UpdateType,
			COUNT(*) as InnerCount
		FROM
			ActivityExecution owa
		WHERE
			owa.SessionId = @sessionId
			and owa.ErrorMessage != ''
		GROUP BY
			owa.ErrorMessage,
			owa.UpdateType
	) i
	GROUP BY
		i.ErrorMessage,
		i.UpdateType
	ORDER BY
		i.UpdateType
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotalsDetails]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a summary of error messages starting with a specific prefix
-- Edited:		11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails] 
	@sessionId nvarchar(50) = 0,
	@shortError nvarchar(255) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ErrorMessage like (@shortError + '%')
	GROUP BY
		owa.ErrorMessage
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstancesPerMinute]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by instance.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstancesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityName as Description,
		owa.ActivityType,
		owa.UpdateType,
		COUNT(owa.ActivityType) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))
	GROUP BY
		owa.ActivityName,
		owa.ActivityType,
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstanceTotals]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata instance.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- ==========================================================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityName as Description,
		owa.ActivityType,
		owa.UpdateType,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		owa.ActivityName,
		owa.ActivityType,
		owa.UpdateType
	ORDER BY
		owa.ActivityName desc,
		owa.UpdateType desc
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstanceTotalsErrors]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for an activity/updatetype pair
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@description nvarchar(50) = 0,
	@updateType nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ActivityName = @description
		and owa.UpdateType = @updateType
	GROUP BY
		owa.ErrorMessage
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypesPerMinute]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by metadata type.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityType,
		owa.UpdateType,
		COUNT(owa.ActivityType) AS Count, 
		DATEPART(YY, owa.EndDateTime) AS Year,
		DATEPART(MONTH, owa.EndDateTime) AS Month,
		DATEPART(DAY, owa.EndDateTime) AS Day,
		DATEPART(HOUR, owa.EndDateTime) AS Hour,
		DATEPART(MINUTE, owa.EndDateTime) AS Minute		
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndDateTime > DATEADD(MINUTE, -@time, GETDATE())))
	GROUP BY
		owa.ActivityType,
		owa.UpdateType,
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime)
	ORDER BY 
		DATEPART(YY, owa.EndDateTime),
		DATEPART(MONTH, owa.EndDateTime),
		DATEPART(DAY, owa.EndDateTime),
		DATEPART(HOUR, owa.EndDateTime),
		DATEPART(MINUTE, owa.EndDateTime) asc
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypeTotals]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata type.
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ActivityType,
		owa.UpdateType,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		owa.ActivityType,
		owa.UpdateType
	ORDER BY
		owa.ActivityType desc,
		owa.UpdateType desc
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypeTotalsErrors]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for a metadatatype/updatetype pair
-- Edited:		7/25/12 BJ Myers - Removed join to VirtualResourceMetadata
--				11/16/12 BJ Myers - Moved to ScalableTestDataLog database
-- 				07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@activityType nvarchar(50) = 0,
	@updateType nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.ErrorMessage,
		COUNT(*) ErrorCount
	FROM
		ActivityExecution owa
	WHERE
		owa.SessionId = @sessionId
		and owa.ActivityType = @activityType
		and owa.UpdateType = @updateType
	GROUP BY
		owa.ErrorMessage
END

GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_WorkerActivitySessions]    Script Date: 9/24/2015 3:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 11/16/2012
-- Description:	Returns a list of sessions that have entries in the WorkerActivityLog
-- Edited:		07/13/15  fdelagarza, Updated for new Log Database Schema
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_WorkerActivitySessions] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SessionId,
		SessionName,
		Owner,
		StartDateTime
	FROM SessionSummary
	WHERE SessionId IN (SELECT SessionId from ActivityExecution)
	ORDER BY StartDateTime desc
END
GO

/****** Object:  StoredProcedure [dbo].[sel_Chart_TaskTotals]    Script Date: 11/9/2015 3:50:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================
-- Author:		John Kennedy
-- Create date: 10/28/2015
-- Description:	Returns a count of tasks grouped by task name
-- Edited:		
-- ==========================================================================
CREATE PROCEDURE [dbo].[sel_Chart_TaskTotals] 
	@sessionId nvarchar(50) = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		te.TaskName,
		te.ExecutionPath,
		te.Status,
		COUNT(*) Count
	FROM 
		ActivityExecution owa
	JOIN TaskExecution te on owa.ActivityExecutionId = te.ActivityExecutionId
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		te.TaskName,
		te.ExecutionPath,		
		te.[Status]
	ORDER BY
		te.TaskName,
		te.ExecutionPath, 
		te.[Status]
END
GO

/****** Object:  Table [dbo].[SessionDevice]    Script Date: 11/6/2015 12:28:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SessionDevice]  WITH CHECK ADD  CONSTRAINT [FK_SessionDevice_SessionSummary] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionSummary] ([SessionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionDevice] CHECK CONSTRAINT [FK_SessionDevice_SessionSummary]
GO
/****** Object:  Table [dbo].[DeviceMemorySnapshot]    Script Date: 11/6/2015 12:29:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeviceMemorySnapshot](
	[DeviceMemorySnapshotId] [uniqueidentifier] NOT NULL,
	[SessionDeviceId] [uniqueidentifier] NOT NULL,
	[SnapshotDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_DeviceMemorySnapshot] PRIMARY KEY CLUSTERED 
(
	[DeviceMemorySnapshotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DeviceMemorySnapshot]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemorySnapshot_SessionDevice] FOREIGN KEY([SessionDeviceId])
REFERENCES [dbo].[SessionDevice] ([SessionDeviceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DeviceMemorySnapshot] CHECK CONSTRAINT [FK_DeviceMemorySnapshot_SessionDevice]
GO

/****** Object:  Table [dbo].[DeviceMemoryCount]    Script Date: 11/6/2015 12:29:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[DeviceMemoryCount]  WITH CHECK ADD  CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot] FOREIGN KEY([DeviceMemorySnapshotId])
REFERENCES [dbo].[DeviceMemorySnapshot] ([DeviceMemorySnapshotId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DeviceMemoryCount] CHECK CONSTRAINT [FK_DeviceMemoryCount_DeviceMemorySnapshot]
GO
