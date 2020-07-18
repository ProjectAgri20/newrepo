DELETE FROM SystemSetting WHERE [Key] = ''

INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
-- Database locations
('FrameworkSetting', '', 'EnterpriseTestDatabase', '', null),
('FrameworkSetting', '', 'DocumentLibraryDatabase', '', null),
('FrameworkSetting', '', 'AssetInventoryDatabase', '', null),
-- VMWare
('FrameworkSetting', '', 'VmStateTransitionTimeoutInSeconds', '600', null),
('FrameworkSetting', '', 'VmBootupBatchSize', '15', null),
('FrameworkSetting', '', 'VMWareServerUri', '', null),
('FrameworkSetting', '', 'VSphereUserName', '', null),
('FrameworkSetting', '', 'VSpherePassword', '', null),
-- OfficeWorker
('FrameworkSetting', '', 'OfficeWorkerPassword', '', null),
('FrameworkSetting', '', 'EwsAutodiscoverEnabled', '', 'If set to false, EwsUrl must be specified'),
('FrameworkSetting', '', 'EwsUrl', '', null),
-- QTP
('FrameworkSetting', '', 'QtpPackageRepository', '', null),
('FrameworkSetting', '', 'QtpTestResults', '', null),
('FrameworkSetting', '', 'QtpUserName', '', null),
('FrameworkSetting', '', 'QtpPassword', '', null),
-- Endpoint Responder
('FrameworkSetting', '', 'EndpointResponderProfileServer', '', null),
-- LowLayerTester
('FrameworkSetting', '', 'TestAdminHostRepo', '', null),
-- Document Library and reporting
('FrameworkSetting', '', 'ReportTemplateRepository', '', null),
('FrameworkSetting', '', 'DocumentLibraryServer', '', null),
('FrameworkSetting', '', 'DocumentLibraryUserName', '', null),
('FrameworkSetting', '', 'DocumentLibraryPassword', '', null),
('FrameworkSetting', '', 'DocumentLibraryWebsite', '', null),
-- Domain Administrator
('FrameworkSetting', '', 'Domain', '', null),
('FrameworkSetting', '', 'DomainAdminUserName', '', null),
('FrameworkSetting', '', 'DomainAdminPassword', '', null),
-- Print drivers
('FrameworkSetting', '', 'PrintDriverServer', '', null),
('FrameworkSetting', '', 'UniversalPrintDriverBaseLocation', '', null),
-- Other settings
('FrameworkSetting', '', 'Environment', '', null),
('FrameworkSetting', '', 'Organization', '', null),
('FrameworkSetting', '', 'AssetInventoryPools', '', null)


-- WCF Host Locations
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('WcfHostSetting', '', 'CentralLog', '', null),
('WcfHostSetting', '', 'EnterpriseLock', '', null),
('WcfHostSetting', '', 'VmInventory', '', null),
('WcfHostSetting', '', 'TestDocument', '', null),
('WcfHostSetting', '', 'DataTicketBuilder', '', null),
('WcfHostSetting', '', 'AssetInventory', '', null),
('WcfHostSetting', '', 'DataGateway', '$Dispatcher', null),
('WcfHostSetting', '', 'SessionResource', '$Dispatcher', null),
('WcfHostSetting', '', 'DispatcherResourceMonitor', '$Dispatcher', null),
('WcfHostSetting', '', 'VMLock', 'localhost', null)


-- Dispatchers
INSERT INTO SystemSetting (Type, [Key], Name, Value, Description) VALUES
('Dispatcher', '', 'Dispatcher Name', 'DispatcherHostnameOrIP', null)



SELECT * FROM SystemSetting