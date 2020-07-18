namespace HP.ScalableTest.Plugin.GFriendExecution
{
    partial class GFriendExecutionConfigurationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GFriendExecutionConfigurationControl));
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.scripts_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.scripts_DataGridView = new System.Windows.Forms.DataGridView();
            this.device_Label = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.scripts_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scripts_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // scripts_ToolStrip
            // 
            this.scripts_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.scripts_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_ToolStripButton,
            this.edit_ToolStripButton,
            this.remove_ToolStripButton});
            this.scripts_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.scripts_ToolStrip.Name = "scripts_ToolStrip";
            this.scripts_ToolStrip.Size = new System.Drawing.Size(763, 25);
            this.scripts_ToolStrip.TabIndex = 16;
            this.scripts_ToolStrip.Text = "toolStrip1";
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(84, 22);
            this.add_ToolStripButton.Text = "Add Script";
            this.add_ToolStripButton.ToolTipText = "Add Script";
            this.add_ToolStripButton.Click += new System.EventHandler(this.Add_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(82, 22);
            this.edit_ToolStripButton.Text = "Edit Script";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.Edit_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(115, 22);
            this.remove_ToolStripButton.Text = "Remove All Files";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.Remove_ToolStripButton_Click);
            // 
            // scripts_DataGridView
            // 
            this.scripts_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scripts_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scripts_DataGridView.EnableHeadersVisualStyles = false;
            this.scripts_DataGridView.Location = new System.Drawing.Point(0, 24);
            this.scripts_DataGridView.Name = "scripts_DataGridView";
            this.scripts_DataGridView.RowHeadersVisible = false;
            this.scripts_DataGridView.Size = new System.Drawing.Size(761, 214);
            this.scripts_DataGridView.TabIndex = 17;
            this.scripts_DataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Scripts_DataGridView_MouseDoubleClick);
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Location = new System.Drawing.Point(3, 253);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(93, 15);
            this.device_Label.TabIndex = 18;
            this.device_Label.Text = "Device Selection";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(0, 271);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(761, 111);
            this.assetSelectionControl.TabIndex = 19;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(0, 398);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 20;
            // 
            // GFriendExecutionConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.device_Label);
            this.Controls.Add(this.scripts_DataGridView);
            this.Controls.Add(this.scripts_ToolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GFriendExecutionConfigurationControl";
            this.Size = new System.Drawing.Size(763, 453);
            this.scripts_ToolStrip.ResumeLayout(false);
            this.scripts_ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scripts_DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.ToolStrip scripts_ToolStrip;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private System.Windows.Forms.DataGridView scripts_DataGridView;
        private System.Windows.Forms.Label device_Label;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
