using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    public partial class PrinterProductsForm : Form
    {
        private AssetInventoryContext _context = null;
        private Dictionary<String, SortableBindingList<StringValue>> _products = null;

        public PrinterProductsForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _context = DbConnect.AssetInventoryContext();
            _products = new Dictionary<string, SortableBindingList<StringValue>>();
        }

        /// <summary>
        /// Populates the dropdown with Printer family and displays the it's Printer names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrinterProductsForm_Load(object sender, EventArgs e)
        {
            productName_dataGridView.AutoGenerateColumns = false;

            productFamily_ComboBox.Refresh();
            List<string> familyItems;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                familyItems = context.PrinterProducts.Select(n => n.Family).Distinct().ToList();
            }
            if (!familyItems.Any())
            {
                MessageBox.Show("There are no Product Categories available in the database.\nPlease add the Product Category and try again.", "Product Category Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var item in familyItems)
            {
                productFamily_ComboBox.Items.Add(item);
                _products.Add(item, new SortableBindingList<StringValue>());
                foreach (var productName in _context.PrinterProducts.Where(n => n.Family == item).Select(n => n.Name).Distinct())
                {
                    _products[item].Add(new StringValue(productName));
                }
            }

            if (productFamily_ComboBox.Items.Count > 0)
            {
                productFamily_ComboBox.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Updates the PrinterProduct table from the DataSource and close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printerProduct_OK_Click(object sender, EventArgs e)
        {
            UpdateProducts(_context, _products);
            Close();
        }
        /// <summary>
        /// /// Updates the PrinterProduct table from the DataSource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printerProduct_Apply_Click(object sender, EventArgs e)
        {
            UpdateProducts(_context, _products);
        }

        /// <summary>
        /// Cancel the operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printerProduct_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Changes the Datagrid based on product name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productFamily_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String familyItem = (String)productFamily_ComboBox.SelectedItem;
            productName_dataGridView.DataSource = _products[familyItem];

            productName_dataGridView.Refresh();
        }

        /// <summary>
        /// Cell Validation event to allow only valid characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productName_dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {            
            String text = (String)e.FormattedValue;            
            if ((text.Length == 0) || !Regex.IsMatch(text, "^[A-Za-z0-9]*$"))
            {
                productName_dataGridView.Rows[e.RowIndex].ErrorText = "Enter valid characters";
                e.Cancel = true;
            }
            else
            {
                productName_dataGridView.Rows[e.RowIndex].ErrorText = "";
            }
        }

        private static void UpdateProducts(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> familyNames)
        {
            AddProducts(entities, familyNames);
            DeleteProducts(entities, familyNames);
        }

        private static void AddProducts(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> familyNames)
        {
            //From the data source retrieve each entry and check if it exists in table. If no entry found, add an entry.
            foreach (KeyValuePair<String, SortableBindingList<StringValue>> familyNamesPair in familyNames)
            {
                foreach (StringValue printerName in familyNamesPair.Value)
                {
                    //Add all the entries if the name does not exist in the table
                    if (!ProductNameExists(entities, familyNamesPair.Key, printerName.Value))
                    {
                        PrinterProduct product = new PrinterProduct();
                        product.Family = familyNamesPair.Key;
                        product.Name = printerName.Value;
                        entities.PrinterProducts.Add(product);
                    }
                }
            }
            entities.SaveChanges();
        }

        private static bool ProductNameExists(AssetInventoryContext entities, string family, string name)
        {
            return entities.PrinterProducts.Any(n => n.Family == family && n.Name == name);
        }

        private static void DeleteProducts(AssetInventoryContext entities, Dictionary<String, SortableBindingList<StringValue>> familyNames)
        {
            IEnumerable<PrinterProduct> printerProducts = entities.PrinterProducts.AsEnumerable();
            foreach (PrinterProduct product in printerProducts)
            {
                SortableBindingList<StringValue> namelist = null;
                familyNames.TryGetValue(product.Family, out namelist);
                if (namelist != null && !namelist.Any(n => n.Value.EqualsIgnoreCase(product.Name)))
                {
                    entities.PrinterProducts.Remove(product);
                }
            }
            entities.SaveChanges();
        }
    }
}
