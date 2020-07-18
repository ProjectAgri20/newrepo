namespace HP.ScalableTest.PluginSupport.PullPrint
{
    partial class PrintingTabConfigControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintingTabConfigControl));
            this.printQueues_GridView = new Telerik.WinControls.UI.RadGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownDelayAfterPrint = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.localQueues_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remoteQueues_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.document_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.shuffle_CheckBox = new System.Windows.Forms.CheckBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBoxDocumentSelection = new System.Windows.Forms.GroupBox();
            this.groupBoxPrintQueue = new System.Windows.Forms.GroupBox();
            this.printServerNotificationcheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView.MasterTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayAfterPrint)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.document_GroupBox.SuspendLayout();
            this.groupBoxDocumentSelection.SuspendLayout();
            this.groupBoxPrintQueue.SuspendLayout();
            this.SuspendLayout();
            // 
            // printQueues_GridView
            // 
            this.printQueues_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printQueues_GridView.BackColor = System.Drawing.SystemColors.Control;
            this.printQueues_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.printQueues_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.printQueues_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.printQueues_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.printQueues_GridView.Location = new System.Drawing.Point(3, 47);
            // 
            // 
            // 
            this.printQueues_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "QueueName";
            gridViewTextBoxColumn1.HeaderText = "Queue Name";
            gridViewTextBoxColumn1.Name = "queueName_GridViewColumn";
            gridViewTextBoxColumn1.Width = 326;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "PrintServer";
            gridViewTextBoxColumn2.HeaderText = "Print Server";
            gridViewTextBoxColumn2.MaxWidth = 150;
            gridViewTextBoxColumn2.Name = "printServer_GridViewColumn";
            gridViewTextBoxColumn2.Width = 150;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "QueueType";
            gridViewTextBoxColumn3.HeaderText = "Type";
            gridViewTextBoxColumn3.MaxWidth = 80;
            gridViewTextBoxColumn3.MinWidth = 80;
            gridViewTextBoxColumn3.Name = "queueType_GridViewColumn";
            gridViewTextBoxColumn3.Width = 80;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Device";
            gridViewTextBoxColumn4.HeaderText = "Device";
            gridViewTextBoxColumn4.MaxWidth = 80;
            gridViewTextBoxColumn4.MinWidth = 80;
            gridViewTextBoxColumn4.Name = "device_GridViewColumn";
            gridViewTextBoxColumn4.Width = 80;
            this.printQueues_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.printQueues_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.printQueues_GridView.Name = "printQueues_GridView";
            this.printQueues_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.printQueues_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 47, 240, 150);
            this.printQueues_GridView.ShowGroupPanel = false;
            this.printQueues_GridView.Size = new System.Drawing.Size(654, 92);
            this.printQueues_GridView.TabIndex = 52;
            this.printQueues_GridView.Text = "radGridView1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.printServerNotificationcheckBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numericUpDownDelayAfterPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 139);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 43);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 15);
            this.label1.TabIndex = 60;
            this.label1.Text = "Delay (Seconds) after print:";
            // 
            // numericUpDownDelayAfterPrint
            // 
            this.numericUpDownDelayAfterPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownDelayAfterPrint.Location = new System.Drawing.Point(162, 9);
            this.numericUpDownDelayAfterPrint.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownDelayAfterPrint.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDelayAfterPrint.Name = "numericUpDownDelayAfterPrint";
            this.numericUpDownDelayAfterPrint.Size = new System.Drawing.Size(46, 23);
            this.numericUpDownDelayAfterPrint.TabIndex = 59;
            this.numericUpDownDelayAfterPrint.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localQueues_ToolStripButton,
            this.remoteQueues_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 19);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(654, 25);
            this.toolStrip1.TabIndex = 53;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // localQueues_ToolStripButton
            // 
            this.localQueues_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("localQueues_ToolStripButton.Image")));
            this.localQueues_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.localQueues_ToolStripButton.Name = "localQueues_ToolStripButton";
            this.localQueues_ToolStripButton.Size = new System.Drawing.Size(98, 22);
            this.localQueues_ToolStripButton.Text = "Local Queues";
            this.localQueues_ToolStripButton.Click += new System.EventHandler(this.localQueues_ToolStripButton_Click);
            // 
            // remoteQueues_ToolStripButton
            // 
            this.remoteQueues_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remoteQueues_ToolStripButton.Image")));
            this.remoteQueues_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remoteQueues_ToolStripButton.Name = "remoteQueues_ToolStripButton";
            this.remoteQueues_ToolStripButton.Size = new System.Drawing.Size(111, 22);
            this.remoteQueues_ToolStripButton.Text = "Remote Queues";
            this.remoteQueues_ToolStripButton.Click += new System.EventHandler(this.remoteQueues_ToolStripButton_Click);
            // 
            // document_GroupBox
            // 
            this.document_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.document_GroupBox.Controls.Add(this.documentSelectionControl);
            this.document_GroupBox.Location = new System.Drawing.Point(6, 47);
            this.document_GroupBox.Name = "document_GroupBox";
            this.document_GroupBox.Size = new System.Drawing.Size(649, 249);
            this.document_GroupBox.TabIndex = 53;
            this.document_GroupBox.TabStop = false;
            this.document_GroupBox.Text = "Document Selection";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(3, 19);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(643, 227);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // shuffle_CheckBox
            // 
            this.shuffle_CheckBox.Location = new System.Drawing.Point(6, 22);
            this.shuffle_CheckBox.Name = "shuffle_CheckBox";
            this.shuffle_CheckBox.Size = new System.Drawing.Size(200, 19);
            this.shuffle_CheckBox.TabIndex = 51;
            this.shuffle_CheckBox.Text = "Shuffle document printing order";
            // 
            // groupBoxDocumentSelection
            // 
            this.groupBoxDocumentSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDocumentSelection.Controls.Add(this.document_GroupBox);
            this.groupBoxDocumentSelection.Controls.Add(this.shuffle_CheckBox);
            this.groupBoxDocumentSelection.Location = new System.Drawing.Point(0, 195);
            this.groupBoxDocumentSelection.Name = "groupBoxDocumentSelection";
            this.groupBoxDocumentSelection.Size = new System.Drawing.Size(661, 302);
            this.groupBoxDocumentSelection.TabIndex = 54;
            this.groupBoxDocumentSelection.TabStop = false;
            this.groupBoxDocumentSelection.Text = "Document Selection";
            // 
            // groupBoxPrintQueue
            // 
            this.groupBoxPrintQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPrintQueue.Controls.Add(this.toolStrip1);
            this.groupBoxPrintQueue.Controls.Add(this.printQueues_GridView);
            this.groupBoxPrintQueue.Controls.Add(this.panel1);
            this.groupBoxPrintQueue.Location = new System.Drawing.Point(4, 4);
            this.groupBoxPrintQueue.Name = "groupBoxPrintQueue";
            this.groupBoxPrintQueue.Size = new System.Drawing.Size(660, 185);
            this.groupBoxPrintQueue.TabIndex = 55;
            this.groupBoxPrintQueue.TabStop = false;
            this.groupBoxPrintQueue.Text = "Print Queue Selection";
            // 
            // printServerNotificationcheckBox
            // 
            this.printServerNotificationcheckBox.AutoSize = true;
            this.printServerNotificationcheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printServerNotificationcheckBox.Location = new System.Drawing.Point(240, 11);
            this.printServerNotificationcheckBox.Name = "printServerNotificationcheckBox";
            this.printServerNotificationcheckBox.Size = new System.Drawing.Size(174, 19);
            this.printServerNotificationcheckBox.TabIndex = 61;
            this.printServerNotificationcheckBox.Text = "Use Print Server Notification";
            this.printServerNotificationcheckBox.UseVisualStyleBackColor = true;
            // 
            // PrintingTabConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPrintQueue);
            this.Controls.Add(this.groupBoxDocumentSelection);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintingTabConfigControl";
            this.Size = new System.Drawing.Size(667, 500);
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayAfterPrint)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.document_GroupBox.ResumeLayout(false);
            this.groupBoxDocumentSelection.ResumeLayout(false);
            this.groupBoxPrintQueue.ResumeLayout(false);
            this.groupBoxPrintQueue.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox shuffle_CheckBox;
        private System.Windows.Forms.GroupBox document_GroupBox;
        private ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private ScalableTest.Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Telerik.WinControls.UI.RadGridView printQueues_GridView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton localQueues_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remoteQueues_ToolStripButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxDocumentSelection;
        private System.Windows.Forms.GroupBox groupBoxPrintQueue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownDelayAfterPrint;
        private System.Windows.Forms.CheckBox printServerNotificationcheckBox;
    }
}
