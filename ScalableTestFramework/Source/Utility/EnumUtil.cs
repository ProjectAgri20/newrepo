using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides extension methods and utilities for working with enumerations and their attributes.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> is either an empty string or only contains white space
        /// <para>or</para>
        /// <paramref name="value" /> is not one of the named constants defined for the enumeration.
        /// </exception>
        public static TEnum Parse<TEnum>(string value) where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object, optionally ignoring case.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="ignoreCase">if set to <c>true</c> ignore case.</param>
        /// <returns>An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> is either an empty string or only contains white space
        /// <para>or</para>
        /// <paramref name="value" /> is not one of the named constants defined for the enumeration.
        /// </exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        /// <summary>
        /// Retrieves a collection of the values of the constants in a specified enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <returns>A collection of the values in <typeparamref name="TEnum" />.</returns>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        }

        /// <summary>
        /// Retrieves the value of the <see cref="DescriptionAttribute" /> decorating the specified <see cref="Enum" /> value.
        /// If no such attribute exists, returns the string value of the <see cref="Enum" />.
        /// </summary>
        /// <param name="value">The <see cref="Enum" /> for which to retrieve a description.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute" /> for the enum.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetDescription(this Enum value)
        {
            return GetDescriptionAttribute(value)?.Description ?? value.ToString();
        }

        /// <summary>
        /// Retrieves the value of the <see cref="DescriptionAttribute" /> decorating all values in the specified enumeration.
        /// Values that do not have a <see cref="DescriptionAttribute" /> are skipped.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <returns>A collection of descriptions in <typeparamref name="TEnum" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<string> GetDescriptions<TEnum>() where TEnum : Enum
        {
            foreach (TEnum value in GetValues<TEnum>())
            {
                DescriptionAttribute descriptionAttribute = GetDescriptionAttribute(value);
                if (descriptionAttribute != null)
                {
                    yield return descriptionAttribute.Description;
                }
            }
        }

        /// <summary>
        /// Gets the <typeparamref name="TEnum" /> value whose <see cref="DescriptionAttribute" /> has the specified value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="description">The description to search for.</param>
        /// <returns>The enumeration value with the specified description, or the default value if none was found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="description" /> is null.</exception>
        /// <exception cref="ArgumentException">No enumeration value with the specified description could be found.</exception>
        public static TEnum GetByDescription<TEnum>(string description) where TEnum : Enum
        {
            return GetByDescription<TEnum>(description, false);
        }

        /// <summary>
        /// Gets the <typeparamref name="TEnum" /> value whose <see cref="DescriptionAttribute" /> has the specified value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <param name="description">The description to search for.</param>
        /// <param name="ignoreCase">if set to <c>true</c> ignore case.</param>
        /// <returns>The enumeration value with the specified description, or the default value if none was found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="description" /> is null.</exception>
        /// <exception cref="ArgumentException">No enumeration value with the specified description could be found.</exception>
        public static TEnum GetByDescription<TEnum>(string description, bool ignoreCase) where TEnum : Enum
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            foreach (TEnum value in GetValues<TEnum>())
            {
                DescriptionAttribute descriptionAttribute = GetDescriptionAttribute(value);
                if (description.Equals(descriptionAttribute?.Description, comparison))
                {
                    return value;
                }
            }
            throw new ArgumentException($"Could not find {typeof(TEnum).Name} value with description '{description}'.", nameof(description));
        }

        private static DescriptionAttribute GetDescriptionAttribute(ValueType value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            return fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        }
    }
}
