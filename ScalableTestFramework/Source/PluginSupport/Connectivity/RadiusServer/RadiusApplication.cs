using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace HP.ScalableTest.PluginSupport.Connectivity.RadiusServer
{
    /// <summary>
    /// Represents a radius client
    /// </summary>
    public struct RadiusClient
    {
        /// <summary>
        /// The name of the client.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The IP address of the client.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// The state of the client
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The shared secret.
        /// </summary>
        public string SharedSecret { get; set; }
    }

    /// <summary>
    /// The encoding used for generating certificates.
    /// </summary>
    public enum Encoding
    {
        /// <summary>
        /// The DER encoding
        /// </summary>
        DER,
        /// <summary>
        /// The Base-64 encoding
        /// </summary>
        Base64
    }

    /// <summary>
    /// Contains the methods for configuring a radius server and the network policy.
    /// </summary>
    public static class RadiusApplication
    {
        /// <summary>
        /// Adds a client on the radius server.
        /// </summary>
        /// <param name="clientName">The name of the client.</param>
        /// <param name="address">The IP address of the client.</param>
        /// <param name="sharedSecret">The shared secret.</param>
        /// <returns>True if the client is added, else false.</returns>
        public static bool AddClient(string clientName, string address, string sharedSecret)
        {
            string command = "/C netsh nps add client name = {0} address = {1} state = ENABLE sharedsecret = {2}".FormatWith(clientName, address.ToString(), sharedSecret);

            if (ProcessUtil.Execute("cmd.exe", command).StandardOutput.Contains("Ok", StringComparison.CurrentCultureIgnoreCase))
            {
                Logger.LogInfo("Successfully added the client: {0} with address: {1} on the radius server.".FormatWith(clientName, address.ToString()));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to add the client: {0} with address: {1} on the radius server.".FormatWith(clientName, address.ToString()));
                return false;
            }
        }

        /// <summary>
        /// Deletes a client on the radius server.
        /// </summary>
        /// <param name="clientName">The name of the client.</param>
        /// <returns>True if the client is deleted, else false.</returns>
        public static bool DeleteClient(string clientName)
        {
            string command = "/C netsh nps delete client name = {0}".FormatWith(clientName);

            Logger.LogInfo("Deleting the client: {0} from radius server.".FormatWith(clientName));

            if (ProcessUtil.Execute("cmd.exe", command).StandardOutput.Contains("Ok", StringComparison.CurrentCultureIgnoreCase))
            {
                Logger.LogInfo("Successfully deleted the client: {0} from the radius server.".FormatWith(clientName));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to delete the client: {0} from the radius server.".FormatWith(clientName));
                return false;
            }
        }

        /// <summary>
        /// Get all the clients available in the radius server.
        /// </summary>
        /// <returns><see cref="RadiusClient"/></returns>
        public static Collection<RadiusClient> GetAllClients()
        {
            string command = "/C netsh nps show client";

            string data = ProcessUtil.Execute("cmd.exe", command).StandardOutput;

            string[] clientDetails = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string[] clientNames = Array.FindAll(clientDetails, x => x.Contains("Name", StringComparison.CurrentCultureIgnoreCase));
            string[] clientAddresses = Array.FindAll(clientDetails, x => x.Contains("Address", StringComparison.CurrentCultureIgnoreCase));
            string[] clientState = Array.FindAll(clientDetails, x => x.Contains("State", StringComparison.CurrentCultureIgnoreCase));
            string[] clientSharedSecrets = Array.FindAll(clientDetails, x => x.Contains("secret", StringComparison.CurrentCultureIgnoreCase));

            Collection<RadiusClient> radiusClients = new Collection<RadiusClient>();

            // TODO: find out a better approach for this.
            for (int i = 0; i < clientNames.Length; i++)
            {
                radiusClients.Add(new RadiusClient
                {
                    Name = clientNames[i].Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1],
                    Address = IPAddress.Parse(clientAddresses[i].Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim()),
                    State = clientState[i].Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1],
                    SharedSecret = clientSharedSecrets[i].Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1]
                });
            }

            return radiusClients;
        }

        /// <summary>
        /// Deletes all the clients available in the radius server.
        /// </summary>
        /// <returns>True if all the clients are deleted, else false.</returns>
        public static bool DeleteAllClients()
        {
            Collection<RadiusClient> radiusClients = GetAllClients();

            Logger.LogInfo("Deleting all the clients from radius server.");

            foreach (RadiusClient client in radiusClients)
            {
                if (!DeleteClient(client.Name))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clears the network policy from the radius server.
        /// </summary>
        /// <returns>True if the network policy is cleared from the server.</returns>
        public static bool ClearNetworkPolicy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds network policy on the radius server.
        /// </summary>
        /// <returns></returns>
        public static bool AddNetworkPolicy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the authentication mode on the radius server for the specified network policy.
        /// </summary>
        /// <param name="networkPolicy">The network policy name.</param>
        /// <param name="authenticationModes"><see cref="AuthenticationMode"/></param>
        /// <param name="priorityMode">The priority <see cref="AuthenticationMode"/> to be set on the server.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        public static bool SetAuthenticationMode(string networkPolicy, AuthenticationMode authenticationModes, AuthenticationMode priorityMode = AuthenticationMode.None)
        {
            string configurePolicy = "/C netsh nps set np name = \"{0}\" state = \"enable\" processingorder = \"1\" policysource = \"0\" conditionid = \"0x3d\" conditiondata = \"^15$|^19$|^17$|^18$\" profileid = \"0x100f\" profiledata = \"TRUE\" profileid = \"0x100a\" {1} profileid = \"0x1009\" profiledata = \"0x5\" profiledata = \"0x3\" profiledata = \"0x9\" profiledata = \"0x4\" profiledata = \"0xa\"";
            string profileData = "profiledata = \"{0}\" ";

            string authenticationMethod = string.Empty;

            Logger.LogInfo("Configuring Authentication mode: {0} on radius server.".FormatWith(authenticationModes));

            if (priorityMode != AuthenticationMode.None)
            {
                Logger.LogInfo("Priority mode: {0}.".FormatWith(priorityMode));
                authenticationMethod = profileData.FormatWith(Enum<AuthenticationMode>.Value(priorityMode));
                authenticationModes &= ~priorityMode;
            }

            foreach (AuthenticationMode item in Enum.GetValues(typeof(AuthenticationMode)))
            {
                if (item != AuthenticationMode.None && authenticationModes.HasFlag(item))
                {
                    // Construct profile data
                    authenticationMethod += profileData.FormatWith(Enum<AuthenticationMode>.Value(item));
                }
            }

            string command = configurePolicy.FormatWith(networkPolicy, authenticationMethod);

            if (ProcessUtil.Execute("cmd.exe", command).StandardOutput.Contains("Ok", StringComparison.CurrentCultureIgnoreCase))
            {
                Logger.LogInfo("Successfully configured the authentication mode: {0} {1} on the radius server.".FormatWith(authenticationModes, priorityMode == AuthenticationMode.None ? string.Empty : "with {0} as priority".FormatWith(priorityMode)));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to configure the authentication mode: {0} {1} on the radius server.".FormatWith(authenticationModes, priorityMode == AuthenticationMode.None ? string.Empty : "with {0} as priority".FormatWith(priorityMode)));
                return true;
            }
        }

        /// <summary>
        /// Generate certificate(.cer) in "C:\\Dynamic Certificate" folder in the radius server.
        /// </summary>
        /// <param name="serverIpAddress">IP address of the server.</param>
        /// <param name="certificateRequest">The certificate request string generated from the device.</param>
        /// <param name="userName">User name to access the http://serverIpAddress/certsrv/certrqxt.asp </param>
        /// <param name="password">Password to access the http://serverIpAddress/certsrv/certrqxt.asp </param>
        /// <param name="certificatePath">Returns the certificate path.</param>
        /// <param name="certificateTemplateName">The certificate template name.</param>
        /// <param name="encoding"><see cref="Encoding"/></param>
        /// <returns>True if the certificate generation is successful, else false.</returns>
        public static bool GenerateCertificate(string certificateRequest, string serverIpAddress, string userName, string password, out string certificatePath, string certificateTemplateName, Encoding encoding = Encoding.DER)
        {
            certificatePath = string.Empty;

            try
            {
                string certificateFolder = @"C:\Dynamic Certificate";
                string Executable_Path = @"{0}\{1}".FormatWith(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "Authenticate.exe");
                Logger.LogInfo("Executable path : {0}".FormatWith(Executable_Path));

                if (Directory.Exists(certificateFolder))
                {
                    foreach (FileInfo file in (new DirectoryInfo(certificateFolder).GetFiles()))
                    {
                        Logger.LogInfo("File: {0} is deleted".FormatWith(file.Name));
                        file.Delete();
                    }
                }
                else
                {
                    Logger.LogInfo("Folder: {0} is created".FormatWith(certificateFolder));
                    Directory.CreateDirectory(certificateFolder);
                }

                FirefoxProfile profile = new FirefoxProfile();
                profile.AcceptUntrustedCertificates = true;
                profile.SetPreference("browser.download.folderList", 2);
                profile.SetPreference("browser.download.manager.showWhenStarting", false);
                profile.SetPreference("browser.download.dir", certificateFolder);
                profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/x-x509-ca-cert, application/pkix-cert");

                FirefoxDriver driver = new FirefoxDriver(profile);
                //IWebDriver driver = new InternetExplorerDriver();

                driver.Navigate().GoToUrl("https://{0}/certsrv/certrqxt.asp".FormatWith(serverIpAddress));
                Thread.Sleep(TimeSpan.FromMinutes(1));

                var result = ProcessUtil.Execute(@"cmd.exe", "/C \"{0}\" {1} {2}".FormatWith(Executable_Path, userName, password));
                if (result.ExitCode == 1)
                {
                    Logger.LogDebug("No authentication popup is present.");
                }

                Thread.Sleep(TimeSpan.FromMinutes(2));
                IWebElement element = driver.FindElement(By.Id("locTaRequest"));

                Thread thread = new Thread(() => Clipboard.SetText(certificateRequest));
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();

                element.SendKeys(OpenQA.Selenium.Keys.Control + "v");
                SelectElement select = new SelectElement(driver.FindElement(By.Id("lbCertTemplateID")));

                select.SelectByText(certificateTemplateName);

                element = driver.FindElement(By.Id("btnSubmit"));
                element.Click();

                if (encoding == Encoding.DER)
                {
                    driver.FindElement(By.Id("rbDerEnc")).Click();
                }
                else
                {
                    driver.FindElement(By.Id("rbB64Enc")).Click();
                }

                driver.FindElement(By.Id("locDownloadCert3")).Click();

                Thread.Sleep(TimeSpan.FromMinutes(1));

                driver.Close();

                if (Directory.GetFiles(certificateFolder).Count() != 0)
                {
                    Logger.LogInfo("Successfully generated the certificate from radius server.");
                    certificatePath = Directory.GetFiles(certificateFolder).FirstOrDefault();
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to generate the certificate from radius server.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        public static bool OpenBrowser()
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.AcceptUntrustedCertificates = true;

            FirefoxDriver driver = new FirefoxDriver(profile);
            //IWebDriver driver = new InternetExplorerDriver();

            Logger.LogInfo("Opening browser...");

            driver.Navigate().GoToUrl("http://localhost/certsrv/certrqxt.asp");

            Logger.LogInfo("Browser is opened...");

            Thread.Sleep(TimeSpan.FromMinutes(1));

            return false;
        }

        /// <summary>
        /// Map Id certificate to the specified user.
        /// </summary>
        /// <param name="userName">The user name for which the certificate is to be mapped.</param>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <param name="certificatePassword">Password of the certificate.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        public static bool MapIdCertificate(string userName, string certificatePath, string certificatePassword = "")
        {
            X509Certificate2 certificate = string.IsNullOrEmpty(certificatePassword) ? new X509Certificate2(certificatePath) : new X509Certificate2(certificatePath, certificatePassword);
            return MapIdCertificate(userName, certificate);
        }

        private static bool MapIdCertificate(string userName, X509Certificate2 certificate)
        {
            // if we directly access issuer and subject from certificate the altSecurityIdentities' will look like this.
            // X509:< I > CN = ROOT2KSHA1 - CA, DC = r2ksha1, DC = com < S > CN = 192.168.90.26, OU = Unit, OU = CXXXXA, OU = XXXXXXXXXX, O = Hewlett - Packard Inc, L = Bangalore, S = Karnataka, C = IN
            // Expected format in radius server
            // X509:<I>DC=com,DC=r2ksha1,CN=ROOT2KSHA1-CA<S>C=IN,S=Karnataka,L=Bangalore,O=Hewlett-Packard Inc,OU=XXXXXXXXXX,OU=CXXXXA,OU=Unit,CN=192.168.90.26 - Expected in radius server
            // So reversing the Issuer and subject names and removing the empty spaces and building 'altSecurityIdentities'
            string command = "Set-ADUser -Identity \"{0}\" -Add @{1}'altSecurityIdentities' = \"X509:<I>{2}<S>{3}\"{4}".FormatWith(userName, "{", string.Join(",", certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Reverse()), string.Join(",", certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Reverse()), "}");

            Logger.LogInfo(command);

            try
            {
                string result = RunScript(command);

                RestartRadiusServices();

                if (string.IsNullOrEmpty(result) || result.EqualsIgnoreCase("\r\n"))
                {
                    Logger.LogInfo("Successfully mapped certificate: {0} to user: {1}".FormatWith(certificate, userName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to map certificate: {0} to user: {1}".FormatWith(certificate, userName));
                    Logger.LogInfo(result);
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                Logger.LogInfo("Failed to map certificate: {0} to user: {1}".FormatWith(certificate, userName));
                Logger.LogDebug(defaultException.Message);
                return false;
            }
        }

        //TODO: Remove certificate path and use certificate details.
        /// <summary>
        /// Delete Id certificate mapped to the specified user.
        /// </summary>
        /// <param name="userName">The user name for which the certificate is to be deleted.</param>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <param name="certificatePassword">Password of the certificate.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        public static bool DeleteIdCertificate(string userName, string certificatePath, string certificatePassword = "")
        {
            X509Certificate2 certificate = string.IsNullOrEmpty(certificatePassword) ? new X509Certificate2(certificatePath) : new X509Certificate2(certificatePath, certificatePassword);
            return DeleteIdCertificate(userName, certificate);
        }

        private static bool DeleteIdCertificate(string userName, X509Certificate2 certificate)
        {
            string command = "Set-ADUser -Identity \"{0}\" -Remove @{1}'altSecurityIdentities' = \"X509:<I>{2}<S>{3}\"{4}".FormatWith(userName, "{", string.Join(",", certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Reverse()), string.Join(",", certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Reverse()), "}");

            Logger.LogInfo(command);

            try
            {
                string result = RunScript(command);

                RestartRadiusServices();

                if (string.IsNullOrEmpty(result) || result.EqualsIgnoreCase("\r\n"))
                {
                    Logger.LogInfo("Successfully deleted certificate: {0} from user: {1}".FormatWith(certificate, userName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to delete certificate: {0} from user: {1}".FormatWith(certificate, userName));
                    Logger.LogInfo(result);
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                Logger.LogInfo("Failed to delete certificate: {0} from user: {1}".FormatWith(certificate, userName));
                Logger.LogDebug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Adds CA certificate to the certificate store.
        /// </summary>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <returns></returns>
        public static bool AddCACertificate(string certificatePath)
        {
            try
            {
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.MaxAllowed | OpenFlags.IncludeArchived);

                X509Certificate2 certificate = new X509Certificate2(certificatePath);

                // Remove the certificate
                store.Add(certificate);

                X509Certificate2Collection certificateCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, certificate.SerialNumber.Replace(" ", string.Empty), true);

                store.Close();

                Logger.LogInfo("Radius Server CA certificate count = {0}".FormatWith(certificateCollection.Count));

                if (certificateCollection.Count > 0)
                {
                    Logger.LogInfo("Successfully added the CA certificate: {0} to the Trusted Root Certification Authorities.".FormatWith(certificatePath));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to add the CA certificate: {0} to the Trusted Root Certification Authorities.".FormatWith(certificatePath));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to add the CA certificate: {0} to the Trusted Root Certification Authorities.".FormatWith(certificatePath));
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        //TODO: Remove certificate path and use certificate details.
        /// <summary>
        /// Deletes the CA certificate from certificate store.
        /// </summary>
        /// <param name="certificatePath">The path of the certificate.</param>
        /// <returns></returns>
        public static bool DeleteCACertificate(string certificatePath)
        {
            try
            {
                    //Certificate store created with Max persmissions
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.MaxAllowed | OpenFlags.IncludeArchived);

                X509Certificate2 certificate = new X509Certificate2(certificatePath);

                // Remove the certificate
                store.Remove(certificate);

                X509Certificate2Collection certificateCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, certificate.SerialNumber.Replace(" ", string.Empty), true);

                store.Close();

                Logger.LogInfo("Radius Server CA certificate count = {0}".FormatWith(certificateCollection.Count));

                if (certificateCollection.Count == 0)
                {
                    Logger.LogInfo("Successfully deleted the CA certificate: {0} from Trusted Root Certification Authorities.".FormatWith(certificatePath));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to delete the CA certificate: {0} from Trusted Root Certification Authorities.".FormatWith(certificatePath));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to delete the CA certificate: {0} from Trusted Root Certification Authorities.".FormatWith(certificatePath));
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the SamAccountName of the specified active directory user.
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>The SamAccountName of the specified user.</returns>
        public static string GetADUserSamAccountName(string userName)
        {
            string command = $"$user = Get-ADUser -Identity {userName}; $user.SamAccountName";
            return Regex.Replace(RunScript(command), @"\s", string.Empty);
        }

        /// <summary>
        /// Renmaes the SamAccountName of the specified active directory user.
        /// </summary>
        /// <param name="userName">The user name to be modified.</param>
        /// <param name="newName">The new user name</param>
        /// <returns>True if the SamAccountName is set, else false.</returns>
        public static bool RenameADUser(string userName, string newName)
        {
            string command = "Set-ADUser -Identity {0} -Replace @{1}samaccountname = '{2}'{3}".FormatWith(userName, "{", newName, "}");
            RunScript(command);
            return GetADUserSamAccountName(newName).EqualsIgnoreCase(newName);
        }

        /// <summary>
        /// Runs the given powershell script and returns the script output.
        /// </summary>
        /// <param name="scriptText">the powershell script text to run</param>
        /// <returns>output of the script</returns>
        private static string RunScript(string scriptText)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            Thread.Sleep(TimeSpan.FromSeconds(20));

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Restarts the Extensible Authentication Protocol and Network Policy Server services.
        /// </summary>
        public static void RestartRadiusServices()
        {
            SystemConfiguration.SystemConfiguration.StopService("IAS");
            SystemConfiguration.SystemConfiguration.StartService("IAS");

            SystemConfiguration.SystemConfiguration.StopService("Eaphost");
            SystemConfiguration.SystemConfiguration.StartService("Eaphost");
        }
    }
}
