namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class WorkerActivityOrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerActivityOrderForm));
            this.activities_ListControl = new Telerik.WinControls.UI.RadListControl();
            this.moveUp_Button = new System.Windows.Forms.Button();
            this.moveDown_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.activities_ListControl)).BeginInit();
            this.SuspendLayout();
            // 
            // activities_ListControl
            // 
            this.activities_ListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activities_ListControl.Location = new System.Drawing.Point(14, 14);
            this.activities_ListControl.Name = "activities_ListControl";
            this.activities_ListControl.Size = new System.Drawing.Size(267, 354);
            this.activities_ListControl.TabIndex = 0;
            this.activities_ListControl.Text = "radListControl1";
            // 
            // moveUp_Button
            // 
            this.moveUp_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveUp_Button.Image = ((System.Drawing.Image)(resources.GetObject("moveUp_Button.Image")));
            this.moveUp_Button.Location = new System.Drawing.Point(308, 39);
            this.moveUp_Button.Name = "moveUp_Button";
            this.moveUp_Button.Size = new System.Drawing.Size(29, 29);
            this.moveUp_Button.TabIndex = 1;
            this.moveUp_Button.UseVisualStyleBackColor = true;
            this.moveUp_Button.Click += new System.EventHandler(this.moveUp_Button_Click);
            // 
            // moveDown_Button
            // 
            this.moveDown_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveDown_Button.Image = ((System.Drawing.Image)(resources.GetObject("moveDown_Button.Image")));
            this.moveDown_Button.Location = new System.Drawing.Point(308, 100);
            this.moveDown_Button.Name = "moveDown_Button";
            this.moveDown_Button.Size = new System.Drawing.Size(29, 29);
            this.moveDown_Button.TabIndex = 1;
            this.moveDown_Button.UseVisualStyleBackColor = true;
            this.moveDown_Button.Click += new System.EventHandler(this.moveDown_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(287, 347);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(70, 23);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(287, 318);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(70, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // WorkerActivityOrderForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(369, 382);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.moveDown_Button);
            this.Controls.Add(this.moveUp_Button);
            this.Controls.Add(this.activities_ListControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WorkerActivityOrderForm";
            this.Text = "Worker Activity Order";
            ((System.ComponentModel.ISupportInitialize)(this.activities_ListControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadListControl activities_ListControl;
        private System.Windows.Forms.Button moveUp_Button;
        private System.Windows.Forms.Button moveDown_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
    }
}