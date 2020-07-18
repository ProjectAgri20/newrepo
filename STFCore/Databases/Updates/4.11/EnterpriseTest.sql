USE [EnterpriseTest]
GO

--==========================================================================
-- D. Anderson insert memory pools system settings
-- Type	SubType	Name	Value	Description
-- SystemSetting	FrameworkSetting	MemoryPools	JediMem_Extensibility,BytesAvailable;JediMem_Extensibility,BytesConsumed;JediMem_Extensibility,HighWater;JediMem_NonJediManaged,BytesAvailable;JediMem_NonJediManaged,BytesConsumed;JediMem_NonJediManaged,HighWater;JediMem_WebkitSmall,BytesAvailable;JediMem_WebkitSmall,BytesConsumed;JediMem_WebkitSmall,HighWater;JediMem_WebkitLarge,BytesAvailable;JediMem_WebkitLarge,BytesConsumed;JediMem_WebkitLarge,HighWater;JediMem_JavaScriptAligned,BytesAvailable;JediMem_JavaScriptAligned,BytesConsumed;JediMem_JavaScriptAligned,HighWater	List of Jedi memory pools, categories and labels to be stored in the device memory count table, i.e.; CategoryName,LabelValue;...,...

if not exists (SELECT Value FROM SystemSetting WHERE Name = 'MemoryPools')
begin
Insert SystemSetting values ('SystemSetting', 'FrameworkSetting', 'MemoryPools', 'JediMem_Extensibility,BytesAvailable;JediMem_Extensibility,BytesConsumed;JediMem_Extensibility,HighWater;JediMem_NonJediManaged,BytesAvailable;JediMem_NonJediManaged,BytesConsumed;JediMem_NonJediManaged,HighWater;JediMem_WebkitSmall,BytesAvailable;JediMem_WebkitSmall,BytesConsumed;JediMem_WebkitSmall,HighWater;JediMem_WebkitLarge,BytesAvailable;JediMem_WebkitLarge,BytesConsumed;JediMem_WebkitLarge,HighWater;JediMem_JavaScriptAligned,BytesAvailable;JediMem_JavaScriptAligned,BytesConsumed;JediMem_JavaScriptAligned,HighWater;Other,BootCycleCounter;Other,ScanCount;JediMem_PdlPersonality,HighWater;ToolHelpAPI,TotalThreadCount;Partition_Extensibility,SpaceUsedInBytes;Partition_CustomerData,SpaceUsedInBytes;GlobalMemoryStatus,AvailablePhysicalBytes;GlobalMemoryStatus,ConsumedPhysicalBytes', 'List of Jedi memory pools, categories and labels to be stored in the device memory count table, i.e.; CategoryName,LabelValue;...,...')
end


-- bmyers  Insert system setting for new lock service
IF NOT EXISTS (SELECT Value FROM SystemSetting WHERE SubType = 'WcfHostSetting' AND Name = 'Lock')
BEGIN
	INSERT INTO SystemSetting (Type, SubType, Name, Value, Description)
		VALUES ('SystemSetting', 'WcfHostSetting', 'Lock', (SELECT Value FROM SystemSetting WHERE SubType = 'WcfHostSetting' AND Name = 'GlobalLock'), null)
END
	
	
-- bermudew  Add ScenarioSettings XML column to track runtime settings like Log location, etc.	
USE [EnterpriseTest]
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.EnterpriseScenario') and name = 'ScenarioSettings')
BEGIN
ALTER TABLE dbo.EnterpriseScenario
ADD ScenarioSettings XML
END
GO

------------------------------------------------------------------------------------------------

--kyougman for bermudew:  Add AssociatedProductVersion table --Will Modified to add proper contraints

IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.AssociatedProductVersion'))
CREATE TABLE [dbo].[AssociatedProductVersion](
	[Name] [varchar](50) NOT NULL,
	[Version] [nvarchar](50) NOT NULL,
	[AssociatedProductId] [uniqueidentifier] NOT NULL,
	[EnterpriseScenarioId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AssociatedProductVersion] PRIMARY KEY CLUSTERED 
(
	[AssociatedProductId] ASC,
	[EnterpriseScenarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_AssociatedProductVersion_AssociatedProduct'))
BEGIN
ALTER TABLE [dbo].[AssociatedProductVersion]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductVersion_AssociatedProduct] FOREIGN KEY([AssociatedProductId])
REFERENCES [dbo].[AssociatedProduct] ([AssociatedProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[AssociatedProductVersion] CHECK CONSTRAINT [FK_AssociatedProductVersion_AssociatedProduct]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_AssociatedProductVersion_EnterpriseScenario'))
BEGIN
ALTER TABLE [dbo].[AssociatedProductVersion]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedProductVersion_EnterpriseScenario] FOREIGN KEY([EnterpriseScenarioId])
REFERENCES [dbo].[EnterpriseScenario] ([EnterpriseScenarioId])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[AssociatedProductVersion] CHECK CONSTRAINT [FK_AssociatedProductVersion_EnterpriseScenario]
END
