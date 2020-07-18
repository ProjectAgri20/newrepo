namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class HpacServerConfigurationConfigControl
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
            this.hpacServer_Label = new System.Windows.Forms.Label();
            this.hpacTile_label = new System.Windows.Forms.Label();
            this.hpacTileCombobox = new System.Windows.Forms.ComboBox();
            this.hpac_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.dynamic_groupBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // hpacServer_Label
            // 
            this.hpacServer_Label.AutoSize = true;
            this.hpacServer_Label.Location = new System.Drawing.Point(11, 7);
            this.hpacServer_Label.Name = "hpacServer_Label";
            this.hpacServer_Label.Size = new System.Drawing.Size(70, 13);
            this.hpacServer_Label.TabIndex = 52;
            this.hpacServer_Label.Text = "HPAC Server";
            this.hpacServer_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hpacTile_label
            // 
            this.hpacTile_label.AutoSize = true;
            this.hpacTile_label.Location = new System.Drawing.Point(11, 43);
            this.hpacTile_label.Name = "hpacTile_label";
            this.hpacTile_label.Size = new System.Drawing.Size(56, 13);
            this.hpacTile_label.TabIndex = 55;
            this.hpacTile_label.Text = "HPAC Tile";
            this.hpacTile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hpacTileCombobox
            // 
            this.hpacTileCombobox.FormattingEnabled = true;
            this.hpacTileCombobox.Location = new System.Drawing.Point(96, 35);
            this.hpacTileCombobox.Name = "hpacTileCombobox";
            this.hpacTileCombobox.Size = new System.Drawing.Size(374, 21);
            this.hpacTileCombobox.TabIndex = 58;
            this.hpacTileCombobox.SelectedIndexChanged += new System.EventHandler(this.hpacTileCombobox_SelectedIndexChanged);
            this.hpacTileCombobox.SelectedValueChanged += new System.EventHandler(this.hpacTileCombobox_SelectedValueChanged);
            // 
            // hpac_ServerComboBox
            // 
            this.hpac_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpac_ServerComboBox.Location = new System.Drawing.Point(96, 3);
            this.hpac_ServerComboBox.Name = "hpac_ServerComboBox";
            this.hpac_ServerComboBox.Size = new System.Drawing.Size(374, 23);
            this.hpac_ServerComboBox.TabIndex = 36;
            // 
            // dynamic_groupBox
            // 
            this.dynamic_groupBox.AutoSize = true;
            this.dynamic_groupBox.Location = new System.Drawing.Point(3, 62);
            this.dynamic_groupBox.Name = "dynamic_groupBox";
            this.dynamic_groupBox.Size = new System.Drawing.Size(654, 348);
            this.dynamic_groupBox.TabIndex = 62;
            this.dynamic_groupBox.TabStop = false;
            // 
            // HpacServerConfigurationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dynamic_groupBox);
            this.Controls.Add(this.hpacTileCombobox);
            this.Controls.Add(this.hpacTile_label);
            this.Controls.Add(this.hpacServer_Label);
            this.Controls.Add(this.hpac_ServerComboBox);
            this.Name = "HpacServerConfigurationConfigControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Framework.UI.ServerComboBox hpac_ServerComboBox;
        private System.Windows.Forms.Label hpacServer_Label;
        private Framework.UI.FieldValidator fieldValidator;

        #endregion
        private System.Windows.Forms.Label hpacTile_label;
        private System.Windows.Forms.ComboBox hpacTileCombobox;
        private System.Windows.Forms.GroupBox dynamic_groupBox;
    }
}
