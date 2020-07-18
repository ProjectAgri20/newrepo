namespace HP.ScalableTest.Plugin.PrintFromUsb
{
    partial class PrintFromUsbConfigControl
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
            this.firmware_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBoxfirmware = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsbName = new System.Windows.Forms.TextBox();
            this.groupBoxfirmware.SuspendLayout();
            this.SuspendLayout();
            // 
            // firmware_assetSelectionControl
            // 
            this.firmware_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firmware_assetSelectionControl.Location = new System.Drawing.Point(-2, 3);
            this.firmware_assetSelectionControl.Name = "firmware_assetSelectionControl";
            this.firmware_assetSelectionControl.Size = new System.Drawing.Size(737, 265);
            this.firmware_assetSelectionControl.TabIndex = 1;
            // 
            // groupBoxfirmware
            // 
            this.groupBoxfirmware.Controls.Add(this.label1);
            this.groupBoxfirmware.Controls.Add(this.textBoxUsbName);
            this.groupBoxfirmware.Location = new System.Drawing.Point(0, 274);
            this.groupBoxfirmware.Name = "groupBoxfirmware";
            this.groupBoxfirmware.Size = new System.Drawing.Size(737, 85);
            this.groupBoxfirmware.TabIndex = 5;
            this.groupBoxfirmware.TabStop = false;
            this.groupBoxfirmware.Text = "USB Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Add the USB name and the Plugin will print the First Document from the USB";
            // 
            // textBoxUsbName
            // 
            this.textBoxUsbName.BackColor = System.Drawing.Color.White;
            this.textBoxUsbName.Location = new System.Drawing.Point(6, 22);
            this.textBoxUsbName.Name = "textBoxUsbName";
            this.textBoxUsbName.Size = new System.Drawing.Size(527, 23);
            this.textBoxUsbName.TabIndex = 1;
            // 
            // PrintFromUsbConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxfirmware);
            this.Controls.Add(this.firmware_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintFromUsbConfigControl";
            this.Size = new System.Drawing.Size(738, 360);
            this.groupBoxfirmware.ResumeLayout(false);
            this.groupBoxfirmware.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl firmware_assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBoxfirmware;
        private System.Windows.Forms.TextBox textBoxUsbName;
        private System.Windows.Forms.Label label1;
    }
}
