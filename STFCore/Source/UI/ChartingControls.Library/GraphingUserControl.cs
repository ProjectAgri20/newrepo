using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Control for configuring and displaying run-time graphs.
    /// </summary>
    public partial class GraphingUserControl : UserControl
    {
        private string _sessionId = string.Empty;
        private string _scenarioName = string.Empty;
        private List<IGraph> _activityGraphs = new List<IGraph>();

        /// <summary>
        /// Occurs when a graph posts a status update.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphingUserControl"/> class.
        /// </summary>
        public GraphingUserControl()
        {
            InitializeComponent();
        }

        private void GraphingUserControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RefreshScenarioList();

                _activityGraphs.Add(new ActivityTypeTimeGraph());
                _activityGraphs.Add(new ActivityInstanceTimeGraph());
                _activityGraphs.Add(new ActivityErrorsTimeGraph());
                _activityGraphs.Add(new ActivityTypeTotalsGraph());
                _activityGraphs.Add(new ActivityInstanceTotalsGraph());
                _activityGraphs.Add(new ActivityErrorTotalsGraph());

                foreach (IGraph graph in _activityGraphs)
                {
                    // Hook into the graphing event so updates can be sent when the
                    // graph is executing from the database or plotting the graph, etc.
                    graph.StatusUpdate += new EventHandler<StatusChangedEventArgs>(OnStatusUpdate);

                    // Create a tab page for each graph
                    Control graphControl = graph as Control;
                    if (graphControl != null)
                    {
                        TabPage tabPage = new TabPage();
                        try
                        {
                            graphControl.Dock = DockStyle.Fill;
                            tabPage.Controls.Add(graphControl);
                            tabPage.Text = graph.GraphName;
                            graphs_TabControl.TabPages.Add(tabPage);
                        }
                        catch
                        {
                            tabPage.Dispose();
                            throw;
                        }
                    }
                }

                // Force all tab pages to load
                foreach (TabPage tab in graphs_TabControl.TabPages)
                {
                    tab.Show();
                }
            }
        }

        /// <summary>
        /// Handles the graphing status update event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="HP.ScalableTest.StatusChangedEventArgs"/> instance containing the event data.</param>
        private void OnStatusUpdate(object sender, StatusChangedEventArgs e)
        {
            if (StatusUpdate != null)
            {
                StatusUpdate(this, e);
            }
        }

        /// <summary>
        /// Refreshes the scenario list.
        /// </summary>
        public void RefreshScenarioList()
        {
            List<SessionDropDownItem> items = new List<SessionDropDownItem>();

            using (DataLogContext context = DbConnect.DataLogContext())
            {
                var sessions = context.Sessions
                                      .OrderByDescending(n => n.StartDateTime)
                                      .Select(n => new
                                      {
                                          n.SessionId,
                                          n.SessionName,
                                          n.Owner,
                                          n.StartDateTime
                                      })
                                      .AsEnumerable()
                                      .Select(n => new SessionDropDownItem
                                      {
                                          SessionId = n.SessionId,
                                          SessionName = n.SessionName ?? "Undefined",
                                          Owner = n.Owner ?? "Undefined",
                                          StartDate = n.StartDateTime?.LocalDateTime ?? DateTime.MinValue
                                      }).ToList();

                // Only include sessions that have activities
                var sessionCounts = context.SessionActivityCounts();
                items.AddRange(sessions.Where(n => sessionCounts.ContainsKey(n.SessionId)));
            }

            sessionId_ComboBox.DataSource = null;
            sessionId_ComboBox.DataSource = items;

            if (items.Count() > 0)
            {
                if (!string.IsNullOrEmpty(_sessionId))
                {
                    SessionDropDownItem itemToSelect = items.FirstOrDefault(i => i.SessionId == _sessionId);
                    if (itemToSelect != null)
                    {
                        sessionId_ComboBox.SelectedItem = itemToSelect;
                        SetSelectedSession();
                    }
                }
                else
                {
                    // Default to the most recent session run by the logged in user
                    SessionDropDownItem mostRecentSession = items.FirstOrDefault(i => i.Owner == UserManager.CurrentUserName);
                    sessionId_ComboBox.SelectedItem = mostRecentSession ?? sessionId_ComboBox.Items[0];
                    SetSelectedSession();
                }
            }
        }

        private void SetSelectedSession()
        {
            SessionDropDownItem item = sessionId_ComboBox.SelectedItem as SessionDropDownItem;
            if (item != null)
            {
                _sessionId = item.SessionId;
                _scenarioName = item.SessionName;
                RefreshGraphs();
            }
        }

        private void RefreshGraphs(bool applyFilters = false)
        {
            if (ValidateChildren())
            {
                Cursor = Cursors.WaitCursor;

                foreach (IGraph graph in _activityGraphs)
                {
                    if (graph != null)
                    {
                        graph.RefreshGraph(_sessionId, _scenarioName, applyFilters);
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void sessionId_ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetSelectedSession();
        }

        private void refresh_Button_Click(object sender, EventArgs e)
        {
            RefreshGraphs();
        }

        private void options_Button_Click(object sender, EventArgs e)
        {
            using (DisplayOptionsForm form = new DisplayOptionsForm())
            {
                form.ShowDialog(this);
            }

            RefreshGraphs(true);
        }

        private void saveImage_Button_Click(object sender, EventArgs e)
        {
            IGraph graph = graphs_TabControl.SelectedTab.Controls[0] as IGraph;
            if (graph != null)
            {
                graph.SaveImageToClipboard();
            }
        }

        /// <summary>
        /// Helper class for displaying session info in the drop down
        /// </summary>
        private class SessionDropDownItem
        {
            public string SessionId { get; set; }
            public string SessionName { get; set; }
            public string Owner { get; set; }
            public DateTime StartDate { get; set; }

            public override string ToString()
            {
                return (StartDate.ToString("MM/dd/yy HH:mm tt", CultureInfo.InvariantCulture) + " - " + SessionId + " - " + Owner + " - " + SessionName);
            }
        }
    }
}
