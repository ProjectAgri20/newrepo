USE [EnterpriseTest]
GO

---------------------------------------------------------------------------------------------------------
-- bmyers -- Drop EprInjector-related tables/data

DELETE FROM VirtualResourceMetadata WHERE ResourceType = 'EprInjector'
DELETE FROM VirtualResource WHERE ResourceType = 'EprInjector'
DELETE FROM MetadataTypeResourceTypeAssoc WHERE ResourceTypeName = 'EprInjector'
DELETE FROM ResourceType WHERE Name ='EprInjector'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EprAlert]') AND type in (N'U'))
DROP TABLE [dbo].[EprAlert]
GO

---------------------------------------------------------------------------------------------------------
-- 7/23/2018 kyoungman : Drop SequentialWorker tables and data from the database.

-- There shouldn't be any SequentialWorker data in either STE or STB databases, but including these just in case there is in DEV.
DELETE FROM VirtualResourceMetadata WHERE ResourceType = 'SequentialWorker'
DELETE FROM VirtualResource WHERE ResourceType = 'SequentialWorker'
DELETE FROM MetadataTypeResourceTypeAssoc WHERE ResourceTypeName = 'SequentialWorker'
DELETE FROM ResourceType WHERE Name ='SequentialWorker'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SequentialWorker]') AND type in (N'U'))
DROP TABLE [dbo].[SequentialWorker]
GO
