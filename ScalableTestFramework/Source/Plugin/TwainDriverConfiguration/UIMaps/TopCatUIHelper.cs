using System;
using System.Threading;
using TopCat.TestApi.GUIAutomation.Controls;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration.UIMaps
{
    public static class TopCatUiHelper
    {
        private static readonly TimeSpan HumanTimeSpan = TimeSpan.FromSeconds(3);

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

        public static void PerformHumanAction(this ListItem listItem, Action<ListItem> action)
        {
            action(listItem);
            Thread.Sleep(HumanTimeSpan);
        }
    }
}
