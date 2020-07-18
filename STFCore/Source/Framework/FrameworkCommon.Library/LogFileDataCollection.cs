using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Collection of log files and data to be serialized and sent to the dispatcher.
    /// </summary>
    [DataContract]
    public class LogFileDataCollection
    {
        /// <summary>
        /// Creates a new <see cref="LogFileDataCollection"/> instance containing the passed-in files.
        /// </summary>
        /// <param name="files">The log files</param>
        /// <returns></returns>
        public static LogFileDataCollection Create(string[] files)
        {
            LogFileDataCollection dataCollection = new LogFileDataCollection()
            {
                MachineName = Environment.MachineName
            };

            foreach (var file in files)
            {
                dataCollection.Items.Add(LogFileData.Create(file));
            }            

            return dataCollection;
        }

        /// <summary>
        /// Creates a new <see cref="LogFileDataCollection"/> instance containing all files in the specified folder.
        /// </summary>
        /// <param name="logFolder"></param>
        /// <returns></returns>
        public static LogFileDataCollection Create(string logFolder)
        {
            if (!Directory.Exists(logFolder))
            {
                return new LogFileDataCollection()
                {
                    MachineName = Environment.MachineName
                };
            }
            else
            {
                return Create(Directory.GetFiles(logFolder));
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public LogFileDataCollection()
        {
            Items = new Collection<LogFileData>();
        }

        /// <summary>
        /// Appends another <see cref="LogFileDataCollection"/> to this instance.
        /// </summary>
        /// <param name="data">The <see cref="LogFileDataCollection"/> to append</param>
        public void Append(LogFileDataCollection data)
        {
            foreach (var item in data.Items)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// The machine name where the data logs were collected.
        /// </summary>
        [DataMember]
        public string MachineName { get; set; }

        /// <summary>
        /// The <see cref="LogFileData"/> items.
        /// </summary>
        [DataMember]
        public Collection<LogFileData> Items { get; private set; }

        /// <summary>
        /// The size in MB of the <see cref="LogFileData"/> items.
        /// </summary>
        public double SizeInMb
        {
            get
            {
                double total = 0;
                foreach (LogFileData item in Items)
                {
                    total += item.SizeInBytes;
                }
                return (total / 1000000);
            }
        }

        /// <summary>
        /// Writes this collection to the specified location.
        /// </summary>
        /// <param name="path">The location where the files are written.</param>
        public void Write(string path)
        {
            Write(path, string.Empty);
        }

        /// <summary>
        /// Writes this collection to the specified location, filtering out all file names
        /// that do not contain the specified filter.
        /// </summary>
        /// <param name="path">The location where the files are written.</param>
        /// <param name="filter">The filter.  Empty string applies no filter (writes all files).</param>
        public void Write(string path, string filter)
        {
            string subPath = Path.Combine(path, MachineName);
            //TraceFactory.Logger.Debug("subPath: {0}".FormatWith(subPath));
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }

            foreach (LogFileData item in Items)
            {
                if (string.IsNullOrEmpty(filter) || item.FileName.Contains(filter))
                {
                    WriteFile(subPath, item);
                }
            }
        }

        private void WriteFile(string directoryPath, LogFileData fileData)
        {
            string filePath = Path.Combine(directoryPath, fileData.FileName);
            if (!File.Exists(filePath))
            {
                //TraceFactory.Logger.Debug("Writing Log file: {0}".FormatWith(filePath));
                File.WriteAllText(filePath, fileData.FileData);
            }
        }
    }

    /// <summary>
    /// Log file data to be serialized.  The data is compressed to be as small as possible over the network.
    /// </summary>
    [DataContract]
    public class LogFileData
    {
        [DataMember]
        private string _rawData = string.Empty;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LogFileData()
        {
        }

        /// <summary>
        /// Creates a new <see cref="LogFileData"/> instance for the specified file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static LogFileData Create(string fileName)
        {
            LogFileData data = new LogFileData();

            data.FileName = Path.GetFileName(fileName);
            data.FileData = LogFileReader.Read(fileName);

            return data;
        }

        /// <summary>
        /// The name of the log file.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// The file data.
        /// </summary>
        public string FileData 
        { 
            get { return CompressionUtil.Decompress(Convert.FromBase64String(_rawData)); }
            set { _rawData = Convert.ToBase64String(CompressionUtil.Compress(value)); }
        }

        /// <summary>
        /// The size in bytes of the compressed file data.
        /// </summary>
        public int SizeInBytes => System.Text.Encoding.ASCII.GetByteCount(_rawData);

    }
}
