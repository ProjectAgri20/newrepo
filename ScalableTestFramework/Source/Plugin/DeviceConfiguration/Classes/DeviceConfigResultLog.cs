using System;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{
    public class DeviceConfigResultLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be assinged.
        /// </summary>
        public override string TableName => "DeviceConfigResult";

        public DeviceConfigResultLog(PluginExecutionData executionData, string deviceId)
            : base(executionData)
        {
            DeviceId = deviceId;
            Result = null;
            FieldChanged = null;
            Value = null;
            ControlChanged = null;
        }

        /// <summary>
        /// Gets or sets the Device ID
        /// </summary>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Which control on the UI was modified.
        /// </summary>
        [DataLogProperty]
        public string ControlChanged { get; set; }
        /// <summary>
        /// Gets or sets the result of setting the value
        /// </summary>
        [DataLogProperty]
        public string Result { get; set; }
        /// <summary>
        /// Gets or sets the name of the field being set
        /// </summary>
        [DataLogProperty]
        public string FieldChanged { get; set; }
        /// <summary>
        /// Gets or sets the value we attempted to set the field to
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string Value { get; set; }

    }
}
