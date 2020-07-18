using System;
using System.Linq;

namespace HP.RDL.RdlHPMibTranslator
{
	/// <summary>
	/// Simple attribute class for storing <see cref="System.String"/> values focused 
	/// on extending <see cref="System.Enum"/> to include string values 
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class EnumValueAttribute : Attribute
	{
		private readonly string _value;

		/// <summary>
		/// Creates a new <see cref="EnumValueAttribute"/> instance.
		/// </summary>
		/// <param name="sValue">Value.</param>
		public EnumValueAttribute(string sValue)
		{
			_value = sValue;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value></value>
		public string Value
		{
			get { return _value; }
		}
	}
}
