namespace HP.ScalableTest.LabConsole
{
    partial class PerfMonConfigurationForm
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
            this.insert_Button = new System.Windows.Forms.Button();
            this.add_Button = new System.Windows.Forms.Button();
            this.remove_Button = new System.Windows.Forms.Button();
            this.selected_Label = new System.Windows.Forms.Label();
            this.selectedCounters_DataGridView = new System.Windows.Forms.DataGridView();
            this.targetHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.counter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.selectedCounters_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(779, 439);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 28);
            this.cancel_Button.TabIndex = 26;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // insert_Button
            // 
            this.insert_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.insert_Button.Location = new System.Drawing.Point(671, 439);
            this.insert_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.insert_Button.Name = "insert_Button";
            this.insert_Button.Size = new System.Drawing.Size(100, 28);
            this.insert_Button.TabIndex = 27;
            this.insert_Button.Text = "Insert";
            this.insert_Button.UseVisualStyleBackColor = true;
            this.insert_Button.Click += new System.EventHandler(this.insert_Button_Click);
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.add_Button.Location = new System.Drawing.Point(341, 226);
            this.add_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(100, 28);
            this.add_Button.TabIndex = 29;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // remove_Button
            // 
            this.remove_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remove_Button.Location = new System.Drawing.Point(449, 226);
            this.remove_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(100, 28);
            this.remove_Button.TabIndex = 28;
            this.remove_Button.Text = "Remove";
            this.remove_Button.UseVisualStyleBackColor = true;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // selected_Label
            // 
            this.selected_Label.AutoSize = true;
            this.selected_Label.Location = new System.Drawing.Point(3, 241);
            this.selected_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selected_Label.Name = "selected_Label";
            this.selected_Label.Size = new System.Drawing.Size(124, 17);
            this.selected_Label.TabIndex = 30;
            this.selected_Label.Text = "Selected Counters";
            // 
            // selectedCounters_DataGridView
            // 
            this.selectedCounters_DataGridView.AllowUserToAddRows = false;
            this.selectedCounters_DataGridView.AllowUserToDeleteRows = false;
            this.selectedCounters_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedCounters_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.selectedCounters_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.targetHost,
            this.category,
            this.instance,
            this.counter});
            this.selectedCounters_DataGridView.Location = new System.Drawing.Point(5, 266);
            this.selectedCounters_DataGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectedCounters_DataGridView.MultiSelect = false;
            this.selectedCounters_DataGridView.Name = "selectedCounters_DataGridView";
            this.selectedCounters_DataGridView.RowHeadersVisible = false;
            this.selectedCounters_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.selectedCounters_DataGridView.Size = new System.Drawing.Size(883, 165);
            this.selectedCounters_DataGridView.TabIndex = 72;
            // 
            // targetHost
            // 
            this.targetHost.DataPropertyName = "TargetHost";
            this.targetHost.HeaderText = "Host Name";
            this.targetHost.Name = "targetHost";
            this.targetHost.Width = 150;
            // 
            // category
            // 
            this.category.DataPropertyName = "Category";
            this.category.HeaderText = "Category";
            this.category.Name = "category";
            this.category.Width = 150;
            // 
            // instance
            // 
            this.instance.DataPropertyName = "InstanceName";
            this.instance.HeaderText = "Instance";
            this.instance.Name = "instance";
            this.instance.Width = 150;
            // 
            // counter
            // 
            this.counter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.counter.DataPropertyName = "Counter";
            this.counter.HeaderText = "Counter";
            this.counter.Name = "counter";
            // 
            // PerfMonConfigurationForm
            // 
            this.AcceptButton = this.insert_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(892, 480);
            this.Controls.Add(this.selectedCounters_DataGridView);
            this.Controls.Add(this.selected_Label);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.remove_Button);
            this.Controls.Add(this.insert_Button);
            this.Controls.Add(this.cancel_Button);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PerfMonConfigurationForm";
            this.Text = "STF PerfMon Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.selectedCounters_DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button insert_Button;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button remove_Button;
        private System.Windows.Forms.Label selected_Label;
        private System.Windows.Forms.DataGridView selectedCounters_DataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn instance;
        private System.Windows.Forms.DataGridViewTextBoxColumn counter;
    }
}