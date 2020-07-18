namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class WizardScenarioBatchPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardScenarioBatchPage));
            this.session_GroupBox = new System.Windows.Forms.GroupBox();
            this.sessionName_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionName_Label = new System.Windows.Forms.Label();
            this.reference_Label = new System.Windows.Forms.Label();
            this.sessionType_Label = new System.Windows.Forms.Label();
            this.sessionCycle_Label = new System.Windows.Forms.Label();
            this.reference_TextBox = new System.Windows.Forms.TextBox();
            this.sessionType_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionCycle_ComboBox = new System.Windows.Forms.ComboBox();
            this.runtime_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.runtime_Label = new System.Windows.Forms.Label();
            this.retention_ComboBox = new System.Windows.Forms.ComboBox();
            this.notes_TextBox = new System.Windows.Forms.TextBox();
            this.notes_Label = new System.Windows.Forms.Label();
            this.retention_Label = new System.Windows.Forms.Label();
            this.scenarios_DataGridView = new System.Windows.Forms.DataGridView();
            this.column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.up_Button = new System.Windows.Forms.Button();
            this.down_Button = new System.Windows.Forms.Button();
            this.remove_Button = new System.Windows.Forms.Button();
            this.add_Button = new System.Windows.Forms.Button();
            this.connection_GroupBox = new System.Windows.Forms.GroupBox();
            this.environment_Label = new System.Windows.Forms.Label();
            this.dispatcher_Label = new System.Windows.Forms.Label();
            this.connection_Label = new System.Windows.Forms.Label();
            this.session_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtime_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarios_DataGridView)).BeginInit();
            this.connection_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // session_GroupBox
            // 
            this.session_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.session_GroupBox.Controls.Add(this.sessionName_ComboBox);
            this.session_GroupBox.Controls.Add(this.sessionName_Label);
            this.session_GroupBox.Controls.Add(this.reference_Label);
            this.session_GroupBox.Controls.Add(this.sessionType_Label);
            this.session_GroupBox.Controls.Add(this.sessionCycle_Label);
            this.session_GroupBox.Controls.Add(this.reference_TextBox);
            this.session_GroupBox.Controls.Add(this.sessionType_ComboBox);
            this.session_GroupBox.Controls.Add(this.sessionCycle_ComboBox);
            this.session_GroupBox.Controls.Add(this.runtime_NumericUpDown);
            this.session_GroupBox.Controls.Add(this.runtime_Label);
            this.session_GroupBox.Controls.Add(this.retention_ComboBox);
            this.session_GroupBox.Controls.Add(this.notes_TextBox);
            this.session_GroupBox.Controls.Add(this.notes_Label);
            this.session_GroupBox.Controls.Add(this.retention_Label);
            this.session_GroupBox.Location = new System.Drawing.Point(7, 171);
            this.session_GroupBox.Name = "session_GroupBox";
            this.session_GroupBox.Size = new System.Drawing.Size(641, 182);
            this.session_GroupBox.TabIndex = 5;
            this.session_GroupBox.TabStop = false;
            this.session_GroupBox.Text = "Session Settings";
            // 
            // sessionName_ComboBox
            // 
            this.sessionName_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionName_ComboBox.DisplayMember = "Name";
            this.sessionName_ComboBox.FormattingEnabled = true;
            this.sessionName_ComboBox.Location = new System.Drawing.Point(124, 32);
            this.sessionName_ComboBox.Name = "sessionName_ComboBox";
            this.sessionName_ComboBox.Size = new System.Drawing.Size(495, 23);
            this.sessionName_ComboBox.TabIndex = 42;
            this.sessionName_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionName_Label
            // 
            this.sessionName_Label.Location = new System.Drawing.Point(16, 35);
            this.sessionName_Label.Name = "sessionName_Label";
            this.sessionName_Label.Size = new System.Drawing.Size(102, 20);
            this.sessionName_Label.TabIndex = 41;
            this.sessionName_Label.Text = "Session Name";
            this.sessionName_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // reference_Label
            // 
            this.reference_Label.AutoSize = true;
            this.reference_Label.Location = new System.Drawing.Point(452, 65);
            this.reference_Label.Name = "reference_Label";
            this.reference_Label.Size = new System.Drawing.Size(59, 15);
            this.reference_Label.TabIndex = 40;
            this.reference_Label.Text = "Reference";
            this.reference_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionType_Label
            // 
            this.sessionType_Label.Location = new System.Drawing.Point(15, 64);
            this.sessionType_Label.Name = "sessionType_Label";
            this.sessionType_Label.Size = new System.Drawing.Size(103, 23);
            this.sessionType_Label.TabIndex = 39;
            this.sessionType_Label.Text = "Session Type";
            this.sessionType_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionCycle_Label
            // 
            this.sessionCycle_Label.AutoSize = true;
            this.sessionCycle_Label.Location = new System.Drawing.Point(269, 65);
            this.sessionCycle_Label.Name = "sessionCycle_Label";
            this.sessionCycle_Label.Size = new System.Drawing.Size(36, 15);
            this.sessionCycle_Label.TabIndex = 38;
            this.sessionCycle_Label.Text = "Cycle";
            this.sessionCycle_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // reference_TextBox
            // 
            this.reference_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reference_TextBox.Location = new System.Drawing.Point(520, 61);
            this.reference_TextBox.Name = "reference_TextBox";
            this.reference_TextBox.Size = new System.Drawing.Size(100, 23);
            this.reference_TextBox.TabIndex = 37;
            // 
            // sessionType_ComboBox
            // 
            this.sessionType_ComboBox.DisplayMember = "Name";
            this.sessionType_ComboBox.FormattingEnabled = true;
            this.sessionType_ComboBox.Location = new System.Drawing.Point(124, 62);
            this.sessionType_ComboBox.Name = "sessionType_ComboBox";
            this.sessionType_ComboBox.Size = new System.Drawing.Size(132, 23);
            this.sessionType_ComboBox.TabIndex = 36;
            this.sessionType_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionCycle_ComboBox
            // 
            this.sessionCycle_ComboBox.DisplayMember = "Name";
            this.sessionCycle_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sessionCycle_ComboBox.FormattingEnabled = true;
            this.sessionCycle_ComboBox.Location = new System.Drawing.Point(311, 61);
            this.sessionCycle_ComboBox.Name = "sessionCycle_ComboBox";
            this.sessionCycle_ComboBox.Size = new System.Drawing.Size(132, 23);
            this.sessionCycle_ComboBox.TabIndex = 35;
            this.sessionCycle_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // runtime_NumericUpDown
            // 
            this.runtime_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runtime_NumericUpDown.Location = new System.Drawing.Point(544, 145);
            this.runtime_NumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.runtime_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.runtime_NumericUpDown.Name = "runtime_NumericUpDown";
            this.runtime_NumericUpDown.Size = new System.Drawing.Size(76, 23);
            this.runtime_NumericUpDown.TabIndex = 32;
            this.runtime_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // runtime_Label
            // 
            this.runtime_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runtime_Label.Location = new System.Drawing.Point(304, 149);
            this.runtime_Label.Name = "runtime_Label";
            this.runtime_Label.Size = new System.Drawing.Size(215, 23);
            this.runtime_Label.TabIndex = 31;
            this.runtime_Label.Text = "Estimated Runtime (hours)";
            this.runtime_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // retention_ComboBox
            // 
            this.retention_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retention_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.retention_ComboBox.FormattingEnabled = true;
            this.retention_ComboBox.ItemHeight = 15;
            this.retention_ComboBox.Location = new System.Drawing.Point(124, 144);
            this.retention_ComboBox.Name = "retention_ComboBox";
            this.retention_ComboBox.Size = new System.Drawing.Size(146, 23);
            this.retention_ComboBox.TabIndex = 29;
            // 
            // notes_TextBox
            // 
            this.notes_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.notes_TextBox.Location = new System.Drawing.Point(124, 95);
            this.notes_TextBox.Multiline = true;
            this.notes_TextBox.Name = "notes_TextBox";
            this.notes_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notes_TextBox.Size = new System.Drawing.Size(495, 43);
            this.notes_TextBox.TabIndex = 28;
            // 
            // notes_Label
            // 
            this.notes_Label.Location = new System.Drawing.Point(62, 98);
            this.notes_Label.Name = "notes_Label";
            this.notes_Label.Size = new System.Drawing.Size(56, 23);
            this.notes_Label.TabIndex = 27;
            this.notes_Label.Text = "Notes";
            this.notes_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // retention_Label
            // 
            this.retention_Label.Location = new System.Drawing.Point(6, 147);
            this.retention_Label.Name = "retention_Label";
            this.retention_Label.Size = new System.Drawing.Size(112, 23);
            this.retention_Label.TabIndex = 30;
            this.retention_Label.Text = "Log Retention";
            this.retention_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // scenarios_DataGridView
            // 
            this.scenarios_DataGridView.AllowUserToAddRows = false;
            this.scenarios_DataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.scenarios_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.scenarios_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scenarios_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.scenarios_DataGridView.ColumnHeadersHeight = 28;
            this.scenarios_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.scenarios_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_Name});
            this.scenarios_DataGridView.EnableHeadersVisualStyles = false;
            this.scenarios_DataGridView.Location = new System.Drawing.Point(6, 3);
            this.scenarios_DataGridView.MultiSelect = false;
            this.scenarios_DataGridView.Name = "scenarios_DataGridView";
            this.scenarios_DataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scenarios_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.scenarios_DataGridView.RowHeadersWidth = 20;
            this.scenarios_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scenarios_DataGridView.Size = new System.Drawing.Size(564, 162);
            this.scenarios_DataGridView.TabIndex = 6;
            // 
            // column_Name
            // 
            this.column_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_Name.DataPropertyName = "Name";
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.column_Name.DefaultCellStyle = dataGridViewCellStyle3;
            this.column_Name.HeaderText = "Selected Scenarios";
            this.column_Name.Name = "column_Name";
            this.column_Name.ReadOnly = true;
            // 
            // up_Button
            // 
            this.up_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.up_Button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.up_Button.Image = ((System.Drawing.Image)(resources.GetObject("up_Button.Image")));
            this.up_Button.Location = new System.Drawing.Point(576, 33);
            this.up_Button.Name = "up_Button";
            this.up_Button.Size = new System.Drawing.Size(72, 27);
            this.up_Button.TabIndex = 7;
            this.up_Button.UseVisualStyleBackColor = false;
            this.up_Button.Click += new System.EventHandler(this.up_Button_Click);
            // 
            // down_Button
            // 
            this.down_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.down_Button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.down_Button.Image = ((System.Drawing.Image)(resources.GetObject("down_Button.Image")));
            this.down_Button.Location = new System.Drawing.Point(576, 66);
            this.down_Button.Name = "down_Button";
            this.down_Button.Size = new System.Drawing.Size(72, 27);
            this.down_Button.TabIndex = 8;
            this.down_Button.UseVisualStyleBackColor = false;
            this.down_Button.Click += new System.EventHandler(this.down_Button_Click);
            // 
            // remove_Button
            // 
            this.remove_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remove_Button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.remove_Button.Location = new System.Drawing.Point(576, 138);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(71, 27);
            this.remove_Button.TabIndex = 9;
            this.remove_Button.Text = "Remove";
            this.remove_Button.UseVisualStyleBackColor = false;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.add_Button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.add_Button.Location = new System.Drawing.Point(577, 105);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(71, 27);
            this.add_Button.TabIndex = 10;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = false;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // connection_GroupBox
            // 
            this.connection_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connection_GroupBox.Controls.Add(this.environment_Label);
            this.connection_GroupBox.Controls.Add(this.dispatcher_Label);
            this.connection_GroupBox.Controls.Add(this.connection_Label);
            this.connection_GroupBox.Location = new System.Drawing.Point(7, 367);
            this.connection_GroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connection_GroupBox.Name = "connection_GroupBox";
            this.connection_GroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connection_GroupBox.Size = new System.Drawing.Size(641, 45);
            this.connection_GroupBox.TabIndex = 11;
            this.connection_GroupBox.TabStop = false;
            this.connection_GroupBox.Text = "Environment Settings";
            // 
            // environment_Label
            // 
            this.environment_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.environment_Label.Location = new System.Drawing.Point(358, 21);
            this.environment_Label.Name = "environment_Label";
            this.environment_Label.Size = new System.Drawing.Size(244, 22);
            this.environment_Label.TabIndex = 2;
            this.environment_Label.Text = "<Environment>";
            this.environment_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dispatcher_Label
            // 
            this.dispatcher_Label.Location = new System.Drawing.Point(112, 21);
            this.dispatcher_Label.Name = "dispatcher_Label";
            this.dispatcher_Label.Size = new System.Drawing.Size(121, 22);
            this.dispatcher_Label.TabIndex = 1;
            this.dispatcher_Label.Text = "[Not Connected]";
            // 
            // connection_Label
            // 
            this.connection_Label.AutoSize = true;
            this.connection_Label.Location = new System.Drawing.Point(17, 21);
            this.connection_Label.Name = "connection_Label";
            this.connection_Label.Size = new System.Drawing.Size(82, 15);
            this.connection_Label.TabIndex = 0;
            this.connection_Label.Text = "Connected to:";
            // 
            // WizardScenarioBatchPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.connection_GroupBox);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.remove_Button);
            this.Controls.Add(this.down_Button);
            this.Controls.Add(this.up_Button);
            this.Controls.Add(this.scenarios_DataGridView);
            this.Controls.Add(this.session_GroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardScenarioBatchPage";
            this.Size = new System.Drawing.Size(656, 424);
            this.session_GroupBox.ResumeLayout(false);
            this.session_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtime_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarios_DataGridView)).EndInit();
            this.connection_GroupBox.ResumeLayout(false);
            this.connection_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox session_GroupBox;
        private System.Windows.Forms.Label reference_Label;
        private System.Windows.Forms.Label sessionType_Label;
        private System.Windows.Forms.Label sessionCycle_Label;
        private System.Windows.Forms.TextBox reference_TextBox;
        protected System.Windows.Forms.ComboBox sessionType_ComboBox;
        protected System.Windows.Forms.ComboBox sessionCycle_ComboBox;
        protected System.Windows.Forms.NumericUpDown runtime_NumericUpDown;
        protected System.Windows.Forms.Label runtime_Label;
        protected System.Windows.Forms.ComboBox retention_ComboBox;
        protected System.Windows.Forms.TextBox notes_TextBox;
        protected System.Windows.Forms.Label notes_Label;
        protected System.Windows.Forms.Label retention_Label;
        private System.Windows.Forms.DataGridView scenarios_DataGridView;
        private System.Windows.Forms.Button up_Button;
        private System.Windows.Forms.Button down_Button;
        private System.Windows.Forms.Button remove_Button;
        private System.Windows.Forms.Button add_Button;
        protected System.Windows.Forms.GroupBox connection_GroupBox;
        protected System.Windows.Forms.Label environment_Label;
        protected System.Windows.Forms.Label dispatcher_Label;
        protected System.Windows.Forms.Label connection_Label;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Name;
        protected System.Windows.Forms.ComboBox sessionName_ComboBox;
        protected System.Windows.Forms.Label sessionName_Label;
    }
}
