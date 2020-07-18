USE [EnterpriseTest]
GO

-- 1/8/2016 - BJ Myers - Add table for storing server selection data.

/****** Object:  Table [dbo].[VirtualResourceMetadataServerUsage]    Script Date: 1/8/2016 4:10:19 PM ******/
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
    ALTER TABLE [dbo].[VirtualResourceMetadataServerUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
    REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
    ON UPDATE CASCADE
    ON DELETE CASCADE
    ALTER TABLE [dbo].[VirtualResourceMetadataServerUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataServerUsage_VirtualResourceMetadata]
END
GO


-- 1/20/2016 - BJ Myers - Remove metadata entries and activities for retired plugins
DELETE FROM VirtualResourceMetadata WHERE MetadataType IN ('ActivityOutcomeDevice', 'CardSwiper', 'EpicPrintJob', 'FileBrowsing', 'MajorKong', 'WebAutomation', 'WebBrowsing', 'WERCollector')
DELETE FROM MetadataTypeResourceTypeAssoc WHERE MetadataTypeName IN ('ActivityOutcomeDevice', 'CardSwiper', 'EpicPrintJob', 'FileBrowsing', 'MajorKong', 'WebAutomation', 'WebBrowsing', 'WERCollector')
DELETE FROM MetadataType WHERE Name IN ('ActivityOutcomeDevice', 'CardSwiper', 'EpicPrintJob', 'FileBrowsing', 'MajorKong', 'WebAutomation', 'WebBrowsing', 'WERCollector')


-- 1/20/2016 - BJ Myers - Drop unused WebSite table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WebSite]') AND type in (N'U'))
DROP TABLE [dbo].[WebSite]
GO

-- 1/29/2016 - BJ Myers - Add table for storing print queue selection data.

/****** Object:  Table [dbo].[VirtualResourceMetadataPrintQueueUsage]    Script Date: 1/29/2016 4:20:49 PM ******/
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
	ALTER TABLE [dbo].[VirtualResourceMetadataPrintQueueUsage]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
	REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	ALTER TABLE [dbo].[VirtualResourceMetadataPrintQueueUsage] CHECK CONSTRAINT [FK_VirtualResourceMetadataPrintQueueUsage_VirtualResourceMetadata]
END
GO

------------------------------------------------------------------------------------------------------------------------
-- 2/4/2016 - kyoungman  Add SubType column to SystemSetting table, then merge data from Plugin Settings.  Drop PluginSettings table.

--First rename the current SystemSetting table
EXEC sp_rename 'SystemSetting', 'SystemSetting_old'
GO
ALTER TABLE SystemSetting_old
DROP CONSTRAINT [PK_SystemSetting]
GO
---------------------------------------------------
-- Create the new SystemSetting table
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SystemSetting](
	[Type] [varchar](50) NOT NULL,
	[SubType] [varchar](50) NOT NULL DEFAULT(''),
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_SystemSetting] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[Subtype] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
---------------------------------------------------
-- Port all data from old SystemSetting into the new one
insert into SystemSetting ([Type], [SubType], [Name], [Value], [Description])
select 'SystemSetting', [Type], Name, Value, [Description] from SystemSetting_old
GO
-- Port all data from PluginSetting into the new one
insert into SystemSetting ([Type], [SubType], [Name], [Value], [Description])
select 'PluginSetting', Plugin, Name, Value, [Description] from PluginSetting
GO

--Set up Exchange Server settings
-- HUB:  You will need to modify this for your environment if you don't already have "EwsUrl" in your SystemSettings.
UPDATE SystemSetting SET [Type] = 'ServerSetting', [SubType] = 'ETLEXCHANGE01' WHERE Name IN ('ExchangeVersion','EwsAutodiscoverEnabled','EwsUrl')
INSERT SystemSetting VALUES ('ServerSetting', 'ETLEXCHANGE01', 'EwsUrl', 'https://etlexchange01.etl.boi.rd.hpicorp.net/ews/exchange.asmx', 'Exhange Web Service URL when not Autodiscovering')
GO

