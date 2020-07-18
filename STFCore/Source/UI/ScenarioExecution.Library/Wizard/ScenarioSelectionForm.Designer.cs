namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class ScenarioSelectionForm
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
            this.select_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.scenarioTreeView = new Telerik.WinControls.UI.RadTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // select_Button
            // 
            this.select_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.select_Button.Location = new System.Drawing.Point(229, 386);
            this.select_Button.Name = "select_Button";
            this.select_Button.Size = new System.Drawing.Size(75, 23);
            this.select_Button.TabIndex = 1;
            this.select_Button.Text = "Select";
            this.select_Button.UseVisualStyleBackColor = true;
            this.select_Button.Click += new System.EventHandler(this.select_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(310, 386);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 1;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // scenarioTreeView
            // 
            this.scenarioTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioTreeView.Location = new System.Drawing.Point(14, 14);
            this.scenarioTreeView.Name = "scenarioTreeView";
            this.scenarioTreeView.Size = new System.Drawing.Size(371, 366);
            this.scenarioTreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.scenarioTreeView.SpacingBetweenNodes = -1;
            this.scenarioTreeView.TabIndex = 2;
            this.scenarioTreeView.Text = "radTreeView1";
            this.scenarioTreeView.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.scenarioTreeView_SelectedNodeChanged);
            this.scenarioTreeView.NodeMouseDoubleClick += new Telerik.WinControls.UI.RadTreeView.TreeViewEventHandler(this.scenarioTreeView_NodeMouseDoubleClick);
            // 
            // ScenarioSelectionForm
            // 
            this.AcceptButton = this.select_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(399, 421);
            this.Controls.Add(this.scenarioTreeView);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.select_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScenarioSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "Scenario Selection";
            this.Load += new System.EventHandler(this.ScenarioSelectionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scenarioTreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button select_Button;
        private System.Windows.Forms.Button cancel_Button;
        private Telerik.WinControls.UI.RadTreeView scenarioTreeView;
    }
}