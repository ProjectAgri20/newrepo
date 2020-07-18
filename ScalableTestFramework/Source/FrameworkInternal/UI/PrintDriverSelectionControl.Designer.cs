namespace HP.ScalableTest.Framework.UI
{
    partial class PrintDriverSelectionControl
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
            this.driverPackage_ComboBox = new System.Windows.Forms.ComboBox();
            this.driverPackage_Label = new System.Windows.Forms.Label();
            this.driverModel_Label = new System.Windows.Forms.Label();
            this.driverModel_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // driverPackage_ComboBox
            // 
            this.driverPackage_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverPackage_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverPackage_ComboBox.FormattingEnabled = true;
            this.driverPackage_ComboBox.Location = new System.Drawing.Point(88, 0);
            this.driverPackage_ComboBox.Name = "driverPackage_ComboBox";
            this.driverPackage_ComboBox.Size = new System.Drawing.Size(262, 23);
            this.driverPackage_ComboBox.Sorted = true;
            this.driverPackage_ComboBox.TabIndex = 1;
            this.driverPackage_ComboBox.SelectedIndexChanged += new System.EventHandler(this.driverPackage_ComboBox_SelectedIndexChanged);
            // 
            // driverPackage_Label
            // 
            this.driverPackage_Label.AutoSize = true;
            this.driverPackage_Label.Location = new System.Drawing.Point(-3, 3);
            this.driverPackage_Label.Name = "driverPackage_Label";
            this.driverPackage_Label.Size = new System.Drawing.Size(85, 15);
            this.driverPackage_Label.TabIndex = 0;
            this.driverPackage_Label.Text = "Driver Package";
            // 
            // driverModel_Label
            // 
            this.driverModel_Label.AutoSize = true;
            this.driverModel_Label.Location = new System.Drawing.Point(7, 36);
            this.driverModel_Label.Name = "driverModel_Label";
            this.driverModel_Label.Size = new System.Drawing.Size(75, 15);
            this.driverModel_Label.TabIndex = 2;
            this.driverModel_Label.Text = "Driver Model";
            // 
            // driverModel_ComboBox
            // 
            this.driverModel_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverModel_ComboBox.DisplayMember = "DriverName";
            this.driverModel_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverModel_ComboBox.FormattingEnabled = true;
            this.driverModel_ComboBox.Location = new System.Drawing.Point(88, 33);
            this.driverModel_ComboBox.Name = "driverModel_ComboBox";
            this.driverModel_ComboBox.Size = new System.Drawing.Size(262, 23);
            this.driverModel_ComboBox.Sorted = true;
            this.driverModel_ComboBox.TabIndex = 3;
            this.driverModel_ComboBox.SelectedIndexChanged += new System.EventHandler(this.driverModel_ComboBox_SelectedIndexChanged);
            // 
            // PrintDriverSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.driverModel_ComboBox);
            this.Controls.Add(this.driverModel_Label);
            this.Controls.Add(this.driverPackage_Label);
            this.Controls.Add(this.driverPackage_ComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintDriverSelectionControl";
            this.Size = new System.Drawing.Size(350, 56);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox driverPackage_ComboBox;
        private System.Windows.Forms.Label driverPackage_Label;
        private System.Windows.Forms.Label driverModel_Label;
        private System.Windows.Forms.ComboBox driverModel_ComboBox;
    }
}
