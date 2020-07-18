namespace HP.ScalableTest
{
    partial class PrinterListForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn5 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn6 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterListForm));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn23 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn24 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn25 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn26 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn27 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn28 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn29 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn30 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn31 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn32 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn33 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor3 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.enumBinder1 = new Telerik.WinControls.UI.Data.EnumBinder();
            this.enumBinder2 = new Telerik.WinControls.UI.Data.EnumBinder();
            this.printer_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reservationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ok_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radGridViewPrinters = new Telerik.WinControls.UI.RadGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.reservationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.importToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printer_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewPrinters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewPrinters.MasterTemplate)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // enumBinder1
            // 
            this.enumBinder1.Source = typeof(HP.ScalableTest.Framework.Assets.AssetAttributes);
            gridViewComboBoxColumn5.DataSource = this.enumBinder1;
            gridViewComboBoxColumn5.DataType = typeof(HP.ScalableTest.Framework.Assets.AssetAttributes);
            gridViewComboBoxColumn5.DisplayMember = "Description";
            gridViewComboBoxColumn5.FieldName = "Attributes";
            gridViewComboBoxColumn5.HeaderText = "Attributes";
            gridViewComboBoxColumn5.IsAutoGenerated = true;
            gridViewComboBoxColumn5.Name = "Attributes";
            gridViewComboBoxColumn5.ReadOnly = true;
            gridViewComboBoxColumn5.ValueMember = "Value";
            this.enumBinder1.Target = gridViewComboBoxColumn5;
            // 
            // enumBinder2
            // 
            this.enumBinder2.Source = typeof(HP.ScalableTest.Framework.Assets.AssetAttributes);
            gridViewComboBoxColumn6.DataSource = this.enumBinder2;
            gridViewComboBoxColumn6.DataType = typeof(HP.ScalableTest.Framework.Assets.AssetAttributes);
            gridViewComboBoxColumn6.DisplayMember = "Description";
            gridViewComboBoxColumn6.FieldName = "Attributes";
            gridViewComboBoxColumn6.HeaderText = "Attributes";
            gridViewComboBoxColumn6.IsAutoGenerated = true;
            gridViewComboBoxColumn6.Name = "Attributes";
            gridViewComboBoxColumn6.ReadOnly = true;
            gridViewComboBoxColumn6.ValueMember = "Value";
            this.enumBinder2.Target = gridViewComboBoxColumn6;
            // 
            // printer_ContextMenuStrip
            // 
            this.printer_ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.printer_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.reservationsToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.printer_ContextMenuStrip.Name = "server_ContextMenuStrip";
            this.printer_ContextMenuStrip.Size = new System.Drawing.Size(145, 108);
            this.printer_ContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.printer_ContextMenuStrip_Opening);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.edit_Button_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // reservationsToolStripMenuItem
            // 
            this.reservationsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reservationsToolStripMenuItem.Image")));
            this.reservationsToolStripMenuItem.Name = "reservationsToolStripMenuItem";
            this.reservationsToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.reservationsToolStripMenuItem.Text = "Reservations";
            this.reservationsToolStripMenuItem.Click += new System.EventHandler(this.manageReservationsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // printerBindingSource
            // 
            this.printerBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.Printer);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(854, 484);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(112, 32);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.radGridViewPrinters);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(969, 476);
            this.panel1.TabIndex = 9;
            // 
            // radGridViewPrinters
            // 
            this.radGridViewPrinters.BackColor = System.Drawing.SystemColors.Control;
            this.radGridViewPrinters.ContextMenuStrip = this.printer_ContextMenuStrip;
            this.radGridViewPrinters.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridViewPrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewPrinters.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radGridViewPrinters.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radGridViewPrinters.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridViewPrinters.Location = new System.Drawing.Point(0, 27);
            // 
            // 
            // 
            this.radGridViewPrinters.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewPrinters.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewPrinters.MasterTemplate.AllowEditRow = false;
            gridViewTextBoxColumn23.EnableExpressionEditor = false;
            gridViewTextBoxColumn23.FieldName = "AssetId";
            gridViewTextBoxColumn23.HeaderText = "Id";
            gridViewTextBoxColumn23.IsAutoGenerated = true;
            gridViewTextBoxColumn23.MinWidth = 50;
            gridViewTextBoxColumn23.Name = "AssetId";
            gridViewTextBoxColumn24.EnableExpressionEditor = false;
            gridViewTextBoxColumn24.FieldName = "Product";
            gridViewTextBoxColumn24.HeaderText = "Name";
            gridViewTextBoxColumn24.IsAutoGenerated = true;
            gridViewTextBoxColumn24.MinWidth = 50;
            gridViewTextBoxColumn24.Name = "Product";
            gridViewTextBoxColumn25.EnableExpressionEditor = false;
            gridViewTextBoxColumn25.FieldName = "Model";
            gridViewTextBoxColumn25.HeaderText = "Model Name";
            gridViewTextBoxColumn25.IsAutoGenerated = true;
            gridViewTextBoxColumn25.MinWidth = 50;
            gridViewTextBoxColumn25.Name = "ModelName";
            gridViewTextBoxColumn25.Width = 73;
            gridViewTextBoxColumn26.EnableExpressionEditor = false;
            gridViewTextBoxColumn26.FieldName = "ModelNumber";
            gridViewTextBoxColumn26.HeaderText = "Model Number";
            gridViewTextBoxColumn26.IsAutoGenerated = true;
            gridViewTextBoxColumn26.MinWidth = 50;
            gridViewTextBoxColumn26.Name = "ModelNumber";
            gridViewTextBoxColumn26.Width = 85;
            gridViewTextBoxColumn27.EnableExpressionEditor = false;
            gridViewTextBoxColumn27.FieldName = "Address1";
            gridViewTextBoxColumn27.HeaderText = "Address";
            gridViewTextBoxColumn27.IsAutoGenerated = true;
            gridViewTextBoxColumn27.MinWidth = 50;
            gridViewTextBoxColumn27.Name = "Address1";
            gridViewTextBoxColumn28.EnableExpressionEditor = false;
            gridViewTextBoxColumn28.FieldName = "Location";
            gridViewTextBoxColumn28.HeaderText = "Location";
            gridViewTextBoxColumn28.IsAutoGenerated = true;
            gridViewTextBoxColumn28.MinWidth = 50;
            gridViewTextBoxColumn28.Name = "Location";
            gridViewTextBoxColumn28.Width = 51;
            gridViewTextBoxColumn29.EnableExpressionEditor = false;
            gridViewTextBoxColumn29.FieldName = "Owner";
            gridViewTextBoxColumn29.HeaderText = "Contact";
            gridViewTextBoxColumn29.IsAutoGenerated = true;
            gridViewTextBoxColumn29.MinWidth = 50;
            gridViewTextBoxColumn29.Name = "Owner";
            gridViewTextBoxColumn30.EnableExpressionEditor = false;
            gridViewTextBoxColumn30.FieldName = "SerialNumber";
            gridViewTextBoxColumn30.HeaderText = "Serial #";
            gridViewTextBoxColumn30.IsAutoGenerated = true;
            gridViewTextBoxColumn30.MinWidth = 50;
            gridViewTextBoxColumn30.Name = "SerialNumber";
            gridViewTextBoxColumn31.EnableExpressionEditor = false;
            gridViewTextBoxColumn31.FieldName = "ReservedFor";
            gridViewTextBoxColumn31.HeaderText = "Reservation";
            gridViewTextBoxColumn31.IsAutoGenerated = true;
            gridViewTextBoxColumn31.MinWidth = 75;
            gridViewTextBoxColumn31.Name = "ReservedFor";
            gridViewTextBoxColumn31.ReadOnly = true;
            gridViewTextBoxColumn31.Width = 100;
            gridViewTextBoxColumn32.EnableExpressionEditor = false;
            gridViewTextBoxColumn32.FieldName = "Description";
            gridViewTextBoxColumn32.HeaderText = "Description";
            gridViewTextBoxColumn32.IsAutoGenerated = true;
            gridViewTextBoxColumn32.MinWidth = 25;
            gridViewTextBoxColumn32.Name = "Description";
            gridViewTextBoxColumn32.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn32.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            gridViewTextBoxColumn33.DataType = typeof(HP.ScalableTest.Core.AssetInventory.AssetPool);
            gridViewTextBoxColumn33.EnableExpressionEditor = false;
            gridViewTextBoxColumn33.FieldName = "PoolName";
            gridViewTextBoxColumn33.HeaderText = "Asset Pool";
            gridViewTextBoxColumn33.MinWidth = 25;
            gridViewTextBoxColumn33.Name = "PoolName";
            gridViewTextBoxColumn33.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radGridViewPrinters.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn23,
            gridViewTextBoxColumn24,
            gridViewTextBoxColumn25,
            gridViewTextBoxColumn26,
            gridViewTextBoxColumn27,
            gridViewTextBoxColumn28,
            gridViewTextBoxColumn29,
            gridViewTextBoxColumn30,
            gridViewTextBoxColumn31,
            gridViewTextBoxColumn32,
            gridViewTextBoxColumn33});
            this.radGridViewPrinters.MasterTemplate.DataSource = this.printerBindingSource;
            this.radGridViewPrinters.MasterTemplate.EnableAlternatingRowColor = true;
            this.radGridViewPrinters.MasterTemplate.EnableFiltering = true;
            this.radGridViewPrinters.MasterTemplate.EnableGrouping = false;
            this.radGridViewPrinters.MasterTemplate.ShowRowHeaderColumn = false;
            sortDescriptor3.PropertyName = "Description";
            this.radGridViewPrinters.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor3});
            this.radGridViewPrinters.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radGridViewPrinters.Name = "radGridViewPrinters";
            this.radGridViewPrinters.ReadOnly = true;
            this.radGridViewPrinters.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.radGridViewPrinters.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 240, 150);
            this.radGridViewPrinters.Size = new System.Drawing.Size(969, 449);
            this.radGridViewPrinters.TabIndex = 2;
            this.radGridViewPrinters.Text = "radGridView1";
            this.radGridViewPrinters.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridViewPrinters_CellDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton,
            this.removeToolStripButton,
            this.reservationToolStripButton,
            this.importToolStripButton,
            this.refresh_ToolStripButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(969, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.newToolStripButton.Text = "Add";
            this.newToolStripButton.ToolTipText = "Add a new print device to your inventory";
            this.newToolStripButton.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripButton.Image")));
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.editToolStripButton.Text = "Edit";
            this.editToolStripButton.ToolTipText = "Edit the selected print device";
            this.editToolStripButton.Click += new System.EventHandler(this.edit_Button_Click);
            // 
            // removeToolStripButton
            // 
            this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
            this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeToolStripButton.Name = "removeToolStripButton";
            this.removeToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.removeToolStripButton.Text = "Remove";
            this.removeToolStripButton.ToolTipText = "Delete the selected print device from your inventory";
            this.removeToolStripButton.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // reservationToolStripButton
            // 
            this.reservationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("reservationToolStripButton.Image")));
            this.reservationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reservationToolStripButton.Name = "reservationToolStripButton";
            this.reservationToolStripButton.Size = new System.Drawing.Size(97, 24);
            this.reservationToolStripButton.Text = "Reservations";
            this.reservationToolStripButton.ToolTipText = "Manage any reservations for the selected print device";
            this.reservationToolStripButton.Click += new System.EventHandler(this.reservationToolStripButton_Click);
            // 
            // importToolStripButton
            // 
            this.importToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("importToolStripButton.Image")));
            this.importToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importToolStripButton.Name = "importToolStripButton";
            this.importToolStripButton.Size = new System.Drawing.Size(67, 24);
            this.importToolStripButton.Text = "Import";
            this.importToolStripButton.Click += new System.EventHandler(this.importToolStripButton_Click);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(70, 24);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // PrinterListForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.ok_Button;
            this.ClientSize = new System.Drawing.Size(969, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrinterListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Device Inventory";
            this.Load += new System.EventHandler(this.AssetListForm_Load);
            this.printer_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.printerBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewPrinters.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewPrinters)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.ContextMenuStrip printer_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.BindingSource printerBindingSource;
        private System.Windows.Forms.ToolStripMenuItem reservationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton reservationToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton importToolStripButton;
        private Telerik.WinControls.UI.RadGridView radGridViewPrinters;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
        private Telerik.WinControls.UI.Data.EnumBinder enumBinder1;
        private Telerik.WinControls.UI.Data.EnumBinder enumBinder2;
    }
}