using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    /// <summary>
    /// For redirecting console output of GFriend to Exeuction control's RichTextbox
    /// </summary>
    public class OutputWriter : TextWriter
    {
        private RichTextBox _textBox;

        /// <summary>
        /// Initialize OutputWriter
        /// </summary>
        /// <param name="richTextBox">richTextBox for append console output</param>
        public OutputWriter(RichTextBox richTextBox)
        {
            _textBox = richTextBox;
        }

        /// <summary>
        /// Encoding of the stream. Default is UTF8.
        /// </summary>
        public override Encoding Encoding => Encoding.UTF8;

        /// <summary>
        /// Overrided Write. Add time stamp in case of Write.
        /// </summary>
        /// <param name="value">value to write</param>
        public override void Write(string value)
        {
            ExecutionServices.SystemTrace.LogNotice("Status = " + value);
            //Hangs if this is not async
            _textBox.AsyncInvokeIfRequired(n =>
            {
                //System trace log must be included here otherwise we can get a hang.  Note that async does wait for the return, it just doesn't block the UI thread
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {value}");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }

        /// <summary>
        /// Overried WriteLine. Override without timestamp because GFriend write lines with result (Pass, Fail or Error) just besides statements.
        /// If it adds timestamp here, output is not comfortable to read.
        /// </summary>
        /// <param name="value">value to write</param>
        public override void WriteLine(string value)
        {
            ExecutionServices.SystemTrace.LogNotice("Status = " + value);
            //Hangs if this is not async
            _textBox.AsyncInvokeIfRequired(n =>
            {
                n.AppendText(value + Environment.NewLine);
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }
    }
}
