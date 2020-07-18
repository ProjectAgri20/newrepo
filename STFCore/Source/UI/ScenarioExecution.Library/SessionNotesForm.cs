using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Class representing the notes for an executed session
    /// </summary>
    public partial class SessionNotesForm : Form
    {
        private EnterpriseTestContext _enterpriseTestContext;
        private DataLogContext _dataLogContext;
        private SessionSummary _sessionSummary = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotesForm"/> class.
        /// </summary>
        public SessionNotesForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            _enterpriseTestContext = new EnterpriseTestContext();
            _dataLogContext = DbConnect.DataLogContext();
            LoadComboBoxes();
        }

        /// <summary>
        /// The Session Id
        /// </summary>
        public string SessionId
        {
            get { return sessionId_Label.Text; }
            set { sessionId_Label.Text = value; }
        }

        private void SessionNotesForm_Load(object sender, EventArgs e)
        {
            _sessionSummary = _dataLogContext.DbSessions.FirstOrDefault(x => x.SessionId == SessionId);

            // Populate the form
            sessionId_Label.Text = _sessionSummary.SessionId;
            notes_TextBox.Text = _sessionSummary.Notes;

            sessionName_ComboBox.Text = _sessionSummary.SessionName;
            sessionType_ComboBox.Text = _sessionSummary.Type;
            sessionCycle_ComboBox.Text = _sessionSummary.Cycle;
            reference_TextBox.Text = _sessionSummary.Reference;
            LoadTags();

            try
            {
                RefreshDataGrid();
            }
            catch
            {
                product_GridView.DataSource = null;
            }

        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (_sessionSummary != null)
            {
                // Build string of selected tags.
                string tagList = BuildTagList();

                // Update DataLog.SessionSummary.
                _sessionSummary.SessionName = sessionName_ComboBox.Text;
                _sessionSummary.Type = sessionType_ComboBox.Text;
                _sessionSummary.Cycle = sessionCycle_ComboBox.Text;
                _sessionSummary.Reference = reference_TextBox.Text;
                _sessionSummary.Tags = tagList;
                _sessionSummary.SessionName = sessionName_ComboBox.Text;
                _sessionSummary.Notes = notes_TextBox.Text;
                _sessionSummary.Tags = tagList;

                //Commit changes to the databases
                _dataLogContext.SaveChanges();
                _enterpriseTestContext.SaveChanges();

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Builds a string of selected tags, comma delimited.
        /// </summary>
        /// <returns></returns>
        private string BuildTagList()
        {
            StringBuilder result = new StringBuilder();
            foreach (string selectedTag in tags_CheckedListBox.CheckedItems)
            {
                if (result.Length == 0)
                {
                    result.Append(selectedTag);
                }
                else
                {
                    result.Append(",").Append(selectedTag);
                }
            }

            return result.ToString();
        }

        private void LoadTags()
        {
            // Add all the possible tags to the list.
            string scenarioTags = GlobalSettings.Items[Setting.ScenarioTags];
            tags_CheckedListBox.Items.AddRange(scenarioTags.Split(','));

            if ((_sessionSummary != null) && (!string.IsNullOrEmpty(_sessionSummary.Tags)))
            {
                string[] selectedTags = _sessionSummary.Tags.Split(',');
                foreach (string selectedTag in selectedTags)
                {
                    if (tags_CheckedListBox.Items.Contains(selectedTag))
                    {
                        tags_CheckedListBox.SetItemChecked(tags_CheckedListBox.Items.IndexOf(selectedTag), true);
                    }
                }
            }
        }

        private void LoadComboBoxes()
        {
            sessionName_ComboBox.DataSource = ResourceWindowsCategory.Select(_enterpriseTestContext, ResourceWindowsCategoryType.SessionName.ToString());
            sessionType_ComboBox.DataSource = ResourceWindowsCategory.Select(_enterpriseTestContext, ResourceWindowsCategoryType.SessionType.ToString());
            sessionCycle_ComboBox.DataSource = ResourceWindowsCategory.Select(_enterpriseTestContext, ResourceWindowsCategoryType.SessionCycle.ToString());
            
        }

        private void RefreshDataGrid()
        {
            product_GridView.DataSource = null;
            addNewProduct_ComboBox.DataSource = null;
            IQueryable<SessionProduct> currentProducts = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.IsDeleted == false);
            //List<ProductGridInfo> temp = currentProducts.Select(x => new ProductGridInfo(x.Name, x.Version )).ToList();
            List<ProductGridInfo> temp = currentProducts.Select(x => new ProductGridInfo() {Name =  x.Name, Version = x.Version }).ToList();
            List<string> names = temp.Select(x => x.Name).ToList();
            product_GridView.DataSource = product_GridView.DataSource = temp.Select(x => new { Product = x.Name, Version = x.Version }).ToList();

            IQueryable<SessionProduct> totalProducts = _dataLogContext.DbSessionProducts.Where(x => !names.Contains(x.Name));

            //product_GridView.DataSource = temp.Select(x=> new { Product = x.Name, Version  = x.Version}).ToList();
            product_GridView.Refresh();
            if (product_GridView.Rows.Count < 1)
            {
                version_TextBox.Enabled = false;
                save_Button.Enabled = false;
                delete_Button.Enabled = false;
            }
            else
            {
                delete_Button.Enabled = true;
            }
            List<string> remainingProducts = totalProducts.Select(x => x.Name).Distinct().ToList();
            addNewProduct_ComboBox.DataSource = remainingProducts;

            product_GridView.CurrentCell.Selected = false;
            save_Button.Enabled = false;
            version_TextBox.Text = string.Empty;
            version_TextBox.Enabled = false;
        }

        private void product_GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (product_GridView.SelectedCells.Count > 0)
            {
                int selectedrowindex = product_GridView.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = product_GridView.Rows[selectedrowindex];
                string selectedProduct = Convert.ToString(selectedRow.Cells["Product"].Value);
                version_TextBox.Enabled = true;
                save_Button.Enabled = true;

                var version = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.Name == selectedProduct && x.IsDeleted == false).Select(x => x.Version).FirstOrDefault().ToString();
                version_TextBox.Text = version;
                version_TextBox.Enabled = true;
                save_Button.Enabled = true;
            }
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            if (addNewProduct_ComboBox.SelectedValue != null)
            {
                try
                {

                    string product = (string)addNewProduct_ComboBox.SelectedValue;

                    SessionProduct template = _dataLogContext.DbSessionProducts.Where(x => x.Name == product && _sessionSummary.SessionId == x.SessionId).FirstOrDefault();
                    if (template == null)
                    {
                        template = _dataLogContext.DbSessionProducts.Where(x => x.Name == product).FirstOrDefault();
                        SessionProduct sessionProduct = new SessionProduct();
                        sessionProduct.SessionId = _sessionSummary.SessionId;
                        sessionProduct.EnterpriseTestAssociatedProductId = template.EnterpriseTestAssociatedProductId;
                        sessionProduct.Name = template.Name;
                        sessionProduct.Vendor = template.Vendor;
                        sessionProduct.Version = string.Empty;
                        sessionProduct.IsDeleted = false;

                        _dataLogContext.DbSessionProducts.Add(sessionProduct);
                    }
                    else
                    {
                        template.IsDeleted = false;
                        template.Version = string.Empty;
                        _dataLogContext.Entry(template).Property("IsDeleted").IsModified = true;
                    }


                    _dataLogContext.SaveChanges();
                    RefreshDataGrid();
                }
                catch
                {
                    MessageBox.Show("Failed to add the Product", "Product Add Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void delete_Button_Click(object sender, EventArgs e)
        {
            if (product_GridView.SelectedCells.Count > 0)
            {
                try
                {
                    int selectedrowindex = product_GridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = product_GridView.Rows[selectedrowindex];
                    string selectedProduct = Convert.ToString(selectedRow.Cells["Product"].Value);
                    var version = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.Name == selectedProduct).Select(x => x.Version).FirstOrDefault().ToString();
                    var removing = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.Name == selectedProduct && x.Version == version).FirstOrDefault();
                    removing.IsDeleted = true;
                    _dataLogContext.Entry(removing).Property("Version").IsModified = true;
                    _dataLogContext.SaveChanges();

                    RefreshDataGrid();
                }
                catch
                {
                    MessageBox.Show("Failed to remove the Product", "Product Removal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void save_Button_Click(object sender, EventArgs e)
        {
            if (version_TextBox.Enabled == true)
            {
                try
                {
                    int selectedrowindex = product_GridView.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = product_GridView.Rows[selectedrowindex];
                    string selectedProduct = Convert.ToString(selectedRow.Cells["Product"].Value);
                    var oldVersion = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.Name == selectedProduct).Select(x => x.Version).FirstOrDefault().ToString();
                    string newVersion = version_TextBox.Text;
                    var updating = _dataLogContext.DbSessionProducts.Where(x => x.SessionId == _sessionSummary.SessionId && x.Name == selectedProduct && x.Version == oldVersion).FirstOrDefault();
                    updating.Version = newVersion;
                    _dataLogContext.Entry(updating).Property("Version").IsModified = true;
                    _dataLogContext.SaveChanges();
                    RefreshDataGrid();
                }
                catch
                {
                    MessageBox.Show("Failed to update the product version", "Version Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }

    public class ProductGridInfo
    {
        public string Name;
        public string Version;

        public ProductGridInfo()
        {
            Name = string.Empty;
            Version = string.Empty;
        }
        public ProductGridInfo(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
