USE [EnterpriseTest]
GO
/****** Object:  Table [dbo].[ActiveDirectoryGroup]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveDirectoryGroup](
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ActiveDirectoryGroup] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActivityExceptionSetting]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AdminWorker]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AdminWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ExecutionMode] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AdminWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssociatedProduct]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CitrixWorker]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CitrixWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ServerHostname] [nvarchar](50) NOT NULL,
	[DBWorkerRunMode] [nvarchar](50) NOT NULL,
	[PublishedApp] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CitrixWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ConfigurationObjectHistory]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConfigurationObjectHistory](
	[ConfigurationObjectHistoryId] [uniqueidentifier] NOT NULL,
	[ConfigurationObjectId] [uniqueidentifier] NOT NULL,
	[ModificationType] [varchar](50) NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[Modifier] [varchar](50) NOT NULL,
	[ConfigurationObjectName] [nvarchar](255) NULL,
	[ConfigurationObjectType] [varchar](50) NULL,
 CONSTRAINT [PK_ConfigurationObjectHistory] PRIMARY KEY CLUSTERED 
(
	[ConfigurationObjectHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConfigurationTreeFolder]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConfigurationTreeFolder](
	[ConfigurationTreeFolderId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[FolderType] [varchar](50) NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ConfigurationTreeFolder] PRIMARY KEY CLUSTERED 
(
	[ConfigurationTreeFolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DigitalSendOutputLocation]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalSendOutputLocation](
	[DigitalSendOutputLocationId] [uniqueidentifier] NOT NULL,
	[ServerHostName] [varchar](50) NOT NULL,
	[OutputType] [varchar](50) NOT NULL,
	[MonitorLocation] [varchar](255) NOT NULL,
	[ValidationOptions] [xml] NOT NULL,
 CONSTRAINT [PK_DigitalSendOutputLocation] PRIMARY KEY CLUSTERED 
(
	[DigitalSendOutputLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EnterpriseScenario]    Script Date: 9/24/2015 3:58:36 PM ******/
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
	[Deleted] [bit] NOT NULL DEFAULT ((0)),
	[FolderId] [uniqueidentifier] NULL,
	[Owner] [varchar](50) NOT NULL CONSTRAINT [ScenarioDefault]  DEFAULT ('Unknown'),
 CONSTRAINT [PK_EnterpriseScenario] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EnterpriseScenarioGroupAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EnterpriseScenarioGroupAssoc](
	[ScenarioId] [uniqueidentifier] NOT NULL,
	[GroupName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EnterpriseScenarioGroupAssoc] PRIMARY KEY CLUSTERED 
(
	[ScenarioId] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EprAlert]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[EprAlert](
	[AlertName] [varchar](50) NULL,
	[AlertType] [varchar](50) NULL,
	[FirmwareType] [varchar](50) NULL,
	[Product] [varchar](50) NULL,
	[AlertSchema] [xml] NULL,
	[AlertId] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlertId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EventLogCollector]    Script Date: 9/24/2015 3:58:36 PM ******/
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
	[PollingInterval] [int] NOT NULL CONSTRAINT [DF_EventLogCollector_PollingInterval]  DEFAULT ((5)),
 CONSTRAINT [PK_EventLogCollector] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoadTester]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoadTester](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ThreadsPerVM] [int] NOT NULL,
 CONSTRAINT [PK_LoadTester] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetadataInstallerPackageAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataInstallerPackageAssoc](
	[MetadataType] [varchar](50) NOT NULL,
	[InstallerPackageId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MetadataInstallerPackageAssoc] PRIMARY KEY CLUSTERED 
(
	[MetadataType] ASC,
	[InstallerPackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataResourceUsage]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataType]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataType](
	[Name] [varchar](50) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
 CONSTRAINT [PK_MetadataType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MetadataTypeResourceTypeAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataTypeResourceTypeAssoc](
	[MetadataTypeName] [varchar](50) NOT NULL,
	[ResourceTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MetadataTypeResourceTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[MetadataTypeName] ASC,
	[ResourceTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OfficeWorker]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OfficeWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[RepeatCount] [int] NOT NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[RandomizeActivities] [bit] NOT NULL,
	[RandomizeActivityDelay] [bit] NOT NULL,
	[MinActivityDelay] [int] NOT NULL,
	[MaxActivityDelay] [int] NOT NULL,
	[RunMode] [varchar](50) NOT NULL CONSTRAINT [DF_OfficeWorker_DurationMode]  DEFAULT ('Iteration'),
	[DurationTime] [int] NOT NULL CONSTRAINT [DF_OfficeWorker_DurationTime]  DEFAULT ((0)),
	[SecurityGroups] [xml] NULL,
	[ExecutionSchedule] [xml] NULL,
	[UserPool] [nvarchar](50) NOT NULL CONSTRAINT [DF_OfficeWorker_UserPool]  DEFAULT ('User'),
 CONSTRAINT [PK_OfficeWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PerfMonCollector]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PerfMonCollector](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[HostName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PerfMonCollector] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceInstallerPackageAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResourceInstallerPackageAssoc](
	[ResourceType] [varchar](50) NOT NULL,
	[InstallerPackageId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ResourceInstallerPackageAssoc] PRIMARY KEY CLUSTERED 
(
	[ResourceType] ASC,
	[InstallerPackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceType]    Script Date: 9/24/2015 3:58:36 PM ******/
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
	[PluginEnabled] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_VirtualResourceType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceTypePlatformAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsCategory]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResourceWindowsCategoryParent]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScenarioProductAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScenarioProductAssoc](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ScenarioProductAssoc] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC,
	[AssociatedProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScenarioSession]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScenarioSession](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Notes] [varchar](max) NULL,
	[EditedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ScenarioSession] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionInfo]    Script Date: 9/24/2015 3:58:36 PM ******/
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
	[Notes] [varchar](max) NULL,
	[ShutdownState] [varchar](50) NOT NULL CONSTRAINT [DF_SessionInfo_ShutdownState]  DEFAULT ('Unknown'),
	[Dispatcher] [varchar](50) NOT NULL,
	[ProjectedEndDate] [datetime] NULL,
	[ExpirationDate] [datetime] NOT NULL DEFAULT (dateadd(week,(2),getdate())),
	[ShutdownUser] [varchar](50) NULL,
	[ShutdownDate] [datetime] NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SessionProductAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SoftwareInstaller]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SoftwareInstaller](
	[InstallerId] [uniqueidentifier] NOT NULL,
	[Description] [varchar](500) NULL,
	[FilePath] [varchar](150) NOT NULL,
	[Arguments] [varchar](250) NULL,
	[RebootSetting] [varchar](50) NOT NULL,
	[CopyDirectory] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Installer] PRIMARY KEY CLUSTERED 
