using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using Newtonsoft.Json;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.Framework.Assets;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    /// <summary>
    /// Used to execute the activity of the LowLevelConfig plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class LowLevelConfigExecutionControl : UserControl, IPluginExecutionEngine
    {
        private StringBuilder _logText = new StringBuilder();
        private LowLevelConfigActivityData _activityData;
        private PluginExecutionData _executionData;
        private NVRAMMapping _mappedData;
        private string _webServiceURL;

        /// <summary>
        /// Initializes a new instance of the LowLevelConfigExecutionControl class.
        /// </summary>
        public LowLevelConfigExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the LowLevelConfig activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            //PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Failed to complete successfully");
            bool result = false;
            _webServiceURL = executionData.Environment.PluginSettings["FalconWebServiceURL"];
            _executionData = executionData;
            _activityData = executionData.GetMetadata<LowLevelConfigActivityData>();
            _mappedData = new NVRAMMapping();
            Dictionary<string, string> _FWMapping = new Dictionary<string, string>();
            TimeSpan lockTimeout = TimeSpan.FromMinutes(30);
            TimeSpan holdTimeout = TimeSpan.FromMinutes(30);
            ConcurrentDictionary<string, bool> results = new ConcurrentDictionary<string, bool>();
            if (_executionData.Assets.OfType<IDeviceInfo>().Count() == 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"There were no assets retrieved.  If this is a count-based run, your reservation in asset inventory may have expired.", "DeviceInfo Asset error");
            }


            ///1. If a path exists, check the folder structure for folder names of the devices with FW in them.
            ///2. If any errors occur, throw with the product type that we failed to find fw for
            ///3. Consider Mapping the device to to the folder path and grabbing it within the parallel loop. Alternatively we can just look for its folder knowing the base path.


            //Code saved as it may be necessary to use Falcon fim client to update FW in the future.

            //if (!string.IsNullOrWhiteSpace(_activityData.FimBundle) && !_activityData.FimBundle.Contains(".bdl"))
            //{
            //    try
            //    {
            //        DirectoryInfo info = new DirectoryInfo(_activityData.FimBundle);
            //        DirectoryInfo[] Folders = info.GetDirectories();

            //        var productNames = _executionData.Assets.OfType<IDeviceInfo>().Select(x => x.ProductName.ToLower()).Distinct();
            //        HashSet<string> DevicesWithNoFW = new HashSet<string>();

            //        foreach (var product in productNames)
            //        {
            //            var associatedFolder = Folders.FirstOrDefault(n => product.Contains(n.Name.ToLower()));
            //            if (null != associatedFolder)
            //            {
            //                string[] files = Directory.GetFiles(associatedFolder.FullName, "*.bdl");
            //                if (files.Count() == 0)
            //                {
            //                    throw new FileNotFoundException($"Unable to find a .bdl file of device of type {product} in folder {associatedFolder}");
            //                }

            //                _FWMapping.Add(product, files.FirstOrDefault());
            //            }
            //            else
            //            {
            //                throw new FileNotFoundException($"Unable to match {product} with a FW folder");
            //            }
            //        }

            //    }
            //    catch (DirectoryNotFoundException)
            //    {
            //        return new PluginExecutionResult(PluginResult.Failed, "Unable to find folder Directory");
            //    }
            //    catch (FileNotFoundException e)
            //    {
            //        return new PluginExecutionResult(PluginResult.Failed, e.Message);
            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //}



            //List<AssetLockToken> assetTokens = _executionData.Assets.OfType<IDeviceInfo>().Select(n => new AssetLockToken(n, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5))).ToList();

            ExecutionServices.SystemTrace.LogDebug("Entering the abyss");
            try
            {
                Parallel.ForEach(_executionData.Assets.OfType<IDeviceInfo>(), new ParallelOptions { MaxDegreeOfParallelism = 10 },
                   asset =>
                   {
                       try
                       {
                           //bool done = false;
                           ExecutionServices.SystemTrace.LogDebug($"Processing { (object)asset.AssetId} on thread { (object)Thread.CurrentThread}");
                           JediDevice device = SetDefaultPassword(asset.Address, asset.AdminPassword);

                           //Keeping as a list because critical section requires it

                           AssetLockToken assetToken = new AssetLockToken(asset, lockTimeout, holdTimeout);

                           ExecutionServices.CriticalSection.Run(assetToken, () =>
                           {

                               ExecutionServices.SystemTrace.LogDebug($"Performing update on device {asset.AssetId} at address {asset.Address}");
                               result = UpdateDevice(asset, _activityData);
                               results.AddOrUpdate(asset.AssetId, result, (key, oldValue) => UpdateDevice(asset, _activityData));

                               //Wait for status to be done


                               //if (!string.IsNullOrEmpty(_activityData.FimBundle))
                               //{
                               //    string json = string.Empty;
                               //    if (_FWMapping.ContainsKey(asset.ProductName.ToLower()))
                               //    {
                               //        json = @"{""printer"":""" + $@"{asset.Address}""" + $@", ""bundle_file_path"": ""{_FWMapping[asset.ProductName.ToLower()].Replace(@"\", @"\\")}"",""username"":""admin"", ""password"":" + $@"""{device.AdminPassword}""" + "}";
                               //    }
                               //    else if(_activityData.FimBundle.Contains(".bdl"))
                               //    {
                               //        json = @"{""printer"":""" + $@"{asset.Address}""" + $@", ""bundle_file_path"": ""{_activityData.FimBundle.Replace(@"\", @"\\")}"",""username"":""admin"", ""password"":" + $@"""{device.AdminPassword}""" + "}";

                               //    }

                               //    results.AddOrUpdate(asset.AssetId, result, (key, oldValue) => (PostFim(asset.AssetId, json)));

                               //}
                           });
                       }
                       catch (Exception e)
                       {
                           //Update DIctionary, Log Error, proceed
                           ExecutionServices.SystemTrace.LogDebug(e);
                           results.AddOrUpdate(asset.AssetId, result, (key, oldValue) => false);

                       }
                   });

            }
            catch
            {
                return new PluginExecutionResult(PluginResult.Error, "Error during NVRAM Setup");
            }

            foreach (var item in results)
            {
                string passFail = item.Value ? "Passed" : "Failed";
                UpdateStatus($"Device {item.Key}: Result: {passFail}");
            }

            //CheckResults from dictionary
            PluginExecutionResult finalResult;
            if (results.Select(x => x.Value).Contains(false))
            {
                finalResult = new PluginExecutionResult(PluginResult.Failed);
            }
            else
            {
                finalResult = new PluginExecutionResult(PluginResult.Passed);
            }

            return finalResult;
        }

        //private bool PostFim(string assetID, string json)
        //{
        //    ExecutionServices.SystemTrace.LogDebug($"json: {json}");
        //    var locationPost = @"/jedi/fim";
        //    try
        //    {
        //        string id = POST(_webServiceURL, locationPost, json);

        //        string status = GET(_webServiceURL, locationPost, id);

        //        while (status.Contains("PENDING"))
        //        {
        //            status = GET(_webServiceURL, locationPost, id);
        //            Thread.Sleep(20000);
        //        }

        //        if (!status.ToUpper().Contains("SUCCESS"))
        //        {
        //            ExecutionServices.SystemTrace.LogDebug($"Device FW Error: {assetID} failed to update NVRAM: Issue--{status}");
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return true;
        //}
        public JediDevice SetDefaultPassword(string address, string password)
        {
            var defPWUrn = "urn:hp:imaging:con:service:security:SecurityService:AdministratorAuthenticationSettings";
            string endpoint = "security";
            JediDevice dev;
                try
                {
                    dev = new JediDevice(address, "");
                    WebServiceTicket tic = dev.WebServices.GetDeviceTicket(endpoint, defPWUrn);
                    tic.FindElement("Password").SetValue(password);
                    tic.FindElement("PasswordStatus").SetValue("set");
                    dev.WebServices.PutDeviceTicket("security", defPWUrn, tic, false);

                    dev = new JediDevice(address, password);
                }
                catch (Exception)
                {
                    dev = new JediDevice(address, password);
                }


            return dev;
        }


        //Updating status during the parallel for each will lock the UI thread
        private bool UpdateDevice(IDeviceInfo device, LowLevelConfigActivityData data)
        {
            PrintDeviceInfoInternal printDevice = device as PrintDeviceInfoInternal;
            string validationCall = string.Empty;
            Dictionary<string, string> validationValues = new Dictionary<string, string>();


        var json = CreateJson(printDevice, data, ref validationCall, ref validationValues);
            if (json == string.Empty)// || !string.IsNullOrEmpty(data.FimBundle))
            {
                return true;
            }
            ExecutionServices.SystemTrace.LogDebug(json);


            try
            {

                string callLocation = @"/jedi/nvpconfig";
                string jobId = POST(_webServiceURL, callLocation, json);

                string status = GET(_webServiceURL, callLocation, jobId);
                //UpdateStatus($"Service Call Successful on device {device.Address}, response: {status}");
                DateTime startTime = DateTime.Now;


                while (status.ToUpper().Contains("PENDING") && startTime + TimeSpan.FromMinutes(15) > DateTime.Now)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    status = GET(_webServiceURL, callLocation, jobId);
                }

                if (!status.ToUpper().Contains("SUCCESS"))
                {
                    ExecutionServices.SystemTrace.LogDebug($"Device: {device.AssetId} failed to update NVRAM: Issue{status}");
                    if (status.ToUpper().Contains("PENDING"))
                    {
                        throw new Exception($"Device Unresponsive Exception, please check on device {device.AssetId}");

                    }
                    return false;
                }
            }
            catch
            {
                throw;
            }


            //Validate here
            if (validationValues.ContainsKey("PartialClean"))
            {
                //We can't validate partial cleans.
                return true;
            }
            else
            {
                //Otherwise send the nvram call to get the values of the current requested changes.
                string endurl = @"/jedi/nvpget";
                string id = POST(_webServiceURL, endurl, validationCall);
                string temp = GET(_webServiceURL,  endurl, id);
                DateTime startTime = DateTime.Now;
                try
                {
                    while (temp.Contains("PENDING") && startTime + TimeSpan.FromMinutes(15) > DateTime.Now)
                    {
                        temp = GET(_webServiceURL, endurl, id);
                        Thread.Sleep(2000);
                        Console.WriteLine(temp);
                    }

                    //JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
                    //temp = temp.Substring(1, temp.Length - 2);
                    var items = JsonConvert.DeserializeObject<List<NVRamPackageData>>(temp);

                    foreach (var item in items)
                    {
                        switch (item.Name)
                        {
                            case "SerialNumber":
                            case "ModelNumber":
                            case "ExecutionMode":
                            case "CRDefSleep":
                                byte[] hexval = FromHex(item.HexValue);
                                string stringRep = Encoding.ASCII.GetString(hexval);

                                if (stringRep == validationValues[item.Name])
                                {
                                    continue;
                                }
                                else
                                {
                                    return false;
                                }

                            case "JDIMfgReset":   
                            case "SaveRecoverFlags":
                            case "Language":
                            case "RebootCrashMode":
                            case "SuppressUsed":
                                if (item.HexValue == validationValues[item.Name])
                                {
                                    continue;
                                }
                                else
                                {
                                    return false;
                                }


                                //break;
                            //case "PartialClean":

                            //    break;
                            default:
                                continue;
                        }


                    }


                }
                catch (Exception ex)
                {
                    ExecutionServices.SystemTrace.LogDebug(ex);
                    throw;
                }

                return true;
            }
        }

        public static byte[] FromHex(string hex)
        {
            //hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        public string CreateJson(PrintDeviceInfoInternal device, LowLevelConfigActivityData data, ref string validationCall, ref Dictionary<string, string> validationValues)
        {
            string jsonString = @"{""printer"": """ + device.Address + @""", ""nvps"":[";
            validationCall = jsonString;
            Type type = typeof(LowLevelConfigActivityData);


            Dictionary<string, string> properties = new Dictionary<string, string>();

            //Reflection Fun?
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (!prop.Name.Equals("EnableFW"))
                {
                    properties.Add(prop.Name, (string)prop.GetValue(data));
                }
            }

            var loopValues = properties.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Where(x => x.Value != "False");
            if (loopValues.Count() == 0)
            {
                return string.Empty;
            }

            foreach (var item in properties)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    switch (item.Key)
                    {
                        case "SerialNumberBool":
                            if (item.Value == "False" || string.IsNullOrWhiteSpace(item.Value))
                            {
                                continue;
                            }
                            else
                            {
                                jsonString += _mappedData.GetNVRAM(item.Key, device.SerialNumber);
                                validationCall += _mappedData.GetValidation("SerialNumber");
                                validationCall += @",";
                                validationValues.Add("SerialNumber", device.SerialNumber);
                            }

                            break;
                        case "ModelNumberBool":
                            if (item.Value == "False" || string.IsNullOrWhiteSpace(item.Value))
                            {
                                continue;
                            }
                            else
                            {
                                jsonString += _mappedData.GetNVRAM(item.Key, device.ModelNumber);
                                validationCall += _mappedData.GetValidation("ModelNumber");
                                validationCall += @",";
                                validationValues.Add("ModelNumber", device.ModelNumber);
                            }

                            break;
                        case "CRDefSleep":
                            jsonString += _mappedData.GetCRSleep(item.Value);
                            validationCall += _mappedData.GetValidation(item.Key);
                            validationValues.Add(item.Key, item.Value);
                            validationCall += @",";
                            break;
                        case "RebootCrashMode":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            switch (item.Value)
                            {
                                case "Enable":
                                    validationValues.Add(item.Key, "01000000");
                                    break;
                                case "Disable":
                                    validationValues.Add(item.Key, "00000000");
                                    break;
                                default:
                                    throw new Exception($@"Unhandled {item.Key} setting detected");
                            }
                            validationCall += @",";
                            break;


                        case "JDIMfgReset":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            switch (item.Value)
                            {
                                case "True":
                                    validationValues.Add(item.Key, "01000000");
                                    break;
                                case "False":
                                    validationValues.Add(item.Key, "00000000");
                                    break;
                                default:
                                    throw new Exception($@"Unhandled {item.Key} setting detected");
                            }
                            validationCall += @",";
                            break;
                        case "SaveRecoverFlags":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            switch (item.Value)
                            {
                                case "None":
                                    validationValues.Add(item.Key, "0000");
                                    break;
                                case "Save":
                                    validationValues.Add(item.Key, "0100");
                                    break;
                                case "Recover":
                                    validationValues.Add(item.Key, "0001");
                                    break;
                                case "SaveRecover":
                                    validationValues.Add(item.Key, "0101");
                                    break;
                                default:
                                    throw new Exception($@"Unhandled {item.Key} setting detected");
                            }
                            validationCall += @",";
                            break;
                        case "Language":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            switch (item.Value)
                            {
                                case "English":
                                    validationValues.Add(item.Key, "09040000");
                                    break;
                                case "Korean":
                                    validationValues.Add(item.Key, "12040000");
                                    break;
                                default:
                                    throw new Exception($@"Unhandled {item.Key} setting detected");
                            }
                            validationCall += @",";
                            break;
                        case "SuppressUsed":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            switch (item.Value)
                            {
                                case "True":
                                    validationValues.Add(item.Key, "01");
                                    break;
                                case "False":
                                    validationValues.Add(item.Key, "00");
                                    break;
                                default:
                                    throw new Exception($@"Unhandled {item.Key} setting detected");
                            }
                            validationCall += @",";
                            break;
                        case "ExecutionMode":
                            jsonString += _mappedData.keyMapping[item.Key][item.Value];
                            validationCall += _mappedData.GetValidation(item.Key);
                            validationValues.Add(item.Key, item.Value);
                            validationCall += @",";
                            break;
                        case "PartialClean":
                            jsonString += _mappedData.GetPartialClean();
                            //validationCall += _mappedData.GetValidation(item.Key);
                            validationValues.Add(item.Key, "True");
                            break;
                        default:
                            continue;
                    }
                    jsonString += @",";
                    
                }
            }
            jsonString = jsonString.Remove(jsonString.Length - 1);
            validationCall = validationCall.Remove(validationCall.Length - 1);
            validationCall += @"]}";

            if (jsonString.Contains("ModelNumber") || jsonString.Contains("SerialNumber"))
            {

                jsonString += @"], ""username"":""admin"", ""password"": " + $@"""{device.AdminPassword}""" + "}";

            }
            else
            {
                jsonString += @",[""845E3285-C67C-4f4b-9AA4-0AE91BD35089"",""LLDataStoreBootAction"",""hex"", ""01000000""],[""845E3285-C67C-4f4b-9AA4-0AE91BD35089"",""PMDataStoreBootAction"",""hex"", ""01000000""]], ""username"":""admin"", ""password"": " + $@"""{device.AdminPassword}""" + "}";
            }

            return jsonString;
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void UpdateStatus(string text)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogInfo(text);
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
            }
                );
        }


        private static string POST(string url, string wherepost, string json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + wherepost);
            request.Method = "POST";
            request.ContentType = "access-control-allow-headers";
            request.Proxy = null;
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(json);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            string temp = "";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    length = response.ContentLength;

                    StreamReader read = new StreamReader(receiveStream, Encoding.UTF8);
                    temp = read.ReadToEnd();

                }
            }
            catch (WebException ex)
            {
                ExecutionServices.SystemTrace.LogDebug(ex);
                // Log exception and throw as for GET example above
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(temp);

            return dict["id"];

        }
        private static string GET(string url, string wherepost, string id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}{wherepost}?id={id}");
            request.Proxy = null;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }


    }
    public class NVRamPackageData
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string HexValue { get; set; }
    }
}
