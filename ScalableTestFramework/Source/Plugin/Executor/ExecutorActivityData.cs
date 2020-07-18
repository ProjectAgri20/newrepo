using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Executor
{
    [DataContract]
    public class ExecutorActivityData
    {
        /// <summary>
        /// List of all executables
        /// </summary>
        [DataMember]
        public Collection<Executable> Executables { get; set; }

        [DataMember]
        public string SetupFileName { get; set; }

        [DataMember]
        public bool CopyDirectory { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public ExecutorActivityData()
        {
            Executables = new Collection<Executable>();
            SetupFileName = string.Empty;
        }

    }

    /// <summary>
    /// The class containing information about the Executables
    /// </summary>
    [DataContract]
    public class Executable
    {
        /// <summary>
        /// Filepath to the EXE, BAT, CMD, MSI
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// optional argument to be passed onto the executable
        /// </summary>
        [DataMember]
        public string Arguments { get; set; }

        /// <summary>
        /// optional setting specifying whether to copy the directory of the executable file
        /// </summary>
        [DataMember]
        public bool CopyDirectory { get; set; }

        /// <summary>
        /// Pass the session Id as an argument to the execution
        /// </summary>
        [DataMember]
        public bool PassSessionId { get; set; }

        /// <summary>
        /// To be used to display in the editor and execution window
        /// </summary>
        [IgnoreDataMember]
        public string FileName { get { return Path.GetFileName(FilePath); } }

        /// <summary>
        /// constructor
        /// </summary>
        public Executable()
        {
            Arguments = string.Empty;
            FilePath = string.Empty;
            PassSessionId = false;
        }

    }
}
