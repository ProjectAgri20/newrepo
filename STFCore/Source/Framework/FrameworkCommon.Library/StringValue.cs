using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Simple class to wrap a string value
    /// </summary>
    public class StringValue
    {
        string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValue"/> class.
        /// </summary>
        public StringValue()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringValue(String value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
