using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// This class is used to describe the information coming in from the UI to the dispatcher.
    /// </summary>
    [DataContract]
    public class SessionTicket
    {
        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>The session id.</value>
        [DataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the scenario id.
        /// </summary>
        /// <value>The scenario id.</value>
        [DataMember]
        public IEnumerable<Guid> ScenarioIds { get; set; }

        /// <summary>
        /// Gets a startup token used by the owning client to manage session startup.
        /// </summary>
        [DataMember]
        public Guid StartupToken { get; private set; }

        /// <summary>
        /// Gets or sets the session name.
        /// </summary>
        /// <value>The session name.</value>
        [DataMember]
        public string SessionName { get; set; }

        /// <summary>
        /// Gets or sets the session owner.
        /// </summary>
        /// <value>The session owner.</value>
        [DataMember]
        public UserCredential SessionOwner { get; set; }

        /// <summary>
        /// Gets or sets the session notes.
        /// </summary>
        /// <value>The session notes.</value>
        [DataMember]
        public string SessionNotes { get; set; }

        /// <summary>
        /// Gets or sets the duration of the test run in hours.
        /// </summary>
        [DataMember]
        public int DurationHours { get; set; }

        /// <summary>
        /// Gets or sets the expiration date for the session log data.
        /// </summary>
        [DataMember]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the email address to notify of failures
        /// </summary>
        [DataMember]
        public string EmailAddresses { get; set; }
        /// <summary>
        /// The value of consecutive failures per user allowed displayed time
        /// </summary>
        [DataMember]
        public int FailureCount { get; set; }

        /// <summary>
        /// The time interval in which to query for failures
        /// </summary>
        [DataMember]
        public TimeSpan FailureTime { get; set; }

        /// <summary>
        /// List of failures which always trigger a dart log collect
        /// </summary>
        [DataMember]
        public string[] TriggerList { get; set; }


        /// <summary>
        /// Gets or sets Session Type.  eg. Regression, Characterization, etc.
        /// </summary>
        [DataMember]
        public string SessionType { get; set; }

        /// <summary>
        /// Gets or sets Session Cycle.  eg. 17.07, 17.09, etc.
        /// </summary>
        [DataMember]
        public string SessionCycle { get; set; }

        /// <summary>
        /// Gets or sets CR Reference.
        /// </summary>
        [DataMember]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets STF Monitor service log location.
        /// </summary>
        [DataMember]
        public string LogLocation { get; set; }

        /// <summary>
        /// Gets or sets the requested VMs.
        /// </summary>
        /// <value>The requested VMs.</value>
        [DataMember]
        public RequestedVMDictionary RequestedVMs { get; private set; }

        /// <summary>
        /// Gets the solutions and versions used.
        /// </summary>
        [DataMember]
        public List<AssociatedProductSerializable> AssociatedProductList { get; private set; }

        /// <summary>
        /// Gets or sets whether the system should collect Windows Event Logs.
        /// </summary>
        [DataMember]
        public bool CollectEventLogs { get; set; }

        /// <summary>
        /// Gets or sets whether the system should collect DART Logs.
        /// </summary>
        [DataMember]
        public bool CollectDARTLogs { get; set; }

        /// <summary>
        /// Gets or sets whether to remove test devices from rotation when they become unresponsive.
        /// </summary>
        [DataMember]
        public bool RemoveUnresponsiveDevices { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTicket"/> class.
        /// </summary>
        public SessionTicket()
        {
            SessionId = CreateSessionId();
            ScenarioIds = new List<Guid>();
            SessionName = string.Empty;
            SessionCycle = string.Empty;
            SessionOwner = null;
            SessionNotes = null;
            DurationHours = 1;
            ExpirationDate = DateTime.Now.AddHours(1);
            RequestedVMs = new RequestedVMDictionary();
            DurationHours = 1;
            StartupToken = SequentialGuid.NewGuid();
            AssociatedProductList = new List<AssociatedProductSerializable>();
            CollectEventLogs = false;
            CollectDARTLogs = false;
            FailureCount = -1;
            FailureTime = TimeSpan.MaxValue;
            LogLocation = string.Empty;
            RemoveUnresponsiveDevices = true;
        }

        /// <summary>
        /// Creates a <see cref="SessionTicket"/> using the specififed scenario name.
        /// </summary>
        /// <param name="scenarioNames">List of the scenarios to execute.</param>
        /// <param name="sessionName">Name of the session to execute scenarios</param>
        /// <param name="durationHours">The session duration in hours.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Scenario with specified name is not found in the database.</exception>
        public static SessionTicket Create(IEnumerable<string> scenarioNames, string sessionName, int durationHours = 2)
        {
            SessionTicket ticket = new SessionTicket()
            {
                ExpirationDate = DateTime.Now.AddHours(durationHours),
                DurationHours = durationHours,
                SessionName = sessionName
            };

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (string scenarioName in scenarioNames)
                {
                    if (!string.IsNullOrEmpty(scenarioName))
                    {
                        var scenario = EnterpriseScenario.Select(context, scenarioName);
                        if (scenario == null)
                        {
                            throw new InvalidOperationException($"'{scenarioName}' is not found in the database.");
                        }
                        ((List<Guid>)ticket.ScenarioIds).Add(scenario.EnterpriseScenarioId);
                    }
                }
            }
            return ticket;
        }

        /// <summary>
        /// Creates a <see cref="SessionTicket"/> using the specififed scenario Ids.
        /// </summary>
        /// <param name="scenarioIds">The Ids of the scnarios to run in the session.</param>
        /// <param name="sessionName">The name of the session.</param>
        /// <param name="durationHours">The session duration in hours.</param>
        /// <returns></returns>
        public static SessionTicket Create(IEnumerable<Guid> scenarioIds, string sessionName, int durationHours = 2)
        {
            SessionTicket ticket = new SessionTicket()
            {
                ExpirationDate = DateTime.Now.AddHours(durationHours),
                DurationHours = durationHours,
                SessionName = sessionName
            };

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (Guid scenarioId in scenarioIds)
                {
                    var scenario = EnterpriseScenario.Select(context, scenarioId);
                    if (scenario == null)
                    {
                        throw new InvalidOperationException($"ScenarioId {scenarioId} is not found in the database.");
                    }
                    ((List<Guid>)ticket.ScenarioIds).Add(scenarioId);
                }
            }

            return ticket;
        }

        /// <summary>
        /// Creates the session unique identifier.
        /// </summary>
        /// <returns></returns>
        public static string CreateSessionId()
        {
            // Store for consistency
            DateTime now = DateTime.Now;

            // Session IDs are 8 characters long, where each character has 34 possibilities (i.e. alphanumeric minus I and O).
            const int length = 8;
            const int baseX = 34;

            // Calculate the total number of valid session IDs with the specified length & base,
            // taking into account that the first character must be a letter.
            long maximumPermutations = (long)Math.Pow(baseX, length) - 1;
            long permutationsStartingWithNumber = (long)Math.Pow(baseX, length - 1) * 10;
            long validPermutations = maximumPermutations - permutationsStartingWithNumber;

            // Initialize dates for a set of IDs that loops once every 100 years.
            int century = now.Year / 100;
            DateTime startDate = new DateTime(century * 100, 1, 1);
            DateTime endDate = startDate.AddYears(100);
            TimeSpan period = endDate - startDate;

            // Calculate how far through the century we are, as a percentile.
            long ticksUntilNow = now.Ticks - startDate.Ticks;
            double percentile = (double)ticksUntilNow / period.Ticks;

            // Determine what permutation we should be on, based on the percentile.
            // Then offset to skip all permutations that start with a number.
            long numericId = (long)(percentile * validPermutations) + permutationsStartingWithNumber;

            // Convert numeric representation to a base 34 character string.
            StringBuilder result = new StringBuilder(length);
            while (numericId > 0)
            {
                int x = (int)(numericId % baseX);
                result.Insert(0, GetAlphaNumericCharacter(x));
                numericId /= baseX;
            }
            return result.ToString();
        }

        private static char GetAlphaNumericCharacter(int x)
        {
            int ascii;
            if (x >= 0 && x <= 9)
            {
                // Convert digits into their char representation
                ascii = x + '0';
            }
            else
            {
                // Convert anything greater than 10 to a letter, starting at A
                ascii = x - 10 + 'A';

                // Eliminate I and O by shifting
                if (ascii >= 'I')
                {
                    ascii++;
                }
                if (ascii >= 'O')
                {
                    ascii++;
                }
            }
            return (char)ascii;
        }
    }
}