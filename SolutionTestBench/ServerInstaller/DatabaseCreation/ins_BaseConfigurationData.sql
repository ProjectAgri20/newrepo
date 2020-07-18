
USE [EnterpriseTest]
GO
INSERT [dbo].[UserGroup] ([GroupName], [Creator], [Description]) VALUES (N'Everyone', N'{ADMIN_USER}', N'This represents all users in the system')
GO
INSERT [dbo].[User] ([UserName], [Domain], [RoleName], [VCenterPassword]) VALUES (N'{ADMIN_USER}', N'{ADMIN_DOMAIN}', N'Administrator', N'NONE')
GO
INSERT [dbo].[ConfigurationTreeFolder] ([ConfigurationTreeFolderId], [Name], [FolderType], [ParentId]) VALUES (N'66f94649-d020-4067-b5fa-6bdd5374a293', N'Test Scenarios', N'ScenarioFolder', NULL)
GO
INSERT [dbo].[ResourceType] ([Name], [MaxResourcesPerHost], [Platform], [PluginEnabled]) VALUES (N'SolutionTester', 10, N'Windows', 1)
GO
-- This might work, but will need documentation.
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AdminEmailAddress', N'{ADMIN_EMAIL}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AdminEmailServer', N'<smtp mail server>', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AllowRootScenarioCreation', N'true', N'Either "true" or "false"')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'WcfHostSetting', N'AssetInventory', N'{SERVER_ADDRESS}', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AssetInventoryDatabase', N'{SERVER_ADDRESS}', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AssetInventoryPools', N'DEFAULT', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'AssetInventoryServiceVersion', N'AssetInventory', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'WcfHostSetting', N'DataLog', N'{SERVER_ADDRESS}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DataLogDatabase', N'{SERVER_ADDRESS}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DataLogFailedWriteRetryTimeout', N'60', N'Defines how long in minutes the DataLog service should try to write a DataTable record that is failing.  After this period of time it will drop the record.')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DataLogMaxCacheEntries', N'200000', N'Defines the maximum number of cached records that will be retained on the server if the DataLog service goes down.')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DefaultLogRetention', N'30', N'Number of days. 0, 1, 7, 30, 90, 180, 270, 365')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DispatcherLogCopyLocation', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\SessionLogs', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DnsDomain', N'{DNS_DOMAIN}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DocumentLibraryDatabase', N'{SERVER_ADDRESS}', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DocumentLibraryPassword', N'', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DocumentLibraryServer', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\TestLibrary\Documents', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DocumentLibraryUserName', N'', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'Domain', N'{AD_DOMAIN}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DomainAdminPassword', N'!QAZ2wsx', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'DomainAdminUserName', N'Jawa', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'EnterpriseTestDatabase', N'{SERVER_ADDRESS}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'Environment', N'Production', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'ExternalSoftware', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\ExternalSoftware', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'EwsAutodiscoverEnabled', N'true', N'If set to false, EwsUrl must be specified')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'WcfHostSetting', N'GlobalLock', N'{SERVER_ADDRESS}', N'Defines the location of the global lock service')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'WcfHostSetting', N'LocalLock', N'{SERVER_ADDRESS}', N'Defines the location of the local lock service')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'LocalQueueInstallHoldLockTimeoutMinutes', N'20', N'Defines how long the lock will be held locally to install a queue.')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'LocalQueueInstallWaitLockTimeoutMinutes', N'20', N'Defines how long to wait for a local lock to begin installation of a local queue.')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'LoggingServicePublishAddress', N'{SERVER_ADDRESS}', N'Logging server address for publishing events')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'LoggingServiceSubscribeAddress', N'{SERVER_ADDRESS}', N'Logging server address for subscribing to events')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'MaxSessionsPerDispatcher', N'10', N'Defines how many sessions can be supported on a single dispatcher instance.  If this is not set, then the internal default is 1.')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'OfficeWorkerPassword', N'1qaz2wsx', NULL)
GO
-- Add reference to this in STB admin documentation
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'Organization', N'{ORGANIZATION}', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'PluginFileNamePattern', N'Plugin.{0}.dll', N'Defines the file name pattern used for all plugin assemblies')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'PluginRelativeLocation', N'..\Plugin', N'Defines the path relative to the executing assembly for all plugin assemblies')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'PrintDriverConfigFileLocation', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\CFM', N'Defines the location for CFM files')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'PrintDriverServer', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\PrintDrivers', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'ReportTemplateRepository', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\Reports', NULL)
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'RetentionBitFlag', N'255', N'For testing 3 months log retention')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'ReportingDatabase', N'ScalableTestDatalog', N'Defines the instance name of the reporting database')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'ReportingDatabaseServer', N'{SERVER_ADDRESS}', N'Defines the location of the reporting database')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'WcfHostSetting', N'TestDocument', N'{SERVER_ADDRESS}', N'Shared with Production')
GO
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'UniversalPrintDriverBaseLocation', N'\\{SERVER_ADDRESS}\{FILE_SHARE}\PrintDrivers', NULL)
GO
-- IS THIS NEEDED FOR STB???? YES - there are updates to the console.
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'TraceLogDatabase', N'{SERVER_ADDRESS}', N'Defines the locaiton of the centralized logging database')
GO
-- even though there are no VMs starting in STB.
INSERT [dbo].[SystemSetting] ([Type], [Name], [Value], [Description]) VALUES (N'FrameworkSetting', N'VmStateTransitionTimeoutInSeconds', N'3600', NULL)
GO


