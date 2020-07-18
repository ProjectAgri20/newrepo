using System;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Basic information about an activity.
    /// </summary>
    public class ActivityInfo
    {
        /// <summary>
        /// Gets the activity id.
        /// </summary>
        /// <value>The activity id.</value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the activity name.
        /// </summary>
        /// <value>The activity name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the activity.
        /// </summary>
        /// <value>The type of the activity.</value>
        public string ActivityType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityInfo" /> class.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public ActivityInfo(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }

            Id = activity.Id;
            Name = activity.Name;
            ActivityType = activity.ActivityType;
        }
    }
}
