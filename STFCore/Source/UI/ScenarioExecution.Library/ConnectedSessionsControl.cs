using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    public partial class ConnectedSessionsControl : UserControl
    {
        private SessionExecutionManager _sessionManager;

        public SessionExecutionManager SessionManager
        {
            get { return _sessionManager;  }
        }

        public ConnectedSessionsControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            _sessionManager = new SessionExecutionManager(this.sessionExecution_TreeView, this.splitContainerSessionExecution.Panel2, this.sessionControl, this.start_ToolStripButton);
            SessionManager.PropertyChanged += (s, e) => this.InvokeIfRequired(c => sessionManager_PropertyChanged(s, e));
            SetButtonStates();
        }

        void sessionManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetButtonStates();
        }

        private void SetButtonStates()
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                start_ToolStripButton.Enabled = _sessionManager.AbleToStartNewSession;
            }
            else
            {
                start_ToolStripButton.Visible = false;
                connectDispatcher_ToolStripButton.Visible = false;
            }
        }

        private void connectDispatcher_ToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                STFDispatcherManager.ConnectToDispatcher();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void start_ToolStripButton_Click(object sender, EventArgs e)
        {
            SessionManager.StartSession();
        }
    }
}
