﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HP.ScalableTest.DeviceAutomation.DeviceSettings {
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
    internal class SettingsManagerResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SettingsManagerResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HP.ScalableTest.DeviceAutomation.DeviceSettings.SettingsManagerResource", typeof(SettingsManagerResource).Assembly);
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
        ///   Looks up a localized string similar to @PJL RDYMSG DISPLAY=&quot;&quot;
        ///@PJL SET SERVICEMODE=HPBOISEID
        ///@PJL SET JOBMEDIA=ON
        ///@PJL DEFAULT JOBMEDIA=ON.
        /// </summary>
        internal static string JediPaperlessDisable {
            get {
                return ResourceManager.GetString("JediPaperlessDisable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @PJL RDYMSG DISPLAY=&quot;Paperless Mode&quot;
        ///@PJL SET SERVICEMODE=HPBOISEID
        ///@PJL SET JOBMEDIA=OFF
        ///@PJL DEFAULT JOBMEDIA=OFF.
        /// </summary>
        internal static string JediPaperlessEnable {
            get {
                return ResourceManager.GetString("JediPaperlessEnable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @PJL INQUIRE JOBMEDIA.
        /// </summary>
        internal static string JediPaperlessInquire {
            get {
                return ResourceManager.GetString("JediPaperlessInquire", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @PJL JOB
        ///@PJL SET SERVICEMODE=HPBOISEID
        ///@PJL DEFAULT DIAGNOSTICS=OFF
        ///@PJL DEFAULT JOBCRCMODE=OFF
        ///@PJL DEFAULT JOBMEDIA=ON
        ///@PJL EOJ.
        /// </summary>
        internal static string OzPaperlessDisable {
            get {
                return ResourceManager.GetString("OzPaperlessDisable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @PJL JOB
        ///@PJL SET SERVICEMODE=HPBOISEID
        ///@PJL DEFAULT DIAGNOSTICS=ON
        ///@PJL DEFAULT JOBCRCMODE=ON
        ///@PJL DEFAULT JOBMEDIA=OFF
        ///@PJL EOJ.
        /// </summary>
        internal static string OzPaperlessEnable {
            get {
                return ResourceManager.GetString("OzPaperlessEnable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=\&quot;1.0\&quot; encoding=\&quot;UTF - 8\&quot;?&gt;
        ///&lt;!--THIS DATA SUBJECT TO DISCLAIMER(S)INCLUDED WITH THE PRODUCT OF ORIGIN.--&gt;
        ///	&lt;prdservicedyn:ProductServiceDyn xmlns:prdservicedyn=&quot;http://www.hp.com/schemas/imaging/con/ledm/productservicedyn/2009/01/10&quot; xmlns:dd=&quot;http://www.hp.com/schemas/imaging/con/dictionaries/1.0/&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xsi:schemaLocation=&quot;http://www.hp.com/schemas/imaging/con/ledm/productservicedyn/2009/01/10 ../schemas/ProductServiceDyn.xsd&quot;&gt;
        ///		&lt;prdserv [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PhoenixTelnetDebugEnable {
            get {
                return ResourceManager.GetString("PhoenixTelnetDebugEnable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [netshell_port]
        ///enable=1.
        /// </summary>
        internal static string SiriusTelnetDebugEnable {
            get {
                return ResourceManager.GetString("SiriusTelnetDebugEnable", resourceCulture);
            }
        }
    }
}
