using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) for SoftwareInstallerSettings.
    /// </summary>
    [DataContract(Name="SoftwareInstallerSetting", Namespace="")]
    public class SoftwareInstallerSettingContract
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SoftwareInstallerSettingContract(SoftwareInstallerSetting setting)
        {
            InstallOrderNumber = setting.InstallOrderNumber;            
        }

        /// <summary>
        /// Creates an instance of a SoftwareInstallerSetting from this instance.
        /// </summary>
        public SoftwareInstallerSetting CreateEntity()
        {
            var setting = new SoftwareInstallerSetting()
            {
                InstallOrderNumber = this.InstallOrderNumber
            };

            return setting;
        }

        /// <summary>
        /// Defines the order in which the SofwareInstaller is executed.
        /// </summary>
        [DataMember]
        public int InstallOrderNumber { get; set; }

        /// <summary>
        /// The Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The SoftwareInstallerContract object (parent object).
        /// </summary>
        [DataMember]
        public SoftwareInstallerContract Installer { get; set; }
    }
}
