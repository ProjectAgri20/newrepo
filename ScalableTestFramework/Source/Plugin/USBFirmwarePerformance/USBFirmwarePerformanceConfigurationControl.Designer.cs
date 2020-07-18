namespace HP.ScalableTest.Plugin.USBFirmwarePerformance
{
    partial class USBFirmwarePerformanceConfigurationControl
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
            this.validatetimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.checkBoxValidate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(737, 265);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // validatetimeSpanControl
            // 
            this.validatetimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validatetimeSpanControl.Location = new System.Drawing.Point(212, 304);
            this.validatetimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.validatetimeSpanControl.Name = "validatetimeSpanControl";
            this.validatetimeSpanControl.Size = new System.Drawing.Size(95, 25);
            this.validatetimeSpanControl.TabIndex = 8;
            // 
            // checkBoxValidate
            // 
            this.checkBoxValidate.AutoSize = true;
            this.checkBoxValidate.Checked = true;
            this.checkBoxValidate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxValidate.Location = new System.Drawing.Point(12, 304);
            this.checkBoxValidate.Name = "checkBoxValidate";
            this.checkBoxValidate.Size = new System.Drawing.Size(197, 19);
            this.checkBoxValidate.TabIndex = 7;
            this.checkBoxValidate.Text = "Validate after completion within:";
            this.checkBoxValidate.UseVisualStyleBackColor = true;
            // 
            // USBFirmwarePerformanceConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.validatetimeSpanControl);
            this.Controls.Add(this.checkBoxValidate);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "USBFirmwarePerformanceConfigurationControl";
            this.Size = new System.Drawing.Size(745, 357);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.TimeSpanControl validatetimeSpanControl;
        private System.Windows.Forms.CheckBox checkBoxValidate;
    }
}
