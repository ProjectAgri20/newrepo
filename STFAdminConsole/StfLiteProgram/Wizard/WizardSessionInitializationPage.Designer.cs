using HP.ScalableTest.Framework.Dispatcher;

namespace HP.SolutionTest.WorkBench
{
    partial class WizardSessionInitializationPage
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
                // Unsubscribe from events
                SessionClient.Instance.SessionStateReceived -= SessionStateReceived;
                SessionClient.Instance.SessionStartupTransitionReceived -= SessionStartupTransitionReceived;
                SessionClient.Instance.SessionMapElementReceived -= SessionMapElementReceived;

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
            Telerik.WinControls.UI.GridViewImageColumn gridViewImageColumn1 = new Telerik.WinControls.UI.GridViewImageColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardSessionInitializationPage));
            this.retry_Button = new System.Windows.Forms.Button();
            this.initializationStatus_Label = new System.Windows.Forms.Label();
            this.assetStatus_GridView = new Telerik.WinControls.UI.RadGridView();
            this.availabilityIcons_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // retry_Button
            // 
            this.retry_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.retry_Button.Location = new System.Drawing.Point(492, 11);
            this.retry_Button.Name = "retry_Button";
            this.retry_Button.Size = new System.Drawing.Size(75, 28);
            this.retry_Button.TabIndex = 8;
            this.retry_Button.Text = "Retry";
            this.retry_Button.UseVisualStyleBackColor = true;
            this.retry_Button.Visible = false;
            this.retry_Button.Click += new System.EventHandler(this.retry_Button_Click);
            // 
            // initializationStatus_Label
            // 
            this.initializationStatus_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.initializationStatus_Label.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.initializationStatus_Label.Location = new System.Drawing.Point(13, 34);
            this.initializationStatus_Label.Name = "initializationStatus_Label";
            this.initializationStatus_Label.Size = new System.Drawing.Size(554, 35);
            this.initializationStatus_Label.TabIndex = 7;
            this.initializationStatus_Label.Text = "Checking resources...";
            // 
            // assetStatus_GridView
            // 
            this.assetStatus_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetStatus_GridView.Location = new System.Drawing.Point(13, 72);
            // 
            // assetStatus_GridView
            // 
            gridViewImageColumn1.FieldName = "Icon";
            gridViewImageColumn1.HeaderText = "";
            gridViewImageColumn1.MaxWidth = 40;
            gridViewImageColumn1.MinWidth = 40;
            gridViewImageColumn1.Name = "icon_GridViewColumn";
            gridViewImageColumn1.Width = 40;
            gridViewTextBoxColumn1.FieldName = "Name";
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "name_GridViewColumn";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 150;
            gridViewTextBoxColumn2.FieldName = "ElementType";
            gridViewTextBoxColumn2.HeaderText = "Type";
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "type_GridViewColumn";
            gridViewTextBoxColumn2.Width = 125;
            gridViewTextBoxColumn3.FieldName = "Details";
            gridViewTextBoxColumn3.HeaderText = "Details";
            gridViewTextBoxColumn3.Name = "details_GridViewColumn";
            gridViewTextBoxColumn3.Width = 465;
            this.assetStatus_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewImageColumn1,
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.assetStatus_GridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.assetStatus_GridView.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            sortDescriptor1.PropertyName = "name_GridViewColumn";
            this.assetStatus_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.assetStatus_GridView.Name = "assetStatus_GridView";
            this.assetStatus_GridView.Size = new System.Drawing.Size(554, 325);
            this.assetStatus_GridView.TabIndex = 6;
            this.assetStatus_GridView.Text = "radGridView1";
            this.assetStatus_GridView.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.assetStatus_GridView_CellFormatting);
            // 
            // availabilityIcons_ImageList
            // 
            this.availabilityIcons_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("availabilityIcons_ImageList.ImageStream")));
            this.availabilityIcons_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.availabilityIcons_ImageList.Images.SetKeyName(0, "Unknown");
            this.availabilityIcons_ImageList.Images.SetKeyName(1, "Unavailable");
            this.availabilityIcons_ImageList.Images.SetKeyName(2, "Available");
            this.availabilityIcons_ImageList.Images.SetKeyName(3, "Warning");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Please wait while the system checks all assets for availability.";
            // 
            // WizardSessionInitializationPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.retry_Button);
            this.Controls.Add(this.initializationStatus_Label);
            this.Controls.Add(this.assetStatus_GridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardSessionInitializationPage";
            this.Size = new System.Drawing.Size(580, 410);
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button retry_Button;
        private System.Windows.Forms.Label initializationStatus_Label;
        private Telerik.WinControls.UI.RadGridView assetStatus_GridView;
        private System.Windows.Forms.ImageList availabilityIcons_ImageList;
        private System.Windows.Forms.Label label1;
    }
}
