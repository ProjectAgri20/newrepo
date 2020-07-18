using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace HP.RDL.RdlHPMibTranslator
{
	/// <summary>
	/// Generic helper class that uses <see cref="EnumValueAttribute"/> to manage enum values.
	/// </summary>
	/// <typeparam name="T">Any <see cref="Enum"/> type</typeparam>
	/// <remarks>
	/// This class allows you to define an attribute for each item in an enumeration that can
	/// be used in other settings.  The example below shows longer names that can be associated
	/// with the enum values for UI display or other needs.
	/// public enum ActivityExceptionRetryAction
	///{
	///    [EnumValue("Log and Continue")]
	///    Continue,
	///
	///    [EnumValue("Halt Execution")]
	///    Halt
	///}
	/// </remarks>
	public static class Enum<T>
	{
		private static readonly Dictionary<Enum, EnumValueAttribute> _enumsWithAttribute = new Dictionary<Enum, EnumValueAttribute>();

		/// <summary>
		/// Parses the given string value for the <typeparamref name="T"/> value and returns
		/// the <see cref="EnumValueAttribute"/> value for the parsed <see cref="Enum"/> value.
		/// </summary>
		/// <param name="valueName">Name of the enum value.</param>
		/// <returns>
		/// String Value associated via a <see cref="EnumValueAttribute"/> attribute, or null if not found.
		/// </returns>
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
		/// Gets the <see cref="EnumValueAttribute"/> values for the specified enum.
		/// </summary>
		public static Dictionary<object, string> EnumValues
		{
			get
			{
				Dictionary<object, string> returnValue = new Dictionary<object, string>();
				Type underlyingType = Enum.GetUnderlyingType(typeof(T));

				// For each field in the enum, get the EnumValueAttribute values for each one
				foreach (FieldInfo fieldInfo in typeof(T).GetFields())
				{
					EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
					if (attributes.Length > 0)
					{
						returnValue.Add
								(
										Convert.ChangeType(Enum.Parse(typeof(T), fieldInfo.Name), underlyingType, CultureInfo.InvariantCulture),
										attributes[0].Value
								);
					}
				}

				return returnValue;
			}
		}

		/// <summary>
		/// Gets the <see cref="EnumValueAttribute"/> string value for a specific enum value.
		/// </summary>
		/// <param name="enumType">Value.</param>
		/// <returns>
		/// String Value associated via a <see cref="EnumValueAttribute"/> attribute, or null if not found.
		/// </returns>
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
				FieldInfo fieldInfo = enumType.GetType().GetField(enumType.ToString());
				EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
				if (attributes.Length > 0)
				{
					_enumsWithAttribute.Add(enumType, attributes[0]);
					returnValue = attributes[0].Value;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Parses the supplied <see cref="EnumValueAttribute"/> value to find an associated enum value.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>
		/// Enum value associated with the string value, or null if not found. (Don has seen that with a valid enum the default(T) will return the first defined value for the enum type.)
		/// </returns>
		public static T Parse(string value, bool ignoreCase = false)
		{
			T returnValue = default(T);

			string enumStringValue = null;

			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("Supplied type must be an Enum.  Type was ", typeof(T).ToString());
			}

			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fieldInfo in typeof(T).GetFields())
			{
				EnumValueAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
				if (attributes.Length > 0)
				{
					enumStringValue = attributes[0].Value;

					StringComparison comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

					if (string.Compare(enumStringValue, value, comparison) == 0)
					{
						returnValue = (T)Enum.Parse(typeof(T), fieldInfo.Name);
						break;
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>
		/// Existence of the string value
		/// </returns>
		public static bool IsDefined(string value, bool ignoreCase = false)
		{
			return Parse(value, ignoreCase) != null;
		}

	}
}