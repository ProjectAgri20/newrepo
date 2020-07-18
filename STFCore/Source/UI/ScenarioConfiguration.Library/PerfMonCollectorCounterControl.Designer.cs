namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class PerfMonCollectorCounterControl
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
            this.title_Label = new System.Windows.Forms.Label();
            this.available_Label = new System.Windows.Forms.Label();
            this.available_TreeView = new System.Windows.Forms.TreeView();
            this.hostName_Label = new System.Windows.Forms.Label();
            this.hostNameDisplay_Label = new System.Windows.Forms.Label();
            this.selected_GroupBox = new System.Windows.Forms.GroupBox();
            this.collect_CheckBox = new System.Windows.Forms.CheckBox();
            this.mmss_Label = new System.Windows.Forms.Label();
            this.interval_TextBox = new System.Windows.Forms.TextBox();
            this.counter_TextBox = new System.Windows.Forms.TextBox();
            this.instance_TextBox = new System.Windows.Forms.TextBox();
            this.category_TextBox = new System.Windows.Forms.TextBox();
            this.category_Label = new System.Windows.Forms.Label();
            this.instance_Label = new System.Windows.Forms.Label();
            this.counter_Label = new System.Windows.Forms.Label();
            this.interval_Label = new System.Windows.Forms.Label();
            this.radSeparator1 = new Telerik.WinControls.UI.RadSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).BeginInit();
            this.selected_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).BeginInit();
            this.SuspendLayout();
            // 
            // title_Label
            // 
            this.title_Label.AutoSize = true;
            this.title_Label.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_Label.Location = new System.Drawing.Point(13, 13);
            this.title_Label.Name = "title_Label";
            this.title_Label.Size = new System.Drawing.Size(374, 25);
            this.title_Label.TabIndex = 17;
            this.title_Label.Text = "PerfMon Collector - Counter Configuration";
            // 
            // available_Label
            // 
            this.available_Label.AutoSize = true;
            this.available_Label.Location = new System.Drawing.Point(15, 86);
            this.available_Label.Name = "available_Label";
            this.available_Label.Size = new System.Drawing.Size(106, 15);
            this.available_Label.TabIndex = 54;
            this.available_Label.Text = "Available Counters";
            // 
            // available_TreeView
            // 
            this.available_TreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.available_TreeView.CheckBoxes = true;
            this.available_TreeView.Location = new System.Drawing.Point(18, 104);
            this.available_TreeView.Name = "available_TreeView";
            this.available_TreeView.Size = new System.Drawing.Size(548, 152);
            this.available_TreeView.TabIndex = 53;
            // 
            // hostName_Label
            // 
            this.hostName_Label.AutoSize = true;
            this.hostName_Label.Location = new System.Drawing.Point(15, 54);
            this.hostName_Label.Name = "hostName_Label";
            this.hostName_Label.Size = new System.Drawing.Size(70, 15);
            this.hostName_Label.TabIndex = 51;
            this.hostName_Label.Text = "Host Name:";
            // 
            // hostNameDisplay_Label
            // 
            this.hostNameDisplay_Label.AutoSize = true;
            this.hostNameDisplay_Label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hostNameDisplay_Label.Location = new System.Drawing.Point(89, 54);
            this.hostNameDisplay_Label.Name = "hostNameDisplay_Label";
            this.hostNameDisplay_Label.Size = new System.Drawing.Size(74, 15);
            this.hostNameDisplay_Label.TabIndex = 57;
            this.hostNameDisplay_Label.Text = "Host Server";
            // 
            // selected_GroupBox
            // 
            this.selected_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selected_GroupBox.Controls.Add(this.collect_CheckBox);
            this.selected_GroupBox.Controls.Add(this.mmss_Label);
            this.selected_GroupBox.Controls.Add(this.interval_TextBox);
            this.selected_GroupBox.Controls.Add(this.counter_TextBox);
            this.selected_GroupBox.Controls.Add(this.instance_TextBox);
            this.selected_GroupBox.Controls.Add(this.category_TextBox);
            this.selected_GroupBox.Controls.Add(this.category_Label);
            this.selected_GroupBox.Controls.Add(this.instance_Label);
            this.selected_GroupBox.Controls.Add(this.counter_Label);
            this.selected_GroupBox.Controls.Add(this.interval_Label);
            this.selected_GroupBox.Location = new System.Drawing.Point(18, 262);
            this.selected_GroupBox.Name = "selected_GroupBox";
            this.selected_GroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.selected_GroupBox.Size = new System.Drawing.Size(548, 162);
            this.selected_GroupBox.TabIndex = 58;
            this.selected_GroupBox.TabStop = false;
            this.selected_GroupBox.Text = "Selected Counter Configuration";
            // 
            // collect_CheckBox
            // 
            this.collect_CheckBox.AutoSize = true;
            this.collect_CheckBox.Location = new System.Drawing.Point(52, 132);
            this.collect_CheckBox.Name = "collect_CheckBox";
            this.collect_CheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.collect_CheckBox.Size = new System.Drawing.Size(107, 19);
            this.collect_CheckBox.TabIndex = 13;
            this.collect_CheckBox.Text = "Collect on Start";
            this.collect_CheckBox.UseVisualStyleBackColor = true;
            // 
            // mmss_Label
            // 
            this.mmss_Label.AutoSize = true;
            this.mmss_Label.Location = new System.Drawing.Point(210, 107);
            this.mmss_Label.Name = "mmss_Label";
            this.mmss_Label.Size = new System.Drawing.Size(50, 15);
            this.mmss_Label.TabIndex = 12;
            this.mmss_Label.Text = "(mm:ss)";
            // 
            // interval_TextBox
            // 
            this.interval_TextBox.Location = new System.Drawing.Point(149, 104);
            this.interval_TextBox.Name = "interval_TextBox";
            this.interval_TextBox.Size = new System.Drawing.Size(55, 23);
            this.interval_TextBox.TabIndex = 4;
            // 
            // counter_TextBox
            // 
            this.counter_TextBox.Location = new System.Drawing.Point(149, 77);
            this.counter_TextBox.Name = "counter_TextBox";
            this.counter_TextBox.ReadOnly = true;
            this.counter_TextBox.Size = new System.Drawing.Size(329, 23);
            this.counter_TextBox.TabIndex = 11;
            // 
            // instance_TextBox
            // 
            this.instance_TextBox.Location = new System.Drawing.Point(121, 50);
            this.instance_TextBox.Name = "instance_TextBox";
            this.instance_TextBox.ReadOnly = true;
            this.instance_TextBox.Size = new System.Drawing.Size(329, 23);
            this.instance_TextBox.TabIndex = 10;
            // 
            // category_TextBox
            // 
            this.category_TextBox.Location = new System.Drawing.Point(93, 25);
            this.category_TextBox.Name = "category_TextBox";
            this.category_TextBox.ReadOnly = true;
            this.category_TextBox.Size = new System.Drawing.Size(329, 23);
            this.category_TextBox.TabIndex = 9;
            // 
            // category_Label
            // 
            this.category_Label.AutoSize = true;
            this.category_Label.Location = new System.Drawing.Point(28, 28);
            this.category_Label.Name = "category_Label";
            this.category_Label.Size = new System.Drawing.Size(58, 15);
            this.category_Label.TabIndex = 0;
            this.category_Label.Text = "Category:";
            // 
            // instance_Label
            // 
            this.instance_Label.AutoSize = true;
            this.instance_Label.Location = new System.Drawing.Point(58, 53);
            this.instance_Label.Name = "instance_Label";
            this.instance_Label.Size = new System.Drawing.Size(54, 15);
            this.instance_Label.TabIndex = 1;
            this.instance_Label.Text = "Instance:";
            // 
            // counter_Label
            // 
            this.counter_Label.AutoSize = true;
            this.counter_Label.Location = new System.Drawing.Point(89, 80);
            this.counter_Label.Name = "counter_Label";
            this.counter_Label.Size = new System.Drawing.Size(53, 15);
            this.counter_Label.TabIndex = 2;
            this.counter_Label.Text = "Counter:";
            // 
            // interval_Label
            // 
            this.interval_Label.AutoSize = true;
            this.interval_Label.Location = new System.Drawing.Point(94, 110);
            this.interval_Label.Name = "interval_Label";
            this.interval_Label.Size = new System.Drawing.Size(49, 15);
            this.interval_Label.TabIndex = 3;
            this.interval_Label.Text = "Interval:";
            // 
            // radSeparator1
            // 
            this.radSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radSeparator1.Location = new System.Drawing.Point(18, 41);
            this.radSeparator1.Name = "radSeparator1";
            this.radSeparator1.Size = new System.Drawing.Size(548, 10);
            this.radSeparator1.TabIndex = 59;
            this.radSeparator1.Text = "radSeparator1";
            // 
            // PerfMonCollectorCounterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radSeparator1);
            this.Controls.Add(this.selected_GroupBox);
            this.Controls.Add(this.hostNameDisplay_Label);
            this.Controls.Add(this.available_TreeView);
            this.Controls.Add(this.title_Label);
            this.Controls.Add(this.available_Label);
            this.Controls.Add(this.hostName_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PerfMonCollectorCounterControl";
            this.Size = new System.Drawing.Size(579, 427);
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).EndInit();
            this.selected_GroupBox.ResumeLayout(false);
            this.selected_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radSeparator1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title_Label;
        private System.Windows.Forms.Label available_Label;
        private System.Windows.Forms.TreeView available_TreeView;
        private System.Windows.Forms.Label hostName_Label;
        private System.Windows.Forms.Label hostNameDisplay_Label;
        private System.Windows.Forms.GroupBox selected_GroupBox;
        private System.Windows.Forms.TextBox interval_TextBox;
        private System.Windows.Forms.Label interval_Label;
        private System.Windows.Forms.Label counter_Label;
        private System.Windows.Forms.Label instance_Label;
        private System.Windows.Forms.Label category_Label;
        private System.Windows.Forms.TextBox category_TextBox;
        private System.Windows.Forms.TextBox instance_TextBox;
        private System.Windows.Forms.TextBox counter_TextBox;
        private System.Windows.Forms.Label mmss_Label;
        private System.Windows.Forms.CheckBox collect_CheckBox;
        private Telerik.WinControls.UI.RadSeparator radSeparator1;
    }
}
