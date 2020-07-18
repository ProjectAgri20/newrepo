using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// An <see cref="AssetHost" /> that represents any BadgeBox
    /// </summary>
    public class BadgeBoxHost : AssetHost
    {
        private readonly BadgeBoxDetail _badgeBox;

        /// <summary>
        /// Badge Box Host
        /// </summary>
        /// <param name="asset"></param>
        public BadgeBoxHost(AssetDetail asset)
            : base(asset, asset.AssetId, ElementType.BadgeBox, "BadgeBox")
        {
            _badgeBox = asset as BadgeBoxDetail;
        }

        /// <summary>
        /// revalidates
        /// </summary>
        /// <param name="loopState"></param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            if (MapElement.State == RuntimeState.Validated)
            {
                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                return;
            }
            Validate(loopState);
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="loopState"></param>
        public override void Validate(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Validating", RuntimeState.Validating);

            TraceFactory.Logger.Debug("Checking if {0}:{1} is online and ready.".FormatWith(_badgeBox.Description, _badgeBox.IPAddress));

            using (var ping = new Ping())
            {
                var response = ping.Send(_badgeBox.IPAddress);

                int retries = 0;
                while (response.Status != IPStatus.Success && retries < 4)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    response = ping.Send(_badgeBox.IPAddress, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
                    retries++;
                }
                if (response.Status != IPStatus.Success)
                {
                    TraceFactory.Logger.Debug("Ping Unsuccessful: {0}:{1}".FormatWith(_badgeBox.Description, response.Status));
                    MapElement.UpdateStatus("Badge Box on {0} not be contacted. You may want to verify the connection before proceeding.".FormatWith(_badgeBox.Description), RuntimeState.Warning);
                }
                else
                {
                    TraceFactory.Logger.Debug("Ping successful: {0}".FormatWith(_badgeBox.AssetId));
                    MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                }

            }

        }
    }


}
