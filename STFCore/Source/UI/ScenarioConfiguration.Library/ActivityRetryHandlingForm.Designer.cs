namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class ActivityRetryHandlingForm
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
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.retry_Label = new System.Windows.Forms.Label();
            this.delay_Label = new System.Windows.Forms.Label();
            this.retryAction_Label = new System.Windows.Forms.Label();
            this.retry_TextBox = new System.Windows.Forms.TextBox();
            this.errorRetryAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.instructions_Label = new System.Windows.Forms.Label();
            this.main_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.errorAction_Label = new System.Windows.Forms.Label();
            this.skipRetryAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.timeSpanControl1 = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.skip_Label = new System.Windows.Forms.Label();
            this.error_Label = new System.Windows.Forms.Label();
            this.type_Label = new System.Windows.Forms.Label();
            this.delay_TimeSpan = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.failedAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.skippedAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.timeSpanControl2 = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.errorAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.fail_Label = new System.Windows.Forms.Label();
            this.errorRetryWarningLabel = new System.Windows.Forms.Label();
            this.main_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(726, 180);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 28);
            this.cancel_Button.TabIndex = 0;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(620, 180);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 28);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // retry_Label
            // 
            this.retry_Label.AutoSize = true;
            this.retry_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.retry_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retry_Label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.retry_Label.Location = new System.Drawing.Point(227, 1);
            this.retry_Label.Name = "retry_Label";
            this.retry_Label.Size = new System.Drawing.Size(78, 26);
            this.retry_Label.TabIndex = 4;
            this.retry_Label.Text = "Retry Limit";
            this.retry_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delay_Label
            // 
            this.delay_Label.AutoSize = true;
            this.delay_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delay_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delay_Label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.delay_Label.Location = new System.Drawing.Point(312, 1);
            this.delay_Label.Name = "delay_Label";
            this.delay_Label.Size = new System.Drawing.Size(150, 26);
            this.delay_Label.TabIndex = 5;
            this.delay_Label.Text = "Retry Delay";
            this.delay_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // retryAction_Label
            // 
            this.retryAction_Label.AutoSize = true;
            this.retryAction_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.retryAction_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retryAction_Label.Location = new System.Drawing.Point(469, 1);
            this.retryAction_Label.Name = "retryAction_Label";
            this.retryAction_Label.Size = new System.Drawing.Size(338, 26);
            this.retryAction_Label.TabIndex = 6;
            this.retryAction_Label.Text = "Action if Retry Limit is Reached";
            this.retryAction_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // retry_TextBox
            // 
            this.retry_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retry_TextBox.Location = new System.Drawing.Point(227, 31);
            this.retry_TextBox.Name = "retry_TextBox";
            this.retry_TextBox.Size = new System.Drawing.Size(78, 23);
            this.retry_TextBox.TabIndex = 7;
            // 
            // errorRetryAction_ComboBox
            // 
            this.errorRetryAction_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorRetryAction_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorRetryAction_ComboBox.FormattingEnabled = true;
            this.errorRetryAction_ComboBox.Location = new System.Drawing.Point(469, 31);
            this.errorRetryAction_ComboBox.Name = "errorRetryAction_ComboBox";
            this.errorRetryAction_ComboBox.Size = new System.Drawing.Size(338, 23);
            this.errorRetryAction_ComboBox.TabIndex = 8;
            // 
            // instructions_Label
            // 
            this.instructions_Label.AutoSize = true;
            this.instructions_Label.Location = new System.Drawing.Point(12, 9);
            this.instructions_Label.Name = "instructions_Label";
            this.instructions_Label.Size = new System.Drawing.Size(322, 15);
            this.instructions_Label.TabIndex = 10;
            this.instructions_Label.Text = "What should happen when the activity result does not pass?";
            // 
            // main_TableLayoutPanel
            // 
            this.main_TableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.main_TableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.main_TableLayoutPanel.ColumnCount = 5;
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.main_TableLayoutPanel.Controls.Add(this.errorAction_Label, 1, 0);
            this.main_TableLayoutPanel.Controls.Add(this.skipRetryAction_ComboBox, 4, 2);
            this.main_TableLayoutPanel.Controls.Add(this.timeSpanControl1, 3, 2);
            this.main_TableLayoutPanel.Controls.Add(this.textBox1, 2, 2);
            this.main_TableLayoutPanel.Controls.Add(this.skip_Label, 0, 1);
            this.main_TableLayoutPanel.Controls.Add(this.error_Label, 0, 3);
            this.main_TableLayoutPanel.Controls.Add(this.type_Label, 0, 0);
            this.main_TableLayoutPanel.Controls.Add(this.retry_Label, 2, 0);
            this.main_TableLayoutPanel.Controls.Add(this.delay_Label, 3, 0);
            this.main_TableLayoutPanel.Controls.Add(this.retryAction_Label, 4, 0);
            this.main_TableLayoutPanel.Controls.Add(this.retry_TextBox, 2, 1);
            this.main_TableLayoutPanel.Controls.Add(this.delay_TimeSpan, 3, 1);
            this.main_TableLayoutPanel.Controls.Add(this.errorRetryAction_ComboBox, 4, 1);
            this.main_TableLayoutPanel.Controls.Add(this.failedAction_ComboBox, 1, 2);
            this.main_TableLayoutPanel.Controls.Add(this.skippedAction_ComboBox, 1, 1);
            this.main_TableLayoutPanel.Controls.Add(this.textBox2, 2, 3);
            this.main_TableLayoutPanel.Controls.Add(this.timeSpanControl2, 3, 3);
            this.main_TableLayoutPanel.Controls.Add(this.comboBox1, 4, 3);
            this.main_TableLayoutPanel.Controls.Add(this.errorAction_ComboBox, 1, 3);
            this.main_TableLayoutPanel.Controls.Add(this.fail_Label, 0, 2);
            this.main_TableLayoutPanel.Location = new System.Drawing.Point(15, 27);
            this.main_TableLayoutPanel.Name = "main_TableLayoutPanel";
            this.main_TableLayoutPanel.RowCount = 4;
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.62069F));
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.08219F));
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.39726F));
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.17241F));
            this.main_TableLayoutPanel.Size = new System.Drawing.Size(811, 147);
            this.main_TableLayoutPanel.TabIndex = 12;
            // 
            // errorAction_Label
            // 
            this.errorAction_Label.AutoSize = true;
            this.errorAction_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorAction_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorAction_Label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorAction_Label.Location = new System.Drawing.Point(99, 1);
            this.errorAction_Label.Name = "errorAction_Label";
            this.errorAction_Label.Size = new System.Drawing.Size(121, 26);
            this.errorAction_Label.TabIndex = 13;
            this.errorAction_Label.Text = "Action";
            this.errorAction_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // skipRetryAction_ComboBox
            // 
            this.skipRetryAction_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipRetryAction_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skipRetryAction_ComboBox.FormattingEnabled = true;
            this.skipRetryAction_ComboBox.Location = new System.Drawing.Point(469, 72);
            this.skipRetryAction_ComboBox.Name = "skipRetryAction_ComboBox";
            this.skipRetryAction_ComboBox.Size = new System.Drawing.Size(338, 23);
            this.skipRetryAction_ComboBox.TabIndex = 13;
            // 
            // timeSpanControl1
            // 
            this.timeSpanControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSpanControl1.AutoSize = true;
            this.timeSpanControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeSpanControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeSpanControl1.Location = new System.Drawing.Point(309, 72);
            this.timeSpanControl1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.timeSpanControl1.Name = "timeSpanControl1";
            this.timeSpanControl1.Size = new System.Drawing.Size(156, 26);
            this.timeSpanControl1.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(227, 72);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 23);
            this.textBox1.TabIndex = 13;
            // 
            // skip_Label
            // 
            this.skip_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skip_Label.AutoSize = true;
            this.skip_Label.Location = new System.Drawing.Point(4, 33);
            this.skip_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.skip_Label.Name = "skip_Label";
            this.skip_Label.Size = new System.Drawing.Size(88, 15);
            this.skip_Label.TabIndex = 15;
            this.skip_Label.Text = "Skipped";
            this.skip_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // error_Label
            // 
            this.error_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.error_Label.AutoSize = true;
            this.error_Label.Location = new System.Drawing.Point(4, 114);
            this.error_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.error_Label.Name = "error_Label";
            this.error_Label.Size = new System.Drawing.Size(88, 15);
            this.error_Label.TabIndex = 14;
            this.error_Label.Text = "Error";
            this.error_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // type_Label
            // 
            this.type_Label.AutoSize = true;
            this.type_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.type_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.type_Label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.type_Label.Location = new System.Drawing.Point(4, 1);
            this.type_Label.Name = "type_Label";
            this.type_Label.Size = new System.Drawing.Size(88, 26);
            this.type_Label.TabIndex = 13;
            this.type_Label.Text = "Activity Result";
            this.type_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delay_TimeSpan
            // 
            this.delay_TimeSpan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.delay_TimeSpan.AutoSize = true;
            this.delay_TimeSpan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.delay_TimeSpan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delay_TimeSpan.Location = new System.Drawing.Point(309, 31);
            this.delay_TimeSpan.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.delay_TimeSpan.Name = "delay_TimeSpan";
            this.delay_TimeSpan.Size = new System.Drawing.Size(156, 26);
            this.delay_TimeSpan.TabIndex = 11;
            // 
            // failedAction_ComboBox
            // 
            this.failedAction_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.failedAction_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.failedAction_ComboBox.FormattingEnabled = true;
            this.failedAction_ComboBox.Location = new System.Drawing.Point(99, 72);
            this.failedAction_ComboBox.Name = "failedAction_ComboBox";
            this.failedAction_ComboBox.Size = new System.Drawing.Size(121, 23);
            this.failedAction_ComboBox.TabIndex = 13;
            this.failedAction_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Action_ComboBox_SelectedIndexChanged);
            // 
            // skippedAction_ComboBox
            // 
            this.skippedAction_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skippedAction_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skippedAction_ComboBox.FormattingEnabled = true;
            this.skippedAction_ComboBox.Location = new System.Drawing.Point(99, 31);
            this.skippedAction_ComboBox.Name = "skippedAction_ComboBox";
            this.skippedAction_ComboBox.Size = new System.Drawing.Size(121, 23);
            this.skippedAction_ComboBox.TabIndex = 12;
            this.skippedAction_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Action_ComboBox_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(227, 112);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(78, 23);
            this.textBox2.TabIndex = 13;
            // 
            // timeSpanControl2
            // 
            this.timeSpanControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSpanControl2.AutoSize = true;
            this.timeSpanControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeSpanControl2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeSpanControl2.Location = new System.Drawing.Point(309, 112);
            this.timeSpanControl2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.timeSpanControl2.Name = "timeSpanControl2";
            this.timeSpanControl2.Size = new System.Drawing.Size(156, 26);
            this.timeSpanControl2.TabIndex = 13;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(469, 112);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(338, 23);
            this.comboBox1.TabIndex = 13;
            // 
            // errorAction_ComboBox
            // 
            this.errorAction_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorAction_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorAction_ComboBox.FormattingEnabled = true;
            this.errorAction_ComboBox.Location = new System.Drawing.Point(99, 112);
            this.errorAction_ComboBox.Name = "errorAction_ComboBox";
            this.errorAction_ComboBox.Size = new System.Drawing.Size(121, 23);
            this.errorAction_ComboBox.TabIndex = 13;
            this.errorAction_ComboBox.SelectedIndexChanged += new System.EventHandler(this.Action_ComboBox_SelectedIndexChanged);
            // 
            // fail_Label
            // 
            this.fail_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fail_Label.AutoSize = true;
            this.fail_Label.Location = new System.Drawing.Point(4, 74);
            this.fail_Label.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.fail_Label.Name = "fail_Label";
            this.fail_Label.Size = new System.Drawing.Size(88, 15);
            this.fail_Label.TabIndex = 15;
            this.fail_Label.Text = "Failed";
            this.fail_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorRetryWarningLabel
            // 
            this.errorRetryWarningLabel.AutoSize = true;
            this.errorRetryWarningLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorRetryWarningLabel.Location = new System.Drawing.Point(19, 187);
            this.errorRetryWarningLabel.Name = "errorRetryWarningLabel";
            this.errorRetryWarningLabel.Size = new System.Drawing.Size(484, 15);
            this.errorRetryWarningLabel.TabIndex = 13;
            this.errorRetryWarningLabel.Text = "Note: Setting Error results to Retry may have unexpected behavior and is not reco" +
    "mmended.";
            this.errorRetryWarningLabel.Visible = false;
            // 
            // ActivityRetryHandlingForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(835, 218);
            this.Controls.Add(this.errorRetryWarningLabel);
            this.Controls.Add(this.main_TableLayoutPanel);
            this.Controls.Add(this.instructions_Label);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActivityRetryHandlingForm";
            this.Text = "Retry Handling Options";
            this.main_TableLayoutPanel.ResumeLayout(false);
            this.main_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label retry_Label;
        private System.Windows.Forms.Label delay_Label;
        private System.Windows.Forms.Label retryAction_Label;
        private System.Windows.Forms.TextBox retry_TextBox;
        private System.Windows.Forms.ComboBox errorRetryAction_ComboBox;
        private System.Windows.Forms.Label instructions_Label;
        private HP.ScalableTest.Framework.UI.TimeSpanControl delay_TimeSpan;
        private System.Windows.Forms.TableLayoutPanel main_TableLayoutPanel;
        private System.Windows.Forms.Label errorAction_Label;
        private System.Windows.Forms.ComboBox skipRetryAction_ComboBox;
        private HP.ScalableTest.Framework.UI.TimeSpanControl timeSpanControl1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label error_Label;
        private System.Windows.Forms.Label type_Label;
        private System.Windows.Forms.Label skip_Label;
        private System.Windows.Forms.ComboBox skippedAction_ComboBox;
        private System.Windows.Forms.ComboBox failedAction_ComboBox;
        private System.Windows.Forms.TextBox textBox2;
        private ScalableTest.Framework.UI.TimeSpanControl timeSpanControl2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label fail_Label;
        private System.Windows.Forms.ComboBox errorAction_ComboBox;
        private System.Windows.Forms.Label errorRetryWarningLabel;
    }
}