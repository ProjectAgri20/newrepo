using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    public partial class EditScriptForm : Form
    {
        private GFriendFile _targetFile;
        public EditScriptForm()
        {
            InitializeComponent();
        }

        public EditScriptForm(GFriendFile file)
        {
            InitializeComponent();
            _targetFile = file;
            Refresh();
        }

        public override void Refresh()
        {
            name_TextBox.Text = _targetFile.FileName;
            typeValue_Label.Text =  _targetFile.FileType.ToString();
            fileText_richTextBox.Text = _targetFile.FileContents;
            base.Refresh();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Ok_Button_Click(object sender, EventArgs e)
        {
            _targetFile.FileContents = fileText_richTextBox.Text;
            Close();
        }
    }
}
