using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpEasyStart.UIMaps
{
    public abstract class UIMap
    {
        public string ScreenName;
        public abstract PluginExecutionResult PerformAction();
    }
}

