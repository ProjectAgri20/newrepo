using System;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.ClientController.ActivityExecution
{
    /// <summary>
    /// A class for logging properties of servers used in a scenario.
    /// </summary>
    public class SessionServerLogger : FrameworkDataLog
    {
        public override string TableName => "SessionServer";

        public override string PrimaryKeyColumn => nameof(SessionServerId);

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionServerLogger" /> class.
        /// </summary>
        public SessionServerLogger()
        {
        }

        [DataLogProperty]
        public Guid SessionServerId { get; set; }

        [DataLogProperty]
        public string SessionId { get; set; }

        [DataLogProperty]
        public Guid ServerId { get; set; }

        [DataLogProperty]
        public string HostName { get; set; }

        [DataLogProperty]
        public string Address { get; set; }

        [DataLogProperty]
        public string OperatingSystem { get; set; }

        [DataLogProperty(MaxLength = 10)]
        public string Architecture { get; set; }

        [DataLogProperty]
        public short Processors { get; set; }

        [DataLogProperty]
        public short Cores { get; set; }

        [DataLogProperty]
        public int Memory { get; set; }
    }
}
