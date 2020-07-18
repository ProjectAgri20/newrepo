using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Core.UI
{
    /// <summary>
    /// Manages setting and resetting a busy cursor for long-running operations.
    /// </summary>
    public sealed class BusyCursor : IDisposable
    {
        private readonly Cursor _originalCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyCursor" /> class.
        /// </summary>
        public BusyCursor()
        {
            _originalCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Cursor.Current = _originalCursor;
        }
    }
}
