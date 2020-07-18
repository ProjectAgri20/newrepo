using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Class for killing popup windows that appear during automation and block activity execution.
    /// </summary>
    public sealed class PopupAssassin : IDisposable
    {
        private readonly List<string> _windowTitles = new List<string>();
        private readonly Timer _timer;
        private TimeSpan _scanInterval;

        private PopupAssassin()
        {
            _timer = new Timer(TimerCallback);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupAssassin" /> class.
        /// </summary>
        /// <param name="windowTitle">The title of the window this <see cref="PopupAssassin" /> should kill.</param>
        public PopupAssassin(string windowTitle)
            : this()
        {
            _windowTitles.Add(windowTitle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupAssassin" /> class.
        /// </summary>
        /// <param name="windowTitles">A list of titles of the windows this <see cref="PopupAssassin" /> should kill.</param>
        /// <exception cref="ArgumentNullException"><paramref name="windowTitles" /> is null.</exception>
        public PopupAssassin(IEnumerable<string> windowTitles)
            : this()
        {
            if (windowTitles == null)
            {
                throw new ArgumentNullException(nameof(windowTitles));
            }

            _windowTitles.AddRange(windowTitles);
        }

        /// <summary>
        /// Starts this instance and instructs it to scan for popups at the specified interval.
        /// </summary>
        /// <param name="scanInterval">The scan interval.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="scanInterval" /> is less than or equal to <see cref="TimeSpan.Zero" />.</exception>
        public void Start(TimeSpan scanInterval)
        {
            if (scanInterval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(scanInterval), "Scan interval must be greater than zero.");
            }

            LogDebug("Starting popup assassin.");
            _scanInterval = scanInterval;
            _timer.Change(0, Timeout.Infinite);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            LogDebug("Stopping popup assassin.");
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void TimerCallback(object unused)
        {
            _windowTitles.ForEach(KillWindow);
            _timer.Change(_scanInterval, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Kills all windows with the specified title.
        /// </summary>
        /// <param name="windowTitle">The title of the window to kill.</param>
        public static void KillWindow(string windowTitle)
        {
            IntPtr handle = NativeMethods.FindWindow(null, windowTitle);

            while (handle != IntPtr.Zero)
            {
                LogDebug($"Killing popup window '{windowTitle}'.");

                NativeMethods.SendMessage(handle, NativeMethods.WindowMessage.Command, IntPtr.Zero, IntPtr.Zero);
                NativeMethods.SendMessage(handle, NativeMethods.WindowMessage.Destroy, IntPtr.Zero, IntPtr.Zero);
                NativeMethods.SendMessage(handle, NativeMethods.WindowMessage.NCDestroy, IntPtr.Zero, IntPtr.Zero);
                NativeMethods.SendMessage(handle, NativeMethods.WindowMessage.Close, IntPtr.Zero, IntPtr.Zero);

                handle = NativeMethods.FindWindow(null, windowTitle);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }
        }

        #endregion

        #region DLL Imports

        private static class NativeMethods
        {
            internal enum WindowMessage : uint
            {
                Destroy = 0x0002,
                Close = 0x0010,
                NCDestroy = 0x0082,
                Command = 0x0111,
            }

            [DllImport("user32", CharSet = CharSet.Unicode)]
            internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32")]
            internal static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
        }

        #endregion
    }
}
