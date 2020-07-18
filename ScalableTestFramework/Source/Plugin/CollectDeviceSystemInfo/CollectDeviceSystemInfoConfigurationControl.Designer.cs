namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    partial class CollectDeviceSystemInfoConfigurationControl
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
            this.groupBoxDevice = new System.Windows.Forms.GroupBox();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label_MemoryCollectionNotes = new System.Windows.Forms.Label();
            this.groupBoxDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDevice
            // 
            this.groupBoxDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDevice.Controls.Add(this.assetSelectionControl);
            this.groupBoxDevice.Location = new System.Drawing.Point(6, 57);
            this.groupBoxDevice.Name = "groupBoxDevice";
            this.groupBoxDevice.Size = new System.Drawing.Size(656, 250);
            this.groupBoxDevice.TabIndex = 30;
            this.groupBoxDevice.TabStop = false;
            this.groupBoxDevice.Text = "Select Jedi Devices for Memory Collection";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 24);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(630, 218);
            this.assetSelectionControl.TabIndex = 0;
            this.toolTip1.SetToolTip(this.assetSelectionControl, "Only Jedi devices are supported at this time");
            // 
            // label_MemoryCollectionNotes
            // 
            this.label_MemoryCollectionNotes.AutoSize = true;
            this.label_MemoryCollectionNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_MemoryCollectionNotes.ForeColor = System.Drawing.Color.Red;
            this.label_MemoryCollectionNotes.Location = new System.Drawing.Point(12, 13);
            this.label_MemoryCollectionNotes.Name = "label_MemoryCollectionNotes";
            this.label_MemoryCollectionNotes.Size = new System.Drawing.Size(181, 16);
            this.label_MemoryCollectionNotes.TabIndex = 31;
            this.label_MemoryCollectionNotes.Text = "Memory Collection Notes";
            // 
            // CollectDeviceSystemInfoConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_MemoryCollectionNotes);
            this.Controls.Add(this.groupBoxDevice);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CollectDeviceSystemInfoConfigurationControl";
            this.Size = new System.Drawing.Size(671, 307);
            this.groupBoxDevice.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBoxDevice;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label_MemoryCollectionNotes;
    }
}
