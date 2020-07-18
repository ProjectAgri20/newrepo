using System;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Allows an assembly to specify a dependency on another assembly, forcing it to be copied to all output locations.
    /// </summary>
    /// <remarks>
    /// If a project A has a reference to assembly B, the compiler knows to copy B to the output directory of A.
    /// If a project X refers to A, the compiler is usually smart enough to copy both A and B to the output directory for X.
    /// However, if A's dependency on B is only through reflection or some other trickery, the compiler will still copy B
    /// into A's output, but may "miss" B when copying to X's output.  This causes X to fail at runtime because B is not there.
    /// 
    /// A concrete example of this is EntityFramework.dll and EntityFramework.SqlServer.dll.  Data models must refer to both,
    /// but don't actually use any of the SQL types - EntityFramework.dll loads them at runtime.  To ensure that anything
    /// referencing the data model DLL gets both assemblies copied to its output, this attribute should be applied to the
    /// data model DLL with a type from the EntityFramework.SqlServer.dll.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class AssemblyDependencyAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="Type" /> provided to this instance.
        /// </summary>
        public Type Dependency { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDependencyAttribute" /> class.
        /// </summary>
        /// <param name="dependency">A <see cref="Type" /> from the assembly that the decorated assembly depends on.</param>
        public AssemblyDependencyAttribute(Type dependency)
        {
            // Don't need to do much here - passing the type into the constructor is enough
            // to indicate a dependency on the assembly containing the referenced type.
            Dependency = dependency;
        }
    }
}
