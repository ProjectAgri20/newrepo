namespace HP.ScalableTest.PluginSupport.Print
{
    partial class LocalQueueSelectionForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalQueueSelectionForm));
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.printQueue_GridView = new Telerik.WinControls.UI.RadGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addExisting_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addDynamic_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(462, 351);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 1;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(381, 351);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // printQueue_GridView
            // 
            this.printQueue_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.printQueue_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printQueue_GridView.Location = new System.Drawing.Point(0, 25);
            // 
            // 
            // 
            this.printQueue_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "QueueName";
            gridViewTextBoxColumn1.HeaderText = "Queue Name";
            gridViewTextBoxColumn1.Name = "queueName_GridViewColumn";
            gridViewTextBoxColumn1.Width = 346;
            gridViewTextBoxColumn2.FieldName = "QueueType";
            gridViewTextBoxColumn2.HeaderText = "Type";
            gridViewTextBoxColumn2.MaxWidth = 80;
            gridViewTextBoxColumn2.MinWidth = 80;
            gridViewTextBoxColumn2.Name = "queueType_GridViewColumn";
            gridViewTextBoxColumn2.Width = 80;
            gridViewTextBoxColumn3.FieldName = "Device";
            gridViewTextBoxColumn3.HeaderText = "Device";
            gridViewTextBoxColumn3.MaxWidth = 80;
            gridViewTextBoxColumn3.MinWidth = 80;
            gridViewTextBoxColumn3.Name = "device_GridViewColumn";
            gridViewTextBoxColumn3.Width = 80;
            this.printQueue_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.printQueue_GridView.MasterTemplate.MultiSelect = true;
            this.printQueue_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.printQueue_GridView.Name = "printQueue_GridView";
            // 
            // 
            // 
            this.printQueue_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 25, 240, 150);
            this.printQueue_GridView.Size = new System.Drawing.Size(525, 308);
            this.printQueue_GridView.TabIndex = 3;
            this.printQueue_GridView.Text = "radGridView1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.printQueue_GridView);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(525, 333);
            this.panel1.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addExisting_ToolStripButton,
            this.edit_ToolStripButton,
            this.remove_ToolStripButton,
            this.addDynamic_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(525, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addExisting_ToolStripButton
            // 
            this.addExisting_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addExisting_ToolStripButton.Image")));
            this.addExisting_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addExisting_ToolStripButton.Name = "addExisting_ToolStripButton";
            this.addExisting_ToolStripButton.Size = new System.Drawing.Size(92, 22);
            this.addExisting_ToolStripButton.Text = "Add Existing";
            this.addExisting_ToolStripButton.Click += new System.EventHandler(this.addExisting_ToolStripButton_Click);
            // 
            // addDynamic_ToolStripButton
            // 

            this.addDynamic_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addDynamic_ToolStripButton.Image")));
            this.addDynamic_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addDynamic_ToolStripButton.Name = "addDynamic_ToolStripButton";
            this.addDynamic_ToolStripButton.Size = new System.Drawing.Size(99, 22);
            this.addDynamic_ToolStripButton.Text = "Add Dynamic";

            this.addDynamic_ToolStripButton.Click += new System.EventHandler(this.addDynamic_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(47, 22);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(70, 22);
            this.remove_ToolStripButton.Text = "Remove";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // LocalQueueSelectionForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(549, 386);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LocalQueueSelectionForm";
            this.Text = "Local Queue Selection";
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private Telerik.WinControls.UI.RadGridView printQueue_GridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addExisting_ToolStripButton;
        private System.Windows.Forms.ToolStripButton addDynamic_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
    }
}