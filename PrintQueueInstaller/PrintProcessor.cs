using System.Collections.Generic;
using System.Collections.ObjectModel;
using HP.ScalableTest.Print.Drivers;
using Microsoft.Win32;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Static class used to detect if Print Processors are installed.
    /// </summary>
    public static class PrintProcessor
    {
        static Dictionary<DriverArchitecture, Collection<string>> _processorsByArchitecture = new Dictionary<DriverArchitecture, Collection<string>>();
        static object _lock = new object();

        /// <summary>
        /// Gets the print processors by architecture.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Dictionary<DriverArchitecture, Collection<string>> ProcessorsByArchitecture
        {
            get
            {
                lock (_lock)
                {
                    _processorsByArchitecture.Clear();

                    Collection<string> printProcessors = new Collection<string>();
                    using (RegistryKey environmentKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Print\Environments"))
                    {
                        foreach (string architectureKey in environmentKey.GetSubKeyNames())
                        {
                            using (RegistryKey printProcessorsKey = environmentKey.OpenSubKey(@"{0}\Print Processors".FormatWith(architectureKey)))
                            {
                                Collection<string> processors = null;
                                switch (architectureKey)
                                {
                                    case "Windows NT x86":
                                        processors = GetCollection(DriverArchitecture.NTx86);
                                        break;
                                    case "Windows x64":
                                        processors = GetCollection(DriverArchitecture.NTAMD64);
                                        break;
                                }

                                foreach (string printProcessor in printProcessorsKey.GetSubKeyNames())
                                {
                                    if (!printProcessors.Contains(printProcessor))
                                    {
                                        processors.Add(printProcessor);
                                    }
                                }
                            }
                        }
                    }
                }

                return _processorsByArchitecture;
            }
        }

        private static Collection<string> GetCollection(DriverArchitecture architecture)
        {
            Collection<string> processors = null;

            if (!_processorsByArchitecture.TryGetValue(architecture, out processors))
            {
                processors = new Collection<string>();
                _processorsByArchitecture.Add(architecture, processors);
            }

            return processors;
        }
    }
}
