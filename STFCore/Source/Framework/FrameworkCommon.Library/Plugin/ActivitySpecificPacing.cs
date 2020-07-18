using System;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Class ActivitySpecificPacing which defines properties for how an activity
    /// will delay between execution.  This is on layer below the default activity
    /// pacing that is managed globally at the worker level.  By default the worker
    /// level pacing is applied between activity execution, unless a specific
    /// activity has a value as defined by this class.
    /// </summary>
    [DataContract]
    public class ActivitySpecificPacing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivitySpecificPacing"/> class.
        /// </summary>
        public ActivitySpecificPacing()
        {
            Clear();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Enabled = false;
            DelayOnRepeat = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ActivitySpecificPacing"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Minimum Delay
        /// </summary>
        [DataMember]
        public TimeSpan MinDelay { get; set; }

        /// <summary>
        /// Maximum Delay
        /// </summary>
        [DataMember]
        public TimeSpan MaxDelay { get; set; }

        /// <summary>
        /// Flag for Randomize option
        /// </summary>
        [DataMember]
        public bool Randomize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to repeat delay on each activity if there run count is > 1.
        /// </summary>
        /// <value><c>true</c> if this delay should apply every time the activity runs; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool DelayOnRepeat { get; set; }

        /// <summary>
        /// Equal Comparator 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ActivitySpecificPacing other)
        {
            return Enabled == other.Enabled
                && MinDelay == other.MinDelay
                && MaxDelay == other.MaxDelay
                && Randomize == other.Randomize
                && DelayOnRepeat == other.DelayOnRepeat;
        }
    }
}
