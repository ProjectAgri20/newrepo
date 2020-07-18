using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DSSInstall.UIMaps
{
    public abstract class UIMap
    {
        public string ScreenName;

        public int ScreenIndex;
        public abstract PluginExecutionResult PerformAction(int cancelScreen);
    }
}