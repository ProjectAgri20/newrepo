USE [AssetInventory]
GO

---------------------------------------------------------------------------
-- bmyers 10/17/18 Resolve issue with incorrect data saved to Printer.Address2

UPDATE Printer SET Address2 = NULL WHERE Address2 = '...'


