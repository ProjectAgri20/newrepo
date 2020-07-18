using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Options for styling a windows form.
    /// </summary>
    public enum FormStyle
    {
        /// <summary>
        /// Standard for secondary windows.  Displays in taskbar and can be resized.
        /// </summary>
        Standard,

        /// <summary>
        /// Popup dialog that can be resized.  Does not display in taskbar.
        /// </summary>
        SizeableDialog,

        /// <summary>
        /// Popup dialog (with help icon) that can be resized.  Does not display in taskbar.
        /// </summary>
        SizeableDialogWithHelp,

        /// <summary>
        /// Popup dialog that cannot be resized.  Does not display in taskbar.
        /// </summary>
        FixedDialog,

        /// <summary>
        /// Popup dialog (with help icon) that cannot be resized.  Does not display in taskbar.
        /// </summary>
        FixedDialogWithHelp,
    }

    /// <summary>
    /// Options for styling a grid view.
    /// </summary>
    public enum GridViewStyle
    {
        /// <summary>
        /// User has full editing control, including adding/removing rows.
        /// </summary>
        FullEdit,

        /// <summary>
        /// Grid is primarily for display.  Cells are editable by default.
        /// </summary>
        Display,

        /// <summary>
        /// Grid is read-only.  User cannot modify anything.
        /// </summary>
        ReadOnly,
    }

    /// <summary>
    /// Provides methods for styling UI elements such as forms, grid views, etc.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Styler")]
    public static class UserInterfaceStyler
    {
        /// <summary>
        /// Initializes the STF UI theme.
        /// This method should be called in the static constructor of the main form of the calling assembly.
        /// </summary>
        public static void Initialize()
        {
#if SDK
            RadTypeResolver.Instance.TypeResolverAssemblyName = "STF.Framework";
#endif
            string themeName = "ScalableTestFramework";
            string resourcePath = $"HP.ScalableTest.Framework.Properties.{themeName}.tssp";

            ThemeResolutionService.LoadPackageResource(resourcePath);
            ThemeResolutionService.ApplicationThemeName = themeName;
        }

        /// <summary>
        /// Configures the specified form for the specified display style.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="style">The style.</param>
        public static void Configure(Form form, FormStyle style)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            // Universal properties
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowIcon = false;
            form.StartPosition = FormStartPosition.CenterParent;

            // Style-specific restrictions
            switch (style)
            {
                case FormStyle.FixedDialog:
                case FormStyle.FixedDialogWithHelp:
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    goto case FormStyle.SizeableDialog;
                case FormStyle.SizeableDialog:
                case FormStyle.SizeableDialogWithHelp:
                    form.ShowInTaskbar = false;
                    break;

                case FormStyle.Standard:
                    break;
            }

            form.HelpButton = (style == FormStyle.FixedDialogWithHelp ||
                               style == FormStyle.SizeableDialogWithHelp);
        }

        /// <summary>
        /// Configures the specified grid view for the specified display style.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="style">The style.</param>
#if !SDK
        public static void Configure(RadGridView gridView, GridViewStyle style)
#else
        internal static void Configure(RadGridView gridView, GridViewStyle style)
#endif
        {
            if (gridView == null)
            {
                throw new ArgumentNullException(nameof(gridView));
            }

            // Universal properties
            gridView.AddNewRowPosition = SystemRowPosition.Bottom;
            gridView.AllowCellContextMenu = false;
            gridView.AllowColumnResize = true;
            gridView.AllowRowResize = false;
            gridView.AutoGenerateColumns = false;
            gridView.EnableAlternatingRowColor = true;
            gridView.ShowGroupPanel = false;
            gridView.ShowRowHeaderColumn = false;
            gridView.TableElement.AlternatingRowColor = Color.FromArgb(240, 240, 240);

            // Style-specific restrictions
            switch (style)
            {
                case GridViewStyle.ReadOnly:
                    gridView.ReadOnly = true;
                    goto case GridViewStyle.Display;

                case GridViewStyle.Display:
                    gridView.AllowAddNewRow = false;
                    gridView.AllowDeleteRow = false;
                    break;
            }

            // Show/hide group panel depending on grouping status
            gridView.GroupByChanged += (sender, e) =>
            {
                RadGridView grid = sender as RadGridView;
                grid.ShowGroupPanel = grid.GroupDescriptors.Any();
            };

            // Hide conditional formatting
            gridView.ContextMenuOpening += (sender, e) =>
            {
                RadItem formattingItem = e.ContextMenu.Items.FirstOrDefault(n => n.Text == "Conditional Formatting");
                if (formattingItem != null)
                {
                    e.ContextMenu.Items.Remove(formattingItem);
                    e.ContextMenu.Items.First(n => n is RadMenuSeparatorItem).Visibility = ElementVisibility.Collapsed;
                }
            };
        }
    }
}
