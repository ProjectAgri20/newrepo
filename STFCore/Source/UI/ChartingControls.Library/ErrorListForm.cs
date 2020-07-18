using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.Charting
{
    /// <summary>
    /// Form that displays a list of errors and their frequency.
    /// </summary>
    public partial class ErrorListForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorListForm"/> class.
        /// </summary>
        /// <param name="errorList">The error list.</param>
        /// <param name="seriesName">Name of the series.</param>
        public ErrorListForm(SortableBindingList<ErrorCount> errorList, string seriesName)
        {
            InitializeComponent();
            error_DataGridView.DataSource = errorList;
            this.Text = "Error List - " + seriesName;
        }

        private void close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// Struct containing an error description and a count of errors.
    /// </summary>
    public struct ErrorCount
    {
        private string _error;
        private int _count;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public string Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCount"/> struct.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="count">The count.</param>
        public ErrorCount(string error, int count)
        {
            _error = error;
            _count = count;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ErrorCount other = (ErrorCount)obj;
            return (_error == other._error && _count == other._count);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="errorCount1">The error count1.</param>
        /// <param name="errorCount2">The error count2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ErrorCount errorCount1, ErrorCount errorCount2)
        {
            return errorCount1.Equals(errorCount2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="errorCount1">The error count1.</param>
        /// <param name="errorCount2">The error count2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ErrorCount errorCount1, ErrorCount errorCount2)
        {
            return !errorCount1.Equals(errorCount2);
        }
    }
}
