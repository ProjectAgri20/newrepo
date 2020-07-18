using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Delegate for the product name change event
    /// </summary>
    /// <param name="productFamily">product family</param>
    /// <param name="productName">product name</param>
    public delegate void ProductNameChangedEventHandler(string productFamily, string productName);

    /// <summary>
    /// Represents the different plug-in types.
    /// </summary>
    public enum PluginType
    {
        /// <summary>
        /// The Print plug-in
        /// </summary>
        Print,

        /// <summary>
        /// The IPConfiguration plug-in
        /// </summary>
        IPConfiguration,

        /// <summary>
        /// NNS Plug-in
        /// </summary>
        NNS,

        /// <summary>
        /// Default represents any plug-in other than Print and IPConfiguration plug-ins.
        /// </summary>
        Default
    }

    /// <summary>
    /// Represents the row movement directions.
    /// </summary>
    enum RowMovement
    {
        MoveUp,
        MoveDown,
        MoveTop,
        MoveBottom
    }

    /// <summary>
    /// Base class for CTC plug-ins that contains all the common functionalities.
    /// </summary>
    public partial class CtcBaseConfigurationControl : UserControl
    {
        /// <summary>
        /// Raised event on product name changed.
        /// </summary>
        public event ProductNameChangedEventHandler OnProductNameChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Local Variables

        private Type _type;
        private static PluginType _pluginType = PluginType.Default;
        private static Collection<PrintTestData> _TestWithDuration = new Collection<PrintTestData>();
        CheckBox selectAll_CheckBox = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of CTCBasePluginEditControl.
        /// </summary>
        public CtcBaseConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireCustom(testCaseDetails_DataGrid, () => SelectedTests.Count > 0, "Select one or more tests.");

            AddHeaderCheckBox();
            //changed it to remove call from Constructor
            //if (!IsInDesignMode())
            //{
            //    LoadUIControls();
            //}
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected test case ids
        /// </summary>
        protected Collection<int> SelectedTests
        {
            get { return GetSelection(); }
        }

        /// <summary>
        /// Gets the selected tests with the duration
        /// </summary>
        public Collection<PrintTestData> TestsWithDuration
        {
            get { return GetSelectedTests(); }
        }

        /// <summary>
        /// Gets the product category
        /// </summary>
        protected ProductFamilies ProductCategory
        {
            get
            {
                if (null != productCategory_ComboBox.SelectedItem)
                {
                    return (ProductFamilies)Enum.Parse(typeof(ProductFamilies), productCategory_ComboBox.SelectedItem.ToString());
                }
                else
                {
                    return ProductFamilies.None;
                }
            }

            set
            {
                productCategory_ComboBox.SelectedItem = value.ToString();
            }
        }

        /// <summary>
        /// Gets the product name
        /// </summary>
        protected new string ProductName
        {
            get
            {
                if (null != productName_ComboBox.SelectedItem)
                {
                    return productName_ComboBox.SelectedItem.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                productName_ComboBox.SelectedItem = value;
            }
        }

        /// <summary>
        /// Sets the status of the Product Name display property
        /// </summary>
        protected bool HideProductName
        {
            set
            {
                if (value)
                {
                    productName_Label.Hide();
                    productName_ComboBox.Hide();
                }
                else
                {
                    productName_Label.Show();
                    productName_ComboBox.Show();
                }
            }
        }

        /// <summary>
        /// Gets the Test case details
        /// </summary>
        protected CTCTestCaseDetailsDataSet.TestCaseDetailsDataTable TestCaseDetails
        {
            get { return cTCTestCaseDetails.TestCaseDetails; }
        }

        /// <summary>
        /// Gets or sets the number of iterations for the test execution
        /// </summary>
        public int Iterations
        {
            get
            {
                return Convert.ToInt32(iterations_NumericUpDown.Value);
            }
            set
            {
                iterations_NumericUpDown.Value = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the test case from the current assembly using the reflection
        /// Read the attributes like id, category, description.
        /// </summary>
        public bool LoadTestCases(Type type, Collection<int> selectedTests = null, PluginType plugin = PluginType.Default, Collection<PrintTestData> testDurationDetails = null)
        {
            if (!plugin.Equals(PluginType.Default))
            {
                _pluginType = plugin;
            }

            _type = type;
            PrintTestData Testdata = null;

            selectAll_CheckBox.Checked = false;

            if (plugin == PluginType.Print)
            {
                // Hide and show the columns based on the plug-in type
                durationDataGridViewTextBoxColumn.Visible = true;
                connectivityDataGridViewTextBoxColumn.Visible = false;
                printProtocolDataGridViewTextBoxColumn.Visible = false;
                portNumberDataGridViewTextBoxColumn.Visible = false;

                testCaseDetails_DataGrid.ScrollBars = ScrollBars.Vertical;
            }

            if (plugin == PluginType.IPConfiguration)
            {
                connectivityDataGridViewTextBoxColumn.Visible = false;
            }

            if (null == type || null == productCategory_ComboBox.SelectedItem)
            {
                return false;
            }

            ProductFamilies selectedCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), productCategory_ComboBox.SelectedItem.ToString());

            // clear the previous rows before adding any new rows
            cTCTestCaseDetails.TestCaseDetails.Clear();

            // walk thru all the methods inside the class and check if the method has TestDetails
            // then add to the data set.
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                object[] attrs = methodInfo.GetCustomAttributes(new TestDetailsAttribute().GetType(), false);

                if (attrs.Length > 0)
                {
                    // since we are having only the TestDetails type custom attributes so we can cast to this type
                    TestDetailsAttribute details = (TestDetailsAttribute)attrs[0];

                    if (details.ProductCategory.HasFlag(selectedCategory))
                    {
                        // create the row data
                        CTCTestCaseDetailsDataSet.TestCaseDetailsRow row = cTCTestCaseDetails.TestCaseDetails.NewTestCaseDetailsRow();

                        row.IsSelected = selectedTests != null && selectedTests.Contains(details.Id);

                        if (plugin.Equals(PluginType.Print) || _pluginType.Equals(PluginType.Print))
                        {

                            bool durationAssigned = false;
                            uint duration = 0;
                            if (testDurationDetails != null && testDurationDetails.Count > 0)
                            {
                                var res = from data in testDurationDetails where data.TestId.Equals(details.Id) select new { Duration = data.Duration };
                                foreach (var item in res)
                                {
                                    duration = Convert.ToUInt32(item.Duration);
                                    row.Duration = duration;
                                    durationAssigned = true;
                                    break;
                                }
                            }
                            if (!durationAssigned)
                            {
                                row.Duration = details.PrintDuration;
                            }

                            if (details.PrintDuration > 0)//row.IsSelected &&
                            {
                                Testdata = new PrintTestData();
                                Testdata.TestId = (int)details.Id;
                                Testdata.Duration = (int)details.PrintDuration;
                                _TestWithDuration.Add(Testdata);
                            }
                            row.PrintProtocol = details.PrintProtocol.ToString();
                            row.PortNumber = details.PortNumber;
                        }

                        row.Category = details.Category;
                        row.ID = (uint)details.Id;
                        row.Description = details.Description;
                        row.Protocol = details.Protocol.ToString();
                        row.Connectivity = details.Connectivity.ToString();


                        cTCTestCaseDetails.TestCaseDetails.AddTestCaseDetailsRow(row);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Selects all the test cases available on the data grid
        /// </summary>
        private void SelectAllTests()
        {
            ChangeSelection(true);
        }

        /// <summary>
        /// Unselect all the test cases available on the data grid
        /// </summary>
        private void DeselectAllTests()
        {
            ChangeSelection(false);
        }

        /// <summary>
        /// Validates the grid control
        /// </summary>
        /// <returns></returns>
        public bool ValidateControls()
        {
            if (SelectedTests.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load UI controls and set default values
        /// </summary>
       // Changed it to protected
        protected void LoadUIControls()
        {
            productCategory_ComboBox.Items.Clear();

            var familyItems = CtcSettings.ProductFamilies;
            if (!(familyItems.Count > 0))
            {
                MessageBox.Show("There are no Product Categories available in the database.\nPlease add the Product Category and try again.", "Product Category Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var item in familyItems)
            {
                productCategory_ComboBox.Items.Add(item);
            }

            productCategory_ComboBox.SelectedIndexChanged += new EventHandler(productCategory_ComboBox_SelectedIndexChanged);

            productCategory_ComboBox.SelectedIndex = 0;

        }

        /// <summary>
        /// Sets the given test case to the data grid as selected
        /// </summary>
        /// <param name="selectedTests">Test numbers to be selected</param>
        private void SetSelection(Collection<int> selectedTests)
        {
            // first deselect all test cases
            DeselectAllTests();

            // walk thru the user list and select the data grid
            foreach (int testId in selectedTests)
            {
                SelectTestCase(testId);
            }
        }

        /// <summary>
        /// Returns the selected test case numbers as collection
        /// </summary>
        /// <returns>Selected test cases</returns>
        private Collection<int> GetSelection()
        {
            Collection<int> selectedTests = new Collection<int>();

            // walk thru the data set and return the selected test cases
            foreach (CTCTestCaseDetailsDataSet.TestCaseDetailsRow row in cTCTestCaseDetails.TestCaseDetails.Rows)
            {
                if (row.IsSelected)
                {
                    selectedTests.Add((int)row.ID);
                }
            }

            return selectedTests;
        }
        /// <summary>
        /// Returns the selected test case numbers as collection
        /// </summary>
        /// <returns>Selected test cases</returns>
        private Collection<PrintTestData> GetSelectedTests()
        {
            int defaultduration = 0;
            Collection<PrintTestData> selectedTests = new Collection<PrintTestData>();
            PrintTestData data = null;
            // walk thru the data set and return the selected test cases
            foreach (CTCTestCaseDetailsDataSet.TestCaseDetailsRow row in cTCTestCaseDetails.TestCaseDetails.Rows)
            {
                var res = from durRes in _TestWithDuration where durRes.TestId.Equals((int)row.ID) select new { Duration = durRes.Duration };
                foreach (var item in res)
                {
                    defaultduration = item.Duration;
                }
                if (row.IsSelected || (defaultduration != row.Duration && (defaultduration > 0 || row.Duration > 0)))//row.IsSelected &&
                {
                    data = new PrintTestData();
                    data.TestId = (int)row.ID;
                    data.Duration = (int)row.Duration;
                    data.PrintProtocol = (PrintProtocolType)Enum.Parse(typeof(PrintProtocolType), row.PrintProtocol);
                    selectedTests.Add(data);
                }
                defaultduration = 0;
            }

            return selectedTests;
        }

        /// <summary>
        /// Selects the specified test case on the data grid.
        /// </summary>
        /// <param name="testId">test case id to be selected</param>
        private void SelectTestCase(int testId)
        {
            // walk thru the data grid each row and select the matching row
            // match the row with the test ID
            foreach (CTCTestCaseDetailsDataSet.TestCaseDetailsRow row in cTCTestCaseDetails.TestCaseDetails.Rows)
            {
                if (row.ID == testId)
                {
                    row.IsSelected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Toggles (all) the test case selection
        /// </summary>
        /// <param name="state"></param>
        private void ChangeSelection(bool state)
        {
            // walk thru the test case details in grid and set the selection state
            // the reason we are walking thru the grid is it will have filtered records.
            foreach (DataGridViewRow row in testCaseDetails_DataGrid.Rows)
            {
                row.Cells[0].Value = state;
            }
            testCaseDetails_DataGrid.RefreshEdit();
        }

        /// <summary>
        /// Returns true if all the tests are selected on the Grid (visible tests)
        /// </summary>
        /// <returns></returns>
        private bool IsAllTestsSelected()
        {
            foreach (DataGridViewRow row in testCaseDetails_DataGrid.Rows)
            {
                if (!Boolean.Parse(row.Cells[0].Value.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the selectAll check box to the data gridview
        /// </summary>
        private void AddHeaderCheckBox()
        {
            selectAll_CheckBox = new CheckBox();
            selectAll_CheckBox.Size = new Size(15, 15);
            //Add the CheckBox into the DataGridView
            this.testCaseDetails_DataGrid.Controls.Add(selectAll_CheckBox);
            //subscribe the events
            this.selectAll_CheckBox.CheckedChanged += selectAll_CheckBox_CheckedChanged;
            testCaseDetails_DataGrid.CellPainting += testCaseDetails_DataGrid_CellPainting;
        }

        /// <summary>
        /// Reset the header checkbox location
        /// </summary>
        /// <param name="ColumnIndex">column index where the header check box is added</param>
        /// <param name="RowIndex">row index where the header check box is added</param>
        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.testCaseDetails_DataGrid.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - selectAll_CheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - selectAll_CheckBox.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            selectAll_CheckBox.Location = oPoint;
        }

        /// <summary>
        /// Change the position of a given row
        /// </summary>
        /// <param name="position">new position</param>
        /// <param name="dataRow">row to be moved</param>
        private void ChangePosition(int position, DataGridViewRow dataRow)
        {
            if (null == dataRow)
            {
                return;
            }

            CTCTestCaseDetailsDataSet.TestCaseDetailsDataTable dtable = cTCTestCaseDetails.TestCaseDetails;
            DataRow row = ((DataRowView)dataRow.DataBoundItem).Row;
            DataRow newRow = cTCTestCaseDetails.TestCaseDetails.NewTestCaseDetailsRow();
            newRow.ItemArray = row.ItemArray;

            if (position == -1 || position == (testCaseDetails_DataGrid.Rows.Count - 1))
            {
                position += 1;
            }

            dtable.Rows.InsertAt(newRow, position);
            testCaseDetails_DataGrid.Rows[position].Selected = true;
            testCaseDetails_DataGrid.CurrentCell = testCaseDetails_DataGrid.Rows[position].Cells[0];
            testCaseDetails_DataGrid.DataBindingComplete -= testCaseDetails_DataGrid_DataBindingComplete;
            dtable.Rows.Remove(row);
            testCaseDetails_DataGrid.DataBindingComplete += testCaseDetails_DataGrid_DataBindingComplete;
            dtable.AcceptChanges();
        }

        /// <summary>
        /// Move the position of a given row
        /// </summary>
        /// <param name="direction">direction of movement</param>
        /// <param name="dataRow">row to be moved</param>
        private void MoveRow(DataGridViewRow dataRow, RowMovement direction)
        {
            // Perform no operation when no rows are selected
            if (null == dataRow)
            {
                return;
            }

            CTCTestCaseDetailsDataSet.TestCaseDetailsDataTable dtable = cTCTestCaseDetails.TestCaseDetails;
            DataRow currentRow = ((DataRowView)dataRow.DataBoundItem).Row;
            DataRow newRow = cTCTestCaseDetails.TestCaseDetails.NewTestCaseDetailsRow();
            newRow.ItemArray = currentRow.ItemArray;
            int newRowIndex = 0;

            if (direction == RowMovement.MoveDown)
            {
                newRowIndex = (dataRow.Index + 2) > testCaseDetails_DataGrid.Rows.Count ? testCaseDetails_DataGrid.Rows.Count : (dataRow.Index + 2);
            }
            else if (direction == RowMovement.MoveUp)
            {
                newRowIndex = (dataRow.Index - 1) < 0 ? 0 : (dataRow.Index - 1);
            }
            else if (direction == RowMovement.MoveBottom)
            {
                newRowIndex = testCaseDetails_DataGrid.Rows.Count;
            }

            dtable.Rows.InsertAt(newRow, newRowIndex);
            testCaseDetails_DataGrid.Rows[newRowIndex].Selected = true;
            testCaseDetails_DataGrid.CurrentCell = testCaseDetails_DataGrid.Rows[newRowIndex].Cells[0];
            testCaseDetails_DataGrid.DataBindingComplete -= testCaseDetails_DataGrid_DataBindingComplete;
            dtable.Rows.Remove(currentRow);
            testCaseDetails_DataGrid.DataBindingComplete += testCaseDetails_DataGrid_DataBindingComplete;
            dtable.AcceptChanges();
        }

        /// <summary>
        /// Tells whether the control is in design mode or not
        /// </summary>
        /// <returns>True if it is in Design mode else false</returns>
        private static bool IsInDesignMode()
        {
            if (Application.ExecutablePath.IndexOf("devenv.exe", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the header Checkbox(selectAll_CheckBox) state based on the number of tests selected.
        /// If no rows are selected, set the header check box state to' Unchecked',set state to 'Intermediate/Tristate' when only few rows are selected.
        /// Set the state to 'Checked' when all rows are selected.
        /// </summary>
        private void SetCheckBoxState()
        {
            int selectedRowsCount = 0;

            foreach (DataGridViewRow row in testCaseDetails_DataGrid.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    selectedRowsCount++;
                }
            }

            if (selectedRowsCount == 0)
            {
                selectAll_CheckBox.CheckState = CheckState.Unchecked;
            }
            else if (selectedRowsCount < testCaseDetails_DataGrid.Rows.Count)
            {
                selectAll_CheckBox.CheckState = CheckState.Indeterminate;
            }
            else
            {
                selectAll_CheckBox.CheckState = CheckState.Checked;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Set the data grid filter status to the label to display to the user
        /// eg: 1 of 25 tests found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testCaseDetails_DataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(testCaseDetails_DataGrid);

            if (string.IsNullOrEmpty(filterStatus))
            {
                filterStatus_Label.Visible = false;
                moveButtons_Panel.Enabled = true;
            }
            else
            {
                filterStatus_Label.Visible = true;
                filterStatus_Label.Text = filterStatus.Replace("records", "tests");
                moveButtons_Panel.Enabled = false;
            }

            // Sets the header check box state based on the selected rows count.
            this.selectAll_CheckBox.CheckedChanged -= selectAll_CheckBox_CheckedChanged;
            SetCheckBoxState();
            this.selectAll_CheckBox.CheckedChanged += selectAll_CheckBox_CheckedChanged;

            // Set the show selected/show all hyper link based on the ui grid selection status			
            showSelected_LinkLabel.Text = "Show Selected";
            //Selected Test Count
            lblSelectedTests.Text = "{0} of {1} tests selected".FormatWith(SelectedTests.Count.ToString(CultureInfo.CurrentCulture), TestCaseDetails.Rows.Count, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// // Displays the drop-down list when the user presses ALT + DOWN or ALT + UP arrow keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testCaseDetails_DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell =
                    testCaseDetails_DataGrid.CurrentCell.OwningColumn.HeaderCell as
                    DataGridViewAutoFilterColumnHeaderCell;

                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Displays the selected tests based on the Link label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (showSelected_LinkLabel.Text == "Show Selected")
            {
                ShowTests(true);
                showSelected_LinkLabel.Text = "Show All";
                moveButtons_Panel.Enabled = false;
            }
            else
            {
                ShowTests(false);
                showSelected_LinkLabel.Text = "Show Selected";
                moveButtons_Panel.Enabled = true;
            }
        }

        /// <summary>
        /// Shows Selected tests or all the tests based on the link text
        /// </summary>
        /// <param name="selected">shows selected tests if true, else shows all tests</param>
        private void ShowTests(bool selected)
        {
            //Removes the filter
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(testCaseDetails_DataGrid);

            // Since the control is bound to a dataset, suspend the binding on all the rows in the currency manager before setting them to invisible:
            CurrencyManager bindingCurrencyManager = (CurrencyManager)BindingContext[testCaseDetails_DataGrid.DataSource];

            if (selected)
            {
                foreach (DataGridViewRow row in testCaseDetails_DataGrid.Rows)
                {

                    if ((bool)row.Cells[0].Value == true)
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        //Suspend the databinding
                        bindingCurrencyManager.SuspendBinding();
                        row.Visible = false;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in testCaseDetails_DataGrid.Rows)
                {
                    row.Visible = true;
                }
            }

            //Resume the databinding
            bindingCurrencyManager.ResumeBinding();
        }

        /// <summary>
        /// Update the products based on the product category
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        void productCategory_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            productName_ComboBox.Items.Clear();

            string productCategory = productCategory_ComboBox.SelectedItem.ToString();

            Collection<string> productNames = CtcSettings.GetProducts(productCategory);
            if (productNames.Count != 0)
            {
                for (int i = 0; i < productNames.Count; i++)
                {
                    productName_ComboBox.Items.Add(productNames[i]);
                }
            }
            else
            {
                MessageBox.Show("There are no Products available for {0} Product Category in the database.\nPlease add the Product and try again.".FormatWith(productCategory), "Product Loading Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            productName_ComboBox.SelectedIndex = 0;

            LoadTestCases(_type);

            OnPropertyChanged(new PropertyChangedEventArgs(ProductCategory.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testCaseDetails_DataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_pluginType.Equals(PluginType.Print))
            {
                // validate only if it is duration column
                if (e.ColumnIndex == 8)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()) || string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                    {
                        MessageBox.Show("Duration can not be empty", "Print Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                    else
                    {
                        string formattedValue = e.FormattedValue.ToString();
                        int duration = -1;
                        if (int.TryParse(formattedValue, out duration))
                        {
                            if (duration < 0 || duration > 120)
                            {
                                MessageBox.Show("Enter valid value in duration column between 0 to 120 \n\n 0 means it will print at least once.", "Print Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter valid value in duration column between 0 to 120 \n\n 0 means it will print at least once.", "Print Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// On Change of Product Name raise the event to all the subscribers
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        private void productName_ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (null != OnProductNameChanged)
            {
                OnProductNameChanged(productCategory_ComboBox.SelectedItem.ToString(), productName_ComboBox.SelectedItem.ToString());
            }

            OnPropertyChanged(new PropertyChangedEventArgs(ProductName));
        }

        /// <summary>
        /// Occurs on cell Painting event of the datagridview
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        void testCaseDetails_DataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
            }
        }

        /// <summary>
        /// Occurs on the state change of a checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void selectAll_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox selectCheckBox = (CheckBox)sender;
            if (selectCheckBox.Checked)
            {
                SelectAllTests();
            }
            else
            {
                DeselectAllTests();
            }
        }

        private void productName_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != OnProductNameChanged)
            {
                OnProductNameChanged(productCategory_ComboBox.SelectedItem.ToString(), productName_ComboBox.SelectedItem.ToString());
            }

            OnPropertyChanged(new PropertyChangedEventArgs(ProductName));
        }

        private void testCaseDetails_DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            lblSelectedTests.Text = "{0} of {1} tests selected".FormatWith(SelectedTests.Count.ToString(CultureInfo.CurrentCulture), TestCaseDetails.Rows.Count);
            OnPropertyChanged(new PropertyChangedEventArgs("Testcase Number"));
        }

        /// <summary>
        /// Clears all the filters and selections on the tests
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void clearAll_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Removes the filter
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(testCaseDetails_DataGrid);
            selectAll_CheckBox.CheckState = CheckState.Unchecked;

            if (showSelected_LinkLabel.Text == "Show All")
            {
                ShowTests(false);
            }

            ChangeSelection(false);
            moveButtons_Panel.Enabled = true;
        }

        /// <summary>
        /// Moves current rows up or down based on the movement directions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void move_Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            DataGridViewRow rowToMove = testCaseDetails_DataGrid.CurrentRow;

            if (clickedButton.Name == down_Button.Name)
            {
                MoveRow(rowToMove, RowMovement.MoveDown);
            }
            else if (clickedButton.Name == up_Button.Name)
            {
                MoveRow(rowToMove, RowMovement.MoveUp);
            }
            else if (clickedButton.Name == top_Button.Name)
            {
                MoveRow(rowToMove, RowMovement.MoveTop);
            }
            else
            {
                MoveRow(rowToMove, RowMovement.MoveBottom);
            }
        }

        /// <summary>
        /// Occurs when a property is changed.
        /// </summary>
        /// <param name="e"><see cref="PropertyChangedEventArgs"/></param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

    }

    /// <summary>
    /// TestData stores duration and print protocol information of the test cases
    /// </summary>
    [DataContract]
    public class PrintTestData
    {
        /// <summary>
        /// Test ID of a test cases
        /// </summary>
        [DataMember]
        public int TestId { get; set; }

        /// <summary>
        /// print duration for the test case
        /// </summary>
        [DataMember]
        public int Duration { get; set; }

        /// <summary>
        /// Protocol for each test case
        /// </summary>
        [DataMember]
        public PrintProtocolType PrintProtocol { get; set; }
    }
}
