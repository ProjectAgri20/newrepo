namespace HP.ScalableTest.LabConsole
{
    partial class DriverImportCompletionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverImportCompletionControl));
            this.destination_Label = new System.Windows.Forms.Label();
            this.destination_ListBox = new System.Windows.Forms.ListBox();
            this.repoGroupBox = new System.Windows.Forms.GroupBox();
            this.repoLabel = new System.Windows.Forms.Label();
            this.repoPathTextBox = new System.Windows.Forms.TextBox();
            this.newFolderPictureBox = new System.Windows.Forms.PictureBox();
            this.driverDestination_Control = new HP.ScalableTest.UI.Framework.PrintDriverDownloadControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.repoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newFolderPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // destination_Label
            // 
            this.destination_Label.Location = new System.Drawing.Point(60, 302);
            this.destination_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.destination_Label.Name = "destination_Label";
            this.destination_Label.Size = new System.Drawing.Size(85, 23);
            this.destination_Label.TabIndex = 11;
            this.destination_Label.Text = "Repository Subfolder";
            this.destination_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // destination_ListBox
            // 
            this.destination_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destination_ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.destination_ListBox.FormattingEnabled = true;
            this.destination_ListBox.ItemHeight = 23;
            this.destination_ListBox.Location = new System.Drawing.Point(150, 300);
            this.destination_ListBox.Margin = new System.Windows.Forms.Padding(4, 9, 4, 4);
            this.destination_ListBox.Name = "destination_ListBox";
            this.destination_ListBox.Size = new System.Drawing.Size(586, 27);
            this.destination_ListBox.TabIndex = 17;
            this.destination_ListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.destination_ListBox_DrawItem);
            // 
            // repoGroupBox
            // 
            this.repoGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repoGroupBox.Controls.Add(this.repoLabel);
            this.repoGroupBox.Controls.Add(this.repoPathTextBox);
            this.repoGroupBox.Controls.Add(this.newFolderPictureBox);
            this.repoGroupBox.Controls.Add(this.driverDestination_Control);
            this.repoGroupBox.Location = new System.Drawing.Point(3, 3);
            this.repoGroupBox.Name = "repoGroupBox";
            this.repoGroupBox.Size = new System.Drawing.Size(741, 284);
            this.repoGroupBox.TabIndex = 18;
            this.repoGroupBox.TabStop = false;
            this.repoGroupBox.Text = "Driver Repository - Choose Location for Import";
            // 
            // repoLabel
            // 
            this.repoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.repoLabel.Location = new System.Drawing.Point(51, 250);
            this.repoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.repoLabel.Name = "repoLabel";
            this.repoLabel.Size = new System.Drawing.Size(91, 23);
            this.repoLabel.TabIndex = 19;
            this.repoLabel.Text = "Repository Subfolder";
            this.repoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // repoPathTextBox
            // 
            this.repoPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.repoPathTextBox.Location = new System.Drawing.Point(148, 248);
            this.repoPathTextBox.Name = "repoPathTextBox";
            this.repoPathTextBox.Size = new System.Drawing.Size(586, 27);
            this.repoPathTextBox.TabIndex = 18;
            // 
            // newFolderPictureBox
            // 
            this.newFolderPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newFolderPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("newFolderPictureBox.Image")));
            this.newFolderPictureBox.InitialImage = null;
            this.newFolderPictureBox.Location = new System.Drawing.Point(709, 12);
            this.newFolderPictureBox.Name = "newFolderPictureBox";
            this.newFolderPictureBox.Size = new System.Drawing.Size(16, 16);
            this.newFolderPictureBox.TabIndex = 17;
            this.newFolderPictureBox.TabStop = false;
            this.newFolderPictureBox.Click += new System.EventHandler(this.newFolder_Button_Click);
            // 
            // driverDestination_Control
            // 
            this.driverDestination_Control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverDestination_Control.DisplayRoot = true;
            this.driverDestination_Control.Location = new System.Drawing.Point(8, 36);
            this.driverDestination_Control.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.driverDestination_Control.Name = "driverDestination_Control";
            this.driverDestination_Control.RepositoryPath = "";
            this.driverDestination_Control.Size = new System.Drawing.Size(725, 202);
            this.driverDestination_Control.TabIndex = 15;
            // 
            // DriverImportCompletionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.repoGroupBox);
            this.Controls.Add(this.destination_ListBox);
            this.Controls.Add(this.destination_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DriverImportCompletionControl";
            this.Size = new System.Drawing.Size(747, 343);
            this.repoGroupBox.ResumeLayout(false);
            this.repoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newFolderPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label destination_Label;
        private UI.Framework.PrintDriverDownloadControl driverDestination_Control;
        private System.Windows.Forms.ListBox destination_ListBox;
        private System.Windows.Forms.GroupBox repoGroupBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox newFolderPictureBox;
        private System.Windows.Forms.Label repoLabel;
        private System.Windows.Forms.TextBox repoPathTextBox;
    }
}
