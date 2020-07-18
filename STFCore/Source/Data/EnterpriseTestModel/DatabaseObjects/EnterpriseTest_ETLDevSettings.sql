DELETE FROM SystemSetting WHERE [Key] = ''

INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
-- Database locations
('FrameworkSetting', '', 'EnterpriseTestDatabase', '16.70.36.209', null),
('FrameworkSetting', '', 'DocumentLibraryDatabase', 'dtestlibrary.swltl.adapps.hp.com', null),
('FrameworkSetting', '', 'AssetInventoryDatabase', '16.70.36.208', null),
-- VMWare
('FrameworkSetting', '', 'VmStateTransitionTimeoutInSeconds', '600', null),
('FrameworkSetting', '', 'VmBootupBatchSize', '15', null),
('FrameworkSetting', '', 'VMWareServerUri', 'https://10.10.1.16/sdk', null),
('FrameworkSetting', '', 'VSphereUserName', 'enterprise', null),
('FrameworkSetting', '', 'VSpherePassword', '1qaz@WSX3edc', null),
-- OfficeWorker
('FrameworkSetting', '', 'OfficeWorkerPassword', '1qaz2wsX', null),
('FrameworkSetting', '', 'EwsAutodiscoverEnabled', 'true', 'If set to false, EwsUrl must be specified'),
('FrameworkSetting', '', 'EwsUrl', 'https://dexchange01.detl.test/EWS/Exchange.asmx', null),
-- QTP
('FrameworkSetting', '', 'QtpPackageRepository', '\\16.70.36.210\QTP\TestPackages', null),
('FrameworkSetting', '', 'QtpTestResults', '\\CQTPServer\QTP\TestResults', null),
('FrameworkSetting', '', 'QtpUserName', 'Administrator', null),
('FrameworkSetting', '', 'QtpPassword', '!QAZ2wsx', null),
-- Endpoint Responder
('FrameworkSetting', '', 'EndpointResponderProfileServer', '\\DTESTLIBRARY\EndpointResponderProfiles', null),
-- LowLayerTester
('FrameworkSetting', '', 'TestAdminHostRepo', '\\TestAdmin01\FrameworkRepo', null),
-- Document Library and reporting
('FrameworkSetting', '', 'ReportTemplateRepository', '\\dtestlibrary.swltl.adapps.hp.com\Reports', null),
('FrameworkSetting', '', 'DocumentLibraryServer', '\\DTESTLIBRARY\TestLibrary\Documents', null),
('FrameworkSetting', '', 'DocumentLibraryUserName', 'Administrator', null),
('FrameworkSetting', '', 'DocumentLibraryPassword', '!QAZ2wsx', null),
('FrameworkSetting', '', 'DocumentLibraryWebsite', 'http://dev.swltl.adapps.hp.com', null),
-- Domain Administrator
('FrameworkSetting', '', 'Domain', 'DETL.TEST', null),
('FrameworkSetting', '', 'DomainAdminUserName', 'Administrator', null),
('FrameworkSetting', '', 'DomainAdminPassword', '!QAZ2wsx', null),
-- Print drivers
('FrameworkSetting', '', 'PrintDriverServer', '\\dtestlibrary.swltl.adapps.hp.com\PrintDrivers', null),
('FrameworkSetting', '', 'UniversalPrintDriverBaseLocation', '\\boi-x9720.cscr.hp.com\mombi\UnidriverInstalls\evo', null),
-- Other settings
('FrameworkSetting', '', 'Environment', 'Development', null),
('FrameworkSetting', '', 'Organization', 'RDL', null),
('FrameworkSetting', '', 'AssetInventoryServiceVersion', 'AssetInventoryDev', null)
('FrameworkSetting', '', 'AssetInventoryPools', 'ETL,SHARED,STF,VDETL', null)


-- WCF Host Locations
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('WcfHostSetting', '', 'CentralLog', 'DGlobalService', null),
('WcfHostSetting', '', 'EnterpriseLock', 'DGlobalService', null),
('WcfHostSetting', '', 'VmInventory', '16.70.36.209', null),
('WcfHostSetting', '', 'TestDocument', 'DTestLibrary', null),
('WcfHostSetting', '', 'DataTicketBuilder', 'DGlobalService', null),
('WcfHostSetting', '', 'AssetInventory', '16.70.36.208', null),
('WcfHostSetting', '', 'DataGateway', '$Dispatcher', null),
('WcfHostSetting', '', 'SessionResource', '$Dispatcher', null),
('WcfHostSetting', '', 'DispatcherResourceMonitor', '$Dispatcher', null),
('WcfHostSetting', '', 'VMLock', 'localhost', null)


-- Dispatchers
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('Dispatcher', '', 'DDISPATCH01', '16.70.36.196', null),
('Dispatcher', '', 'DDISPATCH02', '16.70.36.197', null),
('Dispatcher', '', 'DDISPATCH03', '16.70.36.198', null),
('Dispatcher', '', 'DDISPATCH04', '16.70.36.199', null),
('Dispatcher', '', 'DDISPATCH05', '16.70.36.200', null),
('Dispatcher', '', 'DDISPATCH06', '16.70.36.201', null),
('Dispatcher', '', 'DDISPATCH07', '16.70.36.202', null),
('Dispatcher', '', 'DDISPATCH08', '16.70.36.203', null),
('Dispatcher', '', 'DDISPATCH09', '16.70.36.204', null),
('Dispatcher', '', 'DDISPATCH10', '16.70.36.229', null),
('Dispatcher', '', 'DDISPATCH11', '16.70.36.230', null)



SELECT * FROM SystemSetting