DROP TABLE SystemSetting_old
GO
DROP TABLE PluginSetting

------------------------------------------------------------------------------------------------------------------------

-- 1/20/2016 - BJ Myers - Drop unused VPNConfiguration table
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VPNConfiguration]') AND type in (N'U'))
DROP TABLE [dbo].[VPNConfiguration]
GO

------------------------------------------------------------------------------------------------------------------------
-- 2/11/2016 - JLKennedy - Create table for holding retry settings per scenario activity

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VirtualResourceMetadataRetrySetting]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[VirtualResourceMetadataRetrySetting](
	    [SettingId] [uniqueidentifier] NOT NULL,
	    [VirtualResourceMetadataId] [uniqueidentifier] NOT NULL,
	    [State] [varchar](50) NOT NULL,
	    [Action] [varchar](30) NOT NULL,
	    [RetryLimit] [int] NOT NULL,
	    [RetryDelay] [int] NOT NULL,
	    [LimitExceededAction] [varchar](30) NOT NULL,
     CONSTRAINT [PK_VirtualResourceMetadataRetrySetting_1] PRIMARY KEY CLUSTERED 
    (
	    [SettingId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[VirtualResourceMetadataRetrySetting]  WITH CHECK ADD  CONSTRAINT [FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata] FOREIGN KEY([VirtualResourceMetadataId])
    REFERENCES [dbo].[VirtualResourceMetadata] ([VirtualResourceMetadataId])
    ON UPDATE CASCADE
    ON DELETE CASCADE
    ALTER TABLE [dbo].[VirtualResourceMetadataRetrySetting] CHECK CONSTRAINT [FK_VirtualResourceMetadataRetrySetting_VirtualResourceMetadata]
END
GO


------------------------------------------------------------------------------------------------------------------------
-- 2/15/2016 - BJ Myers - Update MetadataType records for plugins whose names are changing

-- Drop foreign key constraints while we update the tables
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] DROP CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] DROP CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType]
GO

-- Create temporary table to hold metadata type mapping
CREATE TABLE [dbo].[MetadataTypeNameChange](
	[OldName] [varchar](50) NOT NULL,
	[NewName] [varchar](50) NOT NULL
)
GO
INSERT INTO MetadataTypeNameChange (OldName, NewName) VALUES
('ComplexPrinting', 'ComplexPrint'),
('ControlPanelDAT', 'ControlPanel'),
('Dot1X', 'DotOneX'),
('ePrint', 'EPrint'),
('EWS', 'Ews'),
('EWSDAT', 'EwsHeadless'),
('FMS', 'Fms'),
('HpacAuthentication', 'HpacSimulation'),
('HPCR', 'HpcrSimulation'),
('HPEC', 'Hpec'),
('LANFax', 'LanFax'),
('NEST', 'NetworkEmulation'),
('NetworkDiscovery', 'Discovery'),
('NetworkNamingService', 'NetworkNamingServices'),
('PullPrint', 'PSPullPrint'),
('SafeComAuthentication', 'SafeComSimulation'),
('TelnetSNMP', 'TelnetSnmp'),
('VPN', 'Vpn')
GO

-- Rename records in MetadataType
UPDATE mt
SET mt.Name = mtnc.NewName
FROM MetadataType mt
INNER JOIN MetadataTypeNameChange mtnc on mt.Name = mtnc.OldName
GO

-- Rename records in MetadataTypeResourceTypeAssoc
UPDATE mtrta
SET mtrta.MetadataTypeName = mtnc.NewName
FROM MetadataTypeResourceTypeAssoc mtrta
INNER JOIN MetadataTypeNameChange mtnc on mtrta.MetadataTypeName = mtnc.OldName
GO

