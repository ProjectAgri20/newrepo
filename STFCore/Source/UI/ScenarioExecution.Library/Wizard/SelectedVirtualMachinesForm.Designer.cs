namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class SelectedVirtualMachinesForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.close_Button = new System.Windows.Forms.Button();
            this.virtualMachine_GridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.virtualMachine_GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close_Button.Location = new System.Drawing.Point(575, 389);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(87, 23);
            this.close_Button.TabIndex = 2;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // virtualMachine_GridView
            // 
            this.virtualMachine_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.virtualMachine_GridView.Location = new System.Drawing.Point(14, 14);
            // 
            // virtualMachine_GridView
            // 
            this.virtualMachine_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn6.AllowGroup = false;
            gridViewTextBoxColumn6.FieldName = "Name";
            gridViewTextBoxColumn6.HeaderText = "Name";
            gridViewTextBoxColumn6.Name = "name_GridViewColumn";
            gridViewTextBoxColumn6.Width = 126;
            gridViewTextBoxColumn7.FieldName = "PowerState";
            gridViewTextBoxColumn7.HeaderText = "Power State";
            gridViewTextBoxColumn7.Name = "powerState_GridViewColumn";
            gridViewTextBoxColumn7.Width = 126;
            gridViewTextBoxColumn8.FieldName = "UsageState";
            gridViewTextBoxColumn8.HeaderText = "Usage State";
            gridViewTextBoxColumn8.Name = "usageState_GridViewColumn";
            gridViewTextBoxColumn8.Width = 126;
            gridViewTextBoxColumn9.FieldName = "HoldId";
            gridViewTextBoxColumn9.HeaderText = "Hold ID";
            gridViewTextBoxColumn9.Name = "holdId_GridViewColumn";
            gridViewTextBoxColumn9.Width = 126;
            gridViewTextBoxColumn10.AllowGroup = false;
            gridViewTextBoxColumn10.FieldName = "LastUpdated";
            gridViewTextBoxColumn10.HeaderText = "Last Updated";
            gridViewTextBoxColumn10.Name = "lastUpdated_GridViewColumn";
            gridViewTextBoxColumn10.Width = 127;
            this.virtualMachine_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10});
            this.virtualMachine_GridView.Name = "virtualMachine_GridView";
            this.virtualMachine_GridView.Size = new System.Drawing.Size(648, 369);
            this.virtualMachine_GridView.TabIndex = 3;
            this.virtualMachine_GridView.Text = "radGridView1";
            // 
            // SelectedVirtualMachinesForm
            // 
            this.AcceptButton = this.close_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 424);
            this.Controls.Add(this.virtualMachine_GridView);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SelectedVirtualMachinesForm";
            this.Text = "Selected Virtual Machines";
            this.Load += new System.EventHandler(this.SelectedVirtualMachinesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.virtualMachine_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private Telerik.WinControls.UI.RadGridView virtualMachine_GridView;
    }
}