using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTestTriageData.Data.DataLog;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using System.Diagnostics;

namespace HP.ScalableTestTriageData.Data
{
    public partial class TriageDataForm : Form
    {
        
        private bool _databindingComplete;
        private static ILog log = LogManager.GetLogger("Program");
        private TriageDataList _triageDataList = new TriageDataList();
        private TriageDataInfoList _triageDataInfoList = new TriageDataInfoList();

        private List<string> _sessionIds = new List<string>();
        private List<string> _ipAddrs = new List<string>();
        private List<string> _products = new List<string>();
        private bool _starting = true;

        private readonly string _logLocation;

        public TriageDataForm()
        {
            InitializeComponent();

            var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();

            _logLocation = rootAppender != null ? rootAppender.File : string.Empty;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSessionIds();
                string sessionId = cboSessionIds.Text;
                RetrieveTriageData(sessionId);
                SetDeviceData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = ex.Message.Substring(0, 15);
                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message.Substring(0, 15);
                }

                ShowErrorMessage(ex);
            }
        }

        private void TriageDataForm_Load(object sender, EventArgs e)
        {
            SetDatabase();

            dtpStart.Value = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            LoadSessionIds();

            if (cboSessionIds.Text.Length == 8)
            {
                SetDeviceData();
            }
            _starting = false;
        }

        private void SetDeviceData()
        {
            GetSessionActivityDevices();

            LoadIpAddresses();
            LoadProducts();
        }

        private void LoadProducts()
        {
            _products = new List<string>();
            foreach (TriageDataInfo tdi in _triageDataInfoList)
            {
                if (!_products.Exists(prod => prod == tdi.Product))
                {
                    _products.Add(tdi.Product);
                }
            }
            cboProduct.DataSource = _products;
            cboProduct.SelectedIndex = -1;
        }

        private void LoadIpAddresses()
        {
            _ipAddrs = new List<string>();
            foreach (TriageDataInfo tdi in _triageDataInfoList)
            {
                if (!_ipAddrs.Exists(ip => ip == tdi.IPAddress))
                {
                    _ipAddrs.Add(tdi.IPAddress);
                }
            }
            cboIpAddress.DataSource = _ipAddrs;
            cboIpAddress.SelectedIndex = -1;
        }


        private void TriageDataForm_Shown(object sender, EventArgs e)
        {
            if (cboSessionIds.Items.Count > 0)
            {
                CheckGrid();
            }
            if (rgvTriageData.RowCount > 0)
            {
                rgvTriageData.Rows[0].IsSelected = true;

                SetSelectedRow(rgvTriageData.CurrentRow);
            }
           
            
        }
        private void cboSessionIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sessionId = cboSessionIds.Text;
            RetrieveTriageData(sessionId);

            SetDeviceData();
        }

        private void LoadSessionIds()
        {
            DbAccess db = new DbAccess(GlobalSettings.Database);
            using (DataLogContext td = new DataLogContext(db.getConStrSQL()))
            {
                _sessionIds = TriageData.SessionIds(td, dtpStart.Value.ToUniversalTime(), dtpEnd.Value.ToUniversalTime());
                cboSessionIds.DataSource = _sessionIds;
            }
        }

        private void RetrieveTriageData(string sessionId)
        {
            DbAccess db = new DbAccess(GlobalSettings.Database);
            using (DataLogContext dlContext = new DataLogContext(db.getConStrSQL()))
            {
                var myTriageData = TriageData.GetTriageDataBySessionId(dlContext, sessionId).ToList();
                _triageDataList = new TriageDataList();
                _triageDataList.AddRange(myTriageData);

                BindTriageDataGrid(_triageDataList);
                lblCountErrors.Text = $@"Error Count={myTriageData.Count}";
            }
            pbControlPanel.Image = null;
            pbThumbnail.Image = null;
        }

        private void rgvTriageData_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    SetSelectedRow(e.Row);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ShowErrorMessage(ex);
                }
            }
        }

        private void GetSessionActivityDevices()
        {
            _triageDataInfoList = new TriageDataInfoList();

            foreach (TriageData td in _triageDataList)
            {
                TriageDataInfo tdi = new TriageDataInfo();

                DbAccess db = new DbAccess(GlobalSettings.Database);
                using (DataLogContext dlc = new DataLogContext(db.getConStrSQL()))
                {
                    var assetUsage = ActivityExecutionAssetUsage.GetByActivityExecutionId(dlc, td.ActivityExecutionId);
                    if (assetUsage != null)
                    {
                        tdi.DeviceId = assetUsage.AssetId;

                        ActivityExecution ae = ActivityExecution.GetById(dlc, td.ActivityExecutionId);
                        SessionDevice sd = SessionDevice.GetBySessionDeviceId(dlc, cboSessionIds.Text, tdi.DeviceId);

                        tdi.ActivityType = ae.ActivityType;
                        tdi.ActivityName = ae.ActivityName;
                        tdi.UserName = ae.UserName;

                        if (sd != null)
                        {
                            tdi.Firmware = sd.FirmwareRevision;
                            tdi.FirmwareDatecode = sd.FirmwareDatecode;
                            tdi.Model = sd.DeviceName;
                            tdi.IPAddress = sd.IpAddress;
                            tdi.Product = sd.ProductName;
                        }

                        var aepList = ActivityExecutionPerformance.GetByActivityExecutionId(dlc, td.ActivityExecutionId);
                        tdi.AddPerformanceMarkers(aepList);
                    }
                    else
                    {
                        rtbErrorMessage.Text = $@"Missing Activity Asset Usage for activity {td.ActivityExecutionId}";
                        MessageBox.Show($@"Missing Activity Asset Usage for activity {td.ActivityExecutionId}");
                        return;
                    }

                }
                tdi.ActivityExecutionId = td.ActivityExecutionId;
                _triageDataInfoList.Add(tdi);
            }
        }
        private void SetSelectedRow(GridViewRowInfo row)
        {            
            TriageData td = (TriageData)rgvTriageData.CurrentRow.DataBoundItem;
            pbControlPanel.Image = null;
            if (td != null)
            {
                TriageDataInfo tdi = new TriageDataInfo();

                DbAccess db = new DbAccess(GlobalSettings.Database);
                using (DataLogContext dlc = new DataLogContext(db.getConStrSQL()))
                {
                    tdi.DeviceId = ActivityExecutionAssetUsage.GetByActivityExecutionId(dlc, td.ActivityExecutionId).AssetId;
                    ActivityExecution ae = ActivityExecution.GetById(dlc, td.ActivityExecutionId);
                    SessionDevice sd = SessionDevice.GetBySessionDeviceId(dlc, cboSessionIds.Text, tdi.DeviceId);

                    tdi.ActivityType = ae.ActivityType;
                    tdi.ActivityName = ae.ActivityName;
                    tdi.UserName = ae.UserName;

                    if (sd != null)
                    {
                        tdi.Firmware = sd.FirmwareRevision;
                        tdi.FirmwareDatecode = sd.FirmwareDatecode;
                        tdi.Model = sd.DeviceName;
                        tdi.IPAddress = sd.IpAddress;
                        tdi.Product = sd.ProductName;
                    }

                    var aepList = ActivityExecutionPerformance.GetByActivityExecutionId(dlc, td.ActivityExecutionId);
                    tdi.AddPerformanceMarkers(aepList);
                    SetPerformanceMarkers(tdi.PerformanceMarkers);

                    lblDeviceId.Text = tdi.DeviceId;
                    lblIPAddress.Text = tdi.IPAddress;
                    lblModelInfo.Text = tdi.Model;
                    lblProduct.Text = tdi.Product;
                    lblFirmwareDatecode.Text = tdi.FirmwareDatecode;
                    lblFirmwareRevision.Text = tdi.Firmware;
                    lblUserId.Text = tdi.UserName;
                    lbActivityType.Text = tdi.ActivityType;
                    lblActivityName.Text = tdi.ActivityName;
                }

                byte[] thumbnail = (byte[])row.Cells["Thumbnail"].Value;
                if (row.Cells["ControlIds"].Value != null)
                {
                    string regExp = @"[\n\r]+";
                    string[] temp = Regex.Split(row.Cells["ControlIds"].Value.ToString(), regExp);
                    for(int ctr = 0; ctr < temp.Length; ctr++)
                    {
                        errMessageAndroid.Text += temp[ctr];
                    }
                }

                if (row.Cells["Reason"].Value != null)
                {
                    rtbErrorMessage.Text = row.Cells["Reason"].Value.ToString();
                }

                if (!string.IsNullOrEmpty(td.DeviceWarnings))
                {
                    rtbErrorMessage.Text += "\r\n\r\nDevice Warnings: " + td.DeviceWarnings;
                }

                tabControlInfo.SelectedIndex = 0;
                SetImage(pbThumbnail, thumbnail);
                pbControlPanel.Image = null;
            }
        }
        private void SetRowInfo(GridViewRowInfo row)
        {
            TriageDataInfo tdi = new TriageDataInfo();

            DbAccess db = new DbAccess(GlobalSettings.Database);
            using (DataLogContext dlc = new DataLogContext(db.getConStrSQL()))
            {
                Guid aeId = (Guid)row.Cells["ActivityExecutionId"].Value;
                tdi.DeviceId = ActivityExecutionAssetUsage.GetByActivityExecutionId(dlc, aeId).AssetId;
                ActivityExecution ae = ActivityExecution.GetById(dlc, aeId);
                SessionDevice sd = SessionDevice.GetBySessionDeviceId(dlc, cboSessionIds.Text, tdi.DeviceId);

                tdi.ActivityType = ae.ActivityType;
                tdi.ActivityName = ae.ActivityName;
                tdi.UserName = ae.UserName;

                if (sd != null)
                {
                    tdi.Firmware = sd.FirmwareRevision;
                    tdi.FirmwareDatecode = sd.FirmwareDatecode;
                    tdi.Model = sd.DeviceName;
                    tdi.IPAddress = sd.IpAddress;
                    tdi.Product = sd.ProductName;
                }

                var aepList = ActivityExecutionPerformance.GetByActivityExecutionId(dlc, aeId);
                tdi.AddPerformanceMarkers(aepList);
                SetPerformanceMarkers(tdi.PerformanceMarkers);

                lblDeviceId.Text = tdi.DeviceId;
                lblIPAddress.Text = tdi.IPAddress;
                lblModelInfo.Text = tdi.Model;
                lblProduct.Text = tdi.Product;
                lblFirmwareDatecode.Text = tdi.FirmwareDatecode;
                lblFirmwareRevision.Text = tdi.Firmware;
                lblUserId.Text = tdi.UserName;
                lbActivityType.Text = tdi.ActivityType;
                lblActivityName.Text = tdi.ActivityName;

            }

            byte[] thumbnail = (byte[])row.Cells["Thumbnail"].Value;
            if (row.Cells["ControlIds"].Value != null)
            {
                string regExp = @"[\n\r]+";
                string[] temp = Regex.Split(row.Cells["ControlIds"].Value.ToString(), regExp);
                for (int ctr = 0; ctr < temp.Length; ctr++)
                {
                    errMessageAndroid.Text += temp[ctr];
                }
            }
            if (row.Cells["Reason"].Value != null)
            {
                rtbErrorMessage.Text = row.Cells["Reason"].Value.ToString();
            }
            tabControlInfo.SelectedIndex = 0;
            SetImage(pbThumbnail, thumbnail);
            pbControlPanel.Image = null;
        }

        private void SetImage(PictureBox pb,  byte[] thumbnail)
        {
            MemoryStream ms = new MemoryStream(thumbnail);
            try
            {
                Image img = Image.FromStream(ms);
                pb.Image = img;
            }
            catch (Exception)
            {
                pb.Image = null;
                log.Warn("SetImage Error by a stream issue");
                MessageBox.Show("Stream Issue trying to set the thumbnail image.");
            }
        }

        private void SetPerformanceMarkers(PerformanceMarkerList lst)
        {
            activityExecutionPerformanceBindingSource.DataSource = lst;
            activityExecutionPerformanceBindingSource.ResetBindings(false);
        }

        private void pbThumbnail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string triageDataId = rgvTriageData.SelectedRows[0].Cells["TriageDataId"].Value.ToString();
                Guid tdId = Guid.Parse(triageDataId);

                DbAccess db = new DbAccess(GlobalSettings.Database);
                using (DataLogContext dlc = new DataLogContext(db.getConStrSQL()))
                {
                    byte[] picture = TriageData.GetTriageDataById(dlc, tdId).ControlPanelImage;
                    
                    SetImage(pbControlPanel, picture);
                    tabControlInfo.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowErrorMessage(ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowErrorMessage(ex);
            }
        }

        private void btnSessionTriage_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                string sessionId = cboSessionIds.Text;
                if (sessionId.Length == 8)
                {
                    rgvTriageData.Rows.Clear();
                    rgvPerformanceMarkers.Rows.Clear();

                    rtbErrorMessage.Text = string.Empty;
                    pbControlPanel.Image = null;
                    pbThumbnail.Image = null;

                    DbAccess db = new DbAccess(GlobalSettings.Database);
                    using (DataLogContext dlContext = new DataLogContext(db.getConStrSQL()))
                    {
                        var myTriageData = TriageData.GetTriageDataBySessionId(dlContext, sessionId).ToList();
                        if (myTriageData.Any())
                        {
                            dtpStart.Value = myTriageData.First().TriageDateTime;
                            dtpEnd.Value = myTriageData.Last().TriageDateTime;

                            triageDataListBindingSource.DataSource = myTriageData;
                            triageDataListBindingSource.ResetBindings(false);
                        }
                        else
                        {
                            errMsg = "TriageData does not contain data for the given session ID " + sessionId;
                        }
                    }
                    pbControlPanel.Image = null;
                    pbThumbnail.Image = null;
                }
                else
                {
                    errMsg = "SessionId is not correctly formatted.";
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(errMsg))
                {
                    log.Warn(errMsg);
                }
                else
                {
                    log.Warn(ex);
                    ShowErrorMessage(ex);
                }
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                MessageBox.Show(errMsg, @"Session ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Set the database from the App.comfig file
        /// </summary>
        private void SetDatabase()
        {
            if (ConfigurationManager.AppSettings.Count > 1)
            {
                DatabaseConnectDlg connectDialog = new DatabaseConnectDlg();

                if (connectDialog.ShowDialog() != DialogResult.OK)
                {
                    DialogResult result = MessageBox.Show(@"Please select a Server to connect. Press OK to continue, Cancel to exit application", @"Connection Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    if (result == DialogResult.OK)
                    {
                        SetDatabase();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }

            }
            else if (ConfigurationManager.AppSettings.Count == 1)
            {
                GlobalSettings.Database = ConfigurationManager.AppSettings[0];
                GlobalSettings.EnvironmentName = ConfigurationManager.AppSettings.AllKeys[0];
            }
            else
            {
                MessageBox.Show(@"There is no database to connect to. Application is shutting down", @"Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            Text += @" - " + GlobalSettings.Database;
        }

        private void rgvTriageData_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            _databindingComplete = true;
        }

        private void rgvTriageData_Paint(object sender, PaintEventArgs e)
        {
            if (_databindingComplete)
            {
                _databindingComplete = false;
                if (rgvPerformanceMarkers.RowCount > 0)
                {
                    rgvPerformanceMarkers.Rows[0].IsSelected = true;
                }
            }
        }

        private void CheckGrid()
        {
            int cnt = 0;
            DateTime dt = DateTime.Now.AddSeconds(30);
            while (cnt == 0 && dt > DateTime.Now) // Waiting for the grid to populate.
            {
                cnt = rgvTriageData.RowCount;
            }
        }

        private void cboIpAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboIpAddress.SelectedIndex >= 0 && cboProduct.SelectedIndex >= 0)
            {
                cboProduct.SelectedIndex = -1;
            }
        }

        private void GetSelectedIpAddressData()
        {
            if (!_starting && cboIpAddress.SelectedIndex >= 0)
            {
                // set the grid data source
                TriageDataList myTriageData = new TriageDataList();
                string ipAddress = cboIpAddress.Text;

                var triageDataList = (from triageDataInfo in _triageDataInfoList
                    join triageData in _triageDataList on triageDataInfo.ActivityExecutionId equals triageData
                        .ActivityExecutionId
                    where triageDataInfo.IPAddress == ipAddress
                    select triageData).ToList();
                
             
                myTriageData.AddRange(triageDataList);
                BindTriageDataGrid(myTriageData);
            }
        }

        private void GetSelectedProductData()
        {
            if (!_starting && cboProduct.SelectedIndex >= 0)
            {
                // set the grid data source
                TriageDataList myTriageData = new TriageDataList();
                string product = cboProduct.Text;

                foreach (TriageDataInfo tdi in _triageDataInfoList)
                {
                    if (tdi.Product.Equals(product))
                    {
                        foreach (TriageData td in _triageDataList)
                        {
                            if (td.ActivityExecutionId == tdi.ActivityExecutionId)
                            {
                                myTriageData.Add(td);
                            }
                        }
                    }
                }

                BindTriageDataGrid(myTriageData);
            }
        }

        private void BindTriageDataGrid(TriageDataList myTriageData)
        {
            triageDataListBindingSource.DataSource = myTriageData;
            triageDataListBindingSource.ResetBindings(false);
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex >= 0 && cboIpAddress.SelectedIndex >= 0)
            {
                cboIpAddress.SelectedIndex = -1;
            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (cboIpAddress.SelectedIndex >= 0)
            {
                try
                {
                    GetSelectedIpAddressData();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            else if (cboProduct.SelectedIndex >= 0)
            {
                try
                {
                    GetSelectedProductData();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ShowErrorMessage(ex);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboIpAddress.SelectedIndex = -1;
            cboProduct.SelectedIndex = -1;

            BindTriageDataGrid(_triageDataList);
        }

        private void btnFindAEid_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(txtActivityExecutionId.Text))
            {
                try
                {
                    Guid activityExecutionId = Guid.Parse(txtActivityExecutionId.Text);
                    FindActivityExecutionId(activityExecutionId);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    log.Warn(msg);
                }
            }
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
            }
        }

        private void FindActivityExecutionId(Guid activityExecutionId)
        {
            rgvTriageData.CurrentRow.IsCurrent = false;
            
            foreach (GridViewRowInfo row in rgvTriageData.Rows)
            {
                string aeId = row.Cells["ActivityExecutionId"].Value.ToString();
                if (aeId == activityExecutionId.ToString())
                {
                    row.IsSelected = true;
                    row.Cells["ActivityExecutionId"].IsSelected = true;
                    SetRowInfo(row);
                    txtActivityExecutionId.Text = string.Empty;
                    rgvPerformanceMarkers.CurrentRow.IsCurrent = false;
                    break;
                }
            }

            if (txtActivityExecutionId.Text.Length > 0)
            {
                MessageBox.Show($@"ActivityExecutionId {txtActivityExecutionId.Text} is not in the current listing.");
            }
        }

        private void errMessageAndroid_MouseHover(object sender, EventArgs e)
        {
            ToolTip jaToolTip = new ToolTip();
            jaToolTip.ReshowDelay = 3000;
            jaToolTip.UseFading = true;
            jaToolTip.AutoPopDelay = 3000;
            jaToolTip.Show("This tab is showing Android log when you ran Android Test.\n If you did not, This log is to show a path of scenario", errMessageAndroid, 2000);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbErrorMessage.SelectedText);
        }

        private void rtbErrorMessage_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        int xPos = tabControlInfo.Location.X + e.X + 25;
                        int yPos = tabControlInfo.Location.Y + e.Y + 40;

                        cmsErrorInfo.Show(this, new Point(xPos, yPos));
                    }
                    break;
            }
        }

        private void btnUIDump_Click(object sender, EventArgs e)
        {
            try
            {
                TriageData tiageData = (TriageData)rgvTriageData.CurrentRow.DataBoundItem;
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (tiageData.UIDumpData != null)
                {
                    folderBrowserDialog.ShowDialog();
                    GetSelectedRowUidumpData(tiageData, folderBrowserDialog.SelectedPath);
                }
                else
                {
                    MessageBox.Show("Selected Activity has no Android data for the JetAdvantageLink");
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                MessageBox.Show(ex.Message + "\r\n(If a row is not seleted, this exception can be occurred.)");
            }
        }

        private void GetSelectedRowUidumpData(TriageData tiageData, String folderPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(folderPath))
                {
                    StringBuilder shooFile = new StringBuilder(Path.Combine(folderPath));
                    shooFile.Append("\\UiDump");
                    shooFile.Append(tiageData.SessionId);
                    shooFile.Append(tiageData.ActivityExecutionId.ToString());
                    shooFile.Append(".txt");

                    StreamWriter writer = new StreamWriter(shooFile.ToString());
                    writer.Write(tiageData.UIDumpData);
                    writer.Flush();
                    writer.Close();
                    MessageBox.Show("Saved:\r\n" + shooFile);
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                MessageBox.Show(ex.Message);
            }
        }
                
        private void ShowErrorMessage(Exception errorMsg)
        {
            string msg = errorMsg.Message.Substring(0, 15);
            if (errorMsg.InnerException != null)
            {
                msg = errorMsg.InnerException.Message.Substring(0, 15);
            }

            string message = $"{msg}\n\rLog Location: {_logLocation }\n\rOpen the error log?";

            if (MessageBox.Show(message, "STF-STB Triage Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start(_logLocation);
            }
        }
    }
}