(
	[InstallerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SoftwareInstallerPackage]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SoftwareInstallerPackage](
	[PackageId] [uniqueidentifier] NOT NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_InstallerPackage] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SoftwareInstallerSetting]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoftwareInstallerSetting](
	[PackageId] [uniqueidentifier] NOT NULL,
	[InstallerId] [uniqueidentifier] NOT NULL,
	[InstallOrderNumber] [int] NOT NULL,
 CONSTRAINT [PK_SoftwareInstallerSetting] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC,
	[InstallerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SolutionTester]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolutionTester](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[CredentialType] [nvarchar](50) NOT NULL,
	[UseCredential] [bit] NOT NULL,
	[CredentialName] [nvarchar](50) NULL,
	[CredentialDomain] [nvarchar](50) NULL,
	[CredentialPassword] [nvarchar](1024) NULL,
 CONSTRAINT [PK_SolutionTester] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemSetting]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemSetting](
	[Type] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_SystemSetting] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserName] [varchar](50) NOT NULL,
	[Domain] [varchar](50) NOT NULL CONSTRAINT [DF_User_Domain]  DEFAULT (''),
	[RoleName] [varchar](50) NOT NULL CONSTRAINT [GuestDefault]  DEFAULT ('Guest'),
	[VCenterPassword] [varchar](100) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserGroup](
	[GroupName] [varchar](50) NOT NULL,
	[Creator] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_OperatorGroup] PRIMARY KEY CLUSTERED 
