using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// A class for logging the session configuration data of a scenario.
    /// </summary>
    public class SessionConfigurationDataLogger : FrameworkDataLog // CHANGE BACK TO INTERNAL!!
    {
        public override string TableName => "SessionConfiguration";

        public override string PrimaryKeyColumn => nameof(SessionId);

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionConfigurationDataLogger"/> class.
        /// </summary>
        public SessionConfigurationDataLogger()
        {
        }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the order in which this configuration was run.
        /// </summary>
        /// <value>The order in which this configuration was run.</value>
        [DataLogProperty]
        public int RunOrder { get; set; }

        /// <summary>
        /// Gets or sets the configuration data.
        /// </summary>
        /// <value>The configuration data.</value>
        [DataLogProperty(MaxLength = -1)]
        public string ConfigurationData { get; set; }
    }
}
