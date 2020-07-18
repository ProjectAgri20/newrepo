using HP.ScalableTest.Framework.Plugin;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.UserIntervention
{
    /// <summary>
    /// Excecution Controller Module for the Plugin
    /// </summary>
    public class UserInterventionExecutionEngine : IPluginExecutionEngine
    {
        /// <summary>
        /// Executes the Activity given for the plugin
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            UserInterventionActivityData pluginData = executionData.GetMetadata<UserInterventionActivityData>();
            DialogResult result = MessageBox.Show(pluginData.AlertMessage, "STF", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.No:
                    DialogResult showFaultEventHandler = MessageBox.Show("Do you want to bring up Fault Event Handler", "Fault Event Handler Prompt", MessageBoxButtons.YesNo);
                    return new PluginExecutionResult(PluginResult.Failed, pluginData.AlertMessage);
                case DialogResult.Yes:
                    return new PluginExecutionResult(PluginResult.Passed, pluginData.AlertMessage);
                default:
                    return new PluginExecutionResult(PluginResult.Skipped, pluginData.AlertMessage);
            }
        }
    }
}