(
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserGroupAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserGroupAssoc](
	[UserName] [varchar](50) NOT NULL,
	[GroupName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UserGroupAssoc] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ValueGroup]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VirtualMachineGroupAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachineGroupAssoc](
	[MachineName] [varchar](50) NOT NULL,
	[GroupName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualMachineGroupAssoc_1] PRIMARY KEY CLUSTERED 
(
	[MachineName] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachinePlatform]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachinePlatformAssoc]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualResource]    Script Date: 9/24/2015 3:58:36 PM ******/
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
	[Description] [nvarchar](500) NULL,
	[ResourceType] [varchar](50) NOT NULL,
	[InstanceCount] [int] NOT NULL,
	[Platform] [varchar](50) NOT NULL CONSTRAINT [DF_VirtualResource_Platform]  DEFAULT ('NONE'),
	[Enabled] [bit] NOT NULL DEFAULT ((1)),
	[ResourcesPerVM] [int] NULL,
	[Deleted] [bit] NOT NULL DEFAULT ((0)),
	[FolderId] [uniqueidentifier] NULL,
	[TestCaseId] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_VirtualResource] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualResourceMetadata]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualResourceMetadata](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ResourceType] [varchar](50) NOT NULL,
	[MetadataType] [varchar](50) NOT NULL,
	[Metadata] [xml] NOT NULL,
	[VirtualResourceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_VirtualResourceMetatdata_VirtualResourceId]  DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[Enabled] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL DEFAULT ((0)),
	[FolderId] [uniqueidentifier] NULL,
	[ExecutionPlan] [xml] NULL,
 CONSTRAINT [PK_VirtualResourceMetadata] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VPNConfiguration]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VPNConfiguration](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ServerIp] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Domain] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VPNConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WebSite]    Script Date: 9/24/2015 3:58:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SolutionTester] ADD  DEFAULT ((0)) FOR [UseCredential]
