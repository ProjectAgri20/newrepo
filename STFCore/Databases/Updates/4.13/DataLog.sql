USE [ScalableTestDataLog]
GO

--bermudew If the DeviceConfigResult Table exists and the Control Changed Column doesn't. Add it.
IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.DeviceConfigResult')
AND NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =  'DeviceConfigResult' AND COLUMN_NAME = 'ControlChanged')) 
	BEGIN
		ALTER TABLE [dbo].[DeviceConfigResult]
		ADD ControlChanged NVARCHAR(50)
	END

-- parham -- Reversed some of the clustered indexes introduced in the update to version 4.12.
------------ BEGIN THE CLUSTERED INDEX CHANGE

SET ANSI_PADDING ON
GO

/***** Drop indexes *****/
PRINT 'Drop non-clustered indexes'
GO
PRINT 'Dropping Idx_ActivityExecutionAssetUsage_ActivityExecutionId ...'
IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionAssetUsage_ActivityExecutionId')
    DROP INDEX Idx_ActivityExecutionAssetUsage_ActivityExecutionId ON ActivityExecutionAssetUsage
GO
PRINT 'Dropping Idx_ActivityExecutionPerformance_EventLabel ...'
IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionPerformance_EventLabel')
    DROP INDEX Idx_ActivityExecutionPerformance_EventLabel ON ActivityExecutionPerformance
GO
PRINT 'Dropping Idx_ActivityExecutionServerUsage_ActivityExecutionId ...'
IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionServerUsage_ActivityExecutionId')
    DROP INDEX Idx_ActivityExecutionServerUsage_ActivityExecutionId ON ActivityExecutionServerUsage
GO
PRINT 'Dropping Idx_DigitalSendJobInput_ActivityExecutionId ...'
IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_DigitalSendJobInput_ActivityExecutionId')
    DROP INDEX Idx_DigitalSendJobInput_ActivityExecutionId ON DigitalSendJobInput
GO
PRINT 'Dropping Idx_PrintJobClient_ActivityExecutionId ...'
IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_PrintJobClient_ActivityExecutionId')
    DROP INDEX Idx_PrintJobClient_ActivityExecutionId ON PrintJobClient
GO

