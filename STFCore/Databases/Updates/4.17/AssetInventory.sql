USE [AssetInventory]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING ASSETINVENTORY TABLES
-- Not all systems may have the AssetInventory table that is being updated.
-- This is especially true with the STB database.  So, if you are altering the
-- database schema then please wrap the SQL commands with the appropriate check
-- below.  This will help to ensure that an error does not occur when the SQL
-- script is ran.
--
-- * Check if table exists.
-- IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.<YOUR_TABLE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR TABLE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a table does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a view exists.
-- IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID('dbo.<YOUR_VIEW_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR VIEW ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a view does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
--
-- * Check if a stored procedure exists.
-- IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID('dbo.<YOUR_PROCEDURE_NAME_HERE>'))
-- BEGIN
--     -- <ENTER YOUR PROCEDURE ALTER SCRIPT HERE>
-- END
--
-- Note: To check if a stored procedure does not exist, change the 'IF'
--       statement to
--          IF NOT EXISTS ( ... )
--
-- * Check if a column exists in a specific table.
-- IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.<TABLE_NAME_HERE>') and name = 'COLUMN_NAME_HERE')
-- BEGIN
--     -- <ENTER YOUR COLUMN SCRIPT HERE>
-- END
--
-- Note: To check if a column does not exist, change the 'IF' statement to
--       IF NOT EXISTS ( ... )
/*===========================================================================*/

-- bmyers 3/18/19 Rename table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BashLog]') AND type in (N'U'))
EXEC sp_rename 'BashLog', 'BashLogCollector'
GO


-- bmyers 3/18/19 Drop function & constraint so that referenced column can be renamed
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] DROP CONSTRAINT [CK_DuplicateReservations]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_OverlappingReservationExists]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[udf_OverlappingReservationExists]
GO


-- bmyers 3/18/19 Rename columns
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AssetHistory]') AND name = 'HistoryId')
EXEC sp_rename 'AssetHistory.HistoryId', 'AssetHistoryId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AssetReservation]') AND name = 'ReservationId')
EXEC sp_rename 'AssetReservation.ReservationId', 'AssetReservationId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Badge]') AND name = 'Username')
EXEC sp_rename 'Badge.Username', 'UserName'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BashLogCollector]') AND name = 'AssetId')
EXEC sp_rename 'BashLogCollector.AssetId', 'PrinterId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DartBoard]') AND name = 'FwVersion')
EXEC sp_rename 'DartBoard.FwVersion', 'FirmwareVersion'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = 'IpAddress')
EXEC sp_rename 'FrameworkServer.IpAddress', 'IPAddress'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = 'ServerId')
EXEC sp_rename 'FrameworkServer.ServerId', 'FrameworkServerId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerType]') AND name = 'TypeId')
EXEC sp_rename 'FrameworkServerType.TypeId', 'FrameworkServerTypeId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]') AND name = 'ServerId')
EXEC sp_rename 'FrameworkServerTypeAssoc.ServerId', 'FrameworkServerId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServerTypeAssoc]') AND name = 'TypeId')
EXEC sp_rename 'FrameworkServerTypeAssoc.TypeId', 'FrameworkServerTypeId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[License]') AND name = 'ServerId')
EXEC sp_rename 'License.ServerId', 'FrameworkServerId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[LicenseKey]') AND name = 'KeyId')
EXEC sp_rename 'LicenseKey.KeyId', 'LicenseKeyId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverConfig]') AND name = 'DriverConfigId')
EXEC sp_rename 'PrintDriverConfig.DriverConfigId', 'PrintDriverConfigId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverPackage]') AND name = 'Version')
EXEC sp_rename 'PrintDriverPackage.Version', 'Name'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProduct]') AND name = 'DriverProductId')
EXEC sp_rename 'PrintDriverProduct.DriverProductId', 'PrintDriverProductId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]') AND name = 'DriverConfigId')
EXEC sp_rename 'PrintDriverProductAssoc.DriverConfigId', 'PrintDriverConfigId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverProductAssoc]') AND name = 'DriverProductId')
EXEC sp_rename 'PrintDriverProductAssoc.DriverProductId', 'PrintDriverProductId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersion]') AND name = 'DriverVersionId')
EXEC sp_rename 'PrintDriverVersion.DriverVersionId', 'PrintDriverVersionId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]') AND name = 'DriverConfigId')
EXEC sp_rename 'PrintDriverVersionAssoc.DriverConfigId', 'PrintDriverConfigId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PrintDriverVersionAssoc]') AND name = 'DriverVersionId')
EXEC sp_rename 'PrintDriverVersionAssoc.DriverVersionId', 'PrintDriverVersionId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]') AND name = 'InventoryId')
EXEC sp_rename 'RemotePrintQueue.InventoryId', 'PrinterId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]') AND name = 'PrintQueueId')
EXEC sp_rename 'RemotePrintQueue.PrintQueueId', 'RemotePrintQueueId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ReservationHistory]') AND name = 'HistoryId')
EXEC sp_rename 'ReservationHistory.HistoryId', 'ReservationHistoryId'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ServerSetting]') AND name = 'ServerId')
EXEC sp_rename 'ServerSetting.ServerId', 'FrameworkServerId'
GO


-- bmyers 3/18/19 Restore function & constraint
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

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation]  WITH NOCHECK ADD  CONSTRAINT [CK_DuplicateReservations] CHECK  (([dbo].[udf_OverlappingReservationExists]([AssetReservationId],[AssetId],[Start],[End])=(0)))
GO

IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_DuplicateReservations]') AND parent_object_id = OBJECT_ID(N'[dbo].[AssetReservation]'))
ALTER TABLE [dbo].[AssetReservation] CHECK CONSTRAINT [CK_DuplicateReservations]
GO


