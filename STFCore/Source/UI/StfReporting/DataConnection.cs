using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HP.ScalableTest.UI.Reporting
{
    public static class DataConnection
    {
        private const string DOMAIN = "etl.boi.rd.adapps.hp.com";

        public static string GetDatabaseNumber(string selected)
        {
            switch (selected)
            {
                case "Development":
                    return "03";
                case "Beta":
                    return "02";
                case "Production":
                default:
                    return "01";
            }
        }

        public static string ODSConnectionString(string table, string envrionment)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                DataSource = "STFODS" + DataConnection.GetDatabaseNumber(envrionment) + "." + DOMAIN,
                InitialCatalog = table,
                PersistSecurityInfo = true,
                UserID = "enterprise_data",
                Password = "enterprise_data",
                MultipleActiveResultSets = true
            };

            return builder.ToString();
        }

        public static string GetSettingsDatabaseName(string type, string environment)
        {
            StringBuilder builder = new StringBuilder("STF");
            switch (type)
            {
                case "ODS":
                    builder.Append("ODS");
                    break;
                case "Default":
                default:
                    builder.Append("System");
                    break;
            }

            builder.Append(GetDatabaseNumber(environment)).Append(".").Append(DOMAIN);

            return builder.ToString();
        }        
    }
}
