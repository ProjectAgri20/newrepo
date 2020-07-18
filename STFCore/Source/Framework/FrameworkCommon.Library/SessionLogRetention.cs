using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Settings for Session Log retention
    /// </summary>
    public enum SessionLogRetention
    {
        /// <summary>
        /// No retention is applied.  Log data is removed at the next cleanup operation.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Log data is retained for one day.
        /// </summary>
        [Description("1 Day")]
        Day = 1,

        /// <summary>
        /// Log data is retained for one week.
        /// </summary>
        [Description("1 Week (7 days)")]
        Week = 7,

        /// <summary>
        /// Log data is retained for one month.
        /// </summary>
        [Description("1 Month (30 days)")]
        Month = 30,

        /// <summary>
        /// Log data is retained for three months.
        /// </summary>
        [Description("3 Months (90 days)")]
        ThreeMonths = 90,

        /// <summary>
        /// Log data is retained for six months.
        /// </summary>
        [Description("6 Months (180 days)")]
        SixMonths = 180,

        /// <summary>
        /// Log data is retained for nine months.
        /// </summary>
        [Description("9 months (270 days)")]
        NineMonths = 270,

        /// <summary>
        /// Log data is retained for one year.
        /// </summary>
        [Description("1 Year (365 days)")]
        OneYear = 365
    }

    /// <summary>
    /// Provides extension methods for the <see cref="SessionLogRetention"/> enumeration.
    /// </summary>
    public static class SessionLogRetentionHelper
    {
        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        /// <param name="retention">The retention.</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static DateTime GetExpirationDate(this SessionLogRetention retention, DateTime start)
        {
            switch (retention)
            {
                case SessionLogRetention.None:
                    return FormatExpirationDate(start);

                case SessionLogRetention.Day:
                    return FormatExpirationDate(start.AddDays(1));

                case SessionLogRetention.Week:
                    return FormatExpirationDate(start.AddDays(7));

                case SessionLogRetention.Month:
                    return FormatExpirationDate(start.AddDays(30));

                case SessionLogRetention.ThreeMonths:
                    return FormatExpirationDate(start.AddDays(90));

                case SessionLogRetention.SixMonths:
                    return FormatExpirationDate(start.AddDays(120));

                case SessionLogRetention.NineMonths:
                    return FormatExpirationDate(start.AddDays(270));

                case SessionLogRetention.OneYear:
                    return FormatExpirationDate(start.AddDays(365));

                default:
                    throw new ArgumentException("Unknown retention period " + retention.ToString());
            }
        }

        /// <summary>
        /// Retrieves the expiration list from the enum based on the retention flag obtained from database
        /// </summary>
        public static Collection<string> ExpirationList
        {
            get
            {
                int maxValue = -1;
                string stringMaxValue = GlobalSettings.Items[Setting.MaxLogDefault];
                if (int.TryParse(stringMaxValue, out maxValue))
                {
                    int currentMin = maxValue;
                    var items = Enum.GetNames(typeof(SessionLogRetention));

                    foreach (var value in Enum.GetValues(typeof(SessionLogRetention)))
                    {
                        int newMin = Math.Min(maxValue, (int)value);

                        if (newMin <= maxValue)
                        {
                            currentMin = newMin;
                        }
                        else
                        {
                            //We have a min larger than our value, we get out and grab the list of everything below it
                            break;
                        }
                    }

                    Collection<string> result = new Collection<string>();

                    foreach (SessionLogRetention key in EnumUtil.GetValues<SessionLogRetention>())
                    {
                        if ((int)key <= currentMin)
                        {
                            result.Add(EnumUtil.GetDescription(key));
                        }
                    }

                    return result;
                }
                else
                {
                    return (Collection<string>)EnumUtil.GetDescriptions<SessionLogRetention>();
                }
            }
        }

        private static DateTime FormatExpirationDate(DateTime expirationDate)
        {
            // Set expiration to the same day at 11:59:59:995
            DateTime dayWithoutTime = expirationDate.Date;
            return dayWithoutTime.AddDays(1).AddMilliseconds(-5);
        }
    }
}
