using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SafeComVirtualPrinterConfig.Properties;

namespace SafeComVirtualPrinterConfig
{
    /// <summary>
    /// Main form for Printer Definition File Generator utility
    /// </summary>
    public partial class MainForm : Form
    {
        private const string _header = "AssetId,IP Address,MAC Address,Serial Number";
        private StringBuilder _addressPrefix = null;
        private StringBuilder _startScript = null;
        private ErrorProvider _errorProvider = new ErrorProvider();
        
        public MainForm()
        {
            InitializeComponent();
            InitializeErrorProvider();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GenerateSetting_CheckedChanged(null, e);
        }

        /// <summary>
        /// Gets the octet value for the passed-in textbox.
        /// </summary>
        private byte GetByteValue(TextBox textBox)
        {
            byte result;
            if (byte.TryParse(textBox.Text, out result))
            {
                return result;
            }
            return 0;
        }

        private int GetIntValue(TextBox textBox)
        {
            int result;
            if (int.TryParse(textBox.Text, out result))
            {
                return result;
            }
            return int.MaxValue;
        }

        private void IP_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (radioButton_New.Checked)
            {
                e.Cancel = !IsValidOctet((TextBox)sender);
            }
        }

        private bool IsValidOctet(TextBox textBox)
        {
            string errorMessage = null;
            byte val = GetByteValue(textBox);

            if (val < 1 || val > 255)
            {
                errorMessage = "Octet Value must be between 1 and 255.";
            }

            _errorProvider.SetError(textBox, errorMessage);
            return (errorMessage == null);
        }

        private void textBox_Count_Validating(object sender, CancelEventArgs e)
        {
            if (radioButton_New.Checked)
            {
                e.Cancel = !IsValidCount((TextBox)sender);
            }
        }

        private bool IsValidCount(TextBox textBox)
        {
            string errorMessage = null;
            byte start = GetByteValue(textBox_Octet4);
            int count = GetIntValue(textBox_Count);
            int sum = start + count;

            if (sum < 1 || sum > 256)
            {
                errorMessage = string.Format("Count cannot be greater than {0}.", (256 - start));
            }

            _errorProvider.SetError(textBox, errorMessage);
            return errorMessage == null;
        }
       
