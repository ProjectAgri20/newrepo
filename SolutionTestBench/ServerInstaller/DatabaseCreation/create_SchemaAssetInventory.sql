USE [AssetInventory]
GO
/****** Object:  Table [dbo].[Asset]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Asset](
	[AssetId] [varchar](50) NOT NULL,
	[AssetType] [varchar](50) NOT NULL,
	[PoolName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetHistory]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetHistory](
	[HistoryId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[Action] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetPool]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetPool](
	[Name] [varchar](100) NOT NULL,
	[Administrator] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
	[TrackReservations] [bit] NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssetReservation]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetReservation](
	[ReservationId] [uniqueidentifier] NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[ReservedBy] [varchar](50) NOT NULL,
	[ReservedFor] [varchar](50) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[Received] [datetime] NOT NULL,
	[Notes] [varchar](max) NULL,
	[SessionId] [varchar](50) NULL,
	[Notify] [int] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AssetReservation] PRIMARY KEY CLUSTERED 
(
	[ReservationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CitrixPublishedApp]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CitrixPublishedApp](
	[CitrixServer] [varchar](50) NOT NULL,
	[ApplicationName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CitrixPublishedApp] PRIMARY KEY CLUSTERED 
(
	[CitrixServer] ASC,
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeviceSimulator]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeviceSimulator](
	[AssetId] [varchar](50) NOT NULL,
	[Product] [varchar](100) NOT NULL,
	[Address] [varchar](100) NOT NULL,
	[VirtualMachine] [varchar](50) NOT NULL,
	[FirmwareVersion] [varchar](50) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_DeviceSimulator] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DomainAccountPool]    Script Date: 9/24/2015 4:02:27 PM ******/
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
	[Description] [nvarchar](50) NOT NULL CONSTRAINT [DF_DomainAccountPool_Description]  DEFAULT ('Description required'),
 CONSTRAINT [PK_DomainAccountPool] PRIMARY KEY CLUSTERED 
