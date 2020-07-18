using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.UI;
using System.Text.RegularExpressions;
using HP.ScalableTest.Framework.Settings;
using System.Linq;
using System.Collections.Generic;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Class AssetPoolEditForm
    /// </summary>

    public partial class AssetPoolEditForm : Form
    {
        private AssetPool _pool = null;
        private AssetInventoryContext _context = null;
        private ErrorProvider _provider = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPoolEditForm"/>
        /// </summary>
        /// <param name="pool">The account pool</param>
        /// <param name="context">The context</param>
        public AssetPoolEditForm(AssetPool pool, AssetInventoryContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            ShowIcon = true;
            _provider = new ErrorProvider();
            _provider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _provider.SetIconAlignment(poolNameTextBox, ErrorIconAlignment.MiddleLeft);
            _provider.SetIconAlignment(administratorComboBox, ErrorIconAlignment.MiddleLeft);
            _pool = pool;
            _context = context;
        }

        private void AssetPoolEditForm_Load(object sender, EventArgs e)
        {
            List<string> keys = new List<string> { "Select..." };
            keys.AddRange(_context.AssetPools.Select(n => n.Administrator).Distinct());

            poolNameTextBox.Text = _pool.Name;
            administratorComboBox.DataSource = keys;
            administratorComboBox.DisplayMember = string.IsNullOrEmpty(_pool.Administrator) ? "Select..." : _pool.Administrator;
            administratorComboBox.SelectedItem = string.IsNullOrEmpty(_pool.Administrator) ? "Select..." : _pool.Administrator;
            administratorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            reservation_Checkbox.Checked = _pool.TrackReservations == null ? false : _pool.TrackReservations == true ? true : false;

            if (!string.IsNullOrEmpty(_pool.Name))
            {
                poolNameTextBox.Enabled = false;
            }
            poolNameTextBox.Validating += PoolNameTextBox_Validating;
            administratorComboBox.Validating += AdministratorTextBox_Validating;

        }


        private bool Valid()
        {
            var temp = string.IsNullOrEmpty(_provider.GetError(administratorComboBox));
            var temp2 = string.IsNullOrEmpty(_provider.GetError(poolNameTextBox));

            return temp && temp2;
        }


        void AdministratorTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (string.IsNullOrEmpty(administratorTextBox.Text) || string.IsNullOrWhiteSpace(administratorTextBox.Text))
            //{
            //    _provider.SetError(administratorTextBox, "A Group Administrator is required");
            //}
            //else
            //{
            //    var temp = _context.Users.Where(x => x.UserId == administratorTextBox.Text);


            //    if (temp.Count() == 0)
            //    {
            //        _provider.SetError(administratorTextBox, "Please add the administrator to the set of Authorized STB Users");
            //    }
            //    else
            //    {
            //        _provider.SetError(administratorTextBox, "");
            //    }

            //}

            if ((string)administratorComboBox.SelectedValue == "Select...")
            {
                _provider.SetError(administratorComboBox, "Please add the administrator to the set of Authorized STB Users");
            }
            else
            {
                _provider.SetError(administratorComboBox, "");
            }



        }

        void PoolNameTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(poolNameTextBox.Text) || string.IsNullOrWhiteSpace(poolNameTextBox.Text))
            {
                _provider.SetError(poolNameTextBox, "An Asset Pool Name is Required");
            }
            else
            {
                _provider.SetError(poolNameTextBox, "");
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                DialogResult = DialogResult.OK;

                _pool.Name = poolNameTextBox.Text;
                _pool.Administrator = (string)administratorComboBox.SelectedItem;
                _pool.TrackReservations = reservation_Checkbox.Checked;
            }
            else
            {
                MessageBox.Show("Please check your entry", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
