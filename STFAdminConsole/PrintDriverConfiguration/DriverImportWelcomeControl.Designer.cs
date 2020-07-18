namespace HP.ScalableTest.LabConsole
{
    partial class DriverImportWelcomeControl
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
            this.central_RadioButton = new System.Windows.Forms.RadioButton();
            this.folder_RadioButton = new System.Windows.Forms.RadioButton();
            this.package_RadioButton = new System.Windows.Forms.RadioButton();
            this.location_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.locationButton = new System.Windows.Forms.Button();
            this.location_ListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // central_RadioButton
            // 
            this.central_RadioButton.AutoSize = true;
            this.central_RadioButton.Location = new System.Drawing.Point(108, 112);
            this.central_RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.central_RadioButton.Name = "central_RadioButton";
            this.central_RadioButton.Size = new System.Drawing.Size(195, 22);
            this.central_RadioButton.TabIndex = 0;
            this.central_RadioButton.Text = "Central Driver Repository";
            this.central_RadioButton.UseVisualStyleBackColor = true;
            this.central_RadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // folder_RadioButton
            // 
            this.folder_RadioButton.AutoSize = true;
            this.folder_RadioButton.Location = new System.Drawing.Point(108, 52);
            this.folder_RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.folder_RadioButton.Name = "folder_RadioButton";
            this.folder_RadioButton.Size = new System.Drawing.Size(141, 22);
            this.folder_RadioButton.TabIndex = 1;
            this.folder_RadioButton.TabStop = true;
            this.folder_RadioButton.Text = "Driver INF Folder";
            this.folder_RadioButton.UseVisualStyleBackColor = true;
            this.folder_RadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // package_RadioButton
            // 
            this.package_RadioButton.AutoSize = true;
            this.package_RadioButton.Checked = true;
            this.package_RadioButton.Location = new System.Drawing.Point(108, 82);
            this.package_RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.package_RadioButton.Name = "package_RadioButton";
            this.package_RadioButton.Size = new System.Drawing.Size(157, 22);
            this.package_RadioButton.TabIndex = 2;
            this.package_RadioButton.TabStop = true;
            this.package_RadioButton.Text = "Driver Package File";
            this.package_RadioButton.UseVisualStyleBackColor = true;
            this.package_RadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // location_Label
            // 
            this.location_Label.Location = new System.Drawing.Point(26, 155);
            this.location_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.location_Label.Name = "location_Label";
            this.location_Label.Size = new System.Drawing.Size(75, 19);
            this.location_Label.TabIndex = 7;
            this.location_Label.Text = "Driver(s)";
            this.location_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Location = new System.Drawing.Point(27, 20);
            this.description_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(372, 18);
            this.description_Label.TabIndex = 8;
            this.description_Label.Text = "Select the location of the print driver you want to import.";
            // 
            // locationButton
            // 
            this.locationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.locationButton.Location = new System.Drawing.Point(547, 151);
            this.locationButton.Name = "locationButton";
            this.locationButton.Size = new System.Drawing.Size(38, 28);
            this.locationButton.TabIndex = 10;
            this.locationButton.Text = "...";
            this.locationButton.UseVisualStyleBackColor = true;
            this.locationButton.Click += new System.EventHandler(this.locationButton_Click);
            // 
            // location_ListBox
            // 
            this.location_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.location_ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.location_ListBox.FormattingEnabled = true;
            this.location_ListBox.ItemHeight = 23;
            this.location_ListBox.Location = new System.Drawing.Point(108, 152);
            this.location_ListBox.Margin = new System.Windows.Forms.Padding(4, 4, 20, 4);
            this.location_ListBox.Name = "location_ListBox";
            this.location_ListBox.Size = new System.Drawing.Size(432, 27);
            this.location_ListBox.TabIndex = 9;
            this.location_ListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.location_ListBox_DrawItem);
            this.location_ListBox.Validating += new System.ComponentModel.CancelEventHandler(this.location_ListBox_Validating);
            // 
            // DriverImportWelcomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.locationButton);
            this.Controls.Add(this.location_ListBox);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.location_Label);
            this.Controls.Add(this.package_RadioButton);
            this.Controls.Add(this.folder_RadioButton);
            this.Controls.Add(this.central_RadioButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DriverImportWelcomeControl";
            this.Size = new System.Drawing.Size(660, 253);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton central_RadioButton;
        private System.Windows.Forms.RadioButton folder_RadioButton;
        private System.Windows.Forms.RadioButton package_RadioButton;
        private System.Windows.Forms.Label location_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Button locationButton;
        private System.Windows.Forms.ListBox location_ListBox;
    }
}
