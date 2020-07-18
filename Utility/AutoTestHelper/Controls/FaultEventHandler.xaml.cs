using HP.RDL.EDT.AutoTestHelper.DDS;
using System;
using System.Windows;
using System.Windows.Controls;
using HP.RDL.EDT.ClientDDS;
using HP.ScalableTest.Utility;

namespace HP.RDL.EDT.AutoTestHelper.Controls
{
    /// <summary>
    /// Interaction logic for FaultEventHandler.xaml
    /// </summary>
    public partial class FaultEventHandler : Window
    {
        private AccessDDS _accessDds;
        private readonly string _testInstanceId;
        private DateTime _timerStartTime;
        public FaultEventHandler()
        {
            InitializeComponent();
            string environment = "Production";
#if DEBUG
            environment = "Development";
            //_testInstanceId = "D288AB0B-40C1-4C99-83E8-FF9AC293E9F8";
#endif
            _accessDds = new AccessDDS(environment);
        }

        public FaultEventHandler(AccessDDS accessDds)
        {
            _testInstanceId = accessDds.TestInstanceId;
            _accessDds = accessDds;
            InitializeComponent();
 
        }

        private void FaultEventHandler_OnLoaded(object sender, RoutedEventArgs e)
        {
            FaultTypeComboBox.ItemsSource = FaultEventConstants.Events;
            OperationComboBox.ItemsSource = FaultEventConstants.Operations;
            JobComboBox.ItemsSource = FaultEventConstants.JobDispositions;
            RootCauseComboBox.ItemsSource = FaultEventConstants.RootCauses;
        }

        private void FaultTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FaultTypeComboBox.SelectedIndex == -1)
                return;

            var faultType = (ErrorEventTypes)Enum.Parse(typeof(ErrorEventTypes), FaultTypeComboBox.SelectedValue.ToString());
            switch (faultType)
            {
                case ErrorEventTypes.Jam:
                    {
                        FaultSubTypeComboBox.ItemsSource = FaultEventConstants.JamTypes;
                        RecoveryComboBox.ItemsSource = FaultEventConstants.JamRecoveries;
                    }
                    break;

                case ErrorEventTypes.Error:
                    {
                        FaultSubTypeComboBox.ItemsSource = FaultEventConstants.ErrorTypes;
                        RecoveryComboBox.ItemsSource = FaultEventConstants.ErrorRecoveries;
                    }
                    break;
            }
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ValidateFaultContent())
            {
                int ttr = string.IsNullOrEmpty(TimeToReadyTextBox.Text) ? 0 : Convert.ToInt32(TimeToReadyTextBox.Text);

                if (ChainedEventCheckBox.IsChecked.HasValue && ChainedEventCheckBox.IsChecked.Value)
                {
                    string previousId = string.IsNullOrEmpty(_accessDds.FirmwareId)
                        ? string.IsNullOrEmpty(_accessDds.SleepWakeId) ? _accessDds.PowerBootId : _accessDds.SleepWakeId
                        : _accessDds.FirmwareId;
                    _accessDds.InsertChainedFault(_testInstanceId, previousId, FaultTypeComboBox.SelectedValue.ToString(),
                        FaultSubTypeComboBox.SelectedValue.ToString(), OperationComboBox.SelectedValue.ToString(),
                        RecoveryComboBox.SelectedValue.ToString(),
                        JobComboBox.SelectedValue.ToString(), DateTime.UtcNow, ttr, CommentsTextBox.Text,
                        CommentsTextBox.Text,
                        string.IsNullOrEmpty(FaultCodeComboBox.Text)
                            ? FaultCodeComboBox.SelectedValue.ToString()
                            : FaultCodeComboBox.Text,
                        RootCauseComboBox.SelectedValue.ToString());
                }
                else
                {
                    _accessDds.InsertFault(_testInstanceId, FaultTypeComboBox.SelectedValue.ToString(),
                        FaultSubTypeComboBox.SelectedValue.ToString(), OperationComboBox.SelectedValue.ToString(),
                        RecoveryComboBox.SelectedValue.ToString(),
                        JobComboBox.SelectedValue.ToString(), DateTime.UtcNow, ttr, CommentsTextBox.Text,
                        CommentsTextBox.Text, FaultCodeComboBox.IsEnabled? string.IsNullOrEmpty(FaultCodeComboBox.Text) ? FaultCodeComboBox.SelectedValue.ToString() : FaultCodeComboBox.Text: string.Empty,
                        RootCauseComboBox.SelectedValue.ToString());
                }
                

                MessageBox.Show(_accessDds.IsError
                    ? $"The Fault event could not be saved. {_accessDds.GetLastError}"
                    : "Fault Event has been entered successfully!");

                if (!_accessDds.IsError)
                    Close();
            }
        }

        private bool ValidateFaultContent()
        {
            if (FaultSubTypeComboBox.SelectedIndex == -1)
                return false;

            if (FaultCodeComboBox.IsEnabled && string.IsNullOrEmpty(FaultCodeComboBox.Text))
                return false;

            if (string.IsNullOrEmpty(TimeToReadyTextBox.Text))
                return false;

            if (RecoveryComboBox.SelectedIndex == -1)
                return false;

            if (RootCauseComboBox.SelectedIndex == -1)
                return false;

            return true;
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to skip entering a fault?", "Fault Event Handler",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void ChainedEventCheckBox_OnToolTipOpening(object sender, ToolTipEventArgs e)
        {
            e.Handled = true;
        }

        private void FaultSubTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FaultCodeComboBox.ItemsSource = null;
            FaultCodeComboBox.Items?.Clear();

            if (FaultSubTypeComboBox.SelectedIndex < 0 || FaultSubTypeComboBox.SelectedIndex > 2)
            {
                FaultCodeComboBox.IsEnabled = false;
                return;
            }

            FaultCodeComboBox.IsEnabled = true;

            if (FaultSubTypeComboBox.SelectedIndex == 0)
            {
                FaultCodeComboBox.IsReadOnly = true;
                FaultCodeComboBox.IsEditable = false;
                FaultCodeComboBox.ItemsSource = FaultEventConstants.HangTypes;
            }
            else
            {
                FaultCodeComboBox.IsReadOnly = false;
                FaultCodeComboBox.IsEditable = true;
                FaultCodeComboBox.ItemsSource = null;
            }
        }

        private void StartTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            _timerStartTime = DateTime.UtcNow;
            StartTimerButton.IsEnabled = false;
        }

        private void StopTimerButton_OnClick(object sender, RoutedEventArgs e)
        {
            var timerEndTime = DateTime.UtcNow;
            var ttr = (int) (timerEndTime - _timerStartTime).TotalSeconds;
            StartTimerButton.IsEnabled = true;
            TimeToReadyTextBox.Text = ttr.ToString();
        }
    }
}