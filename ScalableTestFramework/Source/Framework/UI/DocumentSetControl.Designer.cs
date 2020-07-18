namespace HP.ScalableTest.Framework.UI
{
    partial class DocumentSetControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.documentSets_ListBox = new System.Windows.Forms.ListBox();
            this.setContents_ListBox = new System.Windows.Forms.ListBox();
            this.documentSets_Label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.copy_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.setContents_Label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.documentSets_ListBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.setContents_ListBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.documentSets_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 154);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // documentSets_ListBox
            // 
            this.documentSets_ListBox.DisplayMember = "Name";
            this.documentSets_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSets_ListBox.FormattingEnabled = true;
            this.documentSets_ListBox.IntegralHeight = false;
            this.documentSets_ListBox.ItemHeight = 15;
            this.documentSets_ListBox.Location = new System.Drawing.Point(3, 18);
            this.documentSets_ListBox.Name = "documentSets_ListBox";
            this.documentSets_ListBox.Size = new System.Drawing.Size(268, 133);
            this.documentSets_ListBox.TabIndex = 1;
            this.documentSets_ListBox.SelectedIndexChanged += new System.EventHandler(this.documentSets_ListBox_SelectedIndexChanged);
            // 
            // setContents_ListBox
            // 
            this.setContents_ListBox.DisplayMember = "FileName";
            this.setContents_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setContents_ListBox.FormattingEnabled = true;
            this.setContents_ListBox.IntegralHeight = false;
            this.setContents_ListBox.ItemHeight = 15;
            this.setContents_ListBox.Location = new System.Drawing.Point(277, 18);
            this.setContents_ListBox.Name = "setContents_ListBox";
            this.setContents_ListBox.Size = new System.Drawing.Size(268, 133);
            this.setContents_ListBox.TabIndex = 2;
            // 
            // documentSets_Label
            // 
            this.documentSets_Label.AutoSize = true;
            this.documentSets_Label.Location = new System.Drawing.Point(3, 0);
            this.documentSets_Label.Name = "documentSets_Label";
            this.documentSets_Label.Size = new System.Drawing.Size(87, 15);
            this.documentSets_Label.TabIndex = 0;
            this.documentSets_Label.Text = "Document Sets";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.copy_LinkLabel);
            this.panel1.Controls.Add(this.setContents_Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(274, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(274, 15);
            this.panel1.TabIndex = 5;
            // 
            // copy_LinkLabel
            // 
            this.copy_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copy_LinkLabel.AutoSize = true;
            this.copy_LinkLabel.Location = new System.Drawing.Point(172, 0);
            this.copy_LinkLabel.Name = "copy_LinkLabel";
            this.copy_LinkLabel.Size = new System.Drawing.Size(99, 15);
            this.copy_LinkLabel.TabIndex = 1;
            this.copy_LinkLabel.TabStop = true;
            this.copy_LinkLabel.Text = "Copy Documents";
            this.copy_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.copy_LinkLabel_LinkClicked);
            // 
            // setContents_Label
            // 
            this.setContents_Label.AutoSize = true;
            this.setContents_Label.Location = new System.Drawing.Point(3, 0);
            this.setContents_Label.Name = "setContents_Label";
            this.setContents_Label.Size = new System.Drawing.Size(74, 15);
            this.setContents_Label.TabIndex = 0;
            this.setContents_Label.Text = "Set Contents";
            // 
            // DocumentSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentSetControl";
            this.Size = new System.Drawing.Size(555, 160);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox documentSets_ListBox;
        private System.Windows.Forms.ListBox setContents_ListBox;
        private System.Windows.Forms.Label documentSets_Label;
        private System.Windows.Forms.Label setContents_Label;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel copy_LinkLabel;
    }
}