        private void NumbersOnlyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Determine whether the keystroke is a number from either the numbers on the keyboard or the keypad
            e.SuppressKeyPress = ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back);
        }

        private void button_Generate_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren() && ValidateFilePath())
            {
                StringBuilder output = new StringBuilder();
                _startScript = new StringBuilder();

                //This is where the magic happens
                if (radioButton_New.Checked)
                {
                    GenerateNew(ref output);
                }
                else
                {
                    GenerateFromOutputFile(textBox_SourceFile.Text, ref output);
                }

                //File.WriteAllBytes(label_InstallPath.Text + "\\snmpsimd.exe", Resources.snmpsimd); //This doesn't work with Win7 and above.
                if (_startScript.Length > 0)
                {
                    File.WriteAllText(label_InstallPath.Text + "\\StartSNMPListeners.cmd", _startScript.ToString());
                }
                File.WriteAllText(label_InstallPath.Text + "\\VirtualPrinterList.csv", output.ToString());
                
                MessageBox.Show("Finished generating virtual printer files.", "Generate Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateFromOutputFile(string sourceFilePath, ref StringBuilder output)
        {
            string line = string.Empty;
            string[] items = null;
            StringBuilder filePath = new StringBuilder(label_OutputPath.Text).Append("\\");
            int removeStart = filePath.Length;

            // Read the file and display it line by line.
            using (StreamReader file = new StreamReader(sourceFilePath))
            {
                // Take care of the header
                line = file.ReadLine();
                output.AppendLine(line);

                while ((line = file.ReadLine()) != null)
                {
                    items = line.Split(',');
                    BuildFilePath(ref filePath, ParseIndex(items[0]));
                    
                    WriteFile(filePath.ToString(), BuildVirtualPrinterFile(items[0], items[1], items[2], items[3]));
                    output.AppendLine(line);
                    filePath.Remove(removeStart, filePath.Length - removeStart);
                }

                file.Close();
            }
        }

        private void GenerateNew(ref StringBuilder output)
        {
            string ipAddress = string.Empty;
            string macAddress = string.Empty;
            string assetId = string.Empty;
            string serialNumber = string.Empty;
            int start = GetByteValue(textBox_Octet4);
            int count = GetByteValue(textBox_Count);
            StringBuilder filePath = new StringBuilder(label_OutputPath.Text).Append("\\");
            int removeStart = filePath.Length;

            BuildPrefixes();
            output.AppendLine(_header);

            for (int i = start; i < (start + count); i++)
            {
                BuildFilePath(ref filePath, i.ToString());

                ipAddress = AppendOctet(_addressPrefix, ref i);
                macAddress = GetRandomMacAddress(i);
                serialNumber = GetRandomSerialNumber(i);
                assetId = GetAssetId(i);

                WriteFile(filePath.ToString(), BuildVirtualPrinterFile(assetId, ipAddress, macAddress, serialNumber));
                _startScript.AppendLine(string.Format(Resources.StartCommand, ipAddress, label_ExePath.Text, filePath.ToString(0, filePath.LastIndexOf('\\'))));
                output.AppendLine(string.Format("{0},{1},{2},{3}", GetAssetId(i), ipAddress, macAddress, serialNumber));

                filePath.Remove(removeStart, filePath.Length - removeStart);
            }
        }

        private void BuildFilePath(ref StringBuilder filePath, string folderName)
        {
            filePath.Append(folderName);
            filePath.Append("\\public.snmprec");
        }

        private void BuildPrefixes()
        {
            byte oct1, oct2, oct3;

            if (! byte.TryParse(textBox_Octet1.Text, out oct1))
            {
                oct1 = 0;
            }

            if (!byte.TryParse(textBox_Octet2.Text, out oct2))
            {
                oct2 = 0;
            }

            if (!byte.TryParse(textBox_Octet3.Text, out oct3))
            {
                oct3 = 0;
            }
            
            _addressPrefix = new StringBuilder(oct1.ToString());
            _addressPrefix.Append(".");
            _addressPrefix.Append(oct2.ToString());
            _addressPrefix.Append(".");
            _addressPrefix.Append(oct3.ToString());
            _addressPrefix.Append(".");
        }

        private string AppendOctet(StringBuilder prefix, ref int octetValue, bool padOctet = false)
        {
            string octetString = padOctet ? octetValue.ToString("D3") : octetValue.ToString();
            int removeStart = prefix.Length;

            prefix.Append(octetString);
            string result = prefix.ToString();
            prefix.Remove(removeStart, octetString.Length);

            return result;
        }

        private string BuildVirtualPrinterFile(string assetId, string ipAddress, string macAddress, string serialNumber)
        {
            StringBuilder result = new StringBuilder(Resources.azalea_Template);
            
            result.Replace("{IPADDRESS}", ipAddress);
            result.Replace("{DEVICEMAC}", macAddress);
            result.Replace("{devicemac}", macAddress.ToLower());
            result.Replace("{DEVICESERIAL}", serialNumber);
            result.Replace("{DEVICENAME}", assetId);
            result.Replace("{MODEL}", "Virtual Printer");

            return result.ToString();
        }

        private void WriteFile(string filePath, string fileText)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();
            File.WriteAllText(filePath, fileText);
        }

        private string ParseIndex(string assetId)
        {
            return assetId.Split('-')[1].TrimStart('0');
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.SetIconAlignment(textBox_Octet1, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet2, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet3, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet4, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Count, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(label_Output, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_SourceFile, ErrorIconAlignment.MiddleLeft);
        }

        private void linkLabel_SetLocation_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    label_InstallPath.Text = dialog.SelectedPath;
                    label_OutputPath.Text = dialog.SelectedPath + "\\Data";
                    label_ExePath.Text = dialog.SelectedPath + "\\snmpsimd.exe";
                }
            }
        }

        private bool ValidateFilePath()
        {
            bool result = ! string.IsNullOrEmpty(label_OutputPath.Text);

            if (string.IsNullOrEmpty(label_OutputPath.Text))
            {
                _errorProvider.SetError(label_Output, "Must select a file destination.");
                result = false;
            }
            else
            {
                _errorProvider.SetError(label_Output, string.Empty);
                result = true;
            }
            return result;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Octet_Leave(object sender, EventArgs e)
        {
            UpdateEndingAddress();
        }

        private void textBox_Count_Leave(object sender, EventArgs e)
        {
            UpdateEndingAddress();
        }

        private void UpdateEndingAddress()
        {
            byte start, count;

            if (! byte.TryParse(textBox_Octet4.Text, out start))
            {
                start = 0;
            }

            if (!byte.TryParse(textBox_Count.Text, out count))
            {
                count = 0;
            }
            
            textBox_End1.Text = textBox_Octet1.Text;
            textBox_End2.Text = textBox_Octet2.Text;
            textBox_End3.Text = textBox_Octet3.Text;
            if (start > 0 && count > 0)
            {
                textBox_End4.Text = (start + count - 1).ToString();
            }
        }

        private static string GetRandomMacAddress(int seed)
        {
            Random random = new Random(seed);
            var buffer = new byte[6];

            random.NextBytes(buffer);
            string result = string.Concat(buffer.Select(x => string.Format("{0}", x.ToString("X2"))).ToArray());

            //return result.TrimEnd(':');
            return result;
        }

        private static string GetRandomSerialNumber(int seed)
        {
            string chars = "AB0CDE1FG2HIJ3KL4MN5PQ6RST7UV8WXY9Z";
            char[] result = new char[10];
            Random random = new Random(seed);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private static string GetAssetId(int lastOctet)
        {
            StringBuilder result = new StringBuilder("VP");
            string numberString = new string(Environment.MachineName.Where(Char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(numberString))
            {
                numberString = "01";
            }

            result.Append(numberString);
            result.Append("-");
            result.Append(lastOctet.ToString("D5"));

            return result.ToString();
        }

        private void GenerateSetting_CheckedChanged(object sender, EventArgs e)
        {
            textBox_SourceFile.Enabled = radioButton_File.Checked;
            button_SetSourceFile.Enabled = radioButton_File.Checked;

            groupBox_Address.Enabled = radioButton_New.Checked;
        }

        private void button_SetSourceFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set filter options and filter index.
                openFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";
                openFileDialog.FilterIndex = 1;

                // Call the ShowDialog method to show the dialog box.
                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    textBox_SourceFile.Text = openFileDialog.FileName;
                }
            }
        }


    }
}
