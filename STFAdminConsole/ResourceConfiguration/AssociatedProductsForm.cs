using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class AssociatedProductsForm : Form
    {
        private EnterpriseTestContext _context;
        private SortableBindingList<AssociatedProduct> _products;

        public AssociatedProductsForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            products_DataGridView.AutoGenerateColumns = false;

            _context = new EnterpriseTestContext();
        }

        private void AssociatedProductsForm_Load(object sender, EventArgs e)
        {
             _products = new SortableBindingList<AssociatedProduct>();

            foreach (AssociatedProduct product in _context.AssociatedProducts.OrderBy(n => n.Name))
            {
                _products.Add(product);
            }

            products_DataGridView.DataSource = _products;
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            AssociatedProduct newProduct = AssociatedProduct.CreateAssociatedProduct(
                associatedProductId: SequentialGuid.NewGuid(),
                name: "New Product"
            );

            _products.Add(newProduct);
            _context.AssociatedProducts.AddObject(newProduct);
        }

        private void delete_Button_Click(object sender, EventArgs e)
        {
            if (products_DataGridView.SelectedCells.Count > 0)
            {
                DataGridViewRow selectedRow = products_DataGridView.SelectedCells[0].OwningRow;
                AssociatedProduct selectedProduct = selectedRow.DataBoundItem as AssociatedProduct;
                if (selectedProduct != null)
                {
                    // 2017 Mar 23 MaxT - This is commented out to allow code to compile.  SessionInfoSet is no longer a valid property.  The AssociatedProductsForm will need to be adapted to new data schema for PR 41116
                    //if (selectedProduct.SessionInfoSet.Count > 0)// || selectedProduct.EnterpriseScenarios.Count > 0)
                    //{
                    //    MessageBox.Show("This product/solution could not be deleted because it is in use by one or more sessions/scenarios.",
                    //        "Delete",
                    //        MessageBoxButtons.OK,
                    //        MessageBoxIcon.Error
                    //    );
                    //}
                    //else
                    //{
                    //    _products.Remove(selectedProduct);
                    //    _context.AssociatedProducts.DeleteObject(selectedProduct);
                    //}
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            _context.SaveChanges();
            this.DialogResult = DialogResult.OK;
        }
    }
}
