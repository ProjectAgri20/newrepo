using System;
using System.Threading;
using TopCat.TestApi.GUIAutomation.Controls;
using TopCat.TestApi.GUIAutomation.Enums;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint.UIMaps
{
    public static class TopCatUiHelper
    {
        private static readonly TimeSpan HumanTimeSpan = TimeSpan.FromSeconds(2);

        public static void ComboBoxSetValue(ComboBox comboBox, string selectedValue, int timeout)
        {
            try
            {
                ListItem listItem = new ListItem(selectedValue + "ListItem", comboBox);
                listItem.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, "ListBoxItem");
                listItem.UIMap.SearchProperties.Add(UIASearchProperty.Name, selectedValue);
                comboBox.Expand(timeout);
                listItem.Select(timeout);
                comboBox.Collapse(timeout);

                Thread.Sleep(HumanTimeSpan);
            }
            catch
            {
                throw new ArgumentException(selectedValue + " not found ");
            }
        }

        public static void PerformHumanAction(this Button button, Action<Button> action)
        {
            action(button);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this Edit edit, Action<Edit> action)
        {
            action(edit);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this RadioButton radio, Action<RadioButton> action)
        {
            action(radio);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this TabItem tab, Action<TabItem> action)
        {
            action(tab);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this CheckBox check, Action<CheckBox> action)
        {
            action(check);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this ListItem listItem, Action<ListItem> action)
        {
            action(listItem);
            Thread.Sleep(HumanTimeSpan);
        }
    }
}
