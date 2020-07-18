using HP.DeviceAutomation;
using HP.SPS.SES;
using System;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Class that represent JetAdvantageLink UI (Android UI)
    /// </summary>
    public class JetAdvantageLinkUI : IDisposable
    {
        /// <summary>
        /// Android Controller. See SES Library documentation for more information
        /// </summary>
        public SESLib Controller { get; }
        
        /// <summary>
        /// IP Address of android
        /// </summary>
        public string Address { get; }


        /// <summary>
        /// Create JetadvantageLinkUI object and connect to Android
        /// </summary>
        /// <param name="device"></param>
        /// <exception cref="DeviceCommunicationException">thrown when can not connect to Link UI</exception>
        public JetAdvantageLinkUI(IDevice device)
        {
            Address = device.Address;
            try
            {
                JetAdvantageLinkLogAdapter.Attach();
            }
            catch
            {
                // Consume exception here.
            }
            
            Controller = SESLib.Create(Address);
            
            if (Controller == null)
            {
                throw new DeviceCommunicationException($"Can not connect to the Link UI on device at {device.Address}.");
            }
            if(!Controller.Connect(true, true))
            {
                throw new DeviceCommunicationException($"Can not start SES Agent on device at {device.Address}");
            }
            

        }

        /// <summary>
        /// Dispose JetAdvantageLinkUI
        /// </summary>
        public void Dispose()
        {
            Controller.Disconnect();
        }
    }
}
