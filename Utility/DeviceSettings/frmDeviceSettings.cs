using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HP.RDL.STF.DeviceSettings
{
    public partial class FrmDeviceSettings : Form
    {
        private DataFims _listDeviceData;
        private const string FILTERS_ALL = "FIM Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv";
		private const string FILTERS_CSV = "CSV Files (*.csv)|*.csv";
        public FrmDeviceSettings()
        {
            InitializeComponent();
            _listDeviceData = new DataFims();
			txtIPAddress.Focus();
        }

        private void BtnRunClick(object sender, EventArgs e)
        {
            if (IsValidIP(txtIPAddress.Text))
            {
                RetrieveDeviceData();
            }
            else
            {
                string msg = "Given IP Address, " + txtIPAddress.Text + ", in not valid.\nPlease try again.";
                MessageBox.Show(msg, "IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIPAddress.Focus();
            }
        }
        /// <summary>
        /// Uses the xml files in resource to retrieve desired settings.
        /// I use the GetTestData for testing various methods of retrieving data.
        /// the RetrieveFirmwareInfo is used to retrieve the product release name and firmware version
        /// </summary>
        private void RetrieveDeviceData()
        {
            Cursor.Current = Cursors.WaitCursor;
            btnRun.Enabled = false;

            RetrieveFirmwareInfo();

            ProcessXML px = new ProcessXML();
            //px.GetTestData();
            px.GetFIM_TestingInfo();
           // px.GetDeviceInformation();
            RetrieveSettings(px.ListDeviceEndPoints);

            // start of new request
            if (_listDeviceData.Count == 0)
            {
                LoadDataFim(px.ListDeviceEndPoints, _listDeviceData);
            }
            else // retrieving the second set for comparison
            {
                RetrieveFirmwareInfo();

                DataFims lstTemp = new DataFims();

                LoadDataFim(px.ListDeviceEndPoints, lstTemp);
                AddedNewValue(lstTemp);
            }

            BindDataGrid();
            SetRowIndicator();

            Cursor.Current = Cursors.Default;
            btnRun.Enabled = true;
        }
        /// <summary>
        /// sets the color of the row depending on if the value comparison
        /// </summary>
        private void SetRowIndicator()
        {
            for(int idx = 0; idx < _listDeviceData.Count; idx++)
            {
                if(!_listDeviceData[idx].SameValue)
                {
                    dgvDataFim.Rows[idx].DefaultCellStyle.BackColor = Color.Maroon;
                    dgvDataFim.Rows[idx].DefaultCellStyle.ForeColor = Color.Beige;
                }
            }
        }
        /// <summary>
        /// Compares the lists in order to later display if any mismatches after firmware update
        /// </summary>
        /// <param name="lstTemp"></param>
        private void AddedNewValue(DataFims lstTemp)
        {
            if(_listDeviceData.Count == lstTemp.Count)
            {
                for(int idx = 0; idx < _listDeviceData.Count; idx++)
                {
                    _listDeviceData[idx].ValueNew = lstTemp[idx].ValueOrig;
                    if(_listDeviceData[idx].ValueNew.Equals(_listDeviceData[idx].ValueOrig))
                    {
                        _listDeviceData[idx].SameValue = true;
                    }
                    else
                    {
                        _listDeviceData[idx].SameValue = false;
                    }
                }
            }
        }

        private void BindDataGrid()
        {
            dataFimsBindingSource.DataSource = _listDeviceData;
            dataFimsBindingSource.ResetBindings(true);
        }
        /// <summary>
        /// Returns true if the given string is a valid IP address
        /// </summary>
        /// <param name="pIPAddr">string</param>
        /// <returns>bool</returns>
        public bool IsValidIP(string pIPAddr)
        {
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-5]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            Regex validate = new Regex(pattern);
            bool bOk = false;

            if (pIPAddr != "")
            {
                bOk = validate.IsMatch(pIPAddr, 0);
            }
            return bOk;
        }
        /// <summary>
        /// Starts the process for placing the retrieved device information into list that will be displayed in the grid.
        /// DataFims is the list displayed in the grid.
        /// </summary>
        /// <param name="listDs">DeviceSettings</param>
        /// <param name="listDat">DataFims</param>
        private void LoadDataFim(DeviceSettings listDs, DataFims listDat)
        {
            DataFim df = new DataFim();
            foreach(EndPoint ds in listDs)
            {
                if (ds.HasValues)
                { 
                    df = SetDeviceData(df, ds, listDat); 
                }
            }
        }
        /// <summary>
        /// Takes the given end point and sets the product and firmware information
        /// </summary>
        /// <param name="ds">EndPoint</param>
        private void SetProductInfo(EndPoint ds)
        {
            lblProductName.Text = FindReleaseName(ds.ListParents);
            if (lblFimOld.Text.Equals("Original FIM"))
            {
                lblFimOld.Text = FindRevisionNumber(ds.ListParents);
            }
            else
            {
                lblFimNew.Text = FindRevisionNumber(ds.ListParents);
            }
            this.Refresh();
        }
        /// <summary>
        /// Recursive call to find and returns the firmware revision with the given list
        /// </summary>
        /// <param name="parents">Parents</param>
        /// <returns>string</returns>
        private string FindRevisionNumber(Parents parents)
        {
            string revisionNumber = string.Empty;

            foreach(Parent p in parents)
            {
                if(CheckForRelease(p.ListPairedValues))
                {
                    revisionNumber = GetDeviceRevisionNumber(p.ListParents);
                    break;
                }
                else
                {
                    revisionNumber = FindRevisionNumber(p.ListParents);
                }              
            }

            return revisionNumber;
        }
        /// <summary>
        /// Checks the node (Parent) for a name of "Version". If found, will set the revision number
        /// </summary>
        /// <param name="parents">Parents</param>
        /// <returns>string</returns>
        private string GetDeviceRevisionNumber(Parents parents)
        {
            string revisionNumber = string.Empty;

            foreach( Parent p in parents)
            {
                if(p.ParentName.Equals("Version"))
                {
                    var rn = (p.ListPairedValues.FirstOrDefault(r => r.IdentName.Equals("Revision")));
                    if (rn != null)
                    {
                        revisionNumber = rn.IdentValue;
                    }
                }
            }

            return revisionNumber;
        }
        /// <summary>
        /// Looking to insure the AssetName is paired with a same product release name
        /// </summary>
        /// <param name="pairedValues">PairedValues</param>
        /// <returns>bool</returns>
        private bool CheckForRelease(PairedValues pairedValues)
        {
            bool bFound = false;

            foreach (PairedValue pv in pairedValues)
            {
                if(pv.IdentName.Equals("AssetName") && pv.IdentValue.Equals(lblProductName.Text))
                {
                    bFound = true;
                    break;
                }
            }

            return bFound;
        }
        /// <summary>
        /// Recursive call into list to find the release name
        /// </summary>
        /// <param name="parents"></param>
        /// <returns></returns>
        private string FindReleaseName(Parents parents)
        {
            string releaseName = string.Empty;

            foreach(Parent p in parents)
            {
                releaseName = GetDeviceReleaseName(p.ListPairedValues);
                if(string.IsNullOrEmpty(releaseName))
                {
                  releaseName = FindReleaseName(p.ListParents);
                }
                else
                {
                    break;
                }
            }

            return releaseName;
        }
        /// <summary>
        /// Need the correct paired values of identify
        /// </summary>
        /// <param name="listPv"></param>
        /// <returns></returns>
        private string GetDeviceReleaseName(PairedValues listPv)
        {
            string releaseName = string.Empty;
            bool bFound = false;

            foreach(PairedValue pv in listPv)
            {
                if(pv.IdentName.Equals("AssetName"))
                {
                    releaseName = pv.IdentValue;
                }
                else if(pv.IdentName.Equals("AssetType") && pv.IdentValue.Equals("Factory"))
                {
                    bFound = true;
                }
            }
            if(!bFound)
            {
                releaseName = string.Empty;
            }

            return releaseName;
        }

        private DataFim SetDeviceData(DataFim df, EndPoint ds, DataFims listData)
        {
            df.EndPoint = ds.EndPointName;

            df = ProcessParents(df, listData, ds.EndPointName, ds.ListParents);
            return df;
        }

        private DataFim ProcessParents(DataFim df, DataFims listData, string endpoint, Parents listParents )
        {
            foreach(Parent p in listParents)
            {
                df.Parent = p.ParentName;
                foreach (PairedValue pv in p.ListPairedValues)
                {
                    df.Element = pv.IdentName;
                    df.ValueOrig = pv.IdentValue;

                    listData.Add(df);

                    df = new DataFim();
                    df.EndPoint = endpoint;
                    df.Parent = p.ParentName;
                }
                if(p.ListParents.Count > 0)
                {
                    df = ProcessParents(df, listData, endpoint, p.ListParents);
                }
            }
            return df;
        }
        private void RetrieveFirmwareInfo()
        {			
            ProcessXML px = new ProcessXML();
            px.GetFirmwareInfo();

            RetrieveSettings(px.ListDeviceEndPoints);
            if (px.ListDeviceEndPoints[0].EndPointName.Equals("fim"))
            {
                SetProductInfo(px.ListDeviceEndPoints[0]);
            }

            this.Refresh();
        }
        private void RetrieveSettings(DeviceSettings lstDS)
        {
            ProccessStatusReq psr = new ProccessStatusReq("admin", "!QAZ2wsx", txtIPAddress.Text);
            try
            {
                foreach (EndPoint ds in lstDS)
                {
                    var epLog = psr.GetEndPointLog(ds.EndPointName, ds.ResourceURI);
                    if (epLog != null)
                    {
                        ProcessParentData(psr, epLog, ds.ListParents);
                        ds.HasValues = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Device Communication", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ProcessParentData(ProccessStatusReq psr, XElement epLog, Parents listParents)
        {
            foreach(Parent p in listParents)
            {
                var parentLog = psr.GetElementsSelf(epLog, p.ParentName);
                GetIdentityValues(psr, parentLog, p.ListPairedValues);
                if(p.ListParents.Count > 0)
                {
                    if (parentLog.Count > 0)
                    {
						// if no values have been set for the device no need to recurs
						if (!parentLog[0].IsEmpty)
						{
							ProcessParentData(psr, parentLog[0], p.ListParents);
						}
                    }
					//else
					//{
					//	MessageBox.Show("Improperly formatted XML data. The parentLog is hosed at: " + p.ParentName);
					//}
                }
            }
        }

        private void GetIdentityValues(ProccessStatusReq psr, List<XElement> parentLog, PairedValues pairedValues)
        {
            int idx = 0;
            List<string> identityValues = new List<string>();
            foreach (PairedValue pv in pairedValues)
            {
                foreach (var pl in parentLog)
                {
                    int nameCount = psr.CountChildName(pl, pv.IdentName);
                    if (nameCount == 1)
                    {
                        pv.IdentValue = psr.GetChildElementValue(pl, pv.IdentName);
                        if (pv.IdentValue.Length > 0) { break; }
                    }
                    else if (nameCount > 1)
                    {
                        identityValues = psr.GetChildElementValues(pl, pv.IdentName);
                        pv.IdentValue = identityValues[idx];
                        idx++;
                    }
                }
            }
        }


        private void SaveCsvFile()
        {
            sdgExcel = new SaveFileDialog();

                // Saving as a csv file
            sdgExcel.Filter = FILTERS_ALL;
            sdgExcel.FilterIndex = 2;
            sdgExcel.FileName = txtIPAddress.Text;
            sdgExcel.RestoreDirectory = true;

            SaveFileCsv();           
        }

        private void SaveFileCsv()
        {
            string dir = Path.GetDirectoryName(sdgExcel.FileName);
            string fileName = Path.GetFileName(sdgExcel.FileName);

            ProcessFile wf = new ProcessFile(fileName, dir, _listDeviceData);
            wf.SetDeviceInfo(txtIPAddress.Text, lblProductName.Text, lblFimOld.Text, lblFimNew.Text);
            if (!wf.WriteDeviceInfo())
            {
                MessageBox.Show(wf.ErrorMessage);
            }
        }
        private void SaveExcelFile()
        {
            string filePath = string.Empty;

            sdgExcel = new SaveFileDialog();
            sdgExcel.Filter = FILTERS_ALL;
            sdgExcel.RestoreDirectory = true;
            sdgExcel.FileName = txtIPAddress.Text + ".xlsx";
            sdgExcel.FilterIndex = 1;
            
            filePath = string.IsNullOrEmpty(sdgExcel.InitialDirectory) == false ? sdgExcel.InitialDirectory : Directory.GetCurrentDirectory();
            filePath += @"/" + sdgExcel.FileName;

            SaveExcelFile(filePath);                          
        }

        private void SaveExcelFile(string filePath)
        {
            WriteExcel we = new WriteExcel(_listDeviceData);
            we.WriteListToExcel();
            if (!we.IsError)
            {
                we.SaveExcel(filePath);
            }
            if (we.IsError)
            {
                MessageBox.Show(we.ErrorMessage);
            }
        }

        private void BtnExitClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MnuFileOpen_Click(object sender, EventArgs e)
        {
            ofdFiles = new OpenFileDialog();
            ofdFiles.Filter = FILTERS_CSV;
            ofdFiles.FilterIndex = 2;
            if(ofdFiles.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofdFiles.FileName;
                if (Path.GetExtension(filePath).ToLower().Equals(".csv"))
                {
                    ProcessFile pf = new ProcessFile(ofdFiles.FileName, ofdFiles.FileName, _listDeviceData);
                    if (pf.LoadDeviceData())
                    {
                        txtIPAddress.Text = pf.IPAddress;
                        lblProductName.Text = pf.DeviceName;
                        lblFimOld.Text = pf.OrigFIM;
                        lblFimNew.Text = pf.NewFIM;

                        _listDeviceData = pf.ListDeviceSettings;
                        BindDataGrid();
                    }
                }
            }

        }

        private void MnuFileSave_Click(object sender, EventArgs e)
        {
            SaveFile(Directory.GetCurrentDirectory());
        }

        private void MnuFileSaveAs_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            SaveFile(filePath);
        }
 
        private void SaveFile(string filePath)
        {
            sdgExcel = new SaveFileDialog();
            sdgExcel.Filter = FILTERS_ALL;
            sdgExcel.RestoreDirectory = true;
            sdgExcel.FileName = txtIPAddress.Text;
            sdgExcel.FilterIndex = GetFilterIndex();

            if (sdgExcel.ShowDialog() == DialogResult.OK)
            {
                if ((filePath = sdgExcel.FileName) != string.Empty)
                {
                    if (sdgExcel.FilterIndex == 1)
                    {
                        SaveExcelFile(filePath);
                    }
                    else if (sdgExcel.FilterIndex == 2)
                    {
                        SaveFileCsv();
                    }
                }
            }
        }

        private int GetFilterIndex()
        {
            return (lblFimNew.Text.Equals("Updated FIM") == false) ? 1 : 2;
        }

        private void MnuFileClear_Click(object sender, EventArgs e)
        {
            txtIPAddress.Text = string.Empty;
            lblProductName.Text = "Device Name and Model";
            lblFimOld.Text = "Original FIM";
            lblFimNew.Text = "Updated FIM";

            _listDeviceData.Clear();
            dgvDataFim.Rows.Clear();
        }

        private void TxtIPAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Return))
            {
                BtnRunClick(sender, e);
            }
        }
    }
}
