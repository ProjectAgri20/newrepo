using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    public partial class HpkInstallControl : UserControl, IGetSetComponentData
    {
        public HPKInstallData _hpkInstallList;
        List<HpkInstallTableData> _viewList;
        public HpkFileInfo openedHpkFileInfo;
        public EventHandler ControlComponentChanged;
        private HpkInstallTableData _selectedRow;
        private HpkFileInfo _SelectedHpkInstall;
        public bool Modified;
        public HpkInstallControl()
        {
            InitializeComponent();
            _hpkInstallList = new HPKInstallData();
            _viewList = new List<HpkInstallTableData>();
            hpkInstallTableDataBindingSource.ListChanged += BindInstallHpkGrid;
            hpkInstallGridView.CellClick += PopulateHpkInstallOptions;
            AddEventHandlers();
        }

        private void BindInstallHpkGrid(object sender, EventArgs e)
        {
            hpkInstallTableDataBindingSource.DataSource = _viewList;
            hpkInstallGridView.Update();
            hpkInstallGridView.Refresh();
        }

        public IComponentData GetData()
        {
            return _hpkInstallList;
        }

        private void PopulateHpkInstallOptions(object sender, EventArgs e)
        {
            var row = hpkInstallGridView.CurrentRow;
            if (row == null)
            {
                return;
            }
            var name = row.Cells[0].Value.ToString();
            var uuid = row.Cells[1].Value.ToString();
            var path = row.Cells[2].Value.ToString();
            _selectedRow = _viewList.FirstOrDefault(x => x.PackageName == name && x.Uuid == uuid && x.FilePath == path);
            _SelectedHpkInstall = _hpkInstallList.InstallFileList.FirstOrDefault(x => x.PackageName == name && x.Uuid == uuid && x.FilePath == path);
         }

        public void SetData()
        {
            //TODO
            //Depending on what type of data we have set the different parameters
            //Unneeded, we have default data and then change on user interaction --Maybe selected the first item if any are available
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _hpkInstallList = list.OfType<HPKInstallData>().FirstOrDefault();

            foreach (var item in _hpkInstallList.InstallFileList)
            {
                _viewList.Add(new HpkInstallTableData(item.PackageName, item.Uuid, item.FilePath));
            }
            hpkInstallTableDataBindingSource.DataSource = _viewList;
            validate_configurationData();
            retryCount_numericUpDown.Value = _hpkInstallList.RetryCount;
            retry_CheckBox.Checked = _hpkInstallList.RetryCheck;
            skip_CheckBox.Checked = _hpkInstallList.SkipCheck;
            AddEventHandlers();
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        private void AddEventHandlers()
        {
            hpkInstallTableDataBindingSource.CurrentChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            hpkInstallTableDataBindingSource.CurrentChanged -= OnControlComponentChanged;
        }

        private void open_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HPK file | *.hpk";
            ofd.Title = "Select HPK file";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                openedHpkFileInfo = new HpkFileInfo(ofd);
                field1_Textbox.Text = openedHpkFileInfo.PackageName;
                field2_Textbox.Text = openedHpkFileInfo.Uuid;
                field3_Textbox.Text = openedHpkFileInfo.FilePath;
            }
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(field1_Textbox.Text) && !string.IsNullOrWhiteSpace(field2_Textbox.Text) && !string.IsNullOrWhiteSpace(field3_Textbox.Text))
            {
                if (openedHpkFileInfo != null)
                {
                    _hpkInstallList.InstallFileList.Add(openedHpkFileInfo);
                    _viewList.Add(new HpkInstallTableData(openedHpkFileInfo.PackageName, openedHpkFileInfo.Uuid, openedHpkFileInfo.FilePath));
                    field1_Textbox.Clear();
                    field2_Textbox.Clear();
                    field3_Textbox.Clear();
                }
            }
            hpkInstallTableDataBindingSource.ResetBindings(false);
            validate_configurationData();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (_SelectedHpkInstall == null || _selectedRow == null)
            {
                return;
            }

            string name = string.Empty;
            string uuid = string.Empty;
            string path = string.Empty;
            var row = hpkInstallGridView.CurrentRow;
            if (row == null)
            {
                return;
            }

            name = row.Cells[0].Value.ToString();
            uuid = row.Cells[1].Value.ToString();
            path = row.Cells[2].Value.ToString();

            _SelectedHpkInstall = _hpkInstallList.InstallFileList.FirstOrDefault(x => x.PackageName == name && x.Uuid == uuid && x.FilePath == path);
            _viewList.Remove(_viewList.FirstOrDefault(x => x.PackageName == _SelectedHpkInstall.PackageName && x.Uuid == _SelectedHpkInstall.Uuid && x.FilePath == _SelectedHpkInstall.FilePath));
            _hpkInstallList.InstallFileList.Remove(_SelectedHpkInstall);
            _SelectedHpkInstall = null;
            _selectedRow = null;

            hpkInstallTableDataBindingSource.ResetBindings(false);
            validate_configurationData();
        }

        private void validate_configurationData()
        {
            if (hpkInstallTableDataBindingSource.Count < 1)
            {
                skip_CheckBox.Enabled = false;
                skip_CheckBox.Checked = false;

                retry_CheckBox.Enabled = false;
                retry_CheckBox.Checked = false;
                
                retryCount_numericUpDown.Value = 1;
                retryCount_numericUpDown.Enabled = false;
            }
            else
            {
                skip_CheckBox.Enabled = true;
                retry_CheckBox.Enabled = true;
                retryCount_numericUpDown.Enabled = true;
            }
        }

        private void retry_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (retry_CheckBox.Checked == true)
            {
                _hpkInstallList.RetryCheck = true;
                retryCount_numericUpDown.Enabled = true;
            }
            else
            {
                _hpkInstallList.RetryCheck = false;
                retryCount_numericUpDown.Enabled = false;
            }
        }

        private void skip_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (skip_CheckBox.Checked == true)
            {
                _hpkInstallList.SkipCheck = true;
            }
            else
            {
                _hpkInstallList.SkipCheck = false;
            }
        }

        private void retryCount_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _hpkInstallList.RetryCount = retryCount_numericUpDown.Value;
        }
    }
}
