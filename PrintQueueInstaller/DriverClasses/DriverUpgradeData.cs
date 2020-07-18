using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This class contains data used to support reporting on the upgrade process for print drivers.
    /// A collection of this class is data bound to a data grid view.
    /// </summary>
    public class DriverUpgradeData
    {
        private string _startTimeValue = string.Empty;
        private string _endTimeValue = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this item should be is included in the upgrade.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include; otherwise, <c>false</c>.
        /// </value>
        public bool Include { get; set; }

        /// <summary>
        /// Gets or sets the status of the upgrade.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the print processor.
        /// </summary>
        /// <value>
        /// The print processor.
        /// </value>
        public string PrintProcessor { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue being upgraded.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the start time in a formatted string.
        /// </summary>
        /// <value>
        /// The start time formatted.
        /// </value>
        public string StartTimeFormatted 
        { 
            get 
            {
                if (StartTime == default(DateTime))
                {
                    return _startTimeValue;
                }
                else
                {
                    return StartTime.ToString(Resource.DateTimeFormat, CultureInfo.CurrentCulture);
                }
            }
            set { _startTimeValue = value; }
        }

        /// <summary>
        /// Gets or sets the end time in a formatted string.
        /// </summary>
        /// <value>
        /// The end time formatted.
        /// </value>
        public string EndTimeFormatted
        {
            get
            {
                if (EndTime == default(DateTime))
                {
                    return _endTimeValue;
                }
                else
                {
                    return EndTime.ToString(Resource.DateTimeFormat, CultureInfo.CurrentCulture);
                }
            }
            set { _endTimeValue = value; }
        }

        /// <summary>
        /// Gets or sets the duration of the upgrade for this queue.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the start time of the upgrade.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the upgrade.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverUpgradeData"/> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        public DriverUpgradeData(string queueName)
        {
            StartTime = default(DateTime);
            EndTime = default(DateTime);
            Include = true;
            QueueName = queueName;
            Status = string.Empty;
            Duration = string.Empty;
        }
    }
}
