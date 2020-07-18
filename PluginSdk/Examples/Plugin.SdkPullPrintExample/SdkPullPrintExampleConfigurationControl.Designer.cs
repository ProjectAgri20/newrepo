namespace Plugin.SdkPullPrintExample
{
    partial class SdkPullPrintExampleConfigurationControl
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
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.textBoxButtonName = new System.Windows.Forms.TextBox();
            this.labelButtonName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 87);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(577, 271);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // textBoxButtonName
            // 
            this.textBoxButtonName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxButtonName.Location = new System.Drawing.Point(4, 32);
            this.textBoxButtonName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxButtonName.Name = "textBoxButtonName";
            this.textBoxButtonName.Size = new System.Drawing.Size(576, 23);
            this.textBoxButtonName.TabIndex = 4;
            this.textBoxButtonName.Text = "PullPrint Local";
            // 
            // labelButtonName
            // 
            this.labelButtonName.AutoSize = true;
            this.labelButtonName.Location = new System.Drawing.Point(4, 9);
            this.labelButtonName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelButtonName.Name = "labelButtonName";
            this.labelButtonName.Size = new System.Drawing.Size(135, 15);
            this.labelButtonName.TabIndex = 3;
            this.labelButtonName.Text = "Top Level Button Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Device:";
            // 
            // SdkPullPrintExampleConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxButtonName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelButtonName);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SdkPullPrintExampleConfigurationControl";
            this.Size = new System.Drawing.Size(617, 361);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.TextBox textBoxButtonName;
        private System.Windows.Forms.Label labelButtonName;
        private System.Windows.Forms.Label label1;
    }
}
