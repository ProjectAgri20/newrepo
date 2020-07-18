namespace HP.ScalableTest.UI.Framework
{
    partial class MainFormConnectDialog
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.connect_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.dispatchers_GridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // connect_Button
            // 
            this.connect_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.connect_Button.Location = new System.Drawing.Point(350, 221);
            this.connect_Button.Name = "connect_Button";
            this.connect_Button.Size = new System.Drawing.Size(100, 32);
            this.connect_Button.TabIndex = 1;
            this.connect_Button.Text = "Connect";
            this.connect_Button.UseVisualStyleBackColor = true;
            this.connect_Button.Click += new System.EventHandler(this.connect_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(456, 221);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // dispatchers_GridView
            // 
            this.dispatchers_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dispatchers_GridView.Location = new System.Drawing.Point(4, 4);
            // 
            // 
            // 
            this.dispatchers_GridView.MasterTemplate.AllowAddNewRow = false;
            this.dispatchers_GridView.MasterTemplate.AllowColumnReorder = false;
            this.dispatchers_GridView.MasterTemplate.AllowDeleteRow = false;
            this.dispatchers_GridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.FieldName = "HostName";
            gridViewTextBoxColumn1.HeaderText = "Dispatcher";
            gridViewTextBoxColumn1.Name = "hostName_Column";
            gridViewTextBoxColumn1.Width = 100;
            gridViewTextBoxColumn2.FieldName = "Contact";
            gridViewTextBoxColumn2.HeaderText = "Contact";
            gridViewTextBoxColumn2.Name = "contact_Column";
            gridViewTextBoxColumn2.Width = 150;
            gridViewTextBoxColumn3.FieldName = "Environment";
            gridViewTextBoxColumn3.HeaderText = "Environment";
            gridViewTextBoxColumn3.Name = "environment_Column";
            gridViewTextBoxColumn3.Width = 100;
            gridViewTextBoxColumn4.FieldName = "StfServiceVersion";
            gridViewTextBoxColumn4.HeaderText = "Version";
            gridViewTextBoxColumn4.IsPinned = true;
            gridViewTextBoxColumn4.Name = "version_Column";
            gridViewTextBoxColumn4.PinPosition = Telerik.WinControls.UI.PinnedColumnPosition.Right;
            gridViewTextBoxColumn4.Width = 100;
            this.dispatchers_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.dispatchers_GridView.MasterTemplate.EnableGrouping = false;
            this.dispatchers_GridView.Name = "dispatchers_GridView";
            this.dispatchers_GridView.Size = new System.Drawing.Size(560, 211);
            this.dispatchers_GridView.TabIndex = 0;
            this.dispatchers_GridView.Text = "Dispatchers";
            // 
            // MainFormConnectDialog
            // 
            this.AcceptButton = this.connect_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(568, 265);
            this.Controls.Add(this.dispatchers_GridView);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.connect_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainFormConnectDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to STF Dispatcher";
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connect_Button;
        private System.Windows.Forms.Button cancel_Button;
        private Telerik.WinControls.UI.RadGridView dispatchers_GridView;
    }
}