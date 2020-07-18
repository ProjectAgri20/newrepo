using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Properties;

namespace HP.ScalableTest
{
    internal partial class AssetReservationEditForm<T> : Form where T : Asset
    {
        private T _asset = null;
        private AssetReservationLocal _reservation = null;
        private AssetInventoryContext _context = null;
        private ErrorProvider _error = new ErrorProvider();
        private readonly DateTime _maxDateTime = new DateTime(3000, 1, 1, 0, 0, 0);

        public AssetReservationEditForm(AssetReservationLocal reservation, T asset, AssetInventoryContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            ShowIcon = true;

            _reservation = reservation;
            _asset = asset;
            _context = context;

            _error.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void AssetReservationEditForm_Load(object sender, EventArgs e)
        {
            assetIdLabel.Text = "{0}".FormatWith(_asset.AssetId);

            var reservations = _context.AssetReservations;

            foreach (var item in reservations.Select(x => x.ReservedBy).Distinct())
            {
                reservedByComboBox.Items.Add(item);
            }

            foreach (var item in reservations.Select(x => x.ReservedFor).Distinct())
            {
                reservedForComboBox.Items.Add(item);
            }

            var startBinding = startDateTimePicker.DataBindings.Add("Value", _reservation, "Start", true, DataSourceUpdateMode.OnPropertyChanged);
            startBinding.Format += StartBinding_Format;
            var endBinding = endDateTimePicker.DataBindings.Add("Value", _reservation, "End", true, DataSourceUpdateMode.OnPropertyChanged);
            endBinding.Format += EndBinding_Format;
            notesTextBox.DataBindings.Add("Text", _reservation, "Notes", true, DataSourceUpdateMode.OnPropertyChanged);

            if (_reservation.End.Equals(Resources.Permanent))
            {
                endDateTimePicker.Value = _maxDateTime;
            }

            if (reservedByComboBox.Items.Cast<string>().Contains(_reservation.ReservedBy))
            {
                reservedByComboBox.SelectedItem = _reservation.ReservedBy;
            }
            if (reservedForComboBox.Items.Cast<string>().Contains(_reservation.ReservedFor))
            {
                reservedForComboBox.SelectedItem = _reservation.ReservedFor;
            }

            permanentCheckBox.Checked = DateTime.Parse(endDateTimePicker.Value.ToString()) == _maxDateTime;
        }

        private void EndBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.Equals(Resources.Permanent))
            {
                e.Value = _maxDateTime;
            }
        }

        private void StartBinding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = DateTime.Parse(e.Value.ToString());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _reservation.ReservedBy = reservedByComboBox.Text;
            _reservation.ReservedFor = reservedForComboBox.Text;
            if (permanentCheckBox.Checked)
            {
                _reservation.End = _maxDateTime.ToString();
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void permanentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (permanentCheckBox.Checked)
            {
                endDateTimePicker.Visible = false;
                permanentLabel.Visible = true;
            }
            else
            {
                if (_reservation.End == _maxDateTime.ToString())
                {
                    _reservation.End = DateTime.Parse(_reservation.Start).AddDays(1).ToString();
                }
                permanentLabel.Visible = false;
                endDateTimePicker.Visible = true;
            }
        }

        private void endDateTimePicker_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateReservation(endDateTimePicker.Value);
            if (e.Cancel)
            {
                _error.SetError(endDateTimePicker, "Overlaps with existing reservation");
            }
            else
            {
                _error.SetError(endDateTimePicker, string.Empty);
            }
        }

        private void startDateTimePicker_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateReservation(startDateTimePicker.Value);
            if (e.Cancel)
            {
                _error.SetError(startDateTimePicker, "Overlaps with existing reservation");
            }
            else
            {
                _error.SetError(startDateTimePicker, string.Empty);
            }
        }

        private bool ValidateReservation(DateTime value)
        {
            return
                (
                    from a in _context.AssetReservations
                    where a.AssetReservationId != _reservation.ReservationId &&
                        a.AssetId.Equals(_asset.AssetId) &&
                        value >= a.Start && value <= a.End
                    select a
                ).Any();
        }

        private void reservedForComboBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = string.IsNullOrEmpty(reservedForComboBox.Text);
            if (e.Cancel)
            {
                _error.SetError(reservedForComboBox, "A value is required");
            }
            else
            {
                _error.SetError(reservedForComboBox, string.Empty);
            }
        }

        private void reservedByComboBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = string.IsNullOrEmpty(reservedByComboBox.Text);
            if (e.Cancel)
            {
                _error.SetError(reservedByComboBox, "A value is required");
            }
            else
            {
                _error.SetError(reservedByComboBox, string.Empty);
            }
        }
    }
}
