﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SafeComVirtualPrinterConfig.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SafeComVirtualPrinterConfig.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to 1.3.6.1.2.1.1.1.0|4|HP ETHERNET MULTI-ENVIRONMENT,ROM none,JETDIRECT,JD149,EEPROM JDI23500064,CIDATE 03/18/2015
        ///1.3.6.1.2.1.1.2.0|6|1.3.6.1.4.1.11.2.3.9.1
        ///1.3.6.1.2.1.1.3.0|67|202366
        ///1.3.6.1.2.1.1.4.0|4|
        ///1.3.6.1.2.1.1.5.0|4|NPI26E0EC
        ///1.3.6.1.2.1.1.6.0|4|
        ///1.3.6.1.2.1.1.7.0|2|64
        ///1.3.6.1.2.1.2.1.0|2|2
        ///1.3.6.1.2.1.2.2.1.1.1|2|1
        ///1.3.6.1.2.1.2.2.1.1.2|2|2
        ///1.3.6.1.2.1.2.2.1.2.1|4|HP ETHERNET MULTI-ENVIRONMENT,ROM none,JETDIRECT,JD149,EEPROM JDI23500064
        ///1.3.6.1.2.1.2.2.1.2.2|4|HP ETHERNET MULTI-ENVIRONMENT,ROM none [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string azalea_Template {
            get {
                return ResourceManager.GetString("azalea_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] snmpsimd {
            get {
                object obj = ResourceManager.GetObject("snmpsimd", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to start &quot;{0} SNMP&quot; &quot;{1}&quot; --agent-udpv4-endpoint={0} --data-dir={2}.
        /// </summary>
        internal static string StartCommand {
            get {
                return ResourceManager.GetString("StartCommand", resourceCulture);
            }
        }
    }
}
