using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    public class HpacSimulationController
    {
        private string _serviceUrl;
        private string _domain;
        private const string DPRAA = "DPRAA/DPR.asmx";
        private const string Authenicator = "Ad-Authenticator/Ad-Authenticator.asmx";

        /// <summary>
        /// Constructor, takes the domain and the name of the HPAC server then combines them for the webservice domain
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <param name="serverName">HPAC server name</param>
        public HpacSimulationController(string domain, string serverName, NetworkCredential user)
        {
            _domain = domain;
            _serviceUrl = $"http://{serverName}.{domain}";
            UserCredential = user;
        }

        public NetworkCredential UserCredential { get; set; }

        /// <summary>
        /// Executes the Capella webservice for HPAC
        /// </summary>
        /// <param name="url">The domain of the webservice</param>
        /// <param name="service">The service name or location</param>
        /// <param name="function">The function of the webservice</param>
        /// <param name="args">Arguments that the webservice uses to process requests</param>
        /// <returns>A string of XML data about the response</returns>
        private static XDocument ExecuteWebService(string url, string service, string function, string args)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create($"{url}/{service}/{function}");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Proxy = null;
            webRequest.UseDefaultCredentials = true; // Uses Windows Credentials to verify user
            byte[] bytes = Encoding.ASCII.GetBytes(args);

            // Get the response
            try
            {
                string response = HttpWebEngine.Post(webRequest, bytes).Response;
                if (response?.StartsWith("<") != true)
                {
                    response = string.Format("<response>{0}</response>", response);
                }
                return XDocument.Parse(response);
            }
            catch (WebException ex)
            {
                throw new WebException($"Unable to get webResponse object for {webRequest.RequestUri.ToString()}", ex);
            }
        }

        /// <summary>
        /// Creates a Collection of jobs on the print server.
        /// </summary>
        /// <returns>A Collection of File objects that contains all of the jobs.</returns>
        public List<File> GetJobCollection()
        {
            List<File> result = new List<File>();

            XDocument response = ExecuteWebService(_serviceUrl, DPRAA, "GetFilesByUser", "username=" + UserCredential.UserName);

            XElement node = response.Element("Files");
            if (node != null)
            {
                foreach (XElement child in node.Nodes())
                {
                    int tempint;
                    bool successparse;
                    //File newFile = Xml.Serializer.Deserialize<File>(child.OuterXml);
                    File newFile = new File();
                    newFile.Guid = (string)child.Element("Guid");
                    newFile.JobName = (string)child.Element("JobName");
                    successparse = int.TryParse((string)child.Element("Size"), out tempint);
                    if (successparse)
                    {
                        newFile.Size = tempint;
                    }
                    //newFile.Size = (int)child.Element("Size");
                    successparse = int.TryParse((string)child.Element("TotalPages"), out tempint);
                    if (successparse)
                    {
                        newFile.TotalPages = tempint;
                    }
                    newFile.ApplicationName = (string)child.Element("ApplicationName");
                    newFile.SubmittedDate = (string)child.Element("SubmittedDate");
                    result.Add(newFile);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds and returns the oldest job in the list.
        /// Note: Excel often creates multiple print jobs for a single document.
        /// This method relies on the fact that STF embeds a GUID into the print job filename
        /// thus allowing us to find all print jobs containing the same unique filename.
        /// </summary>
        /// <returns>A collection of print jobs, or null.</returns>
        public List<File> GetFirstJob()
        {
            List<File> jobs = GetJobCollection();
            File firstJob = jobs.FirstOrDefault();

            if (firstJob != null)
            {
                //TraceFactory.Logger.Debug("Guid:{0}  JobName:{1}",firstJob.Guid, firstJob.JobName));
                return new List<File>(jobs.Where(j => j.JobName == firstJob.JobName).ToList());
            }

            //No Jobs available
            return null;
        }

        /// <summary>
        /// Pulls a single job, with the option to retain or delete it after pulling.
        /// </summary>
        /// <param name="deviceAddress">The IP address of the printer/device</param>
        /// <param name="printJobId">The guid of the job to be pulled</param>
        /// <param name="deleteAfterPull">Whether or not the job should be deleted after pulling</param>
        /// <returns>Empty string if successful, or error message</returns>
        public string PullJob(string deviceAddress, string printJobId, bool deleteAfterPull)
        {
            XDocument response = ExecuteWebService(_serviceUrl, DPRAA, "SendFileByUser", $"username={UserCredential.UserName}&ipaddress={deviceAddress}&guid={printJobId}&delete={Convert.ToInt32(deleteAfterPull).ToString()}");
            return response.Root.Value;
        }

        /// <summary>
        /// Deletes the specified print job from the HPAC server.
        /// </summary>
        /// <param name="printJobGuid">The Job Id of the print job that is to be deleted.</param>
        /// <returns>Returns a bool indicating whether or not the job was successfully deleted.</returns>
        public bool DeleteJob(string printJobGuid)
        {
            XDocument response = ExecuteWebService(_serviceUrl, DPRAA, "DeleteFileByUser", $"username={UserCredential.UserName}&guid={printJobGuid}");
            XElement el = response.Elements().Last();

            if (el != null)
            {
                // If present, the file was not found
                return (el.Attribute("xsi:nil") == null);
            }

            // The delete attempt failed.
            return false;
        }

        /// <summary>
        /// Calls the webservice to authenticate a user with the corresponding PIC. 
        /// This function assumes that you have set up the active directory with pics for each user.
        /// Without a pic, it will fail.
        /// </summary>
        /// <param name="userPic">personal identification code (PIC)</param>
        public void AuthenticateUser(string userPic)
        {
            XDocument response = ExecuteWebService(_serviceUrl, Authenicator, "UserAuthenticate", $"Domain={_domain}&Username=&Password=&Card=&Code={userPic}&Device=");
            //XmlDocument y = new XmlDocument();
            //y.SelectSingleNode
            XElement node = response.Descendants("success/user").FirstOrDefault(); //("success/user");
            if (node == null)
            {
                //Not successful. Get additional information            
                string[] nodes = new string[] { "failure", "message" };
                string message = string.Empty;

                for (int i = 0; i < nodes.Length; i++)
                {
                    node = response.Descendants(nodes[i]).FirstOrDefault();// .SelectSingleNode(nodes[i]);
                    if (node != null)
                    {
                        message = node.Value;// .InnerText;
                        break;
                    }
                }
                throw new WebException(string.Format("Authentication Failure: {0}.  {1}", string.IsNullOrEmpty(message) ? response.Root.Value : message, userPic));
            }

            // Continue evaluating the response
            //XmlNodeList userNode = node.SelectNodes("username");
            //XmlNodeList codeNode = node.SelectNodes("code");

            IEnumerable<XElement> userNode = response.Descendants("username");
            IEnumerable<XElement> codeNode = response.Descendants("code");


            string authenticatedUser = string.Empty;
            if (userNode.Count() < 1 || string.IsNullOrEmpty(authenticatedUser = userNode.FirstOrDefault().Value))
            {
                throw new WebException($"Authentication Failure. Invalid data returned from the server. User:{UserCredential.UserName} PIC:{userPic} Server Response:{node.Value}");
            }

            if (!authenticatedUser.Equals(UserCredential.UserName, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new WebException($"Authentication Failure. Credential Mismatch. User:{UserCredential.UserName} PIC:{userPic} Server Response:{node.Value}");
            }

            Framework.Logger.LogInfo($"User Authenticated: {authenticatedUser}  Code: {codeNode.FirstOrDefault().Value}");
        }

        /// <summary>
        /// Pulls (and removes from the print queue) all of the jobs for the given UserName
        /// </summary>
        /// <param name="deviceIP">Printer/device IP address</param>
        /// <returns>Empty string if successful, or error message</returns>
        public string PullAllJobs(string deviceIP)
        {
            XDocument response = ExecuteWebService(_serviceUrl, DPRAA, "PrintAll", $"user={UserCredential.UserName}&ipaddress={deviceIP}");

            return response.Root.Value;
        }

        /// <summary>
        /// Deletes all print jobs from the HPAC server for the specified user.
        /// </summary>
        public void DeleteAllJobs()
        {
            List<File> jobs = GetJobCollection();

            foreach (File job in jobs)
            {
                DeleteJob(job.Guid);
            }
        }
    }

    /// <summary>
    /// Stores each file in the print queue as an object
    /// </summary>
    [Serializable]
    public class File
    {
        /// <summary>
        /// The ID of the file.  SPP Express uses a Guid.  SPP Enterprise uses a URI.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// The name of the print job
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// The application the job came from
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The total pages of the job
        /// </summary>
        public int? TotalPages { get; set; }

        /// <summary>
        /// The size of the job
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// When the job was submitted
        /// </summary>
        public string SubmittedDate { get; set; }
    }
}
