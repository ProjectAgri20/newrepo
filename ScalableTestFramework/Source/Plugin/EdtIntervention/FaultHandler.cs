using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.EdtIntervention
{
    public partial class FaultHandler : Form
	{
        private int _curEvent;							
		private int _curType;
		private int _elapsedSecs;
		private bool _timerState;

	    readonly PluginExecutionData _executionData;

	    /// <summary>
	    /// Main calls this as the startup for the GUI or non-GUI forms 
	    /// </summary>
	    public FaultHandler(PluginExecutionData executionData = null)
		{
            _executionData = executionData;
			InitializeComponent();
			_curEvent =  _curType = -1;
			_elapsedSecs = 0;
			_timerState = false;
			cbFaultType.DataSource = FaultEventConstants.Events;
			cbOperProgress.DataSource = FaultEventConstants.Operations;
			cbDisposition.DataSource = FaultEventConstants.JobDispositions;
            cbRootcauseId.DataSource = FaultEventConstants.RootCauses;
            cbRootcauseId.DataSource = FaultEventConstants.RootCauses;
		}

	    /// <summary>
		/// Checks if the required fields have valid data to enable the tester to press the
		/// Submit button to send the fault event to DB.
		/// </summary>
		private void CheckButtons()
		{
		    var enable = (cbFaultType.SelectedIndex >= 0)  && (cbSubType.SelectedIndex >= 0) &&
		                  (cbOperProgress.SelectedIndex >= 0) && (cbRecovery.SelectedIndex >= 0) &&
		                  (cbDisposition.SelectedIndex >= 0) && (tbTimeToReady.Text.Length > 0) &&
		                  (!cbFaultCode.Enabled || cbFaultCode.Text.Length > 0);
			btnSubmit.Enabled = enable;
			btnLink.Enabled = false;		// Not fully implemented or designed....
		}

    //---------------------------------------------------------------------------------------------------------
    // Event handling code for cbSubType control. cbSubType specifies what happened.
    //---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the tester enters cbFaultType Control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultType_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Select the whether a Jam or an Error occurred.";
		}

        /// <summary>
        /// Event handler for the cbFaultType dropdown when trhe selection has changed. Allows tester to 
        /// select between a Jam and an Error. Will set up the other UI Fields accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbFaultType.SelectedIndex >= 0)
			{
				cbSubType.Enabled = true;
				cbRecovery.Enabled = true;
				if (_curEvent != cbFaultType.SelectedIndex)
				{
					_curEvent = cbFaultType.SelectedIndex;
					if (string.Compare(cbFaultType.SelectedItem.ToString(), @"Jam", StringComparison.OrdinalIgnoreCase) == 0)
					{
						cbSubType.DataSource = FaultEventConstants.JamTypes;
						cbRecovery.DataSource = FaultEventConstants.JamRecoveries;
						cbFaultCode.DataSource = null;
						cbFaultCode.Enabled = true;
						cbFaultCode.DropDownStyle = ComboBoxStyle.Simple;
						cbFaultCode.ResetText();
					}
					else if (string.Compare(cbFaultType.SelectedItem.ToString(), @"Error", StringComparison.OrdinalIgnoreCase) == 0)
					{
						cbSubType.DataSource = FaultEventConstants.ErrorTypes;
						cbRecovery.DataSource = FaultEventConstants.ErrorRecoveries;
						cbFaultCode.DataSource = null;
						cbFaultCode.Enabled = false;
						cbFaultCode.DropDownStyle = ComboBoxStyle.Simple;
						cbFaultCode.ResetText();
					}
					cbSubType.ResetText();
					cbRecovery.ResetText();
					cbSubType.SelectedIndex = -1;
					cbRecovery.SelectedIndex = -1;
					_curType = -2;
				}
			}
			CheckButtons();
		}

	//---------------------------------------------------------------------------------------------------------
	// Event handling code for cbSubType control. cbSubType specifies what happened.
	//---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the tester enters the cbSybType control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbSubType_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Use the drop down list to select what best described the fault that has occurred";
		}

        /// <summary>
        /// Event Handler for the cbSubType dropdown when the selcetion has chanegd.  Allows tester to 
        /// select the subtype on the test event. Modifies some controls and data sources depending on the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbSubType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbSubType.SelectedIndex >= 0)
			{
				if (_curType != cbSubType.SelectedIndex)
				{
					_curType = cbSubType.SelectedIndex;
					if (string.Compare(cbSubType.SelectedItem.ToString(), @"Hang", StringComparison.OrdinalIgnoreCase) == 0)
					{
						cbFaultCode.Enabled = true;
						cbFaultCode.ResetText();
						cbFaultCode.DataSource = FaultEventConstants.HangTypes;
						cbFaultCode.DropDownStyle = ComboBoxStyle.DropDownList;
						cbFaultCode.SelectedIndex = -1;
					}
					else if ((string.Compare(cbSubType.SelectedItem.ToString(), @"Crash", StringComparison.OrdinalIgnoreCase) == 0) ||
									(string.Compare(cbSubType.SelectedItem.ToString(), @"Orange Screen", StringComparison.OrdinalIgnoreCase) == 0))
					{
						cbFaultCode.Enabled = true;
						cbFaultCode.ResetText();
						cbFaultCode.DropDownStyle = ComboBoxStyle.Simple;
						cbFaultCode.DataSource = null;
					}
					else if (string.Compare(cbFaultType.SelectedItem.ToString(), @"Jam", StringComparison.OrdinalIgnoreCase) != 0)
					{
						cbFaultCode.Enabled = false;
						cbFaultCode.ResetText();
						cbFaultCode.DropDownStyle = ComboBoxStyle.Simple;
						cbFaultCode.DataSource = null;
					}
				}
			}
			CheckButtons();
		}

	//---------------------------------------------------------------------------------------------------------
	// Event handling code for cbFaultCode control. cbFaultCode is only used for Hangs, Crashes and Orange 
	// screens.  For Hangs, the user must select a field, for crashes and Orange Screens, they will enter a
	// Number.
	//---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text based on whether the test event was a Jam or an Error
        /// when the tester enters the cbFaultCode control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultCode_Enter(object sender, EventArgs e)
        {
            tbDescript.Text = cbFaultCode.DataSource == null ? @"Type the Error or Jam number; (e.g. 49.3807 or 99.00.01)." : @"Select from the Drop Down list of Hangs";
        }

        /// <summary>
        /// Event Handler on keystrokes in the cbfaultCode control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultCode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Regex.IsMatch(e.KeyChar.ToString(), @"[^a-zA-Z0-9.]")) &&
				(e.KeyChar != '\b'))
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// Event Handler for cbFaultCode when the control is acting like a dropdown list and 
        /// the selection changes. Might enable the 'Submit' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultCode_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

        /// <summary>
        /// Event Handler for cbFaultCode when the control is axcting like 'text box' and the tester
        /// changes the value. Might enable the 'Submit' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbFaultCode_TextChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

	//---------------------------------------------------------------------------------------------------------
	// Event handling code for cbOperProgress control. cbOperProgress records the operation that was
	// in progress when the event occurred.
	//---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the tester enters the cbOperProgress control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbOperProgress_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Select from the Drop Down list the operation that best describes what was in progress when this event occurred.";
		}

        /// <summary>
        /// Event Handler for the cbOperProgress control when trhe selexction changes.
        /// May result in the 'Submit' button being enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbOperProgress_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for tbTimeToReady control. tbTimeToReady records the number of seconds that it took
	// for the device to return to a ready or usable state.  This field is tightly coupled with the btnStart
	// and btnClear controls, as well as the member variables elapsedSecs and timerState
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the tester enters the tbTimeToReady control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void tbTimeToReady_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Use the buttons to the right to time the recovery and automatically enter the time to ready, OR enter the time (in SECONDS) from when you start to resolve the fault until the UUT retruns to ready.";
		}

        /// <summary>
        /// Event handler for tbTimeToReady control when the tester enters data in the control. 
        /// Validates that only valid keys are pressed for input in the tbTimeToReady control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void tbTimeToReady_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((Regex.IsMatch(e.KeyChar.ToString(), @"[^0-9^#^,^\+^\(^\)^\*^-]")) &&
				(e.KeyChar != '\b'))
			{
				e.Handled = true;
			}
		}

        /// <summary>
        /// Event handler for tbTimeToReady when the control text changes.  May enable the 'Submit' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void tbTimeToReady_TextChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for btnStart control. This button controls a simple timer that can be used to
	// time the UUT's recovery from the Fault.  The button toggles between Start and Stop every press, except
	// if btnClear has been pressed.  btnClear will reset the state of this button as well as the timer!
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the btnStart control is the active control.  The text is
        /// dependent on the state of the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnStart_Enter(object sender, EventArgs e)
        {
            tbDescript.Text = !_timerState ? @"Click on 'Start' when you start resolving the fault in order to start the timing the number of seconds it takes to recover." 
                : @"When the deivce has recovered and is 'Ready', click on 'Stop' to stop the timer. The time will be autmoatically palced in the TimeToReady field";
        }

        /// <summary>
        /// Event handler for btnStart when the butrtron is clicked. Starts or stops the 'Time To Ready' timer.
        /// This is a simple toggle button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnStart_Click(object sender, EventArgs e)
		{
			if (!_timerState)
			{
				tbDescript.Text = @"When the device has recovered and is 'Ready', click on 'Stop' to stop the timer. The time will be autmoatically palced in the TimeToReady field";
				_elapsedSecs = Environment.TickCount;
				_timerState = true;
				btnStart.Text = @"Stop";
			}
			else
			{
				_elapsedSecs = ((Environment.TickCount - _elapsedSecs) + 500) / 1000;
				_timerState = false;
				btnStart.Text = @"Start";
				tbTimeToReady.Text = _elapsedSecs.ToString();
				tbDescript.Text = @"Click on 'Start' when you start resolving the fault in order to start the timing the number of seconds it takes to recover.";
			}
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for btnClear control. This button will reset tbTimeToReady to blank and will
	// for the state of the timer to 0 / uninitialized, as well as forcing btnStart's test to 'Start'.
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the btnClear control is the active control.  The text is
        /// dependent on the state of the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnClear_Enter(object sender, EventArgs e)
		{
			if ((!_timerState) && (string.IsNullOrEmpty(tbTimeToReady.Text)))
			{
				tbDescript.Text = @"Click on 'Start' when you start resolving the fault in order to start the timing the number of seconds it takes to recover.";
			}
			else
			{
				tbDescript.Text = @"Click on Clear to reset the timer or on Start to restart the timer from 0.";
			}
		}

        /// <summary>
        /// Event Handler for btnClear when the button is clicked. Will reset the timer to 0
        /// and change the timer state to not started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnClear_Click(object sender, EventArgs e)
		{
			_timerState = false;
			_elapsedSecs = 0;
			btnStart.Text = @"Start";
			tbTimeToReady.Text = "";
			tbDescript.Text = @"Click on 'Start' when you start resolving the fault in order to start the timing the number of seconds it takes to recover.";
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for cbRecovery control. This control will allow selection of the choices that
	// reflect how the UUT recovered from the fault
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the cbRecovery control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbRecovery_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Select the best description of how the UUT recovered from the fault.";
		}

        /// <summary>
        /// Event Handler for cbRecovery when the selection is changed. May enable the 'Submit' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbRecovery_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for cbDisposition control. This control will allow selection of the choices that
	// reflect what happened to the job that was in progress when the fault occurred
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the cbDisposition control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDisposition_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Select the best description of what hapened with the 'job' or task that was in progress.";
		}

        /// <summary>
        /// Event handler for cbDisposition when the selection is changed. May enable the Submit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void cbDisposition_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckButtons();
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for tbComments control. This control allows the tester to enter a short comment
	// on the fault.  tests will put the test name or other info in the comment.
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the tbComments control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void tbComments_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Optional: Enter a short comment on the fault if applicable.";
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for btnSubmit control. This button will submit the fault to DB.
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the btnSubmit control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnSubmit_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Click this button to submit the data to DB and exit the Fault Handler";
		}

        /// <summary>
        /// Event handler for btnSubmit when the button is clicked.
        /// NOTE: Will close the form and exit the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnSubmit_Click(object sender, EventArgs e)
		{
            string code = cbFaultCode.Enabled ? (string.Compare(cbSubType.SelectedText, @"Hang", StringComparison.OrdinalIgnoreCase) != 0 ?
                            (cbFaultCode.Text.Length > 0 ? cbFaultCode.Text : string.Empty) : cbFaultCode.SelectedItem.ToString()) :
                            string.Empty;

            ERR_EVENT_TYPES eventType = EnumUtil.GetByDescription<ERR_EVENT_TYPES>(cbFaultType.SelectedItem.ToString());

            FaultEventDataLog faultEventDataLog = new FaultEventDataLog(_executionData)
            {
                EventTime = DateTime.UtcNow,                
                EventTypeId = FaultEventConstants.GetEventTypeId(cbFaultType.SelectedItem.ToString()),
                EventSubTypeId = FaultEventConstants.GetEventSubtypeId(eventType, cbSubType.SelectedItem.ToString()),
                OpInProgressId = FaultEventConstants.GetOperationInProgressId(cbOperProgress.SelectedItem.ToString()),
                RecoveryId = FaultEventConstants.GetRecoveryId(eventType, cbRecovery.SelectedItem.ToString()),
                JobDispositionId = FaultEventConstants.GetJobDispositionId(cbDisposition.SelectedItem.ToString()),
                RootCauseId = FaultEventConstants.GetRootCauseId(cbRootcauseId.SelectedItem.ToString()),
                FaultCode = code,
                RecoveryTime = string.IsNullOrEmpty(tbTimeToReady.Text)? 0: Convert.ToInt32(tbTimeToReady.Text),
                EventDetails = tbComments.Text.Length > 0 ? tbComments.Text : string.Empty,
                IsLinked = ckbLinkEvent.Checked
            };

            ExecutionServices.DataLogger.Submit(faultEventDataLog);

            Close();
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for btnLink control. This button will submit one fault and then allow creation
    // of a linked fault.
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the btnLink control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnLink_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Click this button to Submit one fault and immediate allow creation of a new fault which was the result of the first fault";
		}

        /// <summary>
        /// Not yet implemented!  The basic idea here was to allow the tester to immediately create a linked
        /// event.  The existing event would be submitted, fields cleared and appropriately initialized for
        /// the event link.  This was never implemented because the need for it did not surface.  It is currently
        /// disabled and invisible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnLink_Click(object sender, EventArgs e)
		{
			//MessageBox.Show(@"This feature is not yet implemented!  Sorry!", "Not Yet!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			//Environment.Exit(0);
		}

	//----------------------------------------------------------------------------------------------------------
	// Event handling code for btnExit control. This button is for skipping entry when a mistake was made.
	//----------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the tbDescript control text when the btnExit control is the active control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnExit_Enter(object sender, EventArgs e)
		{
			tbDescript.Text = @"Click this button when you mistakenly failed a test, or pressed Ctrl-J (Jam) or Ctrl-E (Error) in error";
		}

        /// <summary>
        /// Event Handler for btnExit when the button is clicked.  May cause the form to drop all data and exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnExit_Click(object sender, EventArgs e)
		{
			DialogResult resp = MessageBox.Show(@"Are you sure you want to skip entering a fault ?", @"SKIP", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
			if (resp == DialogResult.Yes)
			{
                Close();
			}
		}

        
	}
}
