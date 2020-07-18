using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
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

                if (_scheduleStartForm != null)
                {
                    _scheduleStartForm.Dispose();
                }

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
            Telerik.WinControls.UI.GridViewImageColumn gridViewImageColumn3 = new Telerik.WinControls.UI.GridViewImageColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor3 = new Telerik.WinControls.Data.SortDescriptor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardSessionInitializationPage));
            this.retry_Button = new System.Windows.Forms.Button();
            this.initializationStatus_Label = new System.Windows.Forms.Label();
            this.assetStatus_GridView = new Telerik.WinControls.UI.RadGridView();
            this.availabilityIcons_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.delayedStartLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetStatus_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // retry_Button
            // 
            this.retry_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.retry_Button.Location = new System.Drawing.Point(521, 5);
            this.retry_Button.Name = "retry_Button";
            this.retry_Button.Size = new System.Drawing.Size(100, 32);
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
            this.initializationStatus_Label.Size = new System.Drawing.Size(608, 35);
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
            // 
            // 
            gridViewImageColumn3.FieldName = "Icon";
            gridViewImageColumn3.HeaderText = "";
            gridViewImageColumn3.MaxWidth = 40;
            gridViewImageColumn3.MinWidth = 40;
            gridViewImageColumn3.Name = "icon_GridViewColumn";
            gridViewImageColumn3.Width = 40;
            gridViewTextBoxColumn7.FieldName = "Name";
            gridViewTextBoxColumn7.HeaderText = "Name";
            gridViewTextBoxColumn7.MinWidth = 100;
            gridViewTextBoxColumn7.Name = "name_GridViewColumn";
            gridViewTextBoxColumn7.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn7.Width = 150;
            gridViewTextBoxColumn8.FieldName = "ElementType";
            gridViewTextBoxColumn8.HeaderText = "Type";
            gridViewTextBoxColumn8.MinWidth = 100;
            gridViewTextBoxColumn8.Name = "type_GridViewColumn";
            gridViewTextBoxColumn8.Width = 125;
            gridViewTextBoxColumn9.FieldName = "Details";
            gridViewTextBoxColumn9.HeaderText = "Details";
            gridViewTextBoxColumn9.Name = "details_GridViewColumn";
            gridViewTextBoxColumn9.Width = 465;
            this.assetStatus_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewImageColumn3,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9});
            this.assetStatus_GridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.assetStatus_GridView.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            sortDescriptor3.PropertyName = "name_GridViewColumn";
            this.assetStatus_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor3});
            this.assetStatus_GridView.Name = "assetStatus_GridView";
            this.assetStatus_GridView.Size = new System.Drawing.Size(608, 270);
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
            // delayedStartLink
            // 
            this.delayedStartLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delayedStartLink.AutoSize = true;
            this.delayedStartLink.Location = new System.Drawing.Point(9, 350);
            this.delayedStartLink.Name = "delayedStartLink";
            this.delayedStartLink.Size = new System.Drawing.Size(303, 20);
            this.delayedStartLink.TabIndex = 11;
            this.delayedStartLink.TabStop = true;
            this.delayedStartLink.Text = "Configure Delayed Session Start (DISABLED)";
            this.delayedStartLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.delayedStartLink_LinkClicked);
            // 
            // WizardSessionInitializationPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.delayedStartLink);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.retry_Button);
            this.Controls.Add(this.initializationStatus_Label);
            this.Controls.Add(this.assetStatus_GridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardSessionInitializationPage";
            this.Size = new System.Drawing.Size(634, 376);
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
        private System.Windows.Forms.LinkLabel delayedStartLink;
    }
}
