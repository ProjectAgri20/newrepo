namespace HP.ScalableTest.UI
{
    partial class OutputMonitorConfigControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputMonitorConfigControl));
            this.groupBox_Validation = new System.Windows.Forms.GroupBox();
            this.help_PictureBox = new System.Windows.Forms.PictureBox();
            this.metadataExtension_TextBox = new System.Windows.Forms.TextBox();
            this.lookForMetadata_CheckBox = new System.Windows.Forms.CheckBox();
            this.retentionLocation_TextBox = new System.Windows.Forms.TextBox();
            this.retention_ComboBox = new System.Windows.Forms.ComboBox();
            this.label_MetadataExtension = new System.Windows.Forms.Label();
            this.label_RetentionLocation = new System.Windows.Forms.Label();
            this.label_RetentionPolicy = new System.Windows.Forms.Label();
            this.label_Destination = new System.Windows.Forms.Label();
            this.destination_TextBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox_Validation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.help_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_Validation
            // 
            this.groupBox_Validation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Validation.Controls.Add(this.metadataExtension_TextBox);
            this.groupBox_Validation.Controls.Add(this.lookForMetadata_CheckBox);
            this.groupBox_Validation.Controls.Add(this.retentionLocation_TextBox);
            this.groupBox_Validation.Controls.Add(this.retention_ComboBox);
            this.groupBox_Validation.Controls.Add(this.label_MetadataExtension);
            this.groupBox_Validation.Controls.Add(this.label_RetentionLocation);
            this.groupBox_Validation.Controls.Add(this.label_RetentionPolicy);
            this.groupBox_Validation.Location = new System.Drawing.Point(3, 48);
            this.groupBox_Validation.Name = "groupBox_Validation";
            this.groupBox_Validation.Size = new System.Drawing.Size(393, 168);
            this.groupBox_Validation.TabIndex = 22;
            this.groupBox_Validation.TabStop = false;
            this.groupBox_Validation.Text = "Validation/Retention";
            // 
            // help_PictureBox
            // 
            this.help_PictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("help_PictureBox.BackgroundImage")));
            this.help_PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.help_PictureBox.Location = new System.Drawing.Point(104, -2);
            this.help_PictureBox.Name = "help_PictureBox";
            this.help_PictureBox.Size = new System.Drawing.Size(20, 18);
            this.help_PictureBox.TabIndex = 108;
            this.help_PictureBox.TabStop = false;
            this.help_PictureBox.Visible = false;
            // 
            // metadataExtension_TextBox
            // 
            this.metadataExtension_TextBox.CausesValidation = false;
            this.metadataExtension_TextBox.Enabled = false;
            this.metadataExtension_TextBox.Location = new System.Drawing.Point(152, 139);
            this.metadataExtension_TextBox.Name = "metadataExtension_TextBox";
            this.metadataExtension_TextBox.Size = new System.Drawing.Size(207, 20);
            this.metadataExtension_TextBox.TabIndex = 4;
            this.metadataExtension_TextBox.Text = "HPS";
            this.metadataExtension_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.metadataExtension_TextBox_Validating);
            // 
            // lookForMetadata_CheckBox
            // 
            this.lookForMetadata_CheckBox.AutoSize = true;
            this.lookForMetadata_CheckBox.Location = new System.Drawing.Point(16, 112);
            this.lookForMetadata_CheckBox.Name = "lookForMetadata_CheckBox";
            this.lookForMetadata_CheckBox.Size = new System.Drawing.Size(189, 17);
            this.lookForMetadata_CheckBox.TabIndex = 3;
            this.lookForMetadata_CheckBox.Text = "Check for associated metadata file";
            this.lookForMetadata_CheckBox.UseVisualStyleBackColor = true;
            this.lookForMetadata_CheckBox.CheckedChanged += new System.EventHandler(this.lookForMetadata_CheckBox_CheckedChanged);
            // 
            // retentionLocation_TextBox
            // 
            this.retentionLocation_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retentionLocation_TextBox.Location = new System.Drawing.Point(16, 75);
            this.retentionLocation_TextBox.Name = "retentionLocation_TextBox";
            this.retentionLocation_TextBox.Size = new System.Drawing.Size(343, 20);
            this.retentionLocation_TextBox.TabIndex = 2;
            this.retentionLocation_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.retentionLocation_TextBox_Validating);
            // 
            // retention_ComboBox
            // 
            this.retention_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retention_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.retention_ComboBox.FormattingEnabled = true;
            this.retention_ComboBox.Location = new System.Drawing.Point(103, 26);
            this.retention_ComboBox.Name = "retention_ComboBox";
            this.retention_ComboBox.Size = new System.Drawing.Size(256, 21);
            this.retention_ComboBox.TabIndex = 1;
            this.retention_ComboBox.SelectedIndexChanged += new System.EventHandler(this.retention_ComboBox_SelectedIndexChanged);
            this.retention_ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.retention_ComboBox_Validating);
            // 
            // label_MetadataExtension
            // 
            this.label_MetadataExtension.AutoSize = true;
            this.label_MetadataExtension.Location = new System.Drawing.Point(26, 142);
            this.label_MetadataExtension.Name = "label_MetadataExtension";
            this.label_MetadataExtension.Size = new System.Drawing.Size(120, 13);
            this.label_MetadataExtension.TabIndex = 0;
            this.label_MetadataExtension.Text = "Metadata File Extension";
            // 
            // label_RetentionLocation
            // 
            this.label_RetentionLocation.AutoSize = true;
            this.label_RetentionLocation.Location = new System.Drawing.Point(13, 59);
            this.label_RetentionLocation.Name = "label_RetentionLocation";
            this.label_RetentionLocation.Size = new System.Drawing.Size(97, 13);
            this.label_RetentionLocation.TabIndex = 0;
            this.label_RetentionLocation.Text = "Retention Location";
            // 
            // label_RetentionPolicy
            // 
            this.label_RetentionPolicy.AutoSize = true;
            this.label_RetentionPolicy.Location = new System.Drawing.Point(13, 29);
            this.label_RetentionPolicy.Name = "label_RetentionPolicy";
            this.label_RetentionPolicy.Size = new System.Drawing.Size(84, 13);
            this.label_RetentionPolicy.TabIndex = 0;
            this.label_RetentionPolicy.Text = "Retention Policy";
            // 
            // label_Destination
            // 
            this.label_Destination.AutoSize = true;
            this.label_Destination.Location = new System.Drawing.Point(3, 3);
            this.label_Destination.Name = "label_Destination";
            this.label_Destination.Size = new System.Drawing.Size(95, 13);
            this.label_Destination.TabIndex = 16;
            this.label_Destination.Text = "Output Destination";
            // 
            // destination_TextBox
            // 
            this.destination_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destination_TextBox.Location = new System.Drawing.Point(3, 19);
            this.destination_TextBox.Name = "destination_TextBox";
            this.destination_TextBox.Size = new System.Drawing.Size(393, 20);
            this.destination_TextBox.TabIndex = 14;
            this.destination_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.destination_TextBox_Validating);
            // 
            // OutputMonitorConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.help_PictureBox);
            this.Controls.Add(this.label_Destination);
            this.Controls.Add(this.groupBox_Validation);
            this.Controls.Add(this.destination_TextBox);
            this.Name = "OutputMonitorConfigControl";
            this.Size = new System.Drawing.Size(400, 219);
            this.groupBox_Validation.ResumeLayout(false);
            this.groupBox_Validation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.help_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox_Validation;
        private System.Windows.Forms.TextBox metadataExtension_TextBox;
        private System.Windows.Forms.CheckBox lookForMetadata_CheckBox;
        private System.Windows.Forms.TextBox retentionLocation_TextBox;
        private System.Windows.Forms.ComboBox retention_ComboBox;
        private System.Windows.Forms.Label label_MetadataExtension;
        private System.Windows.Forms.Label label_RetentionLocation;
        private System.Windows.Forms.Label label_RetentionPolicy;
        private System.Windows.Forms.Label label_Destination;
        private System.Windows.Forms.TextBox destination_TextBox;
        private System.Windows.Forms.PictureBox help_PictureBox;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
