using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class SnmpExerciser
    {
        private readonly DirtyDeviceManager _owner;
        private readonly JediDevice _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpExerciser" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal SnmpExerciser(DirtyDeviceManager owner, JediDevice device)
        {
            _owner = owner;
            _device = device;
        }

        internal void Exercise(DirtyDeviceActivityData activityData)
        {
            for (int seed1 = 0; seed1 <= 2; seed1++)
            {
                for (int seed2 = 0; seed2 < 40; seed2++)
                {
                    string seed = $"{seed1}.{seed2}"; // HP devices only seem to respond to the following seeds but we will try them all: 1.0, 1.2, 1.3

                    try
                    {
                        _device.Snmp.Walk(seed);
                    }
                    catch (Exception x)
                    {
                        string message = $"Snmp walk failed with seed {seed}.";
                        _owner.OnUpdateStatus(this, message + "  ({x.ToString()})");
                        throw new SnmpException(message, x);
                    }
                }
            }
        }
    }
}