-- Rename records in MetadataInstallerPackageAssoc
UPDATE mipa
SET mipa.MetadataType = mtnc.NewName
FROM MetadataInstallerPackageAssoc mipa
INNER JOIN MetadataTypeNameChange mtnc on mipa.MetadataType = mtnc.OldName
GO

-- Drop temporary table
DROP TABLE [MetadataTypeNameChange]

-- Restore foreign key constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType] FOREIGN KEY([MetadataTypeName])
REFERENCES [dbo].[MetadataType] ([Name])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataTypeResourceTypeAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataTypeResourceTypeAssoc]'))
ALTER TABLE [dbo].[MetadataTypeResourceTypeAssoc] CHECK CONSTRAINT [FK_MetadataTypeResourceTypeAssoc_MetadataType]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc]  WITH CHECK ADD  CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType] FOREIGN KEY([MetadataType])
REFERENCES [dbo].[MetadataType] ([Name])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MetadataInstallerPackageAssoc_MetadataType]') AND parent_object_id = OBJECT_ID(N'[dbo].[MetadataInstallerPackageAssoc]'))
ALTER TABLE [dbo].[MetadataInstallerPackageAssoc] CHECK CONSTRAINT [FK_MetadataInstallerPackageAssoc_MetadataType]
GO

------------------------------------------------------------------------------------------------------------------------
-- 2/22/2016 - kyoungman  Drop VCenterPassword column from User Table

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'User' AND COLUMN_NAME = 'VCenterPassword')
BEGIN
    -- first drop the default constraint
    DECLARE @sql NVARCHAR(MAX)
    WHILE 1=1
    BEGIN
        SELECT TOP 1 @sql = N'alter table [User] drop constraint ['+dc.NAME+N']'
        from sys.default_constraints dc
        JOIN sys.columns c
            ON c.default_object_id = dc.[object_id]
        WHERE 
            dc.parent_object_id = OBJECT_ID('User')
        AND c.name = N'VCenterPassword'
        IF @@ROWCOUNT = 0 BREAK
        EXEC (@sql)
    END

    -- then drop the column
    ALTER TABLE [User] DROP COLUMN VCenterPassword
END
GO

--Change VMWareServerUri to VCenter 6 instance
UPDATE SystemSetting SET [Value] = 'https://15.86.232.22/sdk' WHERE Name = 'VMWareServerUri'
 -- https://16.70.37.41/sdk
-- HUB:  You will not need to run the above since you are already using VCenter 6.  (But you will need to drop the VCenterPassword column)
DELETE SystemSetting WHERE Name = 'VSphereUserDomain' --Not being used anymore

--Delete the DSSDatabaseInstance SystemSetting
DELETE SystemSetting WHERE Name = 'DSSDatabaseInstance'

------------------------------------------------------------------------------------------------------------------------
-- 2/24/2016 - BJ Myers - Drop obsolete tables

/****** Object:  Table [dbo].[ActivityExceptionSetting]    Script Date: 2/24/2016 10:41:02 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityExceptionSetting]') AND type in (N'U'))
DROP TABLE [dbo].[ActivityExceptionSetting]
GO

/****** Object:  Table [dbo].[MetadataResourceUsage]    Script Date: 2/24/2016 10:41:13 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataResourceUsage]') AND type in (N'U'))
DROP TABLE [dbo].[MetadataResourceUsage]
GO


------------------------------------------------------------------------------------------------------------------------
-- 3/3/2016 - BJ Myers - Remove ExceptionDefinitions node from execution plans - prevents unsaved changes dialogs
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

WITH XMLNAMESPACES (DEFAULT 'http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework' )
UPDATE vrm
SET ExecutionPlan.modify('delete /WorkerExecutionPlan/ExceptionDefinitions')
FROM VirtualResourceMetadata vrm
INNER JOIN VirtualResource vr on vrm.VirtualResourceId = vr.VirtualResourceId
WHERE ExecutionPlan.exist('/WorkerExecutionPlan/ExceptionDefinitions') = 1
GO

SET ANSI_PADDING OFF
GO
