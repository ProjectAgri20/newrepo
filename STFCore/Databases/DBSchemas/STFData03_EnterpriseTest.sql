
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[ActiveDirectoryGroup]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDirectoryGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActiveDirectoryGroup](
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_ActiveDirectoryGroup] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AdminWorker]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdminWorker]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AdminWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ExecutionMode] [varchar](20) NOT NULL,
 CONSTRAINT [PK_AdminWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AssociatedProduct]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssociatedProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssociatedProduct](
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Vendor] [nvarchar](50) NULL,
 CONSTRAINT [PK_AssociatedProduct] PRIMARY KEY CLUSTERED 
(
	[AssociatedProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AssociatedProductVersion]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssociatedProductVersion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssociatedProductVersion](
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[Version] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_AssociatedProductVersion] PRIMARY KEY CLUSTERED 
(
	[AssociatedProductId] ASC,
	[EnterpriseScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[CitrixWorker]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CitrixWorker]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CitrixWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ServerHostname] [nvarchar](50) NOT NULL,
	[DBWorkerRunMode] [varchar](20) NOT NULL,
	[PublishedApp] [nvarchar](50) NULL,
 CONSTRAINT [PK_CitrixWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ConfigurationTreeFolder]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationTreeFolder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigurationTreeFolder](
	[ConfigurationTreeFolderId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FolderType] [varchar](50) NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ConfigurationTreeFolder] PRIMARY KEY CLUSTERED 
(
	[ConfigurationTreeFolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[EnterpriseScenario]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenario]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EnterpriseScenario](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[Owner] [nvarchar](50) NOT NULL,
	[Vertical] [nvarchar](50) NULL,
	[Company] [nvarchar](50) NULL,
	[ScenarioSettings] [xml] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_EnterpriseScenario] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[EnterpriseScenarioGroupAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EnterpriseScenarioGroupAssoc](
	[ScenarioId] [uniqueidentifier] NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EnterpriseScenarioGroupAssoc] PRIMARY KEY CLUSTERED 
