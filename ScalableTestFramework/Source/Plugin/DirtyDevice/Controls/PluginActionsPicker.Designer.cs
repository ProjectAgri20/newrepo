namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    partial class PluginActionsPicker
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
            this.ActionGroupBox = new System.Windows.Forms.GroupBox();
            this.NoneButton = new System.Windows.Forms.Button();
            this.AllButton = new System.Windows.Forms.Button();
            this.PluginActionsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.ActionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionGroupBox
            // 
            this.ActionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionGroupBox.Controls.Add(this.NoneButton);
            this.ActionGroupBox.Controls.Add(this.AllButton);
            this.ActionGroupBox.Controls.Add(this.PluginActionsCheckedListBox);
            this.ActionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ActionGroupBox.Name = "ActionGroupBox";
            this.ActionGroupBox.Size = new System.Drawing.Size(251, 150);
            this.ActionGroupBox.TabIndex = 6;
            this.ActionGroupBox.TabStop = false;
            this.ActionGroupBox.Text = "Plugin Actions";
            // 
            // NoneButton
            // 
            this.NoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NoneButton.Location = new System.Drawing.Point(199, 50);
            this.NoneButton.Name = "NoneButton";
            this.NoneButton.Size = new System.Drawing.Size(44, 23);
            this.NoneButton.TabIndex = 16;
            this.NoneButton.Text = "None";
            this.NoneButton.UseVisualStyleBackColor = true;
            this.NoneButton.Click += new System.EventHandler(this.NoneButton_Click);
            // 
            // AllButton
            // 
            this.AllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AllButton.Location = new System.Drawing.Point(199, 20);
            this.AllButton.Name = "AllButton";
            this.AllButton.Size = new System.Drawing.Size(44, 23);
            this.AllButton.TabIndex = 15;
            this.AllButton.Text = "All";
            this.AllButton.UseVisualStyleBackColor = true;
            this.AllButton.Click += new System.EventHandler(this.AllButton_Click);
            // 
            // PluginActionsCheckedListBox
            // 
            this.PluginActionsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PluginActionsCheckedListBox.FormattingEnabled = true;
            this.PluginActionsCheckedListBox.Location = new System.Drawing.Point(10, 20);
            this.PluginActionsCheckedListBox.Name = "PluginActionsCheckedListBox";
            this.PluginActionsCheckedListBox.Size = new System.Drawing.Size(183, 124);
            this.PluginActionsCheckedListBox.TabIndex = 14;
            // 
            // PluginActionsPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ActionGroupBox);
            this.Name = "PluginActionsPicker";
            this.Size = new System.Drawing.Size(257, 156);
            this.ActionGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ActionGroupBox;
        private System.Windows.Forms.Button NoneButton;
        private System.Windows.Forms.Button AllButton;
        private System.Windows.Forms.CheckedListBox PluginActionsCheckedListBox;
        private Framework.UI.FieldValidator fieldValidator;
    }
}
