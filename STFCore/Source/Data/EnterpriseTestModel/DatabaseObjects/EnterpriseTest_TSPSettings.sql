DELETE FROM SystemSetting WHERE [Key] = ''

INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
-- Database locations
('FrameworkSetting', '', 'EnterpriseTestDatabase', '10.0.107.56', null),
('FrameworkSetting', '', 'DocumentLibraryDatabase', '10.0.107.195', null),
('FrameworkSetting', '', 'AssetInventoryDatabase', '10.10.6.107', null),
-- VMWare
('FrameworkSetting', '', 'VmStateTransitionTimeoutInSeconds', '600', null),
('FrameworkSetting', '', 'VmBootupBatchSize', '15', null),
('FrameworkSetting', '', 'VMWareServerUri', 'https://16.88.166.51/sdk', null),
('FrameworkSetting', '', 'VSphereUserName', 'enterprise', null),
('FrameworkSetting', '', 'VSpherePassword', '1qaz@WSX3edc', null),
-- OfficeWorker
('FrameworkSetting', '', 'OfficeWorkerPassword', '1qaz2wsx', null),
('FrameworkSetting', '', 'EwsAutodiscoverEnabled', 'true', 'If set to false, EwsUrl must be specified'),
-- QTP
('FrameworkSetting', '', 'QtpPackageRepository', '\\CQTPSERVER\QTP\TestPackages', null),
('FrameworkSetting', '', 'QtpTestResults', '\\CQTPSERVER\QTP\TestResults', null),
('FrameworkSetting', '', 'QtpUserName', 'Administrator', null),
('FrameworkSetting', '', 'QtpPassword', '!QAZ2wsx', null),
-- Endpoint Responder
--('FrameworkSetting', '', 'EndpointResponderProfileServer', '', null),
-- LowLayerTester
('FrameworkSetting', '', 'TestAdminHostRepo', '\\TestAdmin01\FrameworkRepo', null),
-- Document Library and reporting
--('FrameworkSetting', '', 'ReportTemplateRepository', '', null),
('FrameworkSetting', '', 'DocumentLibraryServer', '\\CTestLibrary01\TestLibrary\Documents', null),
('FrameworkSetting', '', 'DocumentLibraryUserName', 'Administrator', null),
('FrameworkSetting', '', 'DocumentLibraryPassword', '1qaz2wsx', null),
--('FrameworkSetting', '', 'DocumentLibraryWebsite', '', null),
-- Domain Administrator
--('FrameworkSetting', '', 'Domain', '', null),
--('FrameworkSetting', '', 'DomainAdminUserName', '', null),
--('FrameworkSetting', '', 'DomainAdminPassword', '', null),
-- Print drivers
--('FrameworkSetting', '', 'PrintDriverServer', '', null),
('FrameworkSetting', '', 'UniversalPrintDriverBaseLocation', '\\boi-x9720.cscr.hp.com\mombi\UnidriverInstalls\evo', null),
-- Other settings
('FrameworkSetting', '', 'Environment', 'Production', null),
('FrameworkSetting', '', 'Organization', 'TSP', null),
('FrameworkSetting', '', 'AssetInventoryPools', 'TSP', null)


-- WCF Host Locations
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('WcfHostSetting', '', 'CentralLog', 'CGlobalService', null),
('WcfHostSetting', '', 'EnterpriseLock', 'CGlobalService', null),
('WcfHostSetting', '', 'VmInventory', '10.0.107.56', null),
('WcfHostSetting', '', 'TestDocument', 'CTestLibrary', null),
('WcfHostSetting', '', 'DataTicketBuilder', 'CGlobalService', null),
('WcfHostSetting', '', 'AssetInventory', '16.70.36.208', null),
('WcfHostSetting', '', 'DataGateway', '$Dispatcher', null),
('WcfHostSetting', '', 'SessionResource', '$Dispatcher', null),
('WcfHostSetting', '', 'DispatcherResourceMonitor', '$Dispatcher', null),
('WcfHostSetting', '', 'VMLock', 'localhost', null)


-- Dispatchers
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('Dispatcher', '', 'DISP01', '16.88.166.170', null)



SELECT * FROM SystemSetting