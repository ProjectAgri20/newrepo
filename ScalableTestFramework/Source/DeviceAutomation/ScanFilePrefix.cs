using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Generates unique scan file prefixes.
    /// </summary>
    public class ScanFilePrefix
    {
        // Offset used to ensure each file in a session has a unique prefix
        private static int _lastUsedOffset = 0;
        private const int Unlimited = -1;

        // Delimiters used in the scan file name
        private static readonly char[] _delimiters = new char[] { '-', '_', '(', ')' };

        /// <summary>
        /// Gets the session id.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        public string Sender { get; }

        /// <summary>
        /// Gets the type of the scan.
        /// </summary>
        public string ScanType { get; }

        /// <summary>
        /// Gets the unique offset.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets or Set the Maximum Length of the file name.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFilePrefix"/> class.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="scanType">The scan type.</param>
        public ScanFilePrefix(string sessionId, string sender, string scanType)
            : this(sessionId, sender, scanType, ++_lastUsedOffset)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFilePrefix"/> class.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="scanType">The scan type.</param>
        /// <param name="offset">The offset.</param>
        private ScanFilePrefix(string sessionId, string sender, string scanType, int offset)
        {
            SessionId = sessionId.ToUpperInvariant();
            Sender = sender;
            ScanType = scanType;
            Offset = offset;
            MaxLength = Unlimited;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (MaxLength == Unlimited)
            {
                return BuildFilePrefix();
            }
            return BuildShortenedPrefix();
        }

        private string BuildFilePrefix()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(SessionId);
            builder.Append(_delimiters[0]);
            builder.Append(Sender);
            builder.Append(_delimiters[1]);
            builder.Append(ScanType);
            builder.Append(_delimiters[2]);
            builder.Append(Offset.ToString("0000", CultureInfo.InvariantCulture));
            builder.Append(_delimiters[3]);
            return builder.ToString();
        }

        private string BuildShortenedPrefix()
        {
            string pattern = string.Empty;
            StringBuilder strBuilder = new StringBuilder();
            int delimiters = 3;
            int length = SessionId.Length + Sender.Length + ScanType.Length + delimiters;
            if (length < MaxLength)
            {
                pattern = BuildFilePrefix();
            }
            else
            {
                strBuilder.Append(SessionId);
                // Check for the Pattern
                if (Sender.ToUpper().StartsWith("U"))
                {
                    //RDL user pattern.  Include the user number as part of the filename.
                    strBuilder.Append(Sender.Substring(Sender.Length - 1));
                    strBuilder.Append(Offset.ToString("0000000", CultureInfo.InvariantCulture));
                }
                else
                {
                    //User name is an unknown pattern.  Leave it out of the filename.
                    strBuilder.Append(Offset.ToString("00000000", CultureInfo.InvariantCulture));
                }
                pattern = strBuilder.ToString();
            }
            return pattern;
        }
        /// <summary>
        /// Parses the specified file name into a <see cref="ScanFilePrefix"/> instance.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static ScanFilePrefix Parse(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            Match match = MatchPattern(fileName);
            if (match.Success)
            {
                return new ScanFilePrefix(sessionId: match.Groups[1].Value.ToUpperInvariant(),
                                          sender: match.Groups[2].Value,
                                          scanType: ToProper(match.Groups[3].Value),
                                          offset: int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture));
            }
            else
            {
                throw new FormatException("File name was not in the correct format.");
            }
        }

        /// <summary>
        /// Returns a code for this scan file that is compatible for entry as a fax number.
        /// </summary>
        /// <returns></returns>
        public string ToFaxCode()
        {
            // Encode the session ID into a numeric format using ASCII
            var sessionBytes = Encoding.ASCII.GetBytes(SessionId);
            string encodedSession = string.Join(string.Empty, sessionBytes);

            // Join the session ID with the numeric code
            return $"{encodedSession},{Offset}";
        }

        /// <summary>
        /// Parses the specified fax code and sender into a <see cref="ScanFilePrefix"/> instance.
        /// </summary>
        /// <param name="faxCode">The fax code.</param>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        public static ScanFilePrefix ParseFromFax(string faxCode, string sender)
        {
            Match match = Regex.Match(faxCode, "(.*),(.*)");
            if (match.Success)
            {
                // Decode the session ID from ASCII
                var bytePairs = Split(match.Groups[1].Value, 2);
                var sessionBytes = bytePairs.Select(n => byte.Parse(n, CultureInfo.InvariantCulture)).ToArray();
                string sessionId = Encoding.ASCII.GetString(sessionBytes);

                return new ScanFilePrefix(sessionId, sender, "Fax", int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture));
            }
            else
            {
                throw new FormatException("Fax code was not in the correct format.");
            }
        }

        /// <summary>
        /// Determines whether the specified file name has the correct pattern
        /// for a name generated by this class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static bool MatchesPattern(string fileName)
        {
            return MatchPattern(fileName).Success;
        }

        private static Match MatchPattern(string fileName)
        {
            string matchAll = "(.*)";
            var escapedDelimiters = _delimiters.Select(n => Regex.Escape(n.ToString()));

            string pattern = matchAll + string.Join(matchAll, escapedDelimiters) + matchAll;
            return Regex.Match(fileName, pattern);
        }

        private static string ToProper(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            else
            {
                string lower = data.ToLowerInvariant();
                string firstChar = lower[0].ToString().ToUpperInvariant();
                return string.Concat(firstChar, lower.Substring(1));
            }
        }

        private static List<string> Split(string value, int size)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            List<string> splitString = new List<string>();

            // Determine how much of the string will divide in evenly
            int totalLength = value.Length;
            int evenLength = totalLength - (totalLength % size);

            // Break up the string
            for (int i = 0; i < evenLength; i += size)
            {
                splitString.Add(value.Substring(i, size));
            }

            // Pick up the leftovers, if any
            if (totalLength > evenLength)
            {
                splitString.Add(value.Substring(evenLength));
            }

            return splitString;
        }
    }
}
