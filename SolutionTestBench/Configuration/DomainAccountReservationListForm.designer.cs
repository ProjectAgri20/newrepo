namespace HP.ScalableTest
{
    partial class DomainAccountReservationListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DomainAccountReservationListForm));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.domainAccountReservation_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.domainAccountReservationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ok_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.radGridViewDomainAccountReservation = new Telerik.WinControls.UI.RadGridView();
            this.domainAccountReservation_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccountReservationBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDomainAccountReservation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDomainAccountReservation.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // domainAccountReservation_ContextMenuStrip
            // 
            this.domainAccountReservation_ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.domainAccountReservation_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.domainAccountReservation_ContextMenuStrip.Name = "server_ContextMenuStrip";
            this.domainAccountReservation_ContextMenuStrip.Size = new System.Drawing.Size(122, 30);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // domainAccountReservationBindingSource
            // 
            this.domainAccountReservationBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.DomainAccountReservation);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(413, 301);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 8);
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
            this.panel1.Controls.Add(this.radGridViewDomainAccountReservation);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(1, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(524, 289);
            this.panel1.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(524, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // removeToolStripButton
            // 
            this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
            this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeToolStripButton.Name = "removeToolStripButton";
            this.removeToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.removeToolStripButton.Text = "Remove";
            this.removeToolStripButton.ToolTipText = "Delete the selected Domain Account Reservation";
            this.removeToolStripButton.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // radGridViewDomainAccountReservation
            // 
            this.radGridViewDomainAccountReservation.BackColor = System.Drawing.SystemColors.Control;
            this.radGridViewDomainAccountReservation.ContextMenuStrip = this.domainAccountReservation_ContextMenuStrip;
            this.radGridViewDomainAccountReservation.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridViewDomainAccountReservation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewDomainAccountReservation.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radGridViewDomainAccountReservation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radGridViewDomainAccountReservation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridViewDomainAccountReservation.Location = new System.Drawing.Point(0, 27);
            // 
            // 
            // 
            this.radGridViewDomainAccountReservation.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewDomainAccountReservation.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewDomainAccountReservation.MasterTemplate.AllowEditRow = false;
            gridViewTextBoxColumn9.FieldName = "SessionId";
            gridViewTextBoxColumn9.HeaderText = "SessionId";
            gridViewTextBoxColumn9.IsAutoGenerated = true;
            gridViewTextBoxColumn9.Name = "SessionId";
            gridViewTextBoxColumn10.DataType = typeof(int);
            gridViewTextBoxColumn10.FieldName = "StartIndex";
            gridViewTextBoxColumn10.HeaderText = "StartIndex";
            gridViewTextBoxColumn10.IsAutoGenerated = true;
            gridViewTextBoxColumn10.Name = "StartIndex";
            gridViewTextBoxColumn11.DataType = typeof(int);
            gridViewTextBoxColumn11.FieldName = "Count";
            gridViewTextBoxColumn11.HeaderText = "Count";
            gridViewTextBoxColumn11.IsAutoGenerated = true;
            gridViewTextBoxColumn11.Name = "Count";
            gridViewTextBoxColumn12.FieldName = "DomainAccountKey";
            gridViewTextBoxColumn12.HeaderText = "DomainAccountKey";
            gridViewTextBoxColumn12.IsAutoGenerated = true;
            gridViewTextBoxColumn12.Name = "DomainAccountKey";
            this.radGridViewDomainAccountReservation.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12});
            this.radGridViewDomainAccountReservation.MasterTemplate.DataSource = this.domainAccountReservationBindingSource;
            this.radGridViewDomainAccountReservation.MasterTemplate.EnableAlternatingRowColor = true;
            this.radGridViewDomainAccountReservation.MasterTemplate.EnableFiltering = true;
            this.radGridViewDomainAccountReservation.MasterTemplate.EnableGrouping = false;
            this.radGridViewDomainAccountReservation.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridViewDomainAccountReservation.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radGridViewDomainAccountReservation.Name = "radGridViewDomainAccountReservation";
            this.radGridViewDomainAccountReservation.ReadOnly = true;
            this.radGridViewDomainAccountReservation.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.radGridViewDomainAccountReservation.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 240, 150);
            this.radGridViewDomainAccountReservation.Size = new System.Drawing.Size(524, 262);
            this.radGridViewDomainAccountReservation.TabIndex = 2;
            this.radGridViewDomainAccountReservation.Text = "radGridView1";
            // 
            // DomainAccountReservationListForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(530, 340);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DomainAccountReservationListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Domain Account Reservation";
            this.Load += new System.EventHandler(this.DomainAccountReservationListForm_Load);
            this.domainAccountReservation_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.domainAccountReservationBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDomainAccountReservation.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewDomainAccountReservation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.ContextMenuStrip domainAccountReservation_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.BindingSource domainAccountReservationBindingSource;
        private Telerik.WinControls.UI.RadGridView radGridViewDomainAccountReservation;
    }
}