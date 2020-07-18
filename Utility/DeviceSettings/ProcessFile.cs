using System;
using System.IO;


namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Reads or writes device WS* information to and from file.
    /// </summary>
    class ProcessFile
    {
        public DataFims ListDeviceSettings { get; set; }
        public string ErrorMessage { get; private set; }
        public string SaveDirectory { get; set; }
        public string FileName { get; set; }
        public string IPAddress { get; set; }
        public string DeviceName { get; set; }
        public string OrigFIM { get; set; }
        public string NewFIM { get; set; }

        public ProcessFile(string fileName, string path, DataFims lstDeviceSettings)
        {
            FileName = fileName;
            SaveDirectory = path;
            ListDeviceSettings = lstDeviceSettings;
        }
        public ProcessFile(string filePathName)
        {
            FileName = filePathName;
            ListDeviceSettings = new DataFims();
        }
        public bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(ErrorMessage) == false) ? true : false;
            }
        }
        /// <summary>
        /// Writes the data to either the given or current working directory
        /// </summary>
        /// <returns>bool: true on success</returns>
        public bool WriteDeviceInfo()
        {

            if (string.IsNullOrEmpty(SaveDirectory))
            {
                SaveDirectory = Directory.GetCurrentDirectory();
            }

            string pathName = SaveDirectory + @"\" + FileName;
            FileStream fs = null;
            string header = "Source,Region,Element,Before FIM Value,After FIM Value, SameValue";
            string deviceInfo = IPAddress + "," + DeviceName + "," + OrigFIM + "," + NewFIM + "," + DateTime.Now.ToString() + "," + "True";
            try
            {
                fs = new FileStream(pathName, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
                    sw.WriteLine(header);
                    sw.WriteLine(deviceInfo);
                    foreach (DataFim data in ListDeviceSettings)
                    {
                        string line = data.EndPoint + "," + data.Parent + "," + data.Element + "," + data.ValueOrig + "," + data.ValueNew + "," + data.SameValue;
                        sw.WriteLine(line);
                    }
                }
            }
            catch (FileNotFoundException fe) { ErrorMessage = fe.Message; }
            catch (DirectoryNotFoundException fe) { ErrorMessage = fe.Message; }
            catch (IOException e) { ErrorMessage = e.Message; }
            finally
            {
                if (fs != null) { fs.Dispose(); }
            }

            return !IsError;
        }
        /// <summary>
        /// Loads the data members
        /// </summary>
        /// <param name="ipAddr">string</param>
        /// <param name="deviceNm">string</param>
        /// <param name="origFim">string</param>
        /// <param name="newFim">string</param>
        public void SetDeviceInfo(string ipAddr, string deviceNm, string origFim, string newFim)
        {
            IPAddress = ipAddr;
            DeviceName = deviceNm;
            OrigFIM = origFim;
            NewFIM = newFim;
        }
        /// <summary>
        /// Reads data from file and re-inserts back into lists and grid.
        /// </summary>
        /// <returns>bool: true on success</returns>
        public bool LoadDeviceData()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                using(StreamReader sr = new StreamReader(fs))
                {
                    GetDeviceInfo(sr);
                    while(sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        string[] buffer = line.Split(',');

                        DataFim df = new DataFim();

                        df.EndPoint = buffer[0];
                        df.Parent = buffer[1];
                        df.Element = buffer[2];
                        df.ValueOrig = buffer[3];
                        df.ValueNew = buffer[4];
                        df.SameValue = GetSameValue(buffer[5]);

                        ListDeviceSettings.Add(df);
                    }
                }
            }
            catch (FileNotFoundException fe) { ErrorMessage = fe.Message; }
            catch (DirectoryNotFoundException fe) { ErrorMessage = fe.Message;  }
            catch (IOException e) { ErrorMessage = e.Message; }
            finally
            {
                if (fs != null) { fs.Dispose(); }
            }
            return !IsError;
        }
        /// <summary>
        /// First line is the header. The 2nd contains the device IP Address, name and firmware.
        /// </summary>
        /// <param name="sr">StreamReader</param>
        private void GetDeviceInfo(StreamReader sr)
        {

            if (sr.Peek() >= 0)
            {
                // move past the header line
                string line = sr.ReadLine();

                // Device Information
                line = sr.ReadLine();
                string[] buffer = line.Split(',');

                IPAddress = buffer[0];
                DeviceName = buffer[1];
                OrigFIM = buffer[2];
                NewFIM = buffer[3];
            }
        }

        private bool GetSameValue(string value)
        {
            return value.Equals("TRUE");
        }
    }
}
