using HP.ScalableTest.Framework;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Attach Log from SES to STF log
    /// </summary>
    public static class JetAdvantageLinkLogAdapter
    {
        private static bool _attached = false;

        /// <summary>
        /// Attach the log if not attached
        /// </summary>
        public static void Attach()
        {
            if (!_attached)
            {
                HP.SPS.SES.Log.Logger.OnTrace += (s, e) => ExecutionServices.SystemTrace.LogTrace(e.Message);
                HP.SPS.SES.Log.Logger.OnDebug += (s, e) => ExecutionServices.SystemTrace.LogDebug(e.Message);
                HP.SPS.SES.Log.Logger.OnWarn += (s, e) => ExecutionServices.SystemTrace.LogWarn(e.Message);
                HP.SPS.SES.Log.Logger.OnError += (s, e) => ExecutionServices.SystemTrace.LogError(e.Message, e.Exception);

                _attached = true;
            }
        }

        /// <summary>
        /// Force set the attached flag (for testing)
        /// </summary>
        /// <param name="attached">attached</param>
        public static void SetAttachStatus(bool attached)
        {
            _attached = attached;
        }
    }
}
