namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    partial class SafeComSimulationExecutionControl
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
            Framework.ExecutionServices.SystemTrace.LogDebug("Clearing remaining print jobs");
            _safecomController.DeleteAllJobs();

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
            this.status_TextBox = new System.Windows.Forms.TextBox();
            this.status_Label = new System.Windows.Forms.Label();
            this.session_Label = new System.Windows.Forms.Label();
            this.sessionLabel_Label = new System.Windows.Forms.Label();
            this.server_Label = new System.Windows.Forms.Label();
            this.serverLabel_Label = new System.Windows.Forms.Label();
            this.deviceIP_Label = new System.Windows.Forms.Label();
            this.deviceIPLabel_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_TextBox
            // 
            this.status_TextBox.Location = new System.Drawing.Point(2, 110);
            this.status_TextBox.Multiline = true;
            this.status_TextBox.Name = "status_TextBox";
            this.status_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.status_TextBox.Size = new System.Drawing.Size(486, 167);
            this.status_TextBox.TabIndex = 15;
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(-1, 87);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(43, 13);
            this.status_Label.TabIndex = 14;
            this.status_Label.Text = "Status";
            // 
            // session_Label
            // 
            this.session_Label.AutoSize = true;
            this.session_Label.Location = new System.Drawing.Point(174, 6);
            this.session_Label.Name = "session_Label";
            this.session_Label.Size = new System.Drawing.Size(27, 13);
            this.session_Label.TabIndex = 13;
            this.session_Label.Text = "N/A";
            // 
            // sessionLabel_Label
            // 
            this.sessionLabel_Label.AutoSize = true;
            this.sessionLabel_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionLabel_Label.Location = new System.Drawing.Point(2, 6);
            this.sessionLabel_Label.Name = "sessionLabel_Label";
            this.sessionLabel_Label.Size = new System.Drawing.Size(70, 13);
            this.sessionLabel_Label.TabIndex = 12;
            this.sessionLabel_Label.Text = "Session Id:";
            // 
            // server_Label
            // 
            this.server_Label.AutoSize = true;
            this.server_Label.Location = new System.Drawing.Point(174, 32);
            this.server_Label.Name = "server_Label";
            this.server_Label.Size = new System.Drawing.Size(27, 13);
            this.server_Label.TabIndex = 11;
            this.server_Label.Text = "N/A";
            // 
            // serverLabel_Label
            // 
            this.serverLabel_Label.AutoSize = true;
            this.serverLabel_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverLabel_Label.Location = new System.Drawing.Point(2, 32);
            this.serverLabel_Label.Name = "serverLabel_Label";
            this.serverLabel_Label.Size = new System.Drawing.Size(101, 13);
            this.serverLabel_Label.TabIndex = 10;
            this.serverLabel_Label.Text = "Safecom Server:";
            // 
            // deviceIP_Label
            // 
            this.deviceIP_Label.AutoSize = true;
            this.deviceIP_Label.Location = new System.Drawing.Point(174, 58);
            this.deviceIP_Label.Name = "deviceIP_Label";
            this.deviceIP_Label.Size = new System.Drawing.Size(27, 13);
            this.deviceIP_Label.TabIndex = 9;
            this.deviceIP_Label.Text = "N/A";
            // 
            // deviceIPLabel_Label
            // 
            this.deviceIPLabel_Label.AutoSize = true;
            this.deviceIPLabel_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceIPLabel_Label.Location = new System.Drawing.Point(2, 58);
            this.deviceIPLabel_Label.Name = "deviceIPLabel_Label";
            this.deviceIPLabel_Label.Size = new System.Drawing.Size(100, 13);
            this.deviceIPLabel_Label.TabIndex = 8;
            this.deviceIPLabel_Label.Text = "Device Address:";
            // 
            // SafeComSimulationExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.status_TextBox);
            this.Controls.Add(this.status_Label);
            this.Controls.Add(this.session_Label);
            this.Controls.Add(this.sessionLabel_Label);
            this.Controls.Add(this.server_Label);
            this.Controls.Add(this.serverLabel_Label);
            this.Controls.Add(this.deviceIP_Label);
            this.Controls.Add(this.deviceIPLabel_Label);
            this.Name = "SafeComSimulationExecutionControl";
            this.Size = new System.Drawing.Size(494, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox status_TextBox;
        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.Label session_Label;
        private System.Windows.Forms.Label sessionLabel_Label;
        private System.Windows.Forms.Label server_Label;
        private System.Windows.Forms.Label serverLabel_Label;
        private System.Windows.Forms.Label deviceIP_Label;
        private System.Windows.Forms.Label deviceIPLabel_Label;
    }
}
