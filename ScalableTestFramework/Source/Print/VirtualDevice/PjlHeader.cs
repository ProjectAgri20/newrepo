using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// A PJL header for a print job.
    /// </summary>
    public sealed class PjlHeader
    {
        /// <summary>
        /// Gets the PJL job name.
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Gets the PJL language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets the PJL JobAcct fields.
        /// </summary>
        /// <remarks>
        /// For many HP print drivers, the following JobAcct mapping is be used:
        /// JobAcct1: User name
        /// JobAcct2: Host name (sometimes)
        /// JobAcct3: Host name (the rest of the time)
        /// JobAcct4: Date/time job was submitted
        /// </remarks>
        public Dictionary<int, string> JobAccts { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the PJL comments.
        /// </summary>
        /// <remarks>
        /// For many HP print drivers, the Comment fields contain the Print Queue information.
        /// </remarks>
        public Collection<string> Comments { get; } = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PjlHeader" /> class.
        /// </summary>
        public PjlHeader()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
