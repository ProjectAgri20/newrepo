
/*****************************************************************************
 * SCHEMAS                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * TABLES                                                                    *
 *****************************************************************************/

/******  Object:  Table [dbo].[Asset]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Asset]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Asset](
	[AssetId] [varchar](50) NOT NULL,
	[AssetType] [varchar](50) NOT NULL,
	[PoolName] [nvarchar](50) NULL,
	[Capability] [int] NOT NULL,
 CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AssetHistory]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssetHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssetHistory](
	[AssetHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[Action] [varchar](10) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[ModifiedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AssetHistory] PRIMARY KEY CLUSTERED 
(
	[AssetHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AssetPool]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssetPool]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssetPool](
	[Name] [nvarchar](50) NOT NULL,
	[Administrator] [nvarchar](255) NOT NULL,
	[TrackReservations] [bit] NULL,
 CONSTRAINT [PK_AssetPool] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[AssetReservation]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssetReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AssetReservation](
	[AssetReservationId] [uniqueidentifier] NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[ReservedBy] [nvarchar](255) NOT NULL,
	[ReservedFor] [nvarchar](255) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[Received] [datetime] NOT NULL,
	[SessionId] [varchar](50) NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[Notify] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_AssetReservation] PRIMARY KEY CLUSTERED 
(
	[AssetReservationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[Badge]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Badge]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Badge](
	[BadgeId] [varchar](50) NOT NULL,
	[BadgeBoxId] [varchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Index] [tinyint] NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_Badge] PRIMARY KEY CLUSTERED 
(
	[BadgeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[BadgeBox]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BadgeBox]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BadgeBox](
	[BadgeBoxId] [varchar](50) NOT NULL,
	[IPAddress] [varchar](50) NULL,
	[PrinterId] [varchar](50) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_BadgeBox] PRIMARY KEY CLUSTERED 
(
	[BadgeBoxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[BashLogCollector]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BashLogCollector]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BashLogCollector](
	[BashLogCollectorId] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](50) NULL,
	[Port] [int] NULL,
	[PrinterId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_BashLogCollector] PRIMARY KEY CLUSTERED 
(
	[BashLogCollectorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[Camera]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Camera]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Camera](
	[AssetId] [varchar](50) NOT NULL,
	[IPAddress] [varchar](50) NULL,
	[PrinterId] [varchar](50) NULL,
	[CameraServer] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_Camera] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[CitrixPublishedApp]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CitrixPublishedApp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CitrixPublishedApp](
	[CitrixServer] [nvarchar](50) NOT NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CitrixPublishedApp] PRIMARY KEY CLUSTERED 
(
	[CitrixServer] ASC,
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DartBoard]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DartBoard]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DartBoard](
	[DartBoardId] [varchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[FirmwareVersion] [nvarchar](50) NOT NULL,
	[PrinterId] [varchar](50) NULL,
 CONSTRAINT [PK_DartBoard] PRIMARY KEY CLUSTERED 
(
	[DartBoardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DeviceSimulator]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceSimulator]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DeviceSimulator](
	[AssetId] [varchar](50) NOT NULL,
	[Product] [nvarchar](255) NOT NULL,
	[SimulatorType] [varchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[FirmwareVersion] [varchar](50) NULL,
	[VirtualMachine] [nvarchar](50) NULL,
 CONSTRAINT [PK_DeviceSimulator] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DomainAccountPool]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DomainAccountPool]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DomainAccountPool](
	[DomainAccountKey] [varchar](50) NOT NULL,
	[UserNameFormat] [nvarchar](50) NOT NULL,
	[MinimumUserNumber] [int] NOT NULL,
	[MaximumUserNumber] [int] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DomainAccountPool] PRIMARY KEY CLUSTERED 
(
	[DomainAccountKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[DomainAccountReservation]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DomainAccountReservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DomainAccountReservation](
	[DomainAccountKey] [varchar](50) NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[StartIndex] [int] NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_DomainAccountReservation] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[DomainAccountKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkClient]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkClient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkClient](
	[FrameworkClientHostName] [nvarchar](50) NOT NULL,
	[PowerState] [varchar](50) NOT NULL,
	[UsageState] [varchar](50) NOT NULL,
	[SessionId] [varchar](50) NULL,
	[Environment] [nvarchar](50) NULL,
	[PlatformUsage] [varchar](50) NULL,
	[HoldId] [nvarchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_FrameworkClient] PRIMARY KEY CLUSTERED 
(
	[FrameworkClientHostName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkClientPlatform]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatform]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkClientPlatform](
	[FrameworkClientPlatformId] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_FrameworkClientPlatform] PRIMARY KEY CLUSTERED 
(
	[FrameworkClientPlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkClientPlatformAssoc]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkClientPlatformAssoc](
	[FrameworkClientHostName] [nvarchar](50) NOT NULL,
	[FrameworkClientPlatformId] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FrameworkClientPlatformAssoc] PRIMARY KEY CLUSTERED 
(
	[FrameworkClientPlatformId] ASC,
	[FrameworkClientHostName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkServer]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkServer](
	[FrameworkServerId] [uniqueidentifier] NOT NULL,
	[HostName] [nvarchar](50) NOT NULL,
	[IPAddress] [varchar](50) NULL,
	[OperatingSystem] [nvarchar](255) NOT NULL,
	[Architecture] [varchar](10) NOT NULL,
	[Processors] [int] NOT NULL,
	[Cores] [int] NOT NULL,
	[Memory] [int] NOT NULL,
	[DiskSpace] [varchar](255) NULL,
	[Status] [varchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Contact] [nvarchar](255) NULL,
	[Environment] [nvarchar](50) NULL,
	[ServiceVersion] [varchar](50) NULL,
	[StfServiceVersion] [varchar](50) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_FrameworkServer] PRIMARY KEY CLUSTERED 
(
	[FrameworkServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkServerType]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkServerType](
	[FrameworkServerTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_FrameworkServerType] PRIMARY KEY CLUSTERED 
(
	[FrameworkServerTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[FrameworkServerTypeAssoc]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FrameworkServerTypeAssoc](
	[FrameworkServerId] [uniqueidentifier] NOT NULL,
	[FrameworkServerTypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FrameworkServerTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[FrameworkServerId] ASC,
	[FrameworkServerTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[License]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[License]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[License](
	[LicenseId] [uniqueidentifier] NOT NULL,
	[FrameworkServerId] [uniqueidentifier] NOT NULL,
	[Solution] [nvarchar](50) NOT NULL,
	[SolutionVersion] [nvarchar](50) NOT NULL,
	[InstallationKey] [nvarchar](255) NULL,
	[Seats] [int] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[ExpirationNoticeDays] [int] NOT NULL,
	[RequestSentDate] [datetime] NULL,
 CONSTRAINT [PK_License] PRIMARY KEY CLUSTERED 
(
	[LicenseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[LicenseKey]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenseKey]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LicenseKey](
	[LicenseKeyId] [uniqueidentifier] NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[KeyName] [nvarchar](50) NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_LicenseKey] PRIMARY KEY CLUSTERED 
(
	[LicenseKeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[LicenseOwner]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenseOwner]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LicenseOwner](
	[LicenseId] [uniqueidentifier] NOT NULL,
	[Contact] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_LicenseOwner] PRIMARY KEY CLUSTERED 
(
	[LicenseId] ASC,
	[Contact] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[MonitorConfig]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MonitorConfig]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MonitorConfig](
	[MonitorConfigId] [uniqueidentifier] NOT NULL,
	[ServerHostName] [nvarchar](50) NOT NULL,
	[MonitorType] [varchar](50) NOT NULL,
	[Configuration] [xml] NOT NULL,
 CONSTRAINT [PK_MonitorConfig] PRIMARY KEY CLUSTERED 
(
	[MonitorConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[Peripheral]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Peripheral]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Peripheral](
	[AssetId] [varchar](50) NOT NULL,
	[PeripheralType] [nvarchar](255) NOT NULL,
	[SerialNumber] [nvarchar](255) NULL,
	[Interface] [nvarchar](255) NULL,
	[Capacity] [nvarchar](255) NULL,
	[Brand] [nvarchar](255) NULL,
 CONSTRAINT [PK_Peripheral] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PeripheralType]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PeripheralType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PeripheralType](
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PeripheralType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriver]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriver]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriver](
	[PrintDriverId] [uniqueidentifier] NOT NULL,
	[PrintDriverPackageId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[PrintProcessor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PrintDriver] PRIMARY KEY CLUSTERED 
(
	[PrintDriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverConfig]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverConfig]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverConfig](
	[PrintDriverConfigId] [uniqueidentifier] NOT NULL,
	[ConfigFile] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PrintDriverConfig] PRIMARY KEY CLUSTERED 
(
	[PrintDriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverPackage]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverPackage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverPackage](
	[PrintDriverPackageId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[InfX86] [nvarchar](255) NOT NULL,
	[InfX64] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PrintDriverPackage] PRIMARY KEY CLUSTERED 
(
	[PrintDriverPackageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverProduct]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverProduct](
	[PrintDriverProductId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PrintDriverProduct] PRIMARY KEY CLUSTERED 
(
	[PrintDriverProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverProductAssoc]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverProductAssoc](
	[PrintDriverConfigId] [uniqueidentifier] NOT NULL,
	[PrintDriverProductId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PrintDriverProductAssoc] PRIMARY KEY CLUSTERED 
(
	[PrintDriverProductId] ASC,
	[PrintDriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverVersion]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverVersion](
	[PrintDriverVersionId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PrintDriverVersion] PRIMARY KEY CLUSTERED 
(
	[PrintDriverVersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrintDriverVersionAssoc]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrintDriverVersionAssoc](
	[PrintDriverConfigId] [uniqueidentifier] NOT NULL,
	[PrintDriverVersionId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PrintDriverVersionAssoc] PRIMARY KEY CLUSTERED 
(
	[PrintDriverVersionId] ASC,
	[PrintDriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[Printer]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Printer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Printer](
	[AssetId] [varchar](50) NOT NULL,
	[Product] [nvarchar](255) NOT NULL,
	[Model] [nvarchar](255) NOT NULL,
	[Address1] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[Password] [varchar](50) NULL,
	[PortNumber] [int] NOT NULL,
	[SnmpEnabled] [bit] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[Owner] [nvarchar](255) NULL,
	[PrinterType] [nvarchar](50) NULL,
	[FirmwareType] [nvarchar](50) NULL,
	[EngineType] [nvarchar](50) NULL,
	[ModelNumber] [nvarchar](50) NULL,
	[SerialNumber] [nvarchar](50) NULL,
	[Online] [bit] NOT NULL,
 CONSTRAINT [PK_Printer] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[PrinterProduct]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrinterProduct]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PrinterProduct](
	[Family] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PrinterProduct] PRIMARY KEY CLUSTERED 
(
	[Family] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[RemotePrintQueue]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RemotePrintQueue](
	[RemotePrintQueueId] [uniqueidentifier] NOT NULL,
	[PrintServerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[PrinterId] [varchar](50) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_RemotePrintQueue] PRIMARY KEY CLUSTERED 
(
	[RemotePrintQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ReservationHistory]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReservationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReservationHistory](
	[ReservationHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[ReservedFor] [nvarchar](255) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
 CONSTRAINT [PK_ReservationHistory] PRIMARY KEY CLUSTERED 
(
	[ReservationHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[ServerSetting]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerSetting](
	[FrameworkServerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_ServerSetting] PRIMARY KEY CLUSTERED 
(
	[FrameworkServerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[User]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserId] [nvarchar](255) NOT NULL,
	[Role] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/******  Object:  Table [dbo].[VirtualPrinter]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualPrinter]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VirtualPrinter](
	[AssetId] [varchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[PortNumber] [int] NOT NULL,
	[SnmpEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_VirtualPrinter] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/*****************************************************************************
 * INDEXES                                                                   *
 *****************************************************************************/

