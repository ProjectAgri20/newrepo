using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    internal static class Extension
    {
        public static string DataPath(this SqlConnectionStringBuilder builder)
        {
            StringBuilder result = new StringBuilder(builder.DataSource);
            result.Append(".");
            result.Append(builder.InitialCatalog);

            return result.ToString();
        }
    }
}
