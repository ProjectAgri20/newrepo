namespace HP.ScalableTest.Plugin.mPrint
{
    partial class mPrintConfigurationControl
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
            this.mPrintServer_Label = new System.Windows.Forms.Label();
            this.attachments_Label = new System.Windows.Forms.Label();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.mPrint_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.queueIndex_TextBox = new System.Windows.Forms.TextBox();
            this.queueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mPrintServer_Label
            // 
            this.mPrintServer_Label.AutoSize = true;
            this.mPrintServer_Label.Location = new System.Drawing.Point(13, 24);
            this.mPrintServer_Label.Name = "mPrintServer_Label";
            this.mPrintServer_Label.Size = new System.Drawing.Size(78, 15);
            this.mPrintServer_Label.TabIndex = 1;
            this.mPrintServer_Label.Text = "mPrint Server";
            // 
            // attachments_Label
            // 
            this.attachments_Label.AutoSize = true;
            this.attachments_Label.Location = new System.Drawing.Point(13, 134);
            this.attachments_Label.Name = "attachments_Label";
            this.attachments_Label.Size = new System.Drawing.Size(78, 15);
            this.attachments_Label.TabIndex = 4;
            this.attachments_Label.Text = "Attachments:";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(16, 111);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(657, 205);
            this.documentSelectionControl.TabIndex = 5;
            // 
            // mPrint_ServerComboBox
            // 
            this.mPrint_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mPrint_ServerComboBox.Location = new System.Drawing.Point(107, 15);
            this.mPrint_ServerComboBox.Name = "mPrint_ServerComboBox";
            this.mPrint_ServerComboBox.Size = new System.Drawing.Size(233, 24);
            this.mPrint_ServerComboBox.TabIndex = 7;
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new System.Drawing.Point(13, 52);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(336, 15);
            this.warningLabel.TabIndex = 8;
            this.warningLabel.Text = "Currently Only Supports PDF and JPG File Types (Includes Sets)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(346, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Does not work on Win7 VMs";
            // 
            // queueIndex_TextBox
            // 
            this.queueIndex_TextBox.Location = new System.Drawing.Point(153, 82);
            this.queueIndex_TextBox.Name = "queueIndex_TextBox";
            this.queueIndex_TextBox.Size = new System.Drawing.Size(35, 23);
            this.queueIndex_TextBox.TabIndex = 10;
            // 
            // queueLabel
            // 
            this.queueLabel.AutoSize = true;
            this.queueLabel.Location = new System.Drawing.Point(13, 90);
            this.queueLabel.Name = "queueLabel";
            this.queueLabel.Size = new System.Drawing.Size(136, 15);
            this.queueLabel.TabIndex = 11;
            this.queueLabel.Text = "Queue Index to Print (#):";
            // 
            // mPrintConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queueLabel);
            this.Controls.Add(this.queueIndex_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.mPrint_ServerComboBox);
            this.Controls.Add(this.documentSelectionControl);
            this.Controls.Add(this.attachments_Label);
            this.Controls.Add(this.mPrintServer_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "mPrintConfigurationControl";
            this.Size = new System.Drawing.Size(705, 327);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label mPrintServer_Label;
        private System.Windows.Forms.Label attachments_Label;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Framework.UI.ServerComboBox mPrint_ServerComboBox;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox queueIndex_TextBox;
        private System.Windows.Forms.Label queueLabel;
    }
}
