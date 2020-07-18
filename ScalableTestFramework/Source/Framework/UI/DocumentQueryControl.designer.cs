
namespace HP.ScalableTest.Framework.UI
{
    internal partial class DocumentQueryControl
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
            this.preview_Button = new System.Windows.Forms.Button();
            this.property_Label = new System.Windows.Forms.Label();
            this.property_ListBox = new System.Windows.Forms.ListBox();
            this.operation_Label = new System.Windows.Forms.Label();
            this.operator_ListBox = new System.Windows.Forms.ListBox();
            this.addCriteria_Button = new System.Windows.Forms.Button();
            this.values_ListBox = new System.Windows.Forms.ListBox();
            this.value1_TextBox = new System.Windows.Forms.TextBox();
            this.value2_TextBox = new System.Windows.Forms.TextBox();
            this.and_Label = new System.Windows.Forms.Label();
            this.values_Label = new System.Windows.Forms.Label();
            this.criteria_ListBox = new System.Windows.Forms.ListBox();
            this.criteria_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // preview_Button
            // 
            this.preview_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.preview_Button.Location = new System.Drawing.Point(542, 131);
            this.preview_Button.Name = "preview_Button";
            this.preview_Button.Size = new System.Drawing.Size(87, 25);
            this.preview_Button.TabIndex = 9;
            this.preview_Button.Text = "Preview";
            this.preview_Button.UseVisualStyleBackColor = true;
            this.preview_Button.Click += new System.EventHandler(this.preview_Button_Click);
            // 
            // property_Label
            // 
            this.property_Label.AutoSize = true;
            this.property_Label.Location = new System.Drawing.Point(3, 0);
            this.property_Label.Name = "property_Label";
            this.property_Label.Size = new System.Drawing.Size(52, 15);
            this.property_Label.TabIndex = 0;
            this.property_Label.Text = "Property";
            // 
            // property_ListBox
            // 
            this.property_ListBox.DisplayMember = "Label";
            this.property_ListBox.FormattingEnabled = true;
            this.property_ListBox.ItemHeight = 15;
            this.property_ListBox.Location = new System.Drawing.Point(6, 18);
            this.property_ListBox.Name = "property_ListBox";
            this.property_ListBox.Size = new System.Drawing.Size(159, 109);
            this.property_ListBox.TabIndex = 1;
            this.property_ListBox.ValueMember = "Name";
            this.property_ListBox.SelectedValueChanged += new System.EventHandler(this.property_ListBox_SelectedValueChanged);
            // 
            // operation_Label
            // 
            this.operation_Label.AutoSize = true;
            this.operation_Label.Location = new System.Drawing.Point(168, 0);
            this.operation_Label.Name = "operation_Label";
            this.operation_Label.Size = new System.Drawing.Size(60, 15);
            this.operation_Label.TabIndex = 2;
            this.operation_Label.Text = "Operation";
            // 
            // operator_ListBox
            // 
            this.operator_ListBox.DisplayMember = "Label";
            this.operator_ListBox.FormattingEnabled = true;
            this.operator_ListBox.ItemHeight = 15;
            this.operator_ListBox.Location = new System.Drawing.Point(171, 18);
            this.operator_ListBox.Name = "operator_ListBox";
            this.operator_ListBox.Size = new System.Drawing.Size(159, 109);
            this.operator_ListBox.TabIndex = 3;
            this.operator_ListBox.ValueMember = "Name";
            this.operator_ListBox.SelectedValueChanged += new System.EventHandler(this.operator_ListBox_SelectedValueChanged);
            // 
            // addCriteria_Button
            // 
            this.addCriteria_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addCriteria_Button.Location = new System.Drawing.Point(635, 131);
            this.addCriteria_Button.Name = "addCriteria_Button";
            this.addCriteria_Button.Size = new System.Drawing.Size(87, 25);
            this.addCriteria_Button.TabIndex = 10;
            this.addCriteria_Button.Text = "Add Criteria";
            this.addCriteria_Button.UseVisualStyleBackColor = true;
            this.addCriteria_Button.Click += new System.EventHandler(this.addCriteria_Button_Click);
            // 
            // values_ListBox
            // 
            this.values_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.values_ListBox.DisplayMember = "Label";
            this.values_ListBox.FormattingEnabled = true;
            this.values_ListBox.ItemHeight = 15;
            this.values_ListBox.Location = new System.Drawing.Point(336, 18);
            this.values_ListBox.Name = "values_ListBox";
            this.values_ListBox.Size = new System.Drawing.Size(386, 109);
            this.values_ListBox.TabIndex = 8;
            this.values_ListBox.ValueMember = "Name";
            // 
            // value1_TextBox
            // 
            this.value1_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.value1_TextBox.Location = new System.Drawing.Point(336, 18);
            this.value1_TextBox.Name = "value1_TextBox";
            this.value1_TextBox.Size = new System.Drawing.Size(386, 23);
            this.value1_TextBox.TabIndex = 5;
            // 
            // value2_TextBox
            // 
            this.value2_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.value2_TextBox.Location = new System.Drawing.Point(336, 80);
            this.value2_TextBox.Name = "value2_TextBox";
            this.value2_TextBox.Size = new System.Drawing.Size(386, 23);
            this.value2_TextBox.TabIndex = 7;
            // 
            // and_Label
            // 
            this.and_Label.AutoSize = true;
            this.and_Label.Location = new System.Drawing.Point(336, 53);
            this.and_Label.Name = "and_Label";
            this.and_Label.Size = new System.Drawing.Size(27, 15);
            this.and_Label.TabIndex = 6;
            this.and_Label.Text = "and";
            // 
            // values_Label
            // 
            this.values_Label.AutoSize = true;
            this.values_Label.Location = new System.Drawing.Point(333, 0);
            this.values_Label.Name = "values_Label";
            this.values_Label.Size = new System.Drawing.Size(48, 15);
            this.values_Label.TabIndex = 4;
            this.values_Label.Text = "Value(s)";
            // 
            // criteria_ListBox
            // 
            this.criteria_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.criteria_ListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.criteria_ListBox.DisplayMember = "Label";
            this.criteria_ListBox.FormattingEnabled = true;
            this.criteria_ListBox.IntegralHeight = false;
            this.criteria_ListBox.ItemHeight = 15;
            this.criteria_ListBox.Location = new System.Drawing.Point(6, 162);
            this.criteria_ListBox.Name = "criteria_ListBox";
            this.criteria_ListBox.Size = new System.Drawing.Size(716, 112);
            this.criteria_ListBox.TabIndex = 12;
            this.criteria_ListBox.ValueMember = "Name";
            this.criteria_ListBox.DoubleClick += new System.EventHandler(this.criteria_ListBox_DoubleClick);
            // 
            // criteria_Label
            // 
            this.criteria_Label.AutoSize = true;
            this.criteria_Label.Location = new System.Drawing.Point(3, 144);
            this.criteria_Label.Name = "criteria_Label";
            this.criteria_Label.Size = new System.Drawing.Size(223, 15);
            this.criteria_Label.TabIndex = 11;
            this.criteria_Label.Text = "Current Criteria (Double-click to remove)";
            // 
            // DocumentQueryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.criteria_Label);
            this.Controls.Add(this.values_Label);
            this.Controls.Add(this.and_Label);
            this.Controls.Add(this.value2_TextBox);
            this.Controls.Add(this.value1_TextBox);
            this.Controls.Add(this.values_ListBox);
            this.Controls.Add(this.operator_ListBox);
            this.Controls.Add(this.operation_Label);
            this.Controls.Add(this.criteria_ListBox);
            this.Controls.Add(this.property_ListBox);
            this.Controls.Add(this.property_Label);
            this.Controls.Add(this.addCriteria_Button);
            this.Controls.Add(this.preview_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentQueryControl";
            this.Size = new System.Drawing.Size(725, 277);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button preview_Button;
        private System.Windows.Forms.Label property_Label;
        private System.Windows.Forms.ListBox property_ListBox;
        private System.Windows.Forms.Label operation_Label;
        private System.Windows.Forms.ListBox operator_ListBox;
        private System.Windows.Forms.Button addCriteria_Button;
        private System.Windows.Forms.ListBox values_ListBox;
        private System.Windows.Forms.TextBox value1_TextBox;
        private System.Windows.Forms.TextBox value2_TextBox;
        private System.Windows.Forms.Label and_Label;
        private System.Windows.Forms.Label values_Label;
        private System.Windows.Forms.ListBox criteria_ListBox;
        private System.Windows.Forms.Label criteria_Label;
    }
}