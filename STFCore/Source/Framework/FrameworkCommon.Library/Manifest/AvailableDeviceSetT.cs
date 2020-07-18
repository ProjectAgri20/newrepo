using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Represents a collection of devices which enforce availability rules.
    /// </summary>
    [DataContract]
    public class AvailableDeviceSet<T> where T : IAvailable
    {
        [DataMember]
        private Collection<T> _deviceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableDeviceSet"/> class.
        /// </summary>
        public AvailableDeviceSet()
        {
            _deviceCollection = new Collection<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableDeviceSet"/> class.
        /// </summary>
        /// <param name="assets">The assets.</param>
        public AvailableDeviceSet(Collection<T> devices)
        {
            _deviceCollection = devices;
        }

        /// <summary>
        /// Gets the item with the specified device id.
        /// </summary>
        public T this[string inventoryId]
        {
            get
            {
                T device = FindItem(inventoryId);
                if (device != null && IsAvailable(device))
                {
                    return device;
                }
                else
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Adds the specified device to the collection.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Add(T device)
        {
            if (! _deviceCollection.Contains(device))
            {
                _deviceCollection.Add(device);
            }
        }

        /// <summary>
        /// Gets a collection of available devices.
        /// </summary>
        public Collection<T> Devices
        {
            get
            {
                Collection<T> result = new Collection<T>();
                foreach (T device in _deviceCollection)
                {
                    if (IsAvailable(device))
                    {
                        result.Add(device);
                    }
                }
                return result;
            }
        }

        internal Collection<T> Items
        {
            get { return _deviceCollection; }
        }

        /// <summary>
        /// Determines whether the asset with the specified asset id is available.
        /// </summary>
        /// <param name="inventoryId">The asset id.</param>
        /// <returns>
        ///   <c>true</c> if the specified asset is available; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAvailable(string inventoryId)
        {
            T device = FindItem(inventoryId);
            if (device == null)
            {
                TraceFactory.Logger.Debug("Asset is null: {0}".FormatWith(inventoryId));
                return false;
            }

            return IsAvailable(device);
        }

        private static bool IsAvailable(T device)
        {
            bool isAvailable = true;

            if (device.AvailabilityStartTime > DateTime.Now)
            {
                isAvailable = false;
                TraceFactory.Logger.Debug("Availability time not yet reached: {0}".FormatWith(device.AvailabilityStartTime));
            }

            if (device.AvailabilityEndTime < DateTime.Now)
            {
                isAvailable = false;
                TraceFactory.Logger.Debug("Availability time expired: {0}".FormatWith(device.AvailabilityEndTime));
            }

            if (! device.Available)
            {
                isAvailable = false;
                TraceFactory.Logger.Debug("Asset is Offline: {0}".FormatWith(device.InventoryId));
            }

            return isAvailable;
        }

        private T FindItem(string inventoryId)
        {
            return _deviceCollection.FirstOrDefault(n => n.InventoryId == inventoryId);
        }

        /// <summary>
        /// Selects a single random available asset from the specified ids.
        /// </summary>
        /// <param name="assetIds">The asset ids.</param>
        /// <returns></returns>
        public T SelectDevice(Collection<string> inventoryIds)
        {
            Collection<T> devices = SelectDevices(inventoryIds);
            int count = devices.Count;
            TraceFactory.Logger.Debug(count + " devices available.");
            if (count == 0)
            {
                return default(T);
            }

            T result = devices[new Random().Next(count)];
            TraceFactory.Logger.Debug("Selected device: " + result.InventoryId);
            return result;
        }

        /// <summary>
        /// Gets a collection of all available devices with any of the specified ids.
        /// </summary>
        /// <param name="inventoryIds">The asset ids.</param>
        /// <returns></returns>
        public Collection<T> SelectDevices(Collection<string> inventoryIds)
        {
            if (inventoryIds == null)
            {
                throw new ArgumentNullException("inventoryIds");
            }

            Collection<T> result = new Collection<T>();
            foreach (string inventoryId in inventoryIds)
            {
                if (IsAvailable(inventoryId))
                {
                    result.Add(FindItem(inventoryId));
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the available property of the specified device Id.
        /// </summary>
        /// <param name="inventoryId">The device Id.</param>
        /// <param name="isAvailable">Whether or not the device is available.</param>
        public void SetAvailable(string inventoryId, bool isAvailable)
        {
            T item = FindItem(inventoryId);
            if (item != null)
            {
                item.Available = isAvailable;
            }
        }

        /// <summary>
        /// Sets the available property of the specified device Id.
        /// </summary>
        /// <param name="inventoryId">The device Id.</param>
        /// <param name="isAvailable">Whether or not the device is available.</param>
        /// <param name="availabilityEndTime">The expiration date/time of the device.</param>
        internal void SetAvailable(string inventoryId, bool isAvailable, DateTime availabilityEndTime)
        {
            T item = FindItem(inventoryId);
            if (item != null)
            {
                item.Available = isAvailable;
                item.AvailabilityEndTime = availabilityEndTime;
            }
        }
    }
}