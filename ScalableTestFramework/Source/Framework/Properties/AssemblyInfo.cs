using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("STF.Framework")]
[assembly: AssemblyDescription("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b91dcc51-8664-49e3-97d0-6fcfd2918a1a")]

// Allow core and development assemblies to initialize framework services
[assembly: InternalsVisibleTo("STF.PluginFrameworkServices")]
[assembly: InternalsVisibleTo("STF.Development")]
