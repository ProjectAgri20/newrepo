using System;
using System.Diagnostics;
using System.IO;

using Microsoft.Office.Interop.Excel;

namespace HP.ScalableTest.Tools
{
    class Program
    {
        static Application _app = null;
        static Workbook _book   = null;
        static Worksheet _sheet = null;

        private static void Usage()
        {
            Console.WriteLine("Usage: SanityTestTool -config <Configuration Excel File path>");
            Console.WriteLine();
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            TraceFactory.SetThreadContextProperty("PID", Process.GetCurrentProcess().Id.ToString());

            CommandLineArguments arguments = new CommandLineArguments(args);

            string configFilePath = arguments["config"];
            if (string.IsNullOrEmpty(configFilePath))
            {
                Usage();
            }

            DateTime startTime = DateTime.Now;

            try
            {
                if (OpenBook(configFilePath))
                {
                    // Read Database name
                    string database = _sheet.get_Range("B1").get_Value().ToString();

                    int startRowIndex = 7;

                    // walk thru each row and execute sanity test
                    while (true)
                    {
                        object val = _sheet.get_Range("A" + startRowIndex.ToString()).get_Value();

                        string temp = string.Empty;
                        if (null != val)
                        {
                            temp = val.ToString();
                        }
                        
                        int index;

                        if (int.TryParse(temp, out index))
                        {
                            // Get row status
                            temp = _sheet.get_Range("E" + startRowIndex.ToString()).get_Value().ToString();
                            bool isEnabled = temp.Equals("Yes", StringComparison.CurrentCultureIgnoreCase);

                            if (isEnabled)
                            {
                                string dispatcher = _sheet.get_Range("B" + startRowIndex.ToString()).get_Value().ToString();
                                string scenario = _sheet.get_Range("C" + startRowIndex.ToString()).get_Value().ToString();

                                string result, sessionID, clientVM;

                                bool status = SanityTestExecution.Start(dispatcher, database, scenario, out clientVM, out sessionID, out result);

                                SetValue("F" + startRowIndex.ToString(), clientVM);
                                SetValue("G" + startRowIndex.ToString(), sessionID);
                                SetValue("H" + startRowIndex.ToString(), status.ToString());
                                SetValue("I" + startRowIndex.ToString(), result);
                            }
                        }
                        else
                        {
                            // assuming the table ends here and exit the loop
                            break;
                        }                        
                        ++startRowIndex;
                    }
                }

                DateTime endTime = DateTime.Now;


                // Update Start and End times
                // Start Time = B2; End Time = B3; Time Taken B4;
                SetValue("B2", startTime.ToString());
                SetValue("B3", endTime.ToString());
                TimeSpan timeTaken = endTime.Subtract(startTime);
                SetValue("B4", timeTaken.Hours  + ":" + timeTaken.Minutes + "(hh::mm)");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error occurred in executing sanity test...");
                Console.WriteLine(e.Message);
            }
            finally
            {
                // Save the work book
                _book.Save();

                // close the work book
                CloseBook();
            }

            Console.ReadLine();
        }

        static void SetValue(string range, string value)
        {
            object objVal  = value;
            object objType = null;

            _sheet.get_Range(range).set_Value(objType, objVal);
        }

        static bool OpenBook(string configFile)
        {
            if (File.Exists(configFile))
            {
                _app   = new Application();
                _book  = _app.Workbooks.Open(configFile, true);
                _sheet = _book.ActiveSheet;

                return true;
            }
            else
            {
                Console.WriteLine("Configuration file {0} doesn't exists.".FormatWith(configFile));
                return false;
            }
        }

        static void CloseBook()
        {
            if (null != _app)
            {
                try
                {
                    _book.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_app);
                    _app = null;
                    _sheet = null;
                }
                catch (Exception ex)
                {
                    _app = null;
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
    }
}
