USE [master]
GO
IF EXISTS (SELECT 1 FROM fn_listextendedproperty(N'STF Revision', NULL, NULL, NULL, NULL, NULL, NULL))
EXEC sp_dropextendedproperty N'STF Revision', NULL, NULL, NULL, NULL, NULL, NULL
EXEC sp_addextendedproperty N'STF Revision', '{0}', NULL, NULL, NULL, NULL, NULL, NULL
GO