GO
ALTER TABLE [dbo].[ActivityExceptionSetting]  WITH CHECK ADD  CONSTRAINT [FK_ActivityExceptionSetting_OfficeWorker] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[OfficeWorker] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActivityExceptionSetting] CHECK CONSTRAINT [FK_ActivityExceptionSetting_OfficeWorker]
GO
ALTER TABLE [dbo].[AdminWorker]  WITH CHECK ADD  CONSTRAINT [FK_AdminWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AdminWorker] CHECK CONSTRAINT [FK_AdminWorker_VirtualResource]
GO
ALTER TABLE [dbo].[CitrixWorker]  WITH CHECK ADD  CONSTRAINT [FK_CitrixWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CitrixWorker] CHECK CONSTRAINT [FK_CitrixWorker_VirtualResource]
GO
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario]
GO
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_OperatorGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_OperatorGroup]
GO
ALTER TABLE [dbo].[EventLogCollector]  WITH CHECK ADD  CONSTRAINT [FK_EventLogCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventLogCollector] CHECK CONSTRAINT [FK_EventLogCollector_VirtualResource]
GO
ALTER TABLE [dbo].[LoadTester]  WITH CHECK ADD  CONSTRAINT [FK_LoadTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoadTester] CHECK CONSTRAINT [FK_LoadTester_VirtualResource]
GO
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataInstallerPackageAssoc_InstallerPackage] FOREIGN KEY([InstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] CHECK CONSTRAINT [FK_MetadataInstallerPackageAssoc_InstallerPackage]
GO
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType] FOREIGN KEY([MetadataType])
REFERENCES [dbo].[MetadataType] ([Name])
GO
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] CHECK CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType]
GO
ALTER TABLE [dbo].[MetadataResourceUsage]  WITH CHECK ADD  CONSTRAINT [FK_MetadataResourceUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MetadataResourceUsage] CHECK CONSTRAINT [FK_MetadataResourceUsage_VirtualResourceMetadata]
GO
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
GO
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] CHECK CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType]
GO
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
GO
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] CHECK CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_ResourceType]
GO
ALTER TABLE [dbo].[OfficeWorker]  WITH CHECK ADD  CONSTRAINT [FK_OfficeWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OfficeWorker] CHECK CONSTRAINT [FK_OfficeWorker_VirtualResource]
GO
ALTER TABLE [dbo].[PerfMonCollector]  WITH CHECK ADD  CONSTRAINT [FK_PerfMonCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PerfMonCollector] CHECK CONSTRAINT [FK_PerfMonCollector_VirtualResource]
GO
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceInstallerPackageAssoc_InstallerPackage] FOREIGN KEY([InstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc] CHECK CONSTRAINT [FK_ResourceInstallerPackageAssoc_InstallerPackage]
GO
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceInstallerPackageAssoc_ResourceType] FOREIGN KEY([ResourceType])
REFERENCES [dbo].[ResourceType] ([Name])
GO
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc] CHECK CONSTRAINT [FK_ResourceInstallerPackageAssoc_ResourceType]
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypePlatformAssoc_ResourceType] FOREIGN KEY([Name])
REFERENCES [dbo].[ResourceType] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc] CHECK CONSTRAINT [FK_ResourceTypePlatformAssoc_ResourceType]
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypePlatformAssoc_VirtualMachinePlatform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[VirtualMachinePlatform] ([PlatformId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceTypePlatformAssoc] CHECK CONSTRAINT [FK_ResourceTypePlatformAssoc_VirtualMachinePlatform]
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]
GO
ALTER TABLE [dbo].[ScenarioProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioProductAssoc_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScenarioProductAssoc] CHECK CONSTRAINT [FK_ScenarioProductAssoc_AssociatedProduct]
GO
ALTER TABLE [dbo].[ScenarioProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioProductAssoc_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScenarioProductAssoc] CHECK CONSTRAINT [FK_ScenarioProductAssoc_EnterpriseScenario]
GO
ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_AssociatedProduct]
GO
ALTER TABLE [dbo].[SessionProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_SessionProductAssoc_SessionInfo] FOREIGN KEY([SessionId])
REFERENCES [dbo].[SessionInfo] ([SessionId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SessionProductAssoc] CHECK CONSTRAINT [FK_SessionProductAssoc_SessionInfo]
GO
ALTER TABLE [dbo].[SoftwareInstallerSetting]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstaller] FOREIGN KEY([InstallerId])
REFERENCES [dbo].[SoftwareInstaller] ([InstallerId])
GO
ALTER TABLE [dbo].[SoftwareInstallerSetting] CHECK CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstaller]
GO
ALTER TABLE [dbo].[SoftwareInstallerSetting]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstallerPackage] FOREIGN KEY([PackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
ALTER TABLE [dbo].[SoftwareInstallerSetting] CHECK CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstallerPackage]
GO
ALTER TABLE [dbo].[SolutionTester]  WITH CHECK ADD  CONSTRAINT [FK_SolutionTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SolutionTester] CHECK CONSTRAINT [FK_SolutionTester_VirtualResource]
GO
ALTER TABLE [dbo].[UserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_UserGroupAssoc_User] FOREIGN KEY([UserName])
REFERENCES [dbo].[User] ([UserName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroupAssoc] CHECK CONSTRAINT [FK_UserGroupAssoc_User]
GO
ALTER TABLE [dbo].[UserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_UserGroupAssoc_UserGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroupAssoc] CHECK CONSTRAINT [FK_UserGroupAssoc_UserGroup]
GO
ALTER TABLE [dbo].[VirtualMachineGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_VirtualMachineGroupAssoc_OperatorGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualMachineGroupAssoc] CHECK CONSTRAINT [FK_VirtualMachineGroupAssoc_OperatorGroup]
GO
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachinePlatform] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[VirtualMachinePlatform] ([PlatformId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualMachinePlatformAssoc] CHECK CONSTRAINT [FK_VirtualMachinePlatformAssoc_VirtualMachinePlatform]
GO
ALTER TABLE [dbo].[VirtualResource]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResource_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResource] CHECK CONSTRAINT [FK_VirtualResource_EnterpriseScenario]
GO
ALTER TABLE [dbo].[VirtualResourceMetadata]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualResourceMetadata] CHECK CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource]
GO
/****** Object:  StoredProcedure [dbo].[del_SessionInfoCascading]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Kelly Youngman
-- Create date: 4/6/2011
-- Description:	Deletes a SessionInfo record and all associated OfficeWorkerActivityData records
--              Also clears the PrintJobLog table.
-- Updated:		2/16/2011 - VirtualMachineInfo and VirtualResourceInfo tables deleted. -bmyers
--				8/1/2012 - Added removal of data from EventLog and PerfMonLog tables - kyoungman
--				9/27/2012 - Added removal of data from ScanJobLog table. -bmyers
--				11/2/2012 - Clear out session-specific data from the VirtualMachine table - kyoungman
--				11/17/2012 - Changed references to tables that were moved to ScalableTestDataLog. -bmyers
--				1/24/2013 - Modified to use linked server connection for ScalableTestDataLog database. -bmyers
-- =============================================
CREATE PROCEDURE [dbo].[del_SessionInfoCascading] (
	@sessionId varchar(50)
	)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC ScalableTestDataLog.dbo.del_SessionData @sessionId
	DELETE SessionInfo WHERE SessionId = @sessionId
	--UPDATE VirtualMachine SET SessionId = NULL, PlatformUsage = '', LastUpdated = GETDATE() where SessionId = @sessionId

END






GO
/****** Object:  StoredProcedure [dbo].[sel_SessionsWithCounts]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		kyoungman
-- Create date: 5/10/2011
-- Description:	Returns a list of all Sessions with Activity and PrintJob counts
-- Updated: 5/15/2012 - Added ShutdownState to procedure. -aandrews
--          10/31/2012 - Added ExpirationDate and ScanJobCount. -bmyers
--			11/17/2012 - Modified counts to pull from ScalableTestDataLog table. -bmyers
--			1/24/2013 - Modified to use linked server connection for ScalableTestDataLog database. -bmyers
--			6/24/2013 - Added new shutdown columns. -bmyers
-- =============================================
CREATE PROCEDURE [dbo].[sel_SessionsWithCounts]
AS
BEGIN

	SET NOCOUNT ON;

	-- Create a temp table to hold the results from the ScalableTestDataLog stored procedure
	CREATE TABLE #Counts (SessionId VARCHAR(50), ActivityCount INT, PrintJobCount INT, ScanJobCount INT)
	INSERT INTO #Counts (SessionId, ActivityCount, PrintJobCount, ScanJobCount) EXEC ScalableTestDataLog.dbo.sel_SessionCounts

	SELECT
		s.SessionId, 
		s.[Status], 
		s.ShutdownState,
		s.StartDate,
		s.ExpirationDate,
		s.ScenarioName, 
		s.[Owner],
		s.ShutdownUser,
		s.ShutdownDate,
		c.ActivityCount,
		c.PrintJobCount,
		c.ScanJobCount
	FROM SessionInfo s
	INNER JOIN #Counts c on s.SessionId = c.SessionId
	ORDER BY s.StartDate desc
		
END


GO
/****** Object:  StoredProcedure [dbo].[upd_ReserveVMsByPlatformContiguous]    Script Date: 9/24/2015 3:58:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[upd_ReserveVMsByPlatformContiguous] (
	@requiredRange int,
	@platform nvarchar(50),
	@sessionId varchar(50)
	)
AS
BEGIN
	DECLARE @rangeStart INT
	DECLARE	@rangeTable TABLE (RangeStart int, RangeEnd int) --This will hold the list of ranges available

	select @rangeStart = 0
	-- The table aliasing here needs some explanation.  We're only selecting from one table here, but we're doing it 4 times for each record.
	-- I started out using l and r for left and right because of the utilization of the right join which helps us find the beginning and ending
	-- of each contiguous block.  For the inner query, I couldn't use l and r again, so I used a and b.  Not sure if ANY naming convention makes
	-- much sense in this case.
	insert into @rangeTable
	select l.SortOrder as start,
		(
			select min(a.SortOrder) as SortOrder
			from 
				VirtualMachine a left outer join 
				(
					select 
						* 
					from 
						VirtualMachine 
					where 
						UsageState = 'Available' and 
						PowerState = 'Powered Off' and
						Platform = @platform and
						HoldId is null
				) b on a.SortOrder = b.SortOrder - 1
			where 
				a.UsageState = 'Available' and 
				a.PowerState = 'Powered Off' and 
				a.Platform = @platform and
				a.HoldId is null and 
				b.SortOrder is null and 
				a.SortOrder >= l.SortOrder            		
		) as [end]
	from VirtualMachine l
		left outer join 
		(
			select 
				* 
			from 
				VirtualMachine 
			where 
				UsageState = 'Available' and 
				HoldId is null and 
				PowerState = 'Powered Off' and
				Platform = @platform
		) r on r.SortOrder = l.SortOrder - 1
	where 
		l.UsageState = 'Available' and 
		l.PowerState = 'Powered Off' and 
		l.Platform = @platform and
		l.HoldId is null and 
		r.SortOrder is null;

	--Retrieve the first block of VMs that will satisfy the required number
	select 
		top 1 @rangeStart = RangeStart 
	from 
		@rangeTable 
	where 
		(RangeEnd - RangeStart) + 1 >= @requiredRange 
	order by RangeStart
	
	if (@rangeStart = 0) begin
		raiserror (N'A contiguous block of %d VMs is not available.', 16, 1, @requiredRange)
	end
	else begin
		--Reserve the required number of VMs
		update 
			VirtualMachine 
		set 
			UsageState = 'Reserved', 
			SessionId = @sessionId, 
			LastUpdated = GETDATE()
		where 
			SortOrder between @rangeStart and 
			((@rangeStart + @requiredRange) - 1)
	end
	
END


GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Running, Problem, Completed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SessionInfo', @level2type=N'COLUMN',@level2name=N'Status'
GO
