USE [EnterpriseTest]
GO
/****** Object:  Table [dbo].[WebSite]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WebSite](
	[WebSiteId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](500) NOT NULL,
	[LoadWeight] [varchar](50) NULL,
 CONSTRAINT [PK_WebSite] PRIMARY KEY CLUSTERED 
(
	[WebSiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionInfo]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionInfo](
	[SessionId] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[ScenarioName] [varchar](255) NULL,
	[Owner] [varchar](50) NULL,
	[TestResult] [varchar](50) NULL,
	[QualityCenterId] [varchar](50) NULL,
	[Notes] [varchar](max) NULL,
	[ConfigurationData] [xml] NULL,
	[ShutdownState] [varchar](50) NOT NULL,
	[Dispatcher] [varchar](50) NOT NULL,
	[ProjectedEndDate] [datetime] NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Running, Problem, Completed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SessionInfo', @level2type=N'COLUMN',@level2name=N'Status'
GO
/****** Object:  Table [dbo].[TreeViewFolder]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TreeViewFolder](
	[TreeViewFolderId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Name] [varchar](1024) NOT NULL,
	[FolderType] [varchar](255) NOT NULL,
 CONSTRAINT [PK_TreeViewFolder] PRIMARY KEY CLUSTERED 
(
	[TreeViewFolderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestDocument]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TestDocument](
	[ItemId] [uniqueidentifier] NOT NULL,
	[PathName] [varchar](255) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Size] [int] NOT NULL,
 CONSTRAINT [PK_TestDocument] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachinePlatform]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachinePlatform](
	[PlatformId] [varchar](50) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_VirtualMachinePlatform] PRIMARY KEY CLUSTERED 
(
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachineHostSystem]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachineHostSystem](
	[HostSystemId] [varchar](50) NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualMachineHostSystem] PRIMARY KEY CLUSTERED 
(
	[HostSystemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachine]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachine](
	[Name] [varchar](50) NOT NULL,
	[PowerState] [varchar](50) NOT NULL,
	[PlatformUsage] [varchar](50) NOT NULL,
	[UsageState] [varchar](50) NOT NULL,
	[HoldId] [varchar](50) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[SortOrder] [int] NOT NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[VirtualMachine] ADD [SessionId] [varchar](50) NULL
ALTER TABLE [dbo].[VirtualMachine] ADD  CONSTRAINT [PK_VirtualMachine] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ValueGroup]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ValueGroup](
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DomainValues] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[Value] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserName] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceType]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceType](
	[Name] [varchar](50) NOT NULL,
	[MaxResourcesPerHost] [int] NOT NULL,
	[Platform] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualResourceType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Report]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Report](
	[ReportId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Template] [varchar](255) NOT NULL,
	[SheetName] [varchar](50) NOT NULL,
	[StartingCell] [varchar](5) NOT NULL,
	[SqlText] [varchar](max) NOT NULL,
	[PropertiesXml] [xml] NOT NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY NONCLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsCategory]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceWindowsCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[CategoryType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceWindowsCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsPerfMonLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceWindowsPerfMonLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[TargetHost] [varchar](50) NOT NULL,
	[Category] [varchar](255) NOT NULL,
	[InstanceName] [varchar](255) NOT NULL,
	[Counter] [varchar](255) NOT NULL,
	[CounterValue] [float] NOT NULL,
 CONSTRAINT [PK_ResourceWindowsPerfMonLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsEventLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceWindowsEventLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[SolutionModule] [varchar](255) NULL,
	[Source1] [varchar](255) NULL,
	[Source2] [varchar](255) NULL,
	[TimeGenerated] [datetime] NOT NULL,
	[EventId] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[EventType] [varchar](50) NOT NULL,
	[EventData] [varchar](max) NULL,
 CONSTRAINT [PK_ResourceWindowsEventLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QtpTestPackage]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QtpTestPackage](
	[QtpTestPackageId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_QtpTestPackage] PRIMARY KEY NONCLUSTERED 
(
	[QtpTestPackageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductWSProfile]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductWSProfile](
	[ProductWSProfileId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Location] [varchar](255) NOT NULL,
 CONSTRAINT [PK_ProductWSProfile] PRIMARY KEY CLUSTERED 
(
	[ProductWSProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataType]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataType](
	[Name] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[ResourceType] [nvarchar](50) NOT NULL,
	[Version] [varchar](5) NULL,
 CONSTRAINT [PK_Plugin] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LowLayerTestProduct]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LowLayerTestProduct](
	[ScriptId] [uniqueidentifier] NOT NULL,
	[Product] [varchar](50) NOT NULL,
	[TestType] [varchar](50) NOT NULL,
	[Script] [varchar](2048) NOT NULL,
 CONSTRAINT [PK_LowLayerTestProduct] PRIMARY KEY CLUSTERED 
(
	[ScriptId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductMib]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductMib](
	[ProductMibId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Location] [varchar](255) NOT NULL,
 CONSTRAINT [PK_ProductMib] PRIMARY KEY CLUSTERED 
(
	[ProductMibId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintServer]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintServer](
	[Name] [varchar](50) NOT NULL,
	[ServerType] [varchar](50) NOT NULL,
	[PrintServerId] [uniqueidentifier] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_PrintServer] PRIMARY KEY CLUSTERED 
(
	[PrintServerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintMonitorQueueLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintMonitorQueueLog](
	[PrintJobId] [uniqueidentifier] NOT NULL,
	[SessionId] [nvarchar](50) NULL,
	[QueueName] [nvarchar](50) NULL,
	[ServerName] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Size] [int] NULL,
	[SubmitTime] [datetime] NULL,
 CONSTRAINT [PK_PrintJobInfo] PRIMARY KEY CLUSTERED 
(
	[PrintJobId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrintJobLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintJobLog](
	[JobId] [uniqueidentifier] NOT NULL,
	[SessionId] [nvarchar](50) NULL,
	[JobTicketId] [varchar](255) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[PrintServer] [nvarchar](100) NOT NULL,
	[PrintQueue] [nvarchar](500) NOT NULL,
	[FileName] [nvarchar](200) NULL,
	[FileSize] [int] NULL,
	[FileType] [varchar](5) NULL,
	[DevicePlatform] [nvarchar](10) NULL,
	[ClientJobEntersQueueDateTime] [datetime] NULL,
	[ClientJobExitsQueueDateTime] [datetime] NULL,
	[ClientJobRenderTime] [int] NULL,
	[ClientJobQueueWaitTime] [int] NULL,
	[ClientOS] [varchar](50) NULL,
	[ServerJobEntersQueueDateTime] [datetime] NULL,
	[ServerJobExitsQueueDateTime] [datetime] NULL,
	[ServerJobSpoolingTime] [int] NULL,
	[ServerJobQueueIdleTime] [int] NULL,
	[ServerJobPrintingTime] [int] NULL,
	[ServerOS] [varchar](50) NULL,
	[ServerSubJobCount] [int] NULL,
	[DeviceFirstByteInDateTime] [datetime] NULL,
	[DeviceLastByteInDateTime] [datetime] NULL,
	[DeviceSubJobCount] [int] NULL,
	[PjlJobName] [nvarchar](200) NULL,
	[PjlPrintServerOS] [nvarchar](50) NULL,
	[PjlByteCount] [int] NULL,
	[PjlDateTime] [nvarchar](50) NULL,
	[PjlLanguage] [nvarchar](50) NULL,
	[JobComplete] [bit] NULL,
	[ServerJobSpoolStartTime] [datetime] NULL,
	[ServerJobSpoolEndTime] [datetime] NULL,
	[ServerJobPrintStartTime] [datetime] NULL,
	[ServerJobPrintEndTime] [datetime] NULL,
	[ServerJobDriverName] [nvarchar](100) NULL,
 CONSTRAINT [PK_PrintJobLog] PRIMARY KEY NONCLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverPackage]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintDriverPackage](
	[PrintDriverPackageId] [uniqueidentifier] NOT NULL,
	[Version] [varchar](255) NOT NULL,
	[InfX86] [varchar](255) NOT NULL,
	[InfX64] [varchar](255) NOT NULL,
 CONSTRAINT [PK_PrintDriverPackage] PRIMARY KEY CLUSTERED 
(
	[PrintDriverPackageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfficeWorkerActivityLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfficeWorkerActivityLog](
	[ActivityId] [uniqueidentifier] NOT NULL,
	[SessionId] [nvarchar](50) NOT NULL,
	[MetadataId] [uniqueidentifier] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[UpdateType] [nvarchar](50) NOT NULL,
	[Result] [nvarchar](50) NULL,
	[Data] [xml] NULL,
	[Error] [varchar](max) NULL,
 CONSTRAINT [PK_OfficeWorkerActivityLog] PRIMARY KEY NONCLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IPAddressPool]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IPAddressPool](
	[IPAddressPoolId] [uniqueidentifier] NOT NULL,
	[StartAddress] [varchar](15) NOT NULL,
	[EndAddress] [varchar](15) NOT NULL,
	[SubnetMask] [varchar](15) NOT NULL,
	[DefaultGateway] [varchar](15) NOT NULL,
 CONSTRAINT [PK_IpAddressPool] PRIMARY KEY CLUSTERED 
(
	[IPAddressPoolId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DomainAccountReservation]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DomainAccountReservation](
	[SessionId] [varchar](50) NOT NULL,
	[StartIndex] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[DomainAccountKey] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DomainAccountReservation] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[DomainAccountKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DomainAccountPool]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DomainAccountPool](
	[DomainAccountKey] [varchar](50) NOT NULL,
	[UserNameFormat] [varchar](50) NOT NULL,
	[MinimumUserNumber] [int] NOT NULL,
	[MaximumUserNumber] [int] NOT NULL,
 CONSTRAINT [PK_DomainAccountPool] PRIMARY KEY CLUSTERED 
(
	[DomainAccountKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemQueryDomain]    Script Date: 07/16/2012 12:21:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssociatedProduct]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssociatedProduct](
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_AssociatedProduct] PRIMARY KEY CLUSTERED 
(
	[AssociatedProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AlsPrintJobLog]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlsPrintJobLog](
	[SessionId] [nvarchar](50) NULL,
	[JobTicketId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[PrintServer] [nvarchar](100) NOT NULL,
	[PrintQueue] [nvarchar](500) NOT NULL,
	[FileName] [nvarchar](200) NULL,
	[FileSize] [int] NULL,
	[FileType] [varchar](5) NULL,
	[DevicePlatform] [nvarchar](10) NULL,
	[ClientJobEntersQueueDateTime] [datetime] NULL,
	[ClientJobExitsQueueDateTime] [datetime] NULL,
	[ClientJobRenderTime] [int] NULL,
	[ClientJobQueueWaitTime] [int] NULL,
	[ClientOS] [varchar](50) NULL,
	[ServerJobEntersQueueDateTime] [datetime] NULL,
	[ServerJobExitsQueueDateTime] [datetime] NULL,
	[ServerJobSpoolingTime] [int] NULL,
	[ServerJobQueueIdleTime] [int] NULL,
	[ServerJobPrintingTime] [int] NULL,
	[ServerOS] [varchar](50) NULL,
	[ServerSubJobCount] [int] NULL,
	[DeviceFirstByteInDateTime] [datetime] NULL,
	[DeviceLastByteInDateTime] [datetime] NULL,
	[DeviceSubJobCount] [int] NULL,
	[PjlJobName] [nvarchar](200) NULL,
	[PjlPrintServerOS] [nvarchar](50) NULL,
	[PjlByteCount] [int] NULL,
	[PjlDateTime] [nvarchar](50) NULL,
	[PjlLanguage] [nvarchar](50) NULL,
	[JobComplete] [bit] NULL,
	[ServerJobSpoolStartTime] [datetime] NULL,
	[ServerJobSpoolEndTime] [datetime] NULL,
	[ServerJobPrintStartTime] [datetime] NULL,
	[ServerJobPrintEndTime] [datetime] NULL,
	[ServerJobDriverName] [nvarchar](100) NULL,
 CONSTRAINT [PK_AlsPrintJobLog] PRIMARY KEY NONCLUSTERED 
(
	[JobTicketId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EnterpriseScenario]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EnterpriseScenario](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[Vertical] [nvarchar](50) NULL,
	[Company] [nvarchar](50) NULL,
	[Creator] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[Editor] [varchar](50) NULL,
	[EditedDate] [datetime] NULL,
	[TreeViewFolderId] [uniqueidentifier] NOT NULL,
	[LastRunDate] [datetime] NULL,
 CONSTRAINT [PK_EnterpriseScenario] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IPAddressReservation]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IPAddressReservation](
	[IPAddressReservationId] [uniqueidentifier] NOT NULL,
	[IPAddressPoolId] [uniqueidentifier] NOT NULL,
	[StartAddress] [bigint] NOT NULL,
	[EndAddress] [bigint] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_IpAddressReservation] PRIMARY KEY CLUSTERED 
(
	[IPAddressReservationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[del_SessionInfoCascading]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kelly Youngman
-- Create date: 4/6/2011
-- Description:	Deletes a SessionInfo record and all associated OfficeWorkerActivityData records
--              Also clears the PrintJobLog table.
-- Updated:		12/16/2011
--				VirtualMachineInfo and VirtualResourceInfo tables deleted. -bmyers
-- =============================================
CREATE PROCEDURE [dbo].[del_SessionInfoCascading] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE OfficeWorkerActivityLog WHERE SessionId = @sessionId
	DELETE SessionInfo WHERE SessionId = @sessionId
	DELETE PrintJobLog WHERE SessionId = @sessionId

END
GO
/****** Object:  Table [dbo].[PrintDriver]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintDriver](
	[PrintDriverId] [uniqueidentifier] NOT NULL,
	[PrintDriverPackageId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[PrintProcessor] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PrintDriver] PRIMARY KEY CLUSTERED 
(
	[PrintDriverId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfficeWorkerActivityLogData]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfficeWorkerActivityLogData](
	[DataId] [uniqueidentifier] NOT NULL,
	[ActivityId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[LogType] [nvarchar](50) NULL,
 CONSTRAINT [PK_DataId] PRIMARY KEY NONCLUSTERED 
(
	[DataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrintQueue]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintQueue](
	[Name] [nvarchar](200) NOT NULL,
	[DevicePlatform] [nvarchar](50) NOT NULL,
	[InventoryId] [nvarchar](50) NULL,
	[PrintQueueId] [uniqueidentifier] NOT NULL,
	[PrintServerId] [uniqueidentifier] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_PrintQueue] PRIMARY KEY CLUSTERED 
(
	[PrintQueueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductSnmpProfile]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductSnmpProfile](
	[ProductSnmpProfileId] [uniqueidentifier] NOT NULL,
	[ProductMibId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Location] [varchar](255) NOT NULL,
 CONSTRAINT [PK_ProductSnmpProfile] PRIMARY KEY CLUSTERED 
(
	[ProductSnmpProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QtpTest]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QtpTest](
	[QtpTestId] [uniqueidentifier] NOT NULL,
	[QtpTestPackageId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_QtpTest] PRIMARY KEY NONCLUSTERED 
(
	[QtpTestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsCategoryParent]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceWindowsCategoryParent](
	[CategoryId] [int] NOT NULL,
	[ParentCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_ResourceWindowsCategoryParent] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[ParentCategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceTypePlatformAssoc]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceTypePlatformAssoc](
	[Name] [varchar](50) NOT NULL,
	[PlatformId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceTypePlatformAssoc] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[PlatformId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionProductAssoc]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionProductAssoc](
	[SessionId] [varchar](50) NOT NULL,
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SessionProductAssoc] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[AssociatedProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachinePlatformAssoc]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachinePlatformAssoc](
	[PlatformId] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualMachinePlatformAssoc] PRIMARY KEY CLUSTERED 
(
	[PlatformId] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SavedReport]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SavedReport](
	[SavedReportId] [uniqueidentifier] NOT NULL,
	[ReportId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Creator] [varchar](50) NOT NULL,
	[CriteriaSet] [xml] NOT NULL,
 CONSTRAINT [PK_SavedReport] PRIMARY KEY NONCLUSTERED 
(
	[SavedReportId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstanceTotalsErrors]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for an activity/updatetype pair
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@metadataId nvarchar(50) = 0,
	@updateType nvarchar(50) = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.Error,
		COUNT(*) ErrorCount
	FROM
		OfficeWorkerActivityLog owa
	WHERE
		owa.SessionId = @sessionId
		and owa.MetadataId = @metadataId
		and owa.UpdateType = @updateType
	GROUP BY
		owa.Error

END
GO
/****** Object:  StoredProcedure [dbo].[sel_SessionsWithCounts]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		kyoungman
-- Create date: 5/10/2011
-- Description:	Returns a list of all Sessions with Activity and PrintJob counts
-- Modified date: 5/15/2012 aandrews
-- Description: Added ShutdownState to procedure.
-- =============================================
CREATE PROCEDURE [dbo].[sel_SessionsWithCounts]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		s.SessionId, 
		s.[Status], 
		s.ShutdownState,
		s.StartDate, 
		s.ScenarioName, 
		s.[Owner],
		(select count(*) from OfficeWorkerActivityLog where s.SessionId = SessionId) as ActivityCount,
		(select count(*) from PrintJobLog where s.SessionId = SessionId) as PrintJobCount
	FROM SessionInfo s	
	ORDER BY s.StartDate
		
END
GO
/****** Object:  StoredProcedure [dbo].[sel_PrintJobLog]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/2/5/2011
-- Description:	Selects PrintJobLog rows by SessionId
-- =============================================
CREATE PROCEDURE [dbo].[sel_PrintJobLog] 
	-- Add the parameters for the stored procedure here
	@sessionId nvarchar(50) = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		*
	FROM
		PrintJobLog pjl
	WHERE
		pjl.SessionId = @sessionId
END
GO
/****** Object:  StoredProcedure [dbo].[sel_DashboardSummary]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		kyoungman
-- Create date: 6/15/2012
-- Description:	Returns a list of VM usage information
-- Modified date:
-- Description:
-- =============================================
CREATE PROCEDURE [dbo].[sel_DashboardSummary]
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @now DATETIME
	SET @now = GETDATE()

	SELECT DISTINCT v.SessionId, 
		i.ScenarioName,		
		i.[Owner],
		i.Dispatcher,		
		i.StartDate,
		i.ProjectedEndDate,
		i.[Status],
		CONVERT(VARCHAR(10), DATEDIFF(HOUR, i.StartDate, @now)/24) + ' Days ' +  CONVERT(VARCHAR(20),DATEDIFF(HOUR, i.StartDate, @now)%(24)) + ' Hours' as TimeRunning,
		(SELECT COUNT(Name) FROM VirtualMachine WHERE SessionId = v.SessionId AND Name LIKE 'X%') AS XPCount,
		(SELECT COUNT(Name) FROM VirtualMachine WHERE SessionId = v.SessionId AND Name LIKE 'W%') AS W7Count
	FROM VirtualMachine v LEFT JOIN SessionInfo i on v.SessionId = i.SessionId
	WHERE v.SessionId IS NOT null
		
END
GO
/****** Object:  StoredProcedure [dbo].[sel_CurrentSessions]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/25/2011
-- Description:	Returns a list of all Sessions, their description and the start time
-- =============================================
CREATE PROCEDURE [dbo].[sel_CurrentSessions] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		si.SessionId,
		si.StartDate,
		si.ScenarioName
	FROM OfficeWorkerActivityLog owal	
		left outer join SessionInfo si on owal.SessionId = si.SessionId
	ORDER BY
		si.StartDate desc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_CurrentSessionIdValues]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/25/2011
-- Description:	Returns a list of all Sessions, their description and the start time
-- 4/4/2011 kyoungman Removed the join to EnterpriseScenario table
-- 9/27/2011 ghosharp Added a join to OfficeWorkerActivityLog, so only that have 
--					run and has Office workers will show up
-- 2/22/2012 bmyers Replaced join to OfficeWorkerActivityLog with a subquery for performance
-- =============================================
CREATE PROCEDURE [dbo].[sel_CurrentSessionIdValues] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SessionId,
		ScenarioName,
		Owner,
		StartDate
	FROM SessionInfo
	WHERE SessionId IN (SELECT SessionId from OfficeWorkerActivityLog)
	ORDER BY StartDate desc

END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_VMUsage]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		kyoungman
-- Create date: 6/15/2012
-- Description:	Returns a list of VM usage counts
-- Modified date:
-- Description:
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_VMUsage]
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @now DATETIME
	SET @now = GETDATE()

	SELECT v.SessionId, 
		COUNT(v.Name)as VMCount
	FROM VirtualMachine v LEFT JOIN SessionInfo i on v.SessionId = i.SessionId
	WHERE v.SessionId IS NOT null GROUP BY v.SessionId
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachine v WHERE SessionId IS null AND UsageState != 'Available' GROUP BY v.UsageState
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachine v WHERE SessionId IS null AND UsageState = 'Available' GROUP BY v.UsageState
		
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotalsDetails]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a summary of error messages starting with a specific prefix
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails] 
	@sessionId nvarchar(50) = 0,
	@shortError nvarchar(255) = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.Error,
		COUNT(*) ErrorCount
	FROM
		OfficeWorkerActivityLog owa
	WHERE
		owa.SessionId = @sessionId
		and owa.Error like (@shortError + '%')
	GROUP BY
		owa.Error

END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorTotals]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker errors grouped by error message.
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorTotals]
	@sessionId nvarchar(50) = 0
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		(CASE
			WHEN CHARINDEX(':', owa.Error) = 0 THEN owa.Error
			ELSE LEFT(owa.Error, CHARINDEX(':', Error) - 1)
		END) as Error,
		owa.UpdateType,
		COUNT(*) as Count
	FROM
		OfficeWorkerActivityLog owa
	WHERE
		owa.SessionId = @sessionId
		and owa.Error != ''
	GROUP BY
		owa.Error,
		owa.UpdateType
	ORDER BY
		owa.UpdateType
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityErrorsPerMinute]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/18/2012
-- Description:	Returns a count of office worker error messages per minute
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityErrorsPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		(CASE
			WHEN CHARINDEX(':', owa.Error) = 0 THEN owa.Error
			ELSE LEFT(owa.Error, CHARINDEX(':', Error) - 1)
		END) as Error,
		owa.UpdateType,
		COUNT(owa.Error) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour,
		DATEPART(MINUTE, owa.EndTime) AS Minute		
	FROM 
		OfficeWorkerActivityLog owa
	WHERE
		owa.SessionId = @sessionId
		and owa.Error != ''
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(MINUTE, -@time, GETDATE())))
	GROUP BY
		owa.Error,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime)
	ORDER BY 
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime) asc
END
GO
/****** Object:  Table [dbo].[ScenarioProductAssoc]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScenarioProductAssoc](
	[EnterpriseScenarioid] [uniqueidentifier] NOT NULL,
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ScenarioProductAssoc] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioid] ASC,
	[AssociatedProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualResource](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ResourceType] [nvarchar](50) NOT NULL,
	[InstanceCount] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[StartupMode] [nvarchar](50) NULL,
	[Platform] [nvarchar](50) NOT NULL,
	[Creator] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[Editor] [varchar](50) NULL,
	[EditedDate] [datetime] NULL,
	[TreeViewFolderId] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[LastRunDate] [datetime] NULL,
	[ResourcesPerVM] [int] NULL,
 CONSTRAINT [PK_VirtualResource] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QtpTestParameter]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QtpTestParameter](
	[QtpTestParameterId] [uniqueidentifier] NOT NULL,
	[QtpTestId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Required] [bit] NOT NULL,
	[DefaultValue] [varchar](200) NULL,
 CONSTRAINT [PK_QtpTestParameter] PRIMARY KEY NONCLUSTERED 
(
	[QtpTestParameterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfficeWorker]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfficeWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ProfileGroup] [nvarchar](50) NOT NULL,
	[RepeatCount] [int] NOT NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[RandomizeActivities] [bit] NOT NULL,
	[RandomizeActivityDelay] [bit] NOT NULL,
	[MinActivityDelay] [int] NOT NULL,
	[MaxActivityDelay] [int] NOT NULL,
	[RunMode] [varchar](50) NOT NULL,
	[DurationTime] [int] NOT NULL,
	[SecurityGroups] [xml] NULL,
	[CitrixServer] [nvarchar](50) NULL,
 CONSTRAINT [PK_OfficeWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EventLogCollector]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventLogCollector](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[ComponentsData] [xml] NOT NULL,
	[EntryTypesData] [xml] NOT NULL,
	[PollingInterval] [int] NOT NULL,
 CONSTRAINT [PK_EventLogCollector] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EpicPrintService]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EpicPrintService](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ProcessCount] [int] NOT NULL,
	[RunMode] [varchar](50) NOT NULL,
	[RepeatCount] [int] NOT NULL,
	[DurationTime] [int] NOT NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[RandomizePrintJobDelay] [bit] NOT NULL,
	[MinPrintJobDelay] [int] NOT NULL,
	[MaxPrintJobDelay] [int] NOT NULL,
	[PrintData] [xml] NOT NULL,
 CONSTRAINT [PK_EpicPrintService] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EndpointResponder]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EndpointResponder](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[SnmpEnabled] [bit] NOT NULL,
	[ProductSnmpProfileId] [uniqueidentifier] NULL,
	[SnmpConfigurationData] [xml] NULL,
	[WebServicesEnabled] [bit] NOT NULL,
	[ProductWSProfileId] [uniqueidentifier] NULL,
	[WebServicesConfigurationData] [xml] NULL,
 CONSTRAINT [PK_EndpointResponder] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminWorker]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ExecutionMode] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AdminWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QtpResource]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QtpResource](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[QtpTestId] [uniqueidentifier] NOT NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[SecurityGroups] [xml] NULL,
 CONSTRAINT [PK_QtpResource] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LowLayerTester]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LowLayerTester](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[FirmwareVersion] [varchar](50) NOT NULL,
	[ProductConfig] [varchar](50) NOT NULL,
	[DriverVersion] [varchar](50) NOT NULL,
	[SoftwareVersion] [varchar](50) NOT NULL,
	[Profile] [varchar](50) NULL,
	[Device] [varchar](50) NOT NULL,
	[TestEmail] [varchar](50) NULL,
	[NotificationEmail] [varchar](50) NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[ScriptId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LowLayerTester] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualResourceResourceUsage]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualResourceResourceUsage](
	[ResourceId] [varchar](50) NOT NULL,
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ResourceType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualResourceResourceUsage] PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC,
	[VirtualResourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualResourceMetadata]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualResourceMetadata](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ResourceType] [nvarchar](50) NOT NULL,
	[MetadataType] [nvarchar](50) NOT NULL,
	[Metadata] [xml] NOT NULL,
	[Version] [varchar](5) NULL,
	[Creator] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[Editor] [varchar](50) NULL,
	[EditedDate] [datetime] NULL,
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[TreeViewFolderId] [uniqueidentifier] NOT NULL,
	[LinkId] [uniqueidentifier] NULL,
	[ExecutionOrder] [int] NOT NULL,
	[LastRunDate] [datetime] NULL,
 CONSTRAINT [PK_VirtualResourceMetadata] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualResourceDependency]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualResourceDependency](
	[VirtualResourceParentId] [uniqueidentifier] NOT NULL,
	[VirtualResourceChildId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_VirtualResourceDependency] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceParentId] ASC,
	[VirtualResourceChildId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstanceTotals]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata instance.
-- ==========================================================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotals] 
	@sessionId nvarchar(50) = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.Description,
		vrm.VirtualResourceMetadataId,
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(*) Count
	FROM 
		OfficeWorkerActivityLog owa
	INNER JOIN
		VirtualResourceMetadata vrm on owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.Description,
		vrm.VirtualResourceMetadataId,
		vrm.MetadataType,
		owa.UpdateType
	ORDER BY
		vrm.Description desc,
		owa.UpdateType desc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityInstancesPerMinute]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by instance.
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityInstancesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.Description,
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour,
		DATEPART(MINUTE, owa.EndTime) AS Minute		
	FROM 
		OfficeWorkerActivityLog owa
	INNER JOIN 
		VirtualResourceMetadata vrm on owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(MINUTE, -@time, GETDATE())))
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.Description,
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime) asc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_OfficeWorkerActivityTypesBySecond]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 
-- Description:	Returns a summary of Office Worker activities by second
-- =============================================
CREATE PROCEDURE [dbo].[sel_OfficeWorkerActivityTypesBySecond] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour,
		DATEPART(MINUTE, owa.EndTime) AS Minute,		
		DATEPART(SECOND, owa.EndTime) AS Second		
	FROM 
		OfficeWorkerActivityLog owa,
		VirtualResourceMetadata vrm
	WHERE
		owa.MetadataId = vrm.VirtualResourceMetadataId
		and owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(MINUTE, -@time, GETDATE())))
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime),
		DATEPART(SECOND, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime),
		DATEPART(SECOND, owa.EndTime) asc

END
GO
/****** Object:  StoredProcedure [dbo].[sel_OfficeWorkerActivityTypesByMinute]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/24/2011
-- Description:	Selects Office Worker activities per minute
-- =============================================
CREATE PROCEDURE [dbo].[sel_OfficeWorkerActivityTypesByMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour,
		DATEPART(MINUTE, owa.EndTime) AS Minute		
	FROM 
		OfficeWorkerActivityLog owa,
		VirtualResourceMetadata vrm
	WHERE
		owa.MetadataId = vrm.VirtualResourceMetadataId
		and owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(MINUTE, -@time, GETDATE())))
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime) asc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_OfficeWorkerActivityTypesByHour]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/24/2011
-- Description:	Selects Office Worker activities per minute
-- =============================================
CREATE PROCEDURE [dbo].[sel_OfficeWorkerActivityTypesByHour] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour
	FROM 
		OfficeWorkerActivityLog owa,
		VirtualResourceMetadata vrm
	WHERE
		owa.MetadataId = vrm.VirtualResourceMetadataId
		and owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(HOUR, -@time, GETDATE())))
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime) asc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_OfficeWorkerActivityFailuresByHour]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 11/15/2011
-- Description:	Selects Office Worker activities that failed per hour
-- =============================================
CREATE PROCEDURE [dbo].[sel_OfficeWorkerActivityFailuresByHour] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour
	FROM 
		OfficeWorkerActivityLog owa,
		VirtualResourceMetadata vrm
	WHERE
		owa.MetadataId = vrm.VirtualResourceMetadataId
		and owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(HOUR, -@time, GETDATE())))
		and owa.UpdateType = 'ActivityFailed'
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime) asc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_OfficeWorkerActivities]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Cambron
-- Create date: 2/24/2011
-- Description:	Selects Office Worker activities per minute
-- =============================================
CREATE PROCEDURE [dbo].[sel_OfficeWorkerActivities] 
	@sessionId nvarchar(50) = 0,
	@numberOfRows int = 1000
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP (@numberOfRows)
		owa.SessionId,
		vrm.MetadataType,
		vrm.[Description] AS ActivityDescription,
		owa.UpdateType,
		owa.StartTime,
		owa.EndTime,
		owa.Hostname,
		owa.Username,
		owa.Result,
		owa.Error,
		owa.Data,
		owa.ActivityId,
		owa.MetadataId
		
	FROM 
		OfficeWorkerActivityLog owa 
		INNER JOIN VirtualResourceMetadata vrm ON owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
	ORDER BY 
		EndTime asc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypeTotalsErrors]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a summary of Errors for a metadatatype/updatetype pair
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors] 
	@sessionId nvarchar(50) = 0,
	@metadataType nvarchar(50) = 0,
	@updateType nvarchar(50) = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		owa.Error,
		COUNT(*) ErrorCount
	FROM
		OfficeWorkerActivityLog owa
	INNER JOIN
		VirtualResourceMetadata vrm on owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
		and vrm.MetadataType = @metadataType
		and owa.UpdateType = @updateType
	GROUP BY
		owa.Error

END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypeTotals]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/16/2012
-- Description:	Returns a count of office worker activities grouped by metadata type.
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypeTotals] 
	@sessionId nvarchar(50) = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(*) Count
	FROM 
		OfficeWorkerActivityLog owa
	INNER JOIN
		VirtualResourceMetadata vrm on owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType
	ORDER BY
		vrm.MetadataType desc,
		owa.UpdateType desc
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_ActivityTypesPerMinute]    Script Date: 07/16/2012 12:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BJ Myers
-- Create date: 5/17/2012
-- Description:	Returns a count of office worker activities per minute, grouped by metadata type.
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_ActivityTypesPerMinute] 
	@sessionId nvarchar(50) = 0,
	@time int = 0 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		vrm.MetadataType,
		owa.UpdateType,
		COUNT(vrm.MetadataType) AS Count, 
		DATEPART(YY, owa.EndTime) AS Year,
		DATEPART(MONTH, owa.EndTime) AS Month,
		DATEPART(DAY, owa.EndTime) AS Day,
		DATEPART(HOUR, owa.EndTime) AS Hour,
		DATEPART(MINUTE, owa.EndTime) AS Minute		
	FROM 
		OfficeWorkerActivityLog owa
	INNER JOIN 
		VirtualResourceMetadata vrm on owa.MetadataId = vrm.VirtualResourceMetadataId
	WHERE
		owa.SessionId = @sessionId
		and (@time = 0 or (@time > 0 and owa.EndTime > DATEADD(MINUTE, -@time, GETDATE())))
		and owa.UpdateType <> 'ActivityStarted'
	GROUP BY
		vrm.MetadataType,
		owa.UpdateType,
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime)
	ORDER BY 
		DATEPART(YY, owa.EndTime),
		DATEPART(MONTH, owa.EndTime),
		DATEPART(DAY, owa.EndTime),
		DATEPART(HOUR, owa.EndTime),
		DATEPART(MINUTE, owa.EndTime) asc
END
GO
/****** Object:  Table [dbo].[MetadataResourceUsage]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataResourceUsage](
	[ResourceId] [varchar](50) NOT NULL,
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[ResourceType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MetadataResourceUsage] PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC,
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ActivityExceptionSetting]    Script Date: 07/16/2012 12:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ActivityExceptionSetting](
	[SettingId] [uniqueidentifier] NOT NULL,
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ExceptionType] [varchar](50) NOT NULL,
	[Action] [varchar](30) NOT NULL,
	[RetryLimit] [int] NOT NULL,
	[RetryDelay] [int] NOT NULL,
	[RetryAction] [varchar](30) NOT NULL,
 CONSTRAINT [PK_ActivityExceptionSetting_1] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_DomainAccountReservation_StartIndex]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[DomainAccountReservation] ADD  CONSTRAINT [DF_DomainAccountReservation_StartIndex]  DEFAULT ((0)) FOR [StartIndex]
GO
/****** Object:  Default [DF_DomainAccountReservation_Count]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[DomainAccountReservation] ADD  CONSTRAINT [DF_DomainAccountReservation_Count]  DEFAULT ((0)) FOR [Count]
GO
/****** Object:  Default [DF_EndpointResponder_SnmpEnabled]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EndpointResponder] ADD  CONSTRAINT [DF_EndpointResponder_SnmpEnabled]  DEFAULT ((0)) FOR [SnmpEnabled]
GO
/****** Object:  Default [DF_EndpointResponder_WebServicesEnabled]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EndpointResponder] ADD  CONSTRAINT [DF_EndpointResponder_WebServicesEnabled]  DEFAULT ((0)) FOR [WebServicesEnabled]
GO
/****** Object:  Default [DF_EventLogCollector_PollingInterval]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EventLogCollector] ADD  CONSTRAINT [DF_EventLogCollector_PollingInterval]  DEFAULT ((5)) FOR [PollingInterval]
GO
/****** Object:  Default [DF_OfficeWorker_DurationMode]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[OfficeWorker] ADD  CONSTRAINT [DF_OfficeWorker_DurationMode]  DEFAULT ('Iteration') FOR [RunMode]
GO
/****** Object:  Default [DF_OfficeWorker_DurationTime]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[OfficeWorker] ADD  CONSTRAINT [DF_OfficeWorker_DurationTime]  DEFAULT ((0)) FOR [DurationTime]
GO
/****** Object:  Default [DF_PrintQueue_DevicePlatform]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintQueue] ADD  CONSTRAINT [DF_PrintQueue_DevicePlatform]  DEFAULT ('Physical') FOR [DevicePlatform]
GO
/****** Object:  Default [DF_PrintQueue_Active]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintQueue] ADD  CONSTRAINT [DF_PrintQueue_Active]  DEFAULT ((1)) FOR [Active]
GO
/****** Object:  Default [DF_PrintServer_Type]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintServer] ADD  CONSTRAINT [DF_PrintServer_Type]  DEFAULT ('Standard') FOR [ServerType]
GO
/****** Object:  Default [DF_PrintServer_Active]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintServer] ADD  CONSTRAINT [DF_PrintServer_Active]  DEFAULT ((1)) FOR [Active]
GO
/****** Object:  Default [DF_SessionInfo_ShutdownState]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[SessionInfo] ADD  CONSTRAINT [DF_SessionInfo_ShutdownState]  DEFAULT ('Unknown') FOR [ShutdownState]
GO
/****** Object:  Default [DF_VirtualResource_Platform]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResource] ADD  CONSTRAINT [DF_VirtualResource_Platform]  DEFAULT ('NONE') FOR [Platform]
GO
/****** Object:  Default [DF__VirtualRe__Enabl__26EFBBC6]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResource] ADD  DEFAULT ((1)) FOR [Enabled]
GO
/****** Object:  Default [DF_VirtualResourceMetatdata_VirtualResourceId]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceMetadata] ADD  CONSTRAINT [DF_VirtualResourceMetatdata_VirtualResourceId]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [VirtualResourceId]
GO
/****** Object:  ForeignKey [FK_ActivityExceptionSetting_OfficeWorker]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ActivityExceptionSetting]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExceptionSetting_OfficeWorker] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[OfficeWorker] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExceptionSetting] CHECK CONSTRAINT [FK_ActivityExceptionSetting_OfficeWorker]
GO
/****** Object:  ForeignKey [FK_AdminWorker_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[AdminWorker]  WITH CHECK ADD  CONSTRAINT [FK_AdminWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AdminWorker] CHECK CONSTRAINT [FK_AdminWorker_VirtualResource]
GO
/****** Object:  ForeignKey [FK_EndpointResponder_ProductSnmpProfile]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EndpointResponder]  WITH CHECK ADD  CONSTRAINT [FK_EndpointResponder_ProductSnmpProfile] FOREIGN KEY([ProductSnmpProfileId])
REFERENCES [dbo].[ProductSnmpProfile] ([ProductSnmpProfileId])
GO
ALTER TABLE [dbo].[EndpointResponder] CHECK CONSTRAINT [FK_EndpointResponder_ProductSnmpProfile]
GO
/****** Object:  ForeignKey [FK_EndpointResponder_ProductWSProfile]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EndpointResponder]  WITH CHECK ADD  CONSTRAINT [FK_EndpointResponder_ProductWSProfile] FOREIGN KEY([ProductWSProfileId])
REFERENCES [dbo].[ProductWSProfile] ([ProductWSProfileId])
GO
ALTER TABLE [dbo].[EndpointResponder] CHECK CONSTRAINT [FK_EndpointResponder_ProductWSProfile]
GO
/****** Object:  ForeignKey [FK_EndpointResponder_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EndpointResponder]  WITH CHECK ADD  CONSTRAINT [FK_EndpointResponder_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EndpointResponder] CHECK CONSTRAINT [FK_EndpointResponder_VirtualResource]
GO
/****** Object:  ForeignKey [FK_EnterpriseScenario_TreeViewFolder]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EnterpriseScenario]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenario_TreeViewFolder] FOREIGN KEY([TreeViewFolderId])
REFERENCES [dbo].[TreeViewFolder] ([TreeViewFolderId])
GO
ALTER TABLE [dbo].[EnterpriseScenario] CHECK CONSTRAINT [FK_EnterpriseScenario_TreeViewFolder]
GO
/****** Object:  ForeignKey [FK_EpicPrintService_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EpicPrintService]  WITH CHECK ADD  CONSTRAINT [FK_EpicPrintService_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EpicPrintService] CHECK CONSTRAINT [FK_EpicPrintService_VirtualResource]
GO
/****** Object:  ForeignKey [FK_EventLogCollector_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[EventLogCollector]  WITH CHECK ADD  CONSTRAINT [FK_EventLogCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventLogCollector] CHECK CONSTRAINT [FK_EventLogCollector_VirtualResource]
GO
/****** Object:  ForeignKey [FK_IPAddressReservation_IPAddressPool1]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[IPAddressReservation]  WITH CHECK ADD  CONSTRAINT [FK_IPAddressReservation_IPAddressPool1] FOREIGN KEY([IPAddressPoolId])
REFERENCES [dbo].[IPAddressPool] ([IPAddressPoolId])
GO
ALTER TABLE [dbo].[IPAddressReservation] CHECK CONSTRAINT [FK_IPAddressReservation_IPAddressPool1]
GO
/****** Object:  ForeignKey [FK_LowLayerTester_LowLayerTestProduct]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[LowLayerTester]  WITH CHECK ADD  CONSTRAINT [FK_LowLayerTester_LowLayerTestProduct] FOREIGN KEY([ScriptId])
REFERENCES [dbo].[LowLayerTestProduct] ([ScriptId])
GO
ALTER TABLE [dbo].[LowLayerTester] CHECK CONSTRAINT [FK_LowLayerTester_LowLayerTestProduct]
GO
/****** Object:  ForeignKey [FK_LowLayerTester_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[LowLayerTester]  WITH CHECK ADD  CONSTRAINT [FK_LowLayerTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LowLayerTester] CHECK CONSTRAINT [FK_LowLayerTester_VirtualResource]
GO
/****** Object:  ForeignKey [FK_MetadataResourceUsage_VirtualResourceMetadata]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[MetadataResourceUsage]  WITH CHECK ADD  CONSTRAINT [FK_MetadataResourceUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MetadataResourceUsage] CHECK CONSTRAINT [FK_MetadataResourceUsage_VirtualResourceMetadata]
GO
/****** Object:  ForeignKey [FK_OfficeWorker_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[OfficeWorker]  WITH CHECK ADD  CONSTRAINT [FK_OfficeWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OfficeWorker] CHECK CONSTRAINT [FK_OfficeWorker_VirtualResource]
GO
/****** Object:  ForeignKey [FK_OfficeWorkerActivityLogData_OfficeWorkerActivityLog]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[OfficeWorkerActivityLogData]  WITH CHECK ADD  CONSTRAINT [FK_OfficeWorkerActivityLogData_OfficeWorkerActivityLog] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[OfficeWorkerActivityLog] ([ActivityId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OfficeWorkerActivityLogData] CHECK CONSTRAINT [FK_OfficeWorkerActivityLogData_OfficeWorkerActivityLog]
GO
/****** Object:  ForeignKey [FK_PrintDriver_PrintDriverPackage]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintDriver]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriver_PrintDriverPackage] FOREIGN KEY([PrintDriverPackageId])
REFERENCES [dbo].[PrintDriverPackage] ([PrintDriverPackageId])
GO
ALTER TABLE [dbo].[PrintDriver] CHECK CONSTRAINT [FK_PrintDriver_PrintDriverPackage]
GO
/****** Object:  ForeignKey [FK_PrintQueue_PrintServer]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[PrintQueue]  WITH CHECK ADD  CONSTRAINT [FK_PrintQueue_PrintServer] FOREIGN KEY([PrintServerId])
REFERENCES [dbo].[PrintServer] ([PrintServerId])
GO
ALTER TABLE [dbo].[PrintQueue] CHECK CONSTRAINT [FK_PrintQueue_PrintServer]
GO
/****** Object:  ForeignKey [FK_ProductSnmpProfile_ProductMib]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ProductSnmpProfile]  WITH CHECK ADD  CONSTRAINT [FK_ProductSnmpProfile_ProductMib] FOREIGN KEY([ProductMibId])
REFERENCES [dbo].[ProductMib] ([ProductMibId])
GO
ALTER TABLE [dbo].[ProductSnmpProfile] CHECK CONSTRAINT [FK_ProductSnmpProfile_ProductMib]
GO
/****** Object:  ForeignKey [FK_QtpResource_QtpTest]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[QtpResource]  WITH CHECK ADD  CONSTRAINT [FK_QtpResource_QtpTest] FOREIGN KEY([QtpTestId])
REFERENCES [dbo].[QtpTest] ([QtpTestId])
GO
ALTER TABLE [dbo].[QtpResource] CHECK CONSTRAINT [FK_QtpResource_QtpTest]
GO
/****** Object:  ForeignKey [FK_QtpResource_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[QtpResource]  WITH CHECK ADD  CONSTRAINT [FK_QtpResource_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QtpResource] CHECK CONSTRAINT [FK_QtpResource_VirtualResource]
GO
/****** Object:  ForeignKey [FK_QtpTest_QtpTestPackage]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[QtpTest]  WITH CHECK ADD  CONSTRAINT [FK_QtpTest_QtpTestPackage] FOREIGN KEY([QtpTestPackageId])
REFERENCES [dbo].[QtpTestPackage] ([QtpTestPackageId])
GO
ALTER TABLE [dbo].[QtpTest] CHECK CONSTRAINT [FK_QtpTest_QtpTestPackage]
GO
/****** Object:  ForeignKey [FK_QtpTestParameter_QtpTest]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[QtpTestParameter]  WITH CHECK ADD  CONSTRAINT [FK_QtpTestParameter_QtpTest] FOREIGN KEY([QtpTestId])
REFERENCES [dbo].[QtpTest] ([QtpTestId])
GO
ALTER TABLE [dbo].[QtpTestParameter] CHECK CONSTRAINT [FK_QtpTestParameter_QtpTest]
GO
/****** Object:  ForeignKey [FK_ResourceTypePlatformAssoc_ResourceType]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ResourceTypePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypePlatformAssoc_ResourceType] FOREIGN KEY([Name])
REFERENCES [dbo].[ResourceType] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc] CHECK CONSTRAINT [FK_ResourceTypePlatformAssoc_ResourceType]
GO
/****** Object:  ForeignKey [FK_ResourceTypePlatformAssoc_VirtualMachinePlatform]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ResourceTypePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypePlatformAssoc_VirtualMachinePlatform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[VirtualMachinePlatform] ([PlatformId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc] CHECK CONSTRAINT [FK_ResourceTypePlatformAssoc_VirtualMachinePlatform]
GO
/****** Object:  ForeignKey [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]
GO
/****** Object:  ForeignKey [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]
GO
/****** Object:  ForeignKey [FK_SavedReport_Report]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[SavedReport]  WITH CHECK ADD  CONSTRAINT [FK_SavedReport_Report] FOREIGN KEY([ReportId])
REFERENCES [dbo].[Report] ([ReportId])
GO
ALTER TABLE [dbo].[SavedReport] CHECK CONSTRAINT [FK_SavedReport_Report]
GO
/****** Object:  ForeignKey [FK_ScenarioProductAssoc_AssociatedProduct]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ScenarioProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioProductAssoc_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScenarioProductAssoc] CHECK CONSTRAINT [FK_ScenarioProductAssoc_AssociatedProduct]
GO
/****** Object:  ForeignKey [FK_ScenarioProductAssoc_EnterpriseScenario]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[ScenarioProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioProductAssoc_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioid])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScenarioProductAssoc] CHECK CONSTRAINT [FK_ScenarioProductAssoc_EnterpriseScenario]
GO
/****** Object:  ForeignKey [FK_SessionProductAssoc_AssociatedProduct]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_AssociatedProduct]
GO
/****** Object:  ForeignKey [FK_SessionProductAssoc_SessionInfo]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_SessionInfo] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionInfo] ([SessionId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_SessionInfo]
GO
/****** Object:  ForeignKey [FK_TreeViewFolder_TreeViewFolder]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[TreeViewFolder]  WITH CHECK ADD  CONSTRAINT [FK_TreeViewFolder_TreeViewFolder] FOREIGN KEY([ParentId])
REFERENCES [dbo].[TreeViewFolder] ([TreeViewFolderId])
GO
ALTER TABLE [dbo].[TreeViewFolder] CHECK CONSTRAINT [FK_TreeViewFolder_TreeViewFolder]
GO
/****** Object:  ForeignKey [FK_VirtualMachinePlatformAssoc_VirtualMachine]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachine] FOREIGN KEY([Name])
REFERENCES [dbo].[VirtualMachine] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc] CHECK CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachine]
GO
/****** Object:  ForeignKey [FK_VirtualMachinePlatformAssoc_VirtualMachinePlatform]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachinePlatform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[VirtualMachinePlatform] ([PlatformId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc] CHECK CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachinePlatform]
GO
/****** Object:  ForeignKey [FK_VirtualResource_EnterpriseScenario]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResource]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResource_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResource] CHECK CONSTRAINT [FK_VirtualResource_EnterpriseScenario]
GO
/****** Object:  ForeignKey [FK_VirtualResource_TreeViewFolder]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResource]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResource_TreeViewFolder] FOREIGN KEY([TreeViewFolderId])
REFERENCES [dbo].[TreeViewFolder] ([TreeViewFolderId])
GO
ALTER TABLE [dbo].[VirtualResource] CHECK CONSTRAINT [FK_VirtualResource_TreeViewFolder]
GO
/****** Object:  ForeignKey [FK_VirtualResourceDependencyChild_VirtualResourceDependency]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceDependency]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceDependencyChild_VirtualResourceDependency] FOREIGN KEY([VirtualResourceChildId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResourceDependency] CHECK CONSTRAINT [FK_VirtualResourceDependencyChild_VirtualResourceDependency]
GO
/****** Object:  ForeignKey [FK_VirtualResourceDependencyParent_VirtualResourceDependency]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceDependency]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceDependencyParent_VirtualResourceDependency] FOREIGN KEY([VirtualResourceParentId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
GO
ALTER TABLE [dbo].[VirtualResourceDependency] CHECK CONSTRAINT [FK_VirtualResourceDependencyParent_VirtualResourceDependency]
GO
/****** Object:  ForeignKey [FK_VirtualResourceMetadata_TreeViewFolder]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceMetadata]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadata_TreeViewFolder] FOREIGN KEY([TreeViewFolderId])
REFERENCES [dbo].[TreeViewFolder] ([TreeViewFolderId])
GO
ALTER TABLE [dbo].[VirtualResourceMetadata] CHECK CONSTRAINT [FK_VirtualResourceMetadata_TreeViewFolder]
GO
/****** Object:  ForeignKey [FK_VirtualResourceMetadata_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceMetadata]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResourceMetadata] CHECK CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource]
GO
/****** Object:  ForeignKey [FK_VirtualResourceResourceUsage_VirtualResource]    Script Date: 07/16/2012 12:21:39 ******/
ALTER TABLE [dbo].[VirtualResourceResourceUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceResourceUsage_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResourceResourceUsage] CHECK CONSTRAINT [FK_VirtualResourceResourceUsage_VirtualResource]
GO
