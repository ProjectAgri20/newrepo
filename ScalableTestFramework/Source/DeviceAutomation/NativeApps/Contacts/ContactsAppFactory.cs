using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Contacts
{
    /// <summary>
    /// Factory for creating <see cref="IContactsApp" /> objects.
    /// </summary>
    public sealed class ContactsAppFactory : DeviceFactoryCore<IContactsApp>
    {
        private static ContactsAppFactory _instance = new ContactsAppFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsAppFactory" /> class.
        /// </summary>
        private ContactsAppFactory()
        {
            Add<JediOmniDevice, JediOmniContactsApp>();
        }
        /// <summary>
        /// Creates an <see cref="IContactsApp" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IContactsApp" /> for the specified device.</returns>
        public static IContactsApp Create(IDevice device) => _instance.FactoryCreate(device);
    }
}
