using System.Collections.Generic;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManifestComponentAgent
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> RequestedAssets { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manifest"></param>
        void AssignManifestInfo(SystemManifest manifest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        void LogComponents(string sessionId);
    }
}
