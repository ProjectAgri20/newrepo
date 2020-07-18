using System;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Custum attribute class for decorating classes related to Contention activities
    /// </summary>
    public class ContentionActivity : Attribute
    {
        /// <summary>
        /// Gets a value for the name of the Contention activity
        /// </summary>
        public string ActivityName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentionActivity"/> attribute class
        /// </summary>
        /// <param name="contentionActivityName">Name of the Contention activity</param>
        public ContentionActivity(string contentionActivityName)
        {
            ActivityName = contentionActivityName;
        }
    }
}
