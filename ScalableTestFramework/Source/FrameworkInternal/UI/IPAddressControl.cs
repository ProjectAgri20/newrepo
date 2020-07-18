using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// A control for entering a device or server IP address.
    /// </summary>
    [Designer(typeof(IPAddressControlDesigner))]
    public class IPAddressControl : Control
    {
        private const int _fieldCount = 4;
        private static readonly Size _fixed3DOffset = new Size(3, 3);

        private readonly FieldControl[] _fieldControls = new FieldControl[_fieldCount];
        private readonly DotControl[] _dotControls = new DotControl[_fieldCount - 1];
        private readonly TextBox _referenceTextBox = new TextBox();

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        [Browsable(false)]
        public override bool Focused => _fieldControls.Any(n => n.Focused);

        /// <summary>
        /// Gets the size that is the lower limit that <see cref="Control.GetPreferredSize(Size)" /> can specify.
        /// </summary>
        [Browsable(true)]
        public override Size MinimumSize
        {
            get
            {
                _referenceTextBox.Font = Font;
                int controlsWidth = Controls.Cast<Control>().Sum(n => n.MinimumSize.Width);
                int borderWidth = _fixed3DOffset.Width * 2;
                return new Size(controlsWidth + borderWidth, _referenceTextBox.Height);
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                if (_fieldControls.Any(n => !string.IsNullOrWhiteSpace(n.Text)))
                {
                    return string.Join(".", _fieldControls.Select(n => n.Text));
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                string[] values = (value ?? string.Empty).Split('.');
                for (int i = 0; i < _fieldCount; i++)
                {
                    if (i < values.Length)
                    {
                        _fieldControls[i].Text = values[i];
                    }
                    else
                    {
                        _fieldControls[i].Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressControl" /> class.
        /// </summary>
        public IPAddressControl()
        {
            for (int i = 0; i < _fieldCount; i++)
            {
                _fieldControls[i] = new FieldControl(i);
                _fieldControls[i].CreateControl();
                Controls.Add(_fieldControls[i]);

                _fieldControls[i].CedeFocus += OnCedeFocus;
                _fieldControls[i].TextChanged += (s, e) => OnTextChanged(e);

                if (i < _fieldCount - 1)
                {
                    _dotControls[i] = new DotControl();
                    _dotControls[i].CreateControl();
                    Controls.Add(_dotControls[i]);
                }
            }

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = SystemColors.Window;
            Cursor = Cursors.IBeam;
            Size = MinimumSize;

            _referenceTextBox.AutoSize = true;
        }

        /// <summary>
        /// Clears all text from the IP address control.
        /// </summary>
        public void Clear()
        {
            foreach (FieldControl control in _fieldControls)
            {
                control.Clear();
            }
        }

        /// <summary>
        /// Determines whether this control contains a valid IP address.
        /// </summary>
        /// <returns><c>true</c> if this control contains a valid IP address; otherwise, <c>false</c>.</returns>
        public bool IsValidIPAddress()
        {
            return IPAddress.TryParse(Text, out _);
        }

        private void AdjustSize()
        {
            Size = new Size(Math.Max(Width, MinimumSize.Width), MinimumSize.Height);
            LayoutControls();
        }

        private void LayoutControls()
        {
            SuspendLayout();

            int x = _fixed3DOffset.Width;
            int y = _fixed3DOffset.Height;
            int extraSpace = Width - MinimumSize.Width;

            for (int i = 0; i < _fieldCount; i++)
            {
                // Increase the width of the field control
                _fieldControls[i].Width = _fieldControls[i].MinimumSize.Width + extraSpace / _fieldCount;
                if (i < extraSpace % _fieldCount)
                {
                    // Handle pixels that don't divide evenly
                    _fieldControls[i].Width++;
                }

                // Move the field control to the appropriate spot
                _fieldControls[i].Location = new Point(x, y);
                x += _fieldControls[i].Width;
                if (i < _dotControls.Length)
                {
                    _dotControls[i].Location = new Point(x, y);
                    x += _dotControls[i].Width;
                }
            }

            ResumeLayout(false);
        }

        private void OnCedeFocus(object sender, CedeFocusEventArgs e)
        {
            switch (e.Action)
            {
                case Action.Home:
                    _fieldControls[0].Focus(CursorPosition.Start);
                    break;

                case Action.End:
                    _fieldControls[_fieldCount - 1].Focus(CursorPosition.End);
                    break;

                case Action.Trim:
                    if (e.FieldIndex > 0)
                    {
                        _fieldControls[e.FieldIndex - 1].Trim();
                        _fieldControls[e.FieldIndex - 1].Focus(CursorPosition.End);
                    }
                    break;

                case Action.MoveForward:
                    if (e.FieldIndex < _fieldCount - 1)
                    {
                        _fieldControls[e.FieldIndex + 1].Focus(e.SelectAll ? CursorPosition.SelectAll : CursorPosition.Start);
                    }
                    break;

                case Action.MoveReverse:
                    if (e.FieldIndex > 0)
                    {
                        _fieldControls[e.FieldIndex - 1].Focus(e.SelectAll ? CursorPosition.SelectAll : CursorPosition.End);
                    }
                    break;
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e != null)
            {
                Color backColor = Enabled ? BackColor : SystemColors.Control;
                using (SolidBrush backgroundBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
                }

                if (Application.RenderWithVisualStyles)
                {
                    Rectangle border = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                    ControlPaint.DrawVisualStyleBorder(e.Graphics, border);
                }
                else
                {
                    ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.FontChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            AdjustSize();
        }

        /// <summary>
        /// Raises the <see cref="Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustSize();
        }

        /// <summary>
        /// Raises the <see cref="Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _fieldControls[0].Focus(CursorPosition.SelectAll);
        }

        /// <summary>
        /// Raises the <see cref="Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (!Focused)
            {
                base.OnLostFocus(e);
            }
        }

        private enum Action
        {
            Home,
            End,
            Trim,
            MoveForward,
            MoveReverse
        }

        private enum CursorPosition
        {
            Start,
            End,
            SelectAll
        }

        private class FieldControl : TextBox
        {
            private const int _minimumValue = 0;
            private const int _maximumValue = 255;
            private const TextFormatFlags _textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

            private readonly int _fieldIndex;

            public event EventHandler<CedeFocusEventArgs> CedeFocus;

            public override Size MinimumSize
            {
                get
                {
                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        return TextRenderer.MeasureText(g, "333", Font, Size, _textFormatFlags);
                    }
                }
            }

            public FieldControl(int fieldIndex)
            {
                _fieldIndex = fieldIndex;

                BorderStyle = BorderStyle.None;
                Size = MinimumSize;
                TabStop = false;
                TextAlign = HorizontalAlignment.Center;
            }

            public void Focus(CursorPosition position)
            {
                Focus();

                switch (position)
                {
                    case CursorPosition.Start:
                        SelectionStart = 0;
                        SelectionLength = 0;
                        break;

                    case CursorPosition.End:
                        SelectionStart = TextLength;
                        SelectionLength = 0;
                        break;

                    case CursorPosition.SelectAll:
                        SelectionStart = 0;
                        SelectionLength = TextLength;
                        break;
                }
            }

            public void Trim()
            {
                if (TextLength > 0)
                {
                    Text = Text.Remove(TextLength - 1);
                }
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);

                if (e != null)
                {
                    bool empty = (TextLength == 0);
                    bool cursorAtStart = (SelectionStart == 0 && SelectionLength == 0);
                    bool cursorAtEnd = (SelectionLength == 0 && SelectionStart == TextLength);

                    switch (e.KeyCode)
                    {
                        case Keys.Home:
                            SendCedeFocusEvent(Action.Home);
                            break;

                        case Keys.End:
                            SendCedeFocusEvent(Action.End);
                            break;

                        case Keys.Back:
                            if (cursorAtStart)
                            {
                                SendCedeFocusEvent(Action.Trim);
                            }
                            break;

                        case Keys.Space:
                        case Keys.Decimal:
                        case Keys.OemPeriod:
                            if (cursorAtEnd && !empty)
                            {
                                SendCedeFocusEvent(Action.MoveForward, true);
                                e.SuppressKeyPress = true;
                            }
                            break;

                        case Keys.Right:
                        case Keys.Down:
                            if (e.Control)
                            {
                                SendCedeFocusEvent(Action.MoveForward, true);
                            }
                            else if (cursorAtEnd)
                            {
                                SendCedeFocusEvent(Action.MoveForward, false);
                            }
                            break;

                        case Keys.Left:
                        case Keys.Up:
                            if (e.Control)
                            {
                                SendCedeFocusEvent(Action.MoveReverse, true);
                            }
                            else if (cursorAtStart)
                            {
                                SendCedeFocusEvent(Action.MoveReverse, false);
                            }
                            break;
                    }
                }
            }

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                base.OnKeyPress(e);
                if (e != null)
                {
                    // Only allow digits and "control" characters (e.g. delete, arrow keys)
                    e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                }
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);

                if (int.TryParse(Text, out int value))
                {
                    if (value > _maximumValue)
                    {
                        Text = _maximumValue.ToString();
                        SelectionStart = 0;
                    }
                    else if (value < _minimumValue)
                    {
                        Text = _minimumValue.ToString();
                        SelectionStart = 0;
                    }
                    else
                    {
                        // Truncate leading zeros
                        int originalLength = TextLength;
                        int originalSelectionStart = SelectionStart;
                        Text = value.ToString();
                        SelectionStart = originalSelectionStart - Math.Max(0, originalLength - TextLength);
                    }
                }
                else
                {
                    Clear();
                }

                // Automatically move to the next box if adding another digit would make it too large
                if (Focused && value > _maximumValue / 10)
                {
                    SendCedeFocusEvent(Action.MoveForward, true);
                }
            }

            protected override void OnParentBackColorChanged(EventArgs e)
            {
                base.OnParentBackColorChanged(e);
                BackColor = Parent.BackColor;
            }

            protected override void OnParentForeColorChanged(EventArgs e)
            {
                base.OnParentForeColorChanged(e);
                ForeColor = Parent.ForeColor;
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0x007b:  // WM_CONTEXTMENU
                        return;
                }

                base.WndProc(ref m);
            }

            private void SendCedeFocusEvent(Action action, bool selectAll = false)
            {
                CedeFocus?.Invoke(this, new CedeFocusEventArgs(_fieldIndex, action, selectAll));
            }
        }

        private class DotControl : Control
        {
            private const TextFormatFlags _textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;

            public override Size MinimumSize
            {
                get
                {
                    using (Graphics g = Graphics.FromHwnd(Handle))
                    {
                        return TextRenderer.MeasureText(g, Text, Font, Size, _textFormatFlags);
                    }
                }
            }

            public DotControl()
            {
                Text = ".";
                Size = MinimumSize;
                TabStop = false;

                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (e != null)
                {
                    Color backColor = Enabled ? BackColor : SystemColors.Control;
                    Color textColor = Enabled ? ForeColor : SystemColors.GrayText;

                    using (SolidBrush backgroundBrush = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
                    }

                    TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, textColor, _textFormatFlags);
                }
            }

            protected override void OnFontChanged(EventArgs e)
            {
                base.OnFontChanged(e);
                Size = MinimumSize;
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                Size = MinimumSize;
            }

            protected override void OnParentBackColorChanged(EventArgs e)
            {
                base.OnParentBackColorChanged(e);
                BackColor = Parent.BackColor;
            }

            protected override void OnParentForeColorChanged(EventArgs e)
            {
                base.OnParentForeColorChanged(e);
                ForeColor = Parent.ForeColor;
            }
        }

        private class CedeFocusEventArgs : EventArgs
        {
            public int FieldIndex { get; }
            public Action Action { get; }
            public bool SelectAll { get; }

            public CedeFocusEventArgs(int fieldIndex, Action action, bool selectAll)
            {
                FieldIndex = fieldIndex;
                Action = action;
                SelectAll = selectAll;
            }
        }

        private class IPAddressControlDesigner : ControlDesigner
        {
            public override SelectionRules SelectionRules => SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.LeftSizeable | SelectionRules.RightSizeable;

            public override IList SnapLines
            {
                get
                {
                    IList snapLines = base.SnapLines;
                    int baseLine = GetTextAscent() + _fixed3DOffset.Height + 1;
                    snapLines.Add(new SnapLine(SnapLineType.Baseline, baseLine));
                    return snapLines;
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
            private int GetTextAscent()
            {
                IntPtr hdc = NativeMethods.GetWindowDC(Control.Handle);

                NativeMethods.TextMetric textMetric;
                IntPtr hFont = Control.Font.ToHfont();

                try
                {
                    IntPtr hFontPrevious = NativeMethods.SelectObject(hdc, hFont);
                    NativeMethods.GetTextMetrics(hdc, out textMetric);
                    NativeMethods.SelectObject(hdc, hFontPrevious);
                }
                finally
                {
                    NativeMethods.ReleaseDC(Control.Handle, hdc);
                    NativeMethods.DeleteObject(hFont);
                }
                return textMetric.Ascent;
            }

            private static class NativeMethods
            {
                [DllImport("user32")]
                internal static extern IntPtr GetWindowDC(IntPtr hWnd);

                [DllImport("user32")]
                internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hdc);

                [DllImport("gdi32")]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool GetTextMetrics(IntPtr hdc, out TextMetric lptm);

                [DllImport("gdi32")]
                internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

                [DllImport("gdi32")]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool DeleteObject(IntPtr hdc);

                [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
                internal struct TextMetric
                {
                    public int Height;
                    public int Ascent;
                    public int Descent;
                    public int InternalLeading;
                    public int ExternalLeading;
                    public int AveCharWidth;
                    public int MaxCharWidth;
                    public int Weight;
                    public int Overhang;
                    public int DigitizedAspectX;
                    public int DigitizedAspectY;
                    public char FirstChar;
                    public char LastChar;
                    public char DefaultChar;
                    public char BreakChar;
                    public byte Italic;
                    public byte Underlined;
                    public byte StruckOut;
                    public byte PitchAndFamily;
                    public byte TextMetricCharSet;
                }
            }
        }
    }
}
