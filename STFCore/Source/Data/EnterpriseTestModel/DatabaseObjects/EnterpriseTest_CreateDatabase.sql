USE [master]
GO
-- This is to allow xp_cmdshell to be run to allow advanced options to be changed.
EXEC sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
EXEC sp_configure 'xp_cmdshell', 1
GO
RECONFIGURE
GO
DECLARE @basePath nvarchar(1000)


-- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
SET @basePath = 'C:\STF\MSSQL'  -- USE THIS TO DEFINE WHERE THE DATABASE FILES WILL RESIDE
-- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


DECLARE @createDir SYSNAME
SET @createDir = 'MKDIR ' + @basePath
EXEC xp_cmdshell @createDir

DECLARE @mdf NVARCHAR(200)
SET @mdf = @basePath + '\EnterpriseTest.mfd'

DECLARE @ldf NVARCHAR(200)
SET @ldf = @basePath + '\EnterpriseTest_log.lfd'

DECLARE @CreateDatabaseCommand NVARCHAR(4000)
SET @CreateDatabaseCommand =
N'CREATE DATABASE [EnterpriseTest] ON PRIMARY '
+ '(NAME = N' + N'''' + N'EnterpriseTest' + N'''' + N', FILENAME = N' + N'''' + @mdf + N'''' + N', SIZE = 10000KB, MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB)'
+ N' LOG ON ' +
+ '(NAME = N' + N'''' + N'EnterpriseTest_log' + N'''' + N', FILENAME = N' + N'''' + @ldf + N'''' + N' , SIZE = 1000KB , MAXSIZE = 5242880KB , FILEGROWTH = 10%)'
EXEC(@CreateDatabaseCommand)
GO
--CREATE DATABASE [EnterpriseTest] ON  PRIMARY 
--( NAME = N'EnterpriseTest', FILENAME = N'C:\STF\MSSQL\EnterpriseTest.mdf' , SIZE = 10000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
-- LOG ON 
--( NAME = N'EnterpriseTest_log', FILENAME = N'C:\STF\MSSQL\EnterpriseTest_1.ldf' , SIZE = 1000KB , MAXSIZE = 5242880KB , FILEGROWTH = 10%)
--GO

ALTER DATABASE [EnterpriseTest] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EnterpriseTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EnterpriseTest] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [EnterpriseTest] SET ANSI_NULLS OFF
GO
ALTER DATABASE [EnterpriseTest] SET ANSI_PADDING OFF
GO
ALTER DATABASE [EnterpriseTest] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [EnterpriseTest] SET ARITHABORT OFF
GO
ALTER DATABASE [EnterpriseTest] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [EnterpriseTest] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [EnterpriseTest] SET AUTO_SHRINK ON
GO
ALTER DATABASE [EnterpriseTest] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [EnterpriseTest] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [EnterpriseTest] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [EnterpriseTest] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [EnterpriseTest] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [EnterpriseTest] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [EnterpriseTest] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [EnterpriseTest] SET  DISABLE_BROKER
GO
ALTER DATABASE [EnterpriseTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [EnterpriseTest] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [EnterpriseTest] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [EnterpriseTest] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [EnterpriseTest] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [EnterpriseTest] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [EnterpriseTest] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [EnterpriseTest] SET  READ_WRITE
GO
ALTER DATABASE [EnterpriseTest] SET RECOVERY SIMPLE
GO
ALTER DATABASE [EnterpriseTest] SET  MULTI_USER
GO
ALTER DATABASE [EnterpriseTest] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [EnterpriseTest] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'EnterpriseTest', N'ON'
GO

/****** Create both User accounts used by STF for database access. ******/
USE [master]
GO
CREATE LOGIN [enterprise_admin] WITH PASSWORD = 'enterprise_admin', CHECK_POLICY = OFF, DEFAULT_DATABASE = [EnterpriseTest]
GO
EXEC sp_addsrvrolemember 'enterprise_admin', 'sysadmin'
GO
CREATE LOGIN [enterprise_report] WITH PASSWORD = 'enterprise_report', CHECK_POLICY = OFF, DEFAULT_DATABASE = [EnterpriseTest]
GO
EXEC sp_dropsrvrolemember 'enterprise_report', 'sysadmin'
GO

USE [EnterpriseTest]
GO
CREATE USER [enterprise_admin] FOR LOGIN [enterprise_admin] WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember 'db_owner', 'enterprise_admin'
GO
CREATE USER [enterprise_report] FOR LOGIN [enterprise_report] WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember 'db_datareader', 'enterprise_report'
GO


