using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Base class for the gathering of triage data
    /// </summary>
    public abstract class TriageBase
    {
        PluginExecutionData _pluginExecutionData;
        TriageDataLog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriageBase"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        protected TriageBase(PluginExecutionData pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _logger = new TriageDataLog(_pluginExecutionData);
        }

        /// <summary>
        /// Gets or sets the control panel image.
        /// </summary>
        /// <value>
        /// The control panel image.
        /// </value>
        public Image ControlPanelImage { get; set; }

        /// <summary>
        /// Gets or sets the control panel control ids.
        /// </summary>
        /// <value>
        /// The control panel control ids.
        /// </value>
        public string ControlPanelControlIds { get; set; }

        /// <summary>
        /// Gets or sets the Android UI Dump Data.
        /// </summary>
        /// <value>
        /// The Android UI Dump Data.
        /// </value>
        public string UIDumpData { get; set; }

        /// <summary>
        /// Gets or sets the device warnings.
        /// </summary>
        /// <value>
        /// The device warnings.
        /// </value>
        public string DeviceWarnings { get; set; }

        /// <summary>
        /// Virtual method for retrieving the control panel image.
        /// </summary>
        public abstract void GetControlPanelImage();

        /// <summary>
        /// Virtual method for retrieving the current control ids.
        /// </summary>
        public abstract void GetCurrentControlIds();

        /// <summary>
        /// Gets the device warnings - only for Omni devices.
        /// </summary>
        /// <returns>string</returns>
        public virtual string GetDeviceWarnings()
        {
            return string.Empty;
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        public virtual void Submit()
        {
            ExecutionServices.SystemTrace.LogDebug($"Logging Triage data for session {_pluginExecutionData.SessionId} and activity {_pluginExecutionData.ActivityExecutionId}.");
            using (MemoryStream msJpeg = new MemoryStream())
            {
                ControlPanelImage.Save(msJpeg, ImageFormat.Jpeg);
                using (MemoryStream msBmp = new MemoryStream())
                {
                    ControlPanelImage.Save(msBmp, ImageFormat.Bmp);
                    Bitmap bmOrig = new Bitmap(msBmp);
                    Bitmap bmSmall = CreateThumnbail(bmOrig);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmSmall.Save(ms, ImageFormat.Bmp);
                        _logger.Thumbnail = ms.ToArray();
                    }
                }
                _logger.ControlPanelImage = msJpeg.ToArray();
            }
            _logger.ControlIds = ControlPanelControlIds ?? string.Empty;
            _logger.UIDumpData = UIDumpData ?? string.Empty;
            ExecutionServices.DataLogger.Submit(_logger);
        }
        /// <summary>
        /// Creates the thumbnail for the collected image.
        /// </summary>
        /// <param name="origBitmap">The original bitmap.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Graphics methods can throw non-derived Exceptions.")]
        private static Bitmap CreateThumnbail(Bitmap origBitmap)
        {
            Bitmap bmpOut = null;

            try
            {

                int width = (int)(origBitmap.Width * .15);
                int height = (int)(origBitmap.Height * .15);

                bmpOut = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, width, height);
                g.DrawImage(origBitmap, 0, 0, width, height);

                origBitmap.Dispose();
            }
            catch
            {
                bmpOut = null;
            }
            return bmpOut;
        }
        /// <summary>
        /// Processes the control ids into a ',' separated string for storing into the DB.
        /// </summary>
        /// <param name="ctlrIds">The CTLR ids.</param>
        protected void ProcessControlIds(IEnumerable<string> ctlrIds)
        {
            ControlPanelControlIds = string.Join(",", ctlrIds.Select(x => x.Trim()));
        }

        /// <summary>
        /// Collects the triage data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void CollectTriageData()
        {
            try
            {
                _logger.TriageDateTime = DateTime.Now;
                GetControlPanelImage();
                GetCurrentControlIds();
                _logger.DeviceWarnings = GetDeviceWarnings();
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("STF Triage Collection Error: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Collects the triage data along with reason why it was needed.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public void CollectTriageData(string reason)
        {
            _logger.Reason = reason;
            ExecutionServices.SystemTrace.LogTrace(_logger.Reason);
            CollectTriageData();
        }
    }
}
