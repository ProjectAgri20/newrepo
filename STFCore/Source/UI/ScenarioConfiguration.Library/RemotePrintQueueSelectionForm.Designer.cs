namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class RemotePrintQueueSelectionForm
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
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.printQueue_GridView = new Telerik.WinControls.UI.RadGridView();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // printQueue_GridView
            // 
            this.printQueue_GridView.BackColor = System.Drawing.SystemColors.Control;
            this.printQueue_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.printQueue_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.printQueue_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.printQueue_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.printQueue_GridView.Location = new System.Drawing.Point(0, -1);
            // 
            // 
            // 
            this.printQueue_GridView.MasterTemplate.AllowAddNewRow = false;
            this.printQueue_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "AssociatedAssetId";
            gridViewTextBoxColumn1.HeaderText = "Queue ID";
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "printQueueId_GridViewColumn";
            gridViewTextBoxColumn1.Width = 269;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "ServerHostName";
            gridViewTextBoxColumn2.HeaderText = "Server";
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "ServerHostName";
            gridViewTextBoxColumn2.Width = 198;
            gridViewTextBoxColumn3.AllowGroup = false;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "QueueName";
            gridViewTextBoxColumn3.HeaderText = "Name";
            gridViewTextBoxColumn3.MinWidth = 100;
            gridViewTextBoxColumn3.Name = "name_GridViewColumn";
            gridViewTextBoxColumn3.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn3.Width = 477;
            this.printQueue_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.printQueue_GridView.MasterTemplate.EnableFiltering = true;
            sortDescriptor1.PropertyName = "name_GridViewColumn";
            this.printQueue_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.printQueue_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.printQueue_GridView.Name = "printQueue_GridView";
            this.printQueue_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.printQueue_GridView.Size = new System.Drawing.Size(695, 300);
            this.printQueue_GridView.TabIndex = 0;
            this.printQueue_GridView.Text = "radGridView1";
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(532, 305);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(613, 305);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // RemotePrintQueueSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 333);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.printQueue_GridView);
            this.Name = "RemotePrintQueueSelectionForm";
            this.Text = "RemotePrintQueueSelectionForm";
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView printQueue_GridView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
    }
}