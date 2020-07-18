using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.PrintFromJobStorage
{
    //Activity Data for Print From JobStorage
    [DataContract]
    public class PrintFromJobStorageActivityData
    {
        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the FolderName
        /// </summary>
        /// <value>Name of the folder where file exists.</value>
        [DataMember]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the PIN
        /// </summary>
        /// <value>Pin to unlock the file.</value>
        [DataMember]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [PrintAll] is to be used.
        /// </summary>
        ///   <c>true</c> if [PrintAll All]; otherwise, <c>false</c>.
        [DataMember]
        public bool PrintAll { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsPinRequired] is to be used.
        /// </summary>
        ///   <c>true</c> if [Pin Required]; otherwise, <c>false</c>.
        [DataMember]
        public bool IsPinRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of Prints.
        /// </summary>
        ///<value>Integer value between 1-9999</value>
        [DataMember]
        public int NumberOfCopies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Fax app auth (lazy)]
        /// </summary>
        [DataMember]
        public bool ApplicationAuthentication { get; set; }

        /// <summary>
        /// Gets or sets what authentication provider to use.
        /// </summary>
        [DataMember]
        public AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [DeleteJobAfterPrint] is to be used.
        /// </summary>
        ///   <c>true</c> if [DeleteJobAfterPrint]; otherwise, <c>false</c>.
        [DataMember]
        public bool DeleteJobAfterPrint { get; set; }

        /// <summary>
        /// Initializes a new instance of the PluginCollectDeviceSystemInfoActivityData class.
        /// </summary>
        /// 
        public PrintFromJobStorageActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            FolderName = string.Empty;
            Pin = string.Empty;
            PrintAll = false;
            DeleteJobAfterPrint = false;
            IsPinRequired = false;
            NumberOfCopies = 1;
            ApplicationAuthentication = true;
            AuthProvider = AuthenticationProvider.Auto;
        }
    }
}
