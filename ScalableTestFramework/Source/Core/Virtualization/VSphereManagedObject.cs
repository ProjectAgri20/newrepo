using System;
using System.Collections.Generic;
using HP.ScalableTest.Utility;
using Vim25Api;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// A reference to a vSphere managed object.
    /// </summary>
    public sealed class VSphereManagedObject
    {
        /// <summary>
        /// Gets the underlying <see cref="Vim25Api.ManagedObjectReference" /> this instance wraps.
        /// </summary>
        internal ManagedObjectReference ManagedObjectReference { get; }

        /// <summary>
        /// Gets the name of this object.
        /// </summary>
        public string Name => GetPropertyOrDefault("name");

        /// <summary>
        /// Gets the type of the vSphere managed object.
        /// </summary>
        public VSphereManagedObjectType ObjectType { get; }

        /// <summary>
        /// A collection of properties for this instance.
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereManagedObject" /> class.
        /// </summary>
        /// <param name="managedObjectReference">The underlying <see cref="Vim25Api.ManagedObjectReference" /> this instance wraps.</param>
        /// <exception cref="ArgumentNullException"><paramref name="managedObjectReference" /> is null.</exception>
        internal VSphereManagedObject(ManagedObjectReference managedObjectReference)
        {
            ManagedObjectReference = managedObjectReference ?? throw new ArgumentNullException(nameof(managedObjectReference));
            ObjectType = EnumUtil.Parse<VSphereManagedObjectType>(managedObjectReference.type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereManagedObject" /> class.
        /// </summary>
        /// <param name="objectContent">An <see cref="ObjectContent" /> containing the managed object information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="objectContent" /> is null.</exception>
        internal VSphereManagedObject(ObjectContent objectContent)
            : this(objectContent?.obj)
        {
            if (objectContent == null)
            {
                throw new ArgumentNullException(nameof(objectContent));
            }

            if (objectContent.propSet != null)
            {
                foreach (DynamicProperty property in objectContent.propSet)
                {
                    Properties[property.name] = property.val;
                }
            }
        }

        /// <summary>
        /// Gets the value of the specified property, if it exists, or null otherwise.
        /// </summary>
        /// <param name="propertyName">The name of the property value to retrieve.</param>
        /// <returns>The value of the specified property, if it exists; otherwise, null.</returns>
        public string GetPropertyOrDefault(string propertyName)
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the value of the specified property, if it exists, or null otherwise.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <param name="propertyName">The name of the property value to retrieve.</param>
        /// <returns>The value of the specified property, if it exists; otherwise, the default for type <typeparamref name="T" />.</returns>
        /// <exception cref="FormatException">The data could not be converted to the specified type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidCastException">Conversion to the specified type <typeparamref name="T" /> is not supported.</exception>
        public T GetPropertyOrDefault<T>(string propertyName) where T : struct
        {
            if (Properties.TryGetValue(propertyName, out object value))
            {
                Type targetType = typeof(T);
                if (targetType.IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value.ToString(), true);
                }
                else
                {
                    return (T)Convert.ChangeType(value, targetType);
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name ?? ManagedObjectReference.Value;
        }
    }
}
