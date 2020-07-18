using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    public partial class PrinterDetails : UserControl
    {
        /// <summary>
        /// Occurs when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        Printer.PrinterFamilies _printerFamily;
        InterfaceMode _printerInterface = InterfaceMode.EmbeddedWired;
        bool _hidePrimaryInterfaceAddress;
        bool _hideSecondaryInterfaceAddress;
        bool _hideWirelessInterfaceAddress;
        bool _hideWirelessMacAddress;
        bool _hideSecondaryInterfacePortNumber;
        bool _hidePrimaryInterfacePortNumber;
        bool _hideWirelessInterfacePortNumber;
        bool _hideExecuteOnInterface;
        private const string WirelessMacAddress = "Wireless Mac Address:";
        private const string WiredMacAddress = "Wired Mac Address:";
        #region Constructor

        public PrinterDetails()
        {
            InitializeComponent();

            secondaryInterfacePortNo_NumericUpDown.Enabled = !HideSecondaryInterfacePortNumber;
            wirelessInterfacePortNo_NumericUpDown.Enabled = !HideWirelessInterfacePortNumber;
            secondaryInterfaceAddress_IpAddressControl.Enabled = !HideSecondaryInterfaceAddress;
            wirelessAddress_IpAddressControl.Enabled = !HideWirelessInterfaceAddress;
            wirelessMacAddress_TextBox.Enabled = HideMacAddress;
            executeOn_GroupBox.Enabled = !HideExecuteOnInterface;

            if (_printerInterface == InterfaceMode.EmbeddedWired)
            {
                wirelessMacAddress_Label.Text = WirelessMacAddress;
            }
            else if (_printerInterface == InterfaceMode.Wireless)
            {
                wirelessMacAddress_Label.Text = WiredMacAddress;
            }

            PropertyChanged += PrinterDetails_PropertyChanged;

            printerDetails_FieldValidator.RequireCustom(primaryAddress_IPAddressControl, primaryAddress_IPAddressControl.IsValidIPAddress, "Enter Valid {0}.".FormatWith(primaryAddress_IPAddressControl.Tag));
            printerDetails_FieldValidator.RequireCustom(secondaryInterfaceAddress_IpAddressControl, () => (!secondaryInterfaceAddress_IpAddressControl.Enabled || secondaryInterfaceAddress_IpAddressControl.IsValidIPAddress()), "Enter Valid IP Address");
            printerDetails_FieldValidator.RequireCustom(wirelessAddress_IpAddressControl, () => (!wirelessAddress_IpAddressControl.Enabled || secondaryInterfaceAddress_IpAddressControl.IsValidIPAddress()), "Enter Valid IP Address");
            printerDetails_FieldValidator.RequireCustom(primaryInterfacePortNo_NumericUpDown, () => primaryInterfacePortNo_NumericUpDown.Value > 0, "Value should be greater than 0.");
            printerDetails_FieldValidator.RequireCustom(secondaryInterfacePortNo_NumericUpDown, () => (!secondaryInterfacePortNo_NumericUpDown.Enabled || secondaryInterfacePortNo_NumericUpDown.Value > 0), "Value should be greater than 0.");
            printerDetails_FieldValidator.RequireCustom(wirelessInterfacePortNo_NumericUpDown, () => (!wirelessInterfacePortNo_NumericUpDown.Enabled || wirelessInterfacePortNo_NumericUpDown.Value > 0), "Value should be greater than 0.");
            printerDetails_FieldValidator.RequireCustom(wirelessMacAddress_TextBox, () => !wirelessMacAddress_TextBox.Enabled || wirelessMacAddress_TextBox.Text.Length == 12, "Enter a valid mac address with 12 characters.");
        }

        private void PrinterDetails_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!DesignMode)
            {
                if (PrinterFamily == Printer.PrinterFamilies.VEP)
                {
                    singleInterface_RadioButton.Enabled = true;
                    multipleInterface_RadioButton.Enabled = true;
                    primaryAddress_IPAddressControl.Enabled = !HidePrimaryInterfaceAddress;
                    secondaryInterfaceAddress_IpAddressControl.Enabled = !HideSecondaryInterfacePortNumber || (singleInterface_RadioButton.Checked);
                    secondaryInterfacePortNo_NumericUpDown.Enabled = !HideSecondaryInterfacePortNumber || (singleInterface_RadioButton.Checked);
                    wirelessAddress_IpAddressControl.Enabled = !HideWirelessInterfaceAddress || (singleInterface_RadioButton.Checked);
                    wirelessMacAddress_TextBox.Enabled = false;
                    executeOn_Label.Enabled = true;
                    executeOn_GroupBox.Enabled = true;
                    secondary_RadioButton.Enabled = true;
                    wirelessInterfacePortNo_NumericUpDown.Enabled = !HideWirelessInterfacePortNumber || (singleInterface_RadioButton.Checked);
                }
                else if (PrinterFamily == Printer.PrinterFamilies.TPS || PrinterFamily == Printer.PrinterFamilies.InkJet)
                {
                    singleInterface_RadioButton.Enabled = false;
                    multipleInterface_RadioButton.Enabled = false;
                    primaryAddress_IPAddressControl.Enabled = !HidePrimaryInterfaceAddress;
                    secondaryInterfaceAddress_IpAddressControl.Enabled = false;
                    secondaryInterfacePortNo_NumericUpDown.Enabled = false;
                    wirelessAddress_IpAddressControl.Enabled = !HideWirelessInterfaceAddress;
                    wirelessMacAddress_TextBox.Enabled = !HideMacAddress;
                    executeOn_Label.Enabled = false;
                    executeOn_GroupBox.Enabled = false;
                    wirelessInterfacePortNo_NumericUpDown.Enabled = false;
                    executeOn_Label.Enabled = true;
                    executeOn_GroupBox.Enabled = true;
                    secondary_RadioButton.Enabled = false;
                    PrinterInterfaceType = ProductType.None;
                }
                else
                {
                    singleInterface_RadioButton.Enabled = false;
                    multipleInterface_RadioButton.Enabled = false;
                    primaryAddress_IPAddressControl.Enabled = !HidePrimaryInterfaceAddress;
                    secondaryInterfaceAddress_IpAddressControl.Enabled = false;
                    secondaryInterfacePortNo_NumericUpDown.Enabled = false;
                    wirelessAddress_IpAddressControl.Enabled = !HideWirelessInterfaceAddress;
                    wirelessMacAddress_TextBox.Enabled = false;
                    executeOn_Label.Enabled = false;
                    executeOn_GroupBox.Enabled = false;
                    secondary_RadioButton.Enabled = false;
                    wirelessInterfacePortNo_NumericUpDown.Enabled = false;
                    PrinterInterfaceType = ProductType.None;
                }

                executeOn_GroupBox.Enabled = !_hideExecuteOnInterface;
                executeOn_Label.Enabled = !_hideExecuteOnInterface;
            }
        }

        #endregion

        #region Private Properties

        bool IsSingleInterface { get; set; }

        #endregion

        #region Public Properties

        public Printer.PrinterFamilies PrinterFamily
        {
            get
            {
                return _printerFamily;
            }
            set
            {
                _printerFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs(PrinterFamily.ToString()));
            }
        }

        public string PrimaryInterfaceAddress
        {
            get
            {
                return primaryAddress_IPAddressControl.IsValidIPAddress() ? primaryAddress_IPAddressControl.Text : string.Empty;
            }
            set
            {
                primaryAddress_IPAddressControl.Text = value;
                //OnPropertyChanged(new PropertyChangedEventArgs(PrimaryInterfaceAddress));
            }
        }

        [Description("Flag to hide primary interface address"), Category("Data")]
        public bool HidePrimaryInterfaceAddress
        {
            get
            {
                return _hidePrimaryInterfaceAddress;
            }
            set
            {
                _hidePrimaryInterfaceAddress = value;
                secondaryInterfaceAddress_IpAddressControl.Enabled = value;
            }
        }

        public string SecondaryInterfaceAddress
        {
            get
            {
                return secondaryInterfaceAddress_IpAddressControl.IsValidIPAddress() ? secondaryInterfaceAddress_IpAddressControl.Text : string.Empty;
            }
            set
            {
                secondaryInterfaceAddress_IpAddressControl.Text = value;
                //OnPropertyChanged(new PropertyChangedEventArgs(SecondaryInterfaceAddress));
            }
        }

        [Description("Flag to hide secondary interface address"), Category("Data")]
        public bool HideSecondaryInterfaceAddress
        {
            get
            {
                return _hideSecondaryInterfaceAddress;
            }
            set
            {
                _hideSecondaryInterfaceAddress = value;
                secondaryInterfaceAddress_IpAddressControl.Enabled = value;
            }
        }

        public string WirelessAddress
        {
            get
            {
                return wirelessAddress_IpAddressControl.IsValidIPAddress() ? wirelessAddress_IpAddressControl.Text : string.Empty;
            }
            set
            {
                wirelessAddress_IpAddressControl.Text = value;
            }
        }

        [Description("Flag to hide wireless interface address"), Category("Data")]
        public bool HideWirelessInterfaceAddress
        {
            get
            {
                return _hideWirelessInterfaceAddress;
            }
            set
            {
                _hideWirelessInterfaceAddress = value;
                wirelessAddress_IpAddressControl.Enabled = value;
            }
        }

        public string MacAddress
        {
            get
            {
                return wirelessMacAddress_TextBox.Text;
            }
            set
            {
                wirelessMacAddress_TextBox.Text = value;
            }
        }

        [Description("Flag to hide wireless mac addresss"), Category("Data")]
        public bool HideMacAddress
        {
            get
            {
                return _hideWirelessMacAddress;
            }
            set
            {
                _hideWirelessMacAddress = value;
                wirelessMacAddress_TextBox.Enabled = value;
            }
        }

        [Description("Flag to set the executing interface"), Category("Data")]
        public InterfaceMode PrinterInterface
        {
            get
            {
                if (!DesignMode)
                {
                    if (embedded_RadioButton.Checked)
                    {
                        _printerInterface = InterfaceMode.EmbeddedWired;
                    }
                    else if (secondary_RadioButton.Checked)
                    {
                        _printerInterface = InterfaceMode.ExternalWired;
                    }
                    else
                    {
                        _printerInterface = InterfaceMode.Wireless;
                    }
                }

                return _printerInterface;
            }
            set
            {
                _printerInterface = value;

                if (!DesignMode)
                {
                    switch (_printerInterface)
                    {
                        case InterfaceMode.EmbeddedWired:
                            embedded_RadioButton.Checked = true;
                            wirelessMacAddress_Label.Text = WirelessMacAddress;
                            break;
                        case InterfaceMode.ExternalWired:
                            secondary_RadioButton.Enabled = true;
                            break;
                        default:
                            wireless_RadioButton.Enabled = true;
                            wirelessMacAddress_Label.Text = WiredMacAddress;
                            break;
                    }


                    Update();
                }
            }
        }

        public int PrimaryInterfacePortNumber
        {
            get
            {
                return (int)primaryInterfacePortNo_NumericUpDown.Value;
            }
            set
            {
                primaryInterfacePortNo_NumericUpDown.Value = value;
                Update();
            }
        }

        public int SecondaryInterfacePortNumber
        {
            get
            {
                return (int)secondaryInterfacePortNo_NumericUpDown.Value;
            }
            set
            {
                secondaryInterfacePortNo_NumericUpDown.Value = value;
                Update();
            }
        }

        [Description("Flag to hide primary interface port number"), Category("Data")]
        public bool HidePrimaryInterfacePortNumber
        {
            get
            {
                return _hidePrimaryInterfacePortNumber;
            }
            set
            {
                _hidePrimaryInterfacePortNumber = value;
                primaryInterfacePortNo_NumericUpDown.Enabled = !value;
                Update();
            }
        }

        [Description("Flag to hide secondary interface port number"), Category("Data")]
        public bool HideSecondaryInterfacePortNumber
        {
            get
            {
                return _hideSecondaryInterfacePortNumber;
            }
            set
            {
                _hideSecondaryInterfacePortNumber = value;
                secondaryInterfacePortNo_NumericUpDown.Enabled = value;
            }
        }

        public int WirelessInterfacePortNumber
        {
            get
            {
                return (int)wirelessInterfacePortNo_NumericUpDown.Value;
            }
            set
            {
                secondaryInterfacePortNo_NumericUpDown.Value = value;
                //OnPropertyChanged(new PropertyChangedEventArgs(WirelessInterfacePortNumber.ToString()));
            }
        }

        [Description("Flag to hide wireless interface port number"), Category("Data")]
        public bool HideWirelessInterfacePortNumber
        {
            get
            {
                return _hideWirelessInterfacePortNumber;
            }
            set
            {
                _hideWirelessInterfacePortNumber = value;
                wirelessInterfacePortNo_NumericUpDown.Enabled = value;
            }
        }

        public ProductType PrinterInterfaceType
        {
            get
            {
                if (PrinterFamily == Printer.PrinterFamilies.VEP)
                {
                    return singleInterface_RadioButton.Checked ? ProductType.SingleInterface : ProductType.MultipleInterface;
                }
                return ProductType.None;
            }
            set
            {
                if (PrinterFamily == Printer.PrinterFamilies.VEP)
                {
                    if (value == ProductType.SingleInterface)
                    {
                        singleInterface_RadioButton.Checked = true;
                    }
                    else
                    {
                        multipleInterface_RadioButton.Checked = true;
                    }
                }

                //OnPropertyChanged(new PropertyChangedEventArgs(PrinterInterfaceType.ToString()));
            }
        }

        [Description("Flag to hide the execute on interface"), Category("Data")]
        public bool HideExecuteOnInterface
        {
            get { return _hideExecuteOnInterface; }
            set { _hideExecuteOnInterface = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validating the control elements
        /// </summary>
        /// <returns></returns>
        public ValidationResult ValidateControls()
        {
            bool status = printerDetails_FieldValidator.ValidateAll().Aggregate(true, (current, result) => current & result.Succeeded);

            TraceFactory.Logger.Info("Validation status: {0}".FormatWith(status));
            return new ValidationResult(status);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs on property changed
        /// </summary>
        /// <param name="e">PropertyChangedEventArgs</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void embedded_RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton current = (RadioButton)sender;

            if (current.Name == embedded_RadioButton.Name)
            {
                _printerInterface = InterfaceMode.EmbeddedWired;

                if (PrinterFamily == Printer.PrinterFamilies.TPS || PrinterFamily == Printer.PrinterFamilies.InkJet)
                {
                    wirelessMacAddress_Label.Text = WirelessMacAddress;
                }
            }
            else if (current.Name == secondary_RadioButton.Name)
            {
                _printerInterface = InterfaceMode.ExternalWired;
            }
            else
            {
                _printerInterface = InterfaceMode.Wireless;

                if (PrinterFamily == Printer.PrinterFamilies.TPS || PrinterFamily == Printer.PrinterFamilies.InkJet)
                {
                    wirelessMacAddress_Label.Text = WiredMacAddress;
                }
            }

            OnPropertyChanged(new PropertyChangedEventArgs(WirelessAddress));
        }

        private void multipleInterface_RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton current = (RadioButton)sender;

            if (current.Name == multipleInterface_RadioButton.Name)
            {
                secondaryInterfaceAddress_IpAddressControl.Enabled = false;
                wirelessAddress_IpAddressControl.Enabled = !HideWirelessInterfaceAddress;
                secondaryInterfacePortNo_NumericUpDown.Enabled = false;
                wirelessInterfacePortNo_NumericUpDown.Enabled = false;
            }
            else
            {
                secondaryInterfaceAddress_IpAddressControl.Enabled = !_hideSecondaryInterfaceAddress;
                wirelessAddress_IpAddressControl.Enabled = !_hideWirelessInterfaceAddress;
                secondaryInterfacePortNo_NumericUpDown.Enabled = !_hideSecondaryInterfacePortNumber;
                wirelessInterfacePortNo_NumericUpDown.Enabled = !_hideWirelessInterfacePortNumber;
            }
        }

        private void primaryAddress_IPAddressControl_TextChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(WirelessAddress));
        }

        private void wirelessMacAddress_TextBox_TextChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(WirelessAddress));
        }

        #endregion
    }
}
