using Microsoft.SqlServer.Management.Smo;
using System;
using System.IO;
using System.Text;

namespace DBScripter
{
    class DBScripter
    {
        private static Database _db;
        private static ScriptingOptions _scriptOptions;

        internal static int ScriptDatabase(Parameters parameters)
        {
            Server server = new Server();

            // Set the connection properties for the database server.
            if (parameters.WindowsLogin)
            {
                server.ConnectionContext.LoginSecure = true;
            }
            else
            {
                server.ConnectionContext.LoginSecure = false;
                server.ConnectionContext.Login = parameters.UserName;
                server.ConnectionContext.Password = parameters.Password;
            }
            server.ConnectionContext.ServerInstance = parameters.Server;

            // See if the database to be scripted exists on the server.
            try
            {
                if (server.Databases.Contains(parameters.Database))
                {
                    _db = server.Databases[parameters.Database];
                }
                else
                {
                    Console.WriteLine($"ERROR: Database {parameters.Database} does not exist on the SQL server {parameters.Server}.");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return 1;
            }

            // Set the output filename and the general scripting options to use for all database objects.
            _scriptOptions = new ScriptingOptions()
            {
                AnsiFile = true,
                AppendToFile = true,
                ClusteredIndexes = true,
                Default = true,
                DriPrimaryKey = true,
                EnforceScriptingOptions = true,
                ExtendedProperties = true,
                FileName = string.IsNullOrEmpty(parameters.OutputFilename) ? $"{server.Name}_{_db.Name}.sql" : parameters.OutputFilename,
                IncludeDatabaseContext = true,
                IncludeDatabaseRoleMemberships = true,
                IncludeIfNotExists = true,
                NoCollation = true,
                SchemaQualify = true,
                SchemaQualifyForeignKeysReferences = true,
                ScriptSchema = true,
                ToFileOnly = true,
                Triggers = true,
            };

            // If the user selected any of the specific database objects to script then disable the 'All' flag.
            if (parameters.DbObjects)
            {
                parameters.All = false;
            }
            else
            {
                parameters.DbObjects = true;
            }

            // Delete the output file if it exists.
            File.Delete(_scriptOptions.FileName);

            // Prefetch some database objects in an effort to speed up the scripting process.
            _db.PrefetchObjects(typeof(Schema), _scriptOptions);
            _db.PrefetchObjects(typeof(Table), _scriptOptions);
            _db.PrefetchObjects(typeof(StoredProcedure), _scriptOptions);
            _db.PrefetchObjects(typeof(UserDefinedFunction), _scriptOptions);
            _db.PrefetchObjects(typeof(View), _scriptOptions);

            // Script the database objects.
            Script(ScriptSchemas, parameters.Schemas, "schema names");
            Script(ScriptTables, parameters.Tables, "tables");
            Script(ScriptIndexes, parameters.Indexes, "non-clustered indexes");
            Script(ScriptForeignKeys, parameters.ForeignKeys, "foreign keys");
            Script(ScriptStoredProcedures, parameters.Procedures, "stored procedures");
            Script(ScriptUserDefinedFunctions, parameters.Functions, "user-defined functions");
            Script(ScriptViews, parameters.Views, "views");
            Script(ScriptTriggers, parameters.Triggers, "triggers");
            Script(ScriptUsers, parameters.Users, "users");

            return 0;
        }

        private static void Script(Action script, bool scriptFlag, string objectName)
        {
            try
            {
                if (scriptFlag)
                {
                    script();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Scripting {objectName} for database {_db.Name}: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static void ScriptForeignKeys()
        {
            _scriptOptions.DriForeignKeys = true;

            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * FOREIGN KEYS                                                              *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (Table table in _db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    foreach (ForeignKey foreignKey in table.ForeignKeys)
                    {
                        File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Foreign Key [{foreignKey.Name}]  ******/" + Environment.NewLine);
                        foreignKey.Script(_scriptOptions);
                    }
                }
            }

            _scriptOptions.DriForeignKeys = false;
        }

        private static void ScriptIndexes()
        {
            _scriptOptions.DriNonClustered = true;

            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * INDEXES                                                                   *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (Table table in _db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    foreach (Index index in table.Indexes)
                    {
                        if (!index.IsSystemObject && !index.IsClustered)
                        {
                            File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Index [{index.Name}]  ******/" + Environment.NewLine);
                            index.Script(_scriptOptions);
                        }
                    }
                }
            }

            _scriptOptions.DriNonClustered = false;
        }

        private static void ScriptSchemas()
        {
            _scriptOptions.Permissions = true;

            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * SCHEMAS                                                                   *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (Schema schema in _db.Schemas)
            {
                if (!schema.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Schema [{schema.Name}]  ******/" + Environment.NewLine);
                    schema.Script(_scriptOptions);
                }
            }

            _scriptOptions.Permissions = false;
        }

        private static void ScriptStoredProcedures()
        {
            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * STORED PROCEDURES                                                         *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (StoredProcedure storedProcedure in _db.StoredProcedures)
            {
                if (!storedProcedure.IsSystemObject && !storedProcedure.Name.StartsWith("sp_"))
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  StoredProcedure [{storedProcedure.Schema}].[{storedProcedure.Name}]  ******/" + Environment.NewLine);
                    storedProcedure.Script(_scriptOptions);
                }
            }
        }

        private static void ScriptTables()
        {
            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * TABLES                                                                    *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (Table table in _db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Table [{table.Schema}].[{table.Name}]  ******/" + Environment.NewLine);
                    table.Script(_scriptOptions);
                }
            }
        }

