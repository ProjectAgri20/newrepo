using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Core.UI;
using Telerik.WinControls.UI;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing Domain Account Reservation.
    /// </summary>
    public partial class DomainAccountReservationListForm : Form
    {
        private SortableBindingList<DomainAccountReservation> _domainAccountReservations = null;
        private static DomainAccountPool _pool = null;
        private BindingSource _bindingSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountReservationListForm"/> class.
        /// </summary>
        public DomainAccountReservationListForm(DomainAccountPool pool)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(radGridViewDomainAccountReservation, GridViewStyle.ReadOnly);
            ShowIcon = true;
            radGridViewDomainAccountReservation.AutoGenerateColumns = false;
            _domainAccountReservations = new SortableBindingList<DomainAccountReservation>();
            _pool = pool;
        }

        private void DomainAccountReservationListForm_Load(object sender, System.EventArgs e)
        {
            RefreshItems();
            radGridViewDomainAccountReservation.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridViewDomainAccountReservation.BestFitColumns();
        }

        private void RefreshItems()
        {
            object currentItem = (radGridViewDomainAccountReservation.CurrentRow != null ? radGridViewDomainAccountReservation.CurrentRow.DataBoundItem : null);
            using (new BusyCursor())
            {
                _domainAccountReservations.Clear();
                _domainAccountReservations = GetDomainAccountReservations();

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _domainAccountReservations;
                radGridViewDomainAccountReservation.DataSource = _bindingSource;

                if (currentItem != null)
                {
                    GridViewRowInfo foundRow = radGridViewDomainAccountReservation.Rows.FirstOrDefault(x => x.DataBoundItem.Equals(currentItem));
                    if (foundRow != null)
                    {
                        radGridViewDomainAccountReservation.CurrentRow = foundRow;
                    }
                }
            }
        }

        private static SortableBindingList<DomainAccountReservation> GetDomainAccountReservations()
        {
            SortableBindingList<DomainAccountReservation> result = new SortableBindingList<DomainAccountReservation>();
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (DomainAccountReservation domainAccountReservation in context.DomainAccountReservations.Where(n => n.DomainAccountKey.Equals(_pool.DomainAccountKey)))
                {
                    result.Add(domainAccountReservation);
                }
            }
            return result;
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                DomainAccountReservation domainAccountReservation = row.DataBoundItem as DomainAccountReservation;
                DialogResult dialogResult = MessageBox.Show($"Removing Domain Account Reservation with session id'{domainAccountReservation.SessionId.Trim()}'. Do you want to continue?", "Delete Domain Account Reservation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                SessionSummary status = null;
                if (dialogResult == DialogResult.Yes)
                {
                    using (DataLogContext logContext = DbConnect.DataLogContext())
                    {
                        status = logContext.DbSessions.FirstOrDefault(n => n.SessionId == domainAccountReservation.SessionId && n.Status == "Running");
                    }

                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        if (status == null)
                        {
                            DomainAccountReservation asset = context.DomainAccountReservations.FirstOrDefault(n => n.SessionId == domainAccountReservation.SessionId);
                            context.DomainAccountReservations.Remove(asset);
                            context.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show($"Domain Account Reservation cannot be removed for the session id '{domainAccountReservation.SessionId.Trim()}' as the session is still running", "Session is currently running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    RefreshItems();
                }
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return radGridViewDomainAccountReservation.SelectedRows.FirstOrDefault();
        }
    }
}