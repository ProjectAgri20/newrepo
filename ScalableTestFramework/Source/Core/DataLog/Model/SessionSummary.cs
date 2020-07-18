using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// Core data about a test session, including links to other test data collected during execution.
    /// </summary>
    [DebuggerDisplay("{SessionId,nq}")]
    public sealed class SessionSummary
    {
        /// <summary>
        /// Gets or sets the unique identifier for the session.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the session name.
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        /// Gets or sets the session owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the dispatcher that launched the session.
        /// </summary>
        public string Dispatcher { get; set; }

        /// <summary>
        /// Gets or sets the time the session started.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time the session ended.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the current runtime status of the session.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the version of STF used to execute the session.
        /// </summary>
        public string StfVersion { get; set; }

        /// <summary>
        /// Gets or sets the session type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the session test cycle.
        /// </summary>
        public string Cycle { get; set; }

        /// <summary>
        /// Gets or sets the session test reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the tags applied to the session.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the notes for the session.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the state of the session with respect to shutdown.
        /// </summary>
        public string ShutdownState { get; set; }

        /// <summary>
        /// Gets or sets the projected end time of the session.
        /// </summary>
        public DateTime? ProjectedEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the session data will expire.
        /// </summary>
        public DateTime? ExpirationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user that shut down the session.
        /// </summary>
        public string ShutdownUser { get; set; }

        /// <summary>
        /// Gets or sets the time the session was shut down.
        /// </summary>
        public DateTime? ShutdownDateTime { get; set; }

        /// <summary>
        /// Gets the scenario execution data for this session.
        /// </summary>
        public ICollection<SessionScenario> Scenarios { get; private set; } = new HashSet<SessionScenario>();

        /// <summary>
        /// Gets the devices used in this session.
        /// </summary>
        public ICollection<SessionDevice> Devices { get; private set; } = new HashSet<SessionDevice>();

        /// <summary>
        /// Gets the documents used in this session.
        /// </summary>
        public ICollection<SessionDocument> Documents { get; private set; } = new HashSet<SessionDocument>();

        /// <summary>
        /// Gets the servers used in this session.
        /// </summary>
        public ICollection<SessionServer> Servers { get; private set; } = new HashSet<SessionServer>();

        /// <summary>
        /// Gets the products associated with this session.
        /// </summary>
        public ICollection<SessionProduct> AssociatedProducts { get; private set; } = new HashSet<SessionProduct>();
    }
}
