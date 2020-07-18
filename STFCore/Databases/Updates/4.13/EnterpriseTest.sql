USE [EnterpriseTest]
GO

/*===========================================================================*/
-- IF YOU ARE UPDATING ENTERPRISETEST TABLES
-- Not all systems may have the EnterpriseTest table that is being updated.
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



--4/5/2018 bermudew: Adds a plugin setting for falcon service location if the Low Level Configuration plugin is in the database.
IF EXISTS(
SELECT * FROM
(SELECT Count([Name]) as CountTotal
      
  FROM [EnterpriseTest].[dbo].[MetadataType]
  WHERE [Name] = 'LowLevelConfig') as A 
  WHERE A.CountTotal != 0 
	--Add: And we don't already have the value added	
  )
  AND NOT EXISTS(SELECT * FROM 
  (SELECT COUNT([Name]) as ExistingPluginCount
  FROM [EnterpriseTest].dbo.SystemSetting
  WHERE [Name] = 'FalconWebServiceURL') AS B
  WHERE B.ExistingPluginCount = 1
  )
  BEGIN
	INSERT INTO [dbo].SystemSetting
	([Type]
	,[SubType]
	,[Name]
	,[Value]
	,[Description])
	VALUES
	(
		'PluginSetting',
		'LowLevelConfig',
		'FalconWebServiceURL',
		'http://15.41.134.213:8888',
		'Used to Route the LowLevelConfig Plugin to the Falcon Service'
	)
  END
-------------------------------------------------------------------------------

--4/20/2018 kyoungman CR 3034. Correct schedule data for Scheduled Execution OfficeWorkers.  Should default to 1, not 0 
UPDATE OfficeWorker SET ExecutionSchedule = REPLACE(CAST(ExecutionSchedule AS VARCHAR(MAX)), 'RepeatCount>0<', 'RepeatCount>1<')
-------------------------------------------------------------------------------

--5/8/2018 bermudew Scripts for Product Association rewrite

--Creates ProductPluginAssociation Table
 IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ProductPluginAssociation'))
 BEGIN

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	SET ANSI_PADDING ON

	CREATE TABLE [dbo].[ProductPluginAssociation](
		[MetadataType] [varchar](50) NOT NULL,
		[AssociatedProductId] [uniqueidentifier] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[MetadataType] ASC,
		[AssociatedProductId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	SET ANSI_PADDING OFF
	
	ALTER TABLE [dbo].[ProductPluginAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ProductPluginAssociation_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
	REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
	ON UPDATE CASCADE
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[ProductPluginAssociation] CHECK CONSTRAINT [FK_ProductPluginAssociation_AssociatedProduct]

	ALTER TABLE [dbo].[ProductPluginAssociation]  WITH CHECK ADD  CONSTRAINT [FK_ProductPluginAssociation_MetadataType] FOREIGN KEY([MetadataType])
	REFERENCES [dbo].[MetadataType] ([Name])
	ON UPDATE CASCADE
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ProductPluginAssociation] CHECK CONSTRAINT [FK_ProductPluginAssociation_MetadataType]
	
END

--Removes MatchCriteria COLUMN
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProduct') and name = 'MatchCriteria')
BEGIN
     ALTER TABLE dbo.AssociatedProduct DROP COLUMN MatchCriteria
END

--Remove Name COLUMN from AssociatedProductVersion as it's not needed
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProductVersion') and name = 'Name')
BEGIN
     ALTER TABLE dbo.AssociatedProductVersion DROP COLUMN Name
END

--Add Active Field to Associated Product Version Table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProductVersion') and name = 'Active')
BEGIN
     ALTER TABLE dbo.AssociatedProductVersion ADD Active  BIT NOT NULL default 'TRUE'
END

--------------------------------------------------------------------------
-- 05/30/2018 dwa Needed to update the name space for CollectDeviceSystemInfo
-- The following two update statements updated the metadata for the 
-- collectDeviceSystemInfo to current standards

UPDATE VirtualResourceMetadata SET Metadata = Replace(cast(Metadata as varchar(max)), '2004/07/Plugin.CollectDeviceSystemInfo', '2004/07/HP.ScalableTest.Plugin.CollectDeviceSystemInfo') 
where MetadataType = 'CollectDeviceSystemInfo'

UPDATE VirtualResourceMetadata SET Metadata = Replace(cast(Metadata as varchar(max)), 'z:Type="Plugin.CollectDeviceSystemInfo.CollectDeviceSystemInfoActivityData"', 'z:Type="HP.ScalableTest.Plugin.CollectDeviceSystemInfo.CollectDeviceSystemInfoActivityData"')
 where MetadataType = 'CollectDeviceSystemInfo'

--------------------------------------------------------------------------
-- 05/31/2018 dwa Needed to update the name space for Printing
-- The following three update statements updated the metadata for the 
-- Printing to current standards

UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'xmlns="http://schemas.datacontract.org/2004/07/HP.ScalableTest.Plugin.Printing"', 'xmlns="http://schemas.datacontract.org/2004/07/HP.ScalableTest.PluginSupport.Print"') 
WHERE MetadataType = 'Printing'

UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'z:Type="HP.ScalableTest.Plugin.Printing.PrintingActivityData"', 'z:Type="HP.ScalableTest.PluginSupport.Print.PrintingActivityData"') 
WHERE MetadataType = 'Printing'

UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'z:Assembly="Plugin.Printing,', 'z:Assembly="PluginSupport.Print,') 
WHERE MetadataType = 'Printing'

--------------------------------------------------------------------------


