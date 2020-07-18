USE [AssetInventory]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].License') AND name = 'RequestSentDate')
BEGIN
    -- prakapra 9/6/2018  Add RequestSentDate column to track when a request for new license has ocurred.
ALTER TABLE License 
ADD RequestSentDate DATETIME NULL

END


---------------------------------------------------------------------------
-- bmyers 10/1/18  Drop unused computed column

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverConfig]') AND name = N'UC_FileUniqueness')
ALTER TABLE [dbo].[PrintDriverConfig] DROP CONSTRAINT [UC_FileUniqueness]
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverConfig' AND COLUMN_NAME = 'FileUniqueness')
ALTER TABLE PrintDriverConfig DROP COLUMN FileUniqueness
GO


---------------------------------------------------------------------------
-- bmyers 10/2/18  Add foreign keys

-- Check for any orphaned data, just to be safe

DELETE FROM Camera WHERE AssetId NOT IN (SELECT AssetId FROM Asset WHERE AssetType = 'Camera')
DELETE FROM Printer WHERE AssetId NOT IN (SELECT AssetId FROM Asset WHERE AssetType = 'Printer')
DELETE FROM DomainAccountReservation WHERE DomainAccountKey NOT IN (SELECT DomainAccountKey FROM DomainAccountPool)

-- Create keys

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Camera_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Camera]'))
ALTER TABLE [dbo].[Camera] DROP CONSTRAINT [FK_Camera_Asset]
GO

ALTER TABLE [dbo].[Camera]  WITH CHECK ADD  CONSTRAINT [FK_Camera_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Camera] CHECK CONSTRAINT [FK_Camera_Asset]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Printer_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Printer]'))
ALTER TABLE [dbo].[Printer] DROP CONSTRAINT [FK_Printer_Asset]
GO

ALTER TABLE [dbo].[Printer]  WITH CHECK ADD  CONSTRAINT [FK_Printer_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Printer] CHECK CONSTRAINT [FK_Printer_Asset]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DomainAccountReservation_DomainAccountPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[DomainAccountReservation]'))
ALTER TABLE [dbo].[DomainAccountReservation] DROP CONSTRAINT [FK_DomainAccountReservation_DomainAccountPool]
GO

ALTER TABLE [dbo].[DomainAccountReservation]  WITH CHECK ADD  CONSTRAINT [FK_DomainAccountReservation_DomainAccountPool] FOREIGN KEY([DomainAccountKey])
REFERENCES [dbo].[DomainAccountPool] ([DomainAccountKey])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DomainAccountReservation] CHECK CONSTRAINT [FK_DomainAccountReservation_DomainAccountPool]
GO


---------------------------------------------------------------------------
-- bmyers 10/2/18  Rename primary and foreign keys to match standard convention

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Asset_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[Asset]'))
EXEC sp_rename 'FK_Asset_Group', 'FK_Asset_AssetPool'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AssetHistory]') AND name = N'PK_History')
EXEC sp_rename 'PK_History', 'PK_AssetHistory'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AssetPool]') AND name = N'PK_Group')
EXEC sp_rename 'PK_Group', 'PK_AssetPool'
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_Group]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetPool]'))
EXEC sp_rename 'FK_Group_Group', 'FK_AssetPool_User'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BashLog]') AND name = N'PK_BashLog')
EXEC sp_rename 'PK_BashLog', 'PK_BashLogCollector'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = N'PK_ServerInventory')
EXEC sp_rename 'PK_ServerInventory', 'PK_FrameworkServer'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerType]') AND name = N'PK_ServerInventoryType')
EXEC sp_rename 'PK_ServerInventoryType', 'PK_FrameworkServerType'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]') AND name = N'PK_ServerInventoryTypeAssoc')
EXEC sp_rename 'PK_ServerInventoryTypeAssoc', 'PK_FrameworkServerTypeAssoc'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[License]') AND name = N'PK_Licence')
EXEC sp_rename 'PK_Licence', 'PK_License'
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseKey_Licence]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseKey]'))
EXEC sp_rename 'FK_LicenseKey_Licence', 'FK_LicenseKey_License'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MachineOperatingSystem]') AND name = N'PK_OperatingSystemInfo')
EXEC sp_rename 'PK_OperatingSystemInfo', 'PK_MachineOperatingSystem'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MonitorConfig]') AND name = N'PK_ServiceMonitor')
EXEC sp_rename 'PK_ServiceMonitor', 'PK_MonitorConfig'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProduct]') AND name = N'PK_UniversalPrintDriverProduct')
EXEC sp_rename 'PK_UniversalPrintDriverProduct', 'PK_PrintDriverProduct'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersion]') AND name = N'PK_UniversalPrintDriverVersion')
EXEC sp_rename 'PK_UniversalPrintDriverVersion', 'PK_PrintDriverVersion'
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrinterProduct]') AND name = N'PK_PrinterProducts')
EXEC sp_rename 'PK_PrinterProducts', 'PK_PrinterProduct'
GO


