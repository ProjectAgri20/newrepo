using System;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
	/// <summary>
	/// Property class for changing devices used in a scenario
	/// </summary>
	public class BulkDeviceEnt
	{
        public bool Active { get; set; }
		/// <summary>
		/// Gets or sets the current device used in a scenario.
		/// </summary>
		/// <value>
		/// The current device.
		/// </value>
		public string CurrentDevice { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [device changed].
		/// </summary>
		/// <value>
		///   <c>true</c> if [device changed]; otherwise, <c>false</c>.
		/// </value>
		public bool DeviceChanged { get; set; }
		/// <summary>
		/// Gets or sets the new device for the scenario.
		/// </summary>
		/// <value>
		/// The new device.
		/// </value>
		public string NewDevice { get; set; }

		/// <summary>
		/// Gets or sets the virtual meta data identifier. This is part of the PK in MetadataResourceUsage
		/// </summary>
		/// <value>
		/// The virtual meta data identifier.
		/// </value>
		public Guid VirtualResourceMetadataId { get; set; }

		public BulkDeviceEnt()
		{
            Active = false;
			CurrentDevice = string.Empty;
			DeviceChanged = false;
			NewDevice = string.Empty;
            VirtualResourceMetadataId = Guid.Empty;
		}
	}
}