(
	[ScenarioId] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[EventLogCollector]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventLogCollector]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EventLogCollector](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[PollingInterval] [int] NOT NULL,
	[ComponentsData] [xml] NOT NULL,
	[EntryTypesData] [xml] NOT NULL,
 CONSTRAINT [PK_EventLogCollector] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[LoadTester]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoadTester]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LoadTester](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[ThreadsPerVM] [int] NOT NULL,
 CONSTRAINT [PK_LoadTester] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[MetadataInstallerPackageAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MetadataInstallerPackageAssoc](
	[InstallerPackageId] [uniqueidentifier] NOT NULL,
	[MetadataType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MetadataInstallerPackageAssoc] PRIMARY KEY CLUSTERED 
(
	[MetadataType] ASC,
	[InstallerPackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[MetadataType]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MetadataType](
	[Name] [nvarchar](50) NOT NULL,
	[AssemblyName] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Group] [nvarchar](50) NULL,
	[Icon] [varbinary](max) NULL,
 CONSTRAINT [PK_MetadataType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'MetadataType', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MetadataType'
GO

/******  Object:  Table [dbo].[MetadataTypeResourceTypeAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MetadataTypeResourceTypeAssoc](
	[ResourceTypeName] [varchar](50) NOT NULL,
	[MetadataTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MetadataTypeResourceTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[MetadataTypeName] ASC,
	[ResourceTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[OfficeWorker]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OfficeWorker]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OfficeWorker](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[RunMode] [varchar](50) NOT NULL,
	[RepeatCount] [int] NOT NULL,
	[DurationTime] [int] NOT NULL,
	[RandomizeActivities] [bit] NOT NULL,
	[RandomizeStartupDelay] [bit] NOT NULL,
	[MinStartupDelay] [int] NOT NULL,
	[MaxStartupDelay] [int] NOT NULL,
	[RandomizeActivityDelay] [bit] NOT NULL,
	[MinActivityDelay] [int] NOT NULL,
	[MaxActivityDelay] [int] NOT NULL,
	[UserPool] [nvarchar](50) NOT NULL,
	[SecurityGroups] [xml] NULL,
	[ExecutionSchedule] [xml] NULL,
 CONSTRAINT [PK_OfficeWorker] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PerfMonCollector]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerfMonCollector]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PerfMonCollector](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PerfMonCollector] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ProductPluginAssociation]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductPluginAssociation](
	[MetadataType] [nvarchar](50) NOT NULL,
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProductPluginAssociation] PRIMARY KEY CLUSTERED 
(
	[MetadataType] ASC,
	[AssociatedProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ProductPluginAssociation', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPluginAssociation'
GO

/******  Object:  Table [dbo].[ResourceInstallerPackageAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceInstallerPackageAssoc](
	[InstallerPackageId] [uniqueidentifier] NOT NULL,
	[ResourceType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceInstallerPackageAssoc] PRIMARY KEY CLUSTERED 
(
	[ResourceType] ASC,
	[InstallerPackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ResourceType]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceType](
	[Name] [varchar](50) NOT NULL,
	[MaxResourcesPerHost] [int] NOT NULL,
	[PluginEnabled] [bit] NOT NULL,
	[Platform] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualResourceType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ResourceTypeFrameworkClientPlatformAssociation]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation](
	[FrameworkClientPlatformId] [nvarchar](50) NOT NULL,
	[ResourceTypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ResourceTypeFrameworkClientPlatformAssociation] PRIMARY KEY CLUSTERED 
(
	[FrameworkClientPlatformId] ASC,
	[ResourceTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ResourceWindowsCategory]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceWindowsCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryType] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ResourceWindowsCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ResourceWindowsCategoryParent]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ResourceWindowsCategoryParent](
	[CategoryId] [int] NOT NULL,
	[ParentCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_ResourceWindowsCategoryParent] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[ParentCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ScenarioSession]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScenarioSession]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ScenarioSession](
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[EditedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ScenarioSession] PRIMARY KEY CLUSTERED 
(
	[EnterpriseScenarioId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SoftwareInstaller]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstaller]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SoftwareInstaller](
	[InstallerId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[FilePath] [nvarchar](255) NOT NULL,
	[Arguments] [nvarchar](255) NULL,
	[RebootSetting] [varchar](20) NOT NULL,
	[CopyDirectory] [bit] NOT NULL,
 CONSTRAINT [PK_Installer] PRIMARY KEY CLUSTERED 
(
	[InstallerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SoftwareInstallerPackage]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerPackage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SoftwareInstallerPackage](
	[PackageId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_InstallerPackage] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SoftwareInstallerSetting]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]') AND type in (N'U'))
BEGIN
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
END
GO

/******  Object:  Table [dbo].[SolutionTester]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SolutionTester]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SolutionTester](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[UseCredential] [bit] NOT NULL,
	[CredentialType] [varchar](20) NOT NULL,
	[CredentialName] [nvarchar](50) NULL,
	[CredentialDomain] [nvarchar](50) NULL,
	[CredentialPassword] [nvarchar](50) NULL,
 CONSTRAINT [PK_SolutionTester] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[SystemSetting]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SystemSetting](
	[Type] [varchar](20) NOT NULL,
	[SubType] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_SystemSetting] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[SubType] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[User]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserName] [nvarchar](50) NOT NULL,
	[Domain] [nvarchar](50) NOT NULL,
	[RoleName] [varchar](20) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[UserGroup]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGroup](
	[GroupName] [nvarchar](50) NOT NULL,
	[Creator] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1024) NULL,
 CONSTRAINT [PK_OperatorGroup] PRIMARY KEY CLUSTERED 
(
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[UserGroupAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGroupAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGroupAssoc](
	[GroupName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserGroupAssoc] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ValueGroup]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValueGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ValueGroup](
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DomainValues] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualMachineGroupAssoc]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachineGroupAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualMachineGroupAssoc](
	[GroupName] [nvarchar](50) NOT NULL,
	[MachineName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualMachineGroupAssoc_1] PRIMARY KEY CLUSTERED 
(
	[MachineName] ASC,
	[GroupName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResource]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResource]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResource](
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[ResourceType] [varchar](50) NOT NULL,
	[InstanceCount] [int] NOT NULL,
	[Platform] [nvarchar](50) NOT NULL,
	[ResourcesPerVM] [int] NULL,
	[TestCaseId] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_VirtualResource] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadata]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadata](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[VirtualResourceId] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ResourceType] [varchar](50) NOT NULL,
	[MetadataType] [nvarchar](50) NOT NULL,
	[MetadataVersion] [nvarchar](50) NULL,
	[Metadata] [xml] NOT NULL,
	[ExecutionPlan] [xml] NULL,
	[Enabled] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadata] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadataAssetUsage]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataAssetUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadataAssetUsage](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[AssetSelectionData] [xml] NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadataAssetUsage] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadataDocumentUsage]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataDocumentUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadataDocumentUsage](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[DocumentSelectionData] [xml] NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadataDocumentUsage] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadataPrintQueueUsage]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataPrintQueueUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadataPrintQueueUsage](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[PrintQueueSelectionData] [xml] NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadataPrintQueueUsage] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadataRetrySetting]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataRetrySetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadataRetrySetting](
	[SettingId] [uniqueidentifier] NOT NULL,
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[State] [varchar](20) NOT NULL,
	[Action] [varchar](20) NOT NULL,
	[RetryLimit] [int] NOT NULL,
	[RetryDelay] [int] NOT NULL,
	[LimitExceededAction] [varchar](20) NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadataRetrySetting_1] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualResourceMetadataServerUsage]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataServerUsage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualResourceMetadataServerUsage](
	[VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	[ServerSelectionData] [xml] NOT NULL,
 CONSTRAINT [PK_VirtualResourceMetadataServerUsage] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceMetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/*****************************************************************************
 * INDEXES                                                                   *
 *****************************************************************************/

/******  Object:  Index [IDX_MetadataType]  ******/
USE [EnterpriseTest]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadata]') AND name = N'IDX_MetadataType')
CREATE NONCLUSTERED INDEX [IDX_MetadataType] ON [dbo].[VirtualResourceMetadata]
(
	[MetadataType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/******  Object:  Foreign Key [FK_AdminWorker_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AdminWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[AdminWorker]'))
ALTER TABLE [dbo].[AdminWorker]  WITH CHECK ADD  CONSTRAINT [FK_AdminWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AdminWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[AdminWorker]'))
ALTER TABLE [dbo].[AdminWorker] CHECK CONSTRAINT [FK_AdminWorker_VirtualResource]
GO

/******  Object:  Foreign Key [FK_AssociatedProductVersion_AssociatedProduct]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductVersion_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductVersion]'))
ALTER TABLE [dbo].[AssociatedProductVersion]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductVersion_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductVersion_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductVersion]'))
ALTER TABLE [dbo].[AssociatedProductVersion] CHECK CONSTRAINT [FK_AssociatedProductVersion_AssociatedProduct]
GO

/******  Object:  Foreign Key [FK_AssociatedProductVersion_EnterpriseScenario]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductVersion_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductVersion]'))
ALTER TABLE [dbo].[AssociatedProductVersion]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductVersion_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssociatedProductVersion_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssociatedProductVersion]'))
ALTER TABLE [dbo].[AssociatedProductVersion] CHECK CONSTRAINT [FK_AssociatedProductVersion_EnterpriseScenario]
GO

/******  Object:  Foreign Key [FK_CitrixWorker_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CitrixWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[CitrixWorker]'))
ALTER TABLE [dbo].[CitrixWorker]  WITH CHECK ADD  CONSTRAINT [FK_CitrixWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CitrixWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[CitrixWorker]'))
ALTER TABLE [dbo].[CitrixWorker] CHECK CONSTRAINT [FK_CitrixWorker_VirtualResource]
GO

/******  Object:  Foreign Key [FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario] FOREIGN KEY([ScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_EnterpriseScenario]
GO

/******  Object:  Foreign Key [FK_EnterpriseScenarioGroupAssoc_OperatorGroup]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioGroupAssoc_OperatorGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_OperatorGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EnterpriseScenarioGroupAssoc_OperatorGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[EnterpriseScenarioGroupAssoc]'))
ALTER TABLE [dbo].[EnterpriseScenarioGroupAssoc] CHECK CONSTRAINT [FK_EnterpriseScenarioGroupAssoc_OperatorGroup]
GO

/******  Object:  Foreign Key [FK_EventLogCollector_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventLogCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventLogCollector]'))
ALTER TABLE [dbo].[EventLogCollector]  WITH CHECK ADD  CONSTRAINT [FK_EventLogCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventLogCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventLogCollector]'))
ALTER TABLE [dbo].[EventLogCollector] CHECK CONSTRAINT [FK_EventLogCollector_VirtualResource]
GO

/******  Object:  Foreign Key [FK_LoadTester_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoadTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoadTester]'))
ALTER TABLE [dbo].[LoadTester]  WITH CHECK ADD  CONSTRAINT [FK_LoadTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoadTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoadTester]'))
ALTER TABLE [dbo].[LoadTester] CHECK CONSTRAINT [FK_LoadTester_VirtualResource]
GO

/******  Object:  Foreign Key [FK_MetadataInstallerPackageAssoc_InstallerPackage]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_InstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataInstallerPackageAssoc_InstallerPackage] FOREIGN KEY([InstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_InstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] CHECK CONSTRAINT [FK_MetadataInstallerPackageAssoc_InstallerPackage]
GO

/******  Object:  Foreign Key [FK_MetadataInstallerPackageAssoc_MetadataType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType] FOREIGN KEY([MetadataType])
REFERENCES [dbo].[MetadataType] ([Name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] CHECK CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType]
GO

/******  Object:  Foreign Key [FK_MetadataTypeResourceTypeAssoc_MetadataType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] CHECK CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType]
GO

/******  Object:  Foreign Key [FK_MetadataTypeResourceTypeAssoc_ResourceType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] CHECK CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_ResourceType]
GO

/******  Object:  Foreign Key [FK_OfficeWorker_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OfficeWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OfficeWorker]'))
ALTER TABLE [dbo].[OfficeWorker]  WITH CHECK ADD  CONSTRAINT [FK_OfficeWorker_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OfficeWorker_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OfficeWorker]'))
ALTER TABLE [dbo].[OfficeWorker] CHECK CONSTRAINT [FK_OfficeWorker_VirtualResource]
GO

/******  Object:  Foreign Key [FK_PerfMonCollector_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PerfMonCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[PerfMonCollector]'))
ALTER TABLE [dbo].[PerfMonCollector]  WITH CHECK ADD  CONSTRAINT [FK_PerfMonCollector_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PerfMonCollector_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[PerfMonCollector]'))
ALTER TABLE [dbo].[PerfMonCollector] CHECK CONSTRAINT [FK_PerfMonCollector_VirtualResource]
GO

/******  Object:  Foreign Key [FK_ProductPluginAssociation_AssociatedProduct]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductPluginAssociation_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]'))
ALTER TABLE [dbo].[ProductPluginAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ProductPluginAssociation_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductPluginAssociation_AssociatedProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]'))
ALTER TABLE [dbo].[ProductPluginAssociation] CHECK CONSTRAINT [FK_ProductPluginAssociation_AssociatedProduct]
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ProductPluginAssociation', N'CONSTRAINT',N'FK_ProductPluginAssociation_AssociatedProduct'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductPluginAssociation', @level2type=N'CONSTRAINT',@level2name=N'FK_ProductPluginAssociation_AssociatedProduct'
GO

/******  Object:  Foreign Key [FK_ProductPluginAssociation_MetadataType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductPluginAssociation_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]'))
ALTER TABLE [dbo].[ProductPluginAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ProductPluginAssociation_MetadataType] FOREIGN KEY([MetadataType])
REFERENCES [dbo].[MetadataType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductPluginAssociation_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductPluginAssociation]'))
ALTER TABLE [dbo].[ProductPluginAssociation] CHECK CONSTRAINT [FK_ProductPluginAssociation_MetadataType]
GO

/******  Object:  Foreign Key [FK_ResourceInstallerPackageAssoc_InstallerPackage]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceInstallerPackageAssoc_InstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]'))
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceInstallerPackageAssoc_InstallerPackage] FOREIGN KEY([InstallerPackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceInstallerPackageAssoc_InstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]'))
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc] CHECK CONSTRAINT [FK_ResourceInstallerPackageAssoc_InstallerPackage]
GO

/******  Object:  Foreign Key [FK_ResourceInstallerPackageAssoc_ResourceType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceInstallerPackageAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]'))
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_ResourceInstallerPackageAssoc_ResourceType] FOREIGN KEY([ResourceType])
REFERENCES [dbo].[ResourceType] ([Name])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceInstallerPackageAssoc_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceInstallerPackageAssoc]'))
ALTER TABLE [dbo].[ResourceInstallerPackageAssoc] CHECK CONSTRAINT [FK_ResourceInstallerPackageAssoc_ResourceType]
GO

/******  Object:  Foreign Key [FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]'))
ALTER TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType] FOREIGN KEY([ResourceTypeName])
REFERENCES [dbo].[ResourceType] ([Name])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceTypeFrameworkClientPlatformAssociation]'))
ALTER TABLE [dbo].[ResourceTypeFrameworkClientPlatformAssociation] CHECK CONSTRAINT [FK_ResourceTypeFrameworkClientPlatformAssociation_ResourceType]
GO

/******  Object:  Foreign Key [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]'))
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]'))
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory]
GO

/******  Object:  Foreign Key [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]'))
ALTER TABLE [dbo].[ResourceWindowsCategoryParent]  WITH CHECK ADD  CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[ResourceWindowsCategory] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ResourceWindowsCategoryParent]'))
ALTER TABLE [dbo].[ResourceWindowsCategoryParent] CHECK CONSTRAINT [FK_ResourceWindowsCategoryParent_ResourceWindowsCategory1]
GO

/******  Object:  Foreign Key [FK_SoftwareInstallerSetting_SoftwareInstaller]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerSetting_SoftwareInstaller]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]'))
ALTER TABLE [dbo].[SoftwareInstallerSetting]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstaller] FOREIGN KEY([InstallerId])
REFERENCES [dbo].[SoftwareInstaller] ([InstallerId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerSetting_SoftwareInstaller]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]'))
ALTER TABLE [dbo].[SoftwareInstallerSetting] CHECK CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstaller]
GO

/******  Object:  Foreign Key [FK_SoftwareInstallerSetting_SoftwareInstallerPackage]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerSetting_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]'))
ALTER TABLE [dbo].[SoftwareInstallerSetting]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstallerPackage] FOREIGN KEY([PackageId])
REFERENCES [dbo].[SoftwareInstallerPackage] ([PackageId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareInstallerSetting_SoftwareInstallerPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareInstallerSetting]'))
ALTER TABLE [dbo].[SoftwareInstallerSetting] CHECK CONSTRAINT [FK_SoftwareInstallerSetting_SoftwareInstallerPackage]
GO

/******  Object:  Foreign Key [FK_SolutionTester_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SolutionTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[SolutionTester]'))
ALTER TABLE [dbo].[SolutionTester]  WITH CHECK ADD  CONSTRAINT [FK_SolutionTester_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SolutionTester_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[SolutionTester]'))
ALTER TABLE [dbo].[SolutionTester] CHECK CONSTRAINT [FK_SolutionTester_VirtualResource]
GO

/******  Object:  Foreign Key [FK_UserGroupAssoc_User]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupAssoc_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupAssoc]'))
ALTER TABLE [dbo].[UserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_UserGroupAssoc_User] FOREIGN KEY([UserName])
REFERENCES [dbo].[User] ([UserName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupAssoc_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupAssoc]'))
ALTER TABLE [dbo].[UserGroupAssoc] CHECK CONSTRAINT [FK_UserGroupAssoc_User]
GO

/******  Object:  Foreign Key [FK_UserGroupAssoc_UserGroup]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupAssoc_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupAssoc]'))
ALTER TABLE [dbo].[UserGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_UserGroupAssoc_UserGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserGroupAssoc_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserGroupAssoc]'))
ALTER TABLE [dbo].[UserGroupAssoc] CHECK CONSTRAINT [FK_UserGroupAssoc_UserGroup]
GO

/******  Object:  Foreign Key [FK_VirtualMachineGroupAssoc_OperatorGroup]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualMachineGroupAssoc_OperatorGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualMachineGroupAssoc]'))
ALTER TABLE [dbo].[VirtualMachineGroupAssoc]  WITH CHECK ADD  CONSTRAINT [FK_VirtualMachineGroupAssoc_OperatorGroup] FOREIGN KEY([GroupName])
REFERENCES [dbo].[UserGroup] ([GroupName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualMachineGroupAssoc_OperatorGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualMachineGroupAssoc]'))
ALTER TABLE [dbo].[VirtualMachineGroupAssoc] CHECK CONSTRAINT [FK_VirtualMachineGroupAssoc_OperatorGroup]
GO

/******  Object:  Foreign Key [FK_VirtualResource_EnterpriseScenario]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResource_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResource]'))
ALTER TABLE [dbo].[VirtualResource]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResource_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResource_EnterpriseScenario]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResource]'))
ALTER TABLE [dbo].[VirtualResource] CHECK CONSTRAINT [FK_VirtualResource_EnterpriseScenario]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadata_VirtualResource]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadata_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadata]'))
ALTER TABLE [dbo].[VirtualResourceMetadata]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource] FOREIGN KEY([VirtualResourceId])
REFERENCES [dbo].[VirtualResource] ([VirtualResourceId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadata_VirtualResource]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadata]'))
ALTER TABLE [dbo].[VirtualResourceMetadata] CHECK CONSTRAINT [FK_VirtualResourceMetadata_VirtualResource]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataAssetUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataAssetUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataAssetUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataAssetUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataAssetUsage_VirtualResourceMetadata]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataDocumentUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataDocumentUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataDocumentUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataDocumentUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataDocumentUsage_VirtualResourceMetadata]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataPrintQueueUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataPrintQueueUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataPrintQueueUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataPrintQueueUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataRetrySetting]'))
ALTER TABLE [dbo].[VirtualResourceMetadataRetrySetting]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataRetrySetting]'))
ALTER TABLE [dbo].[VirtualResourceMetadataRetrySetting] CHECK CONSTRAINT [FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata]
GO

/******  Object:  Foreign Key [FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataServerUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataServerUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataServerUsage]'))
ALTER TABLE [dbo].[VirtualResourceMetadataServerUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata]
GO

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/******  Object:  Function [dbo].[SessionIdsMatchingPatterns_Remove]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionIdsMatchingPatterns_Remove]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =======================================================================
-- Author:		Fernando De La Garza
-- Create date: 8/10/2016
-- Description:	Table Valued Function that returns all of the SessionId''s
--              that contain tags in passed value list
-- =======================================================================
CREATE FUNCTION [dbo].[SessionIdsMatchingPatterns_Remove]( @list Varchar(255))
RETURNS 
@SessionIds TABLE (
   SessionId Varchar(50) 
)
AS
BEGIN
   IF(@list IS NOT NULL AND len(@list)>0)
   BEGIN
      INSERT INTO @SessionIds
	  SELECT DISTINCT SessionId FROM SessionInfo ss WITH(NOLOCK)
	  INNER JOIN ValueListToContainedPatterns(@list) p ON '',''+ss.Tags+'','' LIKE p.Pattern
   END
   ELSE
   BEGIN
      INSERT INTO @SessionIds
	  SELECT DISTINCT SessionId FROM SessionInfo WITH(NOLOCK)
   END
   RETURN
END
' 
END
GO

/******  Object:  Function [dbo].[ValueListToContainedPatterns_Remove]  ******/
USE [EnterpriseTest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValueListToContainedPatterns_Remove]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =======================================================================
-- Author:		Fernando De La Garza
-- Create date: 8/10/2016
-- Description:	Table Valued Function that extracts comma separated values and returns
--              Table with LIKE statement Contained patterns of each value
-- =======================================================================
CREATE FUNCTION [dbo].[ValueListToContainedPatterns_Remove]( @list Varchar(255))
RETURNS 
@ContainedPatterns TABLE (
   Pattern Varchar(255) 
)
AS
BEGIN
   DECLARE @pos        int,
           @nextpos    int,
           @valuelen   int

   SELECT @pos = 0, @nextpos = 1

   WHILE @nextpos > 0
   BEGIN
      SELECT @nextpos = charindex('','', @list, @pos + 1)
      SELECT @valuelen = CASE WHEN @nextpos > 0
                              THEN @nextpos
                              ELSE len(@list) + 1
                         END - @pos - 1
      INSERT @ContainedPatterns (Pattern)
         VALUES (''%,''+substring(@list, @pos + 1, @valuelen)+'',%'')
      SELECT @pos = @nextpos
   END
   RETURN
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

/******  Object:  User [enterprise_data]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'enterprise_data')
CREATE USER [enterprise_data] FOR LOGIN [enterprise_data] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [enterprise_data]
GO

/******  Object:  User [enterprise_report]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'enterprise_report')
CREATE USER [enterprise_report] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [enterprise_report]
GO

/******  Object:  User [stf_user]  ******/
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'stf_user')
CREATE USER [stf_user] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [stf_user]
GO
