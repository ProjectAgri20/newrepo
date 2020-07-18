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
using HPACPrinterDefGenerator.Properties;

namespace HPACPrinterDefGenerator
{
    /// <summary>
    /// Main form for Printer Definition File Generator utility
    /// </summary>
    public partial class MainForm : Form
    {
        private StringBuilder _addressPrefix = null;
        private StringBuilder _fileNamePrefix = null;
        private ErrorProvider _errorProvider = new ErrorProvider();
        
        public MainForm()
        {
            InitializeComponent();
            InitializeErrorProvider();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            TextBox textBox = (TextBox)sender;
            
            string errorMessage = null;
            byte val = GetByteValue(textBox);

            if (val < 1 || val > 255)
            {
                errorMessage = "Octet Value must be between 1 and 255.";
            }

            _errorProvider.SetError(textBox, errorMessage);
            e.Cancel = errorMessage != null;
        }

        private void textBox_Count_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage = null;
            byte start = GetByteValue(textBox_Octet4);
            int count = GetIntValue(textBox_Count);
            int sum = start + count;

            if (sum < 1 || sum > 256)
            {
                errorMessage = string.Format("Count cannot be greater than {0}.", (256 - start));
            }

            _errorProvider.SetError((TextBox)sender, errorMessage);
            e.Cancel = errorMessage != null;
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
                int start = GetByteValue(textBox_Octet4);
                int count = GetByteValue(textBox_Count);
                string hostName = string.Empty;
                string filePath = string.Empty;

                BuildPrefixes();

                for (int i = start; i < (start + count); i++)
                {
                    hostName = AppendOctet(_fileNamePrefix, ref i, true);
                    filePath = label_FilePath.Text + "\\" + hostName;
                    
                    File.WriteAllText(filePath, BuildPrinterDefFile(_addressPrefix, ref hostName, ref i));
                }

                MessageBox.Show("Finished generating printer definition files.", "Generate Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

            _fileNamePrefix = new StringBuilder(textBox_VPrintHost.Text.ToLower());
            _fileNamePrefix.Append("-");
            _fileNamePrefix.Append(oct3.ToString("D3"));
            _fileNamePrefix.Append("-");
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

        private string BuildPrinterDefFile(StringBuilder ipPrefix, ref string hostName, ref int octetValue)
        {
            // The HPAC virtual printer files used to work with the NoFilter option, but they don't anymore.
            // I have left the file with no filter in this project, but it is not used.
            StringBuilder result = new StringBuilder(Resources.PrinterDefinitionWithFilter);

            result.Replace("{0}", hostName);
            result.Replace("{1}", textBox_Model.Text);
            result.Replace("{2}", AppendOctet(ipPrefix, ref octetValue));

            return result.ToString();
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.SetIconAlignment(label_VPrintHost, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet1, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet2, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet3, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Octet4, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(textBox_Count, ErrorIconAlignment.MiddleRight);
            _errorProvider.SetIconAlignment(label_Destination, ErrorIconAlignment.MiddleRight);
        }

        private void button_ChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                label_FilePath.Text = dialog.SelectedPath;
            }
        }

        private bool ValidateFilePath()
        {
            bool result = ! string.IsNullOrEmpty(label_FilePath.Text);

            if (string.IsNullOrEmpty(label_FilePath.Text))
            {
                _errorProvider.SetError(label_Destination, "Must select a file destination.");
                result = false;
            }
            else
            {
                _errorProvider.SetError(label_Destination, string.Empty);
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

    }
}
