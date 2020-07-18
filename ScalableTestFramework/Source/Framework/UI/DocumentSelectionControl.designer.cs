namespace HP.ScalableTest.Framework.UI
{
    partial class DocumentSelectionControl
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
            this.documentQuery_RadioButton = new System.Windows.Forms.RadioButton();
            this.documentBrowse_RadioButton = new System.Windows.Forms.RadioButton();
            this.documentSet_RadioButton = new System.Windows.Forms.RadioButton();
            this.selectionMode_Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.documentSetControl = new HP.ScalableTest.Framework.UI.DocumentSetControl();
            this.documentQueryControl = new HP.ScalableTest.Framework.UI.DocumentQueryControl();
            this.documentBrowseControl = new HP.ScalableTest.Framework.UI.DocumentBrowseControl();
            this.selectionMode_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentQuery_RadioButton
            // 
            this.documentQuery_RadioButton.AutoSize = true;
            this.documentQuery_RadioButton.Location = new System.Drawing.Point(308, 3);
            this.documentQuery_RadioButton.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.documentQuery_RadioButton.Name = "documentQuery_RadioButton";
            this.documentQuery_RadioButton.Size = new System.Drawing.Size(139, 19);
            this.documentQuery_RadioButton.TabIndex = 2;
            this.documentQuery_RadioButton.Text = "Query for Documents";
            this.documentQuery_RadioButton.UseVisualStyleBackColor = true;
            this.documentQuery_RadioButton.CheckedChanged += new System.EventHandler(this.selectionRadioButton_CheckedChanged);
            // 
            // documentBrowse_RadioButton
            // 
            this.documentBrowse_RadioButton.AutoSize = true;
            this.documentBrowse_RadioButton.Location = new System.Drawing.Point(3, 3);
            this.documentBrowse_RadioButton.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.documentBrowse_RadioButton.Name = "documentBrowse_RadioButton";
            this.documentBrowse_RadioButton.Size = new System.Drawing.Size(145, 19);
            this.documentBrowse_RadioButton.TabIndex = 0;
            this.documentBrowse_RadioButton.Text = "Browse for Documents";
            this.documentBrowse_RadioButton.UseVisualStyleBackColor = true;
            this.documentBrowse_RadioButton.CheckedChanged += new System.EventHandler(this.selectionRadioButton_CheckedChanged);
            // 
            // documentSet_RadioButton
            // 
            this.documentSet_RadioButton.AutoSize = true;
            this.documentSet_RadioButton.Location = new System.Drawing.Point(161, 3);
            this.documentSet_RadioButton.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.documentSet_RadioButton.Name = "documentSet_RadioButton";
            this.documentSet_RadioButton.Size = new System.Drawing.Size(134, 19);
            this.documentSet_RadioButton.TabIndex = 1;
            this.documentSet_RadioButton.Text = "Select Document Set";
            this.documentSet_RadioButton.UseVisualStyleBackColor = true;
            this.documentSet_RadioButton.CheckedChanged += new System.EventHandler(this.selectionRadioButton_CheckedChanged);
            // 
            // selectionMode_Panel
            // 
            this.selectionMode_Panel.Controls.Add(this.documentBrowse_RadioButton);
            this.selectionMode_Panel.Controls.Add(this.documentSet_RadioButton);
            this.selectionMode_Panel.Controls.Add(this.documentQuery_RadioButton);
            this.selectionMode_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.selectionMode_Panel.Location = new System.Drawing.Point(0, 0);
            this.selectionMode_Panel.Name = "selectionMode_Panel";
            this.selectionMode_Panel.Size = new System.Drawing.Size(658, 30);
            this.selectionMode_Panel.TabIndex = 0;
            // 
            // documentSetControl
            // 
            this.documentSetControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSetControl.Location = new System.Drawing.Point(285, 36);
            this.documentSetControl.Name = "documentSetControl";
            this.documentSetControl.ShowCopyLinkLabel = true;
            this.documentSetControl.Size = new System.Drawing.Size(370, 160);
            this.documentSetControl.TabIndex = 2;
            this.documentSetControl.CopyDocumentsSelected += new System.EventHandler(this.documentSetControl_CopyDocumentsSelected);
            // 
            // documentQueryControl
            // 
            this.documentQueryControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentQueryControl.Location = new System.Drawing.Point(3, 202);
            this.documentQueryControl.Name = "documentQueryControl";
            this.documentQueryControl.Size = new System.Drawing.Size(652, 204);
            this.documentQueryControl.TabIndex = 3;
            // 
            // documentBrowseControl
            // 
            this.documentBrowseControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentBrowseControl.Location = new System.Drawing.Point(3, 36);
            this.documentBrowseControl.Name = "documentBrowseControl";
            this.documentBrowseControl.Size = new System.Drawing.Size(276, 160);
            this.documentBrowseControl.TabIndex = 1;
            // 
            // DocumentSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.documentBrowseControl);
            this.Controls.Add(this.documentSetControl);
            this.Controls.Add(this.documentQueryControl);
            this.Controls.Add(this.selectionMode_Panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentSelectionControl";
            this.Size = new System.Drawing.Size(658, 420);
            this.selectionMode_Panel.ResumeLayout(false);
            this.selectionMode_Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton documentQuery_RadioButton;
        private System.Windows.Forms.RadioButton documentBrowse_RadioButton;
        private System.Windows.Forms.RadioButton documentSet_RadioButton;
        private System.Windows.Forms.FlowLayoutPanel selectionMode_Panel;
        private DocumentBrowseControl documentBrowseControl;
        private DocumentQueryControl documentQueryControl;
        private DocumentSetControl documentSetControl;
    }
}
