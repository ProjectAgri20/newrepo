namespace HP.ScalableTest.UI.SessionExecution
{
    partial class GenericMapElementControlLite
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
            this.elementInfoCompositeControl = new HP.ScalableTest.UI.SessionExecution.ElementInfoContainerControl();
            this.SuspendLayout();
            // 
            // elementInfoCompositeControl
            // 
            this.elementInfoCompositeControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementInfoCompositeControl.Location = new System.Drawing.Point(4, 4);
            this.elementInfoCompositeControl.Margin = new System.Windows.Forms.Padding(4);
            this.elementInfoCompositeControl.Name = "elementInfoCompositeControl";
            this.elementInfoCompositeControl.Size = new System.Drawing.Size(848, 609);
            this.elementInfoCompositeControl.TabIndex = 0;
            this.elementInfoCompositeControl.Text = "elementInfoCompositeControl1";
            // 
            // GenericMapElementControlLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.elementInfoCompositeControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GenericMapElementControlLite";
            this.Size = new System.Drawing.Size(856, 617);
            this.ResumeLayout(false);

        }

        #endregion

        private ElementInfoContainerControl elementInfoCompositeControl;

    }
}
