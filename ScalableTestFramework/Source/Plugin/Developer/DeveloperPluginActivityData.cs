using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Developer
{
    internal enum DeveloperPluginResult
    {
        Random,
        Passed,
        Failed,
        Skipped,
        Error,
        ThrowException
    }

    [DataContract]
    internal class DeveloperPluginActivityData
    {
        [DataMember]
        public DeveloperPluginResult Result { get; set; }
    }
}
