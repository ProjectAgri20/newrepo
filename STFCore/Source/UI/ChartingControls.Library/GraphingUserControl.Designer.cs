namespace HP.ScalableTest.UI.Charting
{
    partial class GraphingUserControl
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
            this.sessionId_Label = new System.Windows.Forms.Label();
            this.sessionId_ComboBox = new System.Windows.Forms.ComboBox();
            this.refresh_Button = new System.Windows.Forms.Button();
            this.graphs_TabControl = new System.Windows.Forms.TabControl();
            this.saveImage_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.options_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sessionId_Label
            // 
            this.sessionId_Label.AutoSize = true;
            this.sessionId_Label.Location = new System.Drawing.Point(13, 8);
            this.sessionId_Label.Name = "sessionId_Label";
            this.sessionId_Label.Size = new System.Drawing.Size(46, 15);
            this.sessionId_Label.TabIndex = 11;
            this.sessionId_Label.Text = "Session";
            // 
            // sessionId_ComboBox
            // 
            this.sessionId_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionId_ComboBox.DisplayMember = "Value";
            this.sessionId_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sessionId_ComboBox.DropDownWidth = 767;
            this.sessionId_ComboBox.Location = new System.Drawing.Point(15, 25);
            this.sessionId_ComboBox.Name = "sessionId_ComboBox";
            this.sessionId_ComboBox.Size = new System.Drawing.Size(545, 23);
            this.sessionId_ComboBox.TabIndex = 10;
            this.sessionId_ComboBox.ValueMember = "Key";
            this.sessionId_ComboBox.SelectionChangeCommitted += new System.EventHandler(this.sessionId_ComboBox_SelectionChangeCommitted);
            // 
            // refresh_Button
            // 
            this.refresh_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh_Button.Location = new System.Drawing.Point(569, 24);
            this.refresh_Button.Name = "refresh_Button";
            this.refresh_Button.Size = new System.Drawing.Size(115, 23);
            this.refresh_Button.TabIndex = 16;
            this.refresh_Button.Text = "Refresh";
            this.refresh_Button.UseVisualStyleBackColor = true;
            this.refresh_Button.Click += new System.EventHandler(this.refresh_Button_Click);
            // 
            // graphs_TabControl
            // 
            this.graphs_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphs_TabControl.Location = new System.Drawing.Point(3, 84);
            this.graphs_TabControl.Name = "graphs_TabControl";
            this.graphs_TabControl.SelectedIndex = 0;
            this.graphs_TabControl.Size = new System.Drawing.Size(894, 513);
            this.graphs_TabControl.TabIndex = 17;
            // 
            // saveImage_Button
            // 
            this.saveImage_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveImage_Button.Location = new System.Drawing.Point(690, 24);
            this.saveImage_Button.Name = "saveImage_Button";
            this.saveImage_Button.Size = new System.Drawing.Size(115, 23);
            this.saveImage_Button.TabIndex = 18;
            this.saveImage_Button.Text = "Save to Clipboard";
            this.saveImage_Button.UseVisualStyleBackColor = true;
            this.saveImage_Button.Click += new System.EventHandler(this.saveImage_Button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.graphs_TabControl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 600);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.options_Button);
            this.panel1.Controls.Add(this.sessionId_Label);
            this.panel1.Controls.Add(this.saveImage_Button);
            this.panel1.Controls.Add(this.sessionId_ComboBox);
            this.panel1.Controls.Add(this.refresh_Button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 75);
            this.panel1.TabIndex = 0;
            // 
            // options_Button
            // 
            this.options_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.options_Button.Location = new System.Drawing.Point(811, 24);
            this.options_Button.Name = "options_Button";
            this.options_Button.Size = new System.Drawing.Size(76, 23);
            this.options_Button.TabIndex = 19;
            this.options_Button.Text = "Filters";
            this.options_Button.UseVisualStyleBackColor = true;
            this.options_Button.Click += new System.EventHandler(this.options_Button_Click);
            // 
            // GraphingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphingUserControl";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.GraphingUserControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label sessionId_Label;
        private System.Windows.Forms.ComboBox sessionId_ComboBox;
        private System.Windows.Forms.Button refresh_Button;
        private System.Windows.Forms.TabControl graphs_TabControl;
        private System.Windows.Forms.Button saveImage_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button options_Button;
    }
}
