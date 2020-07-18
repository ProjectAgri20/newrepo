﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HP.ScalableTest.Service.Monitor.Eprint {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HP.ScalableTest.Service.Monitor.Eprint.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [Pending].
        /// </summary>
        internal static string ClearAllSql {
            get {
                return ResourceManager.GetString("ClearAllSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N&apos;[dbo].[trg_InsertPending]&apos;))
        ///DROP TRIGGER [dbo].[trg_InsertPending]
        ///GO
        ///CREATE TRIGGER [dbo].[trg_InsertPending] ON [dbo].[job] AFTER INSERT, UPDATE
        ///AS 
        ///BEGIN
        ///    SET NOCOUNT ON;
        ///	DECLARE @id AS INT
        ///	SELECT @id = ID FROM INSERTED
        ///	IF NOT EXISTS (SELECT JobId FROM [EPrintMonitor].[dbo].[Pending] WHERE JobId = @id)
        ///	BEGIN
        ///		INSERT INTO [EPrintMonitor].[dbo].[Pending] VALUES (@id, 1)
        ///	END
        ///END.
        /// </summary>
        internal static string CloudPrintTrigger {
            get {
                return ResourceManager.GetString("CloudPrintTrigger", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [master]
        ///GO
        ///CREATE DATABASE [EPrintMonitor]
        ///GO
        ///ALTER DATABASE [EPrintMonitor] SET COMPATIBILITY_LEVEL = 100
        ///GO
        ///ALTER DATABASE EPrintMonitor MODIFY FILE
        ///( NAME = N&apos;EPrintMonitor&apos;, SIZE = 4096KB , MAXSIZE = 50MB, FILEGROWTH = 1024KB )
        ///GO
        ///ALTER DATABASE EPrintMonitor MODIFY FILE
        ///( NAME = N&apos;EPrintMonitor_log&apos;, SIZE = 1024KB , MAXSIZE = 50MB , FILEGROWTH = 10% )
        ///GO
        ///IF (1 = FULLTEXTSERVICEPROPERTY(&apos;IsFullTextInstalled&apos;))
        ///begin
        ///EXEC [EPrintMonitor].[dbo].[sp_fulltext_database] @action = &apos;enable&apos;        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateMonitorDatabase {
            get {
                return ResourceManager.GetString("CreateMonitorDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [Pending] WHERE JobId IN ({0}).
        /// </summary>
        internal static string DeleteProcessedJobsSql {
            get {
                return ResourceManager.GetString("DeleteProcessedJobsSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CloudPrint.
        /// </summary>
        internal static string ePrintDatabase {
            get {
                return ResourceManager.GetString("ePrintDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM [Pending]  ORDER BY JobId.
        /// </summary>
        internal static string GetUnprocessedJobsSql {
            get {
                return ResourceManager.GetString("GetUnprocessedJobsSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT j.ID, j.Name AS JobName, j.CreatedAt, j.LastStatusAt, j.GroupID, jg.userDateTime, jg.Subtitle, u.NTUserAccount, jg.[GUID] AS JobFolderId, js.Name AS JobStatus, gs.Name as TransactionStatus, p.Name + &apos; : &apos; + p.ModelName as Printer 
        ///FROM job j 
        ///LEFT OUTER JOIN jobgroup jg ON j.GroupID = jg.ID
        ///LEFT OUTER JOIN useraccount u ON jg.UserAccountID = u.ID
        ///LEFT OUTER JOIN jobgroupstatus gs on jg.StatusID = gs.ID
        ///LEFT OUTER JOIN printer p on jg.PrinterID = p.ID
        ///LEFT OUTER JOIN jobstatus js ON j.StatusID = [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JobDataSql {
            get {
                return ResourceManager.GetString("JobDataSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EPrintMonitor.
        /// </summary>
        internal static string MonitorDatabase {
            get {
                return ResourceManager.GetString("MonitorDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE [Pending] SET IsInsert = 0 WHERE JobId IN ({0}).
        /// </summary>
        internal static string UpdateProcessedJobsSql {
            get {
                return ResourceManager.GetString("UpdateProcessedJobsSql", resourceCulture);
            }
        }
    }
}
