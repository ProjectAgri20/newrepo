USE [AssetInventory]
GO
----------------------------------------------------------------------------
--kyoungman 10/25/2017 - Add MonitorConfig table to Asset Inventory

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MonitorConfig')
BEGIN
	CREATE TABLE [dbo].[MonitorConfig](
		[MonitorConfigId] [uniqueidentifier] NOT NULL,
		[ServerHostName] [varchar](50) NOT NULL,
		[MonitorType] [varchar](50) NOT NULL,
		[Configuration] [xml] NOT NULL,
	 CONSTRAINT [PK_ServiceMonitor] PRIMARY KEY CLUSTERED 
	(
		[MonitorConfigId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

-- Migrate the data from EnterpriseTest.DigitalSendOutputLocation to AssetInventory.MonitorConfig
-- 1. Copy the MonitorLocation field into the ValidationOptions XML block
UPDATE EnterpriseTest.dbo.DigitalSendOutputLocation
SET ValidationOptions.modify(' insert <MonitorLocation>{sql:column("MonitorLocation")}</MonitorLocation> as first into (/DigitalSendValidationOptions)[1] ')
-- 2. Copy and transform the data from EnterpriseTest.DigitalSendOutputLocation to AssetInventory.MonitorConfig
INSERT INTO MonitorConfig (MonitorConfigId, ServerHostName, MonitorType, Configuration)
SELECT DigitalSendOutputLocationId, ServerHostName, OutputType, REPLACE(CAST(ValidationOptions AS VARCHAR(MAX)), 'DigitalSendValidationOptions', 'OutputMonitorConfig') FROM EnterpriseTest.dbo.DigitalSendOutputLocation
GO
--Switch contexts to detect whether the table is there or not
USE EnterpriseTest
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DigitalSendOutputLocation')
    DROP TABLE DigitalSendOutputLocation
GO
--Switch back to AssetInventory
USE [AssetInventory]
GO
----------------------------------------------------------------------------