---------------------------------------------------------------------------
-- bmyers 10/2/18  Modify all column types to match proposed schema

-- Drop keys where required to modify columns

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Asset_AssetPool]') AND parent_object_id = OBJECT_ID(N'[dbo].[Asset]'))
ALTER TABLE [dbo].[Asset] DROP CONSTRAINT [FK_Asset_AssetPool]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AssetPool]') AND name = N'PK_AssetPool')
ALTER TABLE [dbo].[AssetPool] DROP CONSTRAINT [PK_AssetPool] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetPool_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetPool]'))
ALTER TABLE [dbo].[AssetPool] DROP CONSTRAINT [FK_AssetPool_User]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CitrixPublishedApp]') AND name = N'PK_CitrixPublishedApp')
ALTER TABLE [dbo].[CitrixPublishedApp] DROP CONSTRAINT [PK_CitrixPublishedApp] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[LicenseOwner]') AND name = N'PK_LicenseOwner')
ALTER TABLE [dbo].[LicenseOwner] DROP CONSTRAINT [PK_LicenseOwner] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_PeripheralType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral] DROP CONSTRAINT [FK_Peripheral_PeripheralType]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PeripheralType]') AND name = N'PK_PeripheralType')
ALTER TABLE [dbo].[PeripheralType] DROP CONSTRAINT [PK_PeripheralType] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PrinterProduct]') AND name = N'PK_PrinterProduct')
ALTER TABLE [dbo].[PrinterProduct] DROP CONSTRAINT [PK_PrinterProduct] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ServerSetting]') AND name = N'PK_ServerSetting')
ALTER TABLE [dbo].[ServerSetting] DROP CONSTRAINT [PK_ServerSetting] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'PK_User')
ALTER TABLE [dbo].[User] DROP CONSTRAINT [PK_User] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachineReservation]') AND name = N'PK_VirtualMachine')
ALTER TABLE [dbo].[VirtualMachineReservation] DROP CONSTRAINT [PK_VirtualMachine] WITH ( ONLINE = OFF )
GO


