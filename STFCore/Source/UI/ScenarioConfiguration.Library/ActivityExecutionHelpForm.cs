using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class ActivityExecutionHelpForm : Form
    {
        Edge _edge = Edge.Left;

        public enum Edge
        {
            Left,
            Right,
        }

        public ActivityExecutionHelpForm(Edge edge)
        {
            _edge = edge;

            InitializeComponent();
            help_Label.MouseLeave += help_Label_MouseLeave;
        }

        public string HelpString { get; set; }

        void help_Label_MouseLeave(object sender, EventArgs e)
        {
            Close();
        }

        private void ActivityExecutionHelpForm_Load(object sender, EventArgs e)
        {
            help_Label.Text = HelpString;

            if (_edge == Edge.Left)
            {
                Cursor.Position = new Point(Location.X + 1, Location.Y + 1);
            }
            else
            {
                Cursor.Position = new Point(Location.X + Width - 1, Location.Y + 1);
            }

            help_Label.Focus();
        }
    }
}