        private static void ScriptTriggers()
        {
            _scriptOptions.Triggers = true;

            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * TRIGGERS                                                                  *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            // Script database level triggers first.
            foreach (DatabaseDdlTrigger trigger in _db.Triggers)
            {
                if (!trigger.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Trigger [{trigger.Name}]  ******/" + Environment.NewLine);
                    trigger.Script(_scriptOptions);
                }
            }

            // Now script the table level triggers.
            foreach (Table table in _db.Tables)
            {
                if (!table.IsSystemObject)
                {
                    foreach (Trigger trigger in table.Triggers)
                    {
                        if (!trigger.IsSystemObject)
                        {
                            File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Trigger [{trigger.Name}]  ******/" + Environment.NewLine);
                        }
                    }
                }
            }

            _scriptOptions.Triggers = false;
        }

        private static void ScriptUserDefinedFunctions()
        {
            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * USER-DEFINED FUNCTIONS                                                    *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (UserDefinedFunction userDefinedFunction in _db.UserDefinedFunctions)
            {
                if (!userDefinedFunction.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  Function [{userDefinedFunction.Schema}].[{userDefinedFunction.Name}]  ******/" + Environment.NewLine);
                    userDefinedFunction.Script(_scriptOptions);
                }
            }
        }

        private static void ScriptUsers()
        {
            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * USERS                                                                     *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (User user in _db.Users)
            {
                if (!user.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  User [{user.Name}]  ******/" + Environment.NewLine);
                    user.Script(_scriptOptions);
                }
            }
        }

        private static void ScriptViews()
        {
            StringBuilder header = new StringBuilder()
                .AppendLine()
                .AppendLine("/*****************************************************************************")
                .AppendLine(" * VIEWS                                                                     *")
                .AppendLine(" *****************************************************************************/");
            File.AppendAllText(_scriptOptions.FileName, header.ToString());

            foreach (View view in _db.Views)
            {
                if (!view.IsSystemObject)
                {
                    File.AppendAllText(_scriptOptions.FileName, Environment.NewLine + $"/******  Object:  View [{view.Schema}].[{view.Name}]  ******/" + Environment.NewLine);
                    view.Script(_scriptOptions);
                }
            }
        }
    }
}
