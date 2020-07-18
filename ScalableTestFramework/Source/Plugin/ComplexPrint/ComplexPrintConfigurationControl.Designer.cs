namespace HP.ScalableTest.Plugin.ComplexPrint
{
    partial class ComplexPrintConfigurationControl
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.information_Label = new System.Windows.Forms.Label();
            this.configParams_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // information_Label
            // 
            this.information_Label.AutoSize = true;
            this.information_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.information_Label.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.information_Label.Location = new System.Drawing.Point(32, 381);
            this.information_Label.Name = "information_Label";
            this.information_Label.Size = new System.Drawing.Size(490, 13);
            this.information_Label.TabIndex = 12;
            this.information_Label.Text = "Note: Make sure all IPv6 addresses are configured and WS Printer option is enable" +
    "d.";
            // 
            // configParams_Button
            // 
            this.configParams_Button.Location = new System.Drawing.Point(35, 415);
            this.configParams_Button.Name = "configParams_Button";
            this.configParams_Button.Size = new System.Drawing.Size(208, 58);
            this.configParams_Button.TabIndex = 13;
            this.configParams_Button.Text = "Configure Test Parameters";
            this.configParams_Button.UseVisualStyleBackColor = true;
            this.configParams_Button.Click += new System.EventHandler(this.configParams_Button_Click);
            // 
            // ComplexPrintConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.configParams_Button);
            this.Controls.Add(this.information_Label);
            this.Name = "ComplexPrintConfigurationControl";
            this.Size = new System.Drawing.Size(743, 724);
            this.Controls.SetChildIndex(this.information_Label, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.configParams_Button, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label information_Label;
        private System.Windows.Forms.Button configParams_Button;
    }
}
