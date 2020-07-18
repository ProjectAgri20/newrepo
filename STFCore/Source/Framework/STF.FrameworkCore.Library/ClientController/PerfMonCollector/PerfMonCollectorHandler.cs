using System;
using System.Diagnostics;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Properties;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Handles the creation and state changes for an PerfMonCollector Virtual Resource
    /// </summary>
    [VirtualResourceHandler(VirtualResourceType.PerfMonCollector)]
    internal class PerfMonCollectorHandler : VirtualResourceHandler
    {
        /// <summary>
        /// Creates a new PerfMonCollectorHandler instance.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public PerfMonCollectorHandler(SystemManifest manifest)
            : base(manifest)
        {
        }

        /// <summary>
        /// Creates all PerfMonCollector virtual resources.
        /// </summary>
        public override void Start()
        {
            var collector = SystemManifest.Resources.OfType<PerfMonCollectorDetail>().First();

            // Iterate through each resource instance and create a process for each one
            TraceFactory.Logger.Debug("Now creating PerfMonCollector on {0}".FormatWith(collector.Name));
            ChangeResourceState(RuntimeState.Starting);
            CreatePerfMonCollector(collector.ResourceId, collector.Name);
        }

        private static void CreatePerfMonCollector(Guid resourceId, string hostName)
        {
            string commandLine = Resources.PerfMonCollectorApplicationFileName;
            string arguments = "{0} {1}".FormatWith(resourceId.ToString(), hostName);
            TraceFactory.Logger.Debug("Executing {0} {1}".FormatWith(commandLine, arguments));

            using (Process p = new Process())
            {
                p.StartInfo.FileName = commandLine;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                p.Start();
            }
        }
    }
}