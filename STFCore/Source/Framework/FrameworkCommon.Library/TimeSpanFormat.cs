using System;
using System.Globalization;


namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Contains methods for formatting <see cref="TimeSpan" /> instances.
    /// </summary>
    public static class TimeSpanFormat
    {
        /// <summary>
        /// Converts a <see cref="System.String"/> value in a time format to a <see cref="System.TimeSpan"/>
        /// </summary>
        /// <param name="time">The <see cref="System.String"/> time in a delimited format (such as hh:mm or hh:mm:ss).</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>
        /// A new <see cref="System.TimeSpan"/> structure contain the time value
        /// </returns>
        /// <remarks>
        /// This method is not overly flexible, but it does have the ability to parse a string based on the number
        /// of delimited numerical components within it.  Currently it supports the following formats:
        /// <ol>
        /// <li>hh:mm</li>
        /// <li>hh:mm:ss</li>
        /// <li>dd:hh:mm:ss</li>
        /// <li>dd:hh:mm:ss</li>
        /// </ol>
        /// 
        /// The delimiter can be a character other than ':', but that is the default.
        /// </remarks>
        /// <example>
        /// The following will return a <see cref="System.TimeSpan"/> structure containing 10 hours and 20 minutes.
        /// <code>
        /// var timeSpan = TimeSpanFormat.Parse("10:20");
        /// </code>
        /// 
        /// The same example could use a different delimiter if needed.
        /// <code>
        /// var timeSpan = TimeSpanFormat.Parse("10.20", '.');
        /// </code>
        /// </example>
        public static TimeSpan Parse(string time, char delimiter = ':')
        {
            if (string.IsNullOrEmpty(time))
            {
                throw new ArgumentNullException("time");
            }

            try
            {
                string[] parts = time.Split(delimiter);
                switch (parts.Length)
                {
                    case 1:
                        return new TimeSpan(int.Parse(parts[0], CultureInfo.CurrentCulture), 0, 0);

                    case 2:
                        return new TimeSpan(int.Parse(parts[0], CultureInfo.CurrentCulture), int.Parse(parts[1], CultureInfo.CurrentCulture), 0);
                    
                    case 3:
                        return new TimeSpan(int.Parse(parts[0], CultureInfo.CurrentCulture), int.Parse(parts[1], CultureInfo.CurrentCulture), int.Parse(parts[2], CultureInfo.CurrentCulture));

                    default:
                        throw new FormatException("'{0}': Bad Format".FormatWith(time));
                }
            }
            catch (FormatException)
            {
                TraceFactory.Logger.Error("'{0}': Bad Format".FormatWith(time));
                throw;
            }
            catch (OverflowException)
            {
                TraceFactory.Logger.Error("'{0}': Overflow".FormatWith(time));
                throw;
            }
        }

        /// <summary>
        /// Converts the <see cref="System.Int32"/> value into a <see cref=" System.String"/> that represents a time value in hh:mm.
        /// </summary>
        /// <param name="timeInMinutes">The <see cref="System.Int32"/> value representing total minutes.</param>
        /// <returns>Returns a <see cref="System.String"/> in the format into an hh:mm or mm:ss.</returns>
        public static string ToTimeSpanString(int timeInMinutes)
        {
            int bigPart = timeInMinutes / 60;
            int littlePart = timeInMinutes % 60;
            return "{00:00}:{1:00}".FormatWith(bigPart, littlePart);
        }

        /// <summary>
        /// Converts the <see cref="TimeSpan"/> value into a <see cref=" System.String"/> that represents a time value in hh:mm.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> value</param>
        /// <returns>A <see cref="System.String"/> in the format into an hh:mm or mm:ss.</returns>
        public static string ToTimeSpanString(TimeSpan timeSpan)
        {
            int totalMinutes = Convert.ToInt32(timeSpan.TotalMinutes);
            return ToTimeSpanString(totalMinutes);
        }
    }
}
