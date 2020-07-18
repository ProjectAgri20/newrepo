
namespace HP.ScalableTest.Plugin.ServiceStartStop
{
    partial class ServiceStartStopConfigurationControl
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
            this.server_ComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.serverSelectLabel = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.radioRestart = new System.Windows.Forms.RadioButton();
            this.radioStart = new System.Windows.Forms.RadioButton();
            this.radioStop = new System.Windows.Forms.RadioButton();
            this.serviceListBox = new System.Windows.Forms.ListBox();
            this.groupBox_Action = new System.Windows.Forms.GroupBox();
            this.groupBox_Action.SuspendLayout();
            this.SuspendLayout();
            // 
            // server_ComboBox
            // 
            this.server_ComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.server_ComboBox.Location = new System.Drawing.Point(12, 27);
            this.server_ComboBox.Name = "server_ComboBox";
            this.server_ComboBox.Size = new System.Drawing.Size(293, 27);
            this.server_ComboBox.TabIndex = 0;
            // 
            // serverSelectLabel
            // 
            this.serverSelectLabel.AutoSize = true;
            this.serverSelectLabel.Location = new System.Drawing.Point(9, 9);
            this.serverSelectLabel.Name = "serverSelectLabel";
            this.serverSelectLabel.Size = new System.Drawing.Size(73, 15);
            this.serverSelectLabel.TabIndex = 1;
            this.serverSelectLabel.Text = "Select Server";
            // 
            // serviceLabel
            // 
            this.serviceLabel.AutoSize = true;
            this.serviceLabel.Location = new System.Drawing.Point(9, 152);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(83, 15);
            this.serviceLabel.TabIndex = 2;
            this.serviceLabel.Text = "Select Services";
            // 
            // radioRestart
            // 
            this.radioRestart.AutoSize = true;
            this.radioRestart.Location = new System.Drawing.Point(196, 22);
            this.radioRestart.Name = "radioRestart";
            this.radioRestart.Size = new System.Drawing.Size(61, 19);
            this.radioRestart.TabIndex = 2;
            this.radioRestart.TabStop = true;
            this.radioRestart.Text = "Restart";
            this.radioRestart.UseVisualStyleBackColor = true;
            this.radioRestart.CheckedChanged += new System.EventHandler(this.ServiceActions_SelectedChanged);
            // 
            // radioStart
            // 
            this.radioStart.AutoSize = true;
            this.radioStart.Location = new System.Drawing.Point(118, 22);
            this.radioStart.Name = "radioStart";
            this.radioStart.Size = new System.Drawing.Size(49, 19);
            this.radioStart.TabIndex = 1;
            this.radioStart.TabStop = true;
            this.radioStart.Text = "Start";
            this.radioStart.UseVisualStyleBackColor = true;
            this.radioStart.CheckedChanged += new System.EventHandler(this.ServiceActions_SelectedChanged);
            // 
            // radioStop
            // 
            this.radioStop.AutoSize = true;
            this.radioStop.Checked = true;
            this.radioStop.Location = new System.Drawing.Point(41, 23);
            this.radioStop.Name = "radioStop";
            this.radioStop.Size = new System.Drawing.Size(49, 19);
            this.radioStop.TabIndex = 0;
            this.radioStop.TabStop = true;
            this.radioStop.Text = "Stop";
            this.radioStop.UseVisualStyleBackColor = true;
            this.radioStop.CheckedChanged += new System.EventHandler(this.ServiceActions_SelectedChanged);
            // 
            // serviceListBox
            // 
            this.serviceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceListBox.FormattingEnabled = true;
            this.serviceListBox.ItemHeight = 15;
            this.serviceListBox.Location = new System.Drawing.Point(12, 170);
            this.serviceListBox.Name = "serviceListBox";
            this.serviceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.serviceListBox.Size = new System.Drawing.Size(293, 94);
            this.serviceListBox.TabIndex = 4;
            // 
            // groupBox_Action
            // 
            this.groupBox_Action.Controls.Add(this.radioRestart);
            this.groupBox_Action.Controls.Add(this.radioStart);
            this.groupBox_Action.Controls.Add(this.radioStop);
            this.groupBox_Action.Location = new System.Drawing.Point(12, 81);
            this.groupBox_Action.Name = "groupBox_Action";
            this.groupBox_Action.Size = new System.Drawing.Size(293, 58);
            this.groupBox_Action.TabIndex = 5;
            this.groupBox_Action.TabStop = false;
            this.groupBox_Action.Text = "Action";
            // 
            // ServiceStartStopConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Action);
            this.Controls.Add(this.serviceListBox);
            this.Controls.Add(this.serviceLabel);
            this.Controls.Add(this.serverSelectLabel);
            this.Controls.Add(this.server_ComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceStartStopConfigurationControl";
            this.Size = new System.Drawing.Size(322, 280);
            this.groupBox_Action.ResumeLayout(false);
            this.groupBox_Action.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.ServerComboBox server_ComboBox;
        private System.Windows.Forms.Label serverSelectLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.RadioButton radioRestart;
        private System.Windows.Forms.RadioButton radioStart;
        private System.Windows.Forms.RadioButton radioStop;
        private System.Windows.Forms.ListBox serviceListBox;
        private System.Windows.Forms.GroupBox groupBox_Action;
    }
}
