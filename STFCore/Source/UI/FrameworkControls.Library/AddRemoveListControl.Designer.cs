namespace HP.ScalableTest.UI
{
    partial class AddRemoveListControl
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
            this.sourceListBox = new System.Windows.Forms.ListBox();
            this.destinationListBox = new System.Windows.Forms.ListBox();
            this.singleAddButton = new System.Windows.Forms.Button();
            this.singleRemoveButton = new System.Windows.Forms.Button();
            this.allAddButton = new System.Windows.Forms.Button();
            this.allRemoveButton = new System.Windows.Forms.Button();
            this.source_Label = new System.Windows.Forms.Label();
            this.destination_Label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceListBox
            // 
            this.sourceListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceListBox.FormattingEnabled = true;
            this.sourceListBox.HorizontalScrollbar = true;
            this.sourceListBox.IntegralHeight = false;
            this.sourceListBox.ItemHeight = 16;
            this.sourceListBox.Location = new System.Drawing.Point(4, 24);
            this.sourceListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sourceListBox.Name = "sourceListBox";
            this.tableLayoutPanel1.SetRowSpan(this.sourceListBox, 6);
            this.sourceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.sourceListBox.Size = new System.Drawing.Size(242, 243);
            this.sourceListBox.Sorted = true;
            this.sourceListBox.TabIndex = 0;
            // 
            // destinationListBox
            // 
            this.destinationListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationListBox.FormattingEnabled = true;
            this.destinationListBox.HorizontalScrollbar = true;
            this.destinationListBox.IntegralHeight = false;
            this.destinationListBox.ItemHeight = 16;
            this.destinationListBox.Location = new System.Drawing.Point(334, 24);
            this.destinationListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.destinationListBox.Name = "destinationListBox";
            this.tableLayoutPanel1.SetRowSpan(this.destinationListBox, 6);
            this.destinationListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.destinationListBox.Size = new System.Drawing.Size(242, 243);
            this.destinationListBox.Sorted = true;
            this.destinationListBox.TabIndex = 1;
            // 
            // singleAddButton
            // 
            this.singleAddButton.Location = new System.Drawing.Point(254, 24);
            this.singleAddButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.singleAddButton.Name = "singleAddButton";
            this.singleAddButton.Size = new System.Drawing.Size(72, 26);
            this.singleAddButton.TabIndex = 2;
            this.singleAddButton.Text = ">";
            this.singleAddButton.UseVisualStyleBackColor = true;
            this.singleAddButton.Click += new System.EventHandler(this.SingleAddButtonClick);
            // 
            // singleRemoveButton
            // 
            this.singleRemoveButton.Location = new System.Drawing.Point(254, 58);
            this.singleRemoveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.singleRemoveButton.Name = "singleRemoveButton";
            this.singleRemoveButton.Size = new System.Drawing.Size(72, 26);
            this.singleRemoveButton.TabIndex = 3;
            this.singleRemoveButton.Text = "<";
            this.singleRemoveButton.UseVisualStyleBackColor = true;
            this.singleRemoveButton.Click += new System.EventHandler(this.SingleRemoveButtonClick);
            // 
            // allAddButton
            // 
            this.allAddButton.Location = new System.Drawing.Point(254, 126);
            this.allAddButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.allAddButton.Name = "allAddButton";
            this.allAddButton.Size = new System.Drawing.Size(72, 26);
            this.allAddButton.TabIndex = 4;
            this.allAddButton.Text = ">>>";
            this.allAddButton.UseVisualStyleBackColor = true;
            this.allAddButton.Click += new System.EventHandler(this.AllAddButtonClick);
            // 
            // allRemoveButton
            // 
            this.allRemoveButton.Location = new System.Drawing.Point(254, 160);
            this.allRemoveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.allRemoveButton.Name = "allRemoveButton";
            this.allRemoveButton.Size = new System.Drawing.Size(72, 26);
            this.allRemoveButton.TabIndex = 5;
            this.allRemoveButton.Text = "<<<";
            this.allRemoveButton.UseVisualStyleBackColor = true;
            this.allRemoveButton.Click += new System.EventHandler(this.AllRemoveButtonClick);
            // 
            // source_Label
            // 
            this.source_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.source_Label.AutoSize = true;
            this.source_Label.Location = new System.Drawing.Point(4, 3);
            this.source_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.source_Label.Name = "source_Label";
            this.source_Label.Size = new System.Drawing.Size(85, 17);
            this.source_Label.TabIndex = 6;
            this.source_Label.Text = "Source Side";
            // 
            // destination_Label
            // 
            this.destination_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.destination_Label.AutoSize = true;
            this.destination_Label.Location = new System.Drawing.Point(334, 3);
            this.destination_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.destination_Label.Name = "destination_Label";
            this.destination_Label.Size = new System.Drawing.Size(111, 17);
            this.destination_Label.TabIndex = 7;
            this.destination_Label.Text = "Destination Side";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.destination_Label, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.source_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.singleRemoveButton, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.singleAddButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.destinationListBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.sourceListBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.allRemoveButton, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.allAddButton, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 271);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // AddRemoveListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddRemoveListControl";
            this.Size = new System.Drawing.Size(580, 271);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox sourceListBox;
        private System.Windows.Forms.ListBox destinationListBox;
        private System.Windows.Forms.Button singleAddButton;
        private System.Windows.Forms.Button singleRemoveButton;
        private System.Windows.Forms.Button allAddButton;
        private System.Windows.Forms.Button allRemoveButton;
        private System.Windows.Forms.Label source_Label;
        private System.Windows.Forms.Label destination_Label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
