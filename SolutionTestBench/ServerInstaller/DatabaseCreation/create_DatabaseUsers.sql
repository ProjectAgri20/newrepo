USE [EnterpriseTest]
GO
CREATE LOGIN [enterprise_admin] WITH PASSWORD=N'enterprise_admin', DEFAULT_DATABASE=[EnterpriseTest], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [enterprise_admin]
GO
ALTER AUTHORIZATION ON DATABASE::EnterpriseTest TO enterprise_admin
GO
ALTER LOGIN [enterprise_admin] WITH default_database = [EnterpriseTest]
GO
ALTER LOGIN [enterprise_admin] WITH PASSWORD=N'enterprise_admin'
GO


use [AssetInventory]
GO
CREATE LOGIN [asset_admin] WITH PASSWORD=N'asset_admin', DEFAULT_DATABASE=[AssetInventory], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [asset_admin]
GO
ALTER AUTHORIZATION ON DATABASE::AssetInventory TO asset_admin
GO
ALTER LOGIN [asset_admin] WITH default_database = [AssetInventory]
GO
ALTER LOGIN [asset_admin] WITH PASSWORD=N'asset_admin'
GO


USE [TestDocumentLibrary]
GO
CREATE LOGIN [document_admin] WITH PASSWORD=N'document_admin', DEFAULT_DATABASE=[TestDocumentLibrary], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [document_admin]
GO
ALTER AUTHORIZATION ON DATABASE::[TestDocumentLibrary] TO [document_admin]
GO
ALTER LOGIN [document_admin] WITH default_database = [TestDocumentLibrary]
GO
ALTER LOGIN [document_admin] WITH PASSWORD=N'document_admin'
GO


use [ScalableTestDataLog]
GO
CREATE LOGIN [enterprise_data] WITH PASSWORD=N'enterprise_data', DEFAULT_DATABASE=[ScalableTestDataLog], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [enterprise_data]
GO
ALTER AUTHORIZATION ON DATABASE::[ScalableTestDataLog] TO [enterprise_data]
GO
ALTER LOGIN [enterprise_data] WITH default_database = [ScalableTestDataLog]
GO
ALTER LOGIN [enterprise_data] WITH PASSWORD=N'enterprise_data'
GO
