-- Create Database [EnterpriseTest]
CREATE DATABASE [EnterpriseTest] ON  PRIMARY 
( NAME = N'EnterpriseTest', FILENAME = N'{DATABASE_PATH}\EnterpriseTest.mdf' , SIZE = 50MB , MAXSIZE = 5GB, FILEGROWTH = 1MB )
 LOG ON 
( NAME = N'EnterpriseTest_log', FILENAME = N'{DATABASE_PATH}\EnterpriseTest.ldf' , SIZE = 5MB , MAXSIZE = 1GB , FILEGROWTH = 10% )
GO

ALTER DATABASE [EnterpriseTest] SET AUTO_CLOSE OFF 
GO


-- Create Database [AssetInventory]
CREATE DATABASE [AssetInventory] ON  PRIMARY 
( NAME = N'AssetInventory', FILENAME = N'{DATABASE_PATH}\AssetInventory.mdf' , SIZE = 10MB , MAXSIZE = 1GB , FILEGROWTH = 1MB )
 LOG ON 
( NAME = N'AssetInventory_log', FILENAME = N'{DATABASE_PATH}\AssetInventory.ldf' , SIZE = 1MB , MAXSIZE = 200MB , FILEGROWTH = 10% )
GO

ALTER DATABASE [AssetInventory] SET AUTO_CLOSE OFF 
GO


-- Create Database [TestDocumentLibrary]
CREATE DATABASE [TestDocumentLibrary] ON  PRIMARY 
( NAME = N'TestDocumentLibrary', FILENAME = N'{DATABASE_PATH}\TestDocumentLibrary.mdf' , SIZE = 10MB , MAXSIZE = 1GB, FILEGROWTH = 1MB )
 LOG ON 
( NAME = N'TestDocumentLibrary_log', FILENAME = N'{DATABASE_PATH}\TestDocumentLibrary.ldf' , SIZE = 1MB, MAXSIZE = 200MB , FILEGROWTH = 10% )
GO

ALTER DATABASE [TestDocumentLibrary] SET AUTO_CLOSE OFF 
GO


-- Create Database [ScalableTestDatalog]
CREATE DATABASE [ScalableTestDatalog]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Datalog', FILENAME = N'{DATABASE_PATH}\Datalog.mdf' , SIZE =  100MB, MAXSIZE = 10GB , FILEGROWTH = 5MB )
 LOG ON 
( NAME = N'Datalog_log', FILENAME = N'{DATABASE_PATH}\Datalog.ldf' , SIZE = 10MB , MAXSIZE = 2GB , FILEGROWTH = 10% )
GO

ALTER DATABASE [TestDocumentLibrary] SET AUTO_CLOSE OFF 
GO

