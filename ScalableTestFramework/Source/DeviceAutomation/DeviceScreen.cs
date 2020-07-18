using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Provides information about the control panel screen.
    /// </summary>
    public class DeviceScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceScreen"/> class.
        /// </summary>
        /// <param name="controlPanel">The device control panel object.</param>
        public DeviceScreen(IJavaScriptExecutor controlPanel)
        {
            if (controlPanel is JediOmniControlPanel omni)
            {
                Size = omni.GetScreenSize(); //Get the screen size.  Could be one of 3 different sizes for Omni.
            }
            else
            {
                Size = new Size(800, 600);
            }
        }

        /// <summary>
        /// Gets the size of the Screen.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Is the device screen the small screen size (275H X 480W).
        /// </summary>
        /// <returns><c>true</c> if the screen size is the small size, <c>false</c> otherwise.</returns>
        public bool IsSmallSize
        {
            get { return Size.Width == 480; }
        }

        /// <summary>
        /// Is the device screen the default screen size (768H X 1024W).
        /// </summary>
        /// <returns><c>true</c> if the screen size is high definition, <c>false</c> otherwise.</returns>
        public bool IsHighDef
        {
            get { return Size.Width == 1024; }
        }

        /// <summary>
        /// Is the device screen the default screen size (600H X 800W).
        /// </summary>
        /// <returns><c>true</c> if the screen size is the default size, <c>false</c> otherwise.</returns>
        public bool IsDefaultSize
        {
            get { return Size.Width == 800; }
        }

        /// <summary>
        /// Returns whether the specified <see cref="Coordinate"/> exists within the screen boundaries.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns><c>true</c> if the coordinate falls within the screen size boundaries, <c>false</c> otherwise.</returns>
        public bool Contains(Coordinate coordinate)
        {
            return (coordinate.Y <= Size.Height) && (coordinate.X <= Size.Width);
        }
    }
}
