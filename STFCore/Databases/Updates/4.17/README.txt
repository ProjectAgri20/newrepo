For Boise deployment:

AssetInventory_Boise.sql
MUST be run BEFORE
EnterpriseTest.sql.

The EnterpriseTest update script will drop tables that the Asset Inventory update script depends on.

---

Also note that Boise deployment requires a Linked Server on STFGlobal01, connecting to STFData01.  This connection *should* already exist on STFGlobal01.
