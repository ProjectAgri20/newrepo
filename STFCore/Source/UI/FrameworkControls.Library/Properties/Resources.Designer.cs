﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HP.ScalableTest.UI.Framework.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HP.ScalableTest.UI.Framework.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to If you click Continue, the application will ignore the error and attempt to continue.
        ///If you click Quit, the application will close immediately..
        /// </summary>
        internal static string ExceptionInstructionsContinueQuit {
            get {
                return ResourceManager.GetString("ExceptionInstructionsContinueQuit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application cannot continue execution, and must now close..
        /// </summary>
        internal static string ExceptionInstructionsQuitOnly {
            get {
                return ResourceManager.GetString("ExceptionInstructionsQuitOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Local Directory for Monitor Services. (e.g. D:\MySessions\Output)
        ///Folder Path for DirtyDevice Plugin. (e.g. MySessions\Output).
        /// </summary>
        internal static string MonitorLocationToolTip {
            get {
                return ResourceManager.GetString("MonitorLocationToolTip", resourceCulture);
            }
        }
    }
}
