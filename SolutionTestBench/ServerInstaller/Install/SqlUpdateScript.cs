
using System;

namespace HP.SolutionTest.Install
{
    internal class SqlUpdateScript : IComparable
    {
        public string Database { get; set; }
        public Version Version { get; set; }
        public string SqlText { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            SqlUpdateScript otherSqlUpdateScript = obj as SqlUpdateScript;
            if (otherSqlUpdateScript != null)
            {
                return Version.CompareTo(otherSqlUpdateScript.Version);
            }
            else
            {
                throw new ArgumentException("Object is not a SqlUpdateScript");
            }
        }
    }
}