/***** Re-index ActivityExecution *****/
PRINT 'Re-index ActivityExecution'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecution')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecution_SessionId')
        DROP INDEX Idx_ActivityExecution_SessionId ON ActivityExecution

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecution')
    BEGIN
        ALTER TABLE ActivityExecution DROP CONSTRAINT PK_ActivityExecution
        ALTER TABLE ActivityExecution ADD CONSTRAINT PK_ActivityExecution PRIMARY KEY CLUSTERED
            (ActivityExecutionId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionAssetUsage *****/
PRINT 'Re-index ActivityExecutionAssetUsage'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionAssetUsage')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionAssetUsage_SessionId')
        DROP INDEX Idx_ActivityExecutionAssetUsage_SessionId ON ActivityExecutionAssetUsage WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecutionAssetUsage')
    BEGIN
        ALTER TABLE ActivityExecutionAssetUsage DROP CONSTRAINT PK_ActivityExecutionAssetUsage
        ALTER TABLE ActivityExecutionAssetUsage ADD CONSTRAINT PK_ActivityExecutionAssetUsage PRIMARY KEY CLUSTERED
            (ActivityExecutionAssetUsageId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionDetail *****/
PRINT 'Re-index ActivityExecutionDetail'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionDetail')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionDetail_SessionId')
        DROP INDEX Idx_ActivityExecutionDetail_SessionId ON ActivityExecutionDetail WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecutionDetail')
    BEGIN
        ALTER TABLE ActivityExecutionDetail DROP CONSTRAINT PK_ActivityExecutionDetail
        ALTER TABLE ActivityExecutionDetail ADD CONSTRAINT PK_ActivityExecutionDetail PRIMARY KEY CLUSTERED
            (ActivityExecutionDetailId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionDocumentUsage *****/
PRINT 'Re-index ActivityExecutionDocumentUsage'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionDocumentUsage')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionDocumentUsage_SessionId')
        DROP INDEX Idx_ActivityExecutionDocumentUsage_SessionId ON ActivityExecutionDocumentUsage WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecutionDocumentUsage')
    BEGIN
        ALTER TABLE ActivityExecutionDocumentUsage DROP CONSTRAINT PK_ActivityExecutionDocumentUsage
        ALTER TABLE ActivityExecutionDocumentUsage ADD CONSTRAINT PK_ActivityExecutionDocumentUsage PRIMARY KEY CLUSTERED
            (ActivityExecutionDocumentUsageId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionPerformance *****/
PRINT 'Re-index ActivityExecutionPerformance'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionPerformance')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionPerformance_SessionId')
        DROP INDEX Idx_ActivityExecutionPerformance_SessionId ON ActivityExecutionPerformance WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'Pk_ActivityExecutionPerformance')
    BEGIN
        ALTER TABLE ActivityExecutionPerformance DROP CONSTRAINT Pk_ActivityExecutionPerformance
        ALTER TABLE ActivityExecutionPerformance ADD CONSTRAINT Pk_ActivityExecutionPerformance PRIMARY KEY CLUSTERED
            (ActivityExecutionPerformanceId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionRetries *****/
PRINT 'Re-index ActivityExecutionRetries'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionRetries')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionRetries_SessionId')
        DROP INDEX Idx_ActivityExecutionRetries_SessionId ON ActivityExecutionRetries WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecutionRetries')
    BEGIN
        ALTER TABLE ActivityExecutionRetries DROP CONSTRAINT PK_ActivityExecutionRetries
        ALTER TABLE ActivityExecutionRetries ADD CONSTRAINT PK_ActivityExecutionRetries PRIMARY KEY CLUSTERED
            (ActivityExecutionRetriesId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ActivityExecutionServerUsage *****/
PRINT 'Re-index ActivityExecutionServerUsage'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ActivityExecutionServerUsage')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ActivityExecutionServerUsage_SessionId')
        DROP INDEX Idx_ActivityExecutionServerUsage_SessionId ON ActivityExecutionServerUsage WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ActivityExecutionServerUsage')
    BEGIN
        ALTER TABLE ActivityExecutionServerUsage DROP CONSTRAINT PK_ActivityExecutionServerUsage
        ALTER TABLE ActivityExecutionServerUsage ADD CONSTRAINT PK_ActivityExecutionServerUsage PRIMARY KEY CLUSTERED
            (ActivityExecutionServerUsageId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ConnectorJobInput *****/
PRINT 'Re-index ConnectorJobInput'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ConnectorJobInput')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_ConnectorJobInput_SessionId')
        DROP INDEX Idx_ConnectorJobInput_SessionId ON ConnectorJobInput WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ConnectorJobInput')
    BEGIN
        ALTER TABLE ConnectorJobInput DROP CONSTRAINT PK_ConnectorJobInput
        ALTER TABLE ConnectorJobInput ADD CONSTRAINT PK_ConnectorJobInput PRIMARY KEY CLUSTERED
            (ConnectorJobInputId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DeviceEvent *****/
PRINT 'Re-index DeviceEvent'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceEvent')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DeviceEvent_SessionId')
        DROP INDEX Idx_DeviceEvent_SessionId ON DeviceEvent WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DeviceEvent')
    BEGIN
        ALTER TABLE DeviceEvent DROP CONSTRAINT PK_DeviceEvent
        ALTER TABLE DeviceEvent ADD CONSTRAINT PK_DeviceEvent PRIMARY KEY CLUSTERED
            (DeviceEventId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DeviceMemoryCount *****/
PRINT 'Re-index DeviceMemoryCount'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceMemoryCount')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DeviceMemoryCount_DeviceMemorySnapshotId')
        DROP INDEX Idx_DeviceMemoryCount_DeviceMemorySnapshotId ON DeviceMemoryCount WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DeviceMemoryCount')
    BEGIN
        ALTER TABLE DeviceMemoryCount DROP CONSTRAINT PK_DeviceMemoryCount
        ALTER TABLE DeviceMemoryCount ADD CONSTRAINT PK_DeviceMemoryCount PRIMARY KEY CLUSTERED
            (DeviceMemoryCountId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DeviceMemorySnapshot *****/
PRINT 'Re-index DeviceMemorySnapshot'
GO
-- Drop the foreign keys, they will be re-added later
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceMemoryCount') AND
    EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DeviceMemoryCount_DeviceMemorySnapshot')
    ALTER TABLE DeviceMemoryCount DROP CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceMemoryXml') AND
    EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DeviceMemoryXml_DeviceMemorySnapshot')
    ALTER TABLE DeviceMemoryXml DROP CONSTRAINT FK_DeviceMemoryXml_DeviceMemorySnapshot
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceMemorySnapshot')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DeviceMemorySnapshot_SessionId')
        DROP INDEX Idx_DeviceMemorySnapshot_SessionId ON DeviceMemorySnapshot WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DeviceMemorySnapshot')
    BEGIN
        ALTER TABLE DeviceMemorySnapshot DROP CONSTRAINT PK_DeviceMemorySnapshot
        ALTER TABLE DeviceMemorySnapshot ADD CONSTRAINT PK_DeviceMemorySnapshot PRIMARY KEY CLUSTERED
            (DeviceMemorySnapshotId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DeviceMemoryXml *****/
PRINT 'Re-index DeviceMemoryXml'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DeviceMemoryXml')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DeviceMemoryXml_DeviceMemorySnapshotId')
        DROP INDEX Idx_DeviceMemoryXml_DeviceMemorySnapshotId ON DeviceMemoryXml WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DeviceMemoryXML')
    BEGIN
        ALTER TABLE DeviceMemoryXml DROP CONSTRAINT PK_DeviceMemoryXML
        ALTER TABLE DeviceMemoryXml ADD CONSTRAINT PK_DeviceMemoryXML PRIMARY KEY CLUSTERED
            (DeviceMemoryXmlId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendJobInput *****/
PRINT 'Re-index DigitalSendJobInput'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendJobInput')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendJobInput_SessionId')
        DROP INDEX Idx_DigitalSendJobInput_SessionId ON DigitalSendJobInput WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendJobInput')
    BEGIN
        ALTER TABLE DigitalSendJobInput DROP CONSTRAINT PK_DigitalSendJobInput
        ALTER TABLE DigitalSendJobInput ADD CONSTRAINT PK_DigitalSendJobInput PRIMARY KEY CLUSTERED
            (DigitalSendJobInputId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendJobNotification *****/
PRINT 'Re-index DigitalSendJobNotification'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendJobNotification')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendJobNotification_SessionId')
        DROP INDEX Idx_DigitalSendJobNotification_SessionId ON DigitalSendJobNotification WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendJobNotification')
    BEGIN
        ALTER TABLE DigitalSendJobNotification DROP CONSTRAINT PK_DigitalSendJobNotification
        ALTER TABLE DigitalSendJobNotification ADD CONSTRAINT PK_DigitalSendJobNotification PRIMARY KEY CLUSTERED
            (DigitalSendJobNotificationId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendJobOutput *****/
PRINT 'Re-index DigitalSendJobOutput'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendJobOutput')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendJobOutput_SessionId')
        DROP INDEX Idx_DigitalSendJobOutput_SessionId ON DigitalSendJobOutput WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendJobOutput')
    BEGIN
        ALTER TABLE DigitalSendJobOutput DROP CONSTRAINT PK_DigitalSendJobOutput
        ALTER TABLE DigitalSendJobOutput ADD CONSTRAINT PK_DigitalSendJobOutput PRIMARY KEY CLUSTERED
            (DigitalSendJobOutputId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendJobTempFile *****/
PRINT 'Re-index DigitalSendJobTempFile'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendJobTempFile')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendJobTempFile_SessionId')
        DROP INDEX Idx_DigitalSendJobTempFile_SessionId ON DigitalSendJobTempFile WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendJobTempFile')
    BEGIN
        ALTER TABLE DigitalSendJobTempFile DROP CONSTRAINT PK_DigitalSendJobTempFile
        ALTER TABLE DigitalSendJobTempFile ADD CONSTRAINT PK_DigitalSendJobTempFile PRIMARY KEY CLUSTERED
            (DigitalSendJobTempFileId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendServerJob *****/
PRINT 'Re-index DigitalSendServerJob'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendServerJob')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendServerJob_SessionId')
        DROP INDEX Idx_DigitalSendServerJob_SessionId ON DigitalSendServerJob WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendServerJob')
    BEGIN
        ALTER TABLE DigitalSendServerJob DROP CONSTRAINT PK_DigitalSendServerJob
        ALTER TABLE DigitalSendServerJob ADD CONSTRAINT PK_DigitalSendServerJob PRIMARY KEY CLUSTERED
            (DigitalSendServerJobId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DigitalSendTempSnapshot *****/
PRINT 'Re-index DigitalSendTempSnapshot'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DigitalSendTempSnapshot')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_DigitalSendTempSnapshot_SessionId')
        DROP INDEX Idx_DigitalSendTempSnapshot_SessionId ON DigitalSendTempSnapshot WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DigitalSendTempSnapshot')
    BEGIN
        ALTER TABLE DigitalSendTempSnapshot DROP CONSTRAINT PK_DigitalSendTempSnapshot
        ALTER TABLE DigitalSendTempSnapshot ADD CONSTRAINT PK_DigitalSendTempSnapshot PRIMARY KEY CLUSTERED
            (DigitalSendTempSnapshotId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index DirectorySnapshot *****/
PRINT 'Re-index DirectorySnapshot'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'DirectorySnapshot')
BEGIN
    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_DirectorySnapshot')
    BEGIN
        ALTER TABLE DirectorySnapshot DROP CONSTRAINT PK_DirectorySnapshot
        ALTER TABLE DirectorySnapshot ADD CONSTRAINT PK_DirectorySnapshot PRIMARY KEY CLUSTERED
            (DirectorySnapshotId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index EPrintServerJob *****/
PRINT 'Re-index EPrintServerJob'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'EPrintServerJob')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_EPrintServerJob_SessionId')
        DROP INDEX Idx_EPrintServerJob_SessionId ON EPrintServerJob WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_EPrintServerJob')
    BEGIN
        ALTER TABLE EPrintServerJob DROP CONSTRAINT PK_EPrintServerJob
        ALTER TABLE EPrintServerJob ADD CONSTRAINT PK_EPrintServerJob PRIMARY KEY CLUSTERED
            (EPrintServerJobId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index JetAdvantageLinkDeviceMemoryCount *****/
PRINT 'Re-index JetAdvantageLinkDeviceMemoryCount'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'JetAdvantageLinkDeviceMemoryCount')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_JetAdvantageLinkDeviceMemoryCount_SessionId')
        DROP INDEX Idx_JetAdvantageLinkDeviceMemoryCount_SessionId ON JetAdvantageLinkDeviceMemoryCount WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_JetAdvantageLinkDeviceMemoryCount')
    BEGIN
        ALTER TABLE JetAdvantageLinkDeviceMemoryCount DROP CONSTRAINT PK_JetAdvantageLinkDeviceMemoryCount
        ALTER TABLE JetAdvantageLinkDeviceMemoryCount ADD CONSTRAINT PK_JetAdvantageLinkDeviceMemoryCount PRIMARY KEY CLUSTERED
            (JetAdvantageLinkDeviceMemoryCountId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index JetAdvantageLinkDeviceMemorySnapshot *****/
PRINT 'Re-index JetAdvantageLinkDeviceMemorySnapshot'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'JetAdvantageLinkDeviceMemorySnapshot')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_JetAdvantageLinkDeviceMemorySnapshot_SessionId')
        DROP INDEX Idx_JetAdvantageLinkDeviceMemorySnapshot_SessionId ON JetAdvantageLinkDeviceMemorySnapshot WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_JetAdvantageLinkDeviceMemorySnapshot')
    BEGIN
        ALTER TABLE JetAdvantageLinkDeviceMemorySnapshot DROP CONSTRAINT PK_JetAdvantageLinkDeviceMemorySnapshot
        ALTER TABLE JetAdvantageLinkDeviceMemorySnapshot ADD CONSTRAINT PK_JetAdvantageLinkDeviceMemorySnapshot PRIMARY KEY CLUSTERED
            (JetAdvantageLinkDeviceMemorySnapshotId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index JetAdvantagePullPrintJobRetrieval *****/
PRINT 'Re-index JetAdvantagePullPrintJobRetrieval'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'JetAdvantagePullPrintJobRetrieval')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_JetAdvantagePullPrintJobRetrieval_SessionId')
        DROP INDEX Idx_JetAdvantagePullPrintJobRetrieval_SessionId ON JetAdvantagePullPrintJobRetrieval WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_JetAdvantagePullPrintJobRetrieval')
    BEGIN
        ALTER TABLE JetAdvantagePullPrintJobRetrieval DROP CONSTRAINT PK_JetAdvantagePullPrintJobRetrieval
        ALTER TABLE JetAdvantagePullPrintJobRetrieval ADD CONSTRAINT PK_JetAdvantagePullPrintJobRetrieval PRIMARY KEY CLUSTERED
            (JetAdvantagePullPrintJobRetrievalId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index JetAdvantageUpload *****/
PRINT 'Re-index JetAdvantageUpload'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'JetAdvantageUpload')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_JetAdvantageUpload_SessionId')
        DROP INDEX Idx_JetAdvantageUpload_SessionId ON JetAdvantageUpload WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_JetAdvantageUpload')
    BEGIN
        ALTER TABLE JetAdvantageUpload DROP CONSTRAINT PK_JetAdvantageUpload
        ALTER TABLE JetAdvantageUpload ADD CONSTRAINT PK_JetAdvantageUpload PRIMARY KEY CLUSTERED
            (JetAdvantageUploadId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index PhysicalDeviceJob *****/
PRINT 'Re-index PhysicalDeviceJob'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'PhysicalDeviceJob')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_PhysicalDeviceJob_SessionId')
        DROP INDEX Idx_PhysicalDeviceJob_SessionId ON PhysicalDeviceJob WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_PhysicalDeviceJob')
    BEGIN
        ALTER TABLE PhysicalDeviceJob DROP CONSTRAINT PK_PhysicalDeviceJob
        ALTER TABLE PhysicalDeviceJob ADD CONSTRAINT PK_PhysicalDeviceJob PRIMARY KEY CLUSTERED
            (PhysicalDeviceJobId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index PrintJobClient *****/
PRINT 'Re-index PrintJobClient'
GO
-- Drop the foreign keys, they will be re-added later
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'PrintServerJob') AND
    EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_PrintServerJob_PrintJobClient')
    ALTER TABLE PrintServerJob DROP CONSTRAINT FK_PrintServerJob_PrintJobClient
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'VirtualPrinterJob') AND
EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VirtualPrinterJob_PrintJobClient')
    ALTER TABLE VirtualPrinterJob DROP CONSTRAINT FK_VirtualPrinterJob_PrintJobClient
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'PrintJobClient')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_PrintJobClient_SessionId')
        DROP INDEX Idx_PrintJobClient_SessionId ON PrintJobClient WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_PrintJobClient')
    BEGIN
        ALTER TABLE PrintJobClient DROP CONSTRAINT PK_PrintJobClient
        ALTER TABLE PrintJobClient ADD CONSTRAINT PK_PrintJobClient PRIMARY KEY CLUSTERED
            (PrintJobClientId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index PrintServerJob *****/
PRINT 'Re-index PrintServerJob'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'PrintServerJob')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_PrintServerJob_PrintJobClientId')
        DROP INDEX Idx_PrintServerJob_PrintJobClientId ON PrintServerJob WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_PrintServerJob')
    BEGIN
        ALTER TABLE PrintServerJob DROP CONSTRAINT PK_PrintServerJob
        ALTER TABLE PrintServerJob ADD CONSTRAINT PK_PrintServerJob PRIMARY KEY CLUSTERED
            (PrintServerJobId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index ProductDetails *****/
PRINT 'Re-index ProductDetails'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'ProductDetails')
BEGIN
    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_ProductDetails')
    BEGIN
        ALTER TABLE ProductDetails DROP CONSTRAINT PK_ProductDetails
        ALTER TABLE ProductDetails ADD CONSTRAINT PK_ProductDetails PRIMARY KEY CLUSTERED
            (Product ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index PullPrintJobRetrieval *****/
PRINT 'Re-index PullPrintJobRetrieval'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'PullPrintJobRetrieval')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_PullPrintJobRetrieval_SessionId')
        DROP INDEX Idx_PullPrintJobRetrieval_SessionId ON PullPrintJobRetrieval WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_PullPrintJobRetrieval')
    BEGIN
        ALTER TABLE PullPrintJobRetrieval DROP CONSTRAINT PK_PullPrintJobRetrieval
        ALTER TABLE PullPrintJobRetrieval ADD CONSTRAINT PK_PullPrintJobRetrieval PRIMARY KEY CLUSTERED
            (PullPrintJobRetrievalId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionConfiguration *****/
PRINT 'Re-index SessionConfiguration'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionConfiguration')
BEGIN
    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionConfiguration')
    BEGIN
        ALTER TABLE SessionConfiguration DROP CONSTRAINT PK_SessionConfiguration
        ALTER TABLE SessionConfiguration ADD CONSTRAINT PK_SessionConfiguration PRIMARY KEY CLUSTERED
            (SessionId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionDevice *****/
PRINT 'Re-index SessionDevice'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionDevice')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_SessionDevice_SessionId')
        DROP INDEX Idx_SessionDevice_SessionId ON SessionDevice WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionDevice')
    BEGIN
        ALTER TABLE SessionDevice DROP CONSTRAINT PK_SessionDevice
        ALTER TABLE SessionDevice ADD CONSTRAINT PK_SessionDevice PRIMARY KEY CLUSTERED
            (SessionDeviceId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionDocument *****/
PRINT 'Re-index SessionDocument'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionDocument')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_SessionDocument_SessionId')
        DROP INDEX Idx_SessionDocument_SessionId ON SessionDocument WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionDocument')
    BEGIN
        ALTER TABLE SessionDocument DROP CONSTRAINT PK_SessionDocument
        ALTER TABLE SessionDocument ADD CONSTRAINT PK_SessionDocument PRIMARY KEY CLUSTERED
            (SessionDocumentId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionProductAssoc *****/
PRINT 'Re-index SessionProductAssoc'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionProductAssoc')
BEGIN
    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionProductAssoc')
    BEGIN
        ALTER TABLE SessionProductAssoc DROP CONSTRAINT PK_SessionProductAssoc
        ALTER TABLE SessionProductAssoc ADD CONSTRAINT PK_SessionProductAssoc PRIMARY KEY CLUSTERED
            (SessionId ASC, EnterpriseTestAssociatedProductId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionServer *****/
PRINT 'Re-index SessionServer'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionServer')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_SessionServer_SessionId')
        DROP INDEX Idx_SessionServer_SessionId ON SessionServer WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionServer')
    BEGIN
        ALTER TABLE SessionServer DROP CONSTRAINT PK_SessionServer
        ALTER TABLE SessionServer ADD CONSTRAINT PK_SessionServer PRIMARY KEY CLUSTERED
            (SessionServerId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index SessionSummary *****/
PRINT 'Re-index SessionSummary'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'SessionSummary')
BEGIN
    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_SessionSummary')
    BEGIN
        ALTER TABLE SessionSummary DROP CONSTRAINT PK_SessionSummary
        ALTER TABLE SessionSummary ADD CONSTRAINT PK_SessionSummary PRIMARY KEY CLUSTERED
            (SessionId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index TriageData *****/
PRINT 'Re-index TriageData'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'TriageData')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_TriageData_SessionId')
        DROP INDEX Idx_TriageData_SessionId ON TriageData WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_TriageData')
    BEGIN
        ALTER TABLE TriageData DROP CONSTRAINT PK_TriageData
        ALTER TABLE TriageData ADD CONSTRAINT PK_TriageData PRIMARY KEY CLUSTERED
            (TriageDataId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index TriageDataJetAdvantageLink *****/
PRINT 'Re-index TriageDataJetAdvantageLink'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'TriageDataJetAdvantageLink')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_TriageDataJetAdvantageLink_SessionId')
        DROP INDEX Idx_TriageDataJetAdvantageLink_SessionId ON TriageDataJetAdvantageLink WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_TriageDataJetAdvantageLink')
    BEGIN
        ALTER TABLE TriageDataJetAdvantageLink DROP CONSTRAINT PK_TriageDataJetAdvantageLink
        ALTER TABLE TriageDataJetAdvantageLink ADD CONSTRAINT PK_TriageDataJetAdvantageLink PRIMARY KEY CLUSTERED
            (TriageDataJetAdvantageLinkId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Re-index VirtualPrinterJob *****/
PRINT 'Re-index VirtualPrinterJob'
GO
IF EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped = 0 AND name = 'VirtualPrinterJob')
BEGIN
    -- Drop the clustered index if it is not clustered on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'CLUSTERED' AND name = 'Idx_VirtualPrinterJob_PrintJobClientId')
        DROP INDEX Idx_VirtualPrinterJob_PrintJobClientId ON VirtualPrinterJob WITH (ONLINE = OFF)

    -- Create a clustered index on the primary key
    IF EXISTS(SELECT 1 FROM sys.indexes WHERE is_primary_key = 1 AND type_desc = 'NONCLUSTERED' AND name = 'PK_VirtualPrinterJob')
    BEGIN
        ALTER TABLE VirtualPrinterJob DROP CONSTRAINT PK_VirtualPrinterJob
        ALTER TABLE VirtualPrinterJob ADD CONSTRAINT PK_VirtualPrinterJob PRIMARY KEY CLUSTERED
            (VirtualPrinterJobId ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    END
END
GO

/***** Delete orphaned records from DeviceMemoryCount *****/
PRINT 'Delete orphaned records from DeviceMemoryCount'
GO
SET NOCOUNT ON
DECLARE @r INT
SET @r = 1
WHILE @r > 0
BEGIN
    BEGIN TRANSACTION
        DELETE TOP (100000) FROM DeviceMemoryCount WHERE DeviceMemorySnapshotId NOT IN (SELECT DISTINCT DeviceMemorySnapshotId FROM DeviceMemorySnapshot)
        SET @r = @@ROWCOUNT
    COMMIT TRANSACTION
END
GO

/***** Foreign key on DeviceMemoryCount *****/
PRINT 'Add foreign key on DeviceMemoryCount'
GO
IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_DeviceMemoryCount_DeviceMemorySnapshot') AND parent_object_id = OBJECT_ID('dbo.DeviceMemoryCount'))
    ALTER TABLE DeviceMemoryCount  WITH CHECK ADD CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot FOREIGN KEY(DeviceMemorySnapshotId)
    REFERENCES DeviceMemorySnapshot (DeviceMemorySnapshotId)
    ON UPDATE CASCADE
    ON DELETE CASCADE
GO
IF EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_DeviceMemoryCount_DeviceMemorySnapshot') AND parent_object_id = OBJECT_ID('dbo.DeviceMemoryCount'))
    ALTER TABLE DeviceMemoryCount CHECK CONSTRAINT FK_DeviceMemoryCount_DeviceMemorySnapshot
GO

/***** Delete orphaned records from DeviceMemoryXml *****/
PRINT 'Delete orphaned records from DeviceMemoryXml'
GO
SET NOCOUNT ON
DECLARE @r INT
SET @r = 1
WHILE @r > 0
BEGIN
    BEGIN TRANSACTION
        DELETE TOP (100000) FROM DeviceMemoryXml WHERE DeviceMemorySnapshotId NOT IN (SELECT DISTINCT DeviceMemorySnapshotId FROM DeviceMemorySnapshot)
        SET @r = @@ROWCOUNT
    COMMIT TRANSACTION
END
GO

/***** Foreign key on DeviceMemoryXml *****/
PRINT 'Add foreign key on DeviceMemoryXml'
GO
IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_DeviceMemoryXml_DeviceMemorySnapshot') AND parent_object_id = OBJECT_ID('dbo.DeviceMemoryXml'))
    ALTER TABLE DeviceMemoryXml  WITH CHECK ADD CONSTRAINT FK_DeviceMemoryXml_DeviceMemorySnapshot FOREIGN KEY(DeviceMemorySnapshotId)
    REFERENCES DeviceMemorySnapshot (DeviceMemorySnapshotId)
    ON UPDATE CASCADE
    ON DELETE CASCADE
GO
IF EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_DeviceMemoryXml_DeviceMemorySnapshot') AND parent_object_id = OBJECT_ID('dbo.DeviceMemoryXml'))
    ALTER TABLE DeviceMemoryXml CHECK CONSTRAINT FK_DeviceMemoryXml_DeviceMemorySnapshot
GO

/***** Delete orphaned records from PrintServerJob *****/
PRINT 'Delete orphaned records from PrintServerJob'
GO
SET NOCOUNT ON
DECLARE @r INT
SET @r = 1
WHILE @r > 0
BEGIN
    BEGIN TRANSACTION
        DELETE TOP (100000) FROM PrintServerJob WHERE PrintJobClientId NOT IN (SELECT DISTINCT PrintJobClientId FROM PrintJobClient)
        SET @r = @@ROWCOUNT
    COMMIT TRANSACTION
END
GO

/***** Foreign key on PrintServerJob *****/
PRINT 'Add foreign key on PrintServerJob'
GO
IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_PrintServerJob_PrintJobClient') AND parent_object_id = OBJECT_ID('dbo.PrintServerJob'))
    ALTER TABLE PrintServerJob WITH CHECK ADD CONSTRAINT FK_PrintServerJob_PrintJobClient FOREIGN KEY(PrintJobClientId)
    REFERENCES PrintJobClient (PrintJobClientId)
    ON UPDATE CASCADE
    ON DELETE CASCADE
GO
IF EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_PrintServerJob_PrintJobClient') AND parent_object_id = OBJECT_ID('dbo.PrintServerJob'))
    ALTER TABLE PrintServerJob CHECK CONSTRAINT FK_PrintServerJob_PrintJobClient
GO

/***** Delete orphaned records from VirtualPrinterJob *****/
PRINT 'Delete orphaned records from VirtualPrinterJob'
GO
SET NOCOUNT ON
DECLARE @r INT
SET @r = 1
WHILE @r > 0
BEGIN
    BEGIN TRANSACTION
        DELETE TOP (100000) FROM VirtualPrinterJob WHERE PrintJobClientId NOT IN (SELECT DISTINCT PrintJobClientId FROM PrintJobClient)
        SET @r = @@ROWCOUNT
    COMMIT TRANSACTION
END
GO

/***** Foreign key on VirtualPrinterJob *****/
PRINT 'Add foreign key on VirtualPrinterJob'
GO
IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_VirtualPrinterJob_PrintJobClient') AND parent_object_id = OBJECT_ID('dbo.VirtualPrinterJob'))
    ALTER TABLE VirtualPrinterJob WITH CHECK ADD CONSTRAINT FK_VirtualPrinterJob_PrintJobClient FOREIGN KEY(PrintJobClientId)
    REFERENCES PrintJobClient (PrintJobClientId)
    ON UPDATE CASCADE
    ON DELETE CASCADE
GO
IF EXISTS(SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('dbo.FK_VirtualPrinterJob_PrintJobClient') AND parent_object_id = OBJECT_ID('dbo.VirtualPrinterJob'))
    ALTER TABLE VirtualPrinterJob CHECK CONSTRAINT FK_VirtualPrinterJob_PrintJobClient
GO

/***** Create non-clustered index on ActivityExecution *****/
PRINT 'Create non-clustered index on ActivityExecution'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecution_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_ActivityExecution_1 ON ActivityExecution
    (
        ActivityExecutionId ASC,
        SessionId ASC,
        ActivityType ASC,
        StartDateTime ASC,
        Status ASC
    )
    INCLUDE
    (
        ActivityName,
        UserName,
        HostName,
        EndDateTime,
        ResultMessage,
        ResultCategory
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on ActivityExecutionAssetUsage *****/
PRINT 'Create non-clustered index on ActivityExecutionAssetUsage'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionAssetUsage_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_ActivityExecutionAssetUsage_1 ON ActivityExecutionAssetUsage
    (
	    SessionId ASC,
	    ActivityExecutionId ASC,
	    AssetId ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on ActivityExecutionDetail *****/
PRINT 'Create non-clustered index on ActivityExecutionDetail'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionDetail_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_ActivityExecutionDetail_1 ON ActivityExecutionDetail
    (
        ActivityExecutionId ASC,
        SessionId ASC,
        DetailDateTime ASC
    )
    INCLUDE
    (
        Label,
        Message
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on ActivityExecutionPerformance *****/
PRINT 'Create non-clustered index on ActivityExecutionPerformance'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionPerformance_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_ActivityExecutionPerformance_1 ON ActivityExecutionPerformance
    (
	    SessionId ASC,
	    ActivityExecutionId ASC,
	    EventIndex ASC
    )
    INCLUDE
    (
        EventLabel,
	    EventDateTime
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on ActivityExecutionServerUsage *****/
PRINT 'Create non-clustered index on ActivityExecutionServerUsage'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_ActivityExecutionServerUsage_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_ActivityExecutionServerUsage_1 ON ActivityExecutionServerUsage
    (
	    ActivityExecutionId ASC,
	    ServerName ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on DeviceMemoryCount *****/
PRINT 'Create non-clustered index on DeviceMemoryCount'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_DeviceMemoryCount_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_DeviceMemoryCount_1 ON DeviceMemoryCount
    (
	    DeviceMemorySnapshotId ASC,
	    CategoryName ASC,
	    DataLabel ASC,
	    DataValue ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Create non-clustered index on PrintJobClient *****/
PRINT 'Create non-clustered index on PrintJobClient'
GO
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE type_desc = 'NONCLUSTERED' AND name = 'Idx_PrintJobClient_1')
BEGIN
    CREATE NONCLUSTERED INDEX Idx_PrintJobClient_1 ON PrintJobClient
    (
	    ActivityExecutionId ASC,
	    SessionId ASC,
	    JobStartDateTime ASC
    )
    INCLUDE
    (
        PrintJobClientId,
        FileName
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
END
GO

/***** Rebuild or reorganize fragmented indexes *****/
PRINT 'Defragmenting indexes'
DECLARE @fragTemp AS TABLE
(
    id int identity(1, 1),
    [objectId][int] null,
    [indexId][int] null,
    [partitionNum][int] null,
    [frag][float] null
)

DECLARE @count int
DECLARE @i tinyint = 1
DECLARE @schemaName sysname
DECLARE @objectName sysname
DECLARE @indexName sysname
DECLARE @objectId int
DECLARE @indexId int
DECLARE @partitionNum bigint
DECLARE @partitionCount bigint
DECLARE @sqlCmd varchar(max)
DECLARE @fragPercent float

INSERT INTO @fragTemp
SELECT object_id AS objectId
      ,index_id AS indexId
      ,partition_number AS partitionNum
      ,avg_fragmentation_in_percent AS frag
FROM sys.dm_db_index_physical_stats(DB_ID(), null, null, null, 'LIMITED')
WHERE avg_fragmentation_in_percent >= 5.0 and index_id > 0

SELECT @count = COUNT(*) FROM @fragTemp

WHILE(@i <= @count)
BEGIN
    SELECT @objectId = objectId
          ,@indexId = indexId
          ,@partitionNum = partitionNum
          ,@fragPercent = frag
    FROM @fragTemp
    WHERE id = @i

    -- Get table name and its schema
    SELECT @objectName = o.name
          ,@schemaName = c.name
    FROM sys.objects o
    INNER JOIN sys.schemas c ON o.schema_id = c.schema_id
    WHERE o.object_id = @objectId

    -- Get index name
    SELECT @indexName = name
    FROM sys.indexes
    WHERE index_id = @indexId AND object_id = @objectId

    -- Get partition count
    SELECT @partitionCount = COUNT(*)
    FROM sys.partitions
    WHERE object_id = @objectId AND index_id = @indexId

    IF @fragPercent < 30.0
        SELECT @sqlCmd = 'ALTER INDEX ' + @indexName + ' ON ' + @schemaName + '.' + @objectName + ' REORGANIZE'
    if @fragPercent >= 30.0
        SELECT @sqlCmd = 'ALTER INDEX ' + @indexName + ' ON ' + @schemaName + '.' + @objectName + ' REBUILD'
    IF(@partitionCount > 1) SELECT @sqlCmd = @sqlCmd + ' PARTITION = ' + CONVERT(CHAR, @partitionNum)

    EXEC(@sqlCmd)

    PRINT 'Executed: ' + @sqlCmd + ', Fragmentation was ' + CONVERT(VARCHAR, @fragPercent) + '%' + CHAR(13) + CHAR(10)

    -- Increment count
    SET @i = @i + 1
END
GO

------------ END THE CLUSTERED INDEX CHANGE


-- bmyers -- Remove charting stored procedures that are no longer used

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityTypeTotals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityTypeTotals]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityTypeTotalsErrors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityTypeTotalsErrors]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityTypesPerMinute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityTypesPerMinute]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityInstanceTotals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotals]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityInstanceTotalsErrors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityInstanceTotalsErrors]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityInstancesPerMinute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityInstancesPerMinute]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityErrorTotals]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorTotals]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityErrorTotalsDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorTotalsDetails]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_ActivityErrorsPerMinute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_ActivityErrorsPerMinute]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sel_Chart_WorkerActivitySessions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sel_Chart_WorkerActivitySessions]
GO

-- bermudew Creating VirtualResourceInstanceStatus to keep track of worker statuses
IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.VirtualResourceInstanceStatus'))
CREATE TABLE [dbo].[VirtualResourceInstanceStatus](
	[VirtualResourceStatusId] [uniqueidentifier] NOT NULL,
	[SessionId] [varchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Index] [int] NULL,
	[TransitionTo] [nvarchar](50) NULL,
	[TransitionActive] [bit] NULL,
	[Caller] [nvarchar](50) NULL,
	[ResourceInstanceId] [nchar](50) NULL,
 CONSTRAINT [PK_VirtualResourceInstanceStatus] PRIMARY KEY CLUSTERED 
(
	[VirtualResourceStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
