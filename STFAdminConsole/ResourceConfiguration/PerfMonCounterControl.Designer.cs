namespace HP.ScalableTest.LabConsole
{
    partial class PerfMonCounterControl
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.server_Label = new System.Windows.Forms.Label();
            this.category_Label = new System.Windows.Forms.Label();
            this.counter_Label = new System.Windows.Forms.Label();
            this.instance_Label = new System.Windows.Forms.Label();
            this.counter_ListBox = new System.Windows.Forms.ListBox();
            this.instance_ListBox = new System.Windows.Forms.ListBox();
            this.category_ListBox = new System.Windows.Forms.ListBox();
            this.server_ListBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).BeginInit();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.server_Label);
            this.groupBox.Controls.Add(this.category_Label);
            this.groupBox.Controls.Add(this.counter_Label);
            this.groupBox.Controls.Add(this.instance_Label);
            this.groupBox.Controls.Add(this.counter_ListBox);
            this.groupBox.Controls.Add(this.instance_ListBox);
            this.groupBox.Controls.Add(this.category_ListBox);
            this.groupBox.Controls.Add(this.server_ListBox);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(660, 163);
            this.groupBox.TabIndex = 12;
            this.groupBox.TabStop = false;
            // 
            // server_Label
            // 
            this.server_Label.AutoSize = true;
            this.server_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.server_Label.Location = new System.Drawing.Point(10, 0);
            this.server_Label.Name = "server_Label";
            this.server_Label.Size = new System.Drawing.Size(39, 15);
            this.server_Label.TabIndex = 12;
            this.server_Label.Text = "Server";
            // 
            // category_Label
            // 
            this.category_Label.AutoSize = true;
            this.category_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.category_Label.Location = new System.Drawing.Point(176, 0);
            this.category_Label.Name = "category_Label";
            this.category_Label.Size = new System.Drawing.Size(55, 15);
            this.category_Label.TabIndex = 13;
            this.category_Label.Text = "Category";
            // 
            // counter_Label
            // 
            this.counter_Label.AutoSize = true;
            this.counter_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.counter_Label.Location = new System.Drawing.Point(506, 0);
            this.counter_Label.Name = "counter_Label";
            this.counter_Label.Size = new System.Drawing.Size(50, 15);
            this.counter_Label.TabIndex = 15;
            this.counter_Label.Text = "Counter";
            // 
            // instance_Label
            // 
            this.instance_Label.AutoSize = true;
            this.instance_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instance_Label.Location = new System.Drawing.Point(341, 0);
            this.instance_Label.Name = "instance_Label";
            this.instance_Label.Size = new System.Drawing.Size(51, 15);
            this.instance_Label.TabIndex = 14;
            this.instance_Label.Text = "Instance";
            // 
            // counter_ListBox
            // 
            this.counter_ListBox.DisplayMember = "CounterName";
            this.counter_ListBox.FormattingEnabled = true;
            this.counter_ListBox.HorizontalScrollbar = true;
            this.counter_ListBox.Location = new System.Drawing.Point(502, 24);
            this.counter_ListBox.Name = "counter_ListBox";
            this.counter_ListBox.Size = new System.Drawing.Size(147, 134);
            this.counter_ListBox.TabIndex = 3;
            this.counter_ListBox.SelectedIndexChanged += new System.EventHandler(this.counter_ListBox_SelectedIndexChanged);
            // 
            // instance_ListBox
            // 
            this.instance_ListBox.FormattingEnabled = true;
            this.instance_ListBox.HorizontalScrollbar = true;
            this.instance_ListBox.Location = new System.Drawing.Point(338, 24);
            this.instance_ListBox.Name = "instance_ListBox";
            this.instance_ListBox.Size = new System.Drawing.Size(150, 134);
            this.instance_ListBox.TabIndex = 2;
            this.instance_ListBox.SelectedIndexChanged += new System.EventHandler(this.instance_ListBox_SelectedIndexChanged);
            // 
            // category_ListBox
            // 
            this.category_ListBox.FormattingEnabled = true;
            this.category_ListBox.HorizontalScrollbar = true;
            this.category_ListBox.Location = new System.Drawing.Point(174, 24);
            this.category_ListBox.Name = "category_ListBox";
            this.category_ListBox.Size = new System.Drawing.Size(150, 134);
            this.category_ListBox.TabIndex = 1;
            this.category_ListBox.SelectedIndexChanged += new System.EventHandler(this.category_ListBox_SelectedIndexChanged);
            // 
            // server_ListBox
            // 
            this.server_ListBox.FormattingEnabled = true;
            this.server_ListBox.HorizontalScrollbar = true;
            this.server_ListBox.Location = new System.Drawing.Point(10, 24);
            this.server_ListBox.Name = "server_ListBox";
            this.server_ListBox.Size = new System.Drawing.Size(150, 134);
            this.server_ListBox.TabIndex = 0;
            // 
            // PerfMonCounterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "PerfMonCounterControl";
            this.Size = new System.Drawing.Size(660, 163);
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label server_Label;
        private System.Windows.Forms.Label category_Label;
        private System.Windows.Forms.Label counter_Label;
        private System.Windows.Forms.Label instance_Label;
        private System.Windows.Forms.ListBox counter_ListBox;
        private System.Windows.Forms.ListBox instance_ListBox;
        private System.Windows.Forms.ListBox category_ListBox;
        private System.Windows.Forms.ListBox server_ListBox;
    }
}
