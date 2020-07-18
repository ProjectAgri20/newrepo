using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.DnsApp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.CertificateManagement
{
    /// <summary>
    /// Contains the templates for CertificateManagement test cases
    /// </summary>
    internal static class CertificateManagementTemplates
    {
        #region Local Variables        
        private static string EXPORTCERTIFICATEPATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static string CACERTIFICATEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\CA\CA-2K-SHA2_512-WINDOWS.cer");
        private static string EXPORTCERTIFICATEPASSWORD = "12";
        private static string CACertificate_Server = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\CertificateManagement\RadiusServerCA\{0}\CA_certificate.cer");
        private static string CACertificateFormat = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\CertificateManagement\Types\ca1.{0}");
        private static string CACertificate_Invalid = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\FIPS\2KSHA1ID.pfx");
        private static string CACertificate = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\CertificateManagement\{0}\{1}K\CA_certificate.cer");
        private static string CACertificateIntermediate = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\CertificateManagement\{0}\{1}K\subsha.{2}");
        #endregion

        #region Templates

        #region SelfSigned Certificates
        /// <summary>
        /// Create self-signed certificate-1024 bits With Sha1, Sha224, Sha256, Sha384 and Sha512 and validate the certificate Contents using HTTPS.
        ///Step1: Create New Self Signed Certificate-1024 bits with Sha1
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm             
        ///Step2:Create New Self Signed Certificate-1024 bits with Sha224
        ///         Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm
        ///Step3:Create New Self Signed Certificate-1024bits with Sha256
        ///         Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm
        ///Step4:Create New Self Signed Certificate-1024bits with Sha384
        ///         Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm
        ///Step5:Create New Self Signed Certificate-1024 bits with Sha512
        ///         Validate the following certificate Content using HTTPS
        ///         RSA Key Length
        ///         Signature Algorithm
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <param name="keyLength">RSA KeyLength1024/2048 bits</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool CreateSelfSignedCertificateAndValidate(CertificateManagementActivityData activityData, RSAKeyLength keyLength)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

				TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha1)));
				if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha1, keyLength, false, 1))
				{
					return false;
				}
				if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha1, 1))
				{
					return false;
				}

				TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha224)));
				if (!(activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.VEP.ToString()) && EwsWrapper.Instance().isProductSupportCertificateUnification()))
				{
					if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha224, keyLength, false, 1))
					{
						return false;
					}
					if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha224, 1))
					{
						return false;
					}
				}

				TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha256)));
				if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha256, keyLength, false, 1))
				{
					return false;
				}
				if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha256, 1))
				{
					return false;
				} 

				TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha384)));
				if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha384, keyLength, false, 1))
				{
					return false;
				}
				if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha384, 1))
				{
					return false;
				}          

				TraceFactory.Logger.Info(CtcUtility.WriteStep("Step V: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
				if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha512, keyLength, false, 1))
				{
					return false;
				}
				if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha512, 1))
				{
					return false;
				}
										   
				return true;            
			}
			catch (Exception certificateManagementException)
			{
				TraceFactory.Logger.Info(certificateManagementException.Message);
				return false;
			}
		}        

        /// <summary>
        /// Export/Import self-signed certificate enable mark private key as exportable and validate the certificate Contents using HTTPS 
        /// Step1: Create Certificate with Private Key [Sha1-2048]
        ///         Export certificate, enter Password and Confirm Password
        ///         Check the following contents in the certificate
        ///             Private Key – Yes
        ///             Certificate format must be PFX
        ///             Import certificate
        ///             Enter correct password and
        ///             Validate the following certificate Content using HTTPS
        ///             RSA Key Length
        ///             Signature Algorithm
        ///             Validity - Issued on, Expires
        ///             Allow Private Key
        /// Step2: Create Certificate with Private Key [Sha2/512-2048]
        ///         Export certificate, enter Password and Confirm Password
        ///         Check the following contents in the certificate
        ///             Private Key – Yes
        ///             Certificate format must be PFX
        ///             Import certificate
        ///             Enter correct password and
        ///             Validate the following certificate Content using HTTPS
        ///             RSA Key Length
        ///             Signature Algorithm
        ///             Validity - Issued on, Expires
        ///             Allow Private Key
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <param name="keyLength">keyLength</param>
        /// <param name="enablePrivateKey">true/false</param>
        /// <param name="validityPeriod">no of days to expire</param>
        /// <param name="invalidPassword">is this function is to test invalid certificate</param>
        /// <param name="importPrivateKey">whether private key of import page is in enable state</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool ExportAndImportCertificateAndValidate(CertificateManagementActivityData activityData, RSAKeyLength keyLength, bool enablePrivateKey, int validityPeriod, bool invalidPassword = false, bool importPrivateKey = false)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().isProductSupportCertificateUnification())
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha1)));
                    if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha1, keyLength, enablePrivateKey, 1))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Exporting the created self signed certificate and validating the saved certificate format");
                    EwsWrapper.Instance().ExportCertificate(password: EXPORTCERTIFICATEPASSWORD);

                    TraceFactory.Logger.Info("Validating the exported certificate file extension");

                    string[] filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);

                    if (!ValidateFileExtension(activityData, filePaths[0], enablePrivateKey))
                    {
                        return false;
                    }

                    // Since the certificate is not installing again[Already installed[ because of previously installed one, deleting all
                     if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.VEP.ToString()))
                      {
                             EwsWrapper.Instance().UnInstallAllCertificates();
                      }
                    TraceFactory.Logger.Info("Importing the Self Signed certificate which has been exported");
                    if (invalidPassword)
                    {
                        return !EwsWrapper.Instance().InstallIDCertificate(filePaths[0], "1234", enablePrivateKey: importPrivateKey);
                    }
                    else
                    {
                        if (!EwsWrapper.Instance().InstallIDCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, enablePrivateKey: importPrivateKey))
                        {
                            return false;
                        }
                    }

                    if (!invalidPassword)
                    {
                        if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha1, validityPeriod, enablePrivateKey, true, importPrivateKey))
                        {
                            return false;
                        }
                    }

                    TraceFactory.Logger.Info("Performing post requisite after the completion of first step");
                    TestPostRequisites(activityData);

                    // TPS does not supports other Signature algoritham for Self signed certificate apart from SHA1. To check that below condition
                   // if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                  //  {
                        TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
                        if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha512, keyLength, enablePrivateKey, 1))
                        {
                            return false;
                        }

                        TraceFactory.Logger.Info("Exporting the created self signed certificate and validating the saved certificate whether in .pfx format");
                        EwsWrapper.Instance().ExportCertificate(password: EXPORTCERTIFICATEPASSWORD);

                        TraceFactory.Logger.Info("Validating the exported certificate file extension");

                        filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);
                        if (!ValidateFileExtension(activityData, filePaths[0], enablePrivateKey))
                        {
                            return false;
                        }

                        // Since the certificate is not installing again[Already installed[ because of previously installed one, deleting all
                   if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.VEP.ToString()))
                     {
                        EwsWrapper.Instance().UnInstallAllCertificates();
                     }
                        TraceFactory.Logger.Info("Importing the Self Signed certificate which has been exported");
                        if (invalidPassword)
                        {
                            return !EwsWrapper.Instance().InstallIDCertificate(filePaths[0], "1234", enablePrivateKey: importPrivateKey);
                        }
                        else
                        {
                            EwsWrapper.Instance().InstallIDCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, enablePrivateKey: importPrivateKey);
                        }

                        if (!ValidateSelfSignedCertificate(activityData, keyLength, SignatureAlgorithm.Sha512, validityPeriod, enablePrivateKey, true, importPrivateKey))
                        {
                            return false;
                        }
                   // }
                   // else
                  //  {
                    //    TraceFactory.Logger.Info("TPS only supports SHA1 as signature Algoritham for Self Signed Certificates");
                  //  }
                }
                else
                {
                    TraceFactory.Logger.Info("Self Signed Certificate created can not be deleted as part of PostRequisite in SI Product so this test case is not applicable for SI Products");
                }
                return true;
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }
        #endregion

        #region ID Certificate
        /// <summary>
        /// Create ID certificate-1024/2048 bits With Sha1, Sha224, Sha256, Sha384 and Sha512 and validate the certificate Contents using IPPS and HTTPS.
        ///Step1: Create ID Certificate Request-1024 bits with Sha1
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[1024+SHa1 in Root SHa1 server]
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page          
        ///Step2: Create ID Certificate Request-1024 bits with Sha224
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[1024+SHa1 in Root SHa1 server]
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page   
        ///Step3: Create ID Certificate Request-1024 bits with Sha256
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[1024+SHa1 in Root SHa1 server]
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page   
        ///Step4: Create ID Certificate Request-1024 bits with Sha384
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[1024+SHa1 in Root SHa1 server]
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page   
        ///Step5: Create ID Certificate Request-1024 bits with Sha512
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[1024+SHa1 in Root SHa1 server]
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page   
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <param name="keyLength">RSA KeyLength1024/2048 bits</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool CreateIDCertificateRequestWithServerSigningAndValidate(CertificateManagementActivityData activityData, RSAKeyLength keyLength)
        {
            string certificatePath = string.Empty;
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // For IPPS Printing we need to create certificate request with printer FQDN Value
                EwsWrapper.Instance().SetHostname("DefaultHost");
                EwsWrapper.Instance().SetDomainName("Automation.com");

                string fqdn = CtcUtility.GetFqdn();
                string certificateRequest = string.Empty;

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha1)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha1ServerIp, SignatureAlgorithm.Sha1, out certificatePath, keyLength: keyLength, fqdn: fqdn))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                {
                    return false;
                }

                if (keyLength.ToString().Equals("Rsa1024", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!ValidateIDCertificate(activityData, keyLength, SignatureAlgorithm.Sha1, certificatePath, fqdn, validateIPPS: true))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha224)));
                TraceFactory.Logger.Info("Sha224 is not supported in windows to generate ID Certificate Request, hence the step with Sha224 is not applicable");

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha256)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha256, out certificatePath, keyLength: keyLength, fqdn: fqdn))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                {
                    return false;
                }

                if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha384)));
                    if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha384, out certificatePath, keyLength: keyLength, fqdn: fqdn))
                    {
                        TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                        return false;
                    }

                    if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step V: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
                    if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha512, out certificatePath, keyLength: keyLength, fqdn: fqdn))
                    {
                        TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                        return false;
                    }

                    if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                    {
                        return false;
                    }

                    if (keyLength.ToString().Equals("Rsa2048", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!ValidateIDCertificate(activityData, keyLength, SignatureAlgorithm.Sha512, certificatePath, fqdn, validateIPPS: false))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Step IV: TPS does not support Sha384. Skipping this step");
                    TraceFactory.Logger.Info("Step V : TPS does not support Sha512. Skipping this step");

                }
                return true;
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Create ID certificate-2048 bits With Sha1/Sha2 and Reboot the Printer and validate the certificate Contents using IPPS and HTTPS.
        ///Step1: Create ID Certificate Request-2048 bits with Sha1
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[2048+SHa1 in Root SHa1 server]
        ///       Reboot the Printer
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page          
        ///Step2: Create ID Certificate Request-2048 bits with Sha2
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[2048+SHa2 in Root SHa2 server]
        ///       Reboot the Printer
        ///       Install Signed Certificate
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///           Signature Algorithm  
        ///           Organization/City/State/Country
        ///           PrivateKey
        ///           Validity
        ///         Validate IPPS by installing IPPS driver and printing page
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <param name="keyLength">RSA KeyLength1024/2048 bits</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool CreateIDCertificateRequestWithServerSigningAndReboot(CertificateManagementActivityData activityData)
        {
            string certificatePath = string.Empty;
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // For IPPS Printing we need to create certificate request with printer FQDN Value
                EwsWrapper.Instance().SetHostname("DefaultHost");
                EwsWrapper.Instance().SetDomainName("Automation.com");

                string certificateRequest = string.Empty;
                string fqdn = CtcUtility.GetFqdn();
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(RSAKeyLength.Rsa1024, SignatureAlgorithm.Sha1)));
                if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                {
                    if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha1ServerIp, SignatureAlgorithm.Sha1, out certificatePath, keyLength: RSAKeyLength.Rsa1024, fqdn: fqdn))
                    {
                        TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                        return false;
                    }

                    printer.PowerCycle();

                    if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                    {
                        return false;
                    }

                    if (!ValidateIDCertificate(activityData, RSAKeyLength.Rsa1024, SignatureAlgorithm.Sha1, certificatePath, fqdn, false))
                    {
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("RSA Key Length 1024 is not applicable for TPS");
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha512, out certificatePath, keyLength: RSAKeyLength.Rsa2048, fqdn: fqdn))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                printer.PowerCycle();

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                {
                    return false;
                }

                if (!ValidateIDCertificate(activityData, RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512, certificatePath, fqdn, false, validateIPPS: false))
                {
                    return false;
                }
                return true;
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Create ID certificate-2048 bits With Sha1/Sha2 and validate the contents by installing invalid Certificate
        ///Step1: Create ID Certificate Request-2048 bits with Sha1
        ///       Copy the Certificate Contents to Radius Server Root Sha1 and get signed in server[2048+SHa1 in Root SHa1 server]
        ///       Create ID Certificate Request-2048 bits with Sha512
        ///       Install the Certificate which has been created with 2048-Sha1
        ///       Certificate should not install, it should throw error             
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>                
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool CreateIDCertificateRequestWithInvalidCertificate(CertificateManagementActivityData activityData)
        {
            string certificatePath = string.Empty;
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string certificateRequest = string.Empty;
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha512, out certificatePath, keyLength: RSAKeyLength.Rsa2048))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().CreateIDCertificateRequest(SignatureAlgorithm.Sha1, out certificateRequest, keyLength: RSAKeyLength.Rsa2048))
                {
                    return false;
                }
                return !EwsWrapper.Instance().InstallCertificate(certificatePath);
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Export/Import ID certificate enable mark private key as exportable and validate the certificate Contents using HTTPS 
        /// Step1: Create Certificate Request with 2048 bits-Sha1 and mark private key as exportable
        ///        Send the Key contents to Root SHa1 Server
        ///        Generate Certificate and save
        ///        Install the signed Id Certificate in the Printer
        ///        Export the installed Certificate to some location in client
        ///        Import the certificate                     
        ///             Validate the following certificate Content using HTTPS
        ///             RSA Key Length
        ///             Signature Algorithm
        ///              Certificate format
        ///             Validity - Issued on, Expires
        ///             Allow Private Key
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool ExportAndImportIDCertificateAndValidate(CertificateManagementActivityData activityData, bool enablePrivateKey, int validityPeriod, bool invalidPassword = false, bool importPrivateKey = false)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // For IPPS Printing we need to create certificate request with printer FQDN Value                
                EwsWrapper.Instance().SetHostname("DefaultHost");
                EwsWrapper.Instance().SetDomainName("Automation.com");

                string certificatePath = string.Empty;
                string fqdn = CtcUtility.GetFqdn();
                string[] filePaths = null;

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(RSAKeyLength.Rsa1024, SignatureAlgorithm.Sha1)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha1ServerIp, SignatureAlgorithm.Sha1, out certificatePath, enablePrivateKey, RSAKeyLength.Rsa1024, fqdn))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Exporting the Installed ID certificate and validating the saved certificate format");
                if (!EwsWrapper.Instance().ExportCertificate(certificatePath, EXPORTCERTIFICATEPASSWORD, EXPORTCERTIFICATEPATH))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the exported certificate file extension");

                filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);

                if (!ValidateFileExtension(activityData, filePaths[0], enablePrivateKey))
                {
                    return false;
                }

                // Since the certificate is not installing again[Already installed[ because of previously installed one, deleting all
                EwsWrapper.Instance().UnInstallAllCertificates();

                TraceFactory.Logger.Info("Importing the ID certificate which has been exported");
                if (invalidPassword)
                {
                    return !EwsWrapper.Instance().InstallIDCertificate(filePaths[0], "1234", enablePrivateKey: importPrivateKey);
                }
                else
                {
                    if (!EwsWrapper.Instance().InstallIDCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, enablePrivateKey: importPrivateKey))
                    {
                        return false;
                    }
                }

                if (!importPrivateKey)
                {
                    EwsWrapper.Instance().ExportCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, EXPORTCERTIFICATEPATH);
                    filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);
                    TraceFactory.Logger.Info("FilePath: {0}".FormatWith(filePaths[0]));
                    if (!ValidateIDCertificate(activityData, RSAKeyLength.Rsa1024, SignatureAlgorithm.Sha1, filePaths[0], fqdn, enablePrivateKey, true, importPrivateKey))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ValidateIDCertificate(activityData, RSAKeyLength.Rsa1024, SignatureAlgorithm.Sha1, filePaths[0], fqdn, enablePrivateKey, true, importPrivateKey, password: EXPORTCERTIFICATEPASSWORD))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Performing post requisite after the completion of first step");
                TestPostRequisites(activityData);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha512, out certificatePath, true, RSAKeyLength.Rsa2048, fqdn))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath, importPrivateKey))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Exporting the Installed ID certificate and validating the saved certificate format");
                if (!EwsWrapper.Instance().ExportCertificate(certificatePath, EXPORTCERTIFICATEPASSWORD, EXPORTCERTIFICATEPATH))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the exported certificate file extension");

                filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);

                if (!ValidateFileExtension(activityData, filePaths[0], enablePrivateKey))
                {
                    return false;
                }

                // Since the certificate is not installing again[Already installed[ because of previously installed one, deleting all
                EwsWrapper.Instance().UnInstallAllCertificates();

                TraceFactory.Logger.Info("Importing the ID certificate which has been exported");
                if (invalidPassword)
                {
                    return !EwsWrapper.Instance().InstallIDCertificate(filePaths[0], "1234", enablePrivateKey: importPrivateKey);
                }
                else
                {
                    if (!EwsWrapper.Instance().InstallIDCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, enablePrivateKey: importPrivateKey))
                    {
                        return false;
                    }
                }

                if (!importPrivateKey)
                {
                    EwsWrapper.Instance().ExportCertificate(filePaths[0], EXPORTCERTIFICATEPASSWORD, EXPORTCERTIFICATEPATH);
                    filePaths = Directory.GetFiles(EXPORTCERTIFICATEPATH);
                    return ValidateIDCertificate(activityData, RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512, filePaths[0], fqdn, enablePrivateKey, true, importPrivateKey);
                }
                else
                {
                    return ValidateIDCertificate(activityData, RSAKeyLength.Rsa2048, SignatureAlgorithm.Sha512, filePaths[0], fqdn, enablePrivateKey, true, importPrivateKey, password: EXPORTCERTIFICATEPASSWORD);
                }
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Create Self-Signed, ID, CA Certificate and Validate WipeOut Functionality
        ///Step1: Create Self-Signed Certificate with 1024 bits-Sha1
        ///       Enable Wipe out option in EWS Page
        ///       Cold reset the Printer
        ///       Validate the availability of the Certificate, it should not present
        ///Step1: Create ID Certificate with 1024 bits-Sha1
        ///       Enable Wipe out option in EWS Page
        ///       Cold reset the Printer
        ///       Validate the availability of the Certificate, it should not present
        ///Step1: Create CA Certificate with 1024 bits-Sha1
        ///       Enable Wipe out option in EWS Page
        ///       Cold reset the Printer
        ///       Validate the availability of the Certificate, it should not present       
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <param name="keyLength">RSA KeyLength1024/2048 bits</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool ValidateCertificateAvailabilityWithWipeOutOptionEnabled(CertificateManagementActivityData activityData, RSAKeyLength keyLength)
        {
            string certificatePath = string.Empty;
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creation of new self signed certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
                if (!EwsWrapper.Instance().InstallSelfSignedCertificate(SignatureAlgorithm.Sha512, keyLength, false, 1))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetWipe(true))
                {
                    return false;
                }

                printer.ColdReset();

                CertificateDetails certificateContent = EwsWrapper.Instance().GetSelfSignedCertificateDetails();
                if (certificateContent.SignatureAlgorithm.FriendlyName.Contains(SignatureAlgorithm.Sha512.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Even after Wipe and cold reset, still old self signed certificate exist");
                    return false;
                }
                TraceFactory.Logger.Info("After Wipe and Cold reset the old certificate has been deleted successfully");

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creation of ID Certificate Request of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
                if (!EwsWrapper.Instance().GenerateIdCertificate(activityData.RootSha2ServerIp, SignatureAlgorithm.Sha512, out certificatePath, keyLength: keyLength))
                {
                    TraceFactory.Logger.Info("Failed to Generate ID Certificate");
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(certificatePath))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetWipe(true))
                {
                    return false;
                }

                printer.ColdReset();

                certificateContent = EwsWrapper.Instance().GetSelfSignedCertificateDetails();
                if (certificateContent.SignatureAlgorithm.FriendlyName.Contains(SignatureAlgorithm.Sha512.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Even after Wipe and cold reset, still old self signed certificate exist");
                    return false;
                }
                TraceFactory.Logger.Info("After Wipe and COld reset the old certificate has been deleted successfully");

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Creation of CA Certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));

                if (!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH))
                {
                    TraceFactory.Logger.Info("Failed to install CA Certificate");
                    return false;
                }
                certificateContent = EwsWrapper.Instance().GetSelfSignedCertificateDetails();
                if (certificateContent.SignatureAlgorithm.FriendlyName.Contains(SignatureAlgorithm.Sha512.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Even after Wipe and cold reset, still old self signed certificate exist");
                    return false;
                }
                TraceFactory.Logger.Info("After Wipe and COld reset the old certificate has been deleted successfully");

                return true;
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        #endregion

        #region CA Certificates
        /// <summary>
        /// Install CA Certificate-1024 bits With Sha1, Sha224, Sha256, Sha384 and Sha512 and validate the certificate Contents using HTTPS.
        ///Step1: Install CA Certificate-1024 bits with Sha1
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm  
        ///          Validity
        ///Step2: Install CA Certificate-1024 bits with Sha224
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm  
        ///          Validity
        ///Step3: Install CA Certificate-1024 bits with Sha256
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm  
        ///          Validity
        ///Step4: Install CA Certificate-1024 bits with Sha384
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm  
        ///          Validity
        ///Step5: Install CA Certificate-1024 bits with Sha512
        ///        Validate the following certificate Content using HTTPS
        ///           RSA Key Length
        ///          Signature Algorithm  
        ///          Validity
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <param name="keyLength">RSA KeyLength1024/2048 bits</param>
        /// <param name="intermediate">is intermediate</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool InstallCACertificateAndValidate(CertificateManagementActivityData activityData, RSAKeyLength keyLength, bool intermediate = false)
        {
            bool isTPS = activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString());

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string key = keyLength.ToString().Equals("Rsa1024", StringComparison.CurrentCultureIgnoreCase) ? "1" : "2";
                string format = keyLength.ToString().Equals("Rsa1024", StringComparison.CurrentCultureIgnoreCase) ? "pem" : "cer";
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Installing CA certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha1)));
                if (intermediate ? !EwsWrapper.Instance().InstallCACertificate(CACertificateIntermediate.FormatWith("SHA1", key, format), intermediate) : !EwsWrapper.Instance().InstallCACertificate(CACertificate.FormatWith("SHA1", key), intermediate))
                {
                    return false;
                }
                if (intermediate ? !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha1, CACertificateIntermediate.FormatWith("SHA1", key, format)) : !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha1, CACertificate.FormatWith("SHA1", key)))
                {
                    return false;
                }

                // TODO: Once we get intermediate certificate from Amitha then we need to remove this if block
                if (!intermediate)
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Installing CA certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha256)));
                    if (intermediate ? !EwsWrapper.Instance().InstallCACertificate(CACertificateIntermediate.FormatWith("SHA256", key, format), intermediate) : !EwsWrapper.Instance().InstallCACertificate(CACertificate.FormatWith("SHA256", key), intermediate))
                    {
                        return false;
                    }
                    if (intermediate ? !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha256, CACertificateIntermediate.FormatWith("SHA256", key, format)) : !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha256, CACertificate.FormatWith("SHA256", key)))
                    {
                        return false;
                    }
                    if (!isTPS)
                    {
                        TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Installing CA certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha384)));
                        if (intermediate ? !EwsWrapper.Instance().InstallCACertificate(CACertificateIntermediate.FormatWith("SHA384", key, format), intermediate) : !EwsWrapper.Instance().InstallCACertificate(CACertificate.FormatWith("SHA384", key), intermediate))
                        {
                            return false;
                        }
                        if (intermediate ? !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha384, CACertificateIntermediate.FormatWith("SHA384", key, format)) : !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha384, CACertificate.FormatWith("SHA384", key)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info("TPS Does not support SHA384 Skipping this step");
                    }
                }
                // TODO: The provided intermediate certificate for Sha512 is not valid, after installing it is showing SHa1, amitha will get valid certificate once we get valid then we need to remove this if block
                if (!intermediate)
                {
                    if (!isTPS)
                    {
                        TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: Installing CA certificate of {0} bits with {1} Signature Algorithm".FormatWith(keyLength, SignatureAlgorithm.Sha512)));
                        if (intermediate ? !EwsWrapper.Instance().InstallCACertificate(CACertificateIntermediate.FormatWith("SHA512", key, format), intermediate) : !EwsWrapper.Instance().InstallCACertificate(CACertificate.FormatWith("SHA512", key), intermediate))
                        {
                            return false;
                        }
                        if (intermediate ? !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha512, CACertificateIntermediate.FormatWith("SHA512", key, format)) : !ValidateCACertificate(activityData, keyLength, SignatureAlgorithm.Sha512, CACertificate.FormatWith("SHA512", key)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info("TPS Does not support SHA512. Skipping this step");
                    }
                }

                return intermediate ? EwsWrapper.Instance().UnInstallCACertificate(CACertificateIntermediate.FormatWith("SHA1", key, format)) : EwsWrapper.Instance().UnInstallCACertificate(CACertificate.FormatWith("SHA512", key));
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Invalid Certificate Validation
        ///  Install CA Certificate-.pfx format
        ///  Installation Should fail       
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool InstallInvalidCACertificate(CertificateManagementActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Installing invalid CA certificate");
                return !EwsWrapper.Instance().InstallCACertificate(CACertificate_Invalid);
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
        }

        /// <summary>
        /// Install CA Certificate with different file format
        ///Step1: Install CA Certificate-with CER Format
        ///       Certificate Installation should be successful
        ///Step2: Install CA Certificate-with CRT Format
        ///       Certificate Installation should be successful
        ///Step3: Install CA Certificate-with DER Format
        ///       Certificate Installation should be successful
        ///Step4: Install CA Certificate-with PEM Format
        ///       Certificate Installation should be successful
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>        
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool InstallCACertificateWithDifferentFormat(CertificateManagementActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Installing CA certificate of CRT Format"));
                if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.VEP.ToString()))
                {
                    if (!EwsWrapper.Instance().InstallCACertificate(CACertificateFormat.FormatWith("crt")))
                    {
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("CRT Format is not Supported for VEP");
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Installing CA certificate of DER Format"));
                if (!EwsWrapper.Instance().InstallCACertificate(CACertificateFormat.FormatWith("der")))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Installing CA certificate of PEM Format"));
                return EwsWrapper.Instance().InstallCACertificate(CACertificateFormat.FormatWith("pem"));
            }
            catch (Exception certificateManagementException)
            {
                TraceFactory.Logger.Info(certificateManagementException.Message);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Validates the Certificate Content
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <param name="keyLength">RSA Key Length 1024/2048</param>
        /// <param name="signatureAlgorithm">Sha1/256/224/512/384</param>
        /// <param name="validityInDays">no of days to expire</param>
        /// <param name="allowPrivateKey">private key is enabled /disabled while creating self signed certificate</param>
        /// <param name="isImportFunction">Is this the validation for Import/export Certificate</param>
        /// <param name="importPrivateKey">Private Key option while importing certificate</param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool ValidateSelfSignedCertificate(CertificateManagementActivityData activityData, RSAKeyLength keyLength, SignatureAlgorithm signatureAlgorithm, int validityInDays, bool allowPrivateKey = false, bool isImportFunction = false, bool importPrivateKey = false)
        {
            try
            {
                TraceFactory.Logger.Info("Validating the Self Signed Certificate Content");
                CertificateDetails certificateContent = EwsWrapper.Instance().GetSelfSignedCertificateDetails();

                TraceFactory.Logger.Info("****************** Validating the Certificate Content *****************");

                if (activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                {
                    if (keyLength.ToString().Equals("Rsa1024"))
                    {
                        TraceFactory.Logger.Info("Skipping Validation for this step.");
                        return true;
                    }
                    else
                    {
                        //TraceFactory.Logger.Info("Validating the Self Signed Certificate Content");
                        //CertificateDetails certificateContent = EwsWrapper.Instance().GetSelfSignedCertificateDetails();
                        TraceFactory.Logger.Info("****************** Validating the Certificate Content *****************");
                        TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                        TraceFactory.Logger.Info("Expected   *  " + "Start Date: " + string.Join("  *  ", DateTime.Now.Date) + "   " + "End Date: " + string.Join("  *  ", DateTime.Now.AddDays(validityInDays).Date) + " *");
                        TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                        TraceFactory.Logger.Info("Actual     *  " + "Start Date: " + string.Join("  *  ", certificateContent.StartDate.Date) + "   " + "End Date: " + string.Join("  *  ", certificateContent.ExpiryDate.Date) + " *");
                        TraceFactory.Logger.Info("---------------------------------------------------------------------------");


                        if (!((DateTime.Now.AddDays(validityInDays).Date).Equals(certificateContent.ExpiryDate.Date) && DateTime.Now.Date.Equals(certificateContent.StartDate.Date)))
                        {
                            TraceFactory.Logger.Info("Failed to validate: Certificate Content mismatch");
                            return false;
                        }

                        TraceFactory.Logger.Info("Successfully validated the certificate content");
                        return true;
                    }
                }

                else
                {
                    // If the validation is for Export/Import Functionality then while importing we have one option to enable /disable private key, for that we need to validate
                    if (isImportFunction)
                    {
                        allowPrivateKey = importPrivateKey;
                    }
                    TraceFactory.Logger.Info("AlloPrivateKey : {0}".FormatWith(allowPrivateKey));
                    TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                    TraceFactory.Logger.Info("Expected   *  " + "KeyLength: " + string.Join("  *  ", keyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", signatureAlgorithm) + "   " + " AllowPrivateKey: " + string.Join("  *  ", allowPrivateKey) + "   " + "Start Date: " + string.Join("  *  ", DateTime.Now.Date) + "   " + "End Date: " + string.Join("  *  ", DateTime.Now.AddDays(validityInDays).Date) + " *");
                    TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                    TraceFactory.Logger.Info("Actual     *  " + "KeyLength: " + string.Join("  *  ", "Rsa" + certificateContent.KeyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", certificateContent.SignatureAlgorithm.FriendlyName) + "   " + " AllowPrivateKey: " + string.Join("  *  ", certificateContent.PrivateKeyExportable) + "   " + "Start Date: " + string.Join("  *  ", certificateContent.StartDate.Date) + "   " + "End Date: " + string.Join("  *  ", certificateContent.ExpiryDate.Date) + " *");
                    TraceFactory.Logger.Info("---------------------------------------------------------------------------");

                    if (!((DateTime.Now.AddDays(validityInDays).Date).Equals(certificateContent.ExpiryDate.Date) && DateTime.Now.Date.Equals(certificateContent.StartDate.Date) &&
                           keyLength.ToString().Equals("Rsa" + certificateContent.KeyLength, StringComparison.CurrentCultureIgnoreCase) && certificateContent.SignatureAlgorithm.FriendlyName.Contains(signatureAlgorithm.ToString(), StringComparison.CurrentCultureIgnoreCase) && allowPrivateKey.Equals(certificateContent.PrivateKeyExportable)))
                    {
                        TraceFactory.Logger.Info("Failed to validate: Certificate Content mismatch");
                        return false;
                    }

                    TraceFactory.Logger.Info("Successfully validated the certificate content");
                    return true;
                }
            }
            catch (Exception certificateContentMismatch)
            {
                TraceFactory.Logger.Info(certificateContentMismatch.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates the ID Certificate Content
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool ValidateIDCertificate(CertificateManagementActivityData activityData, RSAKeyLength keyLength, SignatureAlgorithm signatureAlgorithm, string certificatePath, string fqdn,
                                                  bool allowPrivateKey = false, bool isImportFunction = false, bool importPrivateKey = false, bool validateIPPS = false, string password = null)
        {
            try
            {
                //If the validation is for Export/Import Functionality then while importing we have one option to enable /disable private key, for that we need to validate
                if (isImportFunction)
                {
                    allowPrivateKey = importPrivateKey;
                }

                TraceFactory.Logger.Info("Validating the ID Certificate Content");
                Thread.Sleep(TimeSpan.FromSeconds(40));
                CertificateDetails certificateContent = CertificateUtility.GetCertificateDetails(certificatePath, password);
                Thread.Sleep(TimeSpan.FromSeconds(10));
                string issuedBy = signatureAlgorithm.ToString().Equals("Sha1", StringComparison.CurrentCultureIgnoreCase) ? "ROOT2KSHA1-CA" : "rootsha2-ROOTSHA2-PC-CA";
                string SHA1server = "ROOT2KSHA1-CA";
                string SHA2server = "rootsha2-ROOTSHA2-PC-CA";

                TraceFactory.Logger.Info("****************** Validating the Certificate Content *****************");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                TraceFactory.Logger.Info("Expected   *  " + "KeyLength: " + string.Join("  *  ", keyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", signatureAlgorithm) + "   " + " AllowPrivateKey: " + string.Join("  *  ", allowPrivateKey) + "   " + " Issued To: " + string.Join("  *  ", fqdn) + " Issued By: " + string.Join("  *  ", issuedBy) + " *");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                TraceFactory.Logger.Info("Actual     *  " + "KeyLength: " + string.Join("  *  ", "Rsa" + certificateContent.KeyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", certificateContent.SignatureAlgorithm.FriendlyName) + "   " + " AllowPrivateKey: " + string.Join("  *  ", certificateContent.PrivateKeyExportable) + "   " + " Issued To: " + string.Join("  *  ", certificateContent.IssuedTo) + " Issued By: " + string.Join("  *  ", certificateContent.IssuedBy) + " *");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");

                // if (!(keyLength.ToString().Equals("Rsa" + certificateContent.KeyLength, StringComparison.CurrentCultureIgnoreCase) && certificateContent.SignatureAlgorithm.FriendlyName.Contains(signatureAlgorithm.ToString(), StringComparison.CurrentCultureIgnoreCase)
                //       && allowPrivateKey.Equals(certificateContent.PrivateKeyExportable && (certificateContent.IssuedTo.Equals(fqdn, StringComparison.CurrentCultureIgnoreCase) || certificateContent.IssuedTo.Equals(activityData.Ipv4Address, StringComparison.CurrentCultureIgnoreCase)) && certificateContent.IssuedBy.Contains(issuedBy, StringComparison.CurrentCultureIgnoreCase))))

                // Validation of signing algorithm removed as it depends on the CA server and not on the certificate request generated
                if (!(keyLength.ToString().Equals("Rsa" + certificateContent.KeyLength, StringComparison.CurrentCultureIgnoreCase) && allowPrivateKey.Equals(certificateContent.PrivateKeyExportable) && (certificateContent.IssuedTo.Equals(fqdn, StringComparison.CurrentCultureIgnoreCase) || certificateContent.IssuedTo.Equals(activityData.Ipv4Address, StringComparison.CurrentCultureIgnoreCase))))
                {
                    TraceFactory.Logger.Info("Failed to validate: Certificate Content mismatch");
                    return false;
                }

                if (!((certificateContent.IssuedBy).Equals(SHA1server) || (certificateContent.IssuedBy).Equals(SHA2server)))
                {

                    TraceFactory.Logger.Info(" Issued by field in the certificate doesnt match expected value");
                }

                TraceFactory.Logger.Info("Successfully validated the certificate content");

				if (validateIPPS)
				{
                    if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Validating the Print with IPPS");
                        if (!IPPS_Prerequisite(IPAddress.Parse(activityData.Ipv4Address), signatureAlgorithm.ToString()))
                        {
                            return false;
                        }
                        // MessageBox.Show("Check Cert are installed abd fqdn is pinging. Check manully ipps is working ");
                        if (!Print(activityData, activityData.Ipv4Address, certificateContent.IssuedTo))
                        {
                            return false;
                        }

                        if (!IPPS_Postrequisite(IPAddress.Parse(activityData.Ipv4Address), fqdn))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception certificateContentMismatch)
            {
                TraceFactory.Logger.Info(certificateContentMismatch.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates the CA Certificate Content
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool ValidateCACertificate(CertificateManagementActivityData activityData, RSAKeyLength keyLength, SignatureAlgorithm signatureAlgorithm, string certificatePath)
        {
            try
            {
                TraceFactory.Logger.Info("Validating the CA Certificate Content");
                CertificateDetails certificateContent = CertificateUtility.GetCertificateDetails(certificatePath);

                TraceFactory.Logger.Info("****************** Validating the CA Certificate Content *****************");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                TraceFactory.Logger.Info("Expected   *  " + "KeyLength: " + string.Join("  *  ", keyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", signatureAlgorithm) + " *");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");
                TraceFactory.Logger.Info("Actual     *  " + "KeyLength: " + string.Join("  *  ", "Rsa" + certificateContent.KeyLength) + "   " + " Signature Algorithm: " + string.Join("  *  ", certificateContent.SignatureAlgorithm.FriendlyName) + " *");
                TraceFactory.Logger.Info("---------------------------------------------------------------------------");

                if (!(keyLength.ToString().Equals("Rsa" + certificateContent.KeyLength, StringComparison.CurrentCultureIgnoreCase) && certificateContent.SignatureAlgorithm.FriendlyName.Contains(signatureAlgorithm.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    TraceFactory.Logger.Info("Failed to validate: Certificate Content mismatch");
                    return false;
                }

                TraceFactory.Logger.Info("Successfully validated the certificate content");
                return true;
            }

            catch (Exception certificateContentMismatch)
            {
                TraceFactory.Logger.Info(certificateContentMismatch.Message);
                return false;
            }
        }

        /// <summary>
        /// Performs Windows test pre requisites
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool TestPreRequisites(CertificateManagementActivityData activityData)
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

                bool continueTest = true;

                // Deleting all the files in Download folder
                Array.ForEach(Directory.GetFiles(EXPORTCERTIFICATEPATH), File.Delete);

                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(30)))
                {
                    continueTest = CtcUtility.ShowErrorPopup("Printer: {0} is not available.\n Please cold reset the printer.".FormatWith(activityData.Ipv4Address));
                }
                if (!continueTest)
                {
                    return false;
                }
                EwsWrapper.Instance().UnInstallAllCertificates();

                EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Performs Windows test post requisites        
        /// </summary>
        /// <param name="activityData"><see cref="CertificateManagementActivityData"/></param>
        private static void TestPostRequisites(CertificateManagementActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            EwsWrapper.Instance().UnInstallAllCertificates();

            if (Directory.Exists(EXPORTCERTIFICATEPATH))
            {
                Array.ForEach(Directory.GetFiles(EXPORTCERTIFICATEPATH), File.Delete);
            }
        }

        /// <summary>
        /// Prerequisites for IPPS Protocol related test cases
        /// 1. Installation of CA and ID certificates in Printer and Client.[Pre Generated Certificate should have host name]
        /// 2. The host name of the certificate has to be set in the Printer and in the server.
        /// 3. WinServerIP Address has to be set in the printer and in the server.
        /// 4. Make sure the Win server is up and running in DHCP server.       
        /// </summary>
        /// <param name="printerIPAddress">Printer IP Address</param>              
        /// <param name="outHostName">host name from the certificate</param> 
        /// <returns>Returns true prerequisites passed else returns false</returns>
        public static bool IPPS_Prerequisite(IPAddress printerIPAddress, string signatureAlgorithm)
        {
            string serverIP = Printer.Printer.GetDHCPServerIP(printerIPAddress).ToString();

            string domainName = EwsWrapper.Instance().GetDomainName();
            string hostName = EwsWrapper.Instance().GetHostname();

            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP))
            {
                if (!dnsClient.Channel.AddDomain(domainName))
                {
                    TraceFactory.Logger.Info("Failed to add zone in DNS Server");
                }
                if (dnsClient.Channel.AddRecord(domainName, hostName, "A", printerIPAddress.ToString()))
                {
                    TraceFactory.Logger.Info("Successfully added record to DNS Server");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to add DNS Record");
                    return false;
                }
            }

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
            string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);

            if (signatureAlgorithm.Contains("Sha1", StringComparison.CurrentCultureIgnoreCase) ? !CtcUtility.InstallCACertificate(CACertificate_Server.FormatWith("RootSha1")) :
                 !CtcUtility.InstallCACertificate(CACertificate_Server.FormatWith("RootSha2")))
            {
                return false;
            }

            if (!serviceMethod.Channel.SetDnsServer(serverIP, scopeIP, serverIP))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// PostRequisite for IPPS Protocol related test cases      
        /// </summary>
        /// <param name="printerIPAddress">Printer IP Address</param>              
        /// <param name="outHostName">host name from the certificate</param> 
        /// <returns>Returns true prerequisites passed else returns false</returns>
        public static bool IPPS_Postrequisite(IPAddress printerIPAddress, string fqdn)
        {
            string serverIP = Printer.Printer.GetDHCPServerIP(printerIPAddress).ToString();

            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP))
            {
                if (dnsClient.Channel.DeleteRecord(EwsWrapper.Instance().GetDomainName(), EwsWrapper.Instance().GetHostname(), "A", printerIPAddress.ToString()))
                {
                    TraceFactory.Logger.Info("Successfully deleted records in DNS Server");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to delete DNS Records");
                }
            }
            EwsWrapper.Instance().SetHostname("automationhost");
            return true;
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>        
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private static bool Print(CertificateManagementActivityData activityData, string ipAddress, string certificateHostName)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            TraceFactory.Logger.Info("certificate host name is : {0}".FormatWith(certificateHostName));
            TraceFactory.Logger.Info("driver package path : {0}".FormatWith(activityData.DriverPackagePath));
            TraceFactory.Logger.Info("driver model: {0}".FormatWith(activityData.DriverModel));
            if (printer.Install(IPAddress.Parse(ipAddress), Printer.Printer.PrintProtocol.IPPS, activityData.DriverPackagePath, activityData.DriverModel, 443, certificateHostName))
            {
                if (!printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(12))))
                {
                    TraceFactory.Logger.Info("Simple Printing failed. All jobs didn't print.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Printer driver installation failed.");
                return false;
            }

            TraceFactory.Logger.Info("IPPS Printing Success");
            return true;
        }

        /// <summary>
        ///Validate Certificate File Extension
        /// </summary>
        /// <param name="filePath">filepath</param>        
        /// <param name="enablePrivateKey">true/false</param>              
        /// <returns>true if all files printed successfully, false otherwise</returns>
        private static bool ValidateFileExtension(CertificateManagementActivityData activityData, string filePath, bool enablePrivateKey)
        {
            if (!activityData.ProductFamily.ToString().EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
            {
                if (enablePrivateKey ? Path.GetExtension(filePath).Equals("pfx") : Path.GetExtension(filePath).Equals("cer"))
                {
                    TraceFactory.Logger.Info("Failed to validate the extension: {0}".FormatWith(Path.GetExtension(filePath)));
                    return false;
                }
                TraceFactory.Logger.Info("Successfully validated the extension: {0}".FormatWith(Path.GetExtension(filePath)));
                return true;
            }
            else
            {
                if (Path.GetExtension(filePath).Equals("cer"))
                {
                    TraceFactory.Logger.Info("Failed to validate the extension .cer");
                    return false;
                }
                TraceFactory.Logger.Info("Successfully validated the extension: .cer");
                return true;
            }
        }
    }
    #endregion
}
