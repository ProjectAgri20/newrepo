namespace HP.ScalableTest.Plugin.PSPullPrint
{
    partial class PSPullPrintConfigurationControl
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
            this.operation_label = new System.Windows.Forms.Label();
            this.operations_comboBox = new System.Windows.Forms.ComboBox();
            this.addactivity_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tasks_dataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remove_button = new System.Windows.Forms.Button();
            this.movedown_button = new System.Windows.Forms.Button();
            this.moveup_button = new System.Windows.Forms.Button();
            this.queue_groupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pullprint_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.printqueueoperations_groupBox = new System.Windows.Forms.GroupBox();
            this.pacing_label = new System.Windows.Forms.Label();
            this.pacing_timeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.safecomPin_textBox = new System.Windows.Forms.TextBox();
            this.safecomPin_label = new System.Windows.Forms.Label();
            this.solutionApp_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.psp_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tasks_dataGridView)).BeginInit();
            this.queue_groupBox.SuspendLayout();
            this.printqueueoperations_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // operation_label
            // 
            this.operation_label.AutoSize = true;
            this.operation_label.Location = new System.Drawing.Point(57, 57);
            this.operation_label.Name = "operation_label";
            this.operation_label.Size = new System.Drawing.Size(53, 13);
            this.operation_label.TabIndex = 12;
            this.operation_label.Text = "Operation";
            // 
            // operations_comboBox
            // 
            this.operations_comboBox.FormattingEnabled = true;
            this.operations_comboBox.Location = new System.Drawing.Point(116, 54);
            this.operations_comboBox.Name = "operations_comboBox";
            this.operations_comboBox.Size = new System.Drawing.Size(149, 21);
            this.operations_comboBox.TabIndex = 13;
            // 
            // addactivity_button
            // 
            this.addactivity_button.Location = new System.Drawing.Point(218, 84);
            this.addactivity_button.Name = "addactivity_button";
            this.addactivity_button.Size = new System.Drawing.Size(147, 30);
            this.addactivity_button.TabIndex = 18;
            this.addactivity_button.Text = "Add Activity";
            this.addactivity_button.UseVisualStyleBackColor = true;
            this.addactivity_button.Click += new System.EventHandler(this.addactivity_button_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tasks_dataGridView);
            this.panel1.Controls.Add(this.remove_button);
            this.panel1.Controls.Add(this.movedown_button);
            this.panel1.Controls.Add(this.moveup_button);
            this.panel1.Location = new System.Drawing.Point(16, 417);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 162);
            this.panel1.TabIndex = 19;
            // 
            // tasks_dataGridView
            // 
            this.tasks_dataGridView.AllowUserToAddRows = false;
            this.tasks_dataGridView.AllowUserToDeleteRows = false;
            this.tasks_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasks_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.tasks_dataGridView.Location = new System.Drawing.Point(8, 7);
            this.tasks_dataGridView.MultiSelect = false;
            this.tasks_dataGridView.Name = "tasks_dataGridView";
            this.tasks_dataGridView.RowHeadersVisible = false;
            this.tasks_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tasks_dataGridView.Size = new System.Drawing.Size(528, 147);
            this.tasks_dataGridView.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Operation";
            this.dataGridViewTextBoxColumn1.HeaderText = "Activity";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 160;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn2.HeaderText = "Description";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 360;
            // 
            // remove_button
            // 
            this.remove_button.Location = new System.Drawing.Point(542, 103);
            this.remove_button.Name = "remove_button";
            this.remove_button.Size = new System.Drawing.Size(47, 30);
            this.remove_button.TabIndex = 2;
            this.remove_button.Text = "Del";
            this.remove_button.UseVisualStyleBackColor = true;
            this.remove_button.Click += new System.EventHandler(this.remove_button_Click);
            // 
            // movedown_button
            // 
            this.movedown_button.Location = new System.Drawing.Point(542, 67);
            this.movedown_button.Name = "movedown_button";
            this.movedown_button.Size = new System.Drawing.Size(47, 30);
            this.movedown_button.TabIndex = 1;
            this.movedown_button.Text = "Down";
            this.movedown_button.UseVisualStyleBackColor = true;
            this.movedown_button.Click += new System.EventHandler(this.movedown_button_Click);
            // 
            // moveup_button
            // 
            this.moveup_button.Location = new System.Drawing.Point(542, 31);
            this.moveup_button.Name = "moveup_button";
            this.moveup_button.Size = new System.Drawing.Size(47, 30);
            this.moveup_button.TabIndex = 0;
            this.moveup_button.Text = "Up";
            this.moveup_button.UseVisualStyleBackColor = true;
            this.moveup_button.Click += new System.EventHandler(this.moveup_button_Click);
            // 
            // queue_groupBox
            // 
            this.queue_groupBox.Controls.Add(this.lockTimeoutControl);
            this.queue_groupBox.Controls.Add(this.label3);
            this.queue_groupBox.Controls.Add(this.pullprint_assetSelectionControl);
            this.queue_groupBox.Controls.Add(this.panel1);
            this.queue_groupBox.Controls.Add(this.printqueueoperations_groupBox);
            this.queue_groupBox.Location = new System.Drawing.Point(0, 9);
            this.queue_groupBox.Name = "queue_groupBox";
            this.queue_groupBox.Size = new System.Drawing.Size(637, 588);
            this.queue_groupBox.TabIndex = 1;
            this.queue_groupBox.TabStop = false;
            this.queue_groupBox.Text = "Pull Printing Plugin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Note: Only the first enabled device will be chosen for execution.";
            // 
            // pullprint_assetSelectionControl
            // 
            this.pullprint_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pullprint_assetSelectionControl.Location = new System.Drawing.Point(16, 19);
            this.pullprint_assetSelectionControl.Name = "pullprint_assetSelectionControl";
            this.pullprint_assetSelectionControl.Size = new System.Drawing.Size(608, 206);
            this.pullprint_assetSelectionControl.TabIndex = 20;
            // 
            // printqueueoperations_groupBox
            // 
            this.printqueueoperations_groupBox.Controls.Add(this.pacing_label);
            this.printqueueoperations_groupBox.Controls.Add(this.addactivity_button);
            this.printqueueoperations_groupBox.Controls.Add(this.pacing_timeSpanControl);
            this.printqueueoperations_groupBox.Controls.Add(this.operation_label);
            this.printqueueoperations_groupBox.Controls.Add(this.operations_comboBox);
            this.printqueueoperations_groupBox.Controls.Add(this.safecomPin_textBox);
            this.printqueueoperations_groupBox.Controls.Add(this.safecomPin_label);
            this.printqueueoperations_groupBox.Controls.Add(this.solutionApp_comboBox);
            this.printqueueoperations_groupBox.Controls.Add(this.label1);
            this.printqueueoperations_groupBox.Location = new System.Drawing.Point(16, 293);
            this.printqueueoperations_groupBox.Name = "printqueueoperations_groupBox";
            this.printqueueoperations_groupBox.Size = new System.Drawing.Size(608, 120);
            this.printqueueoperations_groupBox.TabIndex = 12;
            this.printqueueoperations_groupBox.TabStop = false;
            this.printqueueoperations_groupBox.Text = "Pull Print Operations";
            // 
            // pacing_label
            // 
            this.pacing_label.AutoSize = true;
            this.pacing_label.Location = new System.Drawing.Point(305, 57);
            this.pacing_label.Name = "pacing_label";
            this.pacing_label.Size = new System.Drawing.Size(77, 13);
            this.pacing_label.TabIndex = 19;
            this.pacing_label.Text = "Activity Pacing";
            // 
            // pacing_timeSpanControl
            // 
            this.pacing_timeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pacing_timeSpanControl.Location = new System.Drawing.Point(385, 52);
            this.pacing_timeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.pacing_timeSpanControl.Name = "pacing_timeSpanControl";
            this.pacing_timeSpanControl.Size = new System.Drawing.Size(110, 23);
            this.pacing_timeSpanControl.TabIndex = 9;
            // 
            // safecomPin_textBox
            // 
            this.safecomPin_textBox.Location = new System.Drawing.Point(385, 20);
            this.safecomPin_textBox.Name = "safecomPin_textBox";
            this.safecomPin_textBox.Size = new System.Drawing.Size(110, 20);
            this.safecomPin_textBox.TabIndex = 2;
            // 
            // safecomPin_label
            // 
            this.safecomPin_label.AutoSize = true;
            this.safecomPin_label.Location = new System.Drawing.Point(290, 27);
            this.safecomPin_label.Name = "safecomPin_label";
            this.safecomPin_label.Size = new System.Drawing.Size(92, 13);
            this.safecomPin_label.TabIndex = 14;
            this.safecomPin_label.Text = "Safecom User Pin";
            // 
            // solutionApp_comboBox
            // 
            this.solutionApp_comboBox.FormattingEnabled = true;
            this.solutionApp_comboBox.Items.AddRange(new object[] {
            "HPAC",
            "Safecom"});
            this.solutionApp_comboBox.Location = new System.Drawing.Point(116, 24);
            this.solutionApp_comboBox.Name = "solutionApp_comboBox";
            this.solutionApp_comboBox.Size = new System.Drawing.Size(149, 21);
            this.solutionApp_comboBox.TabIndex = 3;
            this.solutionApp_comboBox.SelectedIndexChanged += new System.EventHandler(this.SolutionApp_comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Partner Solution";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(16, 228);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 22;
            // 
            // PSPullPrintConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queue_groupBox);
            this.Name = "PSPullPrintConfigurationControl";
            this.Size = new System.Drawing.Size(640, 602);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tasks_dataGridView)).EndInit();
            this.queue_groupBox.ResumeLayout(false);
            this.queue_groupBox.PerformLayout();
            this.printqueueoperations_groupBox.ResumeLayout(false);
            this.printqueueoperations_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button addactivity_button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button remove_button;
        private System.Windows.Forms.Button movedown_button;
        private System.Windows.Forms.Button moveup_button;
        private System.Windows.Forms.GroupBox queue_groupBox;
        private System.Windows.Forms.ComboBox operations_comboBox;
        private System.Windows.Forms.Label operation_label;
        private System.Windows.Forms.ComboBox solutionApp_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView tasks_dataGridView;
        private System.Windows.Forms.Label safecomPin_label;
        private System.Windows.Forms.TextBox safecomPin_textBox;
        private Framework.UI.AssetSelectionControl pullprint_assetSelectionControl;
        private System.Windows.Forms.Label label3;
        private Framework.UI.FieldValidator psp_fieldValidator;
        private System.Windows.Forms.GroupBox printqueueoperations_groupBox;
        private System.Windows.Forms.Label pacing_label;
        private Framework.UI.TimeSpanControl pacing_timeSpanControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
