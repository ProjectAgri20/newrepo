using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Form that allows quick entry of asset IDs.
    /// </summary>
    public partial class AssetQuickEntryForm : Form
    {
        /// <summary>
        /// Gets the list of asset IDs entered into the form.
        /// </summary>
        public IEnumerable<string> EnteredAssetIds => assetIds_TextBox.Lines.Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n));

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetQuickEntryForm" /> class.
        /// </summary>
        private AssetQuickEntryForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetQuickEntryForm" /> class.
        /// </summary>
        /// <param name="assetIds">The list of asset IDs to prepopulate in the form.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetIds" /> is null.</exception>
        public AssetQuickEntryForm(IEnumerable<string> assetIds)
            : this()
        {
            if (assetIds == null)
            {
                throw new ArgumentNullException(nameof(assetIds));
            }

            assetIds_TextBox.Lines = assetIds.ToArray();
        }

        private void AssetQuickEntryForm_Load(object sender, EventArgs e)
        {
            assetIds_TextBox.SelectionStart = assetIds_TextBox.TextLength;
        }

        private void assetIds_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                assetIds_TextBox.SelectAll();
                e.Handled = true;
            }
        }
    }
}
