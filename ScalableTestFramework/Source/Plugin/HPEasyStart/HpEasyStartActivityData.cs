using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpEasyStart
{
    /// <summary>
    /// Activity Data Class for the Hp Easy Start Plugin
    /// </summary>
    [DataContract]
    internal class HpEasyStartActivityData
    {
        public HpEasyStartActivityData()
        {
            HpEasyStartInstallerPath = string.Empty;
            PrintTestPage = true;
            SetAsDefaultDriver = true;
            IsWebPackInstallation = false;
        }

        /// <summary>
        /// Path for the HP Easy Start Installer Setup
        /// </summary>
        [DataMember]
        public string HpEasyStartInstallerPath { get; set; }

        /// <summary>
        /// Whether the test page has to be printed post to installation
        /// </summary>
        [DataMember]
        public bool PrintTestPage { get; set; }

        /// <summary>
        /// Whether the driver has to be set as default driver post to installation
        /// </summary>
        [DataMember]
        public bool SetAsDefaultDriver { get; set; }

        /// <summary>
        /// Whether the Installation is of WebPack
        /// </summary>
        [DataMember]
        public bool IsWebPackInstallation { get; set; }

    }
}
