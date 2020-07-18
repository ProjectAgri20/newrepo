namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class PoissonDistributionViewForm
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
            Telerik.WinControls.UI.CartesianArea cartesianArea1 = new Telerik.WinControls.UI.CartesianArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoissonDistributionViewForm));
            this.poisson_ChartView = new Telerik.WinControls.UI.RadChartView();
            this.ok_Button = new System.Windows.Forms.Button();
            this.refresh_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.poisson_ChartView)).BeginInit();
            this.SuspendLayout();
            // 
            // poisson_ChartView
            // 
            this.poisson_ChartView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            cartesianArea1.GridDesign.DrawHorizontalFills = false;
            cartesianArea1.GridDesign.DrawVerticalFills = false;
            cartesianArea1.ShowGrid = true;
            this.poisson_ChartView.AreaDesign = cartesianArea1;
            this.poisson_ChartView.Location = new System.Drawing.Point(12, 55);
            this.poisson_ChartView.Name = "poisson_ChartView";
            this.poisson_ChartView.ShowPanZoom = true;
            this.poisson_ChartView.ShowTitle = true;
            this.poisson_ChartView.Size = new System.Drawing.Size(747, 506);
            this.poisson_ChartView.TabIndex = 0;
            this.poisson_ChartView.Text = "PoissonView";
            this.poisson_ChartView.Title = "Poisson Distribution Sample for Load Testing";
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(684, 578);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // refresh_Button
            // 
            this.refresh_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh_Button.Location = new System.Drawing.Point(603, 578);
            this.refresh_Button.Name = "refresh_Button";
            this.refresh_Button.Size = new System.Drawing.Size(75, 23);
            this.refresh_Button.TabIndex = 2;
            this.refresh_Button.Text = "Refresh";
            this.refresh_Button.UseVisualStyleBackColor = true;
            this.refresh_Button.Click += new System.EventHandler(this.refresh_Button_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(747, 43);
            this.label1.TabIndex = 3;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // PoissonDistributionViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AcceptButton = this.ok_Button;
            this.ClientSize = new System.Drawing.Size(771, 613);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.refresh_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.poisson_ChartView);
            this.Name = "PoissonDistributionViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Poisson Distribution Sample View";
            this.Load += new System.EventHandler(this.PoissonDistributionViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.poisson_ChartView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadChartView poisson_ChartView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button refresh_Button;
        private System.Windows.Forms.Label label1;

    }
}