-- Modify column types

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Asset' AND COLUMN_NAME = 'PoolName')
ALTER TABLE Asset ALTER COLUMN PoolName nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetHistory' AND COLUMN_NAME = 'Action')
ALTER TABLE AssetHistory ALTER COLUMN Action varchar(10) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetHistory' AND COLUMN_NAME = 'Description')
ALTER TABLE AssetHistory ALTER COLUMN Description nvarchar(1024) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetHistory' AND COLUMN_NAME = 'ModifiedBy')
ALTER TABLE AssetHistory ALTER COLUMN ModifiedBy nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetPool' AND COLUMN_NAME = 'Name')
ALTER TABLE AssetPool ALTER COLUMN Name nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetPool' AND COLUMN_NAME = 'Administrator')
ALTER TABLE AssetPool ALTER COLUMN Administrator nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetReservation' AND COLUMN_NAME = 'ReservedBy')
ALTER TABLE AssetReservation ALTER COLUMN ReservedBy nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetReservation' AND COLUMN_NAME = 'ReservedFor')
ALTER TABLE AssetReservation ALTER COLUMN ReservedFor nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetReservation' AND COLUMN_NAME = 'CreatedBy')
ALTER TABLE AssetReservation ALTER COLUMN CreatedBy nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AssetReservation' AND COLUMN_NAME = 'Notes')
ALTER TABLE AssetReservation ALTER COLUMN Notes nvarchar(MAX) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Badge' AND COLUMN_NAME = 'Username')
ALTER TABLE Badge ALTER COLUMN Username nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Badge' AND COLUMN_NAME = 'Description')
ALTER TABLE Badge ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BadgeBox' AND COLUMN_NAME = 'Description')
ALTER TABLE BadgeBox ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BashLog' AND COLUMN_NAME = 'AssetId')
ALTER TABLE BashLog ALTER COLUMN AssetId varchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Camera' AND COLUMN_NAME = 'CameraServer')
ALTER TABLE Camera ALTER COLUMN CameraServer nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Camera' AND COLUMN_NAME = 'Description')
ALTER TABLE Camera ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CitrixPublishedApp' AND COLUMN_NAME = 'CitrixServer')
ALTER TABLE CitrixPublishedApp ALTER COLUMN CitrixServer nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CitrixPublishedApp' AND COLUMN_NAME = 'ApplicationName')
ALTER TABLE CitrixPublishedApp ALTER COLUMN ApplicationName nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DartBoard' AND COLUMN_NAME = 'Address')
ALTER TABLE DartBoard ALTER COLUMN Address nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DartBoard' AND COLUMN_NAME = 'FwVersion')
ALTER TABLE DartBoard ALTER COLUMN FwVersion nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceSimulator' AND COLUMN_NAME = 'Product')
ALTER TABLE DeviceSimulator ALTER COLUMN Product nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceSimulator' AND COLUMN_NAME = 'Address')
ALTER TABLE DeviceSimulator ALTER COLUMN Address nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceSimulator' AND COLUMN_NAME = 'FirmwareVersion')
ALTER TABLE DeviceSimulator ALTER COLUMN FirmwareVersion varchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DeviceSimulator' AND COLUMN_NAME = 'VirtualMachine')
ALTER TABLE DeviceSimulator ALTER COLUMN VirtualMachine nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DomainAccountPool' AND COLUMN_NAME = 'UserNameFormat')
ALTER TABLE DomainAccountPool ALTER COLUMN UserNameFormat nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DomainAccountPool' AND COLUMN_NAME = 'Description')
ALTER TABLE DomainAccountPool ALTER COLUMN Description nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'HostName')
ALTER TABLE FrameworkServer ALTER COLUMN HostName nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'DiskSpace')
ALTER TABLE FrameworkServer ALTER COLUMN DiskSpace varchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'Description')
ALTER TABLE FrameworkServer ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'Contact')
ALTER TABLE FrameworkServer ALTER COLUMN Contact nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServer' AND COLUMN_NAME = 'Environment')
ALTER TABLE FrameworkServer ALTER COLUMN Environment nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServerType' AND COLUMN_NAME = 'Name')
ALTER TABLE FrameworkServerType ALTER COLUMN Name nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'FrameworkServerType' AND COLUMN_NAME = 'Description')
ALTER TABLE FrameworkServerType ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'License' AND COLUMN_NAME = 'Solution')
ALTER TABLE License ALTER COLUMN Solution nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'License' AND COLUMN_NAME = 'SolutionVersion')
ALTER TABLE License ALTER COLUMN SolutionVersion nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'License' AND COLUMN_NAME = 'InstallationKey')
ALTER TABLE License ALTER COLUMN InstallationKey nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LicenseKey' AND COLUMN_NAME = 'KeyName')
ALTER TABLE LicenseKey ALTER COLUMN KeyName nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LicenseKey' AND COLUMN_NAME = 'Key')
ALTER TABLE LicenseKey ALTER COLUMN [Key] nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LicenseOwner' AND COLUMN_NAME = 'Contact')
ALTER TABLE LicenseOwner ALTER COLUMN Contact nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MachineOperatingSystem' AND COLUMN_NAME = 'Name')
ALTER TABLE MachineOperatingSystem ALTER COLUMN Name nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MonitorConfig' AND COLUMN_NAME = 'ServerHostName')
ALTER TABLE MonitorConfig ALTER COLUMN ServerHostName nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Peripheral' AND COLUMN_NAME = 'PeripheralType')
ALTER TABLE Peripheral ALTER COLUMN PeripheralType nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Peripheral' AND COLUMN_NAME = 'SerialNumber')
ALTER TABLE Peripheral ALTER COLUMN SerialNumber nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Peripheral' AND COLUMN_NAME = 'Interface')
ALTER TABLE Peripheral ALTER COLUMN Interface nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Peripheral' AND COLUMN_NAME = 'Capacity')
ALTER TABLE Peripheral ALTER COLUMN Capacity nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Peripheral' AND COLUMN_NAME = 'Brand')
ALTER TABLE Peripheral ALTER COLUMN Brand nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PeripheralType' AND COLUMN_NAME = 'Name')
ALTER TABLE PeripheralType ALTER COLUMN Name nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriver' AND COLUMN_NAME = 'Name')
ALTER TABLE PrintDriver ALTER COLUMN Name nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriver' AND COLUMN_NAME = 'PrintProcessor')
ALTER TABLE PrintDriver ALTER COLUMN PrintProcessor nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverConfig' AND COLUMN_NAME = 'ConfigFile')
ALTER TABLE PrintDriverConfig ALTER COLUMN ConfigFile nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverPackage' AND COLUMN_NAME = 'Version')
ALTER TABLE PrintDriverPackage ALTER COLUMN Version nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverPackage' AND COLUMN_NAME = 'InfX86')
ALTER TABLE PrintDriverPackage ALTER COLUMN InfX86 nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverPackage' AND COLUMN_NAME = 'InfX64')
ALTER TABLE PrintDriverPackage ALTER COLUMN InfX64 nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverProduct' AND COLUMN_NAME = 'Name')
ALTER TABLE PrintDriverProduct ALTER COLUMN Name nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintDriverVersion' AND COLUMN_NAME = 'Value')
ALTER TABLE PrintDriverVersion ALTER COLUMN Value nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Product')
ALTER TABLE Printer ALTER COLUMN Product nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Model')
ALTER TABLE Printer ALTER COLUMN Model nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Address1')
ALTER TABLE Printer ALTER COLUMN Address1 nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Address2')
ALTER TABLE Printer ALTER COLUMN Address2 nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Description')
ALTER TABLE Printer ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Location')
ALTER TABLE Printer ALTER COLUMN Location nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'Owner')
ALTER TABLE Printer ALTER COLUMN Owner nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'PrinterType')
ALTER TABLE Printer ALTER COLUMN PrinterType nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'FirmwareType')
ALTER TABLE Printer ALTER COLUMN FirmwareType nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'EngineType')
ALTER TABLE Printer ALTER COLUMN EngineType nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'ModelNumber')
ALTER TABLE Printer ALTER COLUMN ModelNumber nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printer' AND COLUMN_NAME = 'SerialNumber')
ALTER TABLE Printer ALTER COLUMN SerialNumber nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrinterProduct' AND COLUMN_NAME = 'Family')
ALTER TABLE PrinterProduct ALTER COLUMN Family nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrinterProduct' AND COLUMN_NAME = 'Name')
ALTER TABLE PrinterProduct ALTER COLUMN Name nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RemotePrintQueue' AND COLUMN_NAME = 'Name')
ALTER TABLE RemotePrintQueue ALTER COLUMN Name nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ReservationHistory' AND COLUMN_NAME = 'ReservedFor')
ALTER TABLE ReservationHistory ALTER COLUMN ReservedFor nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ServerSetting' AND COLUMN_NAME = 'Name')
ALTER TABLE ServerSetting ALTER COLUMN Name nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ServerSetting' AND COLUMN_NAME = 'Value')
ALTER TABLE ServerSetting ALTER COLUMN Value nvarchar(MAX) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ServerSetting' AND COLUMN_NAME = 'Description')
ALTER TABLE ServerSetting ALTER COLUMN Description nvarchar(255) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'UserId')
ALTER TABLE [User] ALTER COLUMN UserId nvarchar(255) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'Role')
ALTER TABLE [User] ALTER COLUMN Role varchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualMachineReservation' AND COLUMN_NAME = 'VMName')
ALTER TABLE VirtualMachineReservation ALTER COLUMN VMName nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualMachineReservation' AND COLUMN_NAME = 'Environment')
ALTER TABLE VirtualMachineReservation ALTER COLUMN Environment nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualMachineReservation' AND COLUMN_NAME = 'PlatformUsage')
ALTER TABLE VirtualMachineReservation ALTER COLUMN PlatformUsage varchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualMachineReservation' AND COLUMN_NAME = 'HoldId')
ALTER TABLE VirtualMachineReservation ALTER COLUMN HoldId nvarchar(50) NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualPrinter' AND COLUMN_NAME = 'Address')
ALTER TABLE VirtualPrinter ALTER COLUMN Address nvarchar(50) NOT NULL
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'VirtualPrinter' AND COLUMN_NAME = 'HostName')
ALTER TABLE VirtualPrinter ALTER COLUMN HostName nvarchar(50) NOT NULL
GO


