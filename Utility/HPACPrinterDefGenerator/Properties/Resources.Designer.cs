﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HPACPrinterDefGenerator.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HPACPrinterDefGenerator.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to  PRTLNAME = {0}
        /// DEPT = {1}
        /// COMMTYPE = TCPIP/SOCK
        /// COLOR    = YES
        /// DUPLEX   = YES
        /// PULLPRINT = YES
        /// TCPHOST  = {2}
        /// TCPRPORT = 9100
        /// PJL      = NO
        /// SNMP     = NO
        ///.
        /// </summary>
        internal static string PrinterDefinitionNoFilter {
            get {
                return ResourceManager.GetString("PrinterDefinitionNoFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  PRTLNAME = {0}
        /// DEPT = {1}
        /// COMMTYPE = TCPIP/SOCK
        /// F1ARGS   = &quot;&amp;prt_tcphost&quot; &quot;&amp;infile&quot; &quot;&amp;outfile&quot; &quot;&amp;rmtqueue&quot; &quot;&amp;filename&quot; &quot;&amp;ctime&quot; &quot;&amp;bytes&quot; &quot;&amp;pages&quot; &quot;&amp;color&quot; &quot;&amp;duplex&quot; &quot;&amp;host&quot; &quot;&amp;pagesize&quot; &quot;&amp;spoolid&quot; notracking
        /// ERRACTN  = HOLD
        /// FILTER1  = C:\Program Files\Hewlett-Packard\HP Access Control\bin\printenterprise.exe
        /// F1DTYPE  = all
        /// COLOR    = YES
        /// DUPLEX   = YES
        /// PULLPRINT = YES
        /// TCPHOST  = {3}
        /// TCPRPORT = 9100
        /// PJL      = NO
        /// SNMP     = NO
        ///.
        /// </summary>
        internal static string PrinterDefinitionWithFilter {
            get {
                return ResourceManager.GetString("PrinterDefinitionWithFilter", resourceCulture);
            }
        }
    }
}
