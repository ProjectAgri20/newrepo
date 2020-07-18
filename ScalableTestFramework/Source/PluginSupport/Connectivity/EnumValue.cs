using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public static class Enum<T>
    {
        private static Dictionary<Enum, EnumValueAttribute> _enumsWithAttribute = new Dictionary<Enum, EnumValueAttribute>();

        /// <summary>
        /// Parses the given string value for the <see cref="Enum"/> value of type <typeparamref name="T"/> and returns
        /// the <see cref="EnumValueAttribute"/> value for the parsed <see cref="Enum"/>.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>
        /// String Value associated via a <see cref="EnumValueAttribute"/> attribute, or null if not found.
        /// </returns>
        /// <remarks>
        /// This method helps in the developer to provide a <see cref="System.String"/> value for a given <see cref="System.Enum"/>
        /// and then return the value defined in the <see cref="EnumValueAttribute"/> attribute, which may be used for UI display as an
        /// example.  
        /// </remarks>
        public static string Value(string valueName)
        {
            try
            {
                Enum enumType = (Enum)Enum.Parse(typeof(T), valueName);
                return Value(enumType);
            }
            catch (ArgumentException)
            {
                // Ultimately log something here.
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="EnumValueAttribute" /> string value for a specific enum value.
        /// </summary>
        /// <param name="enumType">The referenced <see cref="Enum" /></param>
        /// <returns>
        /// String Value associated via a <see cref="EnumValueAttribute" /> attribute, or enum name if not found.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">enumType</exception>
        public static string Value(Enum enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            string returnValue = null;

            if (_enumsWithAttribute.ContainsKey(enumType))
            {
                returnValue = _enumsWithAttribute[enumType].Value;
            }
            else
            {
                //Look for our 'EnumValueAttribute' in the field's custom attributes
                string enumName = enumType.ToString();
                FieldInfo fieldInfo = enumType.GetType().GetField(enumName);
                EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
                if (attributes.Length > 0)
                {
                    _enumsWithAttribute.Add(enumType, attributes[0]);
                    returnValue = attributes[0].Value;
                }
                else
                {
                    var newAttribute = new EnumValueAttribute(enumName);
                    _enumsWithAttribute.Add(enumType, newAttribute);
                    returnValue = enumName;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a Dictionary containing the <see cref="Enum"/> value and associated <see cref="EnumValueAttribute"/> values for a given <see cref="Enum"/> type.
        /// </summary>
        public static Dictionary<object, string> EnumValues
        {
            get
            {
                Dictionary<object, string> returnValue = new Dictionary<object, string>();
                Type underlyingType = Enum.GetUnderlyingType(typeof(T));
                var names = Enum.GetNames(typeof(T));

                // For each field in the enum, get the EnumValueAttribute values for each one
                foreach (FieldInfo fieldInfo in typeof(T).GetFields().Where(x => names.Contains(x.Name)))
                {
                    var enumValue = Convert.ChangeType(Enum.Parse(typeof(T), fieldInfo.Name), underlyingType, CultureInfo.InvariantCulture);
                    EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];

                    // set default value = enum value name
                    string value = fieldInfo.Name;

                    // get the EnumValueAttribute value if existing
                    if (attributes.Length > 0)
                    {
                        value = attributes[0].Value;
                    }
                    returnValue.Add(enumValue, value);
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Parses the supplied <see cref="EnumValueAttribute"/> value to find an associated enum value.
        /// </summary>
        /// <param name="value"><see cref="System.String"/> value of the <see cref="Enum"/> value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied <see cref="System.String"/> value</param>
        /// <returns>
        /// <see cref="Enum"/> value associated with the string value, or null if not found.
        /// </returns>
        public static T Parse(string value, bool ignoreCase = false)
        {
            T returnValue = default(T);

            string enumStringValue = null;

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Supplied type must be an Enum.  Type was ", typeof(T).ToString());
            }

            StringComparison comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fieldInfo in typeof(T).GetFields())
            {
                EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
                if (attributes.Length > 0)
                {
                    enumStringValue = attributes[0].Value;

                    if (string.Compare(enumStringValue, value, comparison) == 0)
                    {
                        returnValue = (T)Enum.Parse(typeof(T), fieldInfo.Name);
                        break;
                    }
                    //Changed as above condition was never occuring for VEP Security-Misc test 
                    if (enumStringValue.Contains(value))
                    {
                        returnValue = (T)Enum.Parse(typeof(T), fieldInfo.Name);
                        break;
                    }
                }
                else
                {
                    // enum value not decorated with an attribute, try the enum value name directly
                    if (string.Compare(fieldInfo.Name, value, comparison) == 0)
                    {
                        returnValue = (T)Enum.Parse(typeof(T), fieldInfo.Name);
                        break;
                    }
                }
            }

            return returnValue;
        }
    }
}
