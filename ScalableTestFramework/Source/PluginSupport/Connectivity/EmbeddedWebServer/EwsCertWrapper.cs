using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    [Flags]
    public enum CertificateType
    {
        None,

        [EnumValue("Root CA Certificate")]
        CA,

        [EnumValue("Identity Certificate (CA-Signed)")]
        Identity,

        [EnumValue("Self-Signed CA Certificate")]
        SelfSignedCA,

        [EnumValue("Self-Signed Identity Certificate")]
        SelfSignedIdentity,

        [EnumValue("Intermediate CA Certificate")]
        IntermediateCA
    }

    /// <summary>
    /// The certificate table raw details
    /// </summary>
	public struct CertificatesTableRaw
    {
        /// <summary>
        /// The Id of the select element, usually a radio button
        /// </summary>
		public string selectElementId { get; set; }

        /// <summary>
        /// <see cref="CertificateDetails"></see>
        /// </summary>
		public CertificateDetails CertificateData { get; set; }

        /// <summary>
        /// <see cref="CertificateType"/>
        /// </summary>
        public CertificateType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the certificate is selected as network identity.
        /// </summary>
		public bool IsNetworkIdentity { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            CertificatesTableRaw other = (CertificatesTableRaw)obj;
            return (CertificateData.Equals(other.CertificateData) && Type == other.Type && IsNetworkIdentity == other.IsNetworkIdentity);
        }

        public override string ToString()
        {
            return ("Issued To: {0}, Issuer: {1} , Expiry Date: {2}, Certificate Type: {3} {4}".FormatWith(CertificateData.IssuedTo, CertificateData.Issuer, CertificateData.ExpiryDate, Type, IsNetworkIdentity ? "Network Identity" : ""));
        }
    }

    /// <summary>
    /// EWS wrapper class contains Certificate related functions
    /// </summary>
    public sealed partial class EwsWrapper
    {
        #region Constants

        public const int CERTIFCATE_MINIMUM_VALIDITY_PERIOD = 1;
        public const int CERTIFCATE_MAXIMUM_VALIDITY_PERIOD = 3650;
        private const string CERTIFICATE_TEMPLATE_NAME_2048 = "Copy of Web Server";
        private const string CERTIFICATE_TEMPLATE_NAME_1024 = "1k template";

        #endregion Constants

        #region Certificates

        /// <summary>
        /// Export certificate from the printer
        /// Note: Validation is performed for Certificate Unification/ Store or SI products
        /// </summary>
        /// <param name="certificatePath">Certificate path</param>
        /// <param name="intermediate">true to enable intermediate check box</param>
        /// <param name="validate">true to validate if certificate is installed successfully, false to turn off validation</param>
        /// <returns>true if the certificate is installed successfully, else false.</returns>
        public bool InstallCACertificate(string certificatePath, bool intermediate = false, bool validate = true)
        {
            bool executeFinallyBlock = true;
            try
            {
                TraceFactory.Logger.Info("Installing CA Certificate on the printer");
                _adapter.Navigate("CA_InstallCertificate");

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("CAConfigure"))
                {
                    executeFinallyBlock = false;
                    return InstallCACertificateWithUnification(certificatePath, validate);
                }

                _adapter.Click("CAConfigure");
                if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                {
                    _adapter.Check("InstallCA");
                    _adapter.Click("CertificateNext");
                }
                      
               _adapter.SetBrowseControlText("Install", certificatePath);
                TraceFactory.Logger.Info("CA Certificate Path selected.");
                if (_adapter.Settings.ProductType != PrinterFamilies.TPS && _adapter.Settings.ProductType != PrinterFamilies.InkJet)
                {
                    if (intermediate)
                    {
                        _adapter.Check("IntermediateCA");
                    }
                    else
                    {
                        _adapter.Uncheck("IntermediateCA");
                    }
                }

                // If browser loses connectivity during installation of certificate, close the browser and try to open with https.
                try
                {
                    _adapter.Click("Finish");
                    TraceFactory.Logger.Info("Finished");
                    if (_adapter.IsElementPresent("Invalid_CA"))
                    {
                        _adapter.Click("Invalid_CA");
                        TraceFactory.Logger.Info("Invalid CA Certificate can not be Installed.");
                        return true;

                    }
                }
                catch
                {
                    StopAdapter();
                    _adapter.Start();
                }

                // If validate is turned on and if browser loses connectivity, below validation will fail since SearchText won't be visible when a new instance of browser is opened.
                if (validate)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    if (_adapter.SearchText("certificate has been installed") | _adapter.SearchText("certificate has been successfully installed") | _adapter.SearchText("The changes have been updated successfully"))
                    {
                        TraceFactory.Logger.Info("The certificate has been successfully installed");
                        return true;
                    }
                    else if (_adapter.SearchText("The certificate is already installed"))
                    {
                        TraceFactory.Logger.Info("The certificate is already Installed");
                        return true;
                    }
                    else if (_adapter.SearchText("The cryptographic algorithms used in the ID or CA certificate do not comply with FIPS-140"))
                    {
                        TraceFactory.Logger.Info("The cryptographic algorithms used in the ID or CA certificate do not comply with FIPS-140");
                        return false;
                    }
                    else if (_adapter.SearchText("Failed to install the file because the selected file is not a valid CA certificate."))
                    {
                        TraceFactory.Logger.Info("Failed to install the file because the selected file is not a valid CA certificate.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Error in installing Certificate");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Validation is turned off. Assuming certificate installation to be successful.");
                    return true;
                }
            }
            finally
            {
                if (executeFinallyBlock)
                {
                    if (_adapter.IsElementPresent("Ok"))
                    {
                        _adapter.Click("Ok");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }

                    // In case of few errors, even after pressing OK the control will not come back to the certificates home page.
                    if (_adapter.IsElementPresent("Cancel"))
                    {
                        _adapter.Click("Cancel");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                }
            }
        }

        /// <summary>
        /// Sets Jet direct certificate on the printer
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="password">Certificate Password</param>
        /// <param name="validate"></param>
        /// <param name="enablePrivateKey"></param>
        /// <returns>true if the jetdirect certificate is successfully installed, else false.</returns>
        public bool InstallIDCertificate(string certificatePath, string password, bool validate = true, bool enablePrivateKey = false)
        {
            bool executeFinallyBlock = true;
            try
            {
                TraceFactory.Logger.Info("Installing JD Certificate on the printer");

                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Stop();
                    _adapter.Start();
                }

                _adapter.Navigate("CA_InstallCertificate");

                // Check if printer supports certification unification
                if (!_adapter.IsElementPresent("JDConfigure"))
                {
                    executeFinallyBlock = false;
                    return InstallIDCertificateWithUnification(certificatePath, password, validate,enablePrivateKey);
                }

                _adapter.Click("JDConfigure");
                _adapter.Click("ImportCert");
                _adapter.Click("CertificateNext");
                _adapter.SetBrowseControlText("ImportFile", certificatePath);
                _adapter.SetText("PasswordJD", password);

                if (enablePrivateKey)
                {
                    TraceFactory.Logger.Info("Chnaged Key for Export checkbox Id for this");
                    _adapter.Check("Export_PrivateKey");
                }
                _adapter.Click("Finish");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (_adapter.SearchText("The Jetdirect certificate has been successfully updated") || _adapter.SearchText("The printer certificate has been updated") || _adapter.SearchText("The changes have been updated successfully"))
                {
                    TraceFactory.Logger.Info("The Jet direct certificate has been successfully updated");
                    return true;
                }
                else if (_adapter.SearchText("do not comply with FIPS-140"))
                {
                    TraceFactory.Logger.Info("The cryptographic algorithms used in the ID or CA certificate do not comply with FIPS-140");
                    return false;
                }
                else if (_adapter.SearchText("password"))
                {
                    TraceFactory.Logger.Info("Incorrect Password");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Error in installing Certificate");
                    return false;
                }
            }
            catch (Exception certificateexception)
            {
                TraceFactory.Logger.Info(certificateexception.Message);
                return false;
            }
            finally
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("CA_InstallCertificate", "https");
                }
                else
                {
                    if (executeFinallyBlock)
                    {
                        if (_adapter.IsElementPresent("Ok"))
                        {
                            _adapter.Click("Ok");
                            Thread.Sleep(TimeSpan.FromSeconds(30));
                        }

                        // In case of few errors, even after pressing OK the control will not come back to the certificates home page.
                        if (_adapter.IsElementPresent("Cancel"))
                        {
                            _adapter.Click("Cancel");
                            Thread.Sleep(TimeSpan.FromSeconds(30));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes CA certificate on the printer
        /// </summary>
        public bool UnInstallCACertificate(string certificatePath = null)
        {
            bool executeFinallyBlock = true;
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                executeFinallyBlock = false;
            }

            try
            {
                TraceFactory.Logger.Info("Deleting CA Certificate on the printer");

                _adapter.Navigate("CA_InstallCertificate");

                if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                {
                    if (!_adapter.IsElementPresent("CAConfigure"))
                    {
                        executeFinallyBlock = false;
                        return UnInstallCACertificateWithUnification(certificatePath);
                    }

                    _adapter.Click("CAConfigure");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }

                if (_adapter.SearchText("Delete a CA Certificate") | _adapter.SearchText("Delete the Current CA Certificate") | _adapter.IsElementPresent("Installed_Certificate"))
                {
                    _adapter.Click("DeleteCA");
                    if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                    {
                        executeFinallyBlock = false;
                        _adapter.Click("DeleteCA_Ok");
                    }
                    else
                    {
                        _adapter.Click("CertificateNext");
                        _adapter.Click("Finish");
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(40));

                    if (_adapter.SearchText("The CA certificate has been successfully deleted from the Jetdirect print server") | _adapter.SearchText("The CA certificate has been deleted") | _adapter.SearchText("A Certificate Authority (CA) certificate is required "))
                    {
                        TraceFactory.Logger.Info("CA certificate has been successfully deleted");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to delete CA certificate.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("CA certificate is not installed.");
                    return true;
                }
            }
            finally
            {
                if (executeFinallyBlock)
                {
                    if (_adapter.IsElementPresent("Ok"))
                    {
                        _adapter.Click("Ok");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }

                    if (_adapter.IsElementPresent("Delete_Ok"))
                    {
                        _adapter.Click("Delete_Ok");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }

                    // In case of few errors, even after pressing OK the control will not come back to the certificates home page.
                    if (_adapter.IsElementPresent("Cancel"))
                    {
                        _adapter.Click("Cancel");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                }
            }
        }

        /// <summary>
        /// Deletes JD certificate on the printer
        /// </summary>
        public bool UnInstallIDCertificate(string certificatePath = null, string password = null)
        {
            TraceFactory.Logger.Info("Deleting JD Certificate on the printer");
            _adapter.Navigate("CA_InstallCertificate");
            Thread.Sleep(TimeSpan.FromSeconds(15));
            if (!_adapter.IsElementPresent("JDConfigure"))
            {
                return UnInstallIDCertificateWithUnification(certificatePath, idCertificatePassword: password);
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _adapter.Click("JDConfigure");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _adapter.Click("Create_New_Self_Signed_Certificate");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _adapter.Click("CertificateNext");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _adapter.Click("Finish");            
            Thread.Sleep(TimeSpan.FromMinutes(1));

            if (_adapter.SearchText("The Jetdirect certificate has been successfully updated") || _adapter.SearchText("The printer certificate has been updated") || _adapter.SearchText("The changes have been updated successfully"))
            {
                TraceFactory.Logger.Info("Created new self signed certificate as post requisite");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == PrinterFamilies.LFP)
                {
                    _adapter.Click("Ok");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create ID Certificate Request
        /// </summary>
        /// <param name="algorithm"><see cref="SignatureAlgorithm"/></param>
        /// <param name="certificateRequest">Request ID generated</param>
        /// <param name="exportPrivateKey">true to check export private key option, false otherwise</param>
        /// <param name="keyLength"></param>
        /// <param name="fqdn"></param>
        /// <returns>true is request created successfully, false otherwise</returns>
        public bool CreateIDCertificateRequest(SignatureAlgorithm algorithm, out string certificateRequest, bool exportPrivateKey = false, RSAKeyLength keyLength = RSAKeyLength.Rsa2048, string fqdn = null)
        {
            TraceFactory.Logger.Info("Creating ID Certificate Request.");

            certificateRequest = string.Empty;
            bool executeFinallyBlock = true;

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Stop();
                    _adapter.Start();
                }

                _adapter.Navigate("CA_InstallCertificate");

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("RequestCAConfigure"))
                {
                    executeFinallyBlock = false;
                    return CreateIDCertificateRequestWithUnification(algorithm, out certificateRequest, exportPrivateKey, keyLength, fqdn);
                }

                // Navigate to Create Certificate Page
                _adapter.Click("JDConfigure");
                _adapter.Check("Create_Certificate");
                _adapter.Click("CertificateNext");

                bool isTps = _adapter.Settings.ProductType == PrinterFamilies.TPS;
                bool isInkjet = _adapter.Settings.ProductType == PrinterFamilies.InkJet;
                string searchText = "The certificate request has been created";
                string commonName = fqdn.EqualsIgnoreCase(null) ? _adapter.Settings.DeviceAddress : fqdn;

                // Configure All values
                _adapter.SetText("Common_Name", commonName, sendTab: isInkjet);
                _adapter.SetText("Organization", "Hewlett-Packard Inc", sendTab: isInkjet);
                _adapter.SetText("Organizational_Unit", "Unit", sendTab: isInkjet);
                _adapter.SetText("City", "Bangalore", sendTab: isInkjet);
                _adapter.SetText("State", "Karnataka", sendTab: isInkjet);
                _adapter.SetText("Country", "IN", sendTab: isInkjet);

                if (!(isTps || isInkjet))
                {
                    searchText = "The certificate request has been successfully created";

                    if (keyLength == RSAKeyLength.Rsa2048)
                    {
                        _adapter.Check("RSA_Length_2048");
                    }
                    else if (keyLength == RSAKeyLength.Rsa1024)
                    {
                        _adapter.Check("RSA_Length_1024");
                    }
                    _adapter.Check(algorithm.ToString());

                    if (exportPrivateKey)
                    {
                        _adapter.Check("Export_PrivateKey");
                    }

                    _adapter.Click("Finish");
                }
                else
                {
                    _adapter.Click("CertificateNext");
                }

                Thread.Sleep(TimeSpan.FromMinutes(2));

                if (SearchTextInPage(searchText))
                {
                    if (_adapter.IsElementPresent("Key_Path"))
                    {
                        certificateRequest = _adapter.GetText("Key_Path");
                        TraceFactory.Logger.Info("Certificate key generated successfully.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Unable find the xpath to generated key.");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Certificate key was not generated..");
                    return false;
                }
            }
            finally
            {
                _adapter.Stop();
                _adapter.Start();

                if (executeFinallyBlock)
                {
                    // Both the cases where creation is successful/ error; Cancel button needs to be clicked on completion
                    if (_adapter.IsElementPresent("Install_Cancel"))
                    {
                        _adapter.Click("Install_Cancel");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                }
            }
        }

        /// <summary>
        /// Install ID Certificate
        /// Note: This option is available only when a request id is created on the printer
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="exportPrivateKey">Option to check/Uncheck Export Private Key. True to check, false to uncheck</param>
        /// <returns>true if installation is successful, false otherwise</returns>
        public bool InstallCertificate(string certificatePath, bool exportPrivateKey = false)
        {
            TraceFactory.Logger.Info("Installing ID certificate");
            bool executeFinallyBlock = true;

			try
			{
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    TraceFactory.Logger.Info("closing and opening the browser in Inkdevices");
                    _adapter.Stop();
                    _adapter.Start();
                    TraceFactory.Logger.Info("restarted the browser");
                }
                TraceFactory.Logger.Info("Navigating to the certificate page");
                _adapter.Navigate("CA_InstallCertificate", "https");

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("RequestCAConfigure"))
                {
                    executeFinallyBlock = false;
                    return InstallIDCertificateWithUnification(certificatePath, string.Empty);
                }

                // Navigate to Install Certificate Page
                _adapter.Click("JDConfigure");

                if (!_adapter.IsElementPresent("Install_Certificate"))
                {
                    TraceFactory.Logger.Info("Install Certificate is not available on page.");
                    TraceFactory.Logger.Debug("Option will not be available if certificate request is unsuccessful.");
                    return false;
                }

                _adapter.Check("Install_Certificate");
                _adapter.Click("CertificateNext");
                _adapter.SetBrowseControlText("Install_Path", certificatePath);

                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Click("Finish");

                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (SearchTextInPage("The printer certificate has been updated"))
                    {
                        // Check if confirmation button is present in page
                        if (_adapter.IsElementPresent("Install_Ok"))
                        {
                            _adapter.Click("Install_Ok");
                            TraceFactory.Logger.Info("ID Certificate Installed Successfully");
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    _adapter.Click("Finish");

                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (SearchTextInPage("The certificate has been successfully installed") || SearchTextInPage("The changes have been updated successfully"))
                    {
                        if (!(_adapter.Settings.ProductType == PrinterFamilies.InkJet))
                        {
                            // Check if confirmation button is present in page
                            if (_adapter.IsElementPresent("Ok"))
                            {
                                _adapter.Click("Ok");
                                Thread.Sleep(TimeSpan.FromMinutes(1));
                            }

                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception certificateException)
            {
                TraceFactory.Logger.Info("Failed to install Certificate : Exception {0}".FormatWith(certificateException));
                return false;
            }
            finally
            {
                if (executeFinallyBlock)
                {
                    // Both the cases where install is successful/ error; Cancel button needs to be clicked on completion
                    if (_adapter.IsElementPresent("Cancel"))
                    {
                        _adapter.Click("Cancel");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }
                }
            }
        }

        /// <summary>
        /// Installs a self signed certificate with the default parameters
        /// </summary>
        /// <returns>True if the installation is succesful, else false.</returns>
        public bool InstallSelfSignedCertificate()
        {
            try
            {
                TraceFactory.Logger.Info("Installing Self Signed Certificate.");
                //VEP Tests were failing to install so trying to reopen the page, there were no as such issues
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("CA_InstallCertificate");

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("JDConfigure"))
                {
                    _adapter.Navigate("CertificateManagement");

                    _adapter.Click("Create_SelfSignedIdCert");
                    _adapter.Click("Create_Ok");
                    //_adapter.Click("Confirm_Installation");
                    //Adding this Part since Date fiend give Error messgae if Given string valie instead of selected from calender so checking once again
                    if (_adapter.IsElementPresent("Create_Ok"))
                    {
                        TraceFactory.Logger.Info("Error occured, Trying once again");
                        _adapter.Click("Create_Ok");
                       // _adapter.Click("Confirm_Installation");
                                               
                    }
                    _adapter.Click("Confirm_Installation");
                }
                else
                {
                    _adapter.Click("JDConfigure");
                    _adapter.Click("Create_New_Self_Signed_Certificate");
                    _adapter.Click("CertificateNext");

                    _adapter.Click("Finish");
                }

                Thread.Sleep(TimeSpan.FromMinutes(3));

                if (_adapter.SearchText("The printer certificate has been updated.") || _adapter.SearchText("The Jetdirect certificate has been successfully updated") || _adapter.SearchText("The operation was executed successfully") || _adapter.SearchText("The operation was completed successfully"))
                {
                    TraceFactory.Logger.Info("Successfully installed the self-signed certificate through Web UI.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                TraceFactory.Logger.Debug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                return false;
            }
            finally
            {
                if (_adapter.IsElementPresent("Install_Ok"))
                {
                    _adapter.Click("Install_Ok");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }

                // When install is unsuccessful/ error, Cancel button needs to be clicked maximum of one times to exit the wizard properly
                if (_adapter.IsElementPresent("Cancel"))
                {
                    _adapter.Click("Cancel");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            }
        }

        /// <summary>
        /// Install self signed certificate
        /// </summary>
        /// <param name="algorithm"><see cref="SignatureAlgorithm"/></param>
        /// <param name="keyLength"><see cref="RSAKeyLength"/></param>
        /// <param name="exportPrivateKey">True to export private key.</param>
        /// <param name="validityPeriod">Validity period in no of days.</param>
        /// <returns>True if the installation is successful, else false.</returns>
        public bool InstallSelfSignedCertificate(SignatureAlgorithm algorithm, RSAKeyLength keyLength, bool exportPrivateKey, int validityPeriod)
        {
            if (validityPeriod < CERTIFCATE_MINIMUM_VALIDITY_PERIOD || validityPeriod > CERTIFCATE_MAXIMUM_VALIDITY_PERIOD)
            {
                TraceFactory.Logger.Info("Certificate validity period is out of range. It should be between {0} - {1}".FormatWith(CERTIFCATE_MINIMUM_VALIDITY_PERIOD, CERTIFCATE_MAXIMUM_VALIDITY_PERIOD));
                return false;
            }

            try
            {
                /* Create/ Installation of Self Signed Certificate based on Product Family
                 * -------------------------------------------------------------------------------------------------------------------------------------------------------- *
                 * Product Family        : Inputs                                                                   | Main Page Configuration    | Edit Settings            *
                 * -------------------------------------------------------------------------------------------------------------------------------------------------------- *
                 * VEP - Non-unification : Validity Period, Export Private Key, Signature Algorithm, RSA Key Length | RSA, Signature, Export Key | Validity                 *
                 *     - Unification     : Validity Period, Export Private Key, Signature Algorithm, RSA Key Length | RSA, Signature, Export Key | Validity                 *
                 * LFP                   : Validity Period, Export Private Key, Signature Algorithm, RSA key Length | RSA, Signature             | Validity, Export Key
                 * TPS                   : Validity Period, Export Private Key                                      | No Configuration           | Validity, Export Key     *
                 * Inkjet                : Validity Period, Export Private Key, Signature Algorithm                 | Signature                  | Validity, Export Key     *
                 * -------------------------------------------------------------------------------------------------------------------------------------------------------- *
                 * */

                TraceFactory.Logger.Info("Installing Self Signed Certificate.");

                if (_adapter.Settings.ProductType != PrinterFamilies.InkJet && algorithm == SignatureAlgorithm.MD5)
                {
                    TraceFactory.Logger.Info("The signature algorithm: {0} is not supported.".FormatWith(algorithm));
                    return true;
                }

                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet && algorithm == SignatureAlgorithm.Sha224)
                {
                    TraceFactory.Logger.Info("The signature algorithm: {0} is not supported.".FormatWith(algorithm));
                    return true;
                }

                bool isVep = _adapter.Settings.ProductType == PrinterFamilies.VEP;
                bool isTps = _adapter.Settings.ProductType == PrinterFamilies.TPS;

                _adapter.Navigate("CA_InstallCertificate");

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("JDConfigure"))
                {
                    return InstallSelfSignedCertificateWithUnification(algorithm, keyLength, exportPrivateKey, validityPeriod);
                }

                _adapter.Click("JDConfigure");
                _adapter.Click("Create_New_Self_Signed_Certificate");
                _adapter.Click("CertificateNext");

                // Configure 'Edit Settings' Page before configuring 'Main Page' since state of configuration are not maintained when we traverse through page.

                // Click on Edit for configuring Validity Period
                _adapter.Click("Certificate_Edit_Settings");
                _adapter.SetText("Certificate_Validity_Period", validityPeriod.ToString());

                if (!isVep)
                {
                    if (_adapter.IsElementPresent("Export_PrivateKey"))
                    {
                        if (exportPrivateKey)
                        {
                            _adapter.Check("Export_PrivateKey");
                        }
                        else
                        {
                            _adapter.Uncheck("Export_PrivateKey");
                        }
                    }
                }

                _adapter.Click("Certificate_Validity_Apply");

                // Main page configuration
                if (!isTps)
                {
                    _adapter.Check(algorithm.ToString());

                    if (_adapter.IsElementPresent("Export_PrivateKey"))
                    {
                        if (exportPrivateKey)
                        {
                            _adapter.Check("Export_PrivateKey");
                        }
                        else
                        {
                            _adapter.Uncheck("Export_PrivateKey");
                        }
                    }

                    if (keyLength == RSAKeyLength.Rsa2048)
                    {
                        _adapter.Check("RSA_Length_2048");
                    }
                    else if (keyLength == RSAKeyLength.Rsa1024)
                    {
                        _adapter.Check("RSA_Length_1024");
                    }
                }

                if (_adapter.IsElementPresent("Finish"))
                {
                    _adapter.Click("Finish");
                }

                Thread.Sleep(TimeSpan.FromMinutes(3));

                if (_adapter.SearchText("The printer certificate has been updated.") || _adapter.SearchText("The Jetdirect certificate has been successfully updated") || _adapter.SearchText("The changes have been updated successfully")|| _adapter.SearchText("The changes have been updated successfully"))
                {
                    TraceFactory.Logger.Info("Successfully installed the self-signed certificate through Web UI.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                TraceFactory.Logger.Debug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                return false;
            }
            finally
            {
                if (_adapter.IsElementPresent("Install_Ok"))
                {
                    _adapter.Click("Install_Ok");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }

                // When install is unsuccessful/ error, Cancel button needs to be clicked maximum of two times to exit the wizard properly
                if (_adapter.IsElementPresent("Cancel"))
                {
                    _adapter.Click("Cancel");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }

                if (_adapter.IsElementPresent("Cancel"))
                {
                    _adapter.Click("Cancel");
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            }
        }

        /// <summary>
        /// Install self signed certificate for products that support certificate store
        /// </summary>
        /// <param name="algorithm"><see cref="SignatureAlgorithm"/></param>
        /// <param name="keyLength"><see cref="RSAKeyLength"/></param>
        /// <param name="exportPrivateKey">True to export private key.</param>
        /// <param name="validityPeriod">Validity period in no of days.</param>
        /// <returns>True if the installation is successful, else false.</returns>
        private bool InstallSelfSignedCertificateWithUnification(SignatureAlgorithm algorithm, RSAKeyLength keyLength, bool exportPrivateKey, int validityPeriod)
        {
            if (validityPeriod < CERTIFCATE_MINIMUM_VALIDITY_PERIOD || validityPeriod > CERTIFCATE_MAXIMUM_VALIDITY_PERIOD)
            {
                TraceFactory.Logger.Info("Certificate validity period is out of range. It should be between {0} - {1}".FormatWith(CERTIFCATE_MINIMUM_VALIDITY_PERIOD, CERTIFCATE_MAXIMUM_VALIDITY_PERIOD));
                return false;
            }

            try
            {
                TraceFactory.Logger.Info("Installing Self Signed Certificate.");

                _adapter.Navigate("CertificateManagement");

                _adapter.Click("Create_SelfSignedIdCert");

                if (keyLength == RSAKeyLength.Rsa1024)
                {
                    _adapter.SelectByValue("KeyLength", "Rsa1024");
                }
                else if (keyLength == RSAKeyLength.Rsa2048)
                {
                    _adapter.SelectByValue("KeyLength", "Rsa2048");
                }

                if (SignatureAlgorithm.Sha224.Equals(algorithm))
                {
                    TraceFactory.Logger.Info("{0} algorithm is not supported.".FormatWith(algorithm));
                    //return false;
                }

                _adapter.SelectByValue("SignatureAlgorithm", Enum<SignatureAlgorithm>.Value(algorithm));

                if (exportPrivateKey)
                {
                    _adapter.Check("Export_PrivateKey");
                }
                else
                {
                    _adapter.Uncheck("Export_PrivateKey");
                }

                // Read the start date and add the validity period to obtain the expiry date
                DateTime startDate = DateTime.Parse(_adapter.GetText("StartDate"));
                DateTime newdate = startDate.AddDays(validityPeriod);
                string newdatetext = newdate.Date.ToShortDateString().FormatWith("M/dd/yyyy");


                //string neha = newdate.ToString("MM/dd/yyyy");
               
             
             //   MessageBox.Show("checkToshortDate");
                Thread.Sleep(TimeSpan.FromSeconds(30));
                _adapter.SetText("ExpiryDate", newdatetext);
                TraceFactory.Logger.Info("Expiry date : {0}".FormatWith(newdatetext));
               // MessageBox.Show("check date ");
                _adapter.SetText("ExpiryDate", newdate.Month + "/" + newdate.Day + "/" + newdate.Year);
             //   MessageBox.Show("check date 2");

                //string exdate = startDate.AddDays(validityPeriod).ToString("MM/dd/yyyy");
                //string newdat = exdate.FormatWith("M/dd/yyyy");
                //TraceFactory.Logger.Info("Expiry date : {0}".FormatWith(exdate.FormatWith("MM/dd/yyyy")));

                _adapter.Click("Create_Ok");

                _adapter.Click("Confirm_Installation");
                //Chnaged sleep to 1 min from 3 mins, as below message immidiatly appears 
                Thread.Sleep(TimeSpan.FromMinutes(1));
            
                if (!_adapter.SearchText("The operation was completed successfully"))
                {
                    if (_adapter.IsElementPresent("Create_Ok"))
                    {
                        TraceFactory.Logger.Info("Error occured, Trying once again");
                        _adapter.Click("Create_Ok");
                        _adapter.Click("Confirm_Installation");
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        if (_adapter.SearchText("The operation was completed successfully") || _adapter.SearchText("The operation was executed successfully"))
                        {
                            TraceFactory.Logger.Info("Successfully installed the self-signed certificate through Web UI.");
                            return true;
                        }
                    }
                        TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                        return false;
                  
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully installed the self-signed certificate through Web UI.");
                   // return true;
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to install the self-signed certificate through Web UI.");
                TraceFactory.Logger.Debug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                return false;
            }
            finally
            {
                // To exit the wizard properly in case of any errors
                if (_adapter.IsElementPresent("Create_Back"))
                {
                    _adapter.Click("Create_Back");
                }
            }
        }

        /// <summary>
        /// Get self signed certificate details
        /// </summary>
        /// <returns><see cref="CertificateDetails"/></returns>
        public CertificateDetails GetSelfSignedCertificateDetails()
        {
            /* Get self signed certificate details, the raw values to be searched for fetching the corresponding values
             * --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*
             * Product Family       : Serial No     |  Subject  |  Issuer   | Valid To  | Valid From    | Private key exportable            | Key Length                | Signature Algorithm
             * --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*
             * VEP  Unification     : Serial Number | Subject   | Issuer    | Valid To  | Valid From    | Private Key Exportable            | Public Key Length         | Signature Algorithm
             *      Non Unification : Serial Number | Subject   | Issuer    | Issued On | Expires On    | Allow Private Key to be Exported  | Public Key                | Signature Algorithm
             * TPS                  : Serial Number | Subject   | Issuer    | Issued On | Expires On    | Allow private key to be exported  | Public Key                | Signature Algorithm
             * InkJet               : Serial Number | Subject   | Issuer    | Issued On | Expires On    | Allow Private Key to be Exported  | Public Key                | Signature Algorithm
             * ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*
             * */

            CertificateDetails details = new CertificateDetails();
            bool isCertificateStore = false;

            try
            {
                string serialNumber = "Serial Number";
                string subject = "Subject";
                string issuer = "Issuer";
                string startDate = "Issued On";
                string endDate = "Expires On";
                string privateKeyExportable = "Allow Private Key to be Exported";
                string keyLength = "Public Key";
                string signatureAlgorithm = "Signature Algorithm";

                _adapter.Navigate("CA_InstallCertificate");

                Collection<Collection<string>> certificateDetails;

                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("JDConfigure"))
                {
                    isCertificateStore = true;

                    _adapter.Navigate("CertificateManagement");

                    // Select the self signed certificate and click on view details
                    Collection<CertificatesTableRaw> certTable = GetInstalledCertificatesWithUnification();

                    if (certTable.Count == 0)
                    {
                        TraceFactory.Logger.Info("No certificates are available in certificate store.");
                        return details;
                    }

                    CertificatesTableRaw selfSignedCertificate = certTable.Where(x => (x.Type == CertificateType.SelfSignedIdentity)).FirstOrDefault();

                    if (!string.IsNullOrEmpty(selfSignedCertificate.selectElementId))
                    {
                        TraceFactory.Logger.Debug(selfSignedCertificate.ToString());
                        _adapter.Check(selfSignedCertificate.selectElementId, useSitemapId: false);
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Self signed id certificate is not available in certificate store.");
                        return details;
                    }

                    _adapter.Click("View_Certificate_Details");

                    certificateDetails = _adapter.GetTable("CertificateDetailsTable");

                    startDate = "Valid From";
                    if (!(_adapter.Settings.ProductType == PrinterFamilies.VEP))
                    {
                        endDate = "Valid To";
                    }
                    else
                    {
                        endDate = "Expiration Date";
                    }
                    privateKeyExportable = "Private Key Exportable";
                    keyLength = "Public Key Length";

                    details.IssuedTo = certificateDetails.FirstOrDefault(x => x[0].Contains(subject))[1];
                    details.Issuer = certificateDetails.FirstOrDefault(x => x[0].Contains(issuer))[1];
                    TraceFactory.Logger.Info("Done");
                }
                else
                {
                    TraceFactory.Logger.Info("NO Supports unification");
                    _adapter.Click("JetDirect_View");
                    certificateDetails = _adapter.GetTable("Jetdirect_Certificate_Details", includeHeader: false);
                    //If not Ink then only do this, since INK does not have subject and Issues is spread in two rows, will be taken care later
                    if (!(PrinterFamilies.InkJet == _adapter.Settings.ProductType))
                    {
                        // For non unification products, issuer or subject details are spread in 4 rows. So taking index of the issuer/ subject column and reading the values of 4 subsequent row details to obtain the details.
                        int issuerColumnIndex = certificateDetails.IndexOf(certificateDetails.FirstOrDefault(x => x[0].Contains(issuer)));

                    StringBuilder issuerDetails = new StringBuilder();

                    for (int i = issuerColumnIndex + 1; i <= issuerColumnIndex + 4; i++)
                    {
                        issuerDetails.Append(string.Join("", certificateDetails[i]));
                        issuerDetails.Append("\n");
                    }

                    details.Issuer = issuerDetails.ToString();

                    StringBuilder subjectDetails = new StringBuilder();

                    for (int i = issuerColumnIndex + 1; i <= issuerColumnIndex + 4; i++)
                    {
                        subjectDetails.Append(string.Join("", certificateDetails[i]));
                        subjectDetails.Append("\n");
                    }

                        details.IssuedTo = subjectDetails.ToString();
                    }
                    else
                    {
                        privateKeyExportable = "Private Key";
                        signatureAlgorithm = "Signature";
                    }

                }
                string keyLengthValue = certificateDetails.FirstOrDefault(x => x[0].Contains(keyLength))[1];
                details.KeyLength = keyLengthValue.Contains("2048 bit") ? 2048 : (keyLengthValue.Contains("1024 bit") ? 1024 : -1);
                details.SerialNumber = certificateDetails.FirstOrDefault(x => x[0].Contains(serialNumber))[1];
                details.StartDate = DateTime.Parse(certificateDetails.FirstOrDefault(x => x[0].Contains(startDate))[1].Replace("UTC", string.Empty));
                details.ExpiryDate = DateTime.Parse(certificateDetails.FirstOrDefault(x => x[0].Contains(endDate))[1].Replace("UTC", string.Empty));
                details.PrivateKeyExportable = (certificateDetails.FirstOrDefault(x => x[0].Contains(privateKeyExportable, StringComparison.CurrentCultureIgnoreCase))[1]).EqualsIgnoreCase("No") || (certificateDetails.FirstOrDefault(x => x[0].Contains(privateKeyExportable, StringComparison.CurrentCultureIgnoreCase))[1]).EqualsIgnoreCase("Unexportable") ? false : true;
                details.SignatureAlgorithm = new Oid("", certificateDetails.FirstOrDefault(x => x[0].Contains(signatureAlgorithm))[1]);

                return details;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("An error occurred while fetching the certificate details.");
                TraceFactory.Logger.Debug(ex.Message);
                return details;
            }
            finally
            {
                if (isCertificateStore)
                {
                    if (_adapter.IsElementPresent("Back"))
                    {
                        _adapter.Click("Back");
                    }
                }
                else
                {
                    if (_adapter.IsElementPresent("JetDirect_Details_Ok"))
                    {
                        _adapter.Click("JetDirect_Details_Ok");
                    }
                }
            }
        }

        public CertificateDetails GetIdCertificateDetails()
        {
            _adapter.Click("JetDirect_View");
            Collection<Collection<string>> certificateDetails = _adapter.GetTable("Jetdirect_Certificate_Details", false);

            CertificateDetails details = new CertificateDetails();

            return details;
        }

        /// <summary>
        /// Check Intermediate CA Option is enabled/disabled
        /// </summary>
        public bool GetIntermediateCA()
        {
            TraceFactory.Logger.Info("Checking the Intermediate CA Option");

            _adapter.Navigate("CA_InstallCertificate");
            _adapter.Click("CAConfigure");
            _adapter.Click("InstallCA");
            _adapter.Click("CertificateNext");

            bool isChecked = _adapter.IsChecked("IntermediateCA");

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Intermediate CA", isChecked ? "enable" : "disable"));
            return isChecked;
        }

        /// <summary>
        /// Validates the Authentication mode configured on the printer after successful 802.1X Authentication.
        /// This method applies to VEP and LFP products
        /// </summary>
        /// <param name="mode"><see cref="AuthenticationMode"/></param>
        /// <returns>true if validation is successful, else false</returns>
        public bool ValidateConfiguredAuthentication(AuthenticationMode mode)
        {
            string searchValue = mode == (AuthenticationMode.EAPTLS) ? "EAP-TLS" : "EAP-PEAP";
            return ValidateConfiguredAuthentication(searchValue);
        }

        /// <summary>
        /// Validates the Authentication mode configured on the printer after successful 802.1X Authentication.
        /// This method applies to VEP and LFP products
        /// </summary>
        /// <param name="searchValue">Authentication modes such as EAPTLS, PEAP, LEAP etc.</param>
        /// <returns>true if validation is successful, else false</returns>
        public bool ValidateConfiguredAuthentication(string searchValue)
        {
            if ((_adapter.Settings.ProductType == PrinterFamilies.TPS) || (_adapter.Settings.ProductType == PrinterFamilies.InkJet))
            {
                TraceFactory.Logger.Info("Configured authentication mode is not available from web UI.");
                return true;
            }

            TraceFactory.Logger.Info("Navigating to Configuration Page for verifying configured authentication mode.");
            _adapter.Navigate("Configuration_Page");

            if (_adapter.SearchText(searchValue))
            {
                TraceFactory.Logger.Info("Successfully validated {0} from configuration page.".FormatWith(searchValue));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validate {0} from configuration page.".FormatWith(searchValue));
                return false;
            }
        }

        public bool GenerateIdCertificate(string serverIp, SignatureAlgorithm algorithm, out string certificatePath, bool exportPrivateKey = false,
                                          RSAKeyLength keyLength = RSAKeyLength.Rsa2048, string fqdn = null)
        {
            certificatePath = string.Empty;
            try
            {
                string certificateRequest;

                if (!Instance().CreateIDCertificateRequest(algorithm, out certificateRequest, exportPrivateKey, keyLength, fqdn))
                {
                    return false;
                }

                PluginSupport.Connectivity.RadiusServer.Encoding encoding = ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()) ? PluginSupport.Connectivity.RadiusServer.Encoding.Base64 : PluginSupport.Connectivity.RadiusServer.Encoding.DER;
                string certificateTemplateName = keyLength == RSAKeyLength.Rsa1024 ? CERTIFICATE_TEMPLATE_NAME_1024 : CERTIFICATE_TEMPLATE_NAME_2048;

                if (!RadiusApplication.GenerateCertificate(certificateRequest, serverIp, CtcUtility.SERVER_USERNAME, CtcUtility.SERVER_PASSWORD, out certificatePath, certificateTemplateName, encoding))
                {
                    return false;
                }

                string serverCertificatePath = @"\\{0}\Dynamic Certificates".FormatWith(serverIp);

                using (UserImpersonator localUser = new UserImpersonator())
                {
                    // TODO: Get the user name and password from base plugin.
                    localUser.Impersonate(CtcUtility.SERVER_USERNAME, CtcUtility.SERVER_DOMAIN, CtcUtility.SERVER_PASSWORD);

                    if (Directory.Exists(serverCertificatePath))
                    {
                        foreach (FileInfo file in (new DirectoryInfo(serverCertificatePath).GetFiles()))
                        {
                            TraceFactory.Logger.Info("File: {0} is deleted".FormatWith(file.Name));
                            file.Delete();
                        }

                        // Copy the certificate to \\<server IP>\Dynamic Certificate folder.
                        File.Copy(certificatePath, Path.Combine(serverCertificatePath, Path.GetFileName(certificatePath)));

                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("The path:{0} does not exist".FormatWith(serverCertificatePath));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
        }

        #endregion Certificates

        #region Private Methods

        /// <summary>
        /// Install CA certificate
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="validate"></param>
        /// <returns>true if CA certificate installed successfully, false otherwise</returns>
        private bool InstallCACertificateWithUnification(string certificatePath, bool validate = true)
        {
            try
            {
                if (VerifyCertificateAvailability(certificatePath))
                {
                    TraceFactory.Logger.Info("Specified certificate: {0} is already installed.".FormatWith(certificatePath));
                    return true;
                }
                TraceFactory.Logger.Info("Printer is TNT product, Supports Certificate Unification");
                _adapter.Navigate("CertificateManagement");

              //  string successMessage = "The operation was completed successfully";

                // Unification is supported only for VEP and Inkjet. Installation steps differs with these families.
                // For Inkjet, Import button needs to be clicked to navigate to install CA certificate
                if (ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
                {
                    _adapter.Click("Import");
                  //  successMessage = "The changes have been updated successfully";
                }

                _adapter.SetBrowseControlText("Ca_Certificate_File", certificatePath);

                // If browser loses connectivity during installation of certificate, close the browser and try to open with https
                try
                {
                    _adapter.Click("Install_Ca_Certificate");
                    if (ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
                    {
                        // Additional OK button to be clicked for Inkjet printers
                        _adapter.Click("Confirm_Install_Ca_Certificate");
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }

                }
                catch
                {
                    StopAdapter();
                    _adapter.Start();
                }

                if (validate)
                {
                    // Wait for Certificate installation completion
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    // TODO:
                    // Validate success message?
                    // Click OK button for Inkjet
                    if(SearchTextInPage("The operation was completed successfully")|| SearchTextInPage("The operation was executed successfully") || SearchTextInPage("The changes have been updated successfully"))
                    {
                        TraceFactory.Logger.Info("Successfully installed the CA Certificate");
                       // return true;
                    }
                    else if (SearchTextInPage("Failed to install the file because the selected file is not a valid CA certificate."))
                    {
                        TraceFactory.Logger.Info("Certificate is not Supported or FIPS is enanabled on Printer");
                        //return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to install CA Certificate");
                        return false;
                    }

                    return VerifyCertificateAvailability(certificatePath);
                }
                else
                {
                    TraceFactory.Logger.Info("Validation is turned off. Assuming certificate installation to be successful.");
                    return true;
                }
            }
            catch (Exception installException)
            {
                TraceFactory.Logger.Debug(installException.Message);
                return false;
            }
        }

        /// <summary>
        /// Install ID certificate
        /// Note: Applicable only for VEP. Other products can install only 1 ID certificate
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="password">Password for ID Certificate</param>
        /// <param name="validate"></param>
        /// <returns>true if ID Certificate is installed successfully, false otherwise</returns>
        private bool InstallIDCertificateWithUnification(string certificatePath, string password, bool validate = true, bool enablePrivateKey = false)
        {
            if (!ProductFamilies.VEP.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
            {
                throw new NotSupportedException("ID Certificate Unification installation is applicable only for VEP products. Current family: {0}.".FormatWith(_adapter.Settings.ProductType));
            }

            // If Certificate provided is already installed, select as default ID certificate
            if (VerifyCertificateAvailability(certificatePath, isCACertificate: false, idCertificatePassword: password))
            {
                TraceFactory.Logger.Info("Specified certificate: {0} is already installed.".FormatWith(certificatePath));

                if (!SelectCertificate(certificatePath, isCACertificate: false, idCertificatePassword: password))
                {
                    TraceFactory.Logger.Info("Unable to select certificate from installed certificates.");
                    return false;
                }

                _adapter.Click("Network_Identity");

                if (_adapter.IsElementPresent("Confirm_Installation"))
                {
                    TraceFactory.Logger.Info("Confirm button clicked");
                    _adapter.Click("Confirm_Installation");
                }

                // Browser connectivity is lost for few seconds
                Thread.Sleep(TimeSpan.FromSeconds(30));

                return true;
            }

            _adapter.Navigate("CertificateManagement");

            /* ID Certificate Installation can be of 2 types:
			 * 1. Install ID Certificate which is directly created on server, requires a password for installation.
			 * 2. Create a certificate request on the printer then create a ID certificate on server using this request. No password required.
			*/

            if (string.IsNullOrEmpty(password))
            {
                _adapter.Check("Install_Id_Signed_Certificate");
                _adapter.SetBrowseControlText("Install_Id_Signed_File_Path", certificatePath);
            }
            else
            {
                _adapter.Check("Install_Id");
                _adapter.SetBrowseControlText("Id_Certificate_File", certificatePath);
                _adapter.SetText("Id_Certificate_Password", password);
                if (enablePrivateKey)
                {
                    _adapter.Check("Export_PrivateKey");
                }
                else
                {
                    _adapter.Uncheck("Export_PrivateKey");
                }
                
            }

            _adapter.Click("Install_Id_Certificate");
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (_adapter.SearchText("The Jetdirect certificate has been successfully updated") || _adapter.SearchText("The printer certificate has been updated") || _adapter.SearchText("The changes have been updated successfully") || _adapter.SearchText("The operation was completed successfully."))
            {
                TraceFactory.Logger.Info("The Jet direct certificate has been successfully updated");
                
            }
            else if (_adapter.SearchText("do not comply with FIPS-140"))
            {
                TraceFactory.Logger.Info("The cryptographic algorithms used in the ID or CA certificate do not comply with FIPS-140");
                return false;
            }
            else if (_adapter.SearchText("password"))
            {
                TraceFactory.Logger.Info("Incorrect Password");
                return false;
            }
            else
            {
                TraceFactory.Logger.Info("Error in installing Certificate");
                return false;
            }

            if (!VerifyCertificateAvailability(certificatePath, isCACertificate: false, idCertificatePassword: password))
            {
                TraceFactory.Logger.Info("Failed to install ID certificate: {0}".FormatWith(certificatePath));
                return false;
            }

            if (!SelectCertificate(certificatePath, isCACertificate: false, idCertificatePassword: password))
            {
                TraceFactory.Logger.Info("Unable to select certificate from installed certificates.");
                return false;
            }

            // If browser loses connectivity during installation of certificate, close the browser and try to open with https
            // This code is not used  
            //try
            //{
            //    _adapter.Click("Network_Identity");

            //    if (_adapter.IsElementPresent("Confirm_Installation"))
            //    {
            //        _adapter.Click("Confirm_Installation");
            //    }
            //}
            //catch
            //{
            //    StopAdapter();
            //    _adapter.Start();
            //}

            if (validate)
            {
                // Browser connectivity is lost for few seconds
                Thread.Sleep(TimeSpan.FromSeconds(30));

                return VerifyCertificateAvailability(certificatePath, isCACertificate: false, idCertificatePassword: password);
            }
            else
            {
                TraceFactory.Logger.Info("Validation is turned off. Assuming certificate installation to be successful.");
                return true;
            }
        }

        /// <summary>
        /// Uninstall CA Certificate
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <returns>true if CA certificate is uninstalled successfully, false otherwise</returns>
        private bool UnInstallCACertificateWithUnification(string certificatePath)
        {
            try
            {
                if (string.IsNullOrEmpty(certificatePath))
                {
                    TraceFactory.Logger.Info("Certificate path can not be empty.");
                    return false;
                }

                if (!VerifyCertificateAvailability(certificatePath))
                {
                    TraceFactory.Logger.Info("Unable to find specified certificate: {0}".FormatWith(certificatePath));
                    return false;
                }

                if (!SelectCertificate(certificatePath))
                {
                    TraceFactory.Logger.Info("Unable to select specified certificate: {0} from installed certificates".FormatWith(certificatePath));
                    return false;
                }

                string successMessage = "The operation was completed successfully";

                _adapter.Click("Remove_Certificate");

                // Confirmation page shows up only for VEP printers
                if (_adapter.IsElementPresent("Confirm_Deletion"))
                {
                    _adapter.Click("Confirm_Deletion");
                }

                // Status message on certificate uninstall is shown only for VEP printers
                if (PrinterFamilies.VEP == _adapter.Settings.ProductType)
                {
                    SearchTextInPage(successMessage);
                }

                return !VerifyCertificateAvailability(certificatePath);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception Details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Uninstall all Certificates from certificate store.
        /// </summary>
        /// <returns>true if all certificate except the default are uninstalled successfully, false otherwise</returns>
        public bool UnInstallAllCertificates()
        {
            try
            {
                TraceFactory.Logger.Info("Deleting all Certificates on the printer.");

                _adapter.Navigate("CA_InstallCertificate");

                if (!_adapter.IsElementPresent("CAConfigure"))
                {
                    return UnInstallAllCertificatesWithUnification();
                }
                else
                {
                    TraceFactory.Logger.Debug("Id certificate can be deleted only from the certificate store.");
                    return UnInstallCACertificate();
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception Details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Uninstall all Certificates from certificate store.
        /// </summary>
        /// <returns>true if all certificate except the default are uninstalled successfully, false otherwise</returns>
        public bool UnInstallAllCertificatesWithUnification()
        {
            try
            {
                Collection<CertificatesTableRaw> certTable = GetInstalledCertificatesWithUnification();

                if (certTable.Count == 0)
                {
                    TraceFactory.Logger.Info("No certificates are available to be deleted.");
                    return true;
                }
                TraceFactory.Logger.Info("Total Certificates found : {0}".FormatWith(certTable.Count));
                TraceFactory.Logger.Info("Making Self signed certificate as ID certificate before deleting certs");
                CertificatesTableRaw selfSignedCertificate = certTable.Where(x => (x.Type == CertificateType.SelfSignedIdentity)).FirstOrDefault();

                if (!string.IsNullOrEmpty(selfSignedCertificate.selectElementId))
                {
                    TraceFactory.Logger.Debug(selfSignedCertificate.ToString());
                    _adapter.Click(selfSignedCertificate.selectElementId, useSitemapId: false);

                    if (!selfSignedCertificate.IsNetworkIdentity)
                    {
                        // Making the default self signed id certificate as network Identity before deleting all the certificates.
                        // If browser loses connectivity during installation of certificate, close the browser and try to open with https
                        try
                        {
                            _adapter.Click("Network_Identity");
                            Thread.Sleep(TimeSpan.FromSeconds(30));
                            if (_adapter.IsElementPresent("Confirm_Installation"))
                            {
                                _adapter.Click("Confirm_Installation");
                            }
                            TraceFactory.Logger.Info("Self Signed certificate marked as Network Identity");
                        }
                        catch
                        {
                            StopAdapter();
                            _adapter.Start();
                        }
                    }
                }

                TraceFactory.Logger.Info("Reading the table again and exluding Self signed CA and Self signed ID certificate");
                // Read the table again to reflect the modified properties in the above step.
                certTable = GetInstalledCertificatesWithUnification();
                Collection<CertificatesTableRaw> excludeCerts = new Collection<CertificatesTableRaw>(certTable.Where(x => (x.Type == CertificateType.SelfSignedCA || x.Type == CertificateType.SelfSignedIdentity)).ToList());

                int excludedCertificatesCount = excludeCerts.Count;

                if (certTable.Count == 0 || certTable.Count == excludedCertificatesCount)
                {
                    TraceFactory.Logger.Info("No certificates are available in the certificate store which can be deleted.");
                    return true;
                }
                TraceFactory.Logger.Info("Certificates found: {0}".FormatWith(certTable.Count));
                TraceFactory.Logger.Info("Excluded certs : {0}".FormatWith(excludeCerts.Count));
                
                for (int i = certTable.Count - 1; i >= 0; i--)
                {
                    if (excludeCerts.Contains(certTable[i]))
                    {
                        continue;
                    }

                    _adapter.Click(certTable[i].selectElementId, useSitemapId: false);
                    _adapter.Click("Remove_Certificate");

                    // Confirmation page shows up only for VEP printers
                    if (_adapter.IsElementPresent("Confirm_Deletion"))
                    {
                        _adapter.Click("Confirm_Deletion");
                    }
                    if (_adapter.IsElementPresent("Confirm_Installation"))
                    {
                      
                        _adapter.Click("Confirm_Installation");
                    }

                    // Status message on certificate uninstall is shown only for VEP printers
                    if (PrinterFamilies.VEP == _adapter.Settings.ProductType)
                    {
                        if (!SearchTextInPage("The operation was executed successfully"))
                        {
                            TraceFactory.Logger.Info("Failed to delete the certificate.");
                            return false;
                        }
                    }
                }
                TraceFactory.Logger.Info("Reading the table again afte deleting the certs");
                certTable = GetInstalledCertificatesWithUnification();

                if (certTable.Count == 0)
                {
                    TraceFactory.Logger.Info("Certificates are successfully deleted.");
                    return true;
                }
                else
                {
                    foreach (var item in certTable)
                    {
                        if (!excludeCerts.Any(x => x.Equals(item)))
                        {
                            TraceFactory.Logger.Info("Failed to delete the certificate");
                            return false;
                        }
                    }

                    TraceFactory.Logger.Info("Certificates are successfully deleted.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception Details: {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Uninstall ID Certificate
        /// Note: Applicable only for VEP. Other products have only 1 ID certificate
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="idCertificatePassword">ID Certificate Password</param>
        /// <returns>true if uninstalled ID certificate successfully, false otherwise</returns>
        private bool UnInstallIDCertificateWithUnification(string certificatePath, string idCertificatePassword = null)
        {
            if (!ProductFamilies.VEP.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
            {
                throw new NotSupportedException("ID Certificate Unification is applicable only for VEP products. Current family: {0}.".FormatWith(_adapter.Settings.ProductType));
            }

            if (!VerifyCertificateAvailability(certificatePath, isCACertificate: false, idCertificatePassword: idCertificatePassword))
            {
                TraceFactory.Logger.Info("Unable to find specified certificate: {0}".FormatWith(certificatePath));
                return false;
            }

            // Make sure default IA certificate is selected before deleting the actual 1
            if (!SelectCertificate(certificatePath, isCACertificate: false, isSelfSignedCertificate: true, idCertificatePassword: idCertificatePassword))
            {
                TraceFactory.Logger.Info("Unable to select self signed ID certificate.");
                return false;
            }

            string successMessage = "The operation was completed successfully";

            _adapter.Click("Network_Identity");
            Thread.Sleep(TimeSpan.FromSeconds(30));
            if (_adapter.IsElementPresent("Confirm_Installation"))
            {
                _adapter.Click("Confirm_Installation");
            }

            // Browser connectivity is lost for few seconds
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (!SelectCertificate(certificatePath, isCACertificate: false, idCertificatePassword: idCertificatePassword))
            {
                TraceFactory.Logger.Info("Unable to select specified certificate: {0} from installed certificates".FormatWith(certificatePath));
                return false;
            }

            _adapter.Click("Remove_Certificate");

            // Confirmation page shows up only for VEP printers
            if (_adapter.IsElementPresent("Confirm_Deletion"))
            {
                _adapter.Click("Confirm_Deletion");
            }

            SearchTextInPage(successMessage);

            return !VerifyCertificateAvailability(certificatePath, isCACertificate: false, idCertificatePassword: idCertificatePassword);
        }

        /// <summary>
        /// Verify whether certificate exists.
        /// Note: this implementation works only for builds supporting certificate unification.
        /// VEP: Both CA and ID certificates are shown in table.
        /// Inkjet: Only CA certificates are shown.
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="isCACertificate">true for verifying CA certificate, false for ID certificate</param>
        /// <param name="idCertificatePassword">ID Certificate Password</param>
        /// <returns>true if certificate is available in installed certificates, false if not found</returns>
        private bool VerifyCertificateAvailability(string certificatePath, bool isCACertificate = true, string idCertificatePassword = null)
        {
            // Table header names differ based on different families.
            string issuedTocolumnName = "Issued To";
            string issuedByColumnName = "Issued By";
            string expiryDateColumnName = "Expiration Date";
            string certificateTypeColumnName = "Certificate Type";
            string certificateTypeSearchItem = "CA Certificate";

            if (!isCACertificate)
            {
                certificateTypeSearchItem = "Identity Certificate";
            }

            if (ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
            {
                // Column header are different for Inkjet printers
                issuedByColumnName = "Issuer";
                expiryDateColumnName = "Expires On";
                //certificateTypeSearchItem = "CA:TRUE";
                certificateTypeSearchItem = "Root CA Certificate";

            }

            int[] columnIndex = { 0 };

            // Browser doesn't load soon after installing. Waiting for sometime, closing and opening the browser
            Thread.Sleep(TimeSpan.FromSeconds(10));
            StopAdapter();
            _adapter.Start();

            // Get Certificate table details to validate if certificate was installed successfully
            _adapter.Navigate("CertificateManagement");
            Collection<Collection<string>> certificates = _adapter.GetTable("Certificates_Table", columnIndex: columnIndex, returnValue: false);

            // Gets Certificate properties column numbers to verify certificate installation
            int issuedToColumnIndex = certificates[0].IndexOf(certificates[0].Where(x => x.Contains(issuedTocolumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int issuedByColumnIndex = certificates[0].IndexOf(certificates[0].Where(x => x.Contains(issuedByColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int expiryDateColumnIndex = certificates[0].IndexOf(certificates[0].Where(x => x.Contains(expiryDateColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int certificateTypeColumnIndex = certificates[0].IndexOf(certificates[0].Where(x => x.Contains(certificateTypeColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            TraceFactory.Logger.Info("Getting the certificate details :");
            // Get certificate properties from original certificate used for installation
            CertificateDetails details = CertificateUtility.GetCertificateDetails(certificatePath, idCertificatePassword);

            // Validating Issued By, Issued To, Expiration date and Certificate Type columns to ensure that the correct certificate is selected.
            // For Inkjet SOL, the "certificateTypeColumnIndex" will be '-1' as it is not present. So removing the same from validation if it is not present.
            IEnumerable<Collection<string>> data = certificates.Where(x => x[issuedByColumnIndex].Contains(details.IssuedBy.Trim()) && x[issuedToColumnIndex].Contains(details.IssuedTo.Trim())
                                        && DateTime.Parse(x[expiryDateColumnIndex]).ToLocalTime().ToString("yyyy-MM-dd").Equals(details.ExpiryDate.ToLocalTime().ToString("yyyy-MM-dd"))
                                        && (certificateTypeColumnIndex == -1 || x[certificateTypeColumnIndex].Contains(certificateTypeSearchItem)));

            return data.Any();
		}

        /// <summary>
        /// Select Certificate from Installed certificates
        /// </summary>
        /// <param name="certificatePath">Certificate Path</param>
        /// <param name="isCACertificate">true for selecting CA certificate, false for ID certificate</param>
        /// <param name="isSelfSignedCertificate">true for selecting Self signed certificate, false otherwise</param>
        /// <param name="idCertificatePassword">ID Certificate Password</param>
        /// <returns>true if Certificate is selected, false otherwise</returns>
        private bool SelectCertificate(string certificatePath = null, bool isCACertificate = true, bool isSelfSignedCertificate = false, string idCertificatePassword = null)
        {
            if (string.IsNullOrEmpty(certificatePath) && !isSelfSignedCertificate)
            {
                TraceFactory.Logger.Info("Invalid certificate path.");
                return false;
            }

            // Get certificate properties from original certificate used for installation
            CertificateDetails details = CertificateUtility.GetCertificateDetails(certificatePath, idCertificatePassword);
            return SelectCertificate(details, isCACertificate, isSelfSignedCertificate, idCertificatePassword);
        }

        /// <summary>
        /// Select Certificate from Installed certificates
        /// </summary>
        /// <param name="certificateDetails">Certificate Path</param>
        /// <param name="isCACertificate">true for selecting CA certificate, false for ID certificate</param>
        /// <param name="isSelfSignedCertificate">true for selecting Self signed certificate, false otherwise</param>
        /// <param name="idCertificatePassword">ID Certificate Password</param>
        /// <returns>true if Certificate is selected, false otherwise</returns>
        private bool SelectCertificate(CertificateDetails certificateDetails, bool isCACertificate = true, bool isSelfSignedCertificate = false, string idCertificatePassword = null)
        {
            string certificateTypeSearchItem = string.Empty;

            if (isSelfSignedCertificate)
            {
                if (!isCACertificate)
                {
                    certificateTypeSearchItem = "Self-Signed Identity Certificate";
                }
                else
                {
                    certificateTypeSearchItem = "Self-Signed CA Certificate";
                }
            }
            else
            {
                if (!isCACertificate)
                {
                    certificateTypeSearchItem = "Identity Certificate";
                }
                else
                {
                    certificateTypeSearchItem = "CA Certificate";
                }
            }

            // Table header names differ based on different families.
            string issuedTocolumnName = "Issued To";
            string issuedByColumnName = "Issued By";
            string expiryDateColumnName = "Expiration Date";
            string certificateTypeColumnName = "Certificate Type";

            if (ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
            {
                // Column header are different for Inkjet printers
                issuedByColumnName = "Issuer";
                expiryDateColumnName = "Expires On";
                // Certificate Type column has different content
                //certificateTypeSearchItem = "CA:TRUE";
                certificateTypeSearchItem = "Root CA Certificate";
            }

            int[] radioButtonColumnIndex = { 0 };

            // Get Certificate table details
            _adapter.Navigate("CertificateManagement");
            Collection<Collection<string>> installedCertificates = _adapter.GetTable("Certificates_Table", columnIndex: radioButtonColumnIndex, returnValue: false);

            // Gets Certificate properties column numbers to select certificate
            int issuedToColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].Where(x => x.Contains(issuedTocolumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int issuedByColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].Where(x => x.Contains(issuedByColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int expiryDateColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].Where(x => x.Contains(expiryDateColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            int certificateTypeColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].Where(x => x.Contains(certificateTypeColumnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            List<Collection<string>> selectedCertificate;

            if (isSelfSignedCertificate)
            {
                selectedCertificate = installedCertificates.Where(x => x[certificateTypeColumnIndex].Contains(certificateTypeSearchItem)).ToList();
            }
            else
            {
                selectedCertificate = installedCertificates.Where(x => x[issuedByColumnIndex].Contains(certificateDetails.IssuedBy.Trim()) && x[issuedToColumnIndex].Contains(certificateDetails.IssuedTo.Trim())
                                        && DateTime.Parse(x[expiryDateColumnIndex]).ToLocalTime().ToString("yyyy-MM-dd").Equals(certificateDetails.ExpiryDate.ToLocalTime().ToString("yyyy-MM-dd"))
                                        && (certificateTypeColumnIndex != -1 ? x[certificateTypeColumnIndex].Contains(certificateTypeSearchItem) : true)).ToList();
            }

            if (selectedCertificate.Count != 1)
            {
                TraceFactory.Logger.Info("Invalid/ No {0}certificate found.".FormatWith(isCACertificate ? string.Empty : "Self Signed "));
                TraceFactory.Logger.Debug("Certificate count: {0}".FormatWith(selectedCertificate.Count));
                return false;
            }

            _adapter.Check(selectedCertificate[0][0], false);

            return true;
        }

        /// <summary>
        /// Gets the installed Certificates on the printer. Applicable to printers that support unification.
        /// </summary>
        /// <returns><see cref="CertificatesTableRaw"/></returns>
        private Collection<CertificatesTableRaw> GetInstalledCertificatesWithUnification()
        {
            // Table header names differ based on different families.
            string issuedTocolumnName = string.Empty;
            string issuedByColumnName;
            string expiryDateColumnName;
            string certificateTypeColumnName = string.Empty;
            string certificateUsageColumnName = string.Empty;

            //PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _adapter.Settings.ProductType, true);
            if (ProductFamilies.InkJet.ToString().EqualsIgnoreCase(_adapter.Settings.ProductType.ToString()))
            {
                // Column header are different for Inkjet printers
                issuedByColumnName = "Issuer";
                expiryDateColumnName = "Expires On";
                // Certificate Type column has different content
            }
            else
            {
                issuedTocolumnName = "Issued To";
                issuedByColumnName = "Issued By";
                expiryDateColumnName = "Expiration Date";
                certificateTypeColumnName = "Certificate Type";
                certificateUsageColumnName = "Certificate Usage";
            }

            int[] radioButtonColumnIndex = { 0 };

            // Get Certificate table details
            _adapter.Navigate("CertificateManagement");
            Collection<Collection<string>> installedCertificates = _adapter.GetTable("Certificates_Table", columnIndex: radioButtonColumnIndex, returnValue: false);

            int certificateTypeColumnIndex = -1;
            int certificateUsageColumnIndex = -1;

            // Gets Certificate properties column numbers to select certificate
            int issuedToColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].FirstOrDefault(x => x.Contains(issuedTocolumnName, StringComparison.CurrentCultureIgnoreCase)));
            int issuedByColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].FirstOrDefault(x => x.Contains(issuedByColumnName, StringComparison.CurrentCultureIgnoreCase)));
            int expiryDateColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].FirstOrDefault(x => x.Contains(expiryDateColumnName, StringComparison.CurrentCultureIgnoreCase)));
            certificateTypeColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].FirstOrDefault(x => x.Contains(certificateTypeColumnName, StringComparison.CurrentCultureIgnoreCase)));
            certificateUsageColumnIndex = installedCertificates[0].IndexOf(installedCertificates[0].FirstOrDefault(x => x.Contains(certificateUsageColumnName, StringComparison.CurrentCultureIgnoreCase)));

            Collection<CertificatesTableRaw> table = new Collection<CertificatesTableRaw>();

            // Remove the header column
            installedCertificates.RemoveAt(0);

            foreach (var item in installedCertificates)
            {
                CertificateType type = _adapter.Settings.ProductType == PrinterFamilies.InkJet ? CertificateType.CA : (certificateTypeColumnIndex != -1 ? Enum<CertificateType>.Parse(item[certificateTypeColumnIndex].Trim()) : CertificateType.None);
                table.Add(new CertificatesTableRaw
                {
                    selectElementId = item[0],
                    CertificateData = new CertificateDetails { IssuedTo = item[issuedToColumnIndex], ExpiryDate = DateTime.Parse(item[expiryDateColumnIndex]), IssuedBy = item[issuedByColumnIndex] },
                    Type = type,
                    IsNetworkIdentity = (certificateUsageColumnIndex != -1 && item[certificateUsageColumnIndex].Contains("Network Identity"))
                });
            }

            return table;
        }

        /// <summary>
        /// Create ID Certificate Request
        /// Note: This method is applicable only for Unification supported products (VEP)
        /// </summary>
        /// <param name="algorithm"><see cref="SignatureAlgorithm"/></param>
        /// <param name="certificateRequest">Request ID generated</param>
        /// <param name="exportPrivateKey">true to check export private key option, false otherwise</param>
        /// <param name="keyLength"></param>
        /// <param name="fqdn"></param>
        /// <returns>true is request created successfully, false otherwise</returns>
        private bool CreateIDCertificateRequestWithUnification(SignatureAlgorithm algorithm, out string certificateRequest, bool exportPrivateKey, RSAKeyLength keyLength = RSAKeyLength.Rsa2048, string fqdn = null)
        {
            certificateRequest = string.Empty;
            string commonName = fqdn.EqualsIgnoreCase(null) ? _adapter.Settings.DeviceAddress : fqdn;

            try
            {
                _adapter.Navigate("CertificateManagement");
                _adapter.Click("Create_IDRequest");

                // Configure All values
                _adapter.SetText("Common_Name", commonName);
                _adapter.SetText("Organization", "Hewlett-Packard Inc");
                _adapter.SetText("Organizational_Unit", "Unit");
                _adapter.SetText("City", "Bangalore");
                _adapter.SetText("State", "Karnataka");
                _adapter.SetText("Country", "IN");

                if (keyLength == RSAKeyLength.Rsa2048)
                {
                    _adapter.SelectByValue("KeyLength", "Rsa2048");
                }
                else if (keyLength == RSAKeyLength.Rsa1024)
                {
                    _adapter.SelectByValue("KeyLength", "Rsa1024");
                }

                if (SignatureAlgorithm.Sha224.Equals(algorithm))
                {
                    TraceFactory.Logger.Info("{0} algorithm is not supported.".FormatWith(algorithm));
                    //return false;
                }

                _adapter.SelectByValue("SignatureAlgorithm", Enum<SignatureAlgorithm>.Value(algorithm));

                if (exportPrivateKey)
                {
                    _adapter.Check("Export_PrivateKey");
                }

                _adapter.Click("Create_Ok");

                Thread.Sleep(TimeSpan.FromMinutes(2));

                // If there is already a previously configured certificate request, a confirmation page appears to override the values
                if (_adapter.IsElementPresent("DialogButtonYes"))
                {
                    _adapter.Click("DialogButtonYes");
                }

                Thread.Sleep(TimeSpan.FromMinutes(2));

                if (_adapter.SearchText("The certificate signing request has been successfully created"))
                {
                    if (_adapter.IsElementPresent("Key_Path"))
                    {
                        certificateRequest = _adapter.GetText("Key_Path");
                        TraceFactory.Logger.Info("Certificate key generated successfully.");
                        if (certificateRequest == "null")
                        {
                            TraceFactory.Logger.Info("ERROR : Could not get the KEy value from the WEB UI. Check if Sitmap for key is correct.");
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Unable find the xpath to generated key.");
                        return false;
                    }
                }

                return false;
            }
            finally
            {
                if (_adapter.IsElementPresent("Create_Back"))
                {
                    _adapter.Click("Create_Back");
                }
            }
        }

        /// <summary>
        /// Exporting the ID Certificate
        /// </summary>
        /// <returns>true if Export is successfully, false otherwise</returns>
        public bool ExportCertificate(string certificatePath = null, string password = null, string exportPath = null)
        {
            try
            {
                string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Export.exe");
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                }
                    _adapter.Navigate("CA_InstallCertificate");
                // check if printer supports certificate unification
                if (!_adapter.IsElementPresent("JDConfigure"))
                {
                    return ExportCertificateWithUnification(certificatePath, password, exportPath);
                }
                if (exportPath != null)
                {
                    // Deleting the directory content before exporting new certifciate
                    Array.ForEach(Directory.GetFiles(exportPath), File.Delete);
                }
                _adapter.Click("JDConfigure");
                _adapter.Click("Export");
                _adapter.Click("CertificateNext");
                if (_adapter.IsElementPresent("ExportPassword"))
                {
                    _adapter.SetText("ExportPassword", password);
                }
                if (_adapter.IsElementPresent("ExportConfirmPassword"))
                {
                    _adapter.SetText("ExportConfirmPassword", password);
                }

                if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
                {
                    _adapter.Click("Finish");
                    _adapter.Navigate("ExportSave");
                    _adapter.Click("ExportSave", sourceType: PluginSupport.Connectivity.Selenium.FindType.ByName);
                }
                else
                {
                    _adapter.Click("Save");
                }

                Thread.Sleep(TimeSpan.FromSeconds(8));
                var result = ScalableTest.Utility.ProcessUtil.Execute("cmd.exe", "/C \"{0}\"".FormatWith(executablePath));
                if (result.ExitCode == 1)
                {
                    TraceFactory.Logger.Debug("No Export popup is present.");
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));
                TraceFactory.Logger.Info("Exporting Certificate is Successful");
                return true;
            }
            catch (Exception)
            {
                TraceFactory.Logger.Info("Failed to export certificate, Exception Occured");
                return false;
            }
            finally
            {
                //This is needed to close the wizard properly, else error is shown on the page
           
                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    _adapter.Navigate("JetDirect_CertificateResult");
                    if (_adapter.IsElementPresent("Result_Ok", sourceType: PluginSupport.Connectivity.Selenium.FindType.ByName))
                    {
                        _adapter.Click("Result_Ok", sourceType: PluginSupport.Connectivity.Selenium.FindType.ByName);
                    }
                }
                StopAdapter();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Exporting the ID Certificate with unification
        /// </summary>
        /// <returns>true if Export is successfully, false otherwise</returns>
        public bool ExportCertificateWithUnification(string certificatePath, string password, string exportPath)
        {
            string executablePath = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Export.exe");

            _adapter.Navigate("CertificateManagement");
            if (!(certificatePath == null))
            {
                SelectCertificate(certificatePath, false, idCertificatePassword: password);
            }

            if (!(exportPath == null))
            {
                // Deleting the directory content before exporting new certificate
                Array.ForEach(Directory.GetFiles(exportPath), File.Delete);
            }
            _adapter.Click("ExportButton");

            Thread.Sleep(TimeSpan.FromSeconds(10));

            if (_adapter.IsElementPresent("ExportPassword"))
            {
                _adapter.SetText("ExportPassword", password);
            }
            if (_adapter.IsElementPresent("ExportConfirmPassword"))
            {
                _adapter.SetText("ExportConfirmPassword", password);
            }
            if (_adapter.IsElementPresent("Create_Ok"))
            {
                _adapter.Click("Create_Ok");
            }
            Thread.Sleep(TimeSpan.FromSeconds(8));
            var result = ScalableTest.Utility.ProcessUtil.Execute("cmd.exe", "/C \"{0}\"".FormatWith(executablePath));
            if (result.ExitCode == 1)
            {
                TraceFactory.Logger.Debug("No Export popup is present.");
            }
            return true;
        }

        /// <summary>
        /// Validates whether the Product Supports Unification for VEP
        /// </summary>
        /// <returns>true if Supports, false otherwise</returns>
        public bool isProductSupportCertificateUnification()
        {
            _adapter.Navigate("CA_InstallCertificate");

            // check if printer supports certificate unification
            if (!_adapter.IsElementPresent("CAConfigure"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set Wipe Option
        /// </summary>
        /// <param name="option">true to enable, false to disable</param>
        public bool SetWipe(bool option)
        {
            _adapter.Navigate("Misc_Settings");

            if (option)
            {
                _adapter.Check("Wipe");
            }
            else
            {
                _adapter.Uncheck("Wipe");
            }
            _adapter.Click("Apply_Misc");
            return true;
        }

        #endregion Private Methods
    }
}