USE [ScalableTestDataLog]
GO

/****** Object:  Index [Idx_SessionDevice_DeviceId] ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'Idx_SessionDevice_DeviceId' AND object_id = OBJECT_ID('[dbo].[SessionDevice]'))
BEGIN
    CREATE NONCLUSTERED INDEX [Idx_SessionDevice_DeviceId] ON [dbo].[SessionDevice]
    (
        [DeviceId] ASC
    )
    INCLUDE ( [ProductName], [FirmwareRevision] )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO
