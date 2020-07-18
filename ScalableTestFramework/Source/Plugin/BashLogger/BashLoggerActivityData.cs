using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.BashLogger
{
   
   
    [DataContract]
    public class BashLoggerActivityData
    {
        [DataMember]
        public LoggerAction LoggerAction { get; set; }

        [DataMember]
        public string FolderPath { get; set; }

        [DataMember]
        public int FileSplitSize { get; set; }

        public BashLoggerActivityData()
        {
            FileSplitSize = 1;
            FolderPath = string.Empty;
        }


    }

    public enum LoggerAction
    {
        Start,
        Stop,
        CollectLog
    }
}
