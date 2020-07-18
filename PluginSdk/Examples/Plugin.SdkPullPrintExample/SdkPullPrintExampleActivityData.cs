/*
* © Copyright 2016 HP Development Company, L.P.
*/
using System.Runtime.Serialization;

namespace Plugin.SdkPullPrintExample
{
    /// <summary>
    /// Contains data needed to execute the LocalPullPrintExample activity.
    /// </summary>
    [DataContract]
    internal class SdkPullPrintExampleActivityData
    {
        /// <summary>
        /// Initializes a new instance of the LocalPullPrintExampleActivityData class.
        /// </summary>
        public SdkPullPrintExampleActivityData()
        {
            // Default button name
            TopLevelButtonName = "PullPrint Local";
        }

        [DataMember]
        public string TopLevelButtonName { get; set; }
    }
}
