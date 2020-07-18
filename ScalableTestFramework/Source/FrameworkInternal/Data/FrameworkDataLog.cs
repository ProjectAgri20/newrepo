namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Base class for logging framework data.
    /// Child classes can decorate properties with a <see cref="DataLogPropertyAttribute" />
    /// to define the schema for the data to be logged.
    /// </summary>
    public abstract class FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public abstract string PrimaryKeyColumn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkDataLog" /> class.
        /// </summary>
        protected FrameworkDataLog()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
