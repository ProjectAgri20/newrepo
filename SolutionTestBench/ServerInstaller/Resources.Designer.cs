﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HP.SolutionTest {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HP.SolutionTest.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to /UIMode=AutoAdvance /IACCEPTSQLSERVERLICENSETERMS /UpdateEnabled=0 /CONFIGURATIONFILE=&quot;{0}&quot;.
        /// </summary>
        internal static string Args {
            get {
                return ResourceManager.GetString("Args", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error removing installation files ({0}).  You can manually delete them from {1}..
        /// </summary>
        internal static string CleanupError {
            get {
                return ResourceManager.GetString("CleanupError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER DATABASE {0} set single_user;.
        /// </summary>
        internal static string CloseConnectionSql {
            get {
                return ResourceManager.GetString("CloseConnectionSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP DATABASE {0};.
        /// </summary>
        internal static string DropDatabaseSql {
            get {
                return ResourceManager.GetString("DropDatabaseSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exit Solution Test Bench Server Installer?.
        /// </summary>
        internal static string Exit {
            get {
                return ResourceManager.GetString("Exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /u /x:&quot;{0}&quot;.
        /// </summary>
        internal static string ExtractInstallerArgs {
            get {
                return ResourceManager.GetString("ExtractInstallerArgs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @kill varchar(8000) = &apos;kill &apos;
        ///SELECT @kill = @kill + CONVERT(varchar(5), session_id) + &apos;;&apos;
        ///FROM sys.dm_exec_sessions
        ///WHERE database_id  = db_id(&apos;{0}&apos;)
        ///EXEC @kill;.
        /// </summary>
        internal static string KillDatabaseProcessSql {
            get {
                return ResourceManager.GetString("KillDatabaseProcessSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database installation cancelled.
        /// </summary>
        internal static string ServerInstallCancelled {
            get {
                return ResourceManager.GetString("ServerInstallCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SQL Express installation completed.
        /// </summary>
        internal static string ServerInstallCompleted {
            get {
                return ResourceManager.GetString("ServerInstallCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database installation exists, no action will be taken{0}{1}.
        /// </summary>
        internal static string ServerInstallNotRequired {
            get {
                return ResourceManager.GetString("ServerInstallNotRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SQL Server database installation required.
        /// </summary>
        internal static string ServerInstallRequired {
            get {
                return ResourceManager.GetString("ServerInstallRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data schema installation cancelled.
        /// </summary>
        internal static string StbSchemaInstallCancelled {
            get {
                return ResourceManager.GetString("StbSchemaInstallCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data schema installation completed.
        /// </summary>
        internal static string StbSchemaInstallCompleted {
            get {
                return ResourceManager.GetString("StbSchemaInstallCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data schema up to date, no action will be taken.
        /// </summary>
        internal static string StbSchemaInstallNotRequired {
            get {
                return ResourceManager.GetString("StbSchemaInstallNotRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data schema installation required.
        /// </summary>
        internal static string StbSchemaInstallRequired {
            get {
                return ResourceManager.GetString("StbSchemaInstallRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data service installation/update cancelled.
        /// </summary>
        internal static string StbServiceInstallCancelled {
            get {
                return ResourceManager.GetString("StbServiceInstallCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data service installation/update completed.
        /// </summary>
        internal static string StbServiceInstallCompleted {
            get {
                return ResourceManager.GetString("StbServiceInstallCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data service already installed, no action will be taken.
        /// </summary>
        internal static string StbServiceInstallNotRequired {
            get {
                return ResourceManager.GetString("StbServiceInstallNotRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data service installation required.
        /// </summary>
        internal static string StbServiceInstallRequired {
            get {
                return ResourceManager.GetString("StbServiceInstallRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data service update is required.
        /// </summary>
        internal static string StbServiceUpdateRequired {
            get {
                return ResourceManager.GetString("StbServiceUpdateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench Installation Complete!.
        /// </summary>
        internal static string StbSystemInstallationComplete {
            get {
                return ResourceManager.GetString("StbSystemInstallationComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Test Bench data schema at version {0}, it will be updated to version {1}.
        /// </summary>
        internal static string StbUpdateRequired {
            get {
                return ResourceManager.GetString("StbUpdateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Setup.exe /Action=Uninstall /FEATURES=SQLENGINE /INSTANCENAME=MSSQLSERVER /UIMode=AutoAdvance.
        /// </summary>
        internal static string UninstallSqlServer {
            get {
                return ResourceManager.GetString("UninstallSqlServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [master];.
        /// </summary>
        internal static string UseMaster {
            get {
                return ResourceManager.GetString("UseMaster", resourceCulture);
            }
        }
    }
}
