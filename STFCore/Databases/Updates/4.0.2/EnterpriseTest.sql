USE [EnterpriseTest]
GO

INSERT INTO [dbo].[MetadataType] ([Name], [Title], [Group])
     VALUES (N'EquitracPullPrinting', N'Equitrac Pull Printing', N'')
GO


USE [EnterpriseTest]
GO

INSERT INTO [dbo].[MetadataTypeResourceTypeAssoc] ([MetadataTypeName], [ResourceTypeName])
     VALUES (N'EquitracPullPrinting', N'CitrixWorker')
GO

USE [EnterpriseTest]
GO

INSERT INTO [dbo].[MetadataTypeResourceTypeAssoc] ([MetadataTypeName], [ResourceTypeName])
     VALUES (N'EquitracPullPrinting', N'OfficeWorker')
GO
