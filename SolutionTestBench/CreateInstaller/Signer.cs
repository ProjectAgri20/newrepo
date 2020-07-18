using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CreateInstaller
{
    /// <summary>
    /// Class to encapulate the digital signing of files via HP SecureSign.
    /// </summary>
    internal class Signer : IProcessOutput
    {
        private string _filePathToSign = null;
        private string _fileName = null;
        private string _outputPath = null;
        private bool _processing = false;

        public event EventHandler<InstallEventArgs> OnMessageUpdate;

        /// <summary>
        /// Creates a new instance of the Signer class.
        /// </summary>
        /// <param name="filePathToSign"></param>
        /// <param name="outputPath"></param>
        public Signer(string filePathToSign, string outputPath)
        {
            _filePathToSign = filePathToSign;
            _fileName = Path.GetFileName(_filePathToSign);
            _outputPath = outputPath;
        }

        /// <summary>
        /// Returns a description of the file name being signed.
        /// </summary>
        public string Label { get { return $"SignHP {_fileName}"; } }

        /// <summary>
        /// Returns a configuration description suited for a file name.
        /// </summary>
        public string Configuration
        {
            get
            {
                StringBuilder result = new StringBuilder("Sign_");
                result.Append(_fileName.Substring(0, _fileName.LastIndexOf('.')));
                return result.ToString();
            }
        }

        /// <summary>
        /// Returns whether this instance is processing a sign request.
        /// </summary>
        public bool Processing
        {
            get { return _processing; }
        }

        /// <summary>
        /// Caches credential information for use with HP SecureSign.  Only needs to be done once per Windows session.
        /// </summary>
        public static void Authenticate()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                process.StartInfo.WorkingDirectory = "C:\\Program Files\\HPSecureSign";
                process.StartInfo.Arguments = @"/c ""kinit""";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.Start();
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Cancels the SecureSign process.
        /// </summary>
        public void Cancel()
        {
            _processing = false;
        }

        /// <summary>
        /// Executes the SecureSign process.
        /// </summary>
        public void Execute()
        {
            // Old Sign Tool
            //java -jar C:\Program Files\SignHPClient\SignHP\SignHPClient.jar sign -i C:\Work\STBBuilds\4.0\STBServer-Hpac-4.0.1_20160405.exe -p STB_SDK_Project -o C:\Work\STBBuilds\4.0
            // New Sign Tool
            //java -Xmx1024m -jar "D:\Program Files\HPSecureSign\SecureSign.jar" sign -p RDL_STB_SDK_PROJECT -i D:\Work\STBBuilds\4.8\Installers\STBServer-Base-4.8.9.0_20170814.exe -o D:\Work\STBBuilds\4.8\Installers

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "java.exe";
                process.StartInfo.Arguments = string.Format(Resource.SignArgs, _filePathToSign, _outputPath);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.OutputDataReceived += Process_DataReceived;
                process.ErrorDataReceived += Process_DataReceived;

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();

                _processing = false;
            }
        }

        private void Process_DataReceived(object sender, DataReceivedEventArgs e)
        {
            OnMessageUpdate(sender, new InstallEventArgs(e.Data));
        }

        /// <summary>
        /// Release resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            //Nothing to dispose
        }
    }
}
