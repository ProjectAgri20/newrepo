using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace DBScripter
{
    internal class Parameters
    {
        /*
         * Required parameters.
         */

        [Option('d', "database", HelpText = "Name of the database to be scripted.", Required = true)]
        public string Database { get; set; }

        [Option('p', "password", HelpText = "Password for the user when logging onto the SQL server.", Required = true)]
        public string Password { get; set; }

        [Option('s', "server", HelpText = "Name of the SQL server that contains the database to be scripted.", Required = true)]
        public string Server { get; set; }

        [Option('u', "username", HelpText = "User name for logging onto the SQL server.", Required = true)]
        public string UserName { get; set; }

        /*
         * Optional parameters.
         */

        [Option('o', "output", HelpText = "(Default: <server>_<database>.sql) Name of the SQL output file.")]
        public string OutputFilename { get; set; }

        [Option(Default = true, HelpText = "Script all database objects.")]
        public bool All { get; set; }

        [Option(Default = false, HelpText = "Script foreign keys.")]
        public bool ForeignKeys { get; set; }

        [Option(Default = false, HelpText = "Script user-defined functions.")]
        public bool Functions { get; set; }

        [Option(Default = false, HelpText = "Script non-clustered indexes.")]
        public bool Indexes { get; set; }

        [Option(Default = false, HelpText = "Script stored procedures.")]
        public bool Procedures { get; set; }

        [Option(Default = false, HelpText = "Script tables.")]
        public bool Tables { get; set; }

        [Option(Default = false, HelpText = "Script database and table triggers.")]
        public bool Triggers { get; set; }

        [Option(Default = false, HelpText = "Script schemas.")]
        public bool Schemas { get; set; }

        [Option(Default = false, HelpText = "Script users.")]
        public bool Users { get; set; }

        [Option(Default = false, HelpText = "Script views.")]
        public bool Views { get; set; }

        [Option('w', "windowslogin", Default = false, HelpText = "Use Windows Login account.")]
        public bool WindowsLogin { get; set; }

        /*
         * Usage examples.
         */

        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>()
        {
            new Example("*  Script all objects in the specified database.",
                        new Parameters {Server = "Database.Server", Database = "Database_name", UserName = "UserName", Password = "Password"})
        };

        /*
         * Internal parameters.
         */

        public bool DbObjects
        {
            get => ForeignKeys || Functions || Indexes || Procedures || Tables || Triggers || Schemas || Users || Views;
            set => ForeignKeys = Functions = Indexes = Procedures = Tables = Triggers = Schemas = Users = Views = value;
        }
    }
}