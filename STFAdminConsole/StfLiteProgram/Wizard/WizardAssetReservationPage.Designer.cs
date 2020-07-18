namespace HP.SolutionTest.WorkBench
{
    partial class WizardAssetReservationPage
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
            Telerik.WinControls.UI.GridViewImageColumn gridViewImageColumn1 = new Telerik.WinControls.UI.GridViewImageColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardAssetReservationPage));
            this.assetReservation_GridView = new Telerik.WinControls.UI.RadGridView();
            this.assetReservationRefresh_Button = new System.Windows.Forms.Button();
            this.assetReservationKey_Label = new System.Windows.Forms.Label();
            this.header_Label = new System.Windows.Forms.Label();
            this.availabilityIcons_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.assetReservationKey_ComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.assetReservation_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetReservation_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // assetReservation_GridView
            // 
            this.assetReservation_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetReservation_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.assetReservation_GridView.Location = new System.Drawing.Point(15, 87);
            // 
            // 
            // 
            this.assetReservation_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewImageColumn1.AllowResize = false;
            gridViewImageColumn1.AllowSort = false;
            gridViewImageColumn1.FieldName = "Icon";
            gridViewImageColumn1.HeaderText = "";
            gridViewImageColumn1.MaxWidth = 40;
            gridViewImageColumn1.MinWidth = 40;
            gridViewImageColumn1.Name = "icon_GridViewColumn";
            gridViewImageColumn1.Width = 40;
            gridViewTextBoxColumn1.FieldName = "AssetId";
            gridViewTextBoxColumn1.HeaderText = "Asset ID";
            gridViewTextBoxColumn1.Name = "assetId_GridViewColumn";
            gridViewTextBoxColumn1.Width = 204;
            gridViewTextBoxColumn2.FieldName = "Availability";
            gridViewTextBoxColumn2.HeaderText = "Availability";
            gridViewTextBoxColumn2.Name = "availability_GridViewColumn";
            gridViewTextBoxColumn2.Width = 258;
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Description";
            gridViewTextBoxColumn3.Name = "description_GridViewColumn";
            gridViewTextBoxColumn3.Width = 269;
            this.assetReservation_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewImageColumn1,
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.assetReservation_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.assetReservation_GridView.Name = "assetReservation_GridView";
            // 
            // 
            // 
            this.assetReservation_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(15, 87, 240, 150);
            this.assetReservation_GridView.Size = new System.Drawing.Size(789, 286);
            this.assetReservation_GridView.TabIndex = 9;
            this.assetReservation_GridView.Text = "radGridView1";
            // 
            // assetReservationRefresh_Button
            // 
            this.assetReservationRefresh_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assetReservationRefresh_Button.Location = new System.Drawing.Point(725, 43);
            this.assetReservationRefresh_Button.Name = "assetReservationRefresh_Button";
            this.assetReservationRefresh_Button.Size = new System.Drawing.Size(79, 28);
            this.assetReservationRefresh_Button.TabIndex = 13;
            this.assetReservationRefresh_Button.Text = "Refresh";
            this.assetReservationRefresh_Button.UseVisualStyleBackColor = true;
            this.assetReservationRefresh_Button.Click += new System.EventHandler(this.reserve_Button_Click);
            // 
            // assetReservationKey_Label
            // 
            this.assetReservationKey_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assetReservationKey_Label.Location = new System.Drawing.Point(492, 12);
            this.assetReservationKey_Label.Name = "assetReservationKey_Label";
            this.assetReservationKey_Label.Size = new System.Drawing.Size(117, 20);
            this.assetReservationKey_Label.TabIndex = 11;
            this.assetReservationKey_Label.Text = "Reservation Key";
            this.assetReservationKey_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // header_Label
            // 
            this.header_Label.Location = new System.Drawing.Point(12, 12);
            this.header_Label.Name = "header_Label";
            this.header_Label.Size = new System.Drawing.Size(481, 74);
            this.header_Label.TabIndex = 14;
            this.header_Label.Text = "Checks Asset Reservations...";
            // 
            // availabilityIcons_ImageList
            // 
            this.availabilityIcons_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("availabilityIcons_ImageList.ImageStream")));
            this.availabilityIcons_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.availabilityIcons_ImageList.Images.SetKeyName(0, "Unknown");
            this.availabilityIcons_ImageList.Images.SetKeyName(1, "Available");
            this.availabilityIcons_ImageList.Images.SetKeyName(2, "NotAvailable");
            this.availabilityIcons_ImageList.Images.SetKeyName(3, "PartiallyAvailable");
            this.availabilityIcons_ImageList.Images.SetKeyName(4, "NotInDatabase");
            // 
            // assetReservationKey_ComboBox
            // 
            this.assetReservationKey_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assetReservationKey_ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.assetReservationKey_ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.assetReservationKey_ComboBox.FormattingEnabled = true;
            this.assetReservationKey_ComboBox.Location = new System.Drawing.Point(615, 9);
            this.assetReservationKey_ComboBox.Name = "assetReservationKey_ComboBox";
            this.assetReservationKey_ComboBox.Size = new System.Drawing.Size(189, 23);
            this.assetReservationKey_ComboBox.TabIndex = 15;
            this.assetReservationKey_ComboBox.DropDownClosed += new System.EventHandler(this.assetReservationKey_ComboBox_DropDownClosed);
            this.assetReservationKey_ComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.assetReservationKey_ComboBox_KeyUp);
            // 
            // WizardAssetReservationPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.assetReservationKey_Label);
            this.Controls.Add(this.assetReservationKey_ComboBox);
            this.Controls.Add(this.header_Label);
            this.Controls.Add(this.assetReservationRefresh_Button);
            this.Controls.Add(this.assetReservation_GridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardAssetReservationPage";
            this.Size = new System.Drawing.Size(813, 385);
            ((System.ComponentModel.ISupportInitialize)(this.assetReservation_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assetReservation_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView assetReservation_GridView;
        private System.Windows.Forms.Button assetReservationRefresh_Button;
        private System.Windows.Forms.Label assetReservationKey_Label;
        private System.Windows.Forms.Label header_Label;
        private System.Windows.Forms.ImageList availabilityIcons_ImageList;
        private System.Windows.Forms.ComboBox assetReservationKey_ComboBox;
    }
}
