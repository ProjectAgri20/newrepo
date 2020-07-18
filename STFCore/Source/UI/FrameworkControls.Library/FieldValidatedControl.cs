using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Base class for controls that provides common validation methods.
    /// </summary>
    [Serializable]
    public class FieldValidatedControl : UserControl
    {
        /// <summary>
        /// Error provider for this control.  Available for inheriting classes to set custom errors.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Implementing a protected property and a private member causes designer errors in the UI. "
                          + "This appears to be a deficiency in the VS2010 designer.")]
        private ErrorProvider _fieldValidator = new ErrorProvider();
        /// <summary>
        /// Field Validator
        /// </summary>
        protected ErrorProvider fieldValidator
        {
            get { return _fieldValidator; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValidatedControl"/> class.
        /// </summary>
        public FieldValidatedControl()
        {
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            //fieldValidator = new ErrorProvider();
            _fieldValidator.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        /// <summary>
        /// Gets a list of errors present in this control and all it's children.
        /// </summary>
        public string ErrorList
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (Control child in Controls)
                {
                    AddErrorsToList(child, builder);
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Recursive method that gets the errors on this control and all it's children.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="builder">The string builder that accumulates the error list.</param>
        private void AddErrorsToList(Control control, StringBuilder builder)
        {
            FieldValidatedControl fieldValidatedControl = control as FieldValidatedControl;
            if (fieldValidatedControl != null)
            {
                builder.Append(fieldValidatedControl.ErrorList);
            }
            else
            {
                // Add any errors present in the control
                string controlError = fieldValidator.GetError(control);
                if (!string.IsNullOrEmpty(controlError))
                {
                    builder.AppendLine(controlError);
                }

                // Recurse into this function for any children of the control
                if (control.HasChildren)
                {
                    foreach (Control child in control.Controls)
                    {
                        AddErrorsToList(child, builder);
                    }
                }
            }
        }

        #region Validation Methods

        /// <summary>
        /// Validates the data to first check for an integer and then verify it is within the defined range..
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="lowValue">The low value in the range</param>
        /// <param name="highValue">The high value in the range</param>
        protected void ValidateIntRange(string data, string fieldName, Control control, CancelEventArgs e, int lowValue, int highValue)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            ValidateInt(data, fieldName, control, e);

            string errorMessage = null;

            if (!e.Cancel)
            {
                int value = int.Parse(data, CultureInfo.InvariantCulture);

                if (value < lowValue || value > highValue)
                {
                    errorMessage = "{0} is outside the range {1} to {2}".FormatWith(fieldName, lowValue, highValue);
                }

                fieldValidator.SetError(control, errorMessage);
            }

            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Validates data to check for an integer.  Sets error condition if data is non-numeric.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        protected void ValidateInt(string data, string fieldName, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            int value = 0;
            string errorMessage = null;

            if (string.IsNullOrEmpty(data))
            {
                errorMessage = fieldName + " must have a value.";
            }
            else if (!int.TryParse(data, out value))
            {
                errorMessage = fieldName + " must be an integer value.";
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Validates numerical data to check for a positive integer.  Sets error condition
        /// if data is zero, negative, or non-numeric.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        public void ValidatePositiveInt(string data, string fieldName, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            ValidateInt(data, fieldName, control, e);
            if (!e.Cancel)
            {
                string errorMessage = null;
                if (Int32.Parse(data, CultureInfo.InvariantCulture) < 1)
                {
                    errorMessage = fieldName + " must be greater than 0.";
                }

                fieldValidator.SetError(control, errorMessage);
                e.Cancel = errorMessage != null;
            }
        }

        /// <summary>
        /// Validates numerical data to check for a non-negative integer.  Sets error condition
        /// if data is negative, or non-numeric.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        public void ValidateNonnegativeInt(string data, string fieldName, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            ValidateInt(data, fieldName, control, e);
            if (!e.Cancel)
            {
                string errorMessage = null;
                if (Int32.Parse(data, CultureInfo.InvariantCulture) < 0)
                {
                    errorMessage = fieldName + " cannot be negative.";
                }

                fieldValidator.SetError(control, errorMessage);
                e.Cancel = errorMessage != null;
            }
        }

        /// <summary>
        /// Validates string data to check for presence of a value.  Sets error condition
        /// if string is null or empty.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        public void HasValue(string data, string fieldName, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            string errorMessage = null;

            if (string.IsNullOrWhiteSpace(data))
            {
                errorMessage = fieldName + " must have a value.";
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Checks for Value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="control"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void HasValue(string data, Control control, CancelEventArgs e, string message)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            string errorMessage = null;

            if (string.IsNullOrWhiteSpace(data))
            {
                errorMessage = message;
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Similar to HasValue but with a better error message for combo boxes.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="selectionName">The name of the field being validated (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        public void HasSelected(string data, string selectionName, Control control, CancelEventArgs e)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            string errorMessage = null;
            if (string.IsNullOrEmpty(data))
            {
                errorMessage = selectionName + " must be selected.";
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Similar to HasValue but with a better error message for combo boxes.
        /// </summary>
        /// <param name="comboBox">The combo box being validated.</param>
        /// <param name="label">The label control associated with the combo box.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs" /> instance containing the event data.</param>
        public void HasSelected(Control comboBox, Control label, CancelEventArgs e)
        {
            string data = comboBox.Text;
            string selectionName = label.Text;
            if (selectionName.EndsWith(":"))
            {
                selectionName = selectionName.TrimEnd(':');
            }
            HasSelected(data, selectionName, comboBox, e);
        }

        /// <summary>
        /// Validates string data against a regular expression.  Sets error condition
        /// if string does not match regular expression.
        /// </summary>
        /// <param name="data">The string data to validate.</param>
        /// <param name="fieldName">The name of the field being validated (shown in error message).</param>
        /// <param name="pattern">The regular expression to test against.</param>
        /// <param name="prettyPattern">The "pretty" version of the display pattern (shown in error message).</param>
        /// <param name="control">The control to set errors on.</param>
        /// <param name="e">The CancelEventArgs from the validating event.</param>
        public void ValidateRegex(string data, string fieldName, string pattern, string prettyPattern, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            string errorMessage = null;
            if (!Regex.Match(data, pattern).Success)
            {
                errorMessage = fieldName + " must have the pattern " + prettyPattern;
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        /// <summary>
        /// Checks to see if a certain condition is true.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        /// <param name="selectionName">Selection name.</param>
        /// <param name="control">Control to mark if this fails.</param>
        /// <param name="e"></param>
        public void IsTrue(bool condition, string selectionName, Control control, CancelEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (selectionName == null)
            {
                throw new ArgumentNullException("selectionName");
            }

            string errorMessage = null;
            if (!condition)
            {
                errorMessage = "Failed to validate setting: " + selectionName;
            }

            fieldValidator.SetError(control, errorMessage);
            e.Cancel = errorMessage != null;
        }

        #endregion

    }
}
