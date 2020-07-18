using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Class that supports a combobox with multiple columns in the dropdown
    /// </summary>
    public class PrintDriverComboBox : MultipleColumnComboBox
    {
        private Dictionary<string, int> _padding = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverComboBox"/> class.
        /// </summary>
        public PrintDriverComboBox(Point location)
            : base("DriverDropDown", location, new Size(450, 23), new Collection<int>() { 250, 30, 95, 70 })
        {
        }

        /// <summary>
        /// Gets the selected data for the combo box.
        /// </summary>
        public Dictionary<string, string> SelectedData
        {
            get
            {
                Dictionary<string, string> data = new Dictionary<string, string>();

                MTGCComboBoxItem selectedItem = SelectedItem as MTGCComboBoxItem;
                if (SelectedItem != null)
                {
                    data.Add("Name", selectedItem.Col1.Trim());
                    data.Add("Architecture", selectedItem.Col2.Trim());
                    data.Add("Version", selectedItem.Col3.Trim());
                    data.Add("InfFile", selectedItem.Col4.Trim());
                }

                return data;
            }
        }


        /// <summary>
        /// Adds the drivers to this control.
        /// </summary>
        /// <param name="drivers">The drivers.</param>
        public void AddDrivers(PrintDeviceDriverCollection drivers)
        {
            if (drivers == null)
            {
                throw new ArgumentNullException("drivers");
            }

            Items.Clear();

            foreach (PrintDeviceDriver driver in drivers)
            {
                // Sneak an extra space on each name that is the same to create uniqueness.
                if (!_padding.ContainsKey(driver.Name))
                {
                    _padding.Add(driver.Name, 0);
                }
                string name = driver.Name.PadRight(driver.Name.Length + _padding[driver.Name]++, ' ');

                AddItem
                    (
                        name,
                        EnumUtil.GetDescription(driver.Architecture),
                        driver.Version.ToString(),
                        Path.GetFileName(driver.InfPath)
                    );
            }
        }

        private void AddItem(params string[] args)
        {
            MTGCComboBoxItem item = null;

            switch (args.Length)
            {
                case 1:
                    item = new MTGCComboBoxItem(args[0], string.Empty, string.Empty, string.Empty);
                    break;
                case 2:
                    item = new MTGCComboBoxItem(args[0], args[1], string.Empty, string.Empty);
                    break;
                case 3:
                    item = new MTGCComboBoxItem(args[0], args[1], args[2], string.Empty);
                    break;
                case 4:
                    item = new MTGCComboBoxItem(args[0], args[1], args[2], args[3]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("args");
            }

            Items.Add(item);
        }
    }
}
