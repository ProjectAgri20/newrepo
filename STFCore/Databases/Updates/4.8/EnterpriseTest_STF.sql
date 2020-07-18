USE [EnterpriseTest]
GO


-----------------------------------------------------------------------------------------
-- bmyers 6/26/2017 Add new plugin settings

-- Insert plugin settings for TopCatSetup and PrintDriverServer
WITH Plugin AS(
    SELECT * FROM
    (
        VALUES ('Discovery', 'TopCatSetup'),
               ('HpacClient', 'PrintDriverServer'),
               ('PrintQueueManagement', 'PrintDriverServer')
    ) AS Plugin (PluginName, SettingName)
)
INSERT INTO SystemSetting
SELECT 'PluginSetting', PluginName, SystemSetting.Name, Value, Description
FROM SystemSetting
INNER JOIN Plugin on Plugin.SettingName = SystemSetting.Name
INNER JOIN MetadataType on Plugin.PluginName = MetadataType.Name
WHERE Type = 'SystemSetting'
GO

DELETE FROM SystemSetting
WHERE SubType = 'FrameworkSetting'
AND Name IN ('TopCatSetup', 'TopCatScripts')
GO




-- Insert plugin settings for domain user name and password
IF NOT EXISTS(SELECT * FROM SystemSetting WHERE Type = 'PluginSetting' AND Name = 'DomainAdminUserName')
BEGIN
    WITH Plugin AS(
        SELECT * FROM
        (
            VALUES ('Discovery', 'DomainAdminUserName'),
                   ('DirtyDevice', 'DomainAdminUserName'),
                   ('DirtyDevice', 'DomainAdminPassword'),
                   ('NetworkEmulation', 'DomainAdminUserName'),
                   ('NetworkEmulation', 'DomainAdminPassword'),
                   ('NetworkNamingServices', 'DomainAdminUserName'),
                   ('NetworkNamingServices', 'DomainAdminPassword'),
                   ('ServiceStartStop', 'DomainAdminUserName'),
                   ('ServiceStartStop', 'DomainAdminPassword'),
                   ('TopCat', 'DomainAdminUserName')
        ) AS Plugin (PluginName, SettingName)
    )
    INSERT INTO SystemSetting
    SELECT 'PluginSetting', PluginName, SystemSetting.Name, Value, Description
    FROM SystemSetting
    INNER JOIN Plugin on Plugin.SettingName = SystemSetting.Name
    INNER JOIN MetadataType on Plugin.PluginName = MetadataType.Name
    WHERE Type = 'SystemSetting'
END
GO




-----------------------------------------------------------------------------------------
-- bmyers 7/7/2017 Delete obsolete ProductFamily records
DELETE FROM ValueGroup WHERE Name = 'ProductFamily'


-----------------------------------------------------------------------------------------
--kyoungman 7/10/2017 Remove retired plugins
DELETE MetadataTypeResourceTypeAssoc WHERE MetadataTypeName IN ('Fms','HpacPullPrintingMphasis','LockSmith','ServerConfiguration','SecuritySuite','ShareScanExample','WebTest')
DELETE MetadataInstallerPackageAssoc WHERE MetadataType IN ('Fms','HpacPullPrintingMphasis','LockSmith','ServerConfiguration','SecuritySuite','ShareScanExample','WebTest')
DELETE MetadataType WHERE Name IN ('Fms','HpacPullPrintingMphasis','LockSmith','ServerConfiguration','SecuritySuite','ShareScanExample','WebTest')
