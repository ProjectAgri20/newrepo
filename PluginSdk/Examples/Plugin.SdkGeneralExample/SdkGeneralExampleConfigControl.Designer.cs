/*
* © Copyright 2016 HP Development Company, L.P.
*/
namespace Plugin.SdkGeneralExample
{
    partial class SdkGeneralExampleConfigControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.activityLabel_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabDocuments = new System.Windows.Forms.TabPage();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.tabAssets = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDocuments.SuspendLayout();
            this.tabAssets.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Activity Label:";
            // 
            // activityLabel_TextBox
            // 
            this.activityLabel_TextBox.Location = new System.Drawing.Point(22, 42);
            this.activityLabel_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.activityLabel_TextBox.Name = "activityLabel_TextBox";
            this.activityLabel_TextBox.Size = new System.Drawing.Size(622, 21);
            this.activityLabel_TextBox.TabIndex = 2;
            this.activityLabel_TextBox.Text = "Default";
            // 
            // tabDocuments
            // 
            this.tabDocuments.BackColor = System.Drawing.SystemColors.Control;
            this.tabDocuments.Controls.Add(this.documentSelectionControl);
            this.tabDocuments.Location = new System.Drawing.Point(4, 24);
            this.tabDocuments.Name = "tabDocuments";
            this.tabDocuments.Padding = new System.Windows.Forms.Padding(3);
            this.tabDocuments.Size = new System.Drawing.Size(802, 357);
            this.tabDocuments.TabIndex = 2;
            this.tabDocuments.Text = "Documents";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.documentSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(796, 351);
            this.documentSelectionControl.TabIndex = 6;
            // 
            // tabAssets
            // 
            this.tabAssets.Controls.Add(this.assetSelectionControl);
            this.tabAssets.Location = new System.Drawing.Point(4, 24);
            this.tabAssets.Name = "tabAssets";
            this.tabAssets.Padding = new System.Windows.Forms.Padding(3);
            this.tabAssets.Size = new System.Drawing.Size(802, 357);
            this.tabAssets.TabIndex = 1;
            this.tabAssets.Text = "Assets";
            this.tabAssets.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(796, 351);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabAssets);
            this.tabControl1.Controls.Add(this.tabDocuments);
            this.tabControl1.Location = new System.Drawing.Point(21, 74);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(810, 385);
            this.tabControl1.TabIndex = 8;
            // 
            // SdkGeneralExampleConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.activityLabel_TextBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SdkGeneralExampleConfigControl";
            this.Size = new System.Drawing.Size(846, 477);
            this.tabDocuments.ResumeLayout(false);
            this.tabAssets.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox activityLabel_TextBox;
        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabPage tabDocuments;
        private HP.ScalableTest.Framework.UI.DocumentSelectionControl documentSelectionControl;
        private System.Windows.Forms.TabPage tabAssets;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.TabControl tabControl1;
    }
}
