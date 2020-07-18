using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Settings;
using System.Globalization;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Form that contains information on all registry changes.
    /// </summary>
    public partial class RegistryInformationForm : Form
    {
        private RegistryAnalyzerDictionary _analyzers = null;
        private RegistrySnapshotDictionary _snapshots = null;
        private RegistrySizeDictionary _sizes = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInformationForm"/> class.
        /// </summary>
        /// <param name="analyzers">The analyzers.</param>
        /// <param name="snapshots">The snapshots.</param>
        /// <param name="sizes">The sizes.</param>
        public RegistryInformationForm(RegistryAnalyzerDictionary analyzers, RegistrySnapshotDictionary snapshots, RegistrySizeDictionary sizes)
        {
            if (snapshots == null)
            {
                throw new ArgumentNullException("snapshots");
            }

            InitializeComponent();
            _analyzers = analyzers;
            _snapshots = snapshots;
            _sizes = sizes;

            noData_Label.Visible = (_snapshots.Keys.Count == 0);
        }

        private void RegistryInformation_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            int startSize = _sizes.InitialSize;
            registryStartSizeValue_Label.Text = startSize.ToString(CultureInfo.InvariantCulture);

            Cursor = Cursors.WaitCursor;
            foreach (RegistryAnalyzer analyzer in _analyzers.Values)
            {
                string key = analyzer.Key;

                _sizes[key][1] = analyzer.GetRegistrySize(
                    GlobalSettings.Items[Setting.DomainAdminUserName],
                    GlobalSettings.Items[Setting.DomainAdminPassword]);

                if (_snapshots.ContainsKey(key))
                {
                    _snapshots[key][1] = analyzer.TakeSnapshot();
                }
            }
            Cursor = Cursors.Default;

            int endSize = _sizes.FinalSize;

            int sizeChange = endSize - startSize;
            registrySizeChangeValue_Label.Text = sizeChange.ToString(CultureInfo.InvariantCulture);
            if (sizeChange > 0)
            {
                registrySizeChangeValue_Label.ForeColor = Color.Red;
            }
            registryEndSizeValue_Label.Text = endSize.ToString(CultureInfo.InvariantCulture);

            registryPath_DataGridView.DataSource = _snapshots.RegistryChanges;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void registryPath_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    var data = registryPath_DataGridView.SelectedRows[0].DataBoundItem as RegistryPathSnapshotData;
            //    registryKey_DataGridView.DataSource = null;
            //    registryKey_DataGridView.DataSource = data.SubKeys;
            //}
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void save_Button_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "txt";
                dialog.Filter = "CSV Files(*.csv)|*.csv";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.OverwritePrompt = true;
                dialog.Title = "Save";
                dialog.ValidateNames = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                string destination = dialog.FileName;
                if (!Directory.Exists(Path.GetDirectoryName(destination)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                }

                File.WriteAllText(destination, _snapshots.RegistryChangesCsv);

                Cursor = Cursors.Default;
            }
        }

        private void registryPath_DataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (registryPath_DataGridView.SelectedRows.Count > 0 && e.RowIndex >= 0)
            {
                var data = registryPath_DataGridView.SelectedRows[0].DataBoundItem as RegistryPathSnapshotData;
                registryKey_DataGridView.DataSource = null;
                registryKey_DataGridView.DataSource = data.Subkeys;
            }
        }

        private void refresh_Button_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void registryEndSizeValue_Label_TextChanged(object sender, EventArgs e)
        {

        }

        private void registryStartSizeValue_Label_TextChanged(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Used for Grid data binding
    /// </summary>
    public class RegistryPathSnapshotData
    {
        private SortableBindingList<RegistryKeySnapshotData> _keyData;

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets the sub keys.
        /// </summary>
        public SortableBindingList<RegistryKeySnapshotData> Subkeys
        {
            get { return _keyData; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryPathSnapshotData"/> class.
        /// </summary>
        public RegistryPathSnapshotData()
        {
            _keyData = new SortableBindingList<RegistryKeySnapshotData>();
        }
    }

    /// <summary>
    /// Used for Grid data binding
    /// </summary>
    public class RegistryKeySnapshotData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        public string Kind { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }

    /// <summary>
    /// Contains the Registry analyzers for each Hive and Path
    /// </summary>
    [Serializable]
    public class RegistryAnalyzerDictionary : Dictionary<string, RegistryAnalyzer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryAnalyzerDictionary"/> class.
        /// </summary>
        public RegistryAnalyzerDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryAnalyzerDictionary"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected RegistryAnalyzerDictionary(SerializationInfo info, StreamingContext context)
            :base (info, context)
        { 
        }
    }

    /// <summary>
    /// Contains the Registry Sizes for before and after values
    /// </summary>
    [Serializable]
    public class RegistrySizeDictionary : Dictionary<string, int[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySizeDictionary"/> class.
        /// </summary>
        public RegistrySizeDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySizeDictionary"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected RegistrySizeDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }

        /// <summary>
        /// Gets the initial size.
        /// </summary>
        public int InitialSize
        {
            get { return CalculateSize(0); }
        }

        /// <summary>
        /// Gets the final size.
        /// </summary>
        public int FinalSize
        {
            get { return CalculateSize(1); }
        }

        private int CalculateSize(int index)
        {
            int size = 0;
            foreach (int[] sizes in this.Values)
            {
                size += sizes[index];
            }

            return size;
        }
    }

    /// <summary>
    /// Used to format the data for DataGridView binding
    /// </summary>
    [Serializable]
    public class RegistrySnapshotDictionary : Dictionary<string, RegistrySnapshot[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotDictionary"/> class.
        /// </summary>
        public RegistrySnapshotDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrySnapshotDictionary"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected RegistrySnapshotDictionary(SerializationInfo info, StreamingContext context)
            : base (info, context)
        { 
        }

        /// <summary>
        /// Gets the registry changes.
        /// </summary>
        public SortableBindingList<RegistryPathSnapshotData> RegistryChanges
        {
            get
            {
                var pathList = new SortableBindingList<RegistryPathSnapshotData>();

                foreach (RegistrySnapshot[] items in this.Values)
                {
                    RegistrySnapshot changes = items[0].CompareTo(items[1]);

                    foreach (string key in changes.Keys)
                    {
                        RegistryPathSnapshotData pathData = new RegistryPathSnapshotData();
                        pathData.Path = @"{0}\{1}".FormatWith(changes.Hive, key);
                        pathData.State = changes[key].State.ToString();
                        pathList.Add(pathData);

                        foreach (RegistrySnapshotKey item in changes[key].Entries)
                        {
                            RegistryKeySnapshotData data = new RegistryKeySnapshotData();
                            data.Name = item.Name;
                            data.Kind = item.Kind.ToString();
                            data.Value = item.GetValue();
                            pathData.Subkeys.Add(data);
                        }
                    }
                }

                return pathList;
            }
        }

        /// <summary>
        /// Gets the registry changes in CSV.
        /// </summary>
        public string RegistryChangesCsv
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                bool includeHeader = true;
                foreach (RegistrySnapshot[] items in this.Values)
                {
                    RegistrySnapshot changes = items[0].CompareTo(items[1]);
                    builder.Append(changes.ToStringCsv(includeHeader));
                    includeHeader = false;
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether final is null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if final is null; otherwise, <c>false</c>.
        /// </value>
        public bool FinalIsNull
        {
            get
            {
                bool isNull = false;
                foreach (RegistrySnapshot[] items in this.Values)
                {
                    if (items[1] == null)
                    {
                        isNull = true;
                        break;
                    }
                }
                return isNull;
            }
        }
    }
}
