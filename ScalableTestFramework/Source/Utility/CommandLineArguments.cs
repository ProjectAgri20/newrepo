using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Parses command line arguments used in a console application.
    /// </summary>
    /// <remarks>
    /// Valid parameters forms:
    /// {-,/,--}param{ ,=,:}((",')value(",'))
    /// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
    /// </remarks>
    public sealed class CommandLineArguments
    {
        private static readonly Regex parameterPrefixes = new Regex(@"^-{1,2}|^/", RegexOptions.Compiled);
        private static readonly Regex parameterSplitter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.Compiled);
        private static readonly Regex quoteRemover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.Compiled);

        private readonly List<string> _arguments = new List<string>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the value of the specified argument parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The value of the specified parameter.</returns>
        /// <exception cref="KeyNotFoundException">The specified parameter was not found.</exception>
        public string this[string parameter] => _parameters[parameter].ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArguments" /> class.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <exception cref="ArgumentNullException"><paramref name="args" /> is null.</exception>
        public CommandLineArguments(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            // Store the original arguments, with no modifications
            _arguments.AddRange(args);

            // Parse each argument into parameters and values
            string lastParameter = null;
            foreach (string text in args)
            {
                // Determine whether this is a parameter
                if (parameterPrefixes.IsMatch(text))
                {
                    // Argument starts with one of the parameter flags
                    // Split out the parameter and a possible enclosed value
                    string[] argumentParts = parameterSplitter.Split(text, 3);
                    switch (argumentParts.Length)
                    {
                        // Found a parameter
                        case 2:
                            // We don't know whether this will have an associated value, so add it with a default
                            _parameters[argumentParts[1]] = true;
                            lastParameter = argumentParts[1];
                            break;

                        // Found a parameter with an enclosed value
                        case 3:
                            _parameters[argumentParts[1]] = quoteRemover.Replace(argumentParts[2], "$1");
                            lastParameter = null;
                            break;
                    }
                }
                else
                {
                    // This is a value.  If we have a parameter waiting, assign this value to that parameter
                    if (lastParameter != null)
                    {
                        _parameters[lastParameter] = quoteRemover.Replace(text, "$1");
                        lastParameter = null;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this instance has a value for the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to look for.</param>
        /// <returns><c>true</c> if the specified parameter was found; otherwise, <c>false</c>.</returns>
        public bool HasParameter(string parameter)
        {
            return _parameters.ContainsKey(parameter);
        }

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to retrieve.</param>
        /// <returns>The value of the requested parameter, if found; null otherwise.</returns>
        public string GetParameterValue(string parameter)
        {
            if (_parameters.TryGetValue(parameter, out object value))
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the value of the specified parameter cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="parameter">The parameter to retrieve.</param>
        /// <returns>The value of the requested parameter, if found; otherwise, returns the default value of <typeparamref name="T" />.</returns>
        /// <exception cref="FormatException">The property value could not be converted to the specified return type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidCastException">Conversion to the specified type is not supported.</exception>
        public T GetParameterValue<T>(string parameter) where T : struct
        {
            string value = GetParameterValue(parameter);
            if (value != null)
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Join(" ", _arguments);
        }
    }
}