use [AssetInventory]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[User] ([UserId], [Role]) VALUES (N'{ADMIN_EMAIL}', N'Administrator')
GO
INSERT [dbo].[AssetPool] ([Name], [Administrator], [Group], [TrackReservations]) VALUES (N'DEFAULT', N'{ADMIN_EMAIL}', N'DEFAULT', 1)
GO
INSERT [dbo].[PeripheralType] ([Name]) VALUES (N'Other')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'79b7ba37-00dc-4faf-8b91-471a14c3f850', N'Microsoft(R) Windows(R) Server 2003 Enterprise x64 Edition', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'97a73fd2-48a5-4180-b769-5249801dff0f', N'Microsoft Windows Server 2012 R2 Standard', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'fdaf57d1-50d9-4893-94e3-6ad555b092ff', N'Microsoft Windows Server 2008 R2 Enterprise ', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'f91b5a55-5284-40ed-8267-8c6ce291dfc0', N'Microsoft(R) Windows(R) Server 2003, Enterprise Edition', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'2419432d-4ae7-4f68-8bd2-bfba13884eda', N'Microsoft® Windows Server® 2008 Enterprise ', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'5e5fdfdf-66c1-40a5-965b-cafedf6c4e85', N'Microsoft Windows 7 Enterprise ', N'Windows')
GO
INSERT [dbo].[MachineOperatingSystem] ([OperatingSystemId], [Name], [Platform]) VALUES (N'8a3c7704-0e24-46e9-be77-421916325412', N'Microsoft Windows Server 2012 Standard', N'Windows')
GO
INSERT [dbo].[FrameworkServerType] ([TypeId], [Name], [Description]) VALUES (N'fc2b3cba-65ce-47f1-8e58-416fcced6649', N'FileServer', N'Server used by DSS and other file based systems')
GO
INSERT [dbo].[FrameworkServerType] ([TypeId], [Name], [Description]) VALUES (N'011fbd3f-f26c-4541-a492-46ae8804c5c6', N'Solution', N'Solution applications server (3rd party)')
GO
INSERT [dbo].[FrameworkServerType] ([TypeId], [Name], [Description]) VALUES (N'ab0cef65-1b01-4ca2-afe0-89498193b569', N'Production', N'Production system, used for production level testing')
GO
INSERT [dbo].[FrameworkServerType] ([TypeId], [Name], [Description]) VALUES (N'743c1d0c-9ac0-458f-88e8-fc43a91ab510', N'Print', N'General print server used in UPD testing, etc.')
GO
INSERT [dbo].[DomainAccountPool] ([DomainAccountKey], [UserNameFormat], [MinimumUserNumber], [MaximumUserNumber], [Description]) VALUES (N'User', N'u{0:00000}', 1, 10, N'Default User Pool (u00001 - u00010)')
GO

SET ANSI_PADDING ON
GO

