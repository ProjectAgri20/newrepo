using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// A read-only dictionary of named settings.
    /// </summary>
    [DataContract]
    public sealed class SettingsDictionary : ReadOnlyDictionary<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDictionary" /> class.
        /// </summary>
        /// <param name="settings">The dictionary of settings to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is null.</exception>
        public SettingsDictionary(IDictionary<string, string> settings)
            : base(settings)
        {
        }
    }
}
