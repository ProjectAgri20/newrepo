namespace HP.ScalableTest.Plugin.HpcrSimulation
{
    partial class HpcrSimulationExecController
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
            this.output_TextBox = new System.Windows.Forms.TextBox();
            this.textBox_Originator = new System.Windows.Forms.TextBox();
            this.textBox_Recipients = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_RecipientType = new System.Windows.Forms.TextBox();
            this.textBox_Documents = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // output_TextBox
            // 
            this.output_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output_TextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_TextBox.Location = new System.Drawing.Point(3, 124);
            this.output_TextBox.Multiline = true;
            this.output_TextBox.Name = "output_TextBox";
            this.output_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.output_TextBox.Size = new System.Drawing.Size(333, 208);
            this.output_TextBox.TabIndex = 5;
            // 
            // textBox_Originator
            // 
            this.textBox_Originator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Originator.Location = new System.Drawing.Point(91, 4);
            this.textBox_Originator.Name = "textBox_Originator";
            this.textBox_Originator.Size = new System.Drawing.Size(232, 20);
            this.textBox_Originator.TabIndex = 6;
            // 
            // textBox_Recipients
            // 
            this.textBox_Recipients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Recipients.Location = new System.Drawing.Point(91, 30);
            this.textBox_Recipients.Name = "textBox_Recipients";
            this.textBox_Recipients.Size = new System.Drawing.Size(232, 20);
            this.textBox_Recipients.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Originator:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Documents:";
            // 
            // textBox_RecipientType
            // 
            this.textBox_RecipientType.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_RecipientType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_RecipientType.Location = new System.Drawing.Point(6, 33);
            this.textBox_RecipientType.Name = "textBox_RecipientType";
            this.textBox_RecipientType.Size = new System.Drawing.Size(77, 13);
            this.textBox_RecipientType.TabIndex = 6;
            this.textBox_RecipientType.Text = "To:";
            // 
            // textBox_Documents
            // 
            this.textBox_Documents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Documents.Location = new System.Drawing.Point(91, 57);
            this.textBox_Documents.Multiline = true;
            this.textBox_Documents.Name = "textBox_Documents";
            this.textBox_Documents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Documents.Size = new System.Drawing.Size(232, 61);
            this.textBox_Documents.TabIndex = 8;
            // 
            // HpcrActivityExecutionController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_Documents);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_RecipientType);
            this.Controls.Add(this.textBox_Recipients);
            this.Controls.Add(this.textBox_Originator);
            this.Controls.Add(this.output_TextBox);
            this.Name = "HpcrActivityExecutionController";
            this.Size = new System.Drawing.Size(339, 335);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox output_TextBox;
        private System.Windows.Forms.TextBox textBox_Originator;
        private System.Windows.Forms.TextBox textBox_Recipients;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_RecipientType;
        private System.Windows.Forms.TextBox textBox_Documents;
    }
}