use [TestDocumentLibrary]
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'DefectId.Database', N'IPGWSSB/SWPROCESS')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Application', N'SELECT DISTINCT [Application] FROM TestDocument WHERE [Application] IS NOT NULL AND [Application] != ''''')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Author', N'SELECT DISTINCT Author FROM TestDocument WHERE Author IS NOT NULL AND Author != ''''')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.AuthorType', N'Customer')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.AuthorType', N'Internal')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.ColorMode', N'Color')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.ColorMode', N'Mono')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.DefectId', N'SELECT DISTINCT DefectId FROM TestDocument WHERE Tag IS NOT NULL AND Tag != ''''')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Extension', N'SELECT Extension FROM TestDocumentExtension ORDER BY Extension')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Graphics')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Graphics and Images')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Images')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Text')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Text and Graphics')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Text and Graphics and Images')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.FileType', N'Text and Images')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Orientation', N'Landscape')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Orientation', N'Mixed')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Orientation', N'Portrait')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Submitter', N'SELECT DISTINCT Submitter FROM TestDocument WHERE Submitter IS NOT NULL AND Submitter != ''''')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Tag', N'SELECT DISTINCT Tag FROM TestDocument WHERE Tag IS NOT NULL AND Tag != ''''')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Vertical', N'Financial')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Vertical', N'Government')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Vertical', N'Healthcare')
GO
INSERT [dbo].[ItemQueryDomain] ([Name], [Value]) VALUES (N'TestDocument.Vertical', N'Manufacturing')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'DOC', N'Word', N'application/ms-word')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'DOCX', N'Word', N'application/ms-word')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'JPG', N'Image', N'image/jpeg')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'JPEG', N'Image', N'image/jpeg')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'PNG', N'Image', N'image/png')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'BMP', N'Image', N'image/bmp')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'TIF', N'Image', N'image/tif')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'PDF', N'PDF', N'application/pdf')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'PPT', N'PowerPoint', N'application/mspowerpoint')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'PPTX', N'PowerPoint', N'application/mspowerpoint')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'RTF', N'RTF', N'text/rtf')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'TXT', N'Text', N'text/plain')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'XLS', N'Excel', N'application/vnd.ms-excel')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'XLSM', N'Excel', N'application/vnd.ms-excel')
GO
INSERT [dbo].[TestDocumentExtension] ([Extension], [Location], [ContentType]) VALUES (N'XLSX', N'Excel', N'application/vnd.ms-excel')
GO
INSERT [dbo].[TestDocument] ([DocumentId], [Extension], [FileName], [FileType], [FileSize], [Pages], [ColorMode], [Orientation], [Author], [Vertical], [Notes], [Submitter], [SubmitDate], [AuthorType], [Application], [AppVersion], [DefectId], [Tag]) VALUES (N'0910f4c3-a532-4611-b401-a1ead91f1734', N'XLSX', N'SimpleExcel.xlsx', N'Text', 14894, 1, N'Mono', N'Portrait', N'HP', NULL, NULL, N'hpacct', CAST(N'2015-11-11 14:43:03.670' AS DateTime), NULL, N'Microsoft Excel', NULL, NULL, NULL)
GO
INSERT [dbo].[TestDocument] ([DocumentId], [Extension], [FileName], [FileType], [FileSize], [Pages], [ColorMode], [Orientation], [Author], [Vertical], [Notes], [Submitter], [SubmitDate], [AuthorType], [Application], [AppVersion], [DefectId], [Tag]) VALUES (N'13554e80-2ac6-4770-8085-e032b9384bfa', N'PPTX', N'SimplePowerPoint.pptx', N'Text', 30912, 1, N'Mono', N'Landscape', N'HP', NULL, NULL, N'hpacct', CAST(N'2015-11-11 14:43:24.423' AS DateTime), NULL, N'Microsoft Office PowerPoint', NULL, NULL, NULL)
GO
INSERT [dbo].[TestDocument] ([DocumentId], [Extension], [FileName], [FileType], [FileSize], [Pages], [ColorMode], [Orientation], [Author], [Vertical], [Notes], [Submitter], [SubmitDate], [AuthorType], [Application], [AppVersion], [DefectId], [Tag]) VALUES (N'1f28f90a-3d17-46f8-ae5d-21c9bbf5b716', N'TXT', N'SimpleText.txt', N'Text', 30, 1, N'Mono', N'Portrait', NULL, NULL, NULL, N'hpacct', CAST(N'2015-11-11 14:44:18.143' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TestDocument] ([DocumentId], [Extension], [FileName], [FileType], [FileSize], [Pages], [ColorMode], [Orientation], [Author], [Vertical], [Notes], [Submitter], [SubmitDate], [AuthorType], [Application], [AppVersion], [DefectId], [Tag]) VALUES (N'6d02270c-5bbd-4a87-8605-659f3b9d0da8', N'DOCX', N'SimpleWord.docx', N'Text', 11568, 1, N'Mono', N'Portrait', N'HP', NULL, NULL, N'hpacct', CAST(N'2015-11-11 14:44:32.497' AS DateTime), NULL, N'Microsoft Office Word', NULL, NULL, NULL)
GO
