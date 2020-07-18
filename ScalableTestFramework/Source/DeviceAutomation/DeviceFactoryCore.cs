using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using HP.DeviceAutomation;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Creates objects based on mappings from <see cref="IDevice" /> types.
    /// </summary>
    /// <typeparam name="TFactory">The type of objects this instance creates.</typeparam>
    /// <remarks>
    /// All implementations of the <typeparamref name="TFactory" /> type that are mapped by the factory
    /// must have a constructor that takes the corresponding <see cref="IDevice" /> as the first parameter.
    /// This constructor may have additional parameters following the device parameter.
    /// 
    /// Typical usage of this class consists of creating a concrete inherited type to define <typeparamref name="TFactory" />,
    /// then using the Add method(s) to specify the mapping from devices to the factory type.
    /// The FactoryCreate method(s) can then be used to instantiate the factory type from an <see cref="IDevice" /> instance.
    /// A singleton pattern can be used to store a static instance that can be accessed by any consumers.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyDeviceFactory : DeviceFactoryCore{MyType}
    /// {
    ///     private static MyDeviceFactory _instance = new MyDeviceFactory();
    ///
    ///     private MyDeviceFactory()
    ///     {
    ///         Add{DeviceType1, MyType1}();
    ///         Add{DeviceType2, MyType2}();
    ///     }
    ///
    ///     public static MyType Create(IDevice device) => _instance.FactoryCreate(device);
    /// }
    /// </code>
    /// </example>
    [DebuggerDisplay("Count = {Count}")]
    public class DeviceFactoryCore<TFactory>
    {
        private const BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets the number of key/value pairs in the <see cref="DeviceFactoryCore{TFactory}" />.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Count => _map.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceFactoryCore{TFactory}" /> class.
        /// </summary>
        public DeviceFactoryCore()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Adds the specified device type and mapped type.
        /// </summary>
        /// <param name="deviceType">The type of <see cref="IDevice" /> to map.</param>
        /// <param name="mappedType">The type the specified <see cref="IDevice" /> type should be mapped to.</param>
        public void Add(Type deviceType, Type mappedType)
        {
            if (!deviceType.IsClass)
            {
                throw new DeviceFactoryCoreException($"Device type {deviceType.Name} must be a class.");
            }

            if (!mappedType.IsClass)
            {
                throw new DeviceFactoryCoreException($"Factory type {mappedType.Name} must be a class.");
            }

            if (!typeof(IDevice).IsAssignableFrom(deviceType))
            {
                throw new DeviceFactoryCoreException($"{deviceType.Name} is not a valid device type.");
            }

            if (!typeof(TFactory).IsAssignableFrom(mappedType))
            {
                throw new DeviceFactoryCoreException($"{mappedType.Name} is not compatible with factory type {typeof(TFactory).Name}.");
            }

            _map.Add(deviceType, mappedType);
        }

        /// <summary>
        /// Adds a mapping from the specified device type to the specified mapped type
        /// </summary>
        /// <typeparam name="TDevice">The type of <see cref="IDevice" /> to map.</typeparam>
        /// <typeparam name="TMap">The type the specified <see cref="IDevice" /> type should be mapped to.</typeparam>
        /// <remarks>
        /// This method is similar to the non-generic Add method, but enforces type safety by ensuring that the device type and mapped
        /// type implement/inherit from <see cref="IDevice" /> and <typeparamref name="TFactory" />, respectively.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void Add<TDevice, TMap>()
            where TDevice : class, IDevice
            where TMap : class, TFactory
        {
            _map.Add(typeof(TDevice), typeof(TMap));
        }

        /// <summary>
        /// Creates the appropriate <typeparamref name="TFactory" /> object from the specified <see cref="IDevice" /> using this instance's mappings.
        /// </summary>
        /// <param name="device">The <see cref="IDevice" /> object.</param>
        /// <returns>The constructed <typeparamref name="TFactory" /> object.</returns>
        public TFactory FactoryCreate(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            object[] constructorParameters = new object[] { device };
            return FactoryCreateImpl(device, constructorParameters);
        }

        /// <summary>
        /// Creates the appropriate <typeparamref name="TFactory" /> object from the specified <see cref="IDevice" /> using this instance's mappings.
        /// </summary>
        /// <param name="device">The <see cref="IDevice" /> object.</param>
        /// <param name="parameters">The parameters to pass to the <typeparamref name="TFactory" /> object constructor.</param>
        /// <returns>The constructed <typeparamref name="TFactory" /> object.</returns>
        public TFactory FactoryCreate(IDevice device, params object[] parameters)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            object[] constructorParameters = new object[] { device }.Concat(parameters).ToArray();
            return FactoryCreateImpl(device, constructorParameters);
        }

        private TFactory FactoryCreateImpl(IDevice device, object[] constructorParameters)
        {
            Type deviceType = device.GetType();
            while (deviceType != null)
            {
                if (_map.TryGetValue(deviceType, out Type mappedType))
                {
                    try
                    {
                        return (TFactory)Activator.CreateInstance(mappedType, _bindingFlags, null, constructorParameters, CultureInfo.InvariantCulture);
                    }
                    catch (MissingMethodException ex)
                    {
                        throw new DeviceFactoryCoreException($"{mappedType.Name} does not have a constructor matching the specified parameters.", ex);
                    }
                }
                else
                {
                    deviceType = deviceType.BaseType;
                }
            }

            throw new DeviceFactoryCoreException($"The device at {device.Address} is of type {deviceType.Name}, which does not have a mapped {typeof(TFactory).Name} implementation.");
        }
    }
}