(
	[DomainAccountKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DomainAccountReservation]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DomainAccountReservation](
	[SessionId] [varchar](50) NOT NULL,
	[StartIndex] [int] NOT NULL CONSTRAINT [DF_DomainAccountReservation_StartIndex]  DEFAULT ((0)),
	[Count] [int] NOT NULL CONSTRAINT [DF_DomainAccountReservation_Count]  DEFAULT ((0)),
	[DomainAccountKey] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DomainAccountReservation] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC,
	[DomainAccountKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EndpointResponder]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EndpointResponder](
	[AssetId] [varchar](50) NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[Product] [varchar](50) NOT NULL,
	[FirmwareType] [varchar](50) NOT NULL,
	[AddressPrefix] [varchar](50) NOT NULL,
	[AddressSuffix] [int] NOT NULL,
 CONSTRAINT [PK_EndpointResponder] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FrameworkServer]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FrameworkServer](
	[ServerId] [uniqueidentifier] NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[OperatingSystemId] [uniqueidentifier] NOT NULL,
	[Active] [bit] NOT NULL,
	[ServicePack] [int] NOT NULL,
	[Revision] [int] NOT NULL,
	[Architecture] [varchar](10) NOT NULL,
	[Processors] [int] NOT NULL,
	[Cores] [int] NOT NULL,
	[Memory] [int] NOT NULL CONSTRAINT [DF_ServerInventory_Memory]  DEFAULT ((2048)),
	[DiskSpace] [varchar](255) NOT NULL CONSTRAINT [DF_ServerInventory_DiskSpace]  DEFAULT ((20)),
	[Contact] [varchar](255) NULL,
	[Status] [varchar](50) NOT NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[FrameworkServer] ADD [IpAddress] [varchar](50) NULL
SET ANSI_PADDING ON
ALTER TABLE [dbo].[FrameworkServer] ADD [ServiceVersion] [varchar](50) NULL
ALTER TABLE [dbo].[FrameworkServer] ADD [Environment] [varchar](50) NULL
ALTER TABLE [dbo].[FrameworkServer] ADD [DatabaseHostName] [varchar](50) NULL
 CONSTRAINT [PK_ServerInventory] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FrameworkServerType]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FrameworkServerType](
	[TypeId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_ServerInventoryType] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FrameworkServerTypeAssoc]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FrameworkServerTypeAssoc](
	[ServerId] [uniqueidentifier] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ServerInventoryTypeAssoc] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC,
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MachineOperatingSystem]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MachineOperatingSystem](
	[OperatingSystemId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Platform] [varchar](50) NOT NULL,
 CONSTRAINT [PK_OperatingSystemInfo] PRIMARY KEY CLUSTERED 
(
	[OperatingSystemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Peripheral]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Peripheral](
	[AssetId] [varchar](50) NOT NULL,
	[PeripheralType] [varchar](255) NOT NULL,
	[SerialNumber] [varchar](500) NULL,
	[Interface] [varchar](500) NULL,
	[Capacity] [varchar](500) NULL,
	[Brand] [varchar](500) NULL,
 CONSTRAINT [PK_Peripheral] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PeripheralType]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PeripheralType](
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_PeripheralType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriver]    Script Date: 9/24/2015 4:02:27 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverConfig]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ARITHABORT ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintDriverConfig](
	[DriverConfigId] [uniqueidentifier] NOT NULL,
	[ConfigFile] [varchar](1024) NOT NULL,
	[FileUniqueness]  AS (substring([ConfigFile],(1),(900))),
 CONSTRAINT [PK_PrintDriverConfig] PRIMARY KEY CLUSTERED 
(
	[DriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_FileUniqueness] UNIQUE NONCLUSTERED 
(
	[FileUniqueness] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverPackage]    Script Date: 9/24/2015 4:02:27 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverProduct]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintDriverProduct](
	[DriverProductId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UniversalPrintDriverProduct] PRIMARY KEY CLUSTERED 
(
	[DriverProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverProductAssoc]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintDriverProductAssoc](
	[DriverProductId] [uniqueidentifier] NOT NULL,
	[DriverConfigId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PrintDriverProductAssoc] PRIMARY KEY CLUSTERED 
(
	[DriverProductId] ASC,
	[DriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PrintDriverVersion]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrintDriverVersion](
	[DriverVersionId] [uniqueidentifier] NOT NULL,
	[Value] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UniversalPrintDriverVersion] PRIMARY KEY CLUSTERED 
(
	[DriverVersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrintDriverVersionAssoc]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintDriverVersionAssoc](
	[DriverVersionId] [uniqueidentifier] NOT NULL,
	[DriverConfigId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PrintDriverVersionAssoc] PRIMARY KEY CLUSTERED 
(
	[DriverVersionId] ASC,
	[DriverConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Printer]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Printer](
	[AssetId] [varchar](50) NOT NULL,
	[Product] [varchar](100) NOT NULL,
	[Model] [varchar](100) NOT NULL,
	[Address1] [varchar](25) NULL,
	[Address2] [varchar](25) NULL,
	[Description] [varchar](255) NULL,
	[Location] [varchar](255) NULL,
	[PortNumber] [int] NOT NULL CONSTRAINT [DF_Printer_PortNumber]  DEFAULT ((9100)),
	[SnmpEnabled] [bit] NOT NULL CONSTRAINT [DF_Printer_SnmpEnabled]  DEFAULT ((1)),
	[Owner] [varchar](255) NULL,
	[PrinterType] [varchar](25) NULL,
	[SerialNumber] [varchar](50) NULL,
	[ModelNumber] [varchar](50) NULL,
	[EngineType] [varchar](50) NULL,
	[FirmwareType] [varchar](25) NULL,
	[Online] [bit] NOT NULL CONSTRAINT [DF_Printer_Online]  DEFAULT ((0)),
 CONSTRAINT [PK_Printer] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PrinterProduct]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PrinterProduct](
	[Family] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PrinterProducts] PRIMARY KEY CLUSTERED 
(
	[Family] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RemotePrintQueue]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RemotePrintQueue](
	[Name] [nvarchar](200) NOT NULL,
	[PrintServerId] [uniqueidentifier] NOT NULL,
	[PrintQueueId] [uniqueidentifier] NOT NULL,
	[DevicePlatform] [varchar](50) NOT NULL CONSTRAINT [DF_RemotePrintQueue_DeviceType]  DEFAULT ('Physical'),
	[InventoryId] [varchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_RemotePrintQueue_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_RemotePrintQueue] PRIMARY KEY CLUSTERED 
(
	[Name] ASC,
	[PrintServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReservationHistory]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReservationHistory](
	[HistoryId] [int] IDENTITY(1,1) NOT NULL,
	[AssetId] [varchar](50) NOT NULL,
	[ReservedFor] [varchar](50) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
 CONSTRAINT [PK_ReservationHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [varchar](50) NOT NULL,
	[Role] [varchar](100) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualMachineReservation]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualMachineReservation](
	[VMName] [varchar](50) NOT NULL,
	[PowerState] [varchar](50) NOT NULL,
	[UsageState] [varchar](50) NOT NULL,
	[PlatformUsage] [varchar](50) NOT NULL,
	[HoldId] [varchar](50) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[SessionId] [varchar](50) NULL,
	[OsType] [varchar](50) NOT NULL,
	[Environment] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VirtualMachine] PRIMARY KEY CLUSTERED 
(
	[VMName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VirtualPrinter]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VirtualPrinter](
	[AssetId] [varchar](50) NOT NULL,
	[HostName] [varchar](50) NOT NULL,
	[PortNumber] [int] NOT NULL,
	[SnmpEnabled] [bit] NOT NULL,
	[Address] [varchar](25) NULL,
 CONSTRAINT [PK_VirtualPrinter] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_Group] FOREIGN KEY([PoolName])
REFERENCES [dbo].[AssetPool] ([Name])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Group]
GO
ALTER TABLE [dbo].[AssetPool]  WITH CHECK ADD  CONSTRAINT [FK_Group_Group] FOREIGN KEY([Administrator])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[AssetPool] CHECK CONSTRAINT [FK_Group_Group]
GO
ALTER TABLE [dbo].[AssetReservation]  WITH CHECK ADD  CONSTRAINT [FK_AssetReservation_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
ALTER TABLE [dbo].[AssetReservation] CHECK CONSTRAINT [FK_AssetReservation_Asset]
GO
ALTER TABLE [dbo].[DeviceSimulator]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSimulator_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
ALTER TABLE [dbo].[DeviceSimulator] CHECK CONSTRAINT [FK_DeviceSimulator_Asset]
GO
ALTER TABLE [dbo].[EndpointResponder]  WITH CHECK ADD  CONSTRAINT [FK_EndpointResponder_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
ALTER TABLE [dbo].[EndpointResponder] CHECK CONSTRAINT [FK_EndpointResponder_Asset]
GO
ALTER TABLE [dbo].[FrameworkServer]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem] FOREIGN KEY([OperatingSystemId])
REFERENCES [dbo].[MachineOperatingSystem] ([OperatingSystemId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FrameworkServer] CHECK CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem]
GO
ALTER TABLE [dbo].[FrameworkServerTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServer] FOREIGN KEY([ServerId])
REFERENCES [dbo].[FrameworkServer] ([ServerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FrameworkServerTypeAssoc] CHECK CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServer]
GO
ALTER TABLE [dbo].[FrameworkServerTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServerType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[FrameworkServerType] ([TypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FrameworkServerTypeAssoc] CHECK CONSTRAINT [FK_FrameworkServerTypeAssoc_FrameworkServerType]
GO
ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_Asset]
GO
ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_PeripheralType] FOREIGN KEY([PeripheralType])
REFERENCES [dbo].[PeripheralType] ([Name])
GO
ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_PeripheralType]
GO
ALTER TABLE [dbo].[PrintDriver]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriver_PrintDriverPackage] FOREIGN KEY([PrintDriverPackageId])
REFERENCES [dbo].[PrintDriverPackage] ([PrintDriverPackageId])
GO
ALTER TABLE [dbo].[PrintDriver] CHECK CONSTRAINT [FK_PrintDriver_PrintDriverPackage]
GO
ALTER TABLE [dbo].[PrintDriverProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverConfig] FOREIGN KEY([DriverConfigId])
REFERENCES [dbo].[PrintDriverConfig] ([DriverConfigId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintDriverProductAssoc] CHECK CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverConfig]
GO
ALTER TABLE [dbo].[PrintDriverProductAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverProduct] FOREIGN KEY([DriverProductId])
REFERENCES [dbo].[PrintDriverProduct] ([DriverProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintDriverProductAssoc] CHECK CONSTRAINT [FK_PrintDriverProductAssoc_PrintDriverProduct]
GO
ALTER TABLE [dbo].[PrintDriverVersionAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverConfig] FOREIGN KEY([DriverConfigId])
REFERENCES [dbo].[PrintDriverConfig] ([DriverConfigId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintDriverVersionAssoc] CHECK CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverConfig]
GO
ALTER TABLE [dbo].[PrintDriverVersionAssoc]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverVersion] FOREIGN KEY([DriverVersionId])
REFERENCES [dbo].[PrintDriverVersion] ([DriverVersionId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrintDriverVersionAssoc] CHECK CONSTRAINT [FK_PrintDriverVersionAssoc_PrintDriverVersion]
GO
ALTER TABLE [dbo].[RemotePrintQueue]  WITH CHECK ADD  CONSTRAINT [FK_RemotePrintQueue_FrameworkServer] FOREIGN KEY([PrintServerId])
REFERENCES [dbo].[FrameworkServer] ([ServerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RemotePrintQueue] CHECK CONSTRAINT [FK_RemotePrintQueue_FrameworkServer]
GO
ALTER TABLE [dbo].[VirtualPrinter]  WITH CHECK ADD  CONSTRAINT [FK_VirtualPrinter_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
ALTER TABLE [dbo].[VirtualPrinter] CHECK CONSTRAINT [FK_VirtualPrinter_Asset]
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_SimulatorUsage]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		kyoungman
-- Create date: 6/15/2012
-- Description:	Returns a list of Simulator usage counts
-- Modified date:
-- Description:
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_SimulatorUsage]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT r.SessionId,
		COUNT(r.AssetId) AS SimCount
	FROM AssetReservation r INNER JOIN DeviceSimulator s ON r.AssetId = s.AssetId
	WHERE r.SessionId IS NOT null GROUP BY r.SessionId
	UNION
	SELECT 'Available' AS SessionId,
		COUNT(s.AssetId) AS SimCount
	FROM AssetReservation r RIGHT JOIN DeviceSimulator s ON r.AssetId = s.AssetId
	WHERE r.SessionId IS null Group BY r.SessionId
		
END
GO
/****** Object:  StoredProcedure [dbo].[sel_Chart_VMUsage]    Script Date: 9/24/2015 4:02:27 PM ******/
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
-- Modified date:  7/25/2014 jlkennedy	Copied and modified from proc with same name on stfsystem01.EnterpriseTest
-- =============================================
CREATE PROCEDURE [dbo].[sel_Chart_VMUsage]
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @now DATETIME
	SET @now = GETDATE()

	SELECT v.SessionId, 
		COUNT(v.VMName)as VMCount
	FROM VirtualMachineReservation v 
	WHERE v.SessionId IS NOT null GROUP BY v.SessionId
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachineReservation v WHERE SessionId IS null AND UsageState != 'Available' GROUP BY v.UsageState
	UNION
	SELECT v.UsageState AS SessionId, 
		COUNT(v.UsageState) AS VMCount
	FROM VirtualMachineReservation v WHERE SessionId IS null AND UsageState = 'Available' GROUP BY v.UsageState
			
END

GO
/****** Object:  StoredProcedure [dbo].[sel_HistoricalData]    Script Date: 9/24/2015 4:02:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		kyoungman
-- Create date: 9/01/2011
-- Description:	Returns a list of all Inventory items in the History table
-- =============================================
CREATE PROCEDURE [dbo].[sel_HistoricalData] 
AS
BEGIN
	SET NOCOUNT ON;
	
    SELECT ISNULL(a.AssetId, 'DELETED') AS Status, m.AssetId, m.LastModified, h.ModifiedBy
	FROM (SELECT AssetId, MAX(ModifiedDate) AS LastModified FROM History GROUP BY AssetId) m
	INNER JOIN History h ON m.AssetId = h.AssetId AND m.LastModified = h.ModifiedDate
	LEFT JOIN Asset a ON m.AssetId = a.AssetId
	ORDER BY m.AssetId
END


GO
