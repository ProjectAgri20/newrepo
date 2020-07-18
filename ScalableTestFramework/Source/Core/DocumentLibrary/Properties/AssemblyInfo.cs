using System;
using System.Reflection;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("STF.Core.DocumentLibrary")]
[assembly: AssemblyDescription("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("270d101e-efdb-4b57-883c-47937fe76f5d")]

// Require dependency on the Entity Framework SQL classes
[assembly: AssemblyDependency(typeof(System.Data.Entity.SqlServer.SqlProviderServices))]
