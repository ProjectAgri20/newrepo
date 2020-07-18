using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls;

namespace HP.ScalableTest.LabConsole
{
    public static class StfVisualThemeManager
    {
        /// <summary>
        /// Sets the theme.
        /// </summary>
        public static void SetTheme()
        {
            var themeName = "ScalableTestFramework";
            var resourcePath = "HP.ScalableTest.LabConsole.Themes.{0}.tssp".FormatWith(themeName);
            try
            {
                ThemeResolutionService.LoadPackageResource(resourcePath);
                ThemeResolutionService.ApplicationThemeName = themeName;
            }
            catch(Exception ex)
            {
                TraceFactory.Logger.Error("Unable to set theme {0} from {1}".FormatWith(themeName, resourcePath), ex);
            }
        }
    }
}
