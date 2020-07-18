using HP.DeviceAutomation;

namespace HP.ScalableTest.Core
{
    /// <summary>
    /// log4net adapter for the Device Automation Toolkit.
    /// </summary>
    public static class DatLoggerAdapter
    {
        private static SystemTraceLogger _logger;

        /// <summary>
        /// Attaches to the DAT <see cref="Logger" /> class.
        /// </summary>
        public static void Attach()
        {
            if (_logger == null)
            {
                _logger = new SystemTraceLogger(typeof(Logger), "HP.DeviceAutomation");

                Logger.OnTrace += (s, e) => _logger.LogTrace(e.Message);
                Logger.OnDebug += (s, e) => _logger.LogDebug(e.Message);
                Logger.OnWarn += (s, e) => _logger.LogWarn(e.Message);
                Logger.OnError += (s, e) => _logger.LogError(e.Message);
            }
        }
    }
}
