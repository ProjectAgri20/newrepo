namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class ScenarioSummaryForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.save_Button = new System.Windows.Forms.Button();
			this.cancel_Button = new System.Windows.Forms.Button();
			this.scenario_radPageView = new Telerik.WinControls.UI.RadPageView();
			((System.ComponentModel.ISupportInitialize)(this.scenario_radPageView)).BeginInit();
			this.SuspendLayout();
			// 
			// save_Button
			// 
			this.save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.save_Button.Location = new System.Drawing.Point(705, 637);
			this.save_Button.Name = "save_Button";
			this.save_Button.Size = new System.Drawing.Size(85, 28);
			this.save_Button.TabIndex = 1;
			this.save_Button.Text = "Save Changes";
			this.save_Button.UseVisualStyleBackColor = true;
			this.save_Button.Click += new System.EventHandler(this.ok_Button_Click);
			// 
			// cancel_Button
			// 
			this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel_Button.Location = new System.Drawing.Point(624, 637);
			this.cancel_Button.Name = "cancel_Button";
			this.cancel_Button.Size = new System.Drawing.Size(75, 28);
			this.cancel_Button.TabIndex = 2;
			this.cancel_Button.Text = "Cancel";
			this.cancel_Button.UseVisualStyleBackColor = true;
			this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
			// 
			// scenario_radPageView
			// 
			this.scenario_radPageView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.scenario_radPageView.Location = new System.Drawing.Point(12, 12);
			this.scenario_radPageView.Name = "scenario_radPageView";
			this.scenario_radPageView.Size = new System.Drawing.Size(778, 619);
			this.scenario_radPageView.TabIndex = 4;
			this.scenario_radPageView.Text = "Scenario Summay";
			((Telerik.WinControls.UI.RadPageViewStripElement)(this.scenario_radPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
			// 
			// ScenarioSummaryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(802, 671);
			this.Controls.Add(this.scenario_radPageView);
			this.Controls.Add(this.cancel_Button);
			this.Controls.Add(this.save_Button);
			this.Name = "ScenarioSummaryForm";
			this.Text = "Virtual Resources";
			((System.ComponentModel.ISupportInitialize)(this.scenario_radPageView)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button save_Button;
        private System.Windows.Forms.Button cancel_Button;
        private Telerik.WinControls.UI.RadPageView scenario_radPageView;

    }
}

