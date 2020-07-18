using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace HP.RDL.RdlHPMibTranslator
{
    public partial class FrmTranslator : Form
    {
        /*
          * hp OBJECT IDENTIFIER ::= { iso(1) org(3) dod(6) internet(1) private(4) enterprises(1) 11}
          * dm OBJECT IDENTIFIER ::= { hp nm(2) hpsystem(3) net-peripheral(9) netdm(4) 2}
          *
         */
        private const string HP_OID = "1.3.6.1.4.1.11";
        private const string DM_OID = "2.3.9.4.2";

        private const string FILTERS_MIB = "MIB Files (*.mib)|*.mib|txt files (*.txt)|*.txt|All Files (*.*)|*.*";
        private const string FILTERS_CSV = "CSV Files (*.csv)|*.csv|txt files (*.txt)|*.txt|All Files (*.*)|*.*";


        private ListMibEnt _listMibs = new ListMibEnt();
        private ListOidEnt _listOids = new ListOidEnt();
        private OidInfoList _listOidInfo = new OidInfoList();
        private bool _snmpVersion2 = true;

		private string _password = "!QAZ2wsx";

        public FrmTranslator()
        {
            InitializeComponent();
        }

        private void FrmTranslator_Shown(object sender, EventArgs e)
        {
            MnuOptSnmpVer1.IsChecked = false;
            MnuOptSnmpVer2.IsChecked = true;
        }
        private void MnuFileOpenClick(object sender, EventArgs e)
        {
            BuildNewOidsFromMib();
        }

        private void BuildNewOidsFromMib()
        {
            openDlg = new OpenFileDialog();
            openDlg.Filter = FILTERS_MIB;
            openDlg.InitialDirectory = @"C:\Users\andersod\Documents\IFaceTesting\HP MIB";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = openDlg.FileName;
                string ext = Path.GetExtension(filePath);
                if (ext.ToLower().Equals(".mib"))
                {
                    BuildMibTranslator(filePath);
                    BuildOidListing(filePath);
                    CheckIfOidsBuiltCorrectly();
                    DispayInGrid();
                }
            }
        }

        private void RMnuAddMibClick(object sender, EventArgs e)
        {
            openDlg = new OpenFileDialog();
            openDlg.Filter = FILTERS_MIB;
            openDlg.InitialDirectory = @"C:\Users\andersod\Documents\IFaceTesting\HP MIB";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = openDlg.FileName;
                string ext = Path.GetExtension(filePath);
                if (ext.ToLower().Equals(".mib"))
                {
                    BuildMibTranslator(filePath);
                    BuildOidListing(filePath);
                    CheckIfOidsBuiltCorrectly();
                    DispayInGrid();
                }
            }

        }

        private void MnuFileClearClick(object sender, EventArgs e)
        {
            _listMibs.Clear();
            _listOidInfo.Clear();
            _listOids.Clear();

            rgvOidInfo.Rows.Clear();
        }

        private void MnuFileOpenCsv(object sender, EventArgs e)
        {
            openDlg = new OpenFileDialog();
            openDlg.Filter = FILTERS_CSV;
            openDlg.InitialDirectory = @"D:\RDL\IFaceTestDon\CTSL\SystemTest\InterfaceTesting\MC1.0\SNMP";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = openDlg.FileName;
                string ext = Path.GetExtension(filePath);
                if (ext.ToLower().Equals(".csv"))
                {
                    BuildOidInfo(filePath);
                }

            }

        }

        private void MnuFileSave_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            saveDlg = new SaveFileDialog();
            saveDlg.Filter = FILTERS_CSV;
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = "FullOidListing.csv";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                if ((filePath = saveDlg.FileName) != string.Empty)
                {
                    WriteFile wf = new WriteFile(_listOids);
                    wf.SaveDirectory = Path.GetDirectoryName(saveDlg.FileName);
                    wf.FileName = Path.GetFileName(saveDlg.FileName);
                    wf.WriteOidGridData();
                    if (wf.IsError)
                    {
                        MessageBox.Show(wf.GetLastError);
                    }
                }
            }



        }
        private void AddToOidList()
        {
            ListOidEnt notFound = new ListOidEnt();

            foreach (OidInfo oInfo in _listOidInfo)
            {
                bool bFound = false;
                foreach (OidEnt oid in _listOids)
                {
                    if (oid.OidIndex.Contains(oInfo.OIDValue))
                    {
                        bFound = true;
                        // OID was not in MIB but provided from the HP Device
                        if (oid.OidName.Equals(string.Empty))
                        {
                            oid.OidName = oInfo.NameOid;
                            oid.OidString = oInfo.OIDValue;
                            oid.Usage = USAGES.eMcHpDevice;
                        }
                        else
                        {
                            switch (oid.Usage)
                            {
                                case USAGES.eMib: oid.Usage = USAGES.eMcMib;
                                    break;
                                case USAGES.eHpDeviceMib: oid.Usage = USAGES.eAll;
                                    break;
                            }
                        }
                    }
                }
                if (!bFound)
                {
                    OidEnt temp = new OidEnt();

                    temp.OidName = oInfo.NameOid;
                    temp.OidString = oInfo.OIDValue;
                    temp.OidIndex = oInfo.SupportedIdxStr();
                    temp.OidIndexValue = oInfo.ExpectedValue;
                    temp.Access = oInfo.Access();
                    temp.Syntax.Add(Enum<SNMP_DATATYPES>.Value(oInfo.DataType));
                    temp.Status = oInfo.Implementation;
                    temp.Usage = USAGES.eMc;

                    notFound.Add(temp);
                }
            }
            if (notFound.Count > 0)
            {
                foreach (OidEnt oid in notFound)
                {
                    _listOids.Add(oid);
                }
            }
        }

        private void DispayInGrid()
        {
            this.listOidEntBindingSource.DataSource = _listOids;
            this.listOidEntBindingSource.ResetBindings(true);
        }

        private void CheckIfOidsBuiltCorrectly()
        {
            ListOidEnt listNotFound = new ListOidEnt();

            foreach (OidEnt oidInfo in _listOids)
            {
                string oidValue = GetOidValue(oidInfo.Parent);

                if (oidValue.Contains(HP_OID))
                {
                    if (oidValue[0].Equals('.'))
                    {
                        oidValue = oidValue.Substring(1);
                    }
                }
                else
                {
                    oidValue = HP_OID + oidValue;
                }
                oidValue += "." + oidInfo.Value;
                // The parent values aren't building correctly
                if (!oidValue.Equals(oidInfo.OidString))
                {
                    OidEnt temp = new OidEnt(oidInfo);

                    temp.OidIndex = oidValue;
                    temp.OidIndexValue = string.Empty;
                    temp.Usage = USAGES.eUnknown;

                    listNotFound.Add(temp);
                }
            }
            if (listNotFound.Count > 0)
            {
                WriteFile wf = new WriteFile(listNotFound);
                wf.WritePartOInfoFile();
                MessageBox.Show(listNotFound.Count.ToString() + " building of parent values not matching stated MIB OID.");
            }
        }

        private string GetOidValue(string title)
        {
            string myValue = string.Empty;
            foreach (MibEnt mib in _listMibs)
            {
                if (mib.Title.Equals(title))
                {
                    myValue = GetOidValue(mib.Parent) + "." + mib.Value.ToString();
                    break;
                }
            }

            return myValue;
        }

        private void BuildOidListing(string filePath)
        {
            TranslateOidInfo toi = new TranslateOidInfo(filePath, _listMibs);
            toi.CreateOidListing();
            if (_listOids.Count == 0)
            {
                _listOids = toi.OidsListing;
            }
            else
            {
                AddNewOids(toi.OidsListing);
            }
        }

        private void AddNewOids(ListOidEnt listOidEnt)
        {
            int cnt = 0;
            foreach (OidEnt oid in listOidEnt)
            {
                if (!OidEntFound(oid))
                {
                    _listOids.Add(oid);
                    cnt++;
                }
            }
            MessageBox.Show("New Additions: " + cnt.ToString());
        }
        private bool OidEntFound(OidEnt oid)
        {
            bool bFound = false;

            foreach (OidEnt ent in _listOids)
            {
                if (oid.Equals(ent))
                {
                    bFound = true;
                    break;
                }
            }

            return bFound;
        }
        private void BuildMibTranslator(string filePath)
        {
            TranslateMibInfo tmi = new TranslateMibInfo(filePath);
            tmi.CreateMibListing(_listMibs);
        }
        private void BuildOidInfo(string filePath)
        {
            _listOidInfo.Clear();
            RdMngContCsv rmcc = new RdMngContCsv(filePath);
            if (rmcc.RetrieveOidInfo(_listOidInfo))
            {
                AddToOidList();
                DispayInGrid();
            }
        }
        private void BtnExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGetOidsClick(object sender, EventArgs e)
        {
            if (ValidIPAddress(txtIpAddr.Text) && !string.IsNullOrEmpty(txtOidValue.Text))
            {
                GetDeviceOidListing();
                DispayInGrid();
            }
        }
        private bool ValidIPAddress(string ipAddress)
        {
            String pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-5]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            Regex validate = new Regex(pattern);
            bool bValid = false;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                bValid = validate.IsMatch(ipAddress, 0);
            }
            return bValid;
        }
        private void GetDeviceOidListing()
        {
            DeviceOids device = new DeviceOids(_password, txtIpAddr.Text, txtOidValue.Text);

            if (device.ValidateEngine())
            {
                ListOidEnt tmpOidList = new ListOidEnt();
                lblReleaseName.Text = device.ReleaseName;
                lblReleaseName.Refresh();

                device.GetDeviceOidListings();

                foreach (DeviceOidEnt dOid in device.ListDeviceOids)
                {
                    bool bFound = false;
                    foreach (OidEnt oidInfo in _listOids)
                    {
                        string temp = oidInfo.OidString + ".";
                        if (dOid.OidString.Contains(temp))
                        {
                            bFound = true;
                            AddOidData(oidInfo, dOid, tmpOidList);
                        }
                    }
                    if (!bFound)
                    {
                        OidEnt oid = new OidEnt();
                        oid.OidIndex = dOid.OidString;
                        oid.OidIndexValue = dOid.OidValue;
                        oid.Usage = USAGES.eHPDevice;

                        tmpOidList.Add(oid);
                    }
                }
                foreach (OidEnt oid in tmpOidList)
                {
                    _listOids.Add(oid);
                }
            }
            else
            {
                MessageBox.Show(device.GetLastError);
            }
        }

        private void AddOidData(OidEnt oidInfo, DeviceOidEnt dOid, ListOidEnt tmpOidList)
        {

            if (string.IsNullOrEmpty(oidInfo.OidIndex))
            {
                oidInfo.OidIndex = dOid.OidString;
                oidInfo.OidIndexValue = dOid.OidValue;
                oidInfo.Usage = USAGES.eHpDeviceMib;
            }
            else
            {
                // same family but a different index, so making a copy and setting same OID with different index
                OidEnt oid = new OidEnt(oidInfo);
                oid.OidIndex = dOid.OidString;
                oid.OidIndexValue = dOid.OidValue;
                oidInfo.Usage = USAGES.eHpDeviceMib;
                oid.Usage = USAGES.eHpDeviceMib;

                tmpOidList.Add(oid);
            }
        }

        private void MnuOptSnmpVer_Click(object sender, EventArgs e)
        {
            _snmpVersion2 = MnuOptSnmpVer2.IsChecked;

            if(MnuOptSnmpVer2.IsChecked == MnuOptSnmpVer1.IsChecked)
            {
                _snmpVersion2 = true;
            }

		}

		private void FrmTranslator_Load(object sender, EventArgs e)
		{
			var customMenuItem = new ApplyValueMenuItem();

			customMenuItem.ButtonClick +=customMenuItem_ButtonClick;
			customMenuItem.TextboxKeyDown += ApplyValueMenuItem_KeyDown;
			MnuOptPWD.Items.Add(customMenuItem);

		}

		private void ApplyValueMenuItem_KeyDown(object sender, EventArgs e)
		{
			customMenuItem_ButtonClick(sender, e);
		}
 
		private void customMenuItem_ButtonClick(object sender, EventArgs e)
		{
			var customMenuItem = MnuOptPWD.Items[0] as ApplyValueMenuItem;
			_password = customMenuItem.TextboxText;			
		}

    }

}