/*****************************************************************************
 * FOREIGN KEYS                                                              *
 *****************************************************************************/

/******  Object:  Foreign Key [FK_Asset_AssetPool]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Asset_AssetPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[Asset]'))
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_AssetPool] FOREIGN KEY([PoolName])
REFERENCES [dbo].[AssetPool] ([Name])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Asset_AssetPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[Asset]'))
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_AssetPool]
GO

/******  Object:  Foreign Key [FK_AssetPool_User]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetPool_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetPool]'))
ALTER TABLE [dbo].[AssetPool]  WITH CHECK ADD  CONSTRAINT [FK_AssetPool_User] FOREIGN KEY([Administrator])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetPool_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetPool]'))
ALTER TABLE [dbo].[AssetPool] CHECK CONSTRAINT [FK_AssetPool_User]
GO

/******  Object:  Foreign Key [FK_AssetReservation_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetReservation_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation]  WITH CHECK ADD  CONSTRAINT [FK_AssetReservation_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetReservation_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] CHECK CONSTRAINT [FK_AssetReservation_Asset]
GO

/******  Object:  Foreign Key [FK_Badge_BadgeBox]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Badge_BadgeBox]') AND parent_object_id = OBJECT_ID(N'[dbo].[Badge]'))
ALTER TABLE [dbo].[Badge]  WITH CHECK ADD  CONSTRAINT [FK_Badge_BadgeBox] FOREIGN KEY([BadgeBoxId])
REFERENCES [dbo].[BadgeBox] ([BadgeBoxId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Badge_BadgeBox]') AND parent_object_id = OBJECT_ID(N'[dbo].[Badge]'))
ALTER TABLE [dbo].[Badge] CHECK CONSTRAINT [FK_Badge_BadgeBox]
GO

/******  Object:  Foreign Key [FK_Camera_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Camera_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Camera]'))
ALTER TABLE [dbo].[Camera]  WITH CHECK ADD  CONSTRAINT [FK_Camera_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Camera_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Camera]'))
ALTER TABLE [dbo].[Camera] CHECK CONSTRAINT [FK_Camera_Asset]
GO

/******  Object:  Foreign Key [FK_DeviceSimulator_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceSimulator_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceSimulator]'))
ALTER TABLE [dbo].[DeviceSimulator]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSimulator_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceSimulator_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceSimulator]'))
ALTER TABLE [dbo].[DeviceSimulator] CHECK CONSTRAINT [FK_DeviceSimulator_Asset]
GO

/******  Object:  Foreign Key [FK_DomainAccountReservation_DomainAccountPool]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DomainAccountReservation_DomainAccountPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[DomainAccountReservation]'))
ALTER TABLE [dbo].[DomainAccountReservation]  WITH CHECK ADD  CONSTRAINT [FK_DomainAccountReservation_DomainAccountPool] FOREIGN KEY([DomainAccountKey])
REFERENCES [dbo].[DomainAccountPool] ([DomainAccountKey])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DomainAccountReservation_DomainAccountPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[DomainAccountReservation]'))
ALTER TABLE [dbo].[DomainAccountReservation] CHECK CONSTRAINT [FK_DomainAccountReservation_DomainAccountPool]
GO

/******  Object:  Foreign Key [FK_FrameworkClientPlatformAssoc_FrameworkClient]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClient] FOREIGN KEY([FrameworkClientHostName])
REFERENCES [dbo].[FrameworkClient] ([FrameworkClientHostName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc] CHECK CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClient]
GO

/******  Object:  Foreign Key [FK_FrameworkClientPlatformAssoc_FrameworkClientPlatform]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClientPlatform]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClientPlatform] FOREIGN KEY([FrameworkClientPlatformId])
REFERENCES [dbo].[FrameworkClientPlatform] ([FrameworkClientPlatformId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClientPlatform]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc] CHECK CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClientPlatform]
GO

/******  Object:  Foreign Key [FK_FrameworkServerTypeAssoc_FrameworkServer]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServerTypeAssoc_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]'))
ALTER TABLE [dbo].[FrameworkServerTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServer] FOREIGN KEY([FrameworkServerId])
REFERENCES [dbo].[FrameworkServer] ([FrameworkServerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServerTypeAssoc_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]'))
ALTER TABLE [dbo].[FrameworkServerTypeAssoc] CHECK CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServer]
GO

/******  Object:  Foreign Key [FK_FrameworkServerTypeAssoc_FrameworkServerType]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServerTypeAssoc_FrameworkServerType]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]'))
ALTER TABLE [dbo].[FrameworkServerTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServerType] FOREIGN KEY([FrameworkServerTypeId])
REFERENCES [dbo].[FrameworkServerType] ([FrameworkServerTypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServerTypeAssoc_FrameworkServerType]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]'))
ALTER TABLE [dbo].[FrameworkServerTypeAssoc] CHECK CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServerType]
GO

/******  Object:  Foreign Key [FK_LicenseKey_License]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseKey_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseKey]'))
ALTER TABLE [dbo].[LicenseKey]  WITH CHECK ADD  CONSTRAINT [FK_LicenseKey_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseKey_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseKey]'))
ALTER TABLE [dbo].[LicenseKey] CHECK CONSTRAINT [FK_LicenseKey_License]
GO

/******  Object:  Foreign Key [FK_LicenseOwner_License]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseOwner_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseOwner]'))
ALTER TABLE [dbo].[LicenseOwner]  WITH CHECK ADD  CONSTRAINT [FK_LicenseOwner_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseOwner_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseOwner]'))
ALTER TABLE [dbo].[LicenseOwner] CHECK CONSTRAINT [FK_LicenseOwner_License]
GO

/******  Object:  Foreign Key [FK_Peripheral_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_Asset]
GO

/******  Object:  Foreign Key [FK_Peripheral_PeripheralType]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_PeripheralType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_PeripheralType] FOREIGN KEY([PeripheralType])
REFERENCES [dbo].[PeripheralType] ([Name])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_PeripheralType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_PeripheralType]
GO

/******  Object:  Foreign Key [FK_PrintDriver_PrintDriverPackage]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriver_PrintDriverPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriver]'))
ALTER TABLE [dbo].[PrintDriver]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriver_PrintDriverPackage] FOREIGN KEY([PrintDriverPackageId])
REFERENCES [dbo].[PrintDriverPackage] ([PrintDriverPackageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriver_PrintDriverPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriver]'))
ALTER TABLE [dbo].[PrintDriver] CHECK CONSTRAINT [FK_PrintDriver_PrintDriverPackage]
GO

/******  Object:  Foreign Key [FK_PrintDriverProductAssoc_PrintDriverConfig]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverProductAssoc_PrintDriverConfig]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]'))
ALTER TABLE [dbo].[PrintDriverProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverConfig] FOREIGN KEY([PrintDriverConfigId])
REFERENCES [dbo].[PrintDriverConfig] ([PrintDriverConfigId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverProductAssoc_PrintDriverConfig]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]'))
ALTER TABLE [dbo].[PrintDriverProductAssoc] CHECK CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverConfig]
GO

/******  Object:  Foreign Key [FK_PrintDriverProductAssoc_PrintDriverProduct]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverProductAssoc_PrintDriverProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]'))
ALTER TABLE [dbo].[PrintDriverProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverProduct] FOREIGN KEY([PrintDriverProductId])
REFERENCES [dbo].[PrintDriverProduct] ([PrintDriverProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverProductAssoc_PrintDriverProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]'))
ALTER TABLE [dbo].[PrintDriverProductAssoc] CHECK CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverProduct]
GO

/******  Object:  Foreign Key [FK_PrintDriverVersionAssoc_PrintDriverConfig]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverVersionAssoc_PrintDriverConfig]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]'))
ALTER TABLE [dbo].[PrintDriverVersionAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverConfig] FOREIGN KEY([PrintDriverConfigId])
REFERENCES [dbo].[PrintDriverConfig] ([PrintDriverConfigId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverVersionAssoc_PrintDriverConfig]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]'))
ALTER TABLE [dbo].[PrintDriverVersionAssoc] CHECK CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverConfig]
GO

/******  Object:  Foreign Key [FK_PrintDriverVersionAssoc_PrintDriverVersion]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverVersionAssoc_PrintDriverVersion]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]'))
ALTER TABLE [dbo].[PrintDriverVersionAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverVersion] FOREIGN KEY([PrintDriverVersionId])
REFERENCES [dbo].[PrintDriverVersion] ([PrintDriverVersionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriverVersionAssoc_PrintDriverVersion]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]'))
ALTER TABLE [dbo].[PrintDriverVersionAssoc] CHECK CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverVersion]
GO

/******  Object:  Foreign Key [FK_Printer_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Printer_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Printer]'))
ALTER TABLE [dbo].[Printer]  WITH CHECK ADD  CONSTRAINT [FK_Printer_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Printer_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Printer]'))
ALTER TABLE [dbo].[Printer] CHECK CONSTRAINT [FK_Printer_Asset]
GO

/******  Object:  Foreign Key [FK_RemotePrintQueue_FrameworkServer]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RemotePrintQueue_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]'))
ALTER TABLE [dbo].[RemotePrintQueue]  WITH CHECK ADD  CONSTRAINT [FK_RemotePrintQueue_FrameworkServer] FOREIGN KEY([PrintServerId])
REFERENCES [dbo].[FrameworkServer] ([FrameworkServerId])
ON UPDATE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RemotePrintQueue_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]'))
ALTER TABLE [dbo].[RemotePrintQueue] CHECK CONSTRAINT [FK_RemotePrintQueue_FrameworkServer]
GO

/******  Object:  Foreign Key [FK_ServerSetting_FrameworkServer]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerSetting_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerSetting]'))
ALTER TABLE [dbo].[ServerSetting]  WITH CHECK ADD  CONSTRAINT [FK_ServerSetting_FrameworkServer] FOREIGN KEY([FrameworkServerId])
REFERENCES [dbo].[FrameworkServer] ([FrameworkServerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerSetting_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerSetting]'))
ALTER TABLE [dbo].[ServerSetting] CHECK CONSTRAINT [FK_ServerSetting_FrameworkServer]
GO

/******  Object:  Foreign Key [FK_VirtualPrinter_Asset]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualPrinter_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualPrinter]'))
ALTER TABLE [dbo].[VirtualPrinter]  WITH CHECK ADD  CONSTRAINT [FK_VirtualPrinter_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualPrinter_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualPrinter]'))
ALTER TABLE [dbo].[VirtualPrinter] CHECK CONSTRAINT [FK_VirtualPrinter_Asset]
GO

/*****************************************************************************
 * STORED PROCEDURES                                                         *
 *****************************************************************************/

