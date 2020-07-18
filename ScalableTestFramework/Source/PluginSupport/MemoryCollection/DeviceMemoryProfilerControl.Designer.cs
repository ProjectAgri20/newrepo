namespace HP.ScalableTest.PluginSupport.MemoryCollection
{
    partial class DeviceMemoryProfilerControl
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
            this.numericUpDownCountSampleTarget = new System.Windows.Forms.NumericUpDown();
            this.checkBoxCountBased = new System.Windows.Forms.CheckBox();
            this.checkBoxTimeBased = new System.Windows.Forms.CheckBox();
            this.timeSpanControlSampleTarget = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCountSampleTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownCountSampleTarget
            // 
            this.numericUpDownCountSampleTarget.Enabled = false;
            this.numericUpDownCountSampleTarget.Location = new System.Drawing.Point(156, 33);
            this.numericUpDownCountSampleTarget.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCountSampleTarget.Name = "numericUpDownCountSampleTarget";
            this.numericUpDownCountSampleTarget.Size = new System.Drawing.Size(98, 20);
            this.numericUpDownCountSampleTarget.TabIndex = 35;
            // 
            // checkBoxCountBased
            // 
            this.checkBoxCountBased.AutoSize = true;
            this.checkBoxCountBased.Location = new System.Drawing.Point(4, 34);
            this.checkBoxCountBased.Name = "checkBoxCountBased";
            this.checkBoxCountBased.Size = new System.Drawing.Size(145, 17);
            this.checkBoxCountBased.TabIndex = 33;
            this.checkBoxCountBased.Text = "Sample at count intervals";
            this.checkBoxCountBased.UseVisualStyleBackColor = true;
            this.checkBoxCountBased.CheckedChanged += new System.EventHandler(this.checkBoxCountBased_CheckedChanged);
            // 
            // checkBoxTimeBased
            // 
            this.checkBoxTimeBased.AutoSize = true;
            this.checkBoxTimeBased.Location = new System.Drawing.Point(4, 7);
            this.checkBoxTimeBased.Name = "checkBoxTimeBased";
            this.checkBoxTimeBased.Size = new System.Drawing.Size(143, 17);
            this.checkBoxTimeBased.TabIndex = 34;
            this.checkBoxTimeBased.Text = "Sample at timed intervals";
            this.checkBoxTimeBased.UseVisualStyleBackColor = true;
            this.checkBoxTimeBased.CheckedChanged += new System.EventHandler(this.checkBoxTimeBased_CheckedChanged);
            // 
            // timeSpanControlSampleTarget
            // 
            this.timeSpanControlSampleTarget.AutoSize = true;
            this.timeSpanControlSampleTarget.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeSpanControlSampleTarget.Enabled = false;
            this.timeSpanControlSampleTarget.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeSpanControlSampleTarget.Location = new System.Drawing.Point(156, 4);
            this.timeSpanControlSampleTarget.Margin = new System.Windows.Forms.Padding(0);
            this.timeSpanControlSampleTarget.Name = "timeSpanControlSampleTarget";
            this.timeSpanControlSampleTarget.Size = new System.Drawing.Size(98, 26);
            this.timeSpanControlSampleTarget.TabIndex = 32;
            // 
            // DeviceMemoryProfilerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownCountSampleTarget);
            this.Controls.Add(this.checkBoxCountBased);
            this.Controls.Add(this.checkBoxTimeBased);
            this.Controls.Add(this.timeSpanControlSampleTarget);
            this.Name = "DeviceMemoryProfilerControl";
            this.Size = new System.Drawing.Size(259, 58);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCountSampleTarget)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownCountSampleTarget;
        private System.Windows.Forms.CheckBox checkBoxCountBased;
        private System.Windows.Forms.CheckBox checkBoxTimeBased;
        private HP.ScalableTest.Framework.UI.TimeSpanControl timeSpanControlSampleTarget;
        private Framework.UI.FieldValidator fieldValidator;
    }
}
