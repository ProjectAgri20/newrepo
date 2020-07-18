namespace HP.ScalableTest.UI.Framework
{
    partial class AssetUsageForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ok_Button = new System.Windows.Forms.Button();
            this.asset_TreeView = new System.Windows.Forms.TreeView();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.creator_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lastRun_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.created_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(537, 444);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 0;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // asset_TreeView
            // 
            this.asset_TreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.asset_TreeView.Location = new System.Drawing.Point(12, 28);
            this.asset_TreeView.Name = "asset_TreeView";
            this.asset_TreeView.Size = new System.Drawing.Size(436, 406);
            this.asset_TreeView.TabIndex = 2;
            this.asset_TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.asset_TreeView_AfterSelect);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.treeViewImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(454, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Creator";
            // 
            // creator_TextBox
            // 
            this.creator_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.creator_TextBox.BackColor = System.Drawing.Color.White;
            this.creator_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.creator_TextBox.Location = new System.Drawing.Point(457, 28);
            this.creator_TextBox.Name = "creator_TextBox";
            this.creator_TextBox.ReadOnly = true;
            this.creator_TextBox.Size = new System.Drawing.Size(159, 20);
            this.creator_TextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Created Date";
            // 
            // lastRun_TextBox
            // 
            this.lastRun_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lastRun_TextBox.BackColor = System.Drawing.Color.White;
            this.lastRun_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lastRun_TextBox.Location = new System.Drawing.Point(457, 106);
            this.lastRun_TextBox.Name = "lastRun_TextBox";
            this.lastRun_TextBox.ReadOnly = true;
            this.lastRun_TextBox.Size = new System.Drawing.Size(159, 20);
            this.lastRun_TextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(454, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Last Run";
            // 
            // created_TextBox
            // 
            this.created_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.created_TextBox.BackColor = System.Drawing.Color.White;
            this.created_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.created_TextBox.Location = new System.Drawing.Point(457, 67);
            this.created_TextBox.Name = "created_TextBox";
            this.created_TextBox.ReadOnly = true;
            this.created_TextBox.Size = new System.Drawing.Size(159, 20);
            this.created_TextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(355, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "This dialog shows all locations where an Asset is being referenced.";
            // 
            // AssetUsageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 479);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.created_TextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lastRun_TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.creator_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.asset_TreeView);
            this.Controls.Add(this.ok_Button);
            this.Name = "AssetUsageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Usage Summary";
            this.Load += new System.EventHandler(this.AssetUsageForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.TreeView asset_TreeView;
        private System.Windows.Forms.ImageList treeViewImageList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox creator_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lastRun_TextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox created_TextBox;
        private System.Windows.Forms.Label label4;
    }
}