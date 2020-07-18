USE [TestDocumentLibrary]
GO

IF NOT EXISTS (SELECT * FROM TestDocumentExtension WHERE Extension = 'PRN')
BEGIN
INSERT INTO [dbo].[TestDocumentExtension]
           ([Extension]
           ,[Location]
           ,[ContentType])
     VALUES
           ('PRN'
           ,'Raw'
           ,'application/raw')
END