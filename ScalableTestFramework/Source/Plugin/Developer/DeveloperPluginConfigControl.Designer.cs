namespace HP.ScalableTest.Plugin.Developer
{
    partial class DeveloperPluginConfigControl
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
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.resultTabPage = new System.Windows.Forms.TabPage();
            this.resultComboBox = new System.Windows.Forms.ComboBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.assetsTabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.documentsTabPage = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.printQueuesTabPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.remotePrintQueueSelectionControl = new HP.ScalableTest.Framework.UI.RemotePrintQueueSelectionControl();
            this.mainTabControl.SuspendLayout();
            this.resultTabPage.SuspendLayout();
            this.assetsTabPage.SuspendLayout();
            this.documentsTabPage.SuspendLayout();
            this.printQueuesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.resultTabPage);
            this.mainTabControl.Controls.Add(this.assetsTabPage);
            this.mainTabControl.Controls.Add(this.documentsTabPage);
            this.mainTabControl.Controls.Add(this.printQueuesTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(494, 328);
            this.mainTabControl.TabIndex = 0;
            // 
            // resultTabPage
            // 
            this.resultTabPage.Controls.Add(this.resultComboBox);
            this.resultTabPage.Controls.Add(this.resultLabel);
            this.resultTabPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultTabPage.Location = new System.Drawing.Point(4, 24);
            this.resultTabPage.Name = "resultTabPage";
            this.resultTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.resultTabPage.Size = new System.Drawing.Size(486, 300);
            this.resultTabPage.TabIndex = 0;
            this.resultTabPage.Text = "Plugin Result";
            this.resultTabPage.UseVisualStyleBackColor = true;
            // 
            // resultComboBox
            // 
            this.resultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resultComboBox.FormattingEnabled = true;
            this.resultComboBox.Location = new System.Drawing.Point(91, 10);
            this.resultComboBox.Name = "resultComboBox";
            this.resultComboBox.Size = new System.Drawing.Size(121, 23);
            this.resultComboBox.TabIndex = 1;
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(6, 13);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(79, 15);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Plugin Result:";
            // 
            // assetsTabPage
            // 
            this.assetsTabPage.Controls.Add(this.label1);
            this.assetsTabPage.Controls.Add(this.assetSelectionControl);
            this.assetsTabPage.Location = new System.Drawing.Point(4, 24);
            this.assetsTabPage.Name = "assetsTabPage";
            this.assetsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.assetsTabPage.Size = new System.Drawing.Size(486, 300);
            this.assetsTabPage.TabIndex = 1;
            this.assetsTabPage.Text = "Assets";
            this.assetsTabPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected assets will be reserved for the session and logged during execution.\r\nAs" +
    "sets will not be accessed at runtime or used for any automation.";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 55);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(474, 239);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // documentsTabPage
            // 
            this.documentsTabPage.Controls.Add(this.label2);
            this.documentsTabPage.Controls.Add(this.documentSelectionControl);
            this.documentsTabPage.Location = new System.Drawing.Point(4, 24);
            this.documentsTabPage.Name = "documentsTabPage";
            this.documentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.documentsTabPage.Size = new System.Drawing.Size(486, 300);
            this.documentsTabPage.TabIndex = 2;
            this.documentsTabPage.Text = "Documents";
            this.documentsTabPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(462, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Specified document selection method will be run and documents logged at execution" +
    ".\r\nDocuments will not be downloaded or otherwise accessed.";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(6, 55);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(474, 239);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // printQueuesTabPage
            // 
            this.printQueuesTabPage.Controls.Add(this.remotePrintQueueSelectionControl);
            this.printQueuesTabPage.Controls.Add(this.label3);
            this.printQueuesTabPage.Location = new System.Drawing.Point(4, 24);
            this.printQueuesTabPage.Name = "printQueuesTabPage";
            this.printQueuesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.printQueuesTabPage.Size = new System.Drawing.Size(486, 300);
            this.printQueuesTabPage.TabIndex = 3;
            this.printQueuesTabPage.Text = "Print Queues";
            this.printQueuesTabPage.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(413, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "Selected queues will be reserved for the session and logged during execution.\r\nQu" +
    "eues will not be accessed at runtime or used for any automation.";
            // 
            // remotePrintQueueSelectionControl
            // 
            this.remotePrintQueueSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remotePrintQueueSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remotePrintQueueSelectionControl.Location = new System.Drawing.Point(6, 55);
            this.remotePrintQueueSelectionControl.Name = "remotePrintQueueSelectionControl";
            this.remotePrintQueueSelectionControl.Size = new System.Drawing.Size(474, 239);
            this.remotePrintQueueSelectionControl.TabIndex = 3;
            // 
            // DeveloperPluginConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Name = "DeveloperPluginConfigControl";
            this.Size = new System.Drawing.Size(494, 328);
            this.mainTabControl.ResumeLayout(false);
            this.resultTabPage.ResumeLayout(false);
            this.resultTabPage.PerformLayout();
            this.assetsTabPage.ResumeLayout(false);
            this.assetsTabPage.PerformLayout();
            this.documentsTabPage.ResumeLayout(false);
            this.documentsTabPage.PerformLayout();
            this.printQueuesTabPage.ResumeLayout(false);
            this.printQueuesTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage resultTabPage;
        private System.Windows.Forms.ComboBox resultComboBox;
        private System.Windows.Forms.Label resultLabel;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabPage assetsTabPage;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage documentsTabPage;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage printQueuesTabPage;
        private System.Windows.Forms.Label label3;
        private Framework.UI.RemotePrintQueueSelectionControl remotePrintQueueSelectionControl;
    }
}
