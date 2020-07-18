using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Base class for a combobox that displays information in multiple columns.
    /// </summary>
    public class MultipleColumnComboBox :  MTGCComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleColumnComboBox"/> class.
        /// </summary>
        public MultipleColumnComboBox()
        {
            ArrowBoxColor = SystemColors.Control;
            ArrowColor = Color.Black;
            BorderStyle = MTGCComboBox.TipiBordi.Fixed3D;
            CharacterCasing = CharacterCasing.Normal;
            DisabledArrowBoxColor = SystemColors.Control;
            DisabledArrowColor = Color.LightGray;
            DisabledBackColor = SystemColors.Control;
            DisabledBorderColor = SystemColors.InactiveBorder;
            DisabledForeColor = SystemColors.GrayText;
            DisplayMember = "Text";
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownArrowBackColor = Color.FromArgb(136, 169, 223);
            DropDownBackColor = Color.FromArgb(193, 210, 238);
            DropDownForeColor = Color.Black;
            DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDownList;
            GridLineColor = Color.LightGray;
            GridLineHorizontal = true;
            GridLineVertical = true;
            HighlightBorderColor = Color.Gray;
            HighlightBorderOnMouseEvents = true;
            LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            ManagingFastMouseMoving = true;
            ManagingFastMouseMovingInterval = 30;
            NormalBorderColor = Color.Black;
            SelectedItem = null;
            SelectedValue = null;
            TabIndex = 0;
            LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleColumnComboBox"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="columns">The columns.</param>
        public MultipleColumnComboBox(string name, Point location, Size size, Collection<int> columns)
            : this()
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            Name = name;
            Location = location;
            Size = size;
            DropDownWidth = size.Width;
            if (columns.Count > 4)
            {
                throw new ArgumentException("Column count must be four or less");
            }
            
            ColumnNum = columns.Count;
            ColumnWidth = string.Join(";", columns);
        }
    }
}
