using System.Collections.Generic;
using System.Linq;
using Vim25Api;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Specifies a filter used to select a set of objects in a vSphere query. 
    /// </summary>
    public sealed class VSphereSelectionSpec
    {
        /// <summary>
        /// Gets the underlying <see cref="Vim25Api.SelectionSpec" /> this instance wraps.
        /// </summary>
        internal SelectionSpec SelectionSpec { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereSelectionSpec" /> class.
        /// </summary>
        /// <param name="selectionSpecName">The name of the selection spec to reference/reuse.</param>
        public VSphereSelectionSpec(string selectionSpecName)
        {
            SelectionSpec = new SelectionSpec
            {
                name = selectionSpecName
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereSelectionSpec" /> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to extend the query from.</param>
        /// <param name="path">The name of the property to use to select objects.</param>
        public VSphereSelectionSpec(VSphereManagedObjectType managedObjectType, string path)
            : this(managedObjectType, path, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereSelectionSpec" /> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to extend the query from.</param>
        /// <param name="path">The name of the property to use to select objects.</param>
        /// <param name="selectSet">An optional set of selections to specify additional objects to retrieve.</param>
        public VSphereSelectionSpec(VSphereManagedObjectType managedObjectType, string path, params VSphereSelectionSpec[] selectSet)
            : this(null, managedObjectType, path, selectSet)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereSelectionSpec" /> class.
        /// </summary>
        /// <param name="name">The name to assign this selection spec, which allows it to be referenced/reused.</param>
        /// <param name="managedObjectType">The type of vSphere managed object to extend the query from.</param>
        /// <param name="path">The name of the property to use to select objects.</param>
        public VSphereSelectionSpec(string name, VSphereManagedObjectType managedObjectType, string path)
            : this(name, managedObjectType, path, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereSelectionSpec" /> class.
        /// </summary>
        /// <param name="name">The name to assign this selection spec, which allows it to be referenced/reused.</param>
        /// <param name="managedObjectType">The type of vSphere managed object to extend the query from.</param>
        /// <param name="path">The name of the property to use to select objects.</param>
        /// <param name="selectSet">An optional set of selections to specify additional objects to retrieve.</param>
        public VSphereSelectionSpec(string name, VSphereManagedObjectType managedObjectType, string path, params VSphereSelectionSpec[] selectSet)
        {
            IEnumerable<SelectionSpec> set = Enumerable.Empty<SelectionSpec>();
            if (selectSet != null)
            {
                set = selectSet.Select(n => n.SelectionSpec);
            }

            SelectionSpec = new TraversalSpec
            {
                name = name,
                type = managedObjectType.ToString(),
                path = path,
                selectSet = set.ToArray()
            };
        }
    }
}
