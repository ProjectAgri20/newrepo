namespace HP.STF.DeviceRestart
{
	partial class FrmReboot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReboot));
            this.RbtnExit = new Telerik.WinControls.UI.RadButton();
            this.RbtnReboot = new Telerik.WinControls.UI.RadButton();
            this.RlblFieldName = new Telerik.WinControls.UI.RadLabel();
            this.RlblIPAddress = new Telerik.WinControls.UI.RadLabel();
            this.RtxtIpAddress = new Telerik.WinControls.UI.RadTextBox();
            this.RgbRebootOptions = new Telerik.WinControls.UI.RadGroupBox();
            this.RrbFactoryRestory = new Telerik.WinControls.UI.RadRadioButton();
            this.RrbNvramReboot = new Telerik.WinControls.UI.RadRadioButton();
            this.RrbPowerCycle = new Telerik.WinControls.UI.RadRadioButton();
            this.RlblRebootMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RbtnExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RbtnReboot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlblFieldName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlblIPAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RtxtIpAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RgbRebootOptions)).BeginInit();
            this.RgbRebootOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RrbFactoryRestory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrbNvramReboot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrbPowerCycle)).BeginInit();
            this.SuspendLayout();
            // 
            // RbtnExit
            // 
            this.RbtnExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.RbtnExit.Location = new System.Drawing.Point(557, 475);
            this.RbtnExit.Name = "RbtnExit";
            this.RbtnExit.Size = new System.Drawing.Size(110, 32);
            this.RbtnExit.TabIndex = 6;
            this.RbtnExit.Text = "E&xit";
            this.RbtnExit.Click += new System.EventHandler(this.RbtnExit_Click);
            // 
            // RbtnReboot
            // 
            this.RbtnReboot.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.RbtnReboot.Location = new System.Drawing.Point(12, 475);
            this.RbtnReboot.Name = "RbtnReboot";
            this.RbtnReboot.Size = new System.Drawing.Size(110, 32);
            this.RbtnReboot.TabIndex = 5;
            this.RbtnReboot.Text = "&Reboot";
            this.RbtnReboot.Click += new System.EventHandler(this.RbtnReboot_Click);
            // 
            // RlblFieldName
            // 
            this.RlblFieldName.AutoSize = false;
            this.RlblFieldName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RlblFieldName.ForeColor = System.Drawing.Color.Maroon;
            this.RlblFieldName.Location = new System.Drawing.Point(12, 23);
            this.RlblFieldName.Name = "RlblFieldName";
            this.RlblFieldName.Size = new System.Drawing.Size(655, 27);
            this.RlblFieldName.TabIndex = 2;
            this.RlblFieldName.Text = "Enter Printer\'s IP Address in the text box";
            this.RlblFieldName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RlblIPAddress
            // 
            this.RlblIPAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RlblIPAddress.ForeColor = System.Drawing.Color.Maroon;
            this.RlblIPAddress.Location = new System.Drawing.Point(13, 84);
            this.RlblIPAddress.Name = "RlblIPAddress";
            this.RlblIPAddress.Size = new System.Drawing.Size(73, 21);
            this.RlblIPAddress.TabIndex = 3;
            this.RlblIPAddress.Text = "IP Address";
            // 
            // RtxtIpAddress
            // 
            this.RtxtIpAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RtxtIpAddress.Location = new System.Drawing.Point(93, 84);
            this.RtxtIpAddress.Name = "RtxtIpAddress";
            this.RtxtIpAddress.Size = new System.Drawing.Size(176, 23);
            this.RtxtIpAddress.TabIndex = 0;
            this.RtxtIpAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RtxtIpAddress_KeyDown);
            // 
            // RgbRebootOptions
            // 
            this.RgbRebootOptions.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.RgbRebootOptions.Controls.Add(this.RrbFactoryRestory);
            this.RgbRebootOptions.Controls.Add(this.RrbNvramReboot);
            this.RgbRebootOptions.Controls.Add(this.RrbPowerCycle);
            this.RgbRebootOptions.HeaderText = "Reboot Options";
            this.RgbRebootOptions.Location = new System.Drawing.Point(351, 84);
            this.RgbRebootOptions.Name = "RgbRebootOptions";
            this.RgbRebootOptions.Size = new System.Drawing.Size(299, 157);
            this.RgbRebootOptions.TabIndex = 1;
            this.RgbRebootOptions.Text = "Reboot Options";
            // 
            // RrbFactoryRestory
            // 
            this.RrbFactoryRestory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrbFactoryRestory.ForeColor = System.Drawing.Color.Maroon;
            this.RrbFactoryRestory.Location = new System.Drawing.Point(28, 117);
            this.RrbFactoryRestory.Name = "RrbFactoryRestory";
            this.RrbFactoryRestory.Size = new System.Drawing.Size(243, 21);
            this.RrbFactoryRestory.TabIndex = 4;
            this.RrbFactoryRestory.Tag = "6";
            this.RrbFactoryRestory.Text = "NVRAM/ Factory Restore Power Cycle";
            this.RrbFactoryRestory.Click += new System.EventHandler(this.RrbRebootClicked);
            // 
            // RrbNvramReboot
            // 
            this.RrbNvramReboot.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrbNvramReboot.ForeColor = System.Drawing.Color.Maroon;
            this.RrbNvramReboot.Location = new System.Drawing.Point(28, 74);
            this.RrbNvramReboot.Name = "RrbNvramReboot";
            this.RrbNvramReboot.Size = new System.Drawing.Size(143, 21);
            this.RrbNvramReboot.TabIndex = 3;
            this.RrbNvramReboot.Tag = "5";
            this.RrbNvramReboot.Text = "NVRAM Power Cycle";
            this.RrbNvramReboot.Click += new System.EventHandler(this.RrbRebootClicked);
            // 
            // RrbPowerCycle
            // 
            this.RrbPowerCycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RrbPowerCycle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RrbPowerCycle.ForeColor = System.Drawing.Color.Maroon;
            this.RrbPowerCycle.Location = new System.Drawing.Point(28, 31);
            this.RrbPowerCycle.Name = "RrbPowerCycle";
            this.RrbPowerCycle.Size = new System.Drawing.Size(142, 21);
            this.RrbPowerCycle.TabIndex = 2;
            this.RrbPowerCycle.Tag = "4";
            this.RrbPowerCycle.Text = "General Power Cycle";
            this.RrbPowerCycle.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.RrbPowerCycle.Click += new System.EventHandler(this.RrbRebootClicked);
            // 
            // RlblRebootMessage
            // 
            this.RlblRebootMessage.AutoSize = true;
            this.RlblRebootMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RlblRebootMessage.ForeColor = System.Drawing.Color.Maroon;
            this.RlblRebootMessage.Location = new System.Drawing.Point(19, 373);
            this.RlblRebootMessage.Name = "RlblRebootMessage";
            this.RlblRebootMessage.Size = new System.Drawing.Size(52, 17);
            this.RlblRebootMessage.TabIndex = 7;
            this.RlblRebootMessage.Text = "label1";
            // 
            // FrmReboot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 519);
            this.Controls.Add(this.RlblRebootMessage);
            this.Controls.Add(this.RgbRebootOptions);
            this.Controls.Add(this.RtxtIpAddress);
            this.Controls.Add(this.RlblIPAddress);
            this.Controls.Add(this.RlblFieldName);
            this.Controls.Add(this.RbtnReboot);
            this.Controls.Add(this.RbtnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReboot";
            this.Text = "RDL Reboot Printer";
            ((System.ComponentModel.ISupportInitialize)(this.RbtnExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RbtnReboot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlblFieldName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RlblIPAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RtxtIpAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RgbRebootOptions)).EndInit();
            this.RgbRebootOptions.ResumeLayout(false);
            this.RgbRebootOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RrbFactoryRestory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrbNvramReboot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RrbPowerCycle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Telerik.WinControls.UI.RadButton RbtnExit;
		private Telerik.WinControls.UI.RadButton RbtnReboot;
		private Telerik.WinControls.UI.RadLabel RlblFieldName;
		private Telerik.WinControls.UI.RadLabel RlblIPAddress;
		private Telerik.WinControls.UI.RadTextBox RtxtIpAddress;
		private Telerik.WinControls.UI.RadGroupBox RgbRebootOptions;
		private Telerik.WinControls.UI.RadRadioButton RrbFactoryRestory;
		private Telerik.WinControls.UI.RadRadioButton RrbNvramReboot;
		private Telerik.WinControls.UI.RadRadioButton RrbPowerCycle;
        private System.Windows.Forms.Label RlblRebootMessage;
	}
}

