namespace HP.ScalableTest.Plugin.BashLogger
{
    partial class BashLoggerConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.assetSelectionControl_bashLog = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.file_groupBox = new System.Windows.Forms.GroupBox();
            this.label_splitSize = new System.Windows.Forms.Label();
            this.numericUpDownFileSplitSize = new System.Windows.Forms.NumericUpDown();
            this.label_logLocation = new System.Windows.Forms.Label();
            this.textBoxLoggerDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.source_groupBox = new System.Windows.Forms.GroupBox();
            this.comboBoxLoggerAction = new System.Windows.Forms.ComboBox();
            this.file_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFileSplitSize)).BeginInit();
            this.source_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetSelectionControl_bashLog
            // 
            this.assetSelectionControl_bashLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl_bashLog.Location = new System.Drawing.Point(3, 3);
            this.assetSelectionControl_bashLog.Name = "assetSelectionControl_bashLog";
            this.assetSelectionControl_bashLog.Size = new System.Drawing.Size(692, 347);
            this.assetSelectionControl_bashLog.TabIndex = 13;
            // 
            // file_groupBox
            // 
            this.file_groupBox.Controls.Add(this.label_splitSize);
            this.file_groupBox.Controls.Add(this.numericUpDownFileSplitSize);
            this.file_groupBox.Controls.Add(this.label_logLocation);
            this.file_groupBox.Controls.Add(this.textBoxLoggerDirectory);
            this.file_groupBox.Controls.Add(this.buttonBrowse);
            this.file_groupBox.Location = new System.Drawing.Point(291, 363);
            this.file_groupBox.Name = "file_groupBox";
            this.file_groupBox.Size = new System.Drawing.Size(404, 101);
            this.file_groupBox.TabIndex = 14;
            this.file_groupBox.TabStop = false;
            this.file_groupBox.Text = "Log File Settings";
            // 
            // label_splitSize
            // 
            this.label_splitSize.AutoSize = true;
            this.label_splitSize.Location = new System.Drawing.Point(6, 70);
            this.label_splitSize.Name = "label_splitSize";
            this.label_splitSize.Size = new System.Drawing.Size(126, 15);
            this.label_splitSize.TabIndex = 7;
            this.label_splitSize.Text = "Split Log File Size (MB)";
            // 
            // numericUpDownFileSplitSize
            // 
            this.numericUpDownFileSplitSize.Location = new System.Drawing.Point(138, 68);
            this.numericUpDownFileSplitSize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFileSplitSize.Name = "numericUpDownFileSplitSize";
            this.numericUpDownFileSplitSize.Size = new System.Drawing.Size(63, 23);
            this.numericUpDownFileSplitSize.TabIndex = 6;
            this.numericUpDownFileSplitSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_logLocation
            // 
            this.label_logLocation.AutoSize = true;
            this.label_logLocation.Location = new System.Drawing.Point(6, 21);
            this.label_logLocation.Name = "label_logLocation";
            this.label_logLocation.Size = new System.Drawing.Size(97, 15);
            this.label_logLocation.TabIndex = 5;
            this.label_logLocation.Text = "Log File Location";
            // 
            // textBoxLoggerDirectory
            // 
            this.textBoxLoggerDirectory.BackColor = System.Drawing.Color.White;
            this.textBoxLoggerDirectory.Location = new System.Drawing.Point(6, 39);
            this.textBoxLoggerDirectory.Name = "textBoxLoggerDirectory";
            this.textBoxLoggerDirectory.ReadOnly = true;
            this.textBoxLoggerDirectory.Size = new System.Drawing.Size(302, 23);
            this.textBoxLoggerDirectory.TabIndex = 3;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(314, 39);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(57, 23);
            this.buttonBrowse.TabIndex = 4;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // source_groupBox
            // 
            this.source_groupBox.Controls.Add(this.comboBoxLoggerAction);
            this.source_groupBox.Location = new System.Drawing.Point(5, 363);
            this.source_groupBox.Name = "source_groupBox";
            this.source_groupBox.Size = new System.Drawing.Size(270, 101);
            this.source_groupBox.TabIndex = 12;
            this.source_groupBox.TabStop = false;
            this.source_groupBox.Text = "Logger Action";
            // 
            // comboBoxLoggerAction
            // 
            this.comboBoxLoggerAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLoggerAction.FormattingEnabled = true;
            this.comboBoxLoggerAction.Location = new System.Drawing.Point(6, 39);
            this.comboBoxLoggerAction.Name = "comboBoxLoggerAction";
            this.comboBoxLoggerAction.Size = new System.Drawing.Size(191, 23);
            this.comboBoxLoggerAction.TabIndex = 15;
            this.comboBoxLoggerAction.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoggerAction_SelectedIndexChanged);
            // 
            // BashLoggerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.file_groupBox);
            this.Controls.Add(this.assetSelectionControl_bashLog);
            this.Controls.Add(this.source_groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BashLoggerConfigurationControl";
            this.Size = new System.Drawing.Size(701, 524);
            this.file_groupBox.ResumeLayout(false);
            this.file_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFileSplitSize)).EndInit();
            this.source_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl_bashLog;
        private System.Windows.Forms.GroupBox file_groupBox;
        private System.Windows.Forms.TextBox textBoxLoggerDirectory;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label label_logLocation;
        private System.Windows.Forms.Label label_splitSize;
        private System.Windows.Forms.NumericUpDown numericUpDownFileSplitSize;
        private System.Windows.Forms.GroupBox source_groupBox;
        private System.Windows.Forms.ComboBox comboBoxLoggerAction;
    }
}
