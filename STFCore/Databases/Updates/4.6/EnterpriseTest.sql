USE [EnterpriseTest]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

-- 2017 Mar 09 Max Tuinstra
-- Improve the way solutions and other "products" are associated with scenarios/sessions

  -- Drop constraint in preparation for dropping table.
  IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_SessionProductAssoc_SessionInfo') AND parent_object_id = OBJECT_ID(N'dbo.SessionProductAssoc'))
  BEGIN
    ALTER TABLE [dbo].[SessionProductAssoc] DROP CONSTRAINT [FK_SessionProductAssoc_SessionInfo]
  END
  GO

  -- Drop constraint in preparation for dropping table.
  IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.FK_SessionProductAssoc_AssociatedProduct') AND parent_object_id = OBJECT_ID(N'dbo.SessionProductAssoc'))
  BEGIN
    ALTER TABLE [dbo].[SessionProductAssoc] DROP CONSTRAINT [FK_SessionProductAssoc_AssociatedProduct]
  END
  GO

  -- Drop table that does not belong in EnterpriseTest.  It belongs in DataLog and will be created there.
  IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.SessionProductAssoc'))
  BEGIN
    DROP TABLE dbo.SessionProductAssoc
  END
  GO

  -- Drop table that is not much used and necessitates manual editing by scenario designers and more UI code by developers
  -- We don't want to manually edit this table.  Same info can be derived from plugins.
  IF EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID('dbo.ScenarioProductAssoc'))
  BEGIN
    DROP TABLE dbo.ScenarioProductAssoc 
  END
  GO
  
  -- Capture vendor name.  Among other reasons solution companies are bought and sold.  (e.g. Nuance bought Equitrac & SafeCom)
  IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProduct') and name = 'Vendor')
  BEGIN
    ALTER TABLE dbo.AssociatedProduct ADD
      Vendor    nvarchar(100) NULL
  END
  GO

  -- Regex to match the product name whereever it may be
  IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProduct') and name = 'MatchCriteria')
  BEGIN
    ALTER TABLE dbo.AssociatedProduct ADD
      MatchCriteria     nvarchar(4000) NULL
  END
  GO

  -- Whether a solution is active will be auto-detected now.  If the code detects a solution matching the regular expression it will be automatically added.
  IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AssociatedProduct') and name = 'Active')
  BEGIN
    ALTER TABLE dbo.AssociatedProduct DROP COLUMN
      Active
  END
  GO

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'HP' AND Name = 'DSS')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'HP',     'DSS',        'new Regex(@"(DSS|Digital Send Service|LanFax|ScanToEmail|ScanToFolder|ScanToWorkflow)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'HP' AND Name = 'HPAC')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'HP',     'HPAC',       'new Regex(@"(HP\s*AC|HP Access Control)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'HP' AND Name = 'HPCR')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'HP',     'HPCR',       'new Regex(@"(HP\s*CR|HP Capture & Route|HP Capture and Route)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'HP' AND Name = 'HPEC')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'HP',     'HPEC',       'new Regex(@"(HP\s*EC|HP Embedded Capture)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'HP' AND Name = 'ePrint')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'HP',     'ePrint',     'new Regex(@"ePrint", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'Nuance' AND Name = 'Equitrac')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'Nuance', 'Equitrac',   'new Regex(@"Equitrac", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'Nuance' AND Name = 'SafeCom')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'Nuance', 'SafeCom',    'new Regex(@"SafeCom", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END

  IF NOT EXISTS (SELECT * FROM AssociatedProduct WHERE Vendor = 'Pharos' AND Name = 'Blueprint')
  BEGIN
    INSERT INTO AssociatedProduct (AssociatedProductId, Vendor, Name, MatchCriteria)
    VALUES (NEWID(), 'Pharos', 'Blueprint',  'new Regex(@"Blueprint", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)')
  END
GO


-- 2017 Mar 09 Max Tuinstra
-- Remove plugin reference from database.  Corresponding code to be removed from solution.

  DELETE MetadataTypeResourceTypeAssoc WHERE MetadataTypeName = 'EquitracPullPrintingCertification'
  IF @@ROWCOUNT = 1
    BEGIN
      DELETE MetadataType WHERE name = 'EquitracPullPrintingCertification'
    END

-- 2017 Mar 09 Max Tuinstra
-- Remove plugin reference from database.  Corresponding code to be removed from solution.

  DELETE MetadataTypeResourceTypeAssoc WHERE MetadataTypeName = 'HpacPullPrintingCertification'
  IF @@ROWCOUNT = 1
    BEGIN
      DELETE MetadataType WHERE name = 'HpacPullPrintingCertification'
    END

-- 2017 Mar 09 Max Tuinstra
-- Remove plugin reference from database.  Corresponding code to be removed from solution.

  DELETE MetadataTypeResourceTypeAssoc WHERE MetadataTypeName = 'SafeComPullPrintingCertification'
  IF @@ROWCOUNT = 1
    BEGIN
      DELETE MetadataType WHERE name = 'SafeComPullPrintingCertification'
    END
