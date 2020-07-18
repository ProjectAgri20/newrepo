using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Conditions that specify when <see cref="FieldValidator" /> should validate a control.
    /// </summary>
    public enum ValidationCondition
    {
        /// <summary>
        /// Always validate the control.
        /// </summary>
        Always = 0x0,

        /// <summary>
        /// Validate the control if it is enabled.
        /// </summary>
        IfEnabled = 0x1
    }

    /// <summary>
    /// Provides standardized methods for validating user input.
    /// </summary>
    public partial class FieldValidator : Component
    {
        private readonly ControlValidatorCollection _controlValidators = new ControlValidatorCollection();
        private readonly ControlIconProxyCollection _controlIconProxies = new ControlIconProxyCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValidator" /> class.
        /// </summary>
        public FieldValidator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValidator" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public FieldValidator(IContainer container)
            : this()
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        /// <summary>
        /// Validates all controls that have registered validation methods with this <see cref="FieldValidator" />.
        /// </summary>
        /// <returns>A <see cref="ValidationResult" /> object for each validated control.</returns>
        public IEnumerable<ValidationResult> ValidateAll()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            foreach (Control control in _controlValidators.Controls)
            {
                results.Add(Validate(control));
            }
            return results;
        }

        /// <summary>
        /// Validates the specified control according to the validation method registered with this <see cref="FieldValidator" />.
        /// </summary> 
        /// <param name="control">The control to validate.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="control" /> does not have a registered validation method.</exception>
        public ValidationResult Validate(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            ValidationResult result = _controlValidators[control].Invoke();
            SetError(control, result.Message);
            return result;
        }

        /// <summary>
        /// Removes the validation method for the specified control from this instance.
        /// </summary>
        /// <param name="control">The control to remove.</param>
        public void Remove(Control control)
        {
            SetError(control, null);
            _controlValidators.Remove(control);
        }

        /// <summary>
        /// Removes all registered validation methods from this instance.
        /// </summary>
        public void Clear()
        {
            foreach (Control control in _controlValidators.Controls)
            {
                SetError(control, null);
            }

            _controlValidators.Clear();
        }

        /// <summary>
        /// Sets an "icon proxy" for the specified control.  If the specified control fails validation,
        /// the error icon will be displayed on the proxy control instead.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="iconProxy">The icon proxy.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="control" /> is null.
        /// <para>or</para>
        /// <paramref name="iconProxy" /> is null.
        /// </exception>
        public void SetIconProxy(Control control, Control iconProxy)
        {
            _controlIconProxies.Register(control, iconProxy);
        }

        /// <summary>
        /// Sets the location where the error icon should be displayed in relation to the control.
        /// </summary>
        /// <param name="control">The control where the error icon should be placed.</param>
        /// <param name="value">The location on the control where the error icon should be displayed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control" /> is null.</exception>
        public void SetIconAlignment(Control control, ErrorIconAlignment value)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            errorProvider.SetIconAlignment(control, value);
        }

        private void SetError(Control control, string message)
        {
            errorProvider.SetError(_controlIconProxies[control], message);
        }

        #region RequireValue

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" /> has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, string fieldName)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, fieldName));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, string fieldName, ValidationCondition condition)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, fieldName, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, string fieldName, CheckBox checkBox)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, fieldName, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, string fieldName, RadioButton radioButton)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, fieldName, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" /> has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, Label label)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, label));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, Label label, ValidationCondition condition)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, label, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, Label label, CheckBox checkBox)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, label, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="TextBox" /> or <see cref="RichTextBox" />
        /// has a value if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public void RequireValue(TextBoxBase textBox, Label label, RadioButton radioButton)
        {
            _controlValidators.Register(textBox, () => HasValue(textBox, label, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, string fieldName)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, fieldName));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, string fieldName, ValidationCondition condition)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, fieldName, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, string fieldName, CheckBox checkBox)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, fieldName, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, string fieldName, RadioButton radioButton)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, fieldName, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, Label label)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, label));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, Label label, ValidationCondition condition)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, label, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, Label label, CheckBox checkBox)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, label, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a value
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireValue(ComboBox comboBox, Label label, RadioButton radioButton)
        {
            _controlValidators.Register(comboBox, () => HasValue(comboBox, label, radioButton));
        }

        #endregion

        #region RequireSelection

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, string fieldName)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, fieldName));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, string fieldName, ValidationCondition condition)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, fieldName, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, string fieldName, CheckBox checkBox)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, fieldName, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, string fieldName, RadioButton radioButton)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, fieldName, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, Label label)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, label));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, Label label, ValidationCondition condition)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, label, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, Label label, CheckBox checkBox)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, label, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ComboBox" /> has a selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public void RequireSelection(ComboBox comboBox, Label label, RadioButton radioButton)
        {
            _controlValidators.Register(comboBox, () => HasSelection(comboBox, label, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, string fieldName)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, fieldName));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, string fieldName, ValidationCondition condition)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, fieldName, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, string fieldName, CheckBox checkBox)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, fieldName, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, string fieldName, RadioButton radioButton)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, fieldName, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, Label label)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, label));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, Label label, ValidationCondition condition)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, label, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, Label label, CheckBox checkBox)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, label, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="ServerComboBox" /> has a selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field that will be validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public void RequireSelection(ServerComboBox serverComboBox, Label label, RadioButton radioButton)
        {
            _controlValidators.Register(serverComboBox, () => HasSelection(serverComboBox, label, radioButton));
        }

        #endregion

        #region RequireAssetSelection

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, ValidationCondition condition)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, CheckBox checkBox)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, RadioButton radioButton)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, assetTerm));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, ValidationCondition condition)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, assetTerm, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, CheckBox checkBox)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, assetTerm, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="AssetSelectionControl" /> has
        /// at least one asset selected if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public void RequireAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, RadioButton radioButton)
        {
            _controlValidators.Register(assetSelectionControl, () => HasAssetSelection(assetSelectionControl, assetTerm, radioButton));
        }

        #endregion

        #region RequireDocumentSelection

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, ValidationCondition condition)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, CheckBox checkBox)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, RadioButton radioButton)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, radioButton));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, documentTerm));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection.
        /// if the specified <see cref="ValidationCondition" /> is met.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="condition">The condition(s) that must be met when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, ValidationCondition condition)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, documentTerm, condition));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection
        /// if the specified <see cref="CheckBox" /> is checked.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, CheckBox checkBox)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, documentTerm, checkBox));
        }

        /// <summary>
        /// Registers a validation method that ensures the specified <see cref="DocumentSelectionControl" /> has a document selection
        /// if the specified <see cref="RadioButton" /> is checked.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked when validation is performed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public void RequireDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, RadioButton radioButton)
        {
            _controlValidators.Register(documentSelectionControl, () => HasDocumentSelection(documentSelectionControl, documentTerm, radioButton));
        }

        #endregion

        #region RequireCustom

        /// <summary>
        /// Registers a custom validation method for the specified control.
        /// </summary>
        /// <param name="control">The control to validate. (Error provider icons will display on this control.)</param>
        /// <param name="validationMethod">A method which returns a <see cref="ValidationResult" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="control" /> is null.
        /// <para>or</para>
        /// <paramref name="validationMethod" /> is null.
        /// </exception>
        public void RequireCustom(Control control, Func<ValidationResult> validationMethod)
        {
            _controlValidators.Register(control, validationMethod);
        }

        /// <summary>
        /// Registers a custom validation method for the specified control.
        /// </summary>
        /// <param name="control">The control to validate. (Error provider icons will display on this control.)</param>
        /// <param name="validationMethod">A method which returns a <see cref="bool" /> indicating whether validation was successful.</param>
        /// <param name="message">The message to return in the event validation was not successful.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="control" /> is null.
        /// <para>or</para>
        /// <paramref name="validationMethod" /> is null.
        /// </exception>
        public void RequireCustom(Control control, Func<bool> validationMethod, string message)
        {
            _controlValidators.Register(control, () =>
                {
                    if (validationMethod())
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(false, message);
                    }
                }
            );
        }

        #endregion

        #region HasValue

        /// <summary>
        /// Validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public static ValidationResult HasValue(TextBoxBase textBox, string fieldName)
        {
            return HasValue(GetValue(textBox), fieldName);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBox" /> is null.</exception>
        public static ValidationResult HasValue(TextBoxBase textBox, string fieldName, ValidationCondition condition)
        {
            return HasValue(GetValue(textBox), fieldName, ShouldValidate(textBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, string fieldName, CheckBox checkBox)
        {
            return HasValue(GetValue(textBox), fieldName, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, string fieldName, RadioButton radioButton)
        {
            return HasValue(GetValue(textBox), fieldName, IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, Label label)
        {
            return HasValue(GetValue(textBox), GetText(label));
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, Label label, ValidationCondition condition)
        {
            return HasValue(GetValue(textBox), GetText(label), ShouldValidate(textBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, Label label, CheckBox checkBox)
        {
            return HasValue(GetValue(textBox), GetText(label), IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked,
        /// validates a <see cref="TextBox" /> or <see cref="RichTextBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox" /> or <see cref="RichTextBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasValue(TextBoxBase textBox, Label label, RadioButton radioButton)
        {
            return HasValue(GetValue(textBox), GetText(label), IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public static ValidationResult HasValue(ComboBox comboBox, string fieldName)
        {
            return HasValue(GetValue(comboBox), fieldName);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public static ValidationResult HasValue(ComboBox comboBox, string fieldName, ValidationCondition condition)
        {
            return HasValue(GetValue(comboBox), fieldName, ShouldValidate(comboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, string fieldName, CheckBox checkBox)
        {
            return HasValue(GetValue(comboBox), fieldName, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, string fieldName, RadioButton radioButton)
        {
            return HasValue(GetValue(comboBox), fieldName, IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, Label label)
        {
            return HasValue(GetValue(comboBox), GetText(label));
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, Label label, ValidationCondition condition)
        {
            return HasValue(GetValue(comboBox), GetText(label), ShouldValidate(comboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, Label label, CheckBox checkBox)
        {
            return HasValue(GetValue(comboBox), GetText(label), IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ComboBox" /> to ensure it has a value.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasValue(ComboBox comboBox, Label label, RadioButton radioButton)
        {
            return HasValue(GetValue(comboBox), GetText(label), IsChecked(radioButton));
        }

        private static ValidationResult HasValue(string value, string fieldName, bool evaluate = true)
        {
            if (evaluate && string.IsNullOrEmpty(value.Trim()))
            {
                return new ValidationResult(false, BuildHasValueMessage(fieldName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        #endregion

        #region HasSelection

        /// <summary>
        /// Validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public static ValidationResult HasSelection(ComboBox comboBox, string fieldName)
        {
            return HasSelection(HasSelection(comboBox), fieldName);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBox" /> is null.</exception>
        public static ValidationResult HasSelection(ComboBox comboBox, string fieldName, ValidationCondition condition)
        {
            return HasSelection(HasSelection(comboBox), fieldName, ShouldValidate(comboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, string fieldName, CheckBox checkBox)
        {
            return HasSelection(HasSelection(comboBox), fieldName, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, string fieldName, RadioButton radioButton)
        {
            return HasSelection(HasSelection(comboBox), fieldName, IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, Label label)
        {
            return HasSelection(HasSelection(comboBox), GetText(label));
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, Label label, ValidationCondition condition)
        {
            return HasSelection(HasSelection(comboBox), GetText(label), ShouldValidate(comboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, Label label, CheckBox checkBox)
        {
            return HasSelection(HasSelection(comboBox), GetText(label), IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="comboBox">The <see cref="ComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ComboBox comboBox, Label label, RadioButton radioButton)
        {
            return HasSelection(HasSelection(comboBox), GetText(label), IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, string fieldName)
        {
            return HasSelection(HasSelection(serverComboBox), fieldName);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serverComboBox" /> is null.</exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, string fieldName, ValidationCondition condition)
        {
            return HasSelection(HasSelection(serverComboBox), fieldName, ShouldValidate(serverComboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, string fieldName, CheckBox checkBox)
        {
            return HasSelection(HasSelection(serverComboBox), fieldName, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, string fieldName, RadioButton radioButton)
        {
            return HasSelection(HasSelection(serverComboBox), fieldName, IsChecked(radioButton));
        }

        /// <summary>
        /// Validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, Label label)
        {
            return HasSelection(HasSelection(serverComboBox), GetText(label));
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, Label label, ValidationCondition condition)
        {
            return HasSelection(HasSelection(serverComboBox), GetText(label), ShouldValidate(serverComboBox, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, Label label, CheckBox checkBox)
        {
            return HasSelection(HasSelection(serverComboBox), GetText(label), IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="ServerComboBox" /> to ensure a value is selected.
        /// </summary>
        /// <param name="serverComboBox">The <see cref="ServerComboBox" /> to validate.</param>
        /// <param name="label">The <see cref="Label" /> for the field being validated.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverComboBox" /> is null.
        /// <para>or</para>
        /// <paramref name="label" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasSelection(ServerComboBox serverComboBox, Label label, RadioButton radioButton)
        {
            return HasSelection(HasSelection(serverComboBox), GetText(label), IsChecked(radioButton));
        }

        private static ValidationResult HasSelection(bool selected, string fieldName, bool evaluate = true)
        {
            if (evaluate && !selected)
            {
                return new ValidationResult(false, BuildHasSelectionMessage(fieldName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        #endregion

        #region HasAssetSelection

        /// <summary>
        /// Validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl)
        {
            return HasAssetSelection(assetSelectionControl, "asset");
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, ValidationCondition condition)
        {
            return HasAssetSelection(assetSelectionControl, "asset", condition);
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, CheckBox checkBox)
        {
            return HasAssetSelection(assetSelectionControl, "asset", checkBox);
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, RadioButton radioButton)
        {
            return HasAssetSelection(assetSelectionControl, "asset", radioButton);
        }

        /// <summary>
        /// Validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm)
        {
            return HasAssetSelection(assetSelectionControl, assetTerm, true);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetSelectionControl" /> is null.</exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, ValidationCondition condition)
        {
            return HasAssetSelection(assetSelectionControl, assetTerm, ShouldValidate(assetSelectionControl, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, CheckBox checkBox)
        {
            return HasAssetSelection(assetSelectionControl, assetTerm, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates an <see cref="AssetSelectionControl" /> to ensure at least one asset is selected.
        /// </summary>
        /// <param name="assetSelectionControl">The asset selection control.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "device".</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, RadioButton radioButton)
        {
            return HasAssetSelection(assetSelectionControl, assetTerm, IsChecked(radioButton));
        }

        private static ValidationResult HasAssetSelection(AssetSelectionControl assetSelectionControl, string assetTerm, bool evaluate)
        {
            if (assetSelectionControl == null)
            {
                throw new ArgumentNullException(nameof(assetSelectionControl));
            }

            if (evaluate && !assetSelectionControl.HasSelection)
            {
                return new ValidationResult(false, BuildHasAssetSelectionMessage(assetTerm));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        #endregion

        #region HasDocumentSelection

        /// <summary>
        /// Validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl)
        {
            return HasDocumentSelection(documentSelectionControl, "document");
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, ValidationCondition condition)
        {
            return HasDocumentSelection(documentSelectionControl, "document", condition);
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="documentSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, CheckBox checkBox)
        {
            return HasDocumentSelection(documentSelectionControl, "document", checkBox);
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="documentSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, RadioButton radioButton)
        {
            return HasDocumentSelection(documentSelectionControl, "document", radioButton);
        }

        /// <summary>
        /// Validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm)
        {
            return HasDocumentSelection(documentSelectionControl, documentTerm, true);
        }

        /// <summary>
        /// If the specified <see cref="ValidationCondition" /> is met, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="condition">The condition(s) under which validation should be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="documentSelectionControl" /> is null.</exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, ValidationCondition condition)
        {
            return HasDocumentSelection(documentSelectionControl, documentTerm, ShouldValidate(documentSelectionControl, condition));
        }

        /// <summary>
        /// If the specified <see cref="CheckBox" /> is checked, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="checkBox">The <see cref="CheckBox" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="documentSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="checkBox" /> is null.
        /// </exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, CheckBox checkBox)
        {
            return HasDocumentSelection(documentSelectionControl, documentTerm, IsChecked(checkBox));
        }

        /// <summary>
        /// If the specified <see cref="RadioButton" /> is checked, validates a <see cref="DocumentSelectionControl" /> to ensure it has a document selection.
        /// </summary>
        /// <param name="documentSelectionControl">The document selection control.</param>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "attachment".</param>
        /// <param name="radioButton">The <see cref="RadioButton" /> that must be checked if validation is to be performed.</param>
        /// <returns>A <see cref="ValidationResult" /> object indicating the result of validation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="documentSelectionControl" /> is null.
        /// <para>or</para>
        /// <paramref name="radioButton" /> is null.
        /// </exception>
        public static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, RadioButton radioButton)
        {
            return HasDocumentSelection(documentSelectionControl, documentTerm, IsChecked(radioButton));
        }

        private static ValidationResult HasDocumentSelection(DocumentSelectionControl documentSelectionControl, string documentTerm, bool evaluate)
        {
            if (documentSelectionControl == null)
            {
                throw new ArgumentNullException(nameof(documentSelectionControl));
            }

            if (evaluate && !documentSelectionControl.HasSelection)
            {
                return new ValidationResult(false, BuildHasDocumentSelectionMessage(documentTerm));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        #endregion

        #region Message Building

        /// <summary>
        /// Builds a standard message for indicating the specified field must have a value.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>A string similar to "{fieldName} must have a value.".</returns>
        public static string BuildHasValueMessage(string fieldName)
        {
            return $"{fieldName} must have a value.";
        }

        /// <summary>
        /// Builds a standard message for indicating the specified field must have a value.
        /// </summary>
        /// <param name="fieldLabel">The <see cref="Label" /> with the field name.</param>
        /// <returns>A string similar to "{fieldName} must have a value.".</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fieldLabel" /> is null.</exception>
        public static string BuildHasValueMessage(Label fieldLabel)
        {
            if (fieldLabel == null)
            {
                throw new ArgumentNullException(nameof(fieldLabel));
            }
            return BuildHasValueMessage(GetText(fieldLabel));
        }

        /// <summary>
        /// Builds a standard message for indicating the specified field must be selected.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>A string similar to "{fieldName} must be selected.".</returns>
        public static string BuildHasSelectionMessage(string fieldName)
        {
            return $"{fieldName} must be selected.";
        }

        /// <summary>
        /// Builds a standard message for indicating the specified field must be selected.
        /// </summary>
        /// <param name="fieldLabel">The <see cref="Label" /> with the field name.</param>
        /// <returns>A string similar to "{fieldName} must be selected.".</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fieldLabel" /> is null.</exception>
        public static string BuildHasSelectionMessage(Label fieldLabel)
        {
            if (fieldLabel == null)
            {
                throw new ArgumentNullException(nameof(fieldLabel));
            }
            return BuildHasSelectionMessage(GetText(fieldLabel));
        }

        /// <summary>
        /// Builds a standard message for indicating an asset must be selected.
        /// </summary>
        /// <param name="assetTerm">The term to use to describe the assets; e.g. "asset" or "device".</param>
        /// <returns>A string similar to "At least one {assetTerm} must be selected.".</returns>
        public static string BuildHasAssetSelectionMessage(string assetTerm)
        {
            return $"At least one {assetTerm} must be selected.";
        }

        /// <summary>
        /// Builds a standard message for indicating an asset must be selected.
        /// </summary>
        /// <param name="assetTermLabel">The <see cref="Label" /> with the term to use to describe the assets; e.g. "asset" or "device".</param>
        /// <returns>A string similar to "At least one {assetTerm} must be selected.".</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetTermLabel" /> is null.</exception>
        public static string BuildHasAssetSelectionMessage(Label assetTermLabel)
        {
            if (assetTermLabel == null)
            {
                throw new ArgumentNullException(nameof(assetTermLabel));
            }
            return BuildHasAssetSelectionMessage(GetText(assetTermLabel));
        }

        /// <summary>
        /// Builds a standard message for indicating a document must be selected.
        /// </summary>
        /// <param name="documentTerm">The term to use to describe the documents; e.g. "document" or "attachment".</param>
        /// <returns>A string similar to "A valid {documentTerm} selection is required.".</returns>
        public static string BuildHasDocumentSelectionMessage(string documentTerm)
        {
            return $"A valid {documentTerm} selection is required.";
        }

        /// <summary>
        /// Builds a standard message for indicating a document must be selected.
        /// </summary>
        /// <param name="documentTermLabel">The <see cref="Label" /> with the term to use to describe the documents; e.g. "document" or "attachment".</param>
        /// <returns>A string similar to "A valid {documentTerm} selection is required.".</returns>
        public static string BuildHasDocumentSelectionMessage(Label documentTermLabel)
        {
            if (documentTermLabel == null)
            {
                throw new ArgumentNullException(nameof(documentTermLabel));
            }
            return BuildHasDocumentSelectionMessage(GetText(documentTermLabel));
        }

        /// <summary>
        /// Builds a custom message using a <see cref="Label" /> and a format string.
        /// </summary>
        /// <param name="fieldLabel">The <see cref="Label" /> with the field name to use in the message.</param>
        /// <param name="format">The format string.</param>
        /// <returns>A string built from <paramref name="format" /> with the text of <paramref name="fieldLabel" /> substituted in.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fieldLabel" /> is null.
        /// <para>or</para>
        /// <paramref name="format" /> is null.
        /// </exception>
        /// <exception cref="FormatException"><paramref name="format" /> is invalid.</exception>
        public static string BuildCustomMessage(Label fieldLabel, string format)
        {
            if (fieldLabel == null)
            {
                throw new ArgumentNullException(nameof(fieldLabel));
            }
            return string.Format(format, GetText(fieldLabel));
        }

        #endregion

        #region Control Property Extraction

        private static bool ShouldValidate(Control control, ValidationCondition condition)
        {
            // HasFlag will always return true for the 0 value,
            // so we need to do this check before looking for individual flags
            if (condition == ValidationCondition.Always)
            {
                return true;
            }
            else if (condition.HasFlag(ValidationCondition.IfEnabled) && !control.Enabled)
            {
                return false;
            }
            else
            {
                // All checks passed.
                return true;
            }
        }

        private static string GetValue(TextBoxBase textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException(nameof(textBox));
            }
            return textBox.Text;
        }

        private static string GetValue(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                throw new ArgumentNullException(nameof(comboBox));
            }
            return comboBox.Text;
        }

        private static bool HasSelection(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                throw new ArgumentNullException(nameof(comboBox));
            }

            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                return comboBox.SelectedIndex != -1;
            }
            else
            {
                return !string.IsNullOrEmpty(comboBox.Text);
            }
        }

        private static bool HasSelection(ServerComboBox serverComboBox)
        {
            if (serverComboBox == null)
            {
                throw new ArgumentNullException(nameof(serverComboBox));
            }
            return serverComboBox.HasSelection;
        }

        private static string GetText(Label label)
        {
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }
            return label.Text.TrimEnd(':').Trim();
        }

        private static bool IsChecked(CheckBox checkBox)
        {
            if (checkBox == null)
            {
                throw new ArgumentNullException(nameof(checkBox));
            }
            return checkBox.Checked;
        }

        private static bool IsChecked(RadioButton radioButton)
        {
            if (radioButton == null)
            {
                throw new ArgumentNullException(nameof(radioButton));
            }
            return radioButton.Checked;
        }

        #endregion

        #region Helper Classes

        private sealed class ControlValidatorCollection
        {
            private readonly Dictionary<Control, Func<ValidationResult>> _validators = new Dictionary<Control, Func<ValidationResult>>();

            public Func<ValidationResult> this[Control control]
            {
                get
                {
                    if (_validators.ContainsKey(control))
                    {
                        return _validators[control];
                    }
                    else
                    {
                        throw new ArgumentException("There is no registered validation method for the specified control.", nameof(control));
                    }
                }
            }

            public IEnumerable<Control> Controls
            {
                get { return _validators.Keys; }
            }

            public void Register(TextBoxBase textBox, Func<ValidationResult> validationMethod)
            {
                Register(textBox, validationMethod, nameof(textBox));
            }

            public void Register(ComboBox comboBox, Func<ValidationResult> validationMethod)
            {
                Register(comboBox, validationMethod, nameof(comboBox));
            }

            public void Register(ServerComboBox serverComboBox, Func<ValidationResult> validationMethod)
            {
                Register(serverComboBox, validationMethod, nameof(serverComboBox));
            }

            public void Register(AssetSelectionControl assetSelectionControl, Func<ValidationResult> validationMethod)
            {
                Register(assetSelectionControl, validationMethod, nameof(assetSelectionControl));
            }

            public void Register(DocumentSelectionControl documentSelectionControl, Func<ValidationResult> validationMethod)
            {
                Register(documentSelectionControl, validationMethod, nameof(documentSelectionControl));
            }

            public void Register(Control control, Func<ValidationResult> validationMethod, string paramName = "control")
            {
                if (control == null)
                {
                    throw new ArgumentNullException(paramName);
                }

                _validators[control] = validationMethod ?? throw new ArgumentNullException(nameof(validationMethod));
            }

            public void Remove(Control control)
            {
                if (_validators.ContainsKey(control))
                {
                    _validators.Remove(control);
                }
            }

            public void Clear()
            {
                _validators.Clear();
            }
        }

        private sealed class ControlIconProxyCollection
        {
            private readonly Dictionary<Control, Control> _proxies = new Dictionary<Control, Control>();

            public Control this[Control control]
            {
                get
                {
                    if (_proxies.ContainsKey(control))
                    {
                        return _proxies[control];
                    }
                    else
                    {
                        return control;
                    }
                }
            }

            public void Register(Control control, Control iconProxy)
            {
                if (control == null)
                {
                    throw new ArgumentNullException(nameof(control));
                }

                _proxies[control] = iconProxy ?? throw new ArgumentNullException(nameof(iconProxy));
            }
        }

        #endregion
    }
}
