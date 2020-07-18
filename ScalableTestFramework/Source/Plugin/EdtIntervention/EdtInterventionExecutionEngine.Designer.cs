namespace HP.ScalableTest.Plugin.EdtIntervention
{
    partial class EdtInterventionExecutionEngine
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
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.status_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.Location = new System.Drawing.Point(3, 24);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(481, 276);
            this.status_RichTextBox.TabIndex = 5;
            this.status_RichTextBox.Text = "";
            this.status_RichTextBox.WordWrap = false;
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(0, 6);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(100, 15);
            this.status_Label.TabIndex = 4;
            this.status_Label.Text = "Execution Status";
            // 
            // EdtInterventionExecutionEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.status_RichTextBox);
            this.Controls.Add(this.status_Label);
            this.Name = "EdtInterventionExecutionEngine";
            this.Size = new System.Drawing.Size(487, 303);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label status_Label;
    }
}
