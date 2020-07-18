using System;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Plugin;
using HP.SPS.SES.Helper;

namespace HP.ScalableTest.Plugin.HpRoam
{
    /// <summary>
    /// Manages printing activites from an Android phone to the Roam cloud.
    /// </summary>
    public class RoamAndroidPrintManager : AndroidAppManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamAndroidPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData"></param>
        /// <param name="activityData"></param>
        public RoamAndroidPrintManager(PluginExecutionData pluginExecutionData, HpRoamActivityData activityData)
            :base(pluginExecutionData, activityData)
        {
        }

        /// <summary>
        /// Presses the icon for adding a document.
        /// </summary>
        public void PressAddDocumentIcon()
        {
            if (_androidHelper.WaitForAvailableResourceId("com.hp.roam:id/action_plus_file", TimeSpan.FromSeconds(30)))
            {
                _controller.Click(new UiSelector().ResourceId("com.hp.roam:id/action_plus_file"));
            }
            else
            {
                throw new DeviceWorkflowException("The HP Roam Add File icon was not found.");
            }
        }

        /// <summary>
        /// Selects the document specified by the plugin activity data.
        /// </summary>
        public void SelectDocument()
        {
            if (_androidHelper.WaitForAvailableText(_activityData.PhoneDocument, TimeSpan.FromSeconds(30)))
            {
                _controller.Click(new UiSelector().TextContains(_activityData.PhoneDocument));
            }
        }

        /// <summary>
        /// Presses the button to kick off the upload operation to the Roam cloud.
        /// </summary>
        public void PressUploadButton()
        {
            if (_androidHelper.WaitForAvailableResourceId("com.hp.roam:id/upload_button", TimeSpan.FromSeconds(30)))
            {
                _controller.Click(new UiSelector().ResourceId("com.hp.roam:id/upload_button"));
                if (_androidHelper.WaitForAvailableResourceId("com.hp.roam:id/preparing_job_details", TimeSpan.FromSeconds(10)))
                {
                    _androidHelper.WaitForNotDisplayedResourceId("com.hp.roam:id/preparing_job_details", TimeSpan.FromSeconds(30));
                }
            }
            else
            {
                throw new DeviceWorkflowException("The HP Roam Upload button was not found.");
            }
        }

        /// <summary>
        /// Blocks execution for up to 2 minutes waiting for document upload to complete.
        /// </summary>
        public void WaitForUploadComplete()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(120);
            if (!_androidHelper.WaitForNotDisplayedResourceId("com.android.systemui:id/fake_shadow", timeout))
            {
                throw new DeviceWorkflowException($"The document did not print to cloud within {timeout.TotalMinutes} minutes.");
            }
        }

    }
}
