USE [EnterpriseTest]
GO

-----------------------------------------------------------------------------------------
-- kyoungman 1/24/2018 Insert newly created plugins into the MetadataType Table
--           and associate them with the OfficeWorker resource.

IF NOT EXISTS (SELECT * FROM MetadataType WHERE [Name] = 'PaperCut')
BEGIN
    INSERT INTO MetadataType VALUES ('PaperCut', 'PaperCut Pull Printing', '', 'Plugin.PaperCut.dll', NULL)
    INSERT INTO MetadataTypeResourceTypeAssoc VALUES ('PaperCut','OfficeWorker')
END

IF NOT EXISTS (SELECT * FROM MetadataType WHERE [Name] = 'PharosBluePrint')
BEGIN
    INSERT INTO MetadataType VALUES ('Blueprint', 'Blueprint', '', 'Plugin.Blueprint.dll', NULL)
    INSERT INTO MetadataTypeResourceTypeAssoc VALUES ('Blueprint','OfficeWorker')
END