-- Recreate keys

ALTER TABLE [dbo].[User] ADD  CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssetPool] ADD  CONSTRAINT [PK_AssetPool] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_AssetPool] FOREIGN KEY([PoolName])
REFERENCES [dbo].[AssetPool] ([Name])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_AssetPool]
GO

ALTER TABLE [dbo].[AssetPool]  WITH CHECK ADD  CONSTRAINT [FK_AssetPool_User] FOREIGN KEY([Administrator])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[AssetPool] CHECK CONSTRAINT [FK_AssetPool_User]
GO

ALTER TABLE [dbo].[CitrixPublishedApp] ADD  CONSTRAINT [PK_CitrixPublishedApp] PRIMARY KEY CLUSTERED 
(
	[CitrixServer] ASC,
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LicenseOwner] ADD  CONSTRAINT [PK_LicenseOwner] PRIMARY KEY CLUSTERED 
(
	[LicenseId] ASC,
	[Contact] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PeripheralType] ADD  CONSTRAINT [PK_PeripheralType] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_PeripheralType] FOREIGN KEY([PeripheralType])
REFERENCES [dbo].[PeripheralType] ([Name])
GO

ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_PeripheralType]
GO

ALTER TABLE [dbo].[PrinterProduct] ADD  CONSTRAINT [PK_PrinterProduct] PRIMARY KEY CLUSTERED 
(
	[Family] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ServerSetting] ADD  CONSTRAINT [PK_ServerSetting] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[VirtualMachineReservation] ADD  CONSTRAINT [PK_VirtualMachine] PRIMARY KEY CLUSTERED 
