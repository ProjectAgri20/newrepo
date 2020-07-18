namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    partial class QuickSetControl
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
            this.add_Button = new System.Windows.Forms.Button();
            this.delete_Button = new System.Windows.Forms.Button();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.originalSize_ComboBox = new System.Windows.Forms.ComboBox();
            this.originalSides_ComboBox = new System.Windows.Forms.ComboBox();
            this.fileType_ComboBox = new System.Windows.Forms.ComboBox();
            this.imagePreview_ComboBox = new System.Windows.Forms.ComboBox();
            this.contentOrientation_ComboBox = new System.Windows.Forms.ComboBox();
            this.resolution_ComboBox = new System.Windows.Forms.ComboBox();
            this.quickSetType_ComboBox = new System.Windows.Forms.ComboBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.originalSize_Label = new System.Windows.Forms.Label();
            this.originalSides_Label = new System.Windows.Forms.Label();
            this.orientation_Label = new System.Windows.Forms.Label();
            this.imagePreview_Label = new System.Windows.Forms.Label();
            this.fileType_Label = new System.Windows.Forms.Label();
            this.resolution_Label = new System.Windows.Forms.Label();
            this.quickSet_Label = new System.Windows.Forms.Label();
            this.info_Label1 = new System.Windows.Forms.Label();
            this.quickSet_GridView = new System.Windows.Forms.DataGridView();
            this.quickSetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quickSetTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quickSetTableDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.save_changes = new System.Windows.Forms.Button();
            this.parameter_GridView = new System.Windows.Forms.DataGridView();
            this.parameterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValidValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quickSetParameterDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.quickSet_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickSetTableDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parameter_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickSetParameterDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // add_Button
            // 
            this.add_Button.Location = new System.Drawing.Point(479, 191);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(91, 23);
            this.add_Button.TabIndex = 2;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // delete_Button
            // 
            this.delete_Button.Location = new System.Drawing.Point(588, 218);
            this.delete_Button.Name = "delete_Button";
            this.delete_Button.Size = new System.Drawing.Size(75, 23);
            this.delete_Button.TabIndex = 3;
            this.delete_Button.Text = "Delete";
            this.delete_Button.UseVisualStyleBackColor = true;
            this.delete_Button.Click += new System.EventHandler(this.delete_Button_Click);
            // 
            // name_TextBox
            // 
            this.name_TextBox.Location = new System.Drawing.Point(113, 194);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(121, 20);
            this.name_TextBox.TabIndex = 4;
            // 
            // originalSize_ComboBox
            // 
            this.originalSize_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originalSize_ComboBox.FormattingEnabled = true;
            this.originalSize_ComboBox.Location = new System.Drawing.Point(113, 220);
            this.originalSize_ComboBox.Name = "originalSize_ComboBox";
            this.originalSize_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.originalSize_ComboBox.TabIndex = 5;
            // 
            // originalSides_ComboBox
            // 
            this.originalSides_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originalSides_ComboBox.FormattingEnabled = true;
            this.originalSides_ComboBox.Location = new System.Drawing.Point(113, 247);
            this.originalSides_ComboBox.Name = "originalSides_ComboBox";
            this.originalSides_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.originalSides_ComboBox.TabIndex = 6;
            // 
            // fileType_ComboBox
            // 
            this.fileType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType_ComboBox.FormattingEnabled = true;
            this.fileType_ComboBox.Location = new System.Drawing.Point(113, 328);
            this.fileType_ComboBox.Name = "fileType_ComboBox";
            this.fileType_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.fileType_ComboBox.TabIndex = 7;
            // 
            // imagePreview_ComboBox
            // 
            this.imagePreview_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imagePreview_ComboBox.FormattingEnabled = true;
            this.imagePreview_ComboBox.Location = new System.Drawing.Point(113, 301);
            this.imagePreview_ComboBox.Name = "imagePreview_ComboBox";
            this.imagePreview_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.imagePreview_ComboBox.TabIndex = 8;
            // 
            // contentOrientation_ComboBox
            // 
            this.contentOrientation_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contentOrientation_ComboBox.FormattingEnabled = true;
            this.contentOrientation_ComboBox.Location = new System.Drawing.Point(113, 274);
            this.contentOrientation_ComboBox.Name = "contentOrientation_ComboBox";
            this.contentOrientation_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.contentOrientation_ComboBox.TabIndex = 9;
            // 
            // resolution_ComboBox
            // 
            this.resolution_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolution_ComboBox.FormattingEnabled = true;
            this.resolution_ComboBox.Location = new System.Drawing.Point(113, 355);
            this.resolution_ComboBox.Name = "resolution_ComboBox";
            this.resolution_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.resolution_ComboBox.TabIndex = 10;
            // 
            // quickSetType_ComboBox
            // 
            this.quickSetType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.quickSetType_ComboBox.FormattingEnabled = true;
            this.quickSetType_ComboBox.Location = new System.Drawing.Point(352, 193);
            this.quickSetType_ComboBox.Name = "quickSetType_ComboBox";
            this.quickSetType_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.quickSetType_ComboBox.TabIndex = 11;
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(25, 197);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(82, 13);
            this.name_Label.TabIndex = 12;
            this.name_Label.Text = "QuickSet Name";
            // 
            // originalSize_Label
            // 
            this.originalSize_Label.AutoSize = true;
            this.originalSize_Label.Location = new System.Drawing.Point(25, 223);
            this.originalSize_Label.Name = "originalSize_Label";
            this.originalSize_Label.Size = new System.Drawing.Size(58, 13);
            this.originalSize_Label.TabIndex = 13;
            this.originalSize_Label.Text = "Paper Size";
            // 
            // originalSides_Label
            // 
            this.originalSides_Label.AutoSize = true;
            this.originalSides_Label.Location = new System.Drawing.Point(25, 250);
            this.originalSides_Label.Name = "originalSides_Label";
            this.originalSides_Label.Size = new System.Drawing.Size(61, 13);
            this.originalSides_Label.TabIndex = 14;
            this.originalSides_Label.Text = "Page Sides";
            // 
            // orientation_Label
            // 
            this.orientation_Label.AutoSize = true;
            this.orientation_Label.Location = new System.Drawing.Point(25, 277);
            this.orientation_Label.Name = "orientation_Label";
            this.orientation_Label.Size = new System.Drawing.Size(58, 13);
            this.orientation_Label.TabIndex = 15;
            this.orientation_Label.Text = "Orientation";
            // 
            // imagePreview_Label
            // 
            this.imagePreview_Label.AutoSize = true;
            this.imagePreview_Label.Location = new System.Drawing.Point(25, 304);
            this.imagePreview_Label.Name = "imagePreview_Label";
            this.imagePreview_Label.Size = new System.Drawing.Size(77, 13);
            this.imagePreview_Label.TabIndex = 16;
            this.imagePreview_Label.Text = "Image Preview";
            // 
            // fileType_Label
            // 
            this.fileType_Label.AutoSize = true;
            this.fileType_Label.Location = new System.Drawing.Point(25, 331);
            this.fileType_Label.Name = "fileType_Label";
            this.fileType_Label.Size = new System.Drawing.Size(50, 13);
            this.fileType_Label.TabIndex = 17;
            this.fileType_Label.Text = "File Type";
            // 
            // resolution_Label
            // 
            this.resolution_Label.AutoSize = true;
            this.resolution_Label.Location = new System.Drawing.Point(25, 358);
            this.resolution_Label.Name = "resolution_Label";
            this.resolution_Label.Size = new System.Drawing.Size(57, 13);
            this.resolution_Label.TabIndex = 18;
            this.resolution_Label.Text = "Resolution";
            // 
            // quickSet_Label
            // 
            this.quickSet_Label.AutoSize = true;
            this.quickSet_Label.Location = new System.Drawing.Point(268, 197);
            this.quickSet_Label.Name = "quickSet_Label";
            this.quickSet_Label.Size = new System.Drawing.Size(78, 13);
            this.quickSet_Label.TabIndex = 19;
            this.quickSet_Label.Text = "QuickSet Type";
            // 
            // info_Label1
            // 
            this.info_Label1.AutoSize = true;
            this.info_Label1.Location = new System.Drawing.Point(349, 231);
            this.info_Label1.Name = "info_Label1";
            this.info_Label1.Size = new System.Drawing.Size(109, 13);
            this.info_Label1.TabIndex = 21;
            this.info_Label1.Text = "Additional Parameters";
            // 
            // quickSet_GridView
            // 
            this.quickSet_GridView.AllowUserToAddRows = false;
            this.quickSet_GridView.AllowUserToDeleteRows = false;
            this.quickSet_GridView.AllowUserToResizeColumns = false;
            this.quickSet_GridView.AutoGenerateColumns = false;
            this.quickSet_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.quickSet_GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.quickSetNameDataGridViewTextBoxColumn,
            this.quickSetTypeDataGridViewTextBoxColumn});
            this.quickSet_GridView.DataSource = this.quickSetTableDataBindingSource;
            this.quickSet_GridView.Location = new System.Drawing.Point(14, 15);
            this.quickSet_GridView.Name = "quickSet_GridView";
            this.quickSet_GridView.ReadOnly = true;
            this.quickSet_GridView.Size = new System.Drawing.Size(469, 150);
            this.quickSet_GridView.TabIndex = 22;
            // 
            // quickSetNameDataGridViewTextBoxColumn
            // 
            this.quickSetNameDataGridViewTextBoxColumn.DataPropertyName = "QuickSetName";
            this.quickSetNameDataGridViewTextBoxColumn.HeaderText = "QuickSetName";
            this.quickSetNameDataGridViewTextBoxColumn.Name = "quickSetNameDataGridViewTextBoxColumn";
            this.quickSetNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.quickSetNameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.quickSetNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // quickSetTypeDataGridViewTextBoxColumn
            // 
            this.quickSetTypeDataGridViewTextBoxColumn.DataPropertyName = "QuickSetType";
            this.quickSetTypeDataGridViewTextBoxColumn.HeaderText = "QuickSetType";
            this.quickSetTypeDataGridViewTextBoxColumn.Name = "quickSetTypeDataGridViewTextBoxColumn";
            this.quickSetTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.quickSetTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.quickSetTypeDataGridViewTextBoxColumn.Width = 200;
            // 
            // quickSetTableDataBindingSource
            // 
            this.quickSetTableDataBindingSource.DataSource = typeof(HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.QuickSetTableData);
            // 
            // save_changes
            // 
            this.save_changes.Enabled = false;
            this.save_changes.Location = new System.Drawing.Point(479, 218);
            this.save_changes.Name = "save_changes";
            this.save_changes.Size = new System.Drawing.Size(91, 23);
            this.save_changes.TabIndex = 23;
            this.save_changes.Text = "Save Changes";
            this.save_changes.UseVisualStyleBackColor = true;
            this.save_changes.Click += new System.EventHandler(this.save_changes_Click);
            // 
            // parameter_GridView
            // 
            this.parameter_GridView.AllowUserToAddRows = false;
            this.parameter_GridView.AllowUserToDeleteRows = false;
            this.parameter_GridView.AllowUserToResizeColumns = false;
            this.parameter_GridView.AutoGenerateColumns = false;
            this.parameter_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parameter_GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parameterNameDataGridViewTextBoxColumn,
            this.parameterValueDataGridViewTextBoxColumn,
            this.ValidValues});
            this.parameter_GridView.DataSource = this.quickSetParameterDataBindingSource;
            this.parameter_GridView.Location = new System.Drawing.Point(271, 250);
            this.parameter_GridView.Name = "parameter_GridView";
            this.parameter_GridView.Size = new System.Drawing.Size(392, 154);
            this.parameter_GridView.TabIndex = 24;
            // 
            // parameterNameDataGridViewTextBoxColumn
            // 
            this.parameterNameDataGridViewTextBoxColumn.DataPropertyName = "ParameterName";
            this.parameterNameDataGridViewTextBoxColumn.HeaderText = "Parameter";
            this.parameterNameDataGridViewTextBoxColumn.Name = "parameterNameDataGridViewTextBoxColumn";
            this.parameterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.parameterNameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // parameterValueDataGridViewTextBoxColumn
            // 
            this.parameterValueDataGridViewTextBoxColumn.DataPropertyName = "ParameterValue";
            this.parameterValueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.parameterValueDataGridViewTextBoxColumn.Name = "parameterValueDataGridViewTextBoxColumn";
            this.parameterValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parameterValueDataGridViewTextBoxColumn.Width = 140;
            // 
            // ValidValues
            // 
            this.ValidValues.DataPropertyName = "ValidValues";
            this.ValidValues.HeaderText = "Valid Values";
            this.ValidValues.Name = "ValidValues";
            this.ValidValues.ReadOnly = true;
            this.ValidValues.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ValidValues.Width = 250;
            // 
            // quickSetParameterDataBindingSource
            // 
            this.quickSetParameterDataBindingSource.DataSource = typeof(HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.QuickSetParameterData);
            // 
            // QuickSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parameter_GridView);
            this.Controls.Add(this.save_changes);
            this.Controls.Add(this.quickSet_GridView);
            this.Controls.Add(this.info_Label1);
            this.Controls.Add(this.quickSet_Label);
            this.Controls.Add(this.resolution_Label);
            this.Controls.Add(this.fileType_Label);
            this.Controls.Add(this.imagePreview_Label);
            this.Controls.Add(this.orientation_Label);
            this.Controls.Add(this.originalSides_Label);
            this.Controls.Add(this.originalSize_Label);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.quickSetType_ComboBox);
            this.Controls.Add(this.resolution_ComboBox);
            this.Controls.Add(this.contentOrientation_ComboBox);
            this.Controls.Add(this.imagePreview_ComboBox);
            this.Controls.Add(this.fileType_ComboBox);
            this.Controls.Add(this.originalSides_ComboBox);
            this.Controls.Add(this.originalSize_ComboBox);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.delete_Button);
            this.Controls.Add(this.add_Button);
            this.Name = "QuickSetControl";
            this.Size = new System.Drawing.Size(673, 478);
            ((System.ComponentModel.ISupportInitialize)(this.quickSet_GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickSetTableDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parameter_GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quickSetParameterDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button delete_Button;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.ComboBox originalSize_ComboBox;
        private System.Windows.Forms.ComboBox originalSides_ComboBox;
        private System.Windows.Forms.ComboBox fileType_ComboBox;
        private System.Windows.Forms.ComboBox imagePreview_ComboBox;
        private System.Windows.Forms.ComboBox contentOrientation_ComboBox;
        private System.Windows.Forms.ComboBox resolution_ComboBox;
        private System.Windows.Forms.ComboBox quickSetType_ComboBox;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.Label originalSize_Label;
        private System.Windows.Forms.Label originalSides_Label;
        private System.Windows.Forms.Label orientation_Label;
        private System.Windows.Forms.Label imagePreview_Label;
        private System.Windows.Forms.Label fileType_Label;
        private System.Windows.Forms.Label resolution_Label;
        private System.Windows.Forms.Label quickSet_Label;
        private System.Windows.Forms.Label info_Label1;
        private System.Windows.Forms.DataGridView quickSet_GridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn quickSetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quickSetTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource quickSetTableDataBindingSource;
        private System.Windows.Forms.Button save_changes;
        private System.Windows.Forms.DataGridView parameter_GridView;
        private System.Windows.Forms.BindingSource quickSetParameterDataBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValidValues;
    }
}
