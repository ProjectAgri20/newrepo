namespace WebJetAdminComparisonTool
{
    partial class WjaComparisonTool
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
            _excelApp.Dispose();
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
            this.wjaReport_TextBox = new System.Windows.Forms.TextBox();
            this.eprInjectorReport_TextBox = new System.Windows.Forms.TextBox();
            this.wjaReport_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.results_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.results_Label = new System.Windows.Forms.Label();
            this.report_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openWjaReport_Button = new System.Windows.Forms.Button();
            this.openEprReport_Button = new System.Windows.Forms.Button();
            this.compare_Button = new System.Windows.Forms.Button();
            this.clearTextBox_Button = new System.Windows.Forms.Button();
            this.title_Label = new System.Windows.Forms.Label();
            this.title_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // wjaReport_TextBox
            // 
            this.wjaReport_TextBox.Location = new System.Drawing.Point(196, 88);
            this.wjaReport_TextBox.Name = "wjaReport_TextBox";
            this.wjaReport_TextBox.ReadOnly = true;
            this.wjaReport_TextBox.Size = new System.Drawing.Size(347, 23);
            this.wjaReport_TextBox.TabIndex = 0;
            // 
            // eprInjectorReport_TextBox
            // 
            this.eprInjectorReport_TextBox.Location = new System.Drawing.Point(196, 130);
            this.eprInjectorReport_TextBox.Name = "eprInjectorReport_TextBox";
            this.eprInjectorReport_TextBox.ReadOnly = true;
            this.eprInjectorReport_TextBox.Size = new System.Drawing.Size(347, 23);
            this.eprInjectorReport_TextBox.TabIndex = 1;
            // 
            // wjaReport_Label
            // 
            this.wjaReport_Label.AutoSize = true;
            this.wjaReport_Label.Location = new System.Drawing.Point(12, 91);
            this.wjaReport_Label.Name = "wjaReport_Label";
            this.wjaReport_Label.Size = new System.Drawing.Size(122, 15);
            this.wjaReport_Label.TabIndex = 2;
            this.wjaReport_Label.Text = "Select the WJA report:";
            this.title_ToolTip.SetToolTip(this.wjaReport_Label, "Device Export Report");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select the EPR Injector Summary";
            this.title_ToolTip.SetToolTip(this.label1, "Activity Summary Report");
            // 
            // results_RichTextBox
            // 
            this.results_RichTextBox.Location = new System.Drawing.Point(14, 203);
            this.results_RichTextBox.Name = "results_RichTextBox";
            this.results_RichTextBox.Size = new System.Drawing.Size(576, 277);
            this.results_RichTextBox.TabIndex = 4;
            this.results_RichTextBox.Text = "";
            // 
            // results_Label
            // 
            this.results_Label.AutoSize = true;
            this.results_Label.Location = new System.Drawing.Point(12, 178);
            this.results_Label.Name = "results_Label";
            this.results_Label.Size = new System.Drawing.Size(44, 15);
            this.results_Label.TabIndex = 5;
            this.results_Label.Text = "Results";
            // 
            // openWjaReport_Button
            // 
            this.openWjaReport_Button.Location = new System.Drawing.Point(549, 87);
            this.openWjaReport_Button.Name = "openWjaReport_Button";
            this.openWjaReport_Button.Size = new System.Drawing.Size(41, 23);
            this.openWjaReport_Button.TabIndex = 6;
            this.openWjaReport_Button.Text = "...";
            this.title_ToolTip.SetToolTip(this.openWjaReport_Button, "Click to open a file browser");
            this.openWjaReport_Button.UseVisualStyleBackColor = true;
            this.openWjaReport_Button.Click += new System.EventHandler(this.openWjaReport_Button_Click);
            // 
            // openEprReport_Button
            // 
            this.openEprReport_Button.Location = new System.Drawing.Point(549, 129);
            this.openEprReport_Button.Name = "openEprReport_Button";
            this.openEprReport_Button.Size = new System.Drawing.Size(41, 23);
            this.openEprReport_Button.TabIndex = 7;
            this.openEprReport_Button.Text = "...";
            this.title_ToolTip.SetToolTip(this.openEprReport_Button, "Click to open a file browser");
            this.openEprReport_Button.UseVisualStyleBackColor = true;
            this.openEprReport_Button.Click += new System.EventHandler(this.openEprReport_Button_Click);
            // 
            // compare_Button
            // 
            this.compare_Button.Location = new System.Drawing.Point(451, 174);
            this.compare_Button.Name = "compare_Button";
            this.compare_Button.Size = new System.Drawing.Size(139, 23);
            this.compare_Button.TabIndex = 8;
            this.compare_Button.Text = "Compare Reports";
            this.title_ToolTip.SetToolTip(this.compare_Button, "Click to compare the two reports");
            this.compare_Button.UseVisualStyleBackColor = true;
            this.compare_Button.Click += new System.EventHandler(this.compare_Button_Click);
            // 
            // clearTextBox_Button
            // 
            this.clearTextBox_Button.Location = new System.Drawing.Point(358, 174);
            this.clearTextBox_Button.Name = "clearTextBox_Button";
            this.clearTextBox_Button.Size = new System.Drawing.Size(87, 23);
            this.clearTextBox_Button.TabIndex = 9;
            this.clearTextBox_Button.Text = "Clear Log";
            this.title_ToolTip.SetToolTip(this.clearTextBox_Button, "Click to clear the results text box");
            this.clearTextBox_Button.UseVisualStyleBackColor = true;
            this.clearTextBox_Button.Click += new System.EventHandler(this.clearTextBox_Button_Click);
            // 
            // title_Label
            // 
            this.title_Label.AutoSize = true;
            this.title_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_Label.Location = new System.Drawing.Point(9, 18);
            this.title_Label.Name = "title_Label";
            this.title_Label.Size = new System.Drawing.Size(475, 25);
            this.title_Label.TabIndex = 10;
            this.title_Label.Text = "Web Jetadmin and Epr Injector Comparison Tool";
            // 
            // title_ToolTip
            // 
            this.title_ToolTip.AutoPopDelay = 10000;
            this.title_ToolTip.InitialDelay = 500;
            this.title_ToolTip.ReshowDelay = 100;
            // 
            // WjaComparisonTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 495);
            this.Controls.Add(this.title_Label);
            this.Controls.Add(this.clearTextBox_Button);
            this.Controls.Add(this.compare_Button);
            this.Controls.Add(this.openEprReport_Button);
            this.Controls.Add(this.openWjaReport_Button);
            this.Controls.Add(this.results_Label);
            this.Controls.Add(this.results_RichTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wjaReport_Label);
            this.Controls.Add(this.eprInjectorReport_TextBox);
            this.Controls.Add(this.wjaReport_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WjaComparisonTool";
            this.Text = "Web Jetadmin Comparison Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox wjaReport_TextBox;
        private System.Windows.Forms.TextBox eprInjectorReport_TextBox;
        private System.Windows.Forms.Label wjaReport_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox results_RichTextBox;
        private System.Windows.Forms.Label results_Label;
        private System.Windows.Forms.OpenFileDialog report_OpenFileDialog;
        private System.Windows.Forms.Button openWjaReport_Button;
        private System.Windows.Forms.Button openEprReport_Button;
        private System.Windows.Forms.Button compare_Button;
        private System.Windows.Forms.Button clearTextBox_Button;
        private System.Windows.Forms.Label title_Label;
        private System.Windows.Forms.ToolTip title_ToolTip;
    }
}

