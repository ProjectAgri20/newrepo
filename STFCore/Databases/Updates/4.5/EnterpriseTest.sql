USE [EnterpriseTest]
GO
BEGIN TRANSACTION
	DELETE MetadataTypeResourceTypeAssoc where MetadataTypeName = 'JediPullPrinting'
	if @@ROWCOUNT = 1
	BEGIN
		
	-- JediPullPrinting plugin has been removed form STF
	DELETE MetadataType where name = 'JediPullPrinting'
	if @@ROWCOUNT > 0
	  Commit TRANSACTION
	ELSE
		ROLLBACK TRANSACTION
	END

