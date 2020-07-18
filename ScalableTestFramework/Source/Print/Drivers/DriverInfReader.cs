using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Reads driver INF files and splits them into sections.
    /// </summary>
    /// <remarks>
    /// This class uses a look-ahead technique along with heavy caching to read as little of the INF file
    /// as possible.  When a specific section is needed, each section is read in and cached until the requested
    /// section is found.  Subsequent requests will use the cache rather than backtracking through the file.
    /// The look-ahead is used to determine when a section ends, since INF files do not have a delimiter for such.
    /// </remarks>
    public sealed class DriverInfReader : IDisposable
    {
        private readonly StreamReader _reader;
        private readonly Dictionary<string, InfSection> _sections = new Dictionary<string, InfSection>(StringComparer.OrdinalIgnoreCase);
        private string _nextLine;

        /// <summary>
        /// Gets the location of the INF file this instance is reading.
        /// </summary>
        public string FileLocation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverInfReader" /> class.
        /// </summary>
        /// <param name="fileLocation">The file location.</param>
        public DriverInfReader(string fileLocation)
        {
            FileLocation = fileLocation;
            _reader = new StreamReader(fileLocation);

            // Prime our look-ahead reader.
            ReadLine();
        }

        /// <summary>
        /// Gets the specified section of the INF file.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>The lines from the specified section.</returns>
        public IEnumerable<string> GetSection(string section)
        {
            // If the section has already been read, return it from the cache.
            if (_sections.ContainsKey(section))
            {
                return _sections[section];
            }
            else
            {
                return ReadUntilSection(section);
            }
        }

        private InfSection ReadUntilSection(string sectionName)
        {
            InfSection section = ReadNextSection();
            while (section != null)
            {
                _sections[section.Name] = section;
                if (section.Name.EqualsIgnoreCase(sectionName))
                {
                    return section;
                }
                section = ReadNextSection();
            }

            return new InfSection(sectionName);
        }

        /// <summary>
        /// Reads the next section from the INF file.
        /// </summary>
        /// <returns>The next section from the INF file.</returns>
        private InfSection ReadNextSection()
        {
            InfSection section = null;
            string line = null;
            do
            {
                line = ReadLine();

                if (section != null)
                {
                    // We are inside a section.  Load the next line if it is not blank.
                    if (!string.IsNullOrEmpty(line))
                    {
                        section.Add(line);
                    }

                    // If the next line is a section line, then we are done with this section.
                    if (IsSectionLine(_nextLine))
                    {
                        return section;
                    }
                }
                else
                {
                    // Check to see if the current line is a section.
                    if (IsSectionLine(line))
                    {
                        section = new InfSection(line);

                        // Corner case if a section is immediately followed by another section.
                        if (IsSectionLine(_nextLine))
                        {
                            return section;
                        }
                    }
                }
            } while (line != null);

            // We have reached the end of the file.
            return section;
        }

        /// <summary>
        /// Reads the next line from the INF file.
        /// </summary>
        /// <returns>The next line from the INF file.</returns>
        /// <remarks>
        /// This method treats multiple consecutive blank lines as a single line.
        /// All comments are treated as blank lines as well.
        /// </remarks>
        private string ReadLine()
        {
            string line = _nextLine;
            while (!_reader.EndOfStream)
            {
                line = _nextLine;
                _nextLine = CleanUpLine(_reader.ReadLine());

                // If either this line or the next is not blank, return it.
                // Otherwise, skip this one and read the next.
                if (!string.IsNullOrEmpty(line) || !string.IsNullOrEmpty(_nextLine))
                {
                    return line;
                }
            }

            // End of the file - treat the next line as "null"
            _nextLine = null;
            return line;
        }

        /// <summary>
        /// Cleans up a line by removing extra white space and comments.
        /// </summary>
        /// <param name="line">The line to clean up.</param>
        /// <returns>The cleaned up line.</returns>
        private static string CleanUpLine(string line)
        {
            string cleanLine = line.Trim();

            // If the line is blank or a comment, return an empty string
            if (string.IsNullOrEmpty(cleanLine) || cleanLine.StartsWith(";"))
            {
                return string.Empty;
            }

            // Another location where a comment can show up is on the same line as a section name.
            // If this happens, strip everything after the semicolon.
            if (cleanLine.StartsWith("[") && cleanLine.Contains(";"))
            {
                cleanLine = cleanLine.Substring(0, cleanLine.IndexOf(';')).Trim();
            }

            return cleanLine;
        }

        private static bool IsSectionLine(string line)
        {
            return line != null && Regex.IsMatch(line, @"^\[[^\]]*\]$");
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _reader.Dispose();
        }

        #endregion

        private sealed class InfSection : IEnumerable<string>
        {
            private readonly List<string> _lines = new List<string>();

            public string Name { get; }

            public InfSection(string sectionLine)
            {
                Name = sectionLine.TrimStart('[').TrimEnd(']');
            }

            public void Add(string line) => _lines.Add(line);
            public IEnumerator<string> GetEnumerator() => _lines.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _lines.GetEnumerator();
        }
    }
}