(
	[VMName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


---------------------------------------------------------------------------
-- bmyers 10/2/18  Modify foreign key cascade update/delete

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetPool_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetPool]'))
ALTER TABLE [dbo].[AssetPool] DROP CONSTRAINT [FK_AssetPool_User]
GO

ALTER TABLE [dbo].[AssetPool]  WITH CHECK ADD  CONSTRAINT [FK_AssetPool_User] FOREIGN KEY([Administrator])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[AssetPool] CHECK CONSTRAINT [FK_AssetPool_User]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AssetReservation_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] DROP CONSTRAINT [FK_AssetReservation_Asset]
GO

ALTER TABLE [dbo].[AssetReservation]  WITH CHECK ADD  CONSTRAINT [FK_AssetReservation_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AssetReservation] CHECK CONSTRAINT [FK_AssetReservation_Asset]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Badge_BadgeBox]') AND parent_object_id = OBJECT_ID(N'[dbo].[Badge]'))
ALTER TABLE [dbo].[Badge] DROP CONSTRAINT [FK_Badge_BadgeBox]
GO

ALTER TABLE [dbo].[Badge]  WITH NOCHECK ADD  CONSTRAINT [FK_Badge_BadgeBox] FOREIGN KEY([BadgeBoxId])
REFERENCES [dbo].[BadgeBox] ([BadgeBoxId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Badge] NOCHECK CONSTRAINT [FK_Badge_BadgeBox]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeviceSimulator_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeviceSimulator]'))
ALTER TABLE [dbo].[DeviceSimulator] DROP CONSTRAINT [FK_DeviceSimulator_Asset]
GO

ALTER TABLE [dbo].[DeviceSimulator]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSimulator_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeviceSimulator] CHECK CONSTRAINT [FK_DeviceSimulator_Asset]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServer_MachineOperatingSystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServer]'))
ALTER TABLE [dbo].[FrameworkServer] DROP CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem]
GO

