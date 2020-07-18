using System;
using System.Threading;
using System.Windows.Automation;
using TopCat.TestApi.GUIAutomation.Controls;
using TopCat.TestApi.GUIAutomation.Enums;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ConnectivityPrint.UIMaps
{
    public static class TopCatUiHelper
    {
        private static readonly TimeSpan HumanTimeSpan = TimeSpan.FromSeconds(1);
        public static AutomationElementCollection GetChildren(Control parentControl, string childType)
        {
            
            var listBoxElement = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, parentControl.AutomationID(), PropertyConditionFlags.IgnoreCase));
            return listBoxElement.FindAll(System.Windows.Automation.TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, childType));
        }

        public static AutomationElement GetChildByName(Control parentControl, string childType, string name, bool matchExtactString = false)
        {
            
            var listBoxElement = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, parentControl.AutomationID(), PropertyConditionFlags.IgnoreCase));
            var listBoxChildren = listBoxElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, childType));

            for (int i = 0; i < listBoxChildren.Count; i++)
            {
                if (matchExtactString)
                {
                    if (listBoxChildren[i].Current.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return listBoxChildren[i];
                    }
                }
                else
                {
                    if (listBoxChildren[i].Current.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return listBoxChildren[i];
                    }
                }
            }
            return null;

        }

        public static void ComboBoxSetValue(ComboBox comboBox, string selectedValue, int timeout)
        {
            ListItem listItem = new ListItem(selectedValue + "ListItem", comboBox);
            listItem.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, "ListBoxItem");
            listItem.UIMap.SearchProperties.Add(UIASearchProperty.Name, selectedValue);
            comboBox.Expand(timeout);
            listItem.Select(timeout);
            comboBox.Collapse(timeout);

            Thread.Sleep(HumanTimeSpan);
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

        public static void PerformHumanAction(this DataItem data, Action<DataItem> action)
        {
            action(data);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this ComboBox combo, Action<ComboBox> action)
        {
            action(combo);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this Group group, Action<Group> action)
        {
            action(group);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this MenuItem menuItem, Action<MenuItem> action)
        {
            action(menuItem);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this TreeItem treeItem, Action<TreeItem> action)
        {
            action(treeItem);
            Thread.Sleep(HumanTimeSpan);
        }

        public static void PerformHumanAction(this Hyperlink link, Action<Hyperlink> action)
        {
            action(link);
            Thread.Sleep(HumanTimeSpan);
        }

    }
}
