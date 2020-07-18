DELETE FROM SystemSetting WHERE [Key] = ''

INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
-- Database locations
('FrameworkSetting', '', 'EnterpriseTestDatabase', '10.10.4.171', null),
('FrameworkSetting', '', 'DocumentLibraryDatabase', '10.10.4.204', null),
('FrameworkSetting', '', 'AssetInventoryDatabase', '10.10.6.107', null),
-- VMWare
('FrameworkSetting', '', 'VmStateTransitionTimeoutInSeconds', '600', null),
('FrameworkSetting', '', 'VmBootupBatchSize', '15', null),
('FrameworkSetting', '', 'VMWareServerUri', 'https://10.10.1.16/sdk', null),
('FrameworkSetting', '', 'VSphereUserName', 'enterprise', null),
('FrameworkSetting', '', 'VSpherePassword', '1qaz@WSX3edc', null),
-- OfficeWorker
('FrameworkSetting', '', 'OfficeWorkerPassword', '1qaz2wsX', null),
('FrameworkSetting', '', 'EwsAutodiscoverEnabled', 'true', 'If set to false, EwsUrl must be specified'),
-- QTP
('FrameworkSetting', '', 'QtpPackageRepository', '\\16.70.36.210\QTP\TestPackages', null),
('FrameworkSetting', '', 'QtpTestResults', '\\CQTPServer\QTP\TestResults', null),
('FrameworkSetting', '', 'QtpUserName', 'Administrator', null),
('FrameworkSetting', '', 'QtpPassword', '!QAZ2wsx', null),
-- Endpoint Responder
('FrameworkSetting', '', 'EndpointResponderProfileServer', '\\CTESTLIBRARY\EndpointResponderProfiles', null),
-- LowLayerTester
('FrameworkSetting', '', 'TestAdminHostRepo', '\\TestAdmin01\FrameworkRepo', null),
-- Document Library and reporting
('FrameworkSetting', '', 'ReportTemplateRepository', '\\CTESTLIBRARY\Reports', null),
('FrameworkSetting', '', 'DocumentLibraryServer', '\\CTESTLIBRARY\TestLibrary\Documents', null),
('FrameworkSetting', '', 'DocumentLibraryUserName', 'Administrator', null),
('FrameworkSetting', '', 'DocumentLibraryPassword', '!QAZ2wsx', null),
('FrameworkSetting', '', 'DocumentLibraryWebsite', 'http://core.swltl.adapps.hp.com', null),
-- Domain Administrator
('FrameworkSetting', '', 'Domain', 'ETL', null),
('FrameworkSetting', '', 'DomainAdminUserName', 'Administrator', null),
('FrameworkSetting', '', 'DomainAdminPassword', '!QAZ2wsx', null),
-- Print drivers
('FrameworkSetting', '', 'PrintDriverServer', '\\dtestlibrary.swltl.adapps.hp.com\PrintDrivers', null),
('FrameworkSetting', '', 'UniversalPrintDriverBaseLocation', '\\boi-x9720.cscr.hp.com\mombi\UnidriverInstalls\evo', null),
-- Other settings
('FrameworkSetting', '', 'Environment', 'Production', null),
('FrameworkSetting', '', 'Organization', 'RDL', null),
('FrameworkSetting', '', 'AssetInventoryPools', 'ETL,SHARED,STF,VCETL', null)


-- WCF Host Locations
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('WcfHostSetting', '', 'CentralLog', 'CGlobalService', null),
('WcfHostSetting', '', 'EnterpriseLock', 'CGlobalService', null),
('WcfHostSetting', '', 'VmInventory', '10.10.4.171', null),
('WcfHostSetting', '', 'TestDocument', 'CTestLibrary', null),
('WcfHostSetting', '', 'DataTicketBuilder', 'CGlobalService', null),
('WcfHostSetting', '', 'AssetInventory', '16.70.36.208', null),
('WcfHostSetting', '', 'DataGateway', '$Dispatcher', null),
('WcfHostSetting', '', 'SessionResource', '$Dispatcher', null),
('WcfHostSetting', '', 'DispatcherResourceMonitor', '$Dispatcher', null),
('WcfHostSetting', '', 'VMLock', 'localhost', null)


-- Dispatchers
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('Dispatcher', '', 'CDISPATCH01', '16.70.36.187', null),
('Dispatcher', '', 'CDISPATCH02', '16.70.36.188', null),
('Dispatcher', '', 'CDISPATCH03', '16.70.36.189', null),
('Dispatcher', '', 'CDISPATCH04', '16.70.36.190', null),
('Dispatcher', '', 'CDISPATCH05', '16.70.36.191', null),
('Dispatcher', '', 'CDISPATCH06', '16.70.36.192', null),
('Dispatcher', '', 'CDISPATCH07', '16.70.36.193', null),
('Dispatcher', '', 'CDISPATCH08', '16.70.36.194', null),
('Dispatcher', '', 'CDISPATCH09', '16.70.36.195', null),
('Dispatcher', '', 'CDISPATCH10', '16.70.36.239', null),
('Dispatcher', '', 'CDISPATCH11', '16.70.36.240', null),
('Dispatcher', '', 'CDISPATCH12', '16.70.36.241', null),
('Dispatcher', '', 'CDISPATCH13', '16.70.36.242', null)



SELECT * FROM SystemSetting ORDER BY Type, [Key], Name