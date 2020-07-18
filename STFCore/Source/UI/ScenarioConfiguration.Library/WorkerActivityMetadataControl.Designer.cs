namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class WorkerActivityMetadataControl
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
            this.name_Label = new System.Windows.Forms.Label();
            this.metadataEditor_Panel = new System.Windows.Forms.Panel();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.SuspendLayout();
            // 
            // name_Label
            // 
            this.name_Label.Location = new System.Drawing.Point(3, 16);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(102, 20);
            this.name_Label.TabIndex = 0;
            this.name_Label.Text = "Activity Name";
            this.name_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metadataEditor_Panel
            // 
            this.metadataEditor_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metadataEditor_Panel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metadataEditor_Panel.Location = new System.Drawing.Point(3, 52);
            this.metadataEditor_Panel.Name = "metadataEditor_Panel";
            this.metadataEditor_Panel.Size = new System.Drawing.Size(694, 545);
            this.metadataEditor_Panel.TabIndex = 4;
            // 
            // name_TextBox
            // 
            this.name_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name_TextBox.Location = new System.Drawing.Point(111, 13);
            this.name_TextBox.MaxLength = 255;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(563, 23);
            this.name_TextBox.TabIndex = 0;
            this.name_TextBox.TextChanged += new System.EventHandler(this.name_TextBox_TextChanged);
            // 
            // WorkerActivityMetadataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.metadataEditor_Panel);
            this.Controls.Add(this.name_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WorkerActivityMetadataControl";
            this.Size = new System.Drawing.Size(700, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.Panel metadataEditor_Panel;
        private System.Windows.Forms.TextBox name_TextBox;
        private ScalableTest.Framework.UI.FieldValidator fieldValidator;
    }
}
