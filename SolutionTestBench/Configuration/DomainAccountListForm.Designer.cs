namespace HP.ScalableTest
{
    partial class DomainAccountListForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn2 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DomainAccountListForm));
            this.ok_Button = new System.Windows.Forms.Button();
            this.apply_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.domainAccount_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.domainAccountPoolBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reservationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccount_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccount_RadGridView.MasterTemplate)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccountPoolBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(200, 253);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(414, 253);
            this.apply_Button.Margin = new System.Windows.Forms.Padding(4);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(100, 32);
            this.apply_Button.TabIndex = 6;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.domainAccount_RadGridView);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 239);
            this.panel1.TabIndex = 9;
            // 
            // domainAccount_RadGridView
            // 
            this.domainAccount_RadGridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.domainAccount_RadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.domainAccount_RadGridView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainAccount_RadGridView.Location = new System.Drawing.Point(0, 27);
            // 
            // 
            // 
            gridViewTextBoxColumn1.FieldName = "DomainAccountKey";
            gridViewTextBoxColumn1.HeaderText = "Id";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.Name = "DomainAccountKey";
            gridViewTextBoxColumn2.FieldName = "UserNameFormat";
            gridViewTextBoxColumn2.HeaderText = "Format";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "UserNameFormat";
            gridViewDecimalColumn1.DataType = typeof(int);
            gridViewDecimalColumn1.FieldName = "MinimumUserNumber";
            gridViewDecimalColumn1.HeaderText = "Start";
            gridViewDecimalColumn1.IsAutoGenerated = true;
            gridViewDecimalColumn1.Name = "MinimumUserNumber";
            gridViewDecimalColumn2.DataType = typeof(int);
            gridViewDecimalColumn2.FieldName = "MaximumUserNumber";
            gridViewDecimalColumn2.HeaderText = "End";
            gridViewDecimalColumn2.IsAutoGenerated = true;
            gridViewDecimalColumn2.Name = "MaximumUserNumber";
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Pool Name";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "Description";
            this.domainAccount_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewDecimalColumn1,
            gridViewDecimalColumn2,
            gridViewTextBoxColumn3});
            this.domainAccount_RadGridView.MasterTemplate.DataSource = this.domainAccountPoolBindingSource;
            this.domainAccount_RadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.domainAccount_RadGridView.Name = "domainAccount_RadGridView";
            // 
            // 
            // 
            this.domainAccount_RadGridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 527, 212);
            this.domainAccount_RadGridView.Size = new System.Drawing.Size(527, 212);
            this.domainAccount_RadGridView.TabIndex = 3;
            this.domainAccount_RadGridView.Text = "radGridView1";
            this.domainAccount_RadGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.domainAccount_RadGridView_CellDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton,
            this.removeToolStripButton,
            this.reservationToolStripButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(4, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(527, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.CheckOnClick = true;
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
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.CausesValidation = false;
            this.cancelButton.Location = new System.Drawing.Point(307, 254);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // domainAccountPoolBindingSource
            // 
            this.domainAccountPoolBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.DomainAccountPool);
            // 
            // reservationToolStripButton
            // 
            this.reservationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("reservationToolStripButton.Image")));
            this.reservationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reservationToolStripButton.Name = "reservationToolStripButton";
            this.reservationToolStripButton.Size = new System.Drawing.Size(97, 24);
            this.reservationToolStripButton.Text = "Reservations";
            this.reservationToolStripButton.ToolTipText = "Manage any reservations for the selected camera";
            this.reservationToolStripButton.Click += new System.EventHandler(this.reservationToolStripButton_Click);
            // 
            // DomainAccountListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(527, 298);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DomainAcountListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Virtual Worker Account Pools";
            this.Load += new System.EventHandler(this.DomainAccountList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccount_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccount_RadGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.domainAccountPoolBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.Button cancelButton;
        private Telerik.WinControls.UI.RadGridView domainAccount_RadGridView;
        private System.Windows.Forms.BindingSource domainAccountPoolBindingSource;
        private System.Windows.Forms.ToolStripButton reservationToolStripButton;
    }
}