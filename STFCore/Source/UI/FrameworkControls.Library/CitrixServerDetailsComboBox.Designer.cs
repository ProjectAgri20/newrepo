namespace HP.ScalableTest.UI.Framework
{
    partial class CitrixServerDetailsComboBox
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.servers_ComboBox = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox.EditorControl.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // servers_ComboBox
            // 
            this.servers_ComboBox.AutoSizeDropDownHeight = true;
            this.servers_ComboBox.AutoSizeDropDownToBestFit = true;
            this.servers_ComboBox.DisplayMember = "HostName";
            this.servers_ComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servers_ComboBox.DropDownSizingMode = Telerik.WinControls.UI.SizingMode.RightBottom;
            this.servers_ComboBox.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // servers_ComboBox.NestedRadGridView
            // 
            this.servers_ComboBox.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.servers_ComboBox.EditorControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.servers_ComboBox.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.servers_ComboBox.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.servers_ComboBox.EditorControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.servers_ComboBox.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowColumnReorder = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowDeleteRow = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowDragToGroup = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowEditRow = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AllowRowResize = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.servers_ComboBox.EditorControl.MasterTemplate.EnableAlternatingRowColor = true;
            this.servers_ComboBox.EditorControl.MasterTemplate.EnableGrouping = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.ShowColumnHeaders = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.ShowRowHeaderColumn = false;
            this.servers_ComboBox.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.servers_ComboBox.EditorControl.Name = "NestedRadGridView";
            this.servers_ComboBox.EditorControl.ReadOnly = true;
            this.servers_ComboBox.EditorControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.servers_ComboBox.EditorControl.ShowGroupPanel = false;
            this.servers_ComboBox.EditorControl.Size = new System.Drawing.Size(500, 150);
            this.servers_ComboBox.EditorControl.TabIndex = 0;
            this.servers_ComboBox.Location = new System.Drawing.Point(0, 0);
            this.servers_ComboBox.Name = "servers_ComboBox";
            this.servers_ComboBox.Size = new System.Drawing.Size(224, 23);
            this.servers_ComboBox.TabIndex = 0;
            this.servers_ComboBox.TabStop = false;
            // 
            // CitrixServerDetailsComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servers_ComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CitrixServerDetailsComboBox";
            this.Size = new System.Drawing.Size(224, 23);
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.servers_ComboBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadMultiColumnComboBox servers_ComboBox;
    }
}
