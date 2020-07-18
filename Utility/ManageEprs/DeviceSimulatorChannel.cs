using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Xml;
using HP.Epr.Client;
using HP.ScalableTest.Network;

namespace HP.ScalableTest.EndpointResponder
{
    /// <summary>
    /// Class DeviceSimulatorChannel, used to communicate with an Endpoint Responder service to 
    /// control devices instances, profiles and other features of the service.
    /// </summary>
    public class DeviceSimulatorChannel
    {
        private readonly string _eprServerName = string.Empty;
        private DeviceSimulatorClient _channel = null;

        /// <summary>
        /// Occurs when an event notifications happens on the device simulator.
        /// </summary>
        public event EventHandler<EprNotificationEventArgs> OnEventNotification;

        /// <summary>
        /// Occurs when there is an update with the device recorder.
        /// </summary>
        public event EventHandler<DeviceRecorderStatusUpdateEventArgs> DeviceRecorderStatusUpdate;

        /// <summary>
        /// Occurs when device recorder complete.
        /// </summary>
        public event EventHandler<EventArgs> DeviceRecorderCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSimulatorChannel"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        public DeviceSimulatorChannel(string serverName)
        {
            _eprServerName = serverName;
        }

        private DeviceSimulatorClient Channel
        {
            get
            {
                if (_channel == null)
                {
                    DeviceSimulatorCallback callback = new DeviceSimulatorCallback();
                    callback.DeviceRecorderComplete += callback_DeviceRecorderComplete;
                    callback.DeviceRecorderStatusUpdate += callback_DeviceRecorderStatusUpdate;
                    callback.OnEventNotification += callback_OnEventNotification;
                    InstanceContext ctx = new InstanceContext(callback);

                    var binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
                    binding.ClientBaseAddress = new Uri(string.Format("http://{0}:12763/DeviceSimulatorCallback".FormatWith(NetworkUtil.GetLocalAddress())));
                    binding.MaxReceivedMessageSize = Int32.MaxValue;
                    binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;

                    var endPoint = new EndpointAddress("http://{0}:12345/EndPointResponder".FormatWith(_eprServerName));

                    _channel = new DeviceSimulatorClient(ctx, binding, endPoint);
                }

                return _channel;
            }
        }

        private void callback_DeviceRecorderStatusUpdate(object sender, DeviceRecorderStatusUpdateEventArgs e)
        {
            if (DeviceRecorderStatusUpdate != null)
            {
                DeviceRecorderStatusUpdate(this, e);
            }
        }

        private void callback_DeviceRecorderComplete(object sender, EventArgs e)
        {
            if (DeviceRecorderCompleted != null)
            {
                DeviceRecorderCompleted(this, e);
            }
        }

        private void callback_OnEventNotification(object sender, EprNotificationEventArgs e)
        {
            if (OnEventNotification != null)
            {
                OnEventNotification(this, e);
            }
        }

        /// <summary>
        /// Installs the profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="profilePath">The profile path.</param>
        public void InstallProfile(string profileName, string profilePath)
        {
            Channel.InstallProfile(profileName, profilePath);
        }

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        public void DeleteProfile(string profileName)
        {
            Channel.DeleteProfile(profileName);
        }

        /// <summary>
        /// Gets the installed profiles.
        /// </summary>
        /// <returns>A collection of installed profiles on the server</returns>
        public Collection<DeviceProfile> IntalledProfiles
        {
            get
            {
                Collection<DeviceProfile> profiles = new Collection<DeviceProfile>();

                foreach (var profile in Channel.GetInstalledProfiles())
                {
                    profiles.Add(profile);
                }

                return profiles;
            }
        }

        /// <summary>
        /// Creates the profile from device.
        /// </summary>
        /// <param name="deviceAddress">The device address.</param>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void CreateProfileFromDevice(IPAddress deviceAddress, string profileName, string username, string password, string community)
        {
            Channel.CreateProfileFromDevice(deviceAddress.ToEx(), profileName, username, password, community);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns></returns>
        public DeviceInstance CreateInstance(string profileName, Collection<Protocol> protocols)
        {
            return Channel.CreateInstanceWithProtocolList(profileName, protocols.ToList().ToArray());
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="deviceAddress">The device address.</param>
        public DeviceInstance CreateInstance(string profileName, IPAddress deviceAddress, Collection<Protocol> protocols)
        {
            return Channel.CreateInstanceWithIP(profileName, deviceAddress.ToEx(), protocols.ToList().ToArray());
        }

        /// <summary>
        /// Destroys the instance.
        /// </summary>
        /// <param name="address">The address of the device instance.</param>
        public void DestroyInstance(IPAddress address)
        {
            Channel.DestroyInstance(address.ToEx());
        }

        /// <summary>
        /// Starts the instance.
        /// </summary>
        /// <param name="address">The address of the device instance.</param>
        public void StartInstance(IPAddress address)
        {
            Channel.StartInstance(address.ToEx());
        }

        /// <summary>
        /// Stops the instance.
        /// </summary>
        /// <param name="address">The address of the device instance.</param>
        public void StopInstance(IPAddress address)
        {
            Channel.StopInstance(address.ToEx());
        }

        /// <summary>
        /// Gets the instances.
        /// </summary>
        /// <returns>A collection of device instances</returns>
        public Collection<DeviceInstance> Instances
        {
            get
            {
                Collection<DeviceInstance> instances = new Collection<DeviceInstance>();
                foreach (var instance in Channel.GetInstances())
                {
                    instances.Add(instance);
                }
                return instances;
            }
        }

        /// <summary>
        /// Starts the notification scheduler.
        /// </summary>
        public void StartNotificationScheduler()
        {
            Channel.StartNotificationScheduler();
        }

        /// <summary>
        /// Destroys all instances.
        /// </summary>
        public void DestroyAllInstances()
        {
            Channel.DestroyAllInstances();
        }

        /// <summary>
        /// Gets the available IP addresses installed on the server.
        /// </summary>
        /// <returns></returns>
        public Collection<IPAddress> AvailableIPAddresses
        {
            get
            {
                return Channel.GetAvailableIPAddresses().ToIP();
            }
        }

        /// <summary>
        /// Registers for callbacks.
        /// </summary>
        public void RegisterForCallbacks()
        {
            Channel.RegisterForCallbacks();
        }

        private class DeviceSimulatorCallback : IDeviceSimulatorCallback
        {
            public event EventHandler<EprNotificationEventArgs> OnEventNotification;
            public event EventHandler<DeviceRecorderStatusUpdateEventArgs> DeviceRecorderStatusUpdate;
            public event EventHandler<EventArgs> DeviceRecorderComplete;

            public void OnDeviceRecorderStatusUpdate(string message)
            {
                if (DeviceRecorderStatusUpdate != null)
                {
                    DeviceRecorderStatusUpdate(this, new DeviceRecorderStatusUpdateEventArgs(message));
                }
            }

            public void OnDeviceRecorderComplete()
            {
                if (DeviceRecorderComplete != null)
                {
                    DeviceRecorderComplete(this, EventArgs.Empty);
                }
            }

            public void OnNotification(XmlElement data)
            {
                throw new NotImplementedException();
            }
        }
    }
}
