

This folder will contain any database update scripts for versions beyond the base version.
The STB versions are based on the STF version. If the base version scripts will build
3.11 for example, these scripts will support  (ie 3.12, 3.13, etc.).  The STB base version
value is located in the "SchemaVersion.txt" file located in the DatabaseCreation folder.
This value really shouldn't change once the base version is established.  The only time
it would change is if there is a decision to re-baseline the complete installer.

The Installer will try to determine if the database schema for STB has been installed yet,
and if not it will execute all the scripts in the DatabaseCreation folder, which will 
establish the baseline version of the database.  Once those are complete, it will iterate
over any update scripts in the DatabaseUpdates folder, and it will update the base version
after each script is executed.

If the installer is executed in an environment that already has the database installed.  It
will determine what the current base version of the database is and then look in the UpdateScripts
folder to see if there are any versions greater that the current version.  If there are newer
versions, the installer will execute each version in numerical order, and will update the base
version in the database each time.

The database/STB version is being kept in an SQL Extended Property called "STF Version".  The
installer depends on that property to determine what the current version is. 

Update Scripts Naming Convention

When you add new update scripts, they must follow this format:

In the DatabaseUpdates solution folder, you should add a new subfolder named in the pattern
of "vX.XX" or "vX.XXX" if you have a patch release of a version, like 3.161.

The version specific folders you will add updates scripts targeted for one of the four STF
databases.  The naming convention must be exact.  These file names are used to look up 
system settings in the installed system to determine the location of the actual database
instance. The following names should be used for each database.

AssetInventory.sql - use this for any updates to the Asset Inventory database.
EnterpriseTest.sql - use this for any updates to the main STF database
DocumentLibrary.sql - use this for any updates to the Test Document Library database
DataLog.sql - use this for any updates to the Datalogger Database

The folder structure in the solution should look like this:

DatabaseUpdates
   |--- v3.12
          |--- AssetInventory.sql
		  |--- EnterpriseTest.sql
   |--- v3.13
          |--- AssetInventory.sql
		  |--- EnterpriseTest.sql
		  |--- DocumentLibrary.sql
		  |--- DataLog.sql

Each file should have a version number that corresponds to the database updates applied to STF
for the same version.  They should be in order.  At some point we can update the base scripts
to include all the updates and elevate the base version to the last version contained in the updates.  

**** IMPORTANT *****************************************************************************************
-- When scripts are added to any versioned solution folder, they should be added as "linked" files.  The
scripts will be kept in a general location in the source respository, and if they are simply added 
and not linked, a copy of them will be made which will duplicate them and introduce a maintenance risk.

-- Also make sure that any update scripts you add to this folder are set in Visual Studio properties
as an "Embedded Resource".  This is done through the "Build Action" property of the file. If you
don't do this it will not be included in the installer.

********************************************************************************************************
