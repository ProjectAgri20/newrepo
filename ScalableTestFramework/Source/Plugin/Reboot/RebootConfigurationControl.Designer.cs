namespace HP.ScalableTest.Plugin.Reboot
{
    partial class RebootConfigurationControl
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
            this.paperless_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(692, 347);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // paperless_checkBox
            // 
            this.paperless_checkBox.AutoSize = true;
            this.paperless_checkBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.paperless_checkBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.paperless_checkBox.Location = new System.Drawing.Point(3, 373);
            this.paperless_checkBox.Name = "paperless_checkBox";
            this.paperless_checkBox.Size = new System.Drawing.Size(183, 19);
            this.paperless_checkBox.TabIndex = 1;
            this.paperless_checkBox.Text = "Enable Paperless After Reboot";
            this.paperless_checkBox.UseVisualStyleBackColor = true;
            // 
            // RebootConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paperless_checkBox);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RebootConfigurationControl";
            this.Size = new System.Drawing.Size(699, 422);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.CheckBox paperless_checkBox;
    }
}
