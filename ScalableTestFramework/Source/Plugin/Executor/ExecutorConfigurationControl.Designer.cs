namespace HP.ScalableTest.Plugin.Executor
{
    partial class ExecutorConfigurationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutorConfigurationControl));
            this.dataGridView_executables = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CopyDirectory = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SessionId = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.setup_groupBox = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.copydir_checkBox = new System.Windows.Forms.CheckBox();
            this.browseInstaller_button = new System.Windows.Forms.Button();
            this.setupPath_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addPlugin_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.delete_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.up_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.down_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.executor_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_executables)).BeginInit();
            this.setup_groupBox.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_executables
            // 
            this.dataGridView_executables.AllowUserToAddRows = false;
            this.dataGridView_executables.AllowUserToDeleteRows = false;
            this.dataGridView_executables.AllowUserToResizeRows = false;
            this.dataGridView_executables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_executables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_executables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.Arguments,
            this.CopyDirectory,
            this.SessionId});
            this.dataGridView_executables.Location = new System.Drawing.Point(3, 34);
            this.dataGridView_executables.Name = "dataGridView_executables";
            this.dataGridView_executables.RowHeadersVisible = false;
            this.dataGridView_executables.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_executables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_executables.Size = new System.Drawing.Size(472, 210);
            this.dataGridView_executables.TabIndex = 3;
            // 
            // FileName
            // 
            this.FileName.DataPropertyName = "FileName";
            this.FileName.FillWeight = 200F;
            this.FileName.HeaderText = "Filename";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 220;
            // 
            // Arguments
            // 
            this.Arguments.DataPropertyName = "Arguments";
            this.Arguments.HeaderText = "Arguments";
            this.Arguments.Name = "Arguments";
            this.Arguments.ReadOnly = true;
            this.Arguments.Width = 250;
            // 
            // CopyDirectory
            // 
            this.CopyDirectory.DataPropertyName = "CopyDirectory";
            this.CopyDirectory.FalseValue = "False";
            this.CopyDirectory.HeaderText = "Directory Copied";
            this.CopyDirectory.Name = "CopyDirectory";
            this.CopyDirectory.ReadOnly = true;
            this.CopyDirectory.TrueValue = "True";
            // 
            // SessionId
            // 
            this.SessionId.DataPropertyName = "PassSessionId";
            this.SessionId.FalseValue = "False";
            this.SessionId.HeaderText = "SessionId";
            this.SessionId.Name = "SessionId";
            this.SessionId.ReadOnly = true;
            this.SessionId.TrueValue = "True";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Executable";
            // 
            // setup_groupBox
            // 
            this.setup_groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setup_groupBox.Controls.Add(this.label7);
            this.setup_groupBox.Controls.Add(this.label6);
            this.setup_groupBox.Controls.Add(this.label5);
            this.setup_groupBox.Controls.Add(this.copydir_checkBox);
            this.setup_groupBox.Controls.Add(this.browseInstaller_button);
            this.setup_groupBox.Controls.Add(this.setupPath_textBox);
            this.setup_groupBox.Controls.Add(this.label4);
            this.setup_groupBox.Location = new System.Drawing.Point(3, 259);
            this.setup_groupBox.Name = "setup_groupBox";
            this.setup_groupBox.Size = new System.Drawing.Size(472, 134);
            this.setup_groupBox.TabIndex = 11;
            this.setup_groupBox.TabStop = false;
            this.setup_groupBox.Text = "Dependent Software";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "to specific directory unattended.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "NOTE:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(334, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Ensure that the installer is a batch file, which installs the required AUT";
            // 
            // copydir_checkBox
            // 
            this.copydir_checkBox.AutoSize = true;
            this.copydir_checkBox.Location = new System.Drawing.Point(74, 46);
            this.copydir_checkBox.Name = "copydir_checkBox";
            this.copydir_checkBox.Size = new System.Drawing.Size(101, 17);
            this.copydir_checkBox.TabIndex = 3;
            this.copydir_checkBox.Text = "Copy Directory?";
            this.copydir_checkBox.UseVisualStyleBackColor = true;
            // 
            // browseInstaller_button
            // 
            this.browseInstaller_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseInstaller_button.Location = new System.Drawing.Point(419, 19);
            this.browseInstaller_button.Name = "browseInstaller_button";
            this.browseInstaller_button.Size = new System.Drawing.Size(40, 23);
            this.browseInstaller_button.TabIndex = 2;
            this.browseInstaller_button.Text = "...";
            this.browseInstaller_button.UseVisualStyleBackColor = true;
            this.browseInstaller_button.Click += new System.EventHandler(this.browseInstaller_button_Click);
            // 
            // setupPath_textBox
            // 
            this.setupPath_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setupPath_textBox.BackColor = System.Drawing.Color.White;
            this.setupPath_textBox.Location = new System.Drawing.Point(74, 20);
            this.setupPath_textBox.Name = "setupPath_textBox";
            this.setupPath_textBox.Size = new System.Drawing.Size(336, 20);
            this.setupPath_textBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Installer File";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPlugin_ToolStripButton,
            this.edit_ToolStripButton,
            this.delete_ToolStripButton,
            this.up_toolStripButton,
            this.down_toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(515, 27);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addPlugin_ToolStripButton
            // 
            this.addPlugin_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addPlugin_ToolStripButton.Image")));
            this.addPlugin_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPlugin_ToolStripButton.Name = "addPlugin_ToolStripButton";
            this.addPlugin_ToolStripButton.Size = new System.Drawing.Size(55, 24);
            this.addPlugin_ToolStripButton.Text = "New";
            this.addPlugin_ToolStripButton.Click += new System.EventHandler(this.addPlugin_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // delete_ToolStripButton
            // 
            this.delete_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delete_ToolStripButton.Image")));
            this.delete_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_ToolStripButton.Name = "delete_ToolStripButton";
            this.delete_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.delete_ToolStripButton.Text = "Remove";
            this.delete_ToolStripButton.Click += new System.EventHandler(this.delete_ToolStripButton_Click);
            // 
            // up_toolStripButton
            // 
            this.up_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("up_toolStripButton.Image")));
            this.up_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.up_toolStripButton.Name = "up_toolStripButton";
            this.up_toolStripButton.Size = new System.Drawing.Size(78, 24);
            this.up_toolStripButton.Text = "Move up";
            this.up_toolStripButton.Click += new System.EventHandler(this.moveup_button_Click);
            // 
            // down_toolStripButton
            // 
            this.down_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("down_toolStripButton.Image")));
            this.down_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.down_toolStripButton.Name = "down_toolStripButton";
            this.down_toolStripButton.Size = new System.Drawing.Size(94, 24);
            this.down_toolStripButton.Text = "Move down";
            this.down_toolStripButton.Click += new System.EventHandler(this.movedown_button_Click);
            // 
            // ExecutorConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.setup_groupBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView_executables);
            this.Name = "ExecutorConfigurationControl";
            this.Size = new System.Drawing.Size(515, 405);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_executables)).EndInit();
            this.setup_groupBox.ResumeLayout(false);
            this.setup_groupBox.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView_executables;
        private System.Windows.Forms.GroupBox setup_groupBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox copydir_checkBox;
        private System.Windows.Forms.Button browseInstaller_button;
        private System.Windows.Forms.TextBox setupPath_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addPlugin_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton delete_ToolStripButton;
        private System.Windows.Forms.ToolStripButton up_toolStripButton;
        private System.Windows.Forms.ToolStripButton down_toolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Arguments;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CopyDirectory;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SessionId;
        private Framework.UI.FieldValidator executor_fieldValidator;
    }
}