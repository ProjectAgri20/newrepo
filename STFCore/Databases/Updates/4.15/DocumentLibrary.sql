USE [TestDocumentLibrary]
GO

-- Renaming file location for .jpg files from 'JPEG' to 'Image'
IF EXISTS (SELECT * FROM TestDocumentExtension WHERE [Location] = 'JPEG')
BEGIN
UPDATE [dbo].[TestDocumentExtension]
SET [Location] = N'Image'
WHERE [Location] = N'JPEG'
END
GO

-- Insert Image types

IF NOT EXISTS (SELECT * FROM TestDocumentExtension WHERE Extension='JPEG')
BEGIN
INSERT INTO [dbo].[TestDocumentExtension]
		([Extension]
		,[Location]
		,[ContentType])
	VALUES
		(N'JPEG'
		,N'Image'
		,N'image/jpeg')
END
GO

IF NOT EXISTS (SELECT * FROM TestDocumentExtension WHERE Extension='PNG')
BEGIN
INSERT INTO [dbo].[TestDocumentExtension]
		([Extension]
		,[Location]
		,[ContentType])
	VALUES
		(N'PNG'
		,N'Image'
		,N'image/png')
END
GO

IF NOT EXISTS (SELECT * FROM TestDocumentExtension WHERE Extension='BMP')
BEGIN
INSERT INTO [dbo].[TestDocumentExtension]
		([Extension]
		,[Location]
		,[ContentType])
	VALUES
		(N'BMP'
		,N'Image'
		,N'image/bmp')
END
GO

IF NOT EXISTS (SELECT * FROM TestDocumentExtension WHERE Extension='TIF')
BEGIN
INSERT INTO [dbo].[TestDocumentExtension]
		([Extension]
		,[Location]
		,[ContentType])
	VALUES
		(N'TIF'
		,N'Image'
		,N'image/tif')
END
