namespace HP.ScalableTest.PluginSupport.Connectivity
{
    partial class CtcBaseConfigurationControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.testCaseDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.moveButtons_Panel = new System.Windows.Forms.Panel();
            this.bottom_Button = new System.Windows.Forms.Button();
            this.down_Button = new System.Windows.Forms.Button();
            this.up_Button = new System.Windows.Forms.Button();
            this.top_Button = new System.Windows.Forms.Button();
            this.clearAll_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.lblSelectedTests = new System.Windows.Forms.Label();
            this.showSelected_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.filterStatus_Label = new System.Windows.Forms.Label();
            this.testCaseDetails_DataGrid = new System.Windows.Forms.DataGridView();
            this.isSelectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iDDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.categoryDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.protocolDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.printProtocolDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.portNumberDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.connectivityDataGridViewTextBoxColumn = new HP.ScalableTest.PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn();
            this.durationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testCaseDetails_BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cTCTestCaseDetails = new HP.ScalableTest.PluginSupport.Connectivity.CTCTestCaseDetailsDataSet();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productCategory_Label = new System.Windows.Forms.Label();
            this.productCategory_ComboBox = new System.Windows.Forms.ComboBox();
            this.productName_ComboBox = new System.Windows.Forms.ComboBox();
            this.productName_Label = new System.Windows.Forms.Label();
            this.baseControl_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.iterations_Label = new System.Windows.Forms.Label();
            this.iterations_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.testCaseDetails_GroupBox.SuspendLayout();
            this.moveButtons_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testCaseDetails_DataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testCaseDetails_BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cTCTestCaseDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterations_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // testCaseDetails_GroupBox
            // 
            this.testCaseDetails_GroupBox.Controls.Add(this.moveButtons_Panel);
            this.testCaseDetails_GroupBox.Controls.Add(this.clearAll_LinkLabel);
            this.testCaseDetails_GroupBox.Controls.Add(this.lblSelectedTests);
            this.testCaseDetails_GroupBox.Controls.Add(this.showSelected_LinkLabel);
            this.testCaseDetails_GroupBox.Controls.Add(this.filterStatus_Label);
            this.testCaseDetails_GroupBox.Controls.Add(this.testCaseDetails_DataGrid);
            this.testCaseDetails_GroupBox.Location = new System.Drawing.Point(0, 33);
            this.testCaseDetails_GroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.testCaseDetails_GroupBox.Name = "testCaseDetails_GroupBox";
            this.testCaseDetails_GroupBox.Size = new System.Drawing.Size(723, 330);
            this.testCaseDetails_GroupBox.TabIndex = 2;
            this.testCaseDetails_GroupBox.TabStop = false;
            this.testCaseDetails_GroupBox.Text = "Test Case Details";
            // 
            // moveButtons_Panel
            // 
            this.moveButtons_Panel.Controls.Add(this.bottom_Button);
            this.moveButtons_Panel.Controls.Add(this.down_Button);
            this.moveButtons_Panel.Controls.Add(this.up_Button);
            this.moveButtons_Panel.Controls.Add(this.top_Button);
            this.moveButtons_Panel.Location = new System.Drawing.Point(7, 70);
            this.moveButtons_Panel.Name = "moveButtons_Panel";
            this.moveButtons_Panel.Size = new System.Drawing.Size(25, 131);
            this.moveButtons_Panel.TabIndex = 13;
            // 
            // bottom_Button
            // 
            this.bottom_Button.Location = new System.Drawing.Point(3, 92);
            this.bottom_Button.Name = "bottom_Button";
            this.bottom_Button.Size = new System.Drawing.Size(19, 23);
            this.bottom_Button.TabIndex = 11;
            this.baseControl_ToolTip.SetToolTip(this.bottom_Button, "Moves the selected test to the last position");
            this.bottom_Button.UseVisualStyleBackColor = true;
            this.bottom_Button.Click += new System.EventHandler(this.move_Button_Click);
            // 
            // down_Button
            // 
            this.down_Button.Location = new System.Drawing.Point(3, 66);
            this.down_Button.Name = "down_Button";
            this.down_Button.Size = new System.Drawing.Size(19, 23);
            this.down_Button.TabIndex = 10;
            this.baseControl_ToolTip.SetToolTip(this.down_Button, "Moves the selected test one level down");
            this.down_Button.UseVisualStyleBackColor = true;
            this.down_Button.Click += new System.EventHandler(this.move_Button_Click);
            // 
            // up_Button
            // 
            this.up_Button.Location = new System.Drawing.Point(3, 32);
            this.up_Button.Name = "up_Button";
            this.up_Button.Size = new System.Drawing.Size(19, 23);
            this.up_Button.TabIndex = 9;
            this.baseControl_ToolTip.SetToolTip(this.up_Button, "Moves the selected test one level up");
            this.up_Button.UseVisualStyleBackColor = true;
            this.up_Button.Click += new System.EventHandler(this.move_Button_Click);
            // 
            // top_Button
            // 
            this.top_Button.Location = new System.Drawing.Point(3, 5);
            this.top_Button.Name = "top_Button";
            this.top_Button.Size = new System.Drawing.Size(19, 23);
            this.top_Button.TabIndex = 8;
            this.baseControl_ToolTip.SetToolTip(this.top_Button, "Moves the selected test to first position");
            this.top_Button.UseVisualStyleBackColor = true;
            this.top_Button.Click += new System.EventHandler(this.move_Button_Click);
            // 
            // clearAll_LinkLabel
            // 
            this.clearAll_LinkLabel.AutoSize = true;
            this.clearAll_LinkLabel.Location = new System.Drawing.Point(127, 307);
            this.clearAll_LinkLabel.Name = "clearAll_LinkLabel";
            this.clearAll_LinkLabel.Size = new System.Drawing.Size(45, 13);
            this.clearAll_LinkLabel.TabIndex = 5;
            this.clearAll_LinkLabel.TabStop = true;
            this.clearAll_LinkLabel.Text = "Clear All";
            this.baseControl_ToolTip.SetToolTip(this.clearAll_LinkLabel, "Clears all the filters and selections");
            this.clearAll_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearAll_LinkLabel_LinkClicked);
            // 
            // lblSelectedTests
            // 
            this.lblSelectedTests.AutoSize = true;
            this.lblSelectedTests.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTests.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblSelectedTests.Location = new System.Drawing.Point(313, 307);
            this.lblSelectedTests.Name = "lblSelectedTests";
            this.lblSelectedTests.Size = new System.Drawing.Size(38, 13);
            this.lblSelectedTests.TabIndex = 4;
            this.lblSelectedTests.Text = "Label";
            // 
            // showSelected_LinkLabel
            // 
            this.showSelected_LinkLabel.AutoSize = true;
            this.showSelected_LinkLabel.Location = new System.Drawing.Point(32, 307);
            this.showSelected_LinkLabel.Name = "showSelected_LinkLabel";
            this.showSelected_LinkLabel.Size = new System.Drawing.Size(79, 13);
            this.showSelected_LinkLabel.TabIndex = 1;
            this.showSelected_LinkLabel.TabStop = true;
            this.showSelected_LinkLabel.Text = "Show Selected";
            this.showSelected_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.selectAll_LinkLabel_LinkClicked);
            // 
            // filterStatus_Label
            // 
            this.filterStatus_Label.AutoSize = true;
            this.filterStatus_Label.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.filterStatus_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterStatus_Label.ForeColor = System.Drawing.Color.Maroon;
            this.filterStatus_Label.Location = new System.Drawing.Point(543, 307);
            this.filterStatus_Label.Name = "filterStatus_Label";
            this.filterStatus_Label.Size = new System.Drawing.Size(38, 13);
            this.filterStatus_Label.TabIndex = 2;
            this.filterStatus_Label.Text = "Label";
            // 
            // testCaseDetails_DataGrid
            // 
            this.testCaseDetails_DataGrid.AllowDrop = true;
            this.testCaseDetails_DataGrid.AllowUserToAddRows = false;
            this.testCaseDetails_DataGrid.AllowUserToDeleteRows = false;
            this.testCaseDetails_DataGrid.AutoGenerateColumns = false;
            this.testCaseDetails_DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testCaseDetails_DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isSelectedDataGridViewCheckBoxColumn,
            this.iDDataGridViewTextBoxColumn,
            this.categoryDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.protocolDataGridViewTextBoxColumn,
            this.printProtocolDataGridViewTextBoxColumn,
            this.portNumberDataGridViewTextBoxColumn,
            this.connectivityDataGridViewTextBoxColumn,
            this.durationDataGridViewTextBoxColumn});
            this.testCaseDetails_DataGrid.DataSource = this.testCaseDetails_BindingSource;
            this.testCaseDetails_DataGrid.Location = new System.Drawing.Point(35, 19);
            this.testCaseDetails_DataGrid.MultiSelect = false;
            this.testCaseDetails_DataGrid.Name = "testCaseDetails_DataGrid";
            this.testCaseDetails_DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.testCaseDetails_DataGrid.Size = new System.Drawing.Size(682, 282);
            this.testCaseDetails_DataGrid.TabIndex = 0;
            this.testCaseDetails_DataGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.testCaseDetails_DataGrid_CellValidating);
            this.testCaseDetails_DataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCaseDetails_DataGrid_CellValueChanged);
            this.testCaseDetails_DataGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.testCaseDetails_DataGrid_DataBindingComplete);
            // 
            // isSelectedDataGridViewCheckBoxColumn
            // 
            this.isSelectedDataGridViewCheckBoxColumn.DataPropertyName = "IsSelected";
            this.isSelectedDataGridViewCheckBoxColumn.HeaderText = "";
            this.isSelectedDataGridViewCheckBoxColumn.Name = "isSelectedDataGridViewCheckBoxColumn";
            this.isSelectedDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.isSelectedDataGridViewCheckBoxColumn.Width = 50;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iDDataGridViewTextBoxColumn.Width = 75;
            // 
            // categoryDataGridViewTextBoxColumn
            // 
            this.categoryDataGridViewTextBoxColumn.DataPropertyName = "Category";
            this.categoryDataGridViewTextBoxColumn.HeaderText = "Category";
            this.categoryDataGridViewTextBoxColumn.Name = "categoryDataGridViewTextBoxColumn";
            this.categoryDataGridViewTextBoxColumn.ReadOnly = true;
            this.categoryDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 240;
            // 
            // protocolDataGridViewTextBoxColumn
            // 
            this.protocolDataGridViewTextBoxColumn.DataPropertyName = "Protocol";
            this.protocolDataGridViewTextBoxColumn.HeaderText = "Protocol";
            this.protocolDataGridViewTextBoxColumn.Name = "protocolDataGridViewTextBoxColumn";
            this.protocolDataGridViewTextBoxColumn.ReadOnly = true;
            this.protocolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.protocolDataGridViewTextBoxColumn.Width = 70;
            // 
            // printProtocolDataGridViewTextBoxColumn
            // 
            this.printProtocolDataGridViewTextBoxColumn.DataPropertyName = "PrintProtocol";
            this.printProtocolDataGridViewTextBoxColumn.HeaderText = "Print Protocol";
            this.printProtocolDataGridViewTextBoxColumn.Name = "printProtocolDataGridViewTextBoxColumn";
            this.printProtocolDataGridViewTextBoxColumn.ReadOnly = true;
            this.printProtocolDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.printProtocolDataGridViewTextBoxColumn.Visible = false;
            // 
            // portNumberDataGridViewTextBoxColumn
            // 
            this.portNumberDataGridViewTextBoxColumn.DataPropertyName = "PortNumber";
            this.portNumberDataGridViewTextBoxColumn.HeaderText = "Port Number";
            this.portNumberDataGridViewTextBoxColumn.Name = "portNumberDataGridViewTextBoxColumn";
            this.portNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // connectivityDataGridViewTextBoxColumn
            // 
            this.connectivityDataGridViewTextBoxColumn.DataPropertyName = "Connectivity";
            this.connectivityDataGridViewTextBoxColumn.HeaderText = "Connectivity";
            this.connectivityDataGridViewTextBoxColumn.Name = "connectivityDataGridViewTextBoxColumn";
            this.connectivityDataGridViewTextBoxColumn.ReadOnly = true;
            this.connectivityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.connectivityDataGridViewTextBoxColumn.Width = 95;
            // 
            // durationDataGridViewTextBoxColumn
            // 
            this.durationDataGridViewTextBoxColumn.DataPropertyName = "Duration";
            this.durationDataGridViewTextBoxColumn.HeaderText = "Duration (Minutes)";
            this.durationDataGridViewTextBoxColumn.Name = "durationDataGridViewTextBoxColumn";
            this.durationDataGridViewTextBoxColumn.Visible = false;
            // 
            // testCaseDetails_BindingSource
            // 
            this.testCaseDetails_BindingSource.DataMember = "TestCaseDetails";
            this.testCaseDetails_BindingSource.DataSource = this.cTCTestCaseDetails;
            // 
            // cTCTestCaseDetails
            // 
            this.cTCTestCaseDetails.DataSetName = "CTCTestCaseDetails";
            this.cTCTestCaseDetails.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsSelected";
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn1.HeaderText = "Description";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 300;
            // 
            // productCategory_Label
            // 
            this.productCategory_Label.AutoSize = true;
            this.productCategory_Label.Location = new System.Drawing.Point(4, 4);
            this.productCategory_Label.Name = "productCategory_Label";
            this.productCategory_Label.Size = new System.Drawing.Size(92, 13);
            this.productCategory_Label.TabIndex = 3;
            this.productCategory_Label.Text = "Product Category:";
            // 
            // productCategory_ComboBox
            // 
            this.productCategory_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productCategory_ComboBox.FormattingEnabled = true;
            this.productCategory_ComboBox.Location = new System.Drawing.Point(102, 0);
            this.productCategory_ComboBox.Name = "productCategory_ComboBox";
            this.productCategory_ComboBox.Size = new System.Drawing.Size(91, 21);
            this.productCategory_ComboBox.TabIndex = 0;
            this.baseControl_ToolTip.SetToolTip(this.productCategory_ComboBox, "Select product category");
            // 
            // productName_ComboBox
            // 
            this.productName_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productName_ComboBox.FormattingEnabled = true;
            this.productName_ComboBox.Location = new System.Drawing.Point(300, 0);
            this.productName_ComboBox.Name = "productName_ComboBox";
            this.productName_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.productName_ComboBox.TabIndex = 1;
            this.baseControl_ToolTip.SetToolTip(this.productName_ComboBox, "Select product name");
            this.productName_ComboBox.SelectedIndexChanged += new System.EventHandler(this.productName_ComboBox_SelectedIndexChanged);
            this.productName_ComboBox.SelectionChangeCommitted += new System.EventHandler(this.productName_ComboBox_SelectionChangeCommitted);
            // 
            // productName_Label
            // 
            this.productName_Label.AutoSize = true;
            this.productName_Label.Location = new System.Drawing.Point(216, 4);
            this.productName_Label.Name = "productName_Label";
            this.productName_Label.Size = new System.Drawing.Size(78, 13);
            this.productName_Label.TabIndex = 5;
            this.productName_Label.Text = "Product Name:";
            // 
            // iterations_Label
            // 
            this.iterations_Label.AutoSize = true;
            this.iterations_Label.Location = new System.Drawing.Point(453, 3);
            this.iterations_Label.Name = "iterations_Label";
            this.iterations_Label.Size = new System.Drawing.Size(87, 13);
            this.iterations_Label.TabIndex = 6;
            this.iterations_Label.Text = "No. Of Iterations:";
            this.iterations_Label.Visible = false;
            // 
            // iterations_NumericUpDown
            // 
            this.iterations_NumericUpDown.Location = new System.Drawing.Point(546, 0);
            this.iterations_NumericUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.iterations_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.iterations_NumericUpDown.Name = "iterations_NumericUpDown";
            this.iterations_NumericUpDown.Size = new System.Drawing.Size(38, 20);
            this.iterations_NumericUpDown.TabIndex = 7;
            this.iterations_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.iterations_NumericUpDown.Visible = false;
            // 
            // CtcBaseConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iterations_NumericUpDown);
            this.Controls.Add(this.iterations_Label);
            this.Controls.Add(this.productName_ComboBox);
            this.Controls.Add(this.productName_Label);
            this.Controls.Add(this.productCategory_ComboBox);
            this.Controls.Add(this.productCategory_Label);
            this.Controls.Add(this.testCaseDetails_GroupBox);
            this.Name = "CtcBaseConfigurationControl";
            this.Size = new System.Drawing.Size(723, 527);
            this.testCaseDetails_GroupBox.ResumeLayout(false);
            this.testCaseDetails_GroupBox.PerformLayout();
            this.moveButtons_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testCaseDetails_DataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testCaseDetails_BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cTCTestCaseDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterations_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.GroupBox testCaseDetails_GroupBox;
        private System.Windows.Forms.DataGridView testCaseDetails_DataGrid;
        private System.Windows.Forms.BindingSource testCaseDetails_BindingSource;
        private PluginSupport.Connectivity.CTCTestCaseDetailsDataSet cTCTestCaseDetails;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Label filterStatus_Label;
        private System.Windows.Forms.LinkLabel showSelected_LinkLabel;
        private System.Windows.Forms.Label productCategory_Label;
        private System.Windows.Forms.ComboBox productCategory_ComboBox;
        private System.Windows.Forms.ComboBox productName_ComboBox;
        private System.Windows.Forms.Label productName_Label;
        private System.Windows.Forms.ToolTip baseControl_ToolTip;
		private System.Windows.Forms.Label lblSelectedTests;
        private System.Windows.Forms.Label iterations_Label;
        private System.Windows.Forms.NumericUpDown iterations_NumericUpDown;
        private System.Windows.Forms.LinkLabel clearAll_LinkLabel;
        private System.Windows.Forms.Panel moveButtons_Panel;
        private System.Windows.Forms.Button bottom_Button;
        private System.Windows.Forms.Button down_Button;
        private System.Windows.Forms.Button up_Button;
        private System.Windows.Forms.Button top_Button;
		private System.Windows.Forms.DataGridViewCheckBoxColumn isSelectedDataGridViewCheckBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn iDDataGridViewTextBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn categoryDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn protocolDataGridViewTextBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn printProtocolDataGridViewTextBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn portNumberDataGridViewTextBoxColumn;
		private PluginSupport.Connectivity.UI.DataGridViewAutoFilterTextBoxColumn connectivityDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn durationDataGridViewTextBoxColumn;
        public Framework.UI.FieldValidator fieldValidator;
    }
}
