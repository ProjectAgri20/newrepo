﻿using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpecInstaller
{
    /// <summary>
    /// Class containing the metadata items to be serialized for the plug-in.
    /// </summary>
    /// <remarks>
    /// The STF framework will use <see cref="NetDataContractSerializer"/>, which stores CLR type
    /// information in the serialized XML. This has the advantage of handling polymorphic types,
    /// but requires it to be more stringent regarding type names, namespaces, and assemblies of
    /// the types being serialized. Changes to the plug-in assembly name, plug-in namespace, or
    /// metadata class name will break existing activity data. Some changes to the members of the
    /// plug-in class may also break existing activity data. Ensure the assembly name, namespace,
    /// and plug-in metadata class are as desired before using the plug-in.
    /// 
    /// •  The metadata class must be decorated with a <c>DataContract</c> attribute.
    /// •  Any data that should be persisted during serialization must be decorated with a
    ///    <c>DataMember</c> attribute. This attribute can be applied to fields or properties; any
    ///    properties with this attribute must have a getter and a setter, though these do not have
    ///    to be public.
    /// •  The new metadata class should not include information about selected assets or
    ///    documents. This data is stored elsewhere and will be automatically persisted by the
    ///    framework.
    /// •  The new metadata class should minimize references to types outside of the plug-in,
    ///    other than .NET native types.
    /// 
    /// <seealso cref="System.Runtime.Serialization.NetDataContractSerializer"/>
    /// </remarks>
    [DataContract]
    public class HpecInstallerActivityData
    {
        [DataMember]
        public string WorkflowFile { get; set; }

        [DataMember]
        public HpecInstallerAction InstallerAction { get; set; }

        [DataMember]
        public string ServerVersion { get; set; }

        public HpecInstallerActivityData()
        {
            ServerVersion = "1.5.0.5";
        }
    }

    public enum HpecInstallerAction
    {
        InstallHpec,
        UploadWorkflow,
        RemoveWorkflow,
        UninstallHpec
    }
}