ALTER TABLE [dbo].[FrameworkServer]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem] FOREIGN KEY([OperatingSystemId])
REFERENCES [dbo].[MachineOperatingSystem] ([OperatingSystemId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[FrameworkServer] CHECK CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseKey_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseKey]'))
ALTER TABLE [dbo].[LicenseKey] DROP CONSTRAINT [FK_LicenseKey_License]
GO

ALTER TABLE [dbo].[LicenseKey]  WITH CHECK ADD  CONSTRAINT [FK_LicenseKey_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LicenseKey] CHECK CONSTRAINT [FK_LicenseKey_License]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LicenseOwner_License]') AND parent_object_id = OBJECT_ID(N'[dbo].[LicenseOwner]'))
ALTER TABLE [dbo].[LicenseOwner] DROP CONSTRAINT [FK_LicenseOwner_License]
GO

ALTER TABLE [dbo].[LicenseOwner]  WITH CHECK ADD  CONSTRAINT [FK_LicenseOwner_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LicenseOwner] CHECK CONSTRAINT [FK_LicenseOwner_License]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral] DROP CONSTRAINT [FK_Peripheral_Asset]
GO

ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_Asset]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Peripheral_PeripheralType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Peripheral]'))
ALTER TABLE [dbo].[Peripheral] DROP CONSTRAINT [FK_Peripheral_PeripheralType]
GO

ALTER TABLE [dbo].[Peripheral]  WITH CHECK ADD  CONSTRAINT [FK_Peripheral_PeripheralType] FOREIGN KEY([PeripheralType])
REFERENCES [dbo].[PeripheralType] ([Name])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Peripheral] CHECK CONSTRAINT [FK_Peripheral_PeripheralType]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PrintDriver_PrintDriverPackage]') AND parent_object_id = OBJECT_ID(N'[dbo].[PrintDriver]'))
ALTER TABLE [dbo].[PrintDriver] DROP CONSTRAINT [FK_PrintDriver_PrintDriverPackage]
GO

ALTER TABLE [dbo].[PrintDriver]  WITH CHECK ADD  CONSTRAINT [FK_PrintDriver_PrintDriverPackage] FOREIGN KEY([PrintDriverPackageId])
REFERENCES [dbo].[PrintDriverPackage] ([PrintDriverPackageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PrintDriver] CHECK CONSTRAINT [FK_PrintDriver_PrintDriverPackage]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RemotePrintQueue_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]'))
ALTER TABLE [dbo].[RemotePrintQueue] DROP CONSTRAINT [FK_RemotePrintQueue_FrameworkServer]
GO

ALTER TABLE [dbo].[RemotePrintQueue]  WITH CHECK ADD  CONSTRAINT [FK_RemotePrintQueue_FrameworkServer] FOREIGN KEY([PrintServerId])
REFERENCES [dbo].[FrameworkServer] ([ServerId])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[RemotePrintQueue] CHECK CONSTRAINT [FK_RemotePrintQueue_FrameworkServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerSetting_FrameworkServer]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerSetting]'))
ALTER TABLE [dbo].[ServerSetting] DROP CONSTRAINT [FK_ServerSetting_FrameworkServer]
GO

ALTER TABLE [dbo].[ServerSetting]  WITH CHECK ADD  CONSTRAINT [FK_ServerSetting_FrameworkServer] FOREIGN KEY([ServerId])
REFERENCES [dbo].[FrameworkServer] ([ServerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ServerSetting] CHECK CONSTRAINT [FK_ServerSetting_FrameworkServer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VirtualPrinter_Asset]') AND parent_object_id = OBJECT_ID(N'[dbo].[VirtualPrinter]'))
ALTER TABLE [dbo].[VirtualPrinter] DROP CONSTRAINT [FK_VirtualPrinter_Asset]
GO

ALTER TABLE [dbo].[VirtualPrinter]  WITH CHECK ADD  CONSTRAINT [FK_VirtualPrinter_Asset] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[VirtualPrinter] CHECK CONSTRAINT [FK_VirtualPrinter_Asset]
GO


---------------------------------------------------------------------------
-- bmyers 10/3/18  Drop unused stored procedures

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_SimulatorUsage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_SimulatorUsage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_VMUsage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_VMUsage]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_HistoricalData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_HistoricalData]
GO


---------------------------------------------------------------------------
-- bmyers 10/17/18 Resolve issue with incorrect data saved to Printer.Address2

UPDATE Printer SET Address2 = NULL WHERE Address2 = '...'


