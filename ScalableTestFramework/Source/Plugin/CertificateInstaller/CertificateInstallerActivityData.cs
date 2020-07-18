using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.CertificateInstaller
{
    [DataContract]
    public class CertificateInstallerActivityData
    {
        /// <summary>
        /// Install Printer CA certificate
        /// </summary>
        [DataMember]
        public bool InstallPrinterCA { get; set; }

        /// <summary>
        /// Delete Printer CA certificate
        /// </summary>
        [DataMember]
        public bool DeletePrinterCA { get; set; }

        /// <summary>
        /// Printer CA certificate name including path
        /// </summary>
        [DataMember]
        public string CACertificate { get; set; }

        /// <summary>
        /// Install Client VM CA certificate
        /// </summary>
        [DataMember]
        public bool ClientVMCA { get; set; }

        /// <summary>
        /// Allow Intermediate CA certificate installation
        /// </summary>
        [DataMember]
        public bool IntermediateCA { get; set; }

        /// <summary>
        /// Specifies the browser type for the WebRequest
        /// </summary>
        [DataMember]
        public string BrowserType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateInstallerActivityData"/> class.
        /// </summary>
        public CertificateInstallerActivityData()
        {
            InstallPrinterCA = false;
            DeletePrinterCA = false;
            ClientVMCA = false;
            CACertificate = string.Empty;

            IntermediateCA = false;
            BrowserType = string.Empty;
        }
    }
}