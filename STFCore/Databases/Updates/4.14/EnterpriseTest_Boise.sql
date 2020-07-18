USE [EnterpriseTest]
GO

-----------------------------------------------------------------------------------------
-- 8/27/2018 kyoungman : Fix Metadata type that was incorrectly entered for CloudConnector plugin.

IF EXISTS (SELECT * FROM MetadataType WHERE [Name] = 'JA Link')
BEGIN
    INSERT MetadataType VALUES ('CloudConnector', 'JA Link', '', 'Plugin.CloudConnector.dll', NULL)
    UPDATE MetadataTypeResourceTypeAssoc SET MetadataTypeName = 'CloudConnector' WHERE MetadataTypeName = 'JA Link'
    DELETE MetadataType WHERE [Name] = 'JA Link'
    UPDATE VirtualResourceMetadata SET MetadataType = 'CloudConnector' WHERE MetadataType = 'JA Link'
END
GO
