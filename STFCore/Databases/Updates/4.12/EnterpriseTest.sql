USE [EnterpriseTest]
GO

/*===========================================================================*/
--2/8/2018 kyoungman Retire the LANFax Plugin and convert the existing metadata to Fax plugin data
UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'LanFaxData', 'FaxActivityData') WHERE MetadataType = 'LanFax'
UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), '.LanFax', '.Fax') WHERE MetadataType = 'LanFax'
UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'PageCount>0<', 'PageCount>1<') WHERE MetadataType IN ('Fax','ScanToEmail','ScanToFolder','ScanToJobStorage','ScanToUsb')
UPDATE VirtualResourceMetadata SET Metadata = REPLACE(CAST(Metadata AS VARCHAR(MAX)), 'includeThumbNail', 'IncludeThumbNail') WHERE MetadataType IN ('Fax','ScanToEmail','ScanToFolder','ScanToJobStorage','ScanToUsb')
UPDATE VirtualResourceMetadata SET MetadataType = 'Fax', MetadataVersion = '0.5' WHERE MetadataType = 'LanFax'
UPDATE VirtualResourceMetadata SET MetadataVersion = '1.1' WHERE MetadataType IN ('Fax','ScanToEmail','ScanToFolder','ScanToJobStorage','ScanToUsb') AND CAST(Metadata AS VARCHAR(MAX)) LIKE '%ScanOptions%'

--Remove LanFax from MetadataType table and Add Fax, if there are any Fax records after running the above script
IF EXISTS (SELECT VirtualResourceMetadataId FROM VirtualResourceMetadata WHERE MetadataType = 'Fax') AND NOT EXISTS (SELECT [Name] FROM MetadataType WHERE [Name] = 'Fax')
BEGIN
    INSERT MetadataType ([Name], Title, [Group], AssemblyName, Icon) VALUES ('Fax', 'Fax', 'Digital Send', 'Plugin.Fax.dll', 0x89504E470D0A1A0A0000000D49484452000000100000001008060000001FF3FF61000000017352474200AECE1CE90000000467414D410000B18F0BFC61050000000970485973000012740000127401DE661F780000017649444154384FCD92BFCB416114C79FFF050BE6BB2AFE0393D5208B24315CF9D9AD5B37B91B2EEE6630980DA214B230A128836C0C523645DFB773FCA8B7D77D33BDBDA73ECB793EE73CA7E739E2FF85699A201A8D0653AFD751ABD550AD5651A954F0D07EC6ED76C3E974E2E2D56A85D168F40DCA5183E3F108721F65F7D8ED7628168B300CC3F266A25C2E2397CB211C0E63B3D9DC9B50479267B319B6DBED47D04489440287C301A2D96C623E9F63BFDF2393C97C04B9E3F118E9741A4255555CAF57663A9DE27C3EFFCA643279F9F1781C229BCDE272B930ED769B47A446EFA0339AF8E907834188542AC52F4CD083ADD76B0C0683B790A3EBFACBF7783C10C96412BD5E8F29954A582E97E876BB6F592C165014E5E53B9D4E88582C864EA7C3D037150A0546966544A3511ED3EFF7C3E7F3C1EBF572EEE93B1C0E8848248256ABC5092AD434CD927C3E0FB7DBCD2EED8ACD6683E8F7FBBC448140C0F2664992E072B9B880B0DBED088542180E87D6ABFD4721C4175F750D0792A1950E0000000049454E44AE426082)
END
UPDATE MetadataType SET Icon = (SELECT Icon FROM MetadataType WHERE [Name] = 'LanFax') WHERE [Name] = 'Fax'
DELETE FROM MetadataTypeResourceTypeAssoc WHERE MetadataTypeName = 'LanFax'
DELETE FROM MetadataType WHERE [Name] = 'LanFax'

-------------------------------------------------------------------------------


--3/9/2018 bermudew Add New System setting for Log Retention Maximum Displayed.
IF NOT EXISTS (SELECT Value FROM SystemSetting WHERE SubType = 'FrameworkSetting' AND Name = 'MaxLogDefault')
BEGIN
	INSERT INTO SystemSetting (Type, SubType, Name, Value, Description)
		VALUES ('SystemSetting', 'FrameworkSetting', 'MaxLogDefault', '365', 'Max cut off in days for selecting log retention')
END

--3/9/2018 bermudew Removes RetentionBitFlag field that is being deprecated as it's no longer used in STF/B code.
IF EXISTS (SELECT Value FROM SystemSetting WHERE SubType = 'FrameworkSetting' AND Name = 'RetentionBitFlag')
BEGIN
	DELETE FROM SystemSetting 
		WHERE SystemSetting.Type = 'SystemSetting' AND  SubType = 'FrameworkSetting' AND Name = 'RetentionBitFlag' 
END
