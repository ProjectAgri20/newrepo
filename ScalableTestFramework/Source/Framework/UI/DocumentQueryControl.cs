using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Dialog to support generic querying of test document data.
    /// </summary>
    internal partial class DocumentQueryControl : UserControl
    {
        private DocumentQueryProperty _currentProperty;
        private DocumentQuery _documentQuery = new DocumentQuery();

        private static readonly List<DocumentQueryProperty> _queryProperties = new List<DocumentQueryProperty>
        {
            DocumentQueryProperty.FileName,
            DocumentQueryProperty.Extension,
            DocumentQueryProperty.Tag,
            DocumentQueryProperty.FileSize,
            DocumentQueryProperty.Pages,
            DocumentQueryProperty.ColorMode,
            DocumentQueryProperty.Orientation
        };

        private static readonly Dictionary<QueryOperator, string> _operatorStrings = new Dictionary<QueryOperator, string>
        {
            {QueryOperator.Equal, "is" },
            {QueryOperator.LessThan, "is less than" },
            {QueryOperator.GreaterThan, "is greater than" },
            {QueryOperator.LessThanOrEqual, "is less than or equal to" },
            {QueryOperator.GreaterThanOrEqual, "is greater than or equal to" },
            {QueryOperator.NotEqual, "is not" },
            {QueryOperator.Contains, "contains" },
            {QueryOperator.BeginsWith, "begins with" },
            {QueryOperator.EndsWith, "ends with" },
            {QueryOperator.IsIn, "is in" },
            {QueryOperator.IsNotIn, "is not in" },
            {QueryOperator.IsBetween, "is between" }
        };

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentQueryControl"/> class.
        /// </summary>
        public DocumentQueryControl()
        {
            InitializeComponent();
            property_ListBox.DataSource = _queryProperties;
        }

        /// <summary>
        /// Gets the <see cref="Documents.DocumentQuery" /> from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentQuery DocumentQuery
        {
            get { return _documentQuery; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any query criteria defined.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return _documentQuery.Criteria.Count > 0; }
        }

        /// <summary>
        /// Clears all document selection.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void ClearSelection()
        {
            _documentQuery = new DocumentQuery();
            RefreshCriteriaList();
        }

        /// <summary>
        /// Initializes the document query control.
        /// </summary>
        public void Initialize()
        {
            Initialize(new DocumentQuery());
        }

        /// <summary>
        /// Initializes this control with the specified <see cref="DocumentQuery" />.
        /// </summary>
        /// <param name="documentQuery">The document query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentQuery" /> is null.</exception>
        public void Initialize(DocumentQuery documentQuery)
        {
            _documentQuery = documentQuery ?? throw new ArgumentNullException(nameof(documentQuery));
            RefreshCriteriaList();
        }

        private DocumentQueryCriteria CreateCriteria()
        {
            DocumentQueryProperty queryProperty = property_ListBox.SelectedItem as DocumentQueryProperty;
            QueryOperator queryOperator = _operatorStrings.First(n => n.Value == (string)operator_ListBox.SelectedItem).Key;
            Collection<object> values = new Collection<object>();

            if (values_ListBox.Visible)
            {
                foreach (string item in values_ListBox.SelectedItems)
                {
                    values.Add(item);
                }
            }
            else if (value1_TextBox.Visible)
            {
                values.Add(value1_TextBox.Text);
                if (value2_TextBox.Visible)
                {
                    values.Add(value2_TextBox.Text);
                }
            }

            return new DocumentQueryCriteria(queryProperty, queryOperator, values);
        }

        private void AddOrUpdateCriteria(DocumentQueryCriteria criteria)
        {
            DocumentQueryCriteria existing = _documentQuery.Criteria.FirstOrDefault(n => n.PropertyName == criteria.PropertyName);
            if (existing != null)
            {
                _documentQuery.Criteria.Remove(existing);
            }

            _documentQuery.Criteria.Add(criteria);
            OnSelectionChanged();
        }

        private void RemoveCriteria(DocumentQueryCriteria criteria)
        {
            _documentQuery.Criteria.Remove(criteria);
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshCriteriaList()
        {
            criteria_ListBox.DataSource = null;
            criteria_ListBox.DataSource = _documentQuery.Criteria.Select(n => new DocumentQueryCriteriaDisplay(n)).ToList();
        }

        private void RefreshOperatorList(DocumentQueryProperty property)
        {
            operator_ListBox.DataSource = null;

            if (property != null)
            {
                operator_ListBox.DataSource = property.Operators.Select(n => _operatorStrings[n]).ToList();
                if (operator_ListBox.Items.Count > 0)
                {
                    operator_ListBox.SelectedIndex = 0;
                }
            }
        }

        private void RefreshValues(DocumentQueryProperty property, QueryOperator selectedOperator)
        {
            HideAllControls();

            if (property.Values != null && property.Values.Count > 0)
            {
                EnableValuesListBox(selectedOperator);
            }
            else
            {
                EnableValuesTextBoxes(selectedOperator);
            }
        }

        private void EnableValuesListBox(QueryOperator selectedOperator)
        {
            values_ListBox.Visible = true;
            values_ListBox.DataSource = _currentProperty.Values.Select(n => n.ToString()).ToList();

            if (selectedOperator == QueryOperator.IsIn || selectedOperator == QueryOperator.IsNotIn)
            {
                values_ListBox.SelectionMode = SelectionMode.MultiExtended;
            }
            else
            {
                values_ListBox.SelectionMode = SelectionMode.One;
            }
        }

        private void EnableValuesTextBoxes(QueryOperator queryOperator)
        {
            value1_TextBox.Visible = true;

            if (queryOperator == QueryOperator.IsBetween)
            {
                value2_TextBox.Visible = true;
                and_Label.Visible = true;
            }
            else
            {
                value2_TextBox.Visible = false;
                and_Label.Visible = false;
            }
        }

        private void HideAllControls()
        {
            values_ListBox.Visible = false;
            values_ListBox.DataSource = null;
            value1_TextBox.Visible = false;
            value1_TextBox.Clear();
            value2_TextBox.Visible = false;
            value2_TextBox.Clear();
            and_Label.Visible = false;
        }

        private void addCriteria_Button_Click(object sender, EventArgs e)
        {
            DocumentQueryCriteria criteria = CreateCriteria();
            AddOrUpdateCriteria(criteria);
            RefreshCriteriaList();
        }

        private void criteria_ListBox_DoubleClick(object sender, EventArgs e)
        {
            DocumentQueryCriteriaDisplay display = (DocumentQueryCriteriaDisplay)criteria_ListBox.SelectedItem;
            RemoveCriteria(display.Criteria);
            RefreshCriteriaList();
        }

        private void property_ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            _currentProperty = property_ListBox.SelectedItem as DocumentQueryProperty;
            RefreshOperatorList(_currentProperty);
        }

        private void operator_ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (operator_ListBox.SelectedIndex != -1)
            {
                QueryOperator selectedOperator = _operatorStrings.First(n => n.Value == (string)operator_ListBox.SelectedItem).Key;

                if (_currentProperty != null)
                {
                    RefreshValues(_currentProperty, selectedOperator);
                }
            }
        }

        private void preview_Button_Click(object sender, EventArgs e)
        {
            using (DocumentPreviewForm form = new DocumentPreviewForm(_documentQuery))
            {
                form.ShowDialog();
            }
        }

        private sealed class DocumentQueryCriteriaDisplay
        {
            public DocumentQueryCriteria Criteria { get; set; }

            public DocumentQueryCriteriaDisplay(DocumentQueryCriteria criteria)
            {
                Criteria = criteria;
            }

            public override string ToString()
            {
                DocumentQueryProperty property = _queryProperties.FirstOrDefault(n => n.Name == Criteria.PropertyName);
                string label = property?.Label ?? Criteria.PropertyName;
                string operatorString = _operatorStrings[Criteria.QueryOperator];
                return $"{label} {operatorString} {string.Join(", ", Criteria.Values)}";
            }
        }
    }
}
