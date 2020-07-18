using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// Defines time based status event arguments.
    /// </summary>
    public class TimeStatusEventArgs : StatusChangedEventArgs
    {
        /// <summary>
        /// Notifies start and end times for an event.
        /// </summary>
        /// <param name="statusMessage"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public TimeStatusEventArgs(string statusMessage, DateTime start, DateTime end)
            : base(statusMessage)
        {
            StartDateTime = start;
            EndDateTime = end;
        }

        /// <summary>
        /// Gets the event start date time.
        /// </summary>
        /// <value>The start date time.</value>
        public DateTime StartDateTime { get; }

        /// <summary>
        /// Gets the event end date time.
        /// </summary>
        /// <value>The end date time.</value>
        public DateTime EndDateTime { get; }
    }
}
