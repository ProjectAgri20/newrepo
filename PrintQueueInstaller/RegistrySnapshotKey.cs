using System.Text;
using Microsoft.Win32;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Represents a Registry Key for the snapshot
    /// </summary>
    public class RegistrySnapshotKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotKey"/> class.
        /// </summary>
        public RegistrySnapshotKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotKey"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="kind">The kind.</param>
        /// <param name="value">The value.</param>
        public RegistrySnapshotKey(RegistryKey key, string name)
        {
            Name = name;
            Kind = key.GetValueKind(name);
            Value = key.GetValue(name);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        public RegistryValueKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Returns a CSV based string representation of the data
        /// </summary>
        /// <param name="includeBinary">if set to <c>true</c> include binary data.</param>
        /// <returns></returns>
        public string ToStringCsv(bool includeBinary = false)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Name);
            builder.Append(",");
            builder.Append(Kind);
            builder.Append(",");
            builder.Append(GetValue(includeBinary));

            return builder.ToString();
        }

        public string GetValue(bool includeBinary = false)
        {
            StringBuilder builder = new StringBuilder();
            switch (Kind)
            {
                case RegistryValueKind.Binary:
                    byte[] binaryData = (byte[])Value;
                    if (includeBinary)
                    {
                        if (Value != null)
                        {
                            foreach (byte b in binaryData)
                            {
                                builder.AppendFormat("{0:x2}".ToLower(), b);
                            }
                        }
                    }
                    else
                    {
                        builder.Append(string.Format("Not Viewable - {0} bytes", binaryData.Length));
                    }
                    break;
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    builder.Append(Value);
                    break;
                case RegistryValueKind.MultiString:
                    if (Value != null)
                    {
                        string[] items = (string[])Value;

                        foreach (string item in items)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                builder.Append(string.Format("[{0}] ", item));
                            }
                        }
                    }
                    break;
                case RegistryValueKind.None:
                    builder.Append("None");
                    break;
                case RegistryValueKind.Unknown:
                    builder.Append("Unknown");
                    break;
            }

            return builder.ToString();
        }
    }
}
