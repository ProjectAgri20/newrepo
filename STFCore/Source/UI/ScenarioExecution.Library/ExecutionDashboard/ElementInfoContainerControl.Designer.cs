namespace HP.ScalableTest.UI.SessionExecution
{
    partial class ElementInfoContainerControl
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
            Telerik.WinControls.ThemeSource themeSource1 = new Telerik.WinControls.ThemeSource();
            this.groupBox_ElementInfo = new System.Windows.Forms.GroupBox();
            this.radPanel_ElementInfoHolder = new Telerik.WinControls.UI.RadPanel();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.groupBox_ElementInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_ElementInfoHolder)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_ElementInfo
            // 
            this.groupBox_ElementInfo.Controls.Add(this.radPanel_ElementInfoHolder);
            this.groupBox_ElementInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_ElementInfo.Location = new System.Drawing.Point(0, 0);
            this.groupBox_ElementInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_ElementInfo.Name = "groupBox_ElementInfo";
            this.groupBox_ElementInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_ElementInfo.Size = new System.Drawing.Size(856, 617);
            this.groupBox_ElementInfo.TabIndex = 5;
            this.groupBox_ElementInfo.TabStop = false;
            this.groupBox_ElementInfo.Text = "Element Info";
            // 
            // radPanel_ElementInfoHolder
            // 
            this.radPanel_ElementInfoHolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanel_ElementInfoHolder.Location = new System.Drawing.Point(8, 26);
            this.radPanel_ElementInfoHolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radPanel_ElementInfoHolder.Name = "radPanel_ElementInfoHolder";
            this.radPanel_ElementInfoHolder.Size = new System.Drawing.Size(840, 584);
            this.radPanel_ElementInfoHolder.TabIndex = 0;
            this.radPanel_ElementInfoHolder.Text = "ElementInfoHolder";
            // 
            // radThemeManager1
            // 
            themeSource1.StorageType = Telerik.WinControls.ThemeStorageType.Resource;
            this.radThemeManager1.LoadedThemes.AddRange(new Telerik.WinControls.ThemeSource[] {
            themeSource1});
            // 
            // GenericMapElementControlLite
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox_ElementInfo);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GenericMapElementControlLite";
            this.Size = new System.Drawing.Size(856, 617);
            this.groupBox_ElementInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_ElementInfoHolder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.RadPanel radPanel_ElementInfoHolder;
        private System.Windows.Forms.GroupBox groupBox_ElementInfo;
    }
}
