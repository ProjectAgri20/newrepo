namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    partial class DssConfigurationConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tasksComboBox = new System.Windows.Forms.ComboBox();
            this.parametersGroupBox = new System.Windows.Forms.GroupBox();
            this.tasknameLabel = new System.Windows.Forms.Label();
            this.scrollpanel = new System.Windows.Forms.Panel();
            this.workflowToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.description_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scrollpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tasksComboBox
            // 
            this.tasksComboBox.FormattingEnabled = true;
            this.tasksComboBox.Location = new System.Drawing.Point(49, 30);
            this.tasksComboBox.Name = "tasksComboBox";
            this.tasksComboBox.Size = new System.Drawing.Size(202, 23);
            this.tasksComboBox.TabIndex = 0;
            this.workflowToolTip.SetToolTip(this.tasksComboBox, "Please select a workflow from the list");
            this.tasksComboBox.SelectedIndexChanged += new System.EventHandler(this.TasksComboBoxSelectedIndexChanged);
            // 
            // parametersGroupBox
            // 
            this.parametersGroupBox.AutoSize = true;
            this.parametersGroupBox.Location = new System.Drawing.Point(3, 3);
            this.parametersGroupBox.Name = "parametersGroupBox";
            this.parametersGroupBox.Size = new System.Drawing.Size(558, 378);
            this.parametersGroupBox.TabIndex = 1;
            this.parametersGroupBox.TabStop = false;
            this.parametersGroupBox.Text = "Parameters";
            // 
            // tasknameLabel
            // 
            this.tasknameLabel.AutoSize = true;
            this.tasknameLabel.Location = new System.Drawing.Point(46, 10);
            this.tasknameLabel.Name = "tasknameLabel";
            this.tasknameLabel.Size = new System.Drawing.Size(58, 15);
            this.tasknameLabel.TabIndex = 2;
            this.tasknameLabel.Text = "Workflow";
            // 
            // scrollpanel
            // 
            this.scrollpanel.AutoScroll = true;
            this.scrollpanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scrollpanel.Controls.Add(this.parametersGroupBox);
            this.scrollpanel.Location = new System.Drawing.Point(49, 81);
            this.scrollpanel.Name = "scrollpanel";
            this.scrollpanel.Size = new System.Drawing.Size(567, 387);
            this.scrollpanel.TabIndex = 3;
            // 
            // description_textBox
            // 
            this.description_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.description_textBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description_textBox.Location = new System.Drawing.Point(124, 62);
            this.description_textBox.Name = "description_textBox";
            this.description_textBox.ReadOnly = true;
            this.description_textBox.Size = new System.Drawing.Size(476, 16);
            this.description_textBox.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Description:";
            // 
            // DSSConfigurationConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.description_textBox);
            this.Controls.Add(this.scrollpanel);
            this.Controls.Add(this.tasknameLabel);
            this.Controls.Add(this.tasksComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DssConfigurationConfigurationControl";
            this.Size = new System.Drawing.Size(640, 480);
            this.Load += new System.EventHandler(this.DSSConfigurationConfigurationControl_Load);
            this.scrollpanel.ResumeLayout(false);
            this.scrollpanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.ComboBox tasksComboBox;
        private System.Windows.Forms.GroupBox parametersGroupBox;
        private System.Windows.Forms.Label tasknameLabel;
        private System.Windows.Forms.Panel scrollpanel;
        private System.Windows.Forms.ToolTip workflowToolTip;
        private System.Windows.Forms.TextBox description_textBox;
        private System.Windows.Forms.Label label1;
    }
}
