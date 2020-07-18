using System;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Simple attribute class for storing <see cref="System.String"/> values focused 
    /// on extending <see cref="System.Enum"/> to include descriptive values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumValueAttribute : Attribute
    {
        private readonly string _value;

        /// <summary>
        /// Creates a new <see cref="EnumValueAttribute"/> instance.
        /// </summary>
        /// <param name="value">The descriptive value for the <see cref="Enum"/>.</param>
        public EnumValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the descriptive value defined for the <see cref="Enum"/>.
        /// </summary>
        /// <value></value>
        public string Value
        {
            get { return _value; }
        }
    }
}
