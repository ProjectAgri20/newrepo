
/*****************************************************************************
 * Title:        Delete All Data
 * Author:       Gary Parham
 * Created Date: 2019-02-12
 * Description:  This script deletes all data from every table in a database.
 *               WARNING: Use with care - there is no recovery from this
 *                        script.
 *****************************************************************************/
                
EXEC sys.sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sys.sp_MSforeachtable 'DELETE FROM ?'
GO
EXEC sys.sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO
