using System.Collections.Generic;
using System.ComponentModel;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Public class that maintains the various device Sign out Methods.
    /// </summary>
    public class SignOutMethod
    {
        /// <summary>
        /// Gets the press sign out description.
        /// </summary>
        /// <value>The press sign out.</value>
        public static string PressSignOut { get; } = DeviceSignOutMethod.PressSignOut.GetDescription();
        /// <summary>
        /// Gets the press rest hard key description.
        /// </summary>
        /// <value>The press rest hard key.</value>
        public static string PressRestHardKey { get; } = DeviceSignOutMethod.PressResetHardKey.GetDescription();
        /// <summary>
        /// Gets the press rest soft key description.
        /// </summary>
        /// <value>The press rest soft key.</value>
        public static string PressRestSoftKey { get; } = DeviceSignOutMethod.PressResetSoftKey.GetDescription();
        /// <summary>
        /// Gets the do not sign out description
        /// </summary>
        /// <value>The press rest soft key.</value>
        public static string DoNotSignOut { get; } = DeviceSignOutMethod.DoNotSignOut.GetDescription();
        /// <summary>
        /// Gets the inactivity timeout description.
        /// </summary>
        /// <value>The inactivity timeout.</value>
        public static string InactivityTimeout { get; } = DeviceSignOutMethod.Timeout.GetDescription();

        

        private List<string> _signOutMethods = new List<string>();
        /// <summary>
        /// Gets the sign out method values.
        /// </summary>
        /// <value>The sign out method values.</value>
        public IEnumerable<string> SignOutMethodValues => _signOutMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignOutMethod" /> class.
        /// </summary>
        public SignOutMethod()
        {
            _signOutMethods.Add(PressSignOut);
            _signOutMethods.Add(PressRestHardKey);
            _signOutMethods.Add(PressRestSoftKey);
            _signOutMethods.Add(DoNotSignOut);
            _signOutMethods.Add(InactivityTimeout);          
        }

        /// <summary>
        /// Gets the un authenticate method.
        /// </summary>
        /// <param name="getSignOutMethod">The un authenticate method.</param>
        /// <returns></returns>
        public static DeviceSignOutMethod GetSignOutMethod(string getSignOutMethod)
        {
            DeviceSignOutMethod dum = EnumUtil.GetByDescription<DeviceSignOutMethod>(getSignOutMethod);
            return dum;
        }
    }
    /// <summary>
    /// Enum DeviceSignOutMethod
    /// </summary>
    public enum DeviceSignOutMethod
    {
        /// <summary>
        /// Press the sign out button
        /// </summary>
        [Description("Press Sign Out")]
        PressSignOut,
        /// <summary>
        /// Press the device reset hard key
        /// </summary>
        [Description("Press Reset Hard Key")]
        PressResetHardKey,
        /// <summary>
        /// Press the reset soft key
        /// </summary>
        [Description("Press Reset Soft Key")]
        PressResetSoftKey,
        /// <summary>
        /// Do Not Sign Out
        /// </summary>
        [Description("Do Not Sign Out")]
        DoNotSignOut,
        /// <summary>
        /// Wait for the device to timeout
        /// </summary>
        [Description("Wait For Inactivity Timeout")]
        Timeout
    }

}