/*****************************************************************************
 * USER-DEFINED FUNCTIONS                                                    *
 *****************************************************************************/

/******  Object:  Function [dbo].[udf_OverlappingReservationExists]  ******/
USE [AssetInventory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_OverlappingReservationExists]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- Checks for the existence of asset reservations that overlap with the proposed reservation.
-- Intended for use with the CK_DuplicateReservation constraint on the AssetReservation table.
CREATE FUNCTION [dbo].[udf_OverlappingReservationExists]
(
    @AssetReservationId AS UNIQUEIDENTIFIER,
    @AssetId AS VARCHAR(50),
    @Start AS DATETIME,
    @End AS DATETIME
)
RETURNS BIT
AS
BEGIN
    DECLARE @retval BIT
    IF EXISTS
    (
        SELECT * FROM AssetReservation
        WHERE @AssetReservationId <> AssetReservationId
          AND @AssetId = AssetId
          AND @Start < [End]
          AND @End > [Start]
    )
        SET @retval=1
    ELSE
        SET @retval=0
    RETURN @retval
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

/******  Object:  User [asset_admin]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'asset_admin')
CREATE USER [asset_admin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [asset_admin]
GO

/******  Object:  User [asset_user]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'asset_user')
CREATE USER [asset_user] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO

/******  Object:  User [enterprise_group]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'enterprise_group')
CREATE USER [enterprise_group] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [enterprise_group]
GO

/******  Object:  User [report_viewer]  ******/
USE [AssetInventory]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'report_viewer')
CREATE USER [report_viewer] FOR LOGIN [report_viewer] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [report_viewer]
GO
