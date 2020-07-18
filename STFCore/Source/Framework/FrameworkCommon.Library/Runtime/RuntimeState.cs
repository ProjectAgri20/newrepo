using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Various states that a component can use during session runtime
    /// </summary>
    [DataContract]
    public enum RuntimeState
    {
        /// <summary>
        /// There is no defined state for the element
        /// </summary>
        None,

        /// <summary>
        /// The element is available
        /// </summary>
        [EnumMember]
        Available,

        /// <summary>
        /// The element is validating
        /// </summary>
        [EnumMember]
        Validating,

        /// <summary>
        /// The element is validated
        /// </summary>
        [EnumMember]
        Validated,

        /// <summary>
        /// The element is starting
        /// </summary>
        [EnumMember]
        Starting,

        /// <summary>
        /// The element is ready
        /// </summary>
        [EnumMember]
        Ready,

        /// <summary>
        /// The element if running
        /// </summary>
        [EnumMember]
        Running,

        /// <summary>
        /// The element is pausing
        /// </summary>
        [EnumMember]
        Pausing,

        /// <summary>
        /// The element is paused
        /// </summary>
        [EnumMember]
        Paused,

        /// <summary>
        /// The element is halting
        /// </summary>
        [EnumMember]
        Halting,

        /// <summary>
        /// The element is halted
        /// </summary>
        [EnumMember]
        Halted,

        /// <summary>
        /// The element is completed
        /// </summary>
        [EnumMember]
        Completed,

        /// <summary>
        /// The element is shutting down
        /// </summary>
        [EnumMember]
        ShuttingDown,

        /// <summary>
        /// This element is offline.
        /// </summary>
        [EnumMember]
        Offline,

        /// <summary>
        /// This element is registered.
        /// </summary>
        [EnumMember]
        Registered,

        /// <summary>
        /// The element is in an warning state.
        /// </summary>
        [EnumMember]
        Warning,

        /// <summary>
        /// The element is in an error state.
        /// </summary>
        [EnumMember]
        Error,

        /// <summary>
        /// A child of this element is in an error state.
        /// </summary>
        [EnumMember]
        AggregateError,

        /// <summary>
        /// Specifies that initial setup is complete
        /// </summary>
        [EnumMember]
        SetupCompleted,

        /// <summary>
        /// The post run completed
        /// </summary>
        [EnumMember]
        TeardownCompleted,
    }
}
