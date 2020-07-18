using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{

    /// <summary>
    /// Generic Map element control, which displays the status for virtual resource, session, etc.
    /// </summary>

    public partial class GenericMapElementControl : SessionStatusControlBase
    {
        private const string TIME_STAMP_FORMAT = "{0:MM/dd/yy HH:mm:ss} {1}";
        private const string MESSAGE_LOADING_HISTORY = "Loading history from database...";
        private const string MESSAGE_LOADED_HISTORY = "History loaded from database:";
        private const int MESSAGE_DISPLAY_MAX = 5000;               // max number of messages to display
        private const int MESSAGE_PREVIOUS_MAX = 2000;               // max number of previous messages to retrieve

        string _sessionId;
        SessionMapElement _element = null;
        ControlSessionExecution _sessionExecutionControl = null;
        //System.Timers.Timer _bufferTimer = new System.Timers.Timer();

        /// <summary>
        /// constructor
        /// </summary>
        public GenericMapElementControl()
        {
            InitializeComponent();  
        }        


        /// <summary>
        /// Initializes this instance with the specified <see cref="SessionMapElement" />.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sessionControl">The session execution control used to start/stop etc.</param>        
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        public override void Initialize(SessionMapElement element, ControlSessionExecution sessionControl)
        {
            try
            {
                if (element == null || sessionControl == null)
                {
                    throw new ArgumentNullException("Element and SessionControl arguments must not be null");
                }

                _element = element;
                _sessionId = element.SessionId;

                elementInfoCompositeControl.Initialize(element, sessionControl);

                _sessionExecutionControl = sessionControl;
                _sessionExecutionControl.RefreshRequested += _sessionExecutionControl_RefreshRequested;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Initialize", ex);
            }
        }

        protected virtual void _sessionExecutionControl_RefreshRequested(object sender, SessionControlRefreshEventArgs e)
        {
            elementInfoCompositeControl.Refresh(e);
        }

        private void UpdateListBox(ListBox listbox, string message)
        {
            UpdateListBox(listbox, DateTime.Now, message);
        }

        private void UpdateListBox(ListBox listbox, DateTime timestamp, string message, bool appendToEnd = false)
        {
            listbox.InvokeIfRequired(c =>
                {
                    var oldCount = listbox.Items.Count;
                    var newCount = oldCount + 1;
                    var oldSelectedIndex = listbox.SelectedIndex;
                    var oldTopIndex = listbox.TopIndex;

                    var stampedMessage = TIME_STAMP_FORMAT.FormatWith(timestamp, message);
                    if (appendToEnd)
                    {
                        listbox.Items.Add(stampedMessage);
                        if (newCount > MESSAGE_DISPLAY_MAX && newCount > 1)
                        {
                            listbox.Items.RemoveAt(0);
                        }
                    }
                    else
                    {
                        listbox.Items.Insert(0, stampedMessage);
                        if (newCount > MESSAGE_DISPLAY_MAX)
                        {
                            listbox.Items.RemoveAt(newCount - 1);
                        }
                    }

                    // keep the last selected item displayed
                    if (oldSelectedIndex >= 0)
                    {
                        try
                        {
                            var newTopIndex = oldTopIndex;
                            if (appendToEnd)
                            {
                                listbox.SelectedIndex = oldSelectedIndex;

                            }
                            else
                            {
                                listbox.SelectedIndex = oldSelectedIndex + 1;
                                newTopIndex = oldTopIndex + 1;
                            }
                        }
                        catch
                        {
                            // do nothing if we had a problem trying to preserve displayed position
                        }
                    }
                });
        }

        private void lstBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e != null && e.Index > -1)
                {
                    //
                    // Draw the background of the ListBox control for each item.
                    // Create a new Brush and initialize to a Black colored brush
                    // by default.
                    //
                    e.DrawBackground();
                    Brush myBrush = Brushes.Black;

                    //
                    // Determine the color of the brush to draw each item based on 
                    // the index of the item to draw.
                    //
                    var value = ((ListBox)sender).Items[e.Index].ToString();
                    if (value.Contains("[E]"))
                    {
                        myBrush = Brushes.Red;
                    }
                    else if (value.Contains("[W]"))
                    {
                        myBrush = Brushes.OrangeRed;
                    }
                    else if (value.Contains("[N]"))
                    {
                        myBrush = Brushes.Blue;
                    }
                    //
                    // Draw the current item text based on the current 
                    // Font and the custom brush settings.
                    //
                    e.Graphics.DrawString(value, e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                    //
                    // If the ListBox has focus, draw a focus rectangle 
                    // around the selected item.
                    //
                    e.DrawFocusRectangle();
                }
            }
            catch { }
        }

        private void SetButtonImage(ToolStripButton button)
        {
            if (button.Checked)
            {
                button.Image = new Bitmap(Properties.Resources.CheckboxOn);
            }
            else
            {
                button.Image = new Bitmap(Properties.Resources.CheckboxOff);
            }
        }
    }
}
