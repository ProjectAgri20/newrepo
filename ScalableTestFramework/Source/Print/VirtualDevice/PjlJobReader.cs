using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// Reads a PJL print job and generates a corresponding <see cref="PjlHeader" />.
    /// </summary>
    public sealed class PjlJobReader
    {
        private const string _pjlEscapeSequence = "\u001b%-12345X";
        private const string _jobNameRegex = "@PJL (SET )? JOB NAME = \"(.*?)\"";
        private const string _languageRegex = "@PJL ENTER LANGUAGE = (.*)";
        private const string _jobAcctRegex = "@PJL SET JOBATTR = \"JobAcct(\\d+)=(.*)\"";
        private const string _commentRegex = "@PJL COMMENT \"(.*)\"";

        private readonly StringBuilder _currentRow = new StringBuilder();
        private bool _insidePjlMode = false;
        private int _pjlEscapePosition = 0;

        /// <summary>
        /// Gets the <see cref="PjlHeader" /> read from the PJL job.
        /// </summary>
        public PjlHeader Header { get; } = new PjlHeader();

        /// <summary>
        /// Initializes a new instance of the <see cref="PjlJobReader" /> class.
        /// </summary>
        public PjlJobReader()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Reads the specified bytes into the print job data.
        /// </summary>
        /// <param name="jobBuffer">The buffer containing the job data.</param>
        /// <param name="size">The size of the data.</param>
        public void ReadPrintJobBytes(byte[] jobBuffer, int size)
        {
            foreach (char jobByte in jobBuffer.Take(size).Select(n => (char)n))
            {
                // Check to see if this character signifies entering PJL mode
                if (EnteringPjlMode(jobByte))
                {
                    // Set the flag and then skip to the next character
                    _insidePjlMode = true;
                    continue;
                }

                // If inside PJL mode, process the next character
                if (_insidePjlMode)
                {
                    // If end of line, process the current row
                    if (jobByte == '\n')
                    {
                        string pjlCommand = _currentRow.ToString();
                        ProcessHeaderRow(pjlCommand);
                        _currentRow.Clear();

                        // See if this command is to exit PJL
                        if (PjlRegexMatch(pjlCommand, _languageRegex).Success)
                        {
                            _insidePjlMode = false;
                        }
                    }
                    else
                    {
                        _currentRow.Append(jobByte);
                    }
                }
            }
        }

        private bool EnteringPjlMode(char jobByte)
        {
            // Check to see if the 
            if (jobByte == _pjlEscapeSequence[_pjlEscapePosition])
            {
                _pjlEscapePosition++;
                if (_pjlEscapePosition == _pjlEscapeSequence.Length)
                {
                    // Escape sequence was found - reset position and clear current row text
                    _pjlEscapePosition = 0;
                    _currentRow.Clear();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Did not match the next charater in the escape sequence - reset position
                _pjlEscapePosition = 0;
                return false;
            }
        }

        private void ProcessHeaderRow(string pjlHeaderRow)
        {
            Match regexMatch = null;

            // Check to see if this row contains the job name
            if (string.IsNullOrEmpty(Header.JobName))
            {
                regexMatch = PjlRegexMatch(pjlHeaderRow, _jobNameRegex);
                if (regexMatch.Success)
                {
                    Header.JobName = regexMatch.Groups[2].Value;
                    return;
                }
            }

            // Check to see if this row contains the PJL language
            if (string.IsNullOrEmpty(Header.Language))
            {
                regexMatch = PjlRegexMatch(pjlHeaderRow, _languageRegex);
                if (regexMatch.Success)
                {
                    Header.Language = regexMatch.Groups[1].Value;
                    return;
                }
            }

            // Check to see if this row contains a JobAcct item
            regexMatch = PjlRegexMatch(pjlHeaderRow, _jobAcctRegex);
            if (regexMatch.Success)
            {
                int jobAcctNumber = int.Parse(regexMatch.Groups[1].Value);
                Header.JobAccts[jobAcctNumber] = regexMatch.Groups[2].Value;
                return;
            }

            // Check to see if this row contains a comment
            regexMatch = PjlRegexMatch(pjlHeaderRow, _commentRegex);
            if (regexMatch.Success)
            {
                Header.Comments.Add(regexMatch.Groups[1].Value);
                return;
            }

            // Ignore everything else
        }

        private static Match PjlRegexMatch(string pjlHeaderRow, string pattern)
        {
            // PJL allows any amount of whitespace in between commands and identifiers.
            // Replace any spaces in the regex with a pattern that accepts arbitrary whitespace.
            string whiteSpacePattern = pattern.Replace(" ", @"\s*");
            return Regex.Match(pjlHeaderRow.TrimEnd(), whiteSpacePattern, RegexOptions.IgnoreCase);
        }
    }
}
