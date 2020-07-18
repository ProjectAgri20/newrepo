using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Simple-binds the indicated control property to the inverse of the
    /// specified boolean data member of the data source.
    /// </summary>
    public sealed class InvertedBinding
    {
        private string PropertyName { get; }
        private object DataSource { get; }
        private string DataMember { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvertedBinding" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the control property to bind.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataMember">The property to bind to.</param>
        public InvertedBinding(string propertyName, object dataSource, string dataMember)
        {
            PropertyName = propertyName;
            DataSource = dataSource;
            DataMember = dataMember;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="InvertedBinding" /> to <see cref="Binding" />.
        /// </summary>
        /// <param name="binding">The <see cref="InvertedBinding" />.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Binding(InvertedBinding binding)
        {
            return ToBinding(binding);
        }

        /// <summary>
        /// Converts the specified <see cref="InvertedBinding" /> object into a <see cref="Binding" /> object.
        /// </summary>
        /// <param name="binding">The <see cref="InvertedBinding" /> to convert.</param>
        /// <returns>A <see cref="Binding" /> object.</returns>
        public static Binding ToBinding(InvertedBinding binding)
        {
            if (binding == null)
            {
                return null;
            }

            Binding newBinding = new Binding(binding.PropertyName, binding.DataSource, binding.DataMember, false, DataSourceUpdateMode.OnPropertyChanged);
            newBinding.Parse += InvertBindingValue;
            newBinding.Format += InvertBindingValue;
            return newBinding;
        }

        private static void InvertBindingValue(object sender, ConvertEventArgs e)
        {
            e.Value = !((bool)e.Value);
        }
    }
}
