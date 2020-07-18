using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using HP.ScalableTest;
using HP.ScalableTest.Framework.Automation;

namespace WebJetAdminComparisonTool
{
    public partial class WjaComparisonTool : Form
    {
        private ExcelApp _excelApp;
        private Workbook _wjaWorkbook;
        private Workbook _eprInjectorWorkbook;
        private Worksheet _eprWorksheet;
        private Sheets _eprSheets;
        private Worksheet _wjaWorksheet;
        private Dictionary<string, string> _eprReport;
        private Dictionary<string, string> _wjaReport;
        
        private const string EprInjectorSheet = "Data";
        private const string WjaReportSheet = "HP Web Jetadmin Exported Device";
        private const string WjaReportStatusColumn = "DeviceStatus";
        private const string EprReportStatusColumn = "Activity Name";
        private const string WjaReportIpColumn = "IPv4Address";
        private const string EprReportIpColumn = "Client Name";

        /// <summary>
        /// Creates a new instance of the <see cref="WjaComparisonTool"/> class
        /// </summary>
        public WjaComparisonTool()
        {
            _excelApp = new ExcelApp();

            _eprReport = new Dictionary<string, string>();
            _wjaReport = new Dictionary<string, string>();

            InitializeComponent();
        }

        private void Compare()
        {
            try
            {
                OpenApplication();
                AddDevicesAndStates();
                Log("Total mismatches detected: {0}".FormatWith(CompareReports()));
            }
            catch (Exception ex)
            {
                Log("Error: {0}".FormatWith(ex.Message));
            }
            finally
            {
                _eprReport.Clear();
                _wjaReport.Clear();
            }
        }

        private void OpenApplication()
        {
            _wjaWorkbook = _excelApp.Open(@wjaReport_TextBox.Text);
            _eprInjectorWorkbook = _excelApp.Open(@eprInjectorReport_TextBox.Text);
        }

        private void AddDevicesAndStates()
        {
            AddEprReportToDictionary();
            AddWjaReportToDictionary();
        }

        private void AddEprReportToDictionary()
        {
            _eprSheets = (Sheets)_eprInjectorWorkbook.Worksheets;
            _eprWorksheet = _eprSheets.get_Item(EprInjectorSheet);

            CreateList(_eprWorksheet, _eprReport);
        }

        private void AddWjaReportToDictionary()
        {
            _wjaWorksheet = _wjaWorkbook.Worksheets.get_Item(WjaReportSheet);
            CreateList(_wjaWorksheet, _wjaReport);
        }

        private int FindStatusColumn(object[,] dataBlock)
        {
            for (int i = 1; i <= dataBlock.GetLength(1); i++)
            {
                if (dataBlock[1, i].ToString() == WjaReportStatusColumn || dataBlock[1, i].ToString() == EprReportStatusColumn)
                {
                    return i;
                }
            }

            return 0;
        }

        private int FindIpColumn(object[,] dataBlock)
        {
            for (int i = 1; i <= dataBlock.GetLength(1); i++)
            {
                if (dataBlock[1, i].ToString() == WjaReportIpColumn || dataBlock[1, i].ToString() == EprReportIpColumn)
                {
                    return i;
                }
            }

            return 0;
        }

        private string UniformStatus(string status)
        {
            switch (status)
            {
                case EprInjectorConstants.JediPaperJamAlert:
                    status = EprInjectorConstants.PaperJamAlert;
                    break;
                case EprInjectorConstants.JediCoverOpenAlert:
                case EprInjectorConstants.OzCoverOpenAlert:
                    status = EprInjectorConstants.PrinterCoverOpenAlert;
                    break;
                case EprInjectorConstants.ScarletBlackCartridgeLowAlert:
                    status = EprInjectorConstants.BlackCartridgeAlert;
                    break;
                default:
                    break;
            }

            return status;
        }

        private void CreateList(Worksheet worksheet, Dictionary<string, string> reportDictionary)
        {
            int ipColumn, statusColumn;

            Range occupiedCells = worksheet.UsedRange;            
            object[,] dataBlock = occupiedCells.Value;

            if (dataBlock == null)
            {
                throw new Exception("Empty document");
            }

            ipColumn = FindIpColumn(dataBlock);
            statusColumn = FindStatusColumn(dataBlock);

            if (ipColumn < 1)
            {
                throw new Exception("The IP address column was not found");
            }

            if (statusColumn <  1)
            {
                throw new Exception("The device status column was not found");
            }

            PopulateDictionary(dataBlock, reportDictionary, ipColumn, statusColumn);
        }

        private void PopulateDictionary(object [,] dataBlock, Dictionary<string, string> reportDictionary, int ipColumn, int statusColumn)
        {
            string ipAddress, status;

            for (int j = 2; j <= dataBlock.GetLength(0); j++)
            {
                ipAddress = dataBlock[j, ipColumn].ToString();
                status = UniformStatus(dataBlock[j, statusColumn].ToString());

                if(reportDictionary.ContainsKey(ipAddress))
                {
                    reportDictionary[ipAddress] = status;
                }
                else
                {
                    reportDictionary.Add(ipAddress, status);
                }
            }
        }

        private void Log(string text)
        {            
            results_RichTextBox.AppendText("{0}\n".FormatWith(text));
        }

        private int CompareReports()
        {
            int mismatches = 0;
            
            foreach (string ip in _eprReport.Keys)
            {
                if (!_wjaReport.ContainsKey(ip))
                {
                    Log("Error: IP address {0} not found in the WJA Report".FormatWith(ip));
                }
                else if (!_eprReport[ip].Equals(_wjaReport[ip]))
                {
                    mismatches++;
                    Log("Mismatch detected for IP {0} Epr Injector Report: {1} | WJA Report: {2}".FormatWith(ip, _eprReport[ip], _wjaReport[ip]));
                }
            }

            return mismatches;
        }

        private void openWjaReport_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = report_OpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                wjaReport_TextBox.Text = report_OpenFileDialog.FileName;
            }
        }

        private void openEprReport_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = report_OpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                eprInjectorReport_TextBox.Text = report_OpenFileDialog.FileName;
            }
        }

        private void compare_Button_Click(object sender, EventArgs e)
        {
            if (wjaReport_TextBox.Text.Equals(string.Empty) || eprInjectorReport_TextBox.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please select which files to compare");
            }
            else
            {
                Compare();
            }
        }

        private void clearTextBox_Button_Click(object sender, EventArgs e)
        {
            results_RichTextBox.Clear();
        }
    }
}
