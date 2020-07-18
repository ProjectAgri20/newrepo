namespace HP.ScalableTest.UI.Charting
{
    partial class CalculatedSeriesForm
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
            this.apply_Button = new System.Windows.Forms.Button();
            this.calculatedSeries_ListBox = new System.Windows.Forms.ListBox();
            this.add_Button = new System.Windows.Forms.Button();
            this.remove_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.includedSeries_ListBox = new System.Windows.Forms.CheckedListBox();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(451, 316);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 2;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // calculatedSeries_ListBox
            // 
            this.calculatedSeries_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.calculatedSeries_ListBox.FormattingEnabled = true;
            this.calculatedSeries_ListBox.IntegralHeight = false;
            this.calculatedSeries_ListBox.Location = new System.Drawing.Point(12, 30);
            this.calculatedSeries_ListBox.Name = "calculatedSeries_ListBox";
            this.calculatedSeries_ListBox.Size = new System.Drawing.Size(156, 274);
            this.calculatedSeries_ListBox.TabIndex = 4;
            this.calculatedSeries_ListBox.SelectedIndexChanged += new System.EventHandler(this.calculatedSeries_ListBox_SelectedIndexChanged);
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.add_Button.Location = new System.Drawing.Point(12, 316);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(75, 23);
            this.add_Button.TabIndex = 5;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // remove_Button
            // 
            this.remove_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.remove_Button.Enabled = false;
            this.remove_Button.Location = new System.Drawing.Point(93, 316);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(75, 23);
            this.remove_Button.TabIndex = 5;
            this.remove_Button.Text = "Remove";
            this.remove_Button.UseVisualStyleBackColor = true;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Calculated Series";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Series Included in Total";
            // 
            // includedSeries_ListBox
            // 
            this.includedSeries_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.includedSeries_ListBox.CheckOnClick = true;
            this.includedSeries_ListBox.Enabled = false;
            this.includedSeries_ListBox.FormattingEnabled = true;
            this.includedSeries_ListBox.HorizontalScrollbar = true;
            this.includedSeries_ListBox.Location = new System.Drawing.Point(200, 30);
            this.includedSeries_ListBox.Name = "includedSeries_ListBox";
            this.includedSeries_ListBox.Size = new System.Drawing.Size(326, 274);
            this.includedSeries_ListBox.TabIndex = 8;
            this.includedSeries_ListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.includedSeries_ListBox_ItemCheck);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(370, 316);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 9;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // CalculatedSeriesForm
            // 
            this.AcceptButton = this.apply_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(538, 351);
            this.ControlBox = false;
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.includedSeries_ListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.remove_Button);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.calculatedSeries_ListBox);
            this.Controls.Add(this.apply_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalculatedSeriesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calculated Series";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.ListBox calculatedSeries_ListBox;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button remove_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox includedSeries_ListBox;
        private System.Windows.Forms.Button cancel_Button;
    }
}