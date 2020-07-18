namespace HP.ScalableTest.Framework.UI
{
    partial class ServerComboBox
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.servers_MultiColumnComboBox = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox.EditorControl.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // servers_MultiColumnComboBox
            // 
            this.servers_MultiColumnComboBox.AutoSizeDropDownHeight = true;
            this.servers_MultiColumnComboBox.AutoSizeDropDownToBestFit = true;
            this.servers_MultiColumnComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servers_MultiColumnComboBox.DropDownSizingMode = Telerik.WinControls.UI.SizingMode.RightBottom;
            this.servers_MultiColumnComboBox.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // servers_MultiColumnComboBox.NestedRadGridView
            // 
            this.servers_MultiColumnComboBox.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.servers_MultiColumnComboBox.EditorControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servers_MultiColumnComboBox.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.servers_MultiColumnComboBox.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.AllowColumnChooser = false;
            gridViewTextBoxColumn1.FieldName = "HostName";
            gridViewTextBoxColumn1.HeaderText = "Host Name";
            gridViewTextBoxColumn1.MaxWidth = 110;
            gridViewTextBoxColumn1.MinWidth = 110;
            gridViewTextBoxColumn1.Name = "hostName_GridViewColumn";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 110;
            gridViewTextBoxColumn2.FieldName = "Architecture";
            gridViewTextBoxColumn2.HeaderText = "Arch";
            gridViewTextBoxColumn2.MaxWidth = 30;
            gridViewTextBoxColumn2.MinWidth = 30;
            gridViewTextBoxColumn2.Name = "architecture_GridViewColumn";
            gridViewTextBoxColumn2.Width = 30;
            gridViewTextBoxColumn3.FieldName = "Stats";
            gridViewTextBoxColumn3.HeaderText = "Stats";
            gridViewTextBoxColumn3.MaxWidth = 80;
            gridViewTextBoxColumn3.MinWidth = 80;
            gridViewTextBoxColumn3.Name = "stats_GridViewColumn";
            gridViewTextBoxColumn3.Width = 80;
            gridViewTextBoxColumn4.FieldName = "OperatingSystem";
            gridViewTextBoxColumn4.HeaderText = "Operating System";
            gridViewTextBoxColumn4.MaxWidth = 500;
            gridViewTextBoxColumn4.MinWidth = 250;
            gridViewTextBoxColumn4.Name = "operatingSystem_GridViewColumn";
            gridViewTextBoxColumn4.Width = 250;
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.EnableGrouping = false;
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.ShowFilteringRow = false;
            sortDescriptor1.PropertyName = "hostName_GridViewColumn";
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.servers_MultiColumnComboBox.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.servers_MultiColumnComboBox.EditorControl.Name = "NestedRadGridView";
            this.servers_MultiColumnComboBox.EditorControl.ReadOnly = true;
            this.servers_MultiColumnComboBox.EditorControl.ShowGroupPanel = false;
            this.servers_MultiColumnComboBox.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.servers_MultiColumnComboBox.EditorControl.TabIndex = 0;
            this.servers_MultiColumnComboBox.Location = new System.Drawing.Point(0, 0);
            this.servers_MultiColumnComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.servers_MultiColumnComboBox.Name = "servers_MultiColumnComboBox";
            this.servers_MultiColumnComboBox.Size = new System.Drawing.Size(210, 23);
            this.servers_MultiColumnComboBox.TabIndex = 0;
            this.servers_MultiColumnComboBox.TabStop = false;
            // 
            // ServerComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servers_MultiColumnComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServerComboBox";
            this.Size = new System.Drawing.Size(210, 23);
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_MultiColumnComboBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadMultiColumnComboBox servers_MultiColumnComboBox;
    }
}
