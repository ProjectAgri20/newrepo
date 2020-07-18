namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportCompletionControl
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
            this.saveToRadTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.saveToRadTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // saveToRadTreeView
            // 
            this.saveToRadTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveToRadTreeView.Location = new System.Drawing.Point(18, 35);
            this.saveToRadTreeView.Name = "saveToRadTreeView";
            this.saveToRadTreeView.Size = new System.Drawing.Size(539, 247);
            this.saveToRadTreeView.SpacingBetweenNodes = -1;
            this.saveToRadTreeView.TabIndex = 0;
            this.saveToRadTreeView.Text = "radTreeView1";
            this.saveToRadTreeView.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.saveToRadTreeView_SelectedNodeChanged);
            this.saveToRadTreeView.NodeMouseDoubleClick += new Telerik.WinControls.UI.RadTreeView.TreeViewEventHandler(this.saveToRadTreeView_NodeMouseDoubleClick);
            this.saveToRadTreeView.VisibleChanged += new System.EventHandler(this.saveToRadTreeView_VisibleChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select location to import Scenario";
            // 
            // ImportCompletionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveToRadTreeView);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportCompletionControl";
            this.Size = new System.Drawing.Size(574, 299);
            ((System.ComponentModel.ISupportInitialize)(this.saveToRadTreeView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView saveToRadTreeView;
        private System.Windows.Forms.Label label2;

    }
}
