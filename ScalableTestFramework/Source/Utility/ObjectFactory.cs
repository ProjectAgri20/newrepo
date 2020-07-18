using System;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Attribute used to decorate a class that can be instantiated using <see cref="ObjectFactory" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ObjectFactoryAttribute : Attribute
    {
        /// <summary>
        /// Gets the key used for factory instantiation.
        /// </summary>
        public object Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactoryAttribute" /> class.
        /// </summary>
        /// <param name="key">The key used for factory instantiation.</param>
        public ObjectFactoryAttribute(object key)
        {
            Key = key;
        }
    }

    /// <summary>
    /// A general-purpose factory that instantiates a class using an <see cref="ObjectFactoryAttribute" /> and a base class or interface.
    /// </summary>
    public static class ObjectFactory
    {
        /// <summary>
        /// Creates an object that inherits from or implements the specified type and has an <see cref="ObjectFactoryAttribute" /> with the specified key.
        /// </summary>
        /// <typeparam name="T">The base class or interface type to create.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments that will be passed into the constructor of the instantiated class.</param>
        /// <returns>An instance of the class found by the factory.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// There is no type that is assignable from type <typeparamref name="T" /> and has the specified key.
        /// <para>or</para>
        /// There are multiple types that are assignable from type <typeparamref name="T" /> and have the specified key.
        /// <para>or</para>
        /// The factory-created class does not have a constructor that matches the arguments provided.
        /// </exception>
        public static T Create<T>(object key, params object[] args)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Type baseType = typeof(T);
            var candidates = Assembly.GetAssembly(baseType).GetTypes().Where(n => baseType.IsAssignableFrom(n) && HasAttributeWithKey(n, key));

            try
            {
                return (T)Activator.CreateInstance(candidates.Single(), args);
            }
            catch (InvalidOperationException)
            {
                if (!candidates.Any())
                {
                    throw new InvalidOperationException($"Could not find object of type {baseType.FullName} with key '{key}'.");
                }
                else
                {
                    throw new InvalidOperationException($"Found {candidates.Count()} objects of type {baseType.FullName} with key '{key}'.");
                }
            }
            catch (MissingMethodException)
            {
                throw new InvalidOperationException($"{candidates.First().FullName} does not have a constructor matching the specified parameters.");
            }
        }

        private static bool HasAttributeWithKey(Type type, object key)
        {
            return type.GetCustomAttributes<ObjectFactoryAttribute>().Any(n => n.Key.Equals(key));
        }
    }
}
