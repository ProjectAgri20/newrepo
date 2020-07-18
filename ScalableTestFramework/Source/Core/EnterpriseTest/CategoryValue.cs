using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A value associated with a defined category of values.
    /// </summary>
    [DebuggerDisplay("{Category,nq}={Value}")]
    public class CategoryValue
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category value.
        /// </summary>
        public int CategoryValueId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the collection of parent category values.
        /// </summary>
        public virtual ICollection<CategoryValue> Parents { get; set; } = new HashSet<CategoryValue>();

        /// <summary>
        /// Gets or sets the collection of child category values.
        /// </summary>
        public virtual ICollection<CategoryValue> Children { get; set; } = new HashSet<CategoryValue>();
    }
}
