namespace HP.ScalableTestTriageData.Data
{
    public class DbAccess
    {
        private readonly string _datasource = string.Empty;

        // Datalog database names
        private const string DataLogDatabase = "ScalableTestDataLog";
        private const string DataLogODS = "EnterpriseTestODS";
        private const string BangaloreACT = "DataLog-Bangalore-ACT";
        private const string BangaloreATS = "DataLog-Bangalore-ACS";
        private const string BangaloreERT = "DataLog-Bangalore-ERT";
        private const string BangalorePSL = "DataLog-Bangalore-PSL";

        private const string UserId = "enterprise_data";
        private const string Password = "enterprise_data";

        private const string DsDevelopment = "15.86.232.57";
        private const string DsProduction = "STFOds01.etl.boi.rd.hpicorp.net";
        private const string DsBeta = "15.86.232.55";
        private const string DsHPPK = "130.31.192.225";
        private const string DsHPPKPankyo = "STBSERVER01";
        private const string DsBangalore = "stb-datastore.etl.boi.rd.hpicorp.net ";

        private const string ProductionName = "StfOds01 Production";
        private const string BetaName = "STFData02-Beta";
        private const string DevelopmentName = "STFData03-Development";
        private const string ServerName = "HPPK_STB_Server";
        private const string ServerNamePankyo = "HPPK_STB_Server_Pankyo";

        private DATABASE _theDb;

        /// <summary>
        /// Constructor: initializes the database source to a development system
        /// </summary>
        public DbAccess(string database)
        {
            _datasource = DsDevelopment;

            switch (database)
            {
                case DevelopmentName:
                    _datasource = DsDevelopment;
                    _theDb = DATABASE.Development;
                    break;
                case ProductionName:
                    _datasource = DsProduction;
                    _theDb = DATABASE.Production;
                    break;
                case BetaName:
                    _datasource = DsBeta;
                    _theDb = DATABASE.Test;
                    break;
                case ServerName:
                    _datasource = DsHPPK;
                    _theDb = DATABASE.HPPK;
                    break;
                case BangaloreACT:
                    _datasource = DsBangalore;
                    _theDb = DATABASE.Bangalore_ACT;
                    break;
                case ServerNamePankyo:
                    _datasource = DsHPPKPankyo;
                    _theDb = DATABASE.HPPK_Pankyo;
                    break;
                default:
                    _datasource = database;
                    _theDb = DATABASE.Other;
                    break;
            }
        }

        /// <summary>
        /// Instantiate with either the development or production DB
        /// </summary>
        /// <param name="db">E_Database</param>
        public DbAccess(DATABASE db)
        {
            _theDb = db;
            switch (_theDb)
            {
                case DATABASE.Production:
                    _datasource = DsProduction;
                    break;
                case DATABASE.Test:
                    _datasource = DsBeta;
                    break;
                case DATABASE.Development:
                    _datasource = DsDevelopment;
                    break;
                case DATABASE.HPPK:
                    _datasource = DsHPPK;
                    break;
                case DATABASE.HPPK_Pankyo:
                    _datasource = DsHPPKPankyo;
                    break;
            }
        }

        protected string GetConnectionString(string dataSource, string catalog) => $"Data Source={_datasource};Initial Catalog={catalog};Persist Security Info=True;User ID={UserId};Password={Password}";

        protected string GetConStrSqlReadonly(string dataSource, string catalog) => $"Data Source={dataSource};Initial Catalog={catalog};Persist Security Info=True;User ID=report_viewer;Password=report_viewer";

        /// <summary>
        /// Retrieves either the production, test, or development connection string
        /// based on how the class in instantiated.
        /// </summary>
        /// <returns>string - Connection string</returns>
        public string getConStrSQL()
        {
            string connectionString = string.Empty;
            switch (_theDb)
            {
                case DATABASE.Development:
                    connectionString = GetConnectionString(DsDevelopment, DataLogDatabase);
                    break;
                case DATABASE.Production:
                    connectionString = GetConStrSqlReadonly(DsProduction, DataLogODS);
                    break;
                case DATABASE.Test:
                    connectionString = GetConnectionString(DsBeta, DataLogDatabase);
                    break;
                case DATABASE.HPPK:
                    connectionString = GetConnectionString(DsHPPK, DataLogDatabase);
                    break;
                case DATABASE.Bangalore_ACT:
                    connectionString = GetConStrSqlReadonly(DsBangalore, BangaloreACT);
                    break;
                case DATABASE.Bangalore_ATS:
                    connectionString = GetConStrSqlReadonly(DsBangalore, BangaloreACT);
                    break;
                case DATABASE.Bangalore_ERT:
                    connectionString = GetConStrSqlReadonly(DsBangalore, BangaloreACT);
                    break;
                case DATABASE.Bangalore_PSL:
                    connectionString = GetConStrSqlReadonly(DsBangalore, BangaloreACT);
                    break;
                case DATABASE.HPPK_Pankyo:
                    connectionString = GetConnectionString(DsHPPKPankyo, DataLogDatabase);
                    break;
                case DATABASE.Other:
                    connectionString = $"Data Source={_datasource};Initial Catalog={DataLogDatabase};Persist Security Info=True;User ID={UserId};Password={Password}";
                    break;
            }
            return connectionString;
        }
    }

    public enum DATABASE
    {
        Production = 0,
        Development = 1,
        Test = 2,
        HPPK = 3,
        Bangalore_ACT = 4,
        Bangalore_ATS = 5,
        Bangalore_ERT = 6,
        Bangalore_PSL = 7,
        HPPK_Pankyo = 8,
        Other = 9
    };
}
