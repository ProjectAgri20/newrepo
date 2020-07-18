namespace HP.ScalableTest.UI.SessionExecution
{
    partial class GenericMapElementControl
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
            this.radSplitContainer_Main = new Telerik.WinControls.UI.RadSplitContainer();
            this.splitPanel1 = new Telerik.WinControls.UI.SplitPanel();
            this.elementInfoCompositeControl = new HP.ScalableTest.UI.SessionExecution.ElementInfoContainerControl();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer_Main)).BeginInit();
            this.radSplitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).BeginInit();
            this.splitPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radSplitContainer_Main
            // 
            this.radSplitContainer_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radSplitContainer_Main.BackColor = System.Drawing.SystemColors.Control;
            this.radSplitContainer_Main.Controls.Add(this.splitPanel1);
            this.radSplitContainer_Main.Location = new System.Drawing.Point(2, 2);
            this.radSplitContainer_Main.Name = "radSplitContainer_Main";
            this.radSplitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.radSplitContainer_Main.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.radSplitContainer_Main.Size = new System.Drawing.Size(636, 497);
            this.radSplitContainer_Main.TabIndex = 5;
            this.radSplitContainer_Main.TabStop = false;
            this.radSplitContainer_Main.Text = "radSplitContainer1";
            ((Telerik.WinControls.UI.SplitPanelElement)(this.radSplitContainer_Main.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radSplitContainer_Main.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.SystemColors.Control;
            // 
            // splitPanel1
            // 
            this.splitPanel1.Controls.Add(this.elementInfoCompositeControl);
            this.splitPanel1.Location = new System.Drawing.Point(0, 0);
            this.splitPanel1.Name = "splitPanel1";
            // 
            // 
            // 
            this.splitPanel1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.splitPanel1.Size = new System.Drawing.Size(636, 497);
            this.splitPanel1.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0F, 0.06795132F);
            this.splitPanel1.SizeInfo.SplitterCorrection = new System.Drawing.Size(0, 91);
            this.splitPanel1.TabIndex = 0;
            this.splitPanel1.TabStop = false;
            this.splitPanel1.Text = "splitPanel1";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.splitPanel1.GetChildAt(0).GetChildAt(1))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.splitPanel1.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.splitPanel1.GetChildAt(0).GetChildAt(1))).Enabled = true;
            // 
            // elementInfoCompositeControl
            // 
            this.elementInfoCompositeControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementInfoCompositeControl.Location = new System.Drawing.Point(3, 3);
            this.elementInfoCompositeControl.Name = "elementInfoCompositeControl";
            this.elementInfoCompositeControl.Size = new System.Drawing.Size(630, 491);
            this.elementInfoCompositeControl.TabIndex = 0;
            this.elementInfoCompositeControl.Text = "elementInfoCompositeControl1";
            // 
            // radThemeManager1
            // 
            themeSource1.StorageType = Telerik.WinControls.ThemeStorageType.Resource;
            this.radThemeManager1.LoadedThemes.AddRange(new Telerik.WinControls.ThemeSource[] {
            themeSource1});
            // 
            // GenericMapElementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.radSplitContainer_Main);
            this.Name = "GenericMapElementControl";
            this.Size = new System.Drawing.Size(642, 501);
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer_Main)).EndInit();
            this.radSplitContainer_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).EndInit();
            this.splitPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadSplitContainer radSplitContainer_Main;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.UI.SplitPanel splitPanel1;
        private ElementInfoContainerControl elementInfoCompositeControl;
    }
}
