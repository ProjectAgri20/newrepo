USE [EnterpriseTest]
GO

-- bmyers 6/7/16  Updated serialized execution plans to match code changes
WITH XMLNAMESPACES('http://schemas.datacontract.org/2004/07/HP.ScalableTest' as d3p1, DEFAULT 'http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework')
UPDATE VirtualResourceMetadata
SET ExecutionPlan.modify('insert <MaxDelay>{ (/WorkerExecutionPlan/ActivityPacing/Delay/d3p1:_x003C_MaxDelay_x003E_k__BackingField/d3p1:Time/text()) }</MaxDelay> as last into (/WorkerExecutionPlan/ActivityPacing)[1]')
WHERE ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/Delay') = 1 and ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/MaxDelay') = 0
GO

WITH XMLNAMESPACES('http://schemas.datacontract.org/2004/07/HP.ScalableTest' as d3p1, DEFAULT 'http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework')
UPDATE VirtualResourceMetadata
SET ExecutionPlan.modify('insert <MinDelay>{ (/WorkerExecutionPlan/ActivityPacing/Delay/d3p1:_x003C_MinDelay_x003E_k__BackingField/d3p1:Time/text()) }</MinDelay> as last into (/WorkerExecutionPlan/ActivityPacing)[1]')
WHERE ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/Delay') = 1 and ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/MinDelay') = 0
GO

WITH XMLNAMESPACES('http://schemas.datacontract.org/2004/07/HP.ScalableTest' as d3p1, DEFAULT 'http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework')
UPDATE VirtualResourceMetadata
SET ExecutionPlan.modify('insert <Randomize>{ (/WorkerExecutionPlan/ActivityPacing/Delay/d3p1:_x003C_RandomDelay_x003E_k__BackingField/text()) }</Randomize> as last into (/WorkerExecutionPlan/ActivityPacing)[1]')
WHERE ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/Delay') = 1 and ExecutionPlan.exist('/WorkerExecutionPlan/ActivityPacing/Randomize') = 0
GO

INSERT INTO [dbo].[ResourceWindowsCategory]
           ([Name]
           ,[CategoryType])
     VALUES
           ('15.86.233.177','ServerService')
		,('15.86.233.178', 'ServerService')
		,('HP ACJA Push', 'ServerService')
		,('HP AC SPP Enterprise', 'ServerService')
		,('HP AC SPPE Mail', 'ServerService')
		,('HP AC SPPE', 'ServerService')
		,('HP ACJA Agent', 'ServerService')
		,('HP ACJA Process', 'ServerService')
		,('HP ACJA Spool Monitor', 'ServerService')
		,('HP ACJA Trap', 'ServerService')
GO

INSERT INTO [dbo].[ResourceWindowsCategoryParent]
SELECT 
ServiceData.CategoryId as ChildID, ServerData.CategoryId as ParentID
FROM [ResourceWindowsCategory] ServerData
CROSS JOIN
	(SELECT CategoryId, Name
	FROM [ResourceWindowsCategory]
	WHERE CategoryType = 'ServerService'
	AND
	(Name not like '%_.%_.%_.%_')
	
	) ServiceData

WHERE CategoryType = 'ServerService'
AND (
	ServerData.Name like '%_.%_.%_.%_'
	and ServerData.Name not like '%.%.%.%.%'
	and ServerData.Name not like '%[^0-9.]%'
	and ServerData.Name not like '%[0-9][0-9][0-9][0-9]%'
	and ServerData.Name not like '%[3-9][0-9][0-9]%'
	and ServerData.Name not like '%2[6-9][0-9]%'
	and ServerData.Name not like '%25[6-9]%')
GO

-- parham 7/15/16  Added record to hold scenario tag values.
IF NOT EXISTS (SELECT 1 FROM dbo.SystemSetting 
                WHERE [Type] = 'SystemSetting' AND [SubType] = 'FrameworkSetting' AND [Name] = 'ScenarioTags')
BEGIN
    INSERT INTO [dbo].[SystemSetting]
                      ([Type]
                      ,[SubType]
                      ,[Name]
                      ,[Value]
                      ,[Description])
                VALUES
                      (N'SystemSetting'
					  ,N'FrameworkSetting'
                      ,N'ScenarioTags'
                      ,N'Official'
                      ,N'These are the values that can be selected in the Scenario Tags field.')
END
GO


-- bmyers 7/20/16  Add system setting for storing device admin password
IF NOT EXISTS (SELECT 1 FROM SystemSetting WHERE Name='DeviceAdminPassword')
INSERT INTO SystemSetting([Type], [SubType], [Name], [Value], [Description])
     VALUES('SystemSetting', 'FrameworkSetting', 'DeviceAdminPassword', '!QAZ2wsx', 'Admin password used for devices')
GO


-- parham 7/28/2016  Add a new column to the SessionInfo table to hold the comma-delimited list of selected tags.
IF COL_LENGTH('SessionInfo', 'Tags') IS NULL
BEGIN
	ALTER TABLE dbo.SessionInfo ADD Tags VARCHAR(255) NULL;
END
