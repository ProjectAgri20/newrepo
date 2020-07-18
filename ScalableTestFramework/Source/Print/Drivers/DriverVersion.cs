using System;
using System.Diagnostics;
using System.Linq;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Represents a print driver version number.
    /// </summary>
    [DebuggerDisplay("{ToString(),nq}")]
    public struct DriverVersion : IEquatable<DriverVersion>, IComparable<DriverVersion>, IComparable
    {
        private readonly int[] _versionData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverVersion" /> struct.
        /// </summary>
        /// <param name="version">The version number as a string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="version" /> is null.</exception>
        /// <exception cref="FormatException"><paramref name="version" /> is not a valid version number.</exception>
        public DriverVersion(string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            _versionData = version.Split('.').Select(n => int.Parse(n)).ToArray();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (_versionData != null)
            {
                return string.Join(".", _versionData);
            }
            else
            {
                return base.ToString();
            }
        }

        #region IEquatable Members

        /// <summary>
        /// Tests whether this <see cref="DriverVersion" /> represents the same version as another <see cref="DriverVersion" />.
        /// </summary>
        /// <param name="other">A <see cref="DriverVersion" /> to compare with this <see cref="DriverVersion" />.</param>
        /// <returns>true if the current <see cref="DriverVersion" /> represents the same version as the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(DriverVersion other)
        {
            if (_versionData == null || other._versionData == null)
            {
                return _versionData == other._versionData;
            }

            return _versionData.SequenceEqual(other._versionData);
        }

        /// <summary>
        /// Tests whether <paramref name="obj" /> is a <see cref="DriverVersion" /> representing the same version as this <see cref="DriverVersion" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="DriverVersion" /> representing the same version as this <see cref="DriverVersion" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DriverVersion)
            {
                return Equals((DriverVersion)obj);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _versionData?.Sum() ?? 0;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this <see cref="DriverVersion" /> to another <see cref="DriverVersion" /> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The <see cref="DriverVersion" /> to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods")]
        public int CompareTo(DriverVersion other)
        {
            int thisLength = _versionData?.Length ?? 0;
            int otherLength = other._versionData?.Length ?? 0;

            for (int i = 0; i < thisLength && i < otherLength; i++)
            {
                if (_versionData[i] != other._versionData[i])
                {
                    return _versionData[i].CompareTo(other._versionData[i]);
                }
            }

            return thisLength.CompareTo(otherLength);
        }

        /// <summary>
        /// Compares this <see cref="DriverVersion" /> to another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not a <see cref="DriverVersion" />.</exception>
        public int CompareTo(object obj)
        {
            if (obj is DriverVersion)
            {
                return CompareTo((DriverVersion)obj);
            }
            else if (obj == null)
            {
                return 1;
            }
            else
            {
                throw new ArgumentException($"The specified object is not a {nameof(DriverVersion)}.", nameof(obj));
            }
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(DriverVersion left, DriverVersion right) => left.Equals(right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(DriverVersion left, DriverVersion right) => !left.Equals(right);

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(DriverVersion left, DriverVersion right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(DriverVersion left, DriverVersion right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(DriverVersion left, DriverVersion right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(DriverVersion left, DriverVersion right) => left.CompareTo(right) >= 0;

        #endregion
    }
}
