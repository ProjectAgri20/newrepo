namespace HP.ScalableTest.Plugin.Authentication
{
    partial class AuthenticationConfigControl
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
            this.comboBox_InitiatorButton = new System.Windows.Forms.ComboBox();
            this.label_InitiatingButton = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.label_UnAuthenticateMethod = new System.Windows.Forms.Label();
            this.comboBox_Unauthenticate = new System.Windows.Forms.ComboBox();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.label_AuthProvider = new System.Windows.Forms.Label();
            this.comboBox_AuthMethod = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBox_InitiatorButton
            // 
            this.comboBox_InitiatorButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_InitiatorButton.FormattingEnabled = true;
            this.comboBox_InitiatorButton.Location = new System.Drawing.Point(272, 7);
            this.comboBox_InitiatorButton.Name = "comboBox_InitiatorButton";
            this.comboBox_InitiatorButton.Size = new System.Drawing.Size(255, 23);
            this.comboBox_InitiatorButton.TabIndex = 0;
            this.comboBox_InitiatorButton.SelectedIndexChanged += new System.EventHandler(this.comboBox_InitiatorButton_SelectedIndexChanged);
            // 
            // label_InitiatingButton
            // 
            this.label_InitiatingButton.AutoSize = true;
            this.label_InitiatingButton.Location = new System.Drawing.Point(3, 11);
            this.label_InitiatingButton.Name = "label_InitiatingButton";
            this.label_InitiatingButton.Size = new System.Drawing.Size(198, 15);
            this.label_InitiatingButton.TabIndex = 2;
            this.label_InitiatingButton.Text = "Authentication Initialization Method";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 167);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(521, 247);
            this.assetSelectionControl.TabIndex = 17;
            // 
            // label_UnAuthenticateMethod
            // 
            this.label_UnAuthenticateMethod.AutoSize = true;
            this.label_UnAuthenticateMethod.Location = new System.Drawing.Point(7, 112);
            this.label_UnAuthenticateMethod.Name = "label_UnAuthenticateMethod";
            this.label_UnAuthenticateMethod.Size = new System.Drawing.Size(98, 15);
            this.label_UnAuthenticateMethod.TabIndex = 19;
            this.label_UnAuthenticateMethod.Text = "Sign Out Method";
            // 
            // comboBox_Unauthenticate
            // 
            this.comboBox_Unauthenticate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Unauthenticate.FormattingEnabled = true;
            this.comboBox_Unauthenticate.Location = new System.Drawing.Point(111, 108);
            this.comboBox_Unauthenticate.Name = "comboBox_Unauthenticate";
            this.comboBox_Unauthenticate.Size = new System.Drawing.Size(158, 23);
            this.comboBox_Unauthenticate.TabIndex = 20;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(272, 70);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(255, 23);
            this.comboBox_AuthProvider.TabIndex = 21;
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(3, 43);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 22;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(286, 108);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 23;
            // 
            // label_AuthProvider
            // 
            this.label_AuthProvider.AutoSize = true;
            this.label_AuthProvider.Location = new System.Drawing.Point(3, 70);
            this.label_AuthProvider.Name = "label_AuthProvider";
            this.label_AuthProvider.Size = new System.Drawing.Size(133, 15);
            this.label_AuthProvider.TabIndex = 24;
            this.label_AuthProvider.Text = "Authentication Provider";
            // 
            // comboBox_AuthMethod
            // 
            this.comboBox_AuthMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthMethod.FormattingEnabled = true;
            this.comboBox_AuthMethod.Location = new System.Drawing.Point(272, 38);
            this.comboBox_AuthMethod.Name = "comboBox_AuthMethod";
            this.comboBox_AuthMethod.Size = new System.Drawing.Size(255, 23);
            this.comboBox_AuthMethod.TabIndex = 25;
            this.comboBox_AuthMethod.SelectedIndexChanged += new System.EventHandler(this.comboBox_AuthMethod_SelectedIndexChanged);
            // 
            // AuthenticationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_AuthMethod);
            this.Controls.Add(this.label_AuthProvider);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.label_AuthMethod);
            this.Controls.Add(this.comboBox_AuthProvider);
            this.Controls.Add(this.comboBox_Unauthenticate);
            this.Controls.Add(this.label_UnAuthenticateMethod);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.label_InitiatingButton);
            this.Controls.Add(this.comboBox_InitiatorButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AuthenticationConfigControl";
            this.Size = new System.Drawing.Size(544, 417);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_InitiatorButton;
        private System.Windows.Forms.Label label_InitiatingButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label label_UnAuthenticateMethod;
        private System.Windows.Forms.ComboBox comboBox_Unauthenticate;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.Label label_AuthProvider;
        private System.Windows.Forms.ComboBox comboBox_AuthMethod;
    }
}
