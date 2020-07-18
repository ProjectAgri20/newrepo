USE [AssetInventory]
GO

-- kyoungman 3/7/2016  Add RequestSentDate column to track when a request for new license has ocurred.
ALTER TABLE License 
ADD RequestSentDate DATETIME NULL
GO
---------------------------------------------------------------------------