-- bmyers 3/18/19 Drop obsolete columns
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AssetPool]') AND name = 'Group')
ALTER TABLE [dbo].[AssetPool] DROP COLUMN [Group]
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VirtualPrinter]') AND name = 'HostName')
ALTER TABLE [dbo].[VirtualPrinter] DROP COLUMN [HostName]
GO


-- bmyers 3/18/19 Drop default value constraints
DECLARE @dropConstraintsSql NVARCHAR(MAX) = N'';

SELECT @dropConstraintsSql += N'
ALTER TABLE ' + OBJECT_NAME(parent_object_id) + ' DROP CONSTRAINT ' + name + ';'
FROM sys.default_constraints;

--PRINT @dropConstraintsSql;
EXEC sp_executesql @dropConstraintsSql;
GO


-- kyoungman 3/26/19 Modify primary key for Badge and BadgeBox to varchar

--Drop Foreign Key Constraint from Badge
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_Badge_BadgeBox') AND parent_object_id = OBJECT_ID(N'dbo.Badge'))
   ALTER TABLE [Badge] DROP CONSTRAINT [FK_Badge_BadgeBox]

--Drop Primary Key Constraint from Badge
IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND object_name(parent_object_id) = 'Badge')
   ALTER TABLE [Badge] DROP CONSTRAINT [PK_Badge]

--Drop Primary Key Constraint from BadgeBox
IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND object_name(parent_object_id) = 'BadgeBox')
   ALTER TABLE [BadgeBox] DROP CONSTRAINT [PK_BadgeBox]
GO
--Update BadgeBox
ALTER TABLE dbo.BadgeBox
   ALTER COLUMN BadgeBoxId VARCHAR(50) NOT NULL

--Update Badge
ALTER TABLE dbo.Badge
   ALTER COLUMN BadgeId VARCHAR(50) NOT NULL
ALTER TABLE dbo.Badge
   ALTER COLUMN BadgeBoxId VARCHAR(50)
GO

--Reset the Ids with the text from Description
UPDATE Badge SET BadgeId = [Description]
UPDATE Badge SET BadgeBoxId = (SELECT [Description] FROM BadgeBox WHERE BadgeBoxId = Badge.BadgeBoxId) 
UPDATE BadgeBox SET BadgeBoxId = [Description]

--Restore Primary Key Constraint to BadgeBox
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND object_name(parent_object_id) = 'BadgeBox')
   ALTER TABLE [BadgeBox] ADD CONSTRAINT [PK_BadgeBox] PRIMARY KEY CLUSTERED (BadgeBoxId)

--Restore Primary Key Constraint to Badge
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND object_name(parent_object_id) = 'Badge')
   ALTER TABLE [Badge] ADD CONSTRAINT [PK_Badge] PRIMARY KEY CLUSTERED (BadgeId)

--Restore Foreign Key Constraint to Badge
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_Badge_BadgeBox') AND parent_object_id = OBJECT_ID(N'dbo.Badge'))
   ALTER TABLE [Badge] ADD CONSTRAINT [FK_Badge_BadgeBox] FOREIGN KEY (BadgeBoxId) REFERENCES [BadgeBox] (BadgeBoxId) ON UPDATE CASCADE
GO


-- bmyers 3/26/19 Change primary key column of RemotePrintQueue
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]') AND name = N'PK_RemotePrintQueue')
ALTER TABLE [dbo].[RemotePrintQueue] DROP CONSTRAINT [PK_RemotePrintQueue] WITH ( ONLINE = OFF )
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RemotePrintQueue]') AND name = N'PK_RemotePrintQueue')
ALTER TABLE [dbo].[RemotePrintQueue] ADD  CONSTRAINT [PK_RemotePrintQueue] PRIMARY KEY CLUSTERED 
(
	[RemotePrintQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


-- bmyers 3/27/19 Move operating system info to FrameworkServer table and drop MachineOperatingSystem table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = 'OperatingSystem')
ALTER TABLE FrameworkServer ADD OperatingSystem nvarchar(255) NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MachineOperatingSystem]') AND type in (N'U'))
UPDATE FrameworkServer
SET OperatingSystem  = MachineOperatingSystem.Name
FROM FrameworkServer
INNER JOIN MachineOperatingSystem ON FrameworkServer.OperatingSystemId = MachineOperatingSystem.OperatingSystemId
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = 'OperatingSystem')
ALTER TABLE FrameworkServer ALTER COLUMN OperatingSystem nvarchar(255) NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkServer_MachineOperatingSystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkServer]'))
ALTER TABLE [dbo].[FrameworkServer] DROP CONSTRAINT [FK_FrameworkServer_MachineOperatingSystem]
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FrameworkServer]') AND name = 'OperatingSystemId')
ALTER TABLE FrameworkServer DROP COLUMN OperatingSystemId, Revision, ServicePack
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MachineOperatingSystem]') AND type in (N'U'))
DROP TABLE [dbo].[MachineOperatingSystem]
GO


-- bmyers 3/29/19 Create FrameworkClient and FrameworkClientPlatform tables
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

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClient] FOREIGN KEY([FrameworkClientHostName])
REFERENCES [dbo].[FrameworkClient] ([FrameworkClientHostName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FrameworkClientPlatformAssoc_FrameworkClient]') AND parent_object_id = OBJECT_ID(N'[dbo].[FrameworkClientPlatformAssoc]'))
ALTER TABLE [dbo].[FrameworkClientPlatformAssoc] CHECK CONSTRAINT [FK_FrameworkClientPlatformAssoc_FrameworkClient]
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


-- bmyers 3/29/19 Delete old table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualMachineReservation]') AND type in (N'U'))
DROP TABLE [dbo].[VirtualMachineReservation]
GO

