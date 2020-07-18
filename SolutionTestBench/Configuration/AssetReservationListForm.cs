using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Properties;
using Telerik.WinControls.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// Wrapper class created to handle Generic implementation of a WinForm in the Visual Studio IDE.
    /// </summary>
    public class DummyForm : Form
    {
    }

    /// <summary>
    /// List form showing STF Server entries
    /// </summary>
    public partial class AssetReservationListForm<T> : DummyForm where T : Asset
    {
        //private SortableBindingList<AssetReservation> _reservations = null;
        private SortableBindingList<AssetReservationLocal> _reservations = null;
        private AssetInventoryContext _context = null;
        private Collection<AssetReservationLocal> _deletedItems = new Collection<AssetReservationLocal>();
        bool _unsavedChanges = false;
        private readonly T _asset;
        private readonly DateTime _maxDateTime = new DateTime(3000, 1, 1, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the PrinterReservationListForm class.
        /// </summary>
        public AssetReservationListForm(T asset)
        {
            InitializeComponent();

            ShowInTaskbar = false;

            UserInterfaceStyler.Configure(reservation_RadGridView, GridViewStyle.ReadOnly);
            reservation_RadGridView.AutoGenerateColumns = false;
            reservation_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            reservation_RadGridView.BestFitColumns();
            reservation_RadGridView.CellFormatting += Reservation_RadGridView_CellFormatting;
            reservation_RadGridView.SelectionChanged += Reservation_RadGridView_SelectionChanged;

            _asset = asset;

            Text = $"Reservations for '{asset.AssetId.Trim()}'";

            _context = DbConnect.AssetInventoryContext();
            _reservations = new SortableBindingList<AssetReservationLocal>();
        }

        private void AssetListForm_Load(object sender, EventArgs e)
        {
            using (new BusyCursor())
            {
                toolStripMenuItem_Current_Click(sender, e);
            }

            if (reservation_RadGridView.Rows.Count > 0)
            {
                reservation_RadGridView.CurrentRow = reservation_RadGridView.Rows.First();
            }
        }

        private void LoadReservationData()
        {
            reservation_RadGridView.DataSource = null;
            _reservations.Clear();
            if (toolStripMenuItem_Current.Checked)
            {
                foreach (var item in _context.AssetReservations.Where(x => x.AssetId.Equals(_asset.AssetId)).OrderBy(x => x.Start))
                {
                    _reservations.Add(new AssetReservationLocal(item));
                }
            }

            if (toolStripMenuItem_History.Checked)
            {
                foreach (var item in _context.ReservationHistory.Where(x => x.AssetId.Equals(_asset.AssetId)).OrderBy(x => x.Start))
                {
                    var reservation = new AssetReservationLocal(item);
                    reservation.SessionId = "Expired / Deleted";
                    _reservations.Add(reservation);
                }
            }
            reservation_RadGridView.DataSource = _reservations;
        }

        private void Reservation_RadGridView_SelectionChanged(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var reservation = row.DataBoundItem as AssetReservationLocal;
                editToolStripButton.Enabled = removeToolStripButton.Enabled = CanEditDelete(reservation);
            }
        }

        private void Reservation_RadGridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            var data = e.Row.DataBoundItem as AssetReservationLocal;
            if (data != null && data.Historical)
            {
                foreach (GridViewCellInfo cell in e.Row.Cells)
                {
                    cell.Style.ForeColor = Color.DarkGoldenrod;
                }
            }

            var column = reservation_RadGridView.Columns[e.ColumnIndex].Name;
            if (column == "End" || column == "Start")
            {
                if (e.CellElement.Value != null)
                {
                    var color = e.CellElement.BackColor;

                    DateTime dateTimeValue;
                    if (DateTime.TryParse(e.CellElement.Value.ToString(), out dateTimeValue))
                    {
                        if (dateTimeValue == _maxDateTime)
                        {
                            e.CellElement.Value = Resources.Permanent;
                        }
                    }
                }
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return reservation_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void asset_DataGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var reservation = row.DataBoundItem as AssetReservationLocal;

                if (!reservation.Historical && EditEntry(reservation) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    reservation_RadGridView.Refresh();
                }
            }
        }

        private void ok_Button_Click(object sender, System.EventArgs e)
        {
            Commit();
            Close();
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        private void Commit()
        {
            using (new BusyCursor())
            {
                foreach (var item in _deletedItems)
                {
                    var reservation = _context.AssetReservations.FirstOrDefault(x => x.AssetId.Equals(item.AssetId) && x.AssetReservationId.Equals(item.ReservationId));
                    if (reservation != null)
                    {
                        _context.ReservationHistory.Add(new ReservationHistory(reservation));
                        _context.AssetReservations.Remove(reservation);
                    }
                }

                foreach (var item in _reservations.Where(x => x.Historical == false))
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            var email = UserPrincipal.Current.EmailAddress;
                            var addedEntity = new AssetReservation
                            {
                                AssetReservationId = SequentialGuid.NewGuid(),
                                AssetId = item.AssetId,
                                ReservedBy = item.ReservedBy,
                                ReservedFor = item.ReservedFor,
                                Start = DateTime.Parse(item.Start),
                                End = item.End.Equals(Resources.Permanent) ? _maxDateTime : DateTime.Parse(item.End),
                                Received = DateTime.Now,
                                Notify = AssetReservationExpirationNotify.DoNotNotify,
                                CreatedBy = email
                            };

                            _context.AssetReservations.Add(addedEntity);
                            break;
                        case EntityState.Modified:
                            var modifedEntity = _context.AssetReservations.FirstOrDefault(x => x.AssetId.Equals(item.AssetId));
                            if (modifedEntity != null)
                            {
                                item.UpdateEntity(modifedEntity);
                            }
                            break;
                    }
                }

                _context.SaveChanges();
                _deletedItems.Clear();
            }
        }

        private DialogResult EditEntry(AssetReservationLocal reservation)
        {
            DialogResult result = DialogResult.None;

            using (var form = new AssetReservationEditForm<T>(reservation, _asset, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Close();
                    return;
                }
            }
            else
            {
                Close();
                return;
            }
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var reservation = row.DataBoundItem as AssetReservationLocal;

                if (EditEntry(reservation) == DialogResult.OK)
                {
                    reservation.State = EntityState.Modified;
                    _unsavedChanges = true;
                    reservation_RadGridView.Refresh();
                }
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Commit();
            _unsavedChanges = false;
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            AssetReservationLocal reservation = new AssetReservationLocal()
            {
                ReservationId = SequentialGuid.NewGuid(),
                AssetId = _asset.AssetId,
                Start = now.ToString(),
                End = now.AddDays(1).ToString()
            };

            if (EditEntry(reservation) == DialogResult.OK)
            {
                reservation.State = EntityState.Added;
                reservation.Received = DateTime.Now;
                reservation.CreatedBy = Environment.UserName;
                _reservations.Add(reservation);

                int index = reservation_RadGridView.Rows.Count - 1;

                reservation_RadGridView.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                reservation_RadGridView.TableElement.ScrollToRow(index);
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var reservation = row.DataBoundItem as AssetReservationLocal;
                _unsavedChanges = true;
                _deletedItems.Add(reservation);
                _reservations.Remove(reservation);
            }
        }

        private void toolStripMenuItem_Current_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_Current.Checked = true;
            toolStripMenuItem_History.Checked = false;
            LoadReservationData();
            addToolStripButton.Enabled = true;
        }

        private void toolStripMenuItem_History_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_History.Checked = true;
            toolStripMenuItem_Current.Checked = false;
            LoadReservationData();
            addToolStripButton.Enabled = false;
        }

        private bool CanEditDelete(AssetReservationLocal reservation)
        {
            //System.Diagnostics.Debug.WriteLine($"Historical=false:{reservation.Historical == false}");
            //System.Diagnostics.Debug.WriteLine($"Null SessionId:{string.IsNullOrEmpty(reservation.SessionId)}");
            //System.Diagnostics.Debug.WriteLine($"Admin:{UserManager.CurrentUserRole == UserRole.Administrator}");
            //System.Diagnostics.Debug.WriteLine("---------------------------------------------------------------");

            return (reservation.Historical == false && (string.IsNullOrEmpty(reservation.SessionId) || UserManager.CurrentUser.HasPrivilege(UserRole.Administrator)));
        }
    }

    internal class AssetReservationLocal : INotifyPropertyChanged
    {
        private string _assetId;
        private Guid _reservationId;
        private string _start;
        private string _end;
        private string _reservedFor;
        private string _reservedBy;
        private string _sessionId;
        private string _notes;
        private string _createdBy;
        private DateTime _received;

        public bool Historical { get; set; }
        public EntityState State { get; set; }

        public void SetPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string AssetId
        {
            get
            {
                return _assetId;
            }

            set
            {
                _assetId = value;
                SetPropertyChanged("AssetId");
            }
        }

        public Guid ReservationId
        {
            get
            {
                return _reservationId;
            }

            set
            {
                _reservationId = value;
                SetPropertyChanged("ReservationId");
            }
        }

        public string Start
        {
            get
            {
                return _start;
            }

            set
            {
                _start = value;
                SetPropertyChanged("Start");
            }
        }

        public string End
        {
            get
            {
                return _end;
            }

            set
            {
                _end = value;
                SetPropertyChanged("End");
            }
        }

        public string ReservedFor
        {
            get
            {
                return _reservedFor;
            }

            set
            {
                _reservedFor = value;
                SetPropertyChanged("ReservedFor");
            }
        }

        public string ReservedBy
        {
            get
            {
                return _reservedBy;
            }

            set
            {
                _reservedBy = value;
                SetPropertyChanged("ReservedBy");
            }
        }

        public string SessionId
        {
            get
            {
                return _sessionId;
            }

            set
            {
                _sessionId = value;
                SetPropertyChanged("SessionId");
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                _notes = value;
                SetPropertyChanged("AssetId");
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }

            set
            {
                _createdBy = value;
                SetPropertyChanged("CreatedBy");
            }
        }

        public DateTime Received
        {
            get
            {
                return _received;
            }

            set
            {
                _received = value;
                SetPropertyChanged("Received");
            }
        }

        public AssetReservationLocal()
        {
        }

        public AssetReservationLocal(AssetReservation reservation)
        {
            Historical = false;
            AssetId = reservation.AssetId;
            //End = reservation.End.ToString("{0:dd-MMM-yyyy HH:mm:ss}");
            End = reservation.End.ToString();
            ReservationId = reservation.AssetReservationId;
            ReservedBy = reservation.ReservedBy;
            ReservedFor = reservation.ReservedFor;
            SessionId = reservation.SessionId;
            //Start = reservation.Start.ToString("{0:dd-MMM-yyyy HH:mm:ss}"); ;
            Start = reservation.Start.ToString();
            Notes = reservation.Notes;
            CreatedBy = reservation.CreatedBy;
            Received = reservation.Received;
            State = EntityState.Unchanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateEntity(AssetReservation item)
        {
            item.AssetId = AssetId;
            item.End = DateTime.Parse(End);
            item.AssetReservationId = ReservationId;
            item.ReservedBy = ReservedBy;
            item.ReservedFor = ReservedFor;
            item.SessionId = SessionId;
            item.Start = DateTime.Parse(Start);
            item.Notes = Notes;
            item.CreatedBy = CreatedBy;
            Received = Received;
        }

        public AssetReservationLocal(ReservationHistory history)
        {
            Historical = true;
            AssetId = history.AssetId;
            End = history.End.ToString();
            ReservedFor = history.ReservedFor;
            Start = history.Start.ToString();
            State = EntityState.Unchanged;
        }
    }
}
