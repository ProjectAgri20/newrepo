namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportGroupControl
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
            this.editorGroups_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editorGroups_CheckedListBox
            // 
            this.editorGroups_CheckedListBox.BackColor = System.Drawing.Color.White;
            this.editorGroups_CheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorGroups_CheckedListBox.CheckOnClick = true;
            this.editorGroups_CheckedListBox.FormattingEnabled = true;
            this.editorGroups_CheckedListBox.Location = new System.Drawing.Point(13, 17);
            this.editorGroups_CheckedListBox.Margin = new System.Windows.Forms.Padding(10);
            this.editorGroups_CheckedListBox.Name = "editorGroups_CheckedListBox";
            this.editorGroups_CheckedListBox.Size = new System.Drawing.Size(225, 200);
            this.editorGroups_CheckedListBox.Sorted = true;
            this.editorGroups_CheckedListBox.TabIndex = 33;
            this.editorGroups_CheckedListBox.ThreeDCheckBoxes = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(244, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(312, 119);
            this.label1.TabIndex = 34;
            this.label1.Text = "Test Scenario Groups are security groups that restrict edit access to test scenar" +
    "ios.  You can select what groups will have access to this imported scenario from" +
    " the list to the left.";
            // 
            // ImportGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editorGroups_CheckedListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportGroupControl";
            this.Size = new System.Drawing.Size(607, 254);
            this.Load += new System.EventHandler(this.ImportGroupControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox editorGroups_CheckedListBox;
        private System.Windows.Forms.Label label1;
    }
}
