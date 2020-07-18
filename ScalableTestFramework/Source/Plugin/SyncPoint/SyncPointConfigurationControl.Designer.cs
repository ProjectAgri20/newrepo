namespace HP.ScalableTest.Plugin.SyncPoint
{
    partial class SyncPointConfigurationControl
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
            this.eventName_TextBox = new System.Windows.Forms.TextBox();
            this.eventName_Label = new System.Windows.Forms.Label();
            this.signalEvent_RadioButton = new System.Windows.Forms.RadioButton();
            this.waitForEvent_RadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // eventName_TextBox
            // 
            this.eventName_TextBox.Location = new System.Drawing.Point(125, 20);
            this.eventName_TextBox.Name = "eventName_TextBox";
            this.eventName_TextBox.Size = new System.Drawing.Size(206, 23);
            this.eventName_TextBox.TabIndex = 0;
            // 
            // eventName_Label
            // 
            this.eventName_Label.AutoSize = true;
            this.eventName_Label.Location = new System.Drawing.Point(20, 23);
            this.eventName_Label.Name = "eventName_Label";
            this.eventName_Label.Size = new System.Drawing.Size(97, 15);
            this.eventName_Label.TabIndex = 1;
            this.eventName_Label.Text = "Sync event name";
            // 
            // signalEvent_RadioButton
            // 
            this.signalEvent_RadioButton.AutoSize = true;
            this.signalEvent_RadioButton.Location = new System.Drawing.Point(25, 73);
            this.signalEvent_RadioButton.Name = "signalEvent_RadioButton";
            this.signalEvent_RadioButton.Size = new System.Drawing.Size(176, 19);
            this.signalEvent_RadioButton.TabIndex = 2;
            this.signalEvent_RadioButton.TabStop = true;
            this.signalEvent_RadioButton.Text = "Signal synchronization event";
            this.signalEvent_RadioButton.UseVisualStyleBackColor = true;
            // 
            // waitForEvent_RadioButton
            // 
            this.waitForEvent_RadioButton.AutoSize = true;
            this.waitForEvent_RadioButton.Location = new System.Drawing.Point(25, 108);
            this.waitForEvent_RadioButton.Name = "waitForEvent_RadioButton";
            this.waitForEvent_RadioButton.Size = new System.Drawing.Size(186, 19);
            this.waitForEvent_RadioButton.TabIndex = 2;
            this.waitForEvent_RadioButton.TabStop = true;
            this.waitForEvent_RadioButton.Text = "Wait for synchronization event";
            this.waitForEvent_RadioButton.UseVisualStyleBackColor = true;
            // 
            // SyncPointConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.waitForEvent_RadioButton);
            this.Controls.Add(this.signalEvent_RadioButton);
            this.Controls.Add(this.eventName_Label);
            this.Controls.Add(this.eventName_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SyncPointConfigurationControl";
            this.Size = new System.Drawing.Size(353, 251);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TextBox eventName_TextBox;
        private System.Windows.Forms.Label eventName_Label;
        private System.Windows.Forms.RadioButton signalEvent_RadioButton;
        private System.Windows.Forms.RadioButton waitForEvent_RadioButton;
    }
}
