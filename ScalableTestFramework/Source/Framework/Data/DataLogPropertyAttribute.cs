using System;
using System.Runtime.CompilerServices;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Attribute used to mark an <see cref="ActivityDataLog" /> property that contains log data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataLogPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether this property will be included in update operations.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool IncludeInUpdates { get; set; } = false;

        /// <summary>
        /// If the associated property is of type <see cref="string"/>, sets the maximum length of the corresponding text property.
        /// A value of -1 represents the maximum available length. The default value is 50.
        /// </summary>
        public int MaxLength { get; set; } = 50;

        /// <summary>
        /// If the associated property is of type <see cref="string"/>, indicates whether the text can be truncated if it exceeds <see cref="MaxLength" />.
        /// If set to <c>false</c>, a string that exceeds <see cref="MaxLength" /> will cause an error. The default value is <c>false</c>.
        /// </summary>
        public bool TruncationAllowed { get; set; } = false;

        /// <summary>
        /// Gets the order for this property.  Used for ordering the properties when creating a new backing data store.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogPropertyAttribute" /> class.
        /// </summary>
        /// <param name="order">
        /// An ordinal value used to specify the order of properties when creating a new backing data store.
        /// By default, this value is set to the line number on which the attribute is defined.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Required for CallerLineNumber.")]
        public DataLogPropertyAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }
    }
}
