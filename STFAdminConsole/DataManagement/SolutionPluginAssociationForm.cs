using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HP.ScalableTest.LabConsole.DataManagement
{
    /// <summary>
    /// Form for editing Solution Associations
    /// </summary>
    public partial class SolutionPluginAssociationForm : Form
    {

        private List<AssociatedProduct> _associatedProducts;
        private List<MetadataType> _metadataTypes;
        private Guid _selectedProductId;
        private string _selectedMetadata;

        /// <summary>
        /// Creates a new instance of solution Plugin Association Form
        /// </summary>
        public SolutionPluginAssociationForm()
        {
            InitializeComponent();
        }

        private void SolutionPluginAssociationForm_Load(object sender, EventArgs e)
        {
            _metadataTypes = new List<MetadataType>();
            LoadPage();
        }

        private void LoadPage()
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                _associatedProducts = context.AssociatedProducts.ToList();
                product_DatagridView.DataSource = _associatedProducts;
                metaData_ComboBox.DataSource = context.MetadataTypes;
            }
            product_DatagridView.ClearSelection();
            _selectedProductId = Guid.Empty;
            name_TextBox.Text = string.Empty;
            vendor_TextBox.Text = string.Empty;
            _selectedMetadata = string.Empty;
            _metadataTypes.Clear();

            metadata_DatagridView.DataSource = null;
            metadata_DatagridView.ClearSelection();
            removeAssociation_Button.Enabled = false;
            addAssociation_Button.Enabled = false;
            editProduct_Button.Enabled = false;
            deleteProduct_Button.Enabled = false;
        }

        private void addProduct_button_Click(object sender, EventArgs e)
        {
            string vendor = vendor_TextBox.Text;
            string name = name_TextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(vendor))
            {
                MessageBox.Show("Name and Vendor Fields can not be blank", "Product Add Error", MessageBoxButtons.OK);
                return;
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var temp = context.AssociatedProducts.Where(y => y.Name == name && y.Vendor == vendor);
                if (temp.Count() > 0)
                {
                    MessageBox.Show("Name and Vendor already exists in the database", "Product Add Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Guid id = SequentialGuid.NewGuid();

                    AssociatedProduct newProd = new AssociatedProduct();
                    newProd.AssociatedProductId = id;
                    newProd.Name = name;
                    newProd.Vendor = vendor;

                    context.AssociatedProducts.AddObject(newProd);

                    context.SaveChanges();

                    LoadPage();
                }
            }

        }

        private void editProduct_Button_Click(object sender, EventArgs e)
        {
            string vendor = vendor_TextBox.Text;
            string name = name_TextBox.Text;
            
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(vendor))
            {
                MessageBox.Show("Name and Vendor Fields can not be blank", "Edit Product Error", MessageBoxButtons.OK);
                return;
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var temp = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId);
                if (temp.Count() == 0)
                {
                    MessageBox.Show("Name and Vendor don't exist in the database to edit", "Product Add Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {

                    AssociatedProduct edit = temp.First();
                    edit.Name = name;
                    edit.Vendor = vendor;

                    context.SaveChanges();


                    LoadPage();
                }
            }
        }

        private void product_DatagridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            editProduct_Button.Enabled = true;
            deleteProduct_Button.Enabled = true;
            addAssociation_Button.Enabled = true;
            removeAssociation_Button.Enabled = false;

            var temp = product_DatagridView.Rows[e.RowIndex].DataBoundItem as AssociatedProduct;
            _selectedProductId = temp.AssociatedProductId;
            name_TextBox.Text = temp.Name;
            vendor_TextBox.Text = temp.Vendor;


            //Populate Metadataside
            GetMetaData();


        }

        private void GetMetaData()
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var temp = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId);
                if (temp.Count() == 0)
                {
                    MessageBox.Show("Name and Vendor don't exist in the database to edit", "Product Add Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    var item = temp.First();

                    //if (item.MetadataTypes.Count > 0)
                    //{
                        metadata_DatagridView.DataSource = item.MetadataTypes;
                    if (item.MetadataTypes.Count > 0)
                    {
                        removeAssociation_Button.Enabled = true;
                    }
                    metadata_DatagridView.ClearSelection();

                    //}

                }
            }
        }


        private void deleteProduct_Button_Click(object sender, EventArgs e)
        {
            string vendor = vendor_TextBox.Text;
            string name = name_TextBox.Text;


            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var temp = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId);
                if (temp.Count() == 0)
                {
                    MessageBox.Show("Name and Vendor don't exist in the database to delete", "Product Delete Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    AssociatedProduct del = temp.First();
                    context.DeleteObject(del);

                    context.SaveChanges();

                    LoadPage();
                }

                if (context.AssociatedProducts.Count() == 0)
                {
                    editProduct_Button.Enabled = false;
                }
                
            }
            deleteProduct_Button.Enabled = false;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void addAssociation_Button_Click(object sender, EventArgs e)
        {

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var meta = metaData_ComboBox.SelectedItem as MetadataType;
                var temp = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId && y.MetadataTypes.Any(x => x.Name == meta.Name));
                //var otherTemp = temp.First().Value.Where(x => x.AssociatedProductId == _selectedProductId).First();

                if (temp.Count() > 0)
                {
                    MessageBox.Show("Plugin Association already exists for this metadata", "Product Association Error", MessageBoxButtons.OK);
                    return;
                }
                else
                {

                    var metadata = context.MetadataTypes.Where(y => y.Name == meta.Name).First();

                    var prod = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId).First();

                    prod.MetadataTypes.Add(metadata);
                    context.SaveChanges();
                }
                
            }
            LoadPage();
        }

        private void removeAssociation_Button_Click(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                //var meta = metaData_ComboBox.SelectedItem as MetadataType;
                var metadata = context.MetadataTypes.Where(y => y.Name == _selectedMetadata).First();
                var prod = context.AssociatedProducts.Where(y => y.AssociatedProductId == _selectedProductId).First();

                prod.MetadataTypes.Remove(metadata);
                context.SaveChanges();

            }
            LoadPage();
        }

        private void metadata_DatagridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var temp = metadata_DatagridView.Rows[e.RowIndex].DataBoundItem as MetadataType;
            _selectedMetadata = temp.Name;
            removeAssociation_Button.Enabled = true;
        }
    }
}
