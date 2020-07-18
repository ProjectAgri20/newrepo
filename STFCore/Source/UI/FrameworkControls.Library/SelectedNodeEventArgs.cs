using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Event Arguments object used for node changes.
    /// </summary>
    public class SelectedNodeEventArgs : EventArgs
    {
        public object Tag { get; private set; }
        public string Name { get; private set; }
        public string ImageKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedNodeEventArgs"/> class.
        /// </summary>
        public SelectedNodeEventArgs()
            : base()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedNodeEventArgs"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="imageKey"></param>
        /// <param name="tag"></param>
        public SelectedNodeEventArgs(string name, string imageKey, object tag)
            : base()
        {
            Name = name;
            ImageKey = imageKey;
            Tag = tag;
        }
    }
}
