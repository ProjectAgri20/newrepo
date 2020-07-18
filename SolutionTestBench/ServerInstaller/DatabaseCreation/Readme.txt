The SchemaVersion.txt file in this directory should contain a single entry that is the
current version of this STB release.  The value must be a double value type such 
as 3.15, 3.16, etc.  It will be used to update the STB database to flag what its
schema version is set to.  

If there are update scripts in the DatabaseUpdates folder that extend the schema beyond
the base version represented in the DatabaseCreation folder, then the SchemaVersion.txt
file should have the highest version entered.