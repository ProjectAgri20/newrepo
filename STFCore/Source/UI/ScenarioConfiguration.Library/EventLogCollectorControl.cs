using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring an Event Log Collector virtual resource.
    /// </summary>
    public partial class EventLogCollectorControl : ScenarioConfigurationControlBase
    {
        private EventLogCollector _eventLogCollector = null;
        private const string Any = "<Any>";

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogCollectorControl"/> class.
        /// </summary>
        public EventLogCollectorControl()
        {
            InitializeComponent();
        }

        public override string EditFormTitle
        {
            get
            {
                return "EventLog Collector Configuration";
            }
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _eventLogCollector; }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            Initialize(new EventLogCollector());
        }

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        ///   </exception>
        public override void Initialize(object entity)
        {
            _eventLogCollector = entity as EventLogCollector;
            if (_eventLogCollector == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(EventLogCollector));
            }

            ServerInfo server = ConfigurationServices.AssetInventory.GetServers().FirstOrDefault(n => n.HostName == _eventLogCollector.HostName);
            if (server != null)
            {
                serverComboBox.Initialize(server, "EventLog");
            }
            else
            {
                serverComboBox.Initialize("EventLog");
            }

            platform_ComboBox.SetPlatform(_eventLogCollector.Platform, VirtualResourceType.EventLogCollector);

            // Set up data bindings
            name_TextBox.DataBindings.Add("Text", _eventLogCollector, "Name");
            description_TextBox.DataBindings.Add("Text", _eventLogCollector, "Description");
            platform_ComboBox.DataBindings.Add("SelectedValue", _eventLogCollector, "Platform");

            Binding intervalBinding = new Binding("Text", _eventLogCollector, "PollingInterval");
            intervalBinding.Format += new ConvertEventHandler(IntervalBinding_Format);
            intervalBinding.Parse += new ConvertEventHandler(IntervalBinding_Parse);
            interval_TextBox.DataBindings.Add(intervalBinding);

            serverComboBox_SelectionChanged(serverComboBox, EventArgs.Empty);
            SelectedComponents = LegacySerializer.DeserializeXml<List<string>>(_eventLogCollector.ComponentsData);
            SelectedEntryTypes = LegacySerializer.DeserializeXml<List<string>>(_eventLogCollector.EntryTypesData);

            if (platform_ComboBox.SelectedIndex == -1) //Default to first item if platform isn't set
            {
                platform_ComboBox.SelectedIndex = 0;
            }

            serverComboBox.SelectionChanged += serverComboBox_SelectionChanged;
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public override void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_Label.Focus();

            // Update the host name
            _eventLogCollector.HostName = serverComboBox.SelectedServer.HostName;

            // Only assign these properties if the selected values have changed.
            // This will avoid unnecessary "unsaved changes" prompts.
            List<string> originalComponents = LegacySerializer.DeserializeXml<List<string>>(_eventLogCollector.ComponentsData);
            if (!AreEqual(originalComponents, SelectedComponents))
            {
                _eventLogCollector.ComponentsData = LegacySerializer.SerializeXml(SelectedComponents).ToString();
            }

            List<string> originalEntryTypes = LegacySerializer.DeserializeXml<List<string>>(_eventLogCollector.EntryTypesData);
            if (!AreEqual(originalEntryTypes, SelectedEntryTypes))
            {
                _eventLogCollector.EntryTypesData = LegacySerializer.SerializeXml(SelectedEntryTypes).ToString();
            }
        }

        private List<string> SelectedComponents
        {
            get { return GetSelected(components_ListBox); }
            set { SetSelected(components_ListBox, value); }
        }

        private List<string> SelectedEntryTypes
        {
            get { return GetSelected(entryTypes_ListBox); }
            set { SetSelected(entryTypes_ListBox, value); }
        }

        private static bool AreEqual(List<string> original, List<string> current)
        {
            return (original.Count == current.Count && original.Union(current).Count() == original.Count);
        }

        private void IntervalBinding_Parse(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.Parse(((string)e.Value)).TotalMinutes;
        }

        private void IntervalBinding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.ToTimeSpanString((int)e.Value);
        }

        private void serverComboBox_SelectionChanged(object sender, EventArgs e)
        {
            components_ListBox.Items.Clear();
            if (serverComboBox.HasSelection)
            {
                LoadComponentsListBox(serverComboBox.SelectedServer.HostName);
            }
        }

        private void LoadComponentsListBox(string selectedHost)
        {
            ResourceWindowsCategory component = null;
            components_ListBox.Items.Add(Any);
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                component = ResourceWindowsCategory.Select(context, "EventLog", selectedHost).FirstOrDefault();
            }

            if (component != null)
            {
                foreach (ResourceWindowsCategory child in component.Children)
                {
                    components_ListBox.Items.Add(child.Name);
                }
            }
        }

        private static List<string> GetSelected(ListBox listBox)
        {
            List<string> selected = new List<string>();
            foreach (string item in listBox.SelectedItems)
            {
                if (item == Any)
                {
                    //If <Any> is selected, make it the only selection
                    if (selected.Count > 0)
                    {
                        selected.Clear();
                    }
                    selected.Add(item);
                    break;
                }
                else
                {
                    selected.Add(item);
                }
            }
            return selected;
        }

        private static void SetSelected(ListBox listBox, List<string> selected)
        {
            foreach (string selectedItem in selected)
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    if (((string)listBox.Items[i]) == selectedItem)
                    {
                        listBox.SetSelected(i, true);
                        break;
                    }
                }
            }
        }

        private void entryTypes_ListBox_Validating(object sender, CancelEventArgs e)
        {
            ListBox_Validating("Event Log Entry Type", (ListBox)sender, e);
        }

        private void components_ListBox_Validating(object sender, CancelEventArgs e)
        {
            ListBox_Validating("Components/Services", (ListBox)sender, e);
        }

        private void ListBox_Validating(string label, ListBox listBox, CancelEventArgs e)
        {
            string selectedValue = listBox.SelectedItems.Count == 0 ? string.Empty : (string)listBox.SelectedItems[0];
            HasSelected(selectedValue, label, listBox, e);
        }

        private void interval_TextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateRegex(interval_TextBox.Text, "Interval", "\\d+:\\d+", "hh:mm and be > 0", interval_TextBox, e);
        }
    }
}
