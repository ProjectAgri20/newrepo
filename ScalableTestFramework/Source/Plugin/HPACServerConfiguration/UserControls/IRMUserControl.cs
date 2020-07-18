using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// User control that allows users to perform Intelligent Right Managment and AD Authenticator Operations.
    /// </summary>
    internal partial class IRMUserControl : UserControl
    {
        private IrmOperation selectedOperation;
        private HpacAuthenticationMode authenticationMode;
        private HpacDataStorage datastorageMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="IRMUserControl"/> class.
        /// </summary>
        public IRMUserControl()
        {
            InitializeComponent();
        }

        private void irmTabs_CheckedChanged(object sender, EventArgs e)
        {

            if (general_RadioButton.Checked)
            {
                SetAuthenticationMode();
                ldapServerPassword_Label.Visible = false;
                ldapServerPassword_TextBox.Visible = false;
                ldapServerUsername_Label.Visible = false;
                ldapServerUsername_TextBox.Visible = false;
                ldapServer_Label.Visible = false;
                ldapServer_textBox.Visible = false;
                codeAttribute_Label.Visible = false;
                cardAttribute_Label.Visible = false;
                code_TextBox.Visible = false;
                card_TextBox.Visible = false;
                dataStorage_GroupBox.Visible = true;
                deviceAuthenticationMethod_GroupBox.Visible = true;
                display_Label.Visible = false;
                selectedOperation = IrmOperation.GeneralSettings;
            }
            else if (ldap_RadioButton.Checked)
            {
                SetDataStorage();
                ldapServerPassword_Label.Visible = true;
                ldapServerPassword_TextBox.Visible = true;
                ldapServerUsername_Label.Visible = true;
                ldapServerUsername_TextBox.Visible = true;
                ldapServer_Label.Visible = true;
                ldapServer_textBox.Visible = true;
                dataStorage_GroupBox.Visible = false;
                deviceAuthenticationMethod_GroupBox.Visible = false;
                codeAttribute_Label.Visible = false;
                cardAttribute_Label.Visible = false;
                code_TextBox.Visible = false;
                card_TextBox.Visible = false;
                display_Label.Visible = false;
                selectedOperation = IrmOperation.LDAPServerConfigure;
            }
            else if (cardsCodes_RadioButton.Checked)
            {
                ldapServerPassword_Label.Visible = false;
                ldapServerPassword_TextBox.Visible = false;
                ldapServerUsername_Label.Visible = false;
                ldapServerUsername_TextBox.Visible = false;
                ldapServer_Label.Visible = false;
                ldapServer_textBox.Visible = false;
                dataStorage_GroupBox.Visible = false;
                deviceAuthenticationMethod_GroupBox.Visible = false;
                if (ldapServer_RadioButton.Checked)
                {
                    display_Label.Visible = false;
                    if (cardOnly_RadioButton.Checked)
                    {
                        card_TextBox.Visible = true;
                        cardAttribute_Label.Visible = true;
                        code_TextBox.Visible = false;
                        codeAttribute_Label.Visible = false;
                    }
                    else if (codeOnly_RadioButton.Checked)
                    {
                        card_TextBox.Visible = false;
                        cardAttribute_Label.Visible = false;
                        code_TextBox.Visible = true;
                        codeAttribute_Label.Visible = true;
                    }
                    else
                    {
                        card_TextBox.Visible = true;
                        cardAttribute_Label.Visible = true;
                        code_TextBox.Visible = true;
                        codeAttribute_Label.Visible = true;
                    }
                }
                else
                {
                    display_Label.Visible = true;
                }
                selectedOperation = IrmOperation.CodeandorCardAttribute;
            }
        }

        private void SetAuthenticationMode()
        {
            if (cardOnly_RadioButton.Checked)
            {
                authenticationMode = HpacAuthenticationMode.Card;
            }
            else if (codeOnly_RadioButton.Checked)
            {
                authenticationMode = HpacAuthenticationMode.Code;
            }
            else if (cardAndCode_RadioButton.Checked)
            {
                authenticationMode = HpacAuthenticationMode.CodeAndCard;
            }
            else if (cardOrCode_RadioButton.Checked)
            {
                authenticationMode = HpacAuthenticationMode.CodeOrCard;
            }
        }

        private void SetDataStorage()
        {
            if (ldapServer_RadioButton.Checked)
            {
                datastorageMethod = HpacDataStorage.LDAP;
            }
            else
            {
                datastorageMethod = HpacDataStorage.Database;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="IRMTabData" /> instance containing the
        /// IRM tab data from this control.
        /// </summary>
        /// <returns>The IRM data.</returns>
        public IRMTabData GetConfigurationData()
        {
            IRMTabData irmdata = new IRMTabData();
            irmdata.IrmOperation = selectedOperation;
            irmdata.AuthenticationMode = authenticationMode;
            irmdata.DataStorage = datastorageMethod;
            irmdata.IRMUserCardNumber = card_TextBox.Text;
            irmdata.IRMUserCodeNumber = code_TextBox.Text;
            irmdata.ADUserCardNumber = cardNumber_TextBox.Text;
            irmdata.ADUserCodeNumber = codeNumber_TextBox.Text;
            irmdata.LDAPServer = ldapServer_textBox.Text;
            irmdata.LDAPServerPassword = ldapServerPassword_TextBox.Text;
            irmdata.LDAPServerUserName = ldapServerUsername_TextBox.Text;
            irmdata.Username = username_TextBox.Text;

            return irmdata;
        }

        private void irmTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (irmTabControl.SelectedTab == irmTabControl.TabPages["adUserEditTabPage"])
            {
                selectedOperation = IrmOperation.ADUserEditor;
            }
        }

        /// <summary>
        /// Configures the controls per the IRM data either derived from initialization or the saved meta data.
        /// </summary>
        public void LoadConfiguration(IRMTabData irmdata)
        {
            if (irmdata.IrmOperation == IrmOperation.ADUserEditor)
            {
                irmTabControl.SelectedIndex = 1;
            }
            else
            {
                irmTabControl.SelectedIndex = 0;
            }
            switch (irmdata.IrmOperation)
            {
                case IrmOperation.CodeandorCardAttribute:
                    cardsCodes_RadioButton.Checked = true;
                    break;
                case IrmOperation.GeneralSettings:
                    general_RadioButton.Checked = true;
                    break;
                case IrmOperation.LDAPServerConfigure:
                    ldap_RadioButton.Checked = true;
                    break;
                default:
                    break;
            }

            switch (irmdata.AuthenticationMode)
            {
                case HpacAuthenticationMode.Card:
                    cardOnly_RadioButton.Checked = true;
                    break;
                case HpacAuthenticationMode.Code:
                    codeOnly_RadioButton.Checked = true;
                    break;
                case HpacAuthenticationMode.CodeAndCard:
                    cardAndCode_RadioButton.Checked = true;
                    break;
                case HpacAuthenticationMode.CodeOrCard:
                    cardOrCode_RadioButton.Checked = true;
                    break;
                default:
                    break;
            }

            switch (irmdata.DataStorage)
            {
                case HpacDataStorage.Database:
                    database_RadioButton.Checked = true;
                    break;
                case HpacDataStorage.LDAP:
                    ldapServer_RadioButton.Checked = true;
                    break;
                default:
                    break;
            }

            card_TextBox.Text = irmdata.IRMUserCardNumber;
            code_TextBox.Text = irmdata.IRMUserCodeNumber;
            cardNumber_TextBox.Text = irmdata.ADUserCardNumber;
            codeNumber_TextBox.Text = irmdata.ADUserCodeNumber;
            ldapServer_textBox.Text = irmdata.LDAPServer;
            ldapServerPassword_TextBox.Text = irmdata.LDAPServerPassword;
            ldapServerUsername_TextBox.Text = irmdata.LDAPServerUserName;
            username_TextBox.Text = irmdata.Username;

        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult AddValidaton()
        {
            if (irmTabControl.SelectedTab == irmTabControl.TabPages["adUserEditTabPage"])
            {
                fieldValidator.RequireValue(username_TextBox, username_Label);
                fieldValidator.RequireValue(cardNumber_TextBox, cardNumber_Label);
                fieldValidator.RequireValue(codeNumber_TextBox, codeNumber_Label);

                fieldValidator.Remove(card_TextBox);
                fieldValidator.Remove(code_TextBox);
                fieldValidator.Remove(ldapServerPassword_TextBox);
                fieldValidator.Remove(ldapServerUsername_TextBox);
                fieldValidator.Remove(ldapServer_textBox);
            }
            else
            {
                if (ldap_RadioButton.Checked)
                {
                    fieldValidator.RequireValue(ldapServerPassword_TextBox, ldapServerPassword_Label);
                    fieldValidator.RequireValue(ldapServerUsername_TextBox, ldapServerUsername_Label);
                    fieldValidator.RequireValue(ldapServer_textBox, ldapServer_Label);
                }
                else
                {
                    fieldValidator.Remove(code_TextBox);
                    fieldValidator.Remove(card_TextBox);

                    if (cardsCodes_RadioButton.Checked)
                    {
                        if (ldapServer_RadioButton.Checked)
                        {
                            if (cardOnly_RadioButton.Checked)
                            {
                                fieldValidator.RequireValue(card_TextBox, cardAttribute_Label);
                            }
                            else if (codeOnly_RadioButton.Checked)
                            {
                                fieldValidator.RequireValue(code_TextBox, codeAttribute_Label);
                            }
                            else
                            {
                                fieldValidator.RequireValue(code_TextBox, codeAttribute_Label);
                                fieldValidator.RequireValue(card_TextBox, cardAttribute_Label);
                            }
                        }
                    }
                    fieldValidator.Remove(ldapServerPassword_TextBox);
                    fieldValidator.Remove(ldapServerUsername_TextBox);
                    fieldValidator.Remove(ldapServer_textBox);
                }

                fieldValidator.Remove(cardNumber_TextBox);
                fieldValidator.Remove(codeNumber_TextBox);
                fieldValidator.Remove(username_TextBox);
            }

            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Removes the Validation for the controls.
        /// </summary>
        public void RemoveValidation()
        {
            fieldValidator.Remove(cardNumber_TextBox);
            fieldValidator.Remove(codeNumber_TextBox);
            fieldValidator.Remove(username_TextBox);
            fieldValidator.Remove(card_TextBox);
            fieldValidator.Remove(code_TextBox);
            fieldValidator.Remove(ldapServerPassword_TextBox);
            fieldValidator.Remove(ldapServerUsername_TextBox);
            fieldValidator.Remove(ldapServer_textBox);
        }
    }
}
