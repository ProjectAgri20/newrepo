using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    [DataContract]
    public class SessionStartSchedule
    {
        /// <summary>
        /// Gets or sets the Date/Time when the session Run sequence will start.
        /// </summary>
        /// <value>The start date time.</value>
        [DataMember]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the amount of time allocated for session setup in minutes.
        /// </summary>
        /// <value>The setup buffer.</value>
        [DataMember]
        public decimal SetupTimePadding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SessionStartSchedule"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool ScheduleEnabled { get; set; }

        /// <summary>
        /// Gets the Date/Time when the session PowerUp sequence will start.
        /// </summary>
        /// <value>The setup date time.</value>
        public DateTime SetupDateTime
        {
            get { return StartDateTime.Subtract(TimeSpan.FromMinutes((double)SetupTimePadding)); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStartSchedule"/> class.
        /// </summary>
        public SessionStartSchedule()
        {
            StartDateTime = DateTime.Now;
            SetupTimePadding = 0;
            ScheduleEnabled = false;
        }
    }
}
