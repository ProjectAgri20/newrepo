namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Operators that can be used in dynamic queries.
    /// </summary>
    public enum QueryOperator
    {
        /// <summary>
        /// No operator selected
        /// </summary>
        None,

        /// <summary>
        /// Equal
        /// </summary>
        Equal,

        /// <summary>
        /// Less than
        /// </summary>
        LessThan,

        /// <summary>
        /// Greater than
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Less than or equal
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Greater than or equal
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Not equal
        /// </summary>
        NotEqual,

        /// <summary>
        /// Contains
        /// </summary>
        Contains,

        /// <summary>
        /// Begins with
        /// </summary>
        BeginsWith,

        /// <summary>
        /// Ends with
        /// </summary>
        EndsWith,

        /// <summary>
        /// Is in
        /// </summary>
        IsIn,

        /// <summary>
        /// Is not in
        /// </summary>
        IsNotIn,

        /// <summary>
        /// Is between
        /// </summary>
        IsBetween
    }
}
