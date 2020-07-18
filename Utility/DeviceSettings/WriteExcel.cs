using System;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;

namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Writes the data to an Excel worksheet.
    /// </summary>
    class WriteExcel
    {
        public DataFims ListDeviceValues { get; set; }
        private int Row { get; set; }

        private readonly Excel.Application _xlApp = new Excel.Application();
        private readonly Excel.Workbook _xlWorkBook;
        private readonly Excel.Worksheet _xlWorkSheet;

        const int SOURCE = 1;
        const int REGION = 2;
        const int ELEMENT = 3;
        const int BEFORE_FIM = 4;
        const int AFTER_FIM = 5;

        public string ErrorMessage { get; private set; }


        public WriteExcel(DataFims lstDeviceValues)
        {
            ListDeviceValues = lstDeviceValues;
            Row = 1;

            _xlWorkBook = _xlApp.Workbooks.Add();
            _xlWorkSheet = _xlWorkBook.Worksheets.get_Item(1);
        }
        public bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(ErrorMessage) == false) ? true : false;
            }
        }
        /// <summary>
        /// Writes the column data to the excel spreadsheet
        /// </summary>
        public void WriteListToExcel()
        {
            WriteHeader();

            Row = 2;

            foreach (DataFim df in ListDeviceValues)
            {
                _xlWorkSheet.Cells[Row, SOURCE] = df.EndPoint;
                _xlWorkSheet.Cells[Row, REGION] = df.Parent;
                _xlWorkSheet.Cells[Row, ELEMENT] = df.Element;
                _xlWorkSheet.Cells[Row, BEFORE_FIM] = df.ValueOrig;
                _xlWorkSheet.Cells[Row, AFTER_FIM] = df.ValueNew;

                if (!df.SameValue)
                {
                    ((Excel.Range)_xlWorkSheet.Rows[Row]).Interior.Color = Color.Yellow;
                }

                Row++;
            }
            _xlWorkSheet.Columns.AutoFit();
        }

        private void WriteHeader()
        {
            _xlWorkSheet.Cells[Row, SOURCE] = "Source";
            _xlWorkSheet.Cells[Row, REGION] = "Region";
            _xlWorkSheet.Cells[Row, ELEMENT] = "Element";
            _xlWorkSheet.Cells[Row, BEFORE_FIM] = "Before FIM Value";
            _xlWorkSheet.Cells[Row, AFTER_FIM] = "After FIM Value";
        }
        /// <summary>
        /// Saves the generated excel worksheet to the given path
        /// </summary>
        /// <param name="pathFile">string</param>
        public void SaveExcel(string pathFile)
        {
            Excel.Range currentRow = _xlWorkSheet.Rows[1];
            currentRow.EntireRow.Font.Bold = true;
            _xlApp.DisplayAlerts = false;
            try
            {
                _xlWorkBook.SaveAs(pathFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                this.ErrorMessage = "Path \"" + pathFile + "\" does not exist.";
                ErrorMessage = ex.Message;
            }
            _xlApp.DisplayAlerts = true;

            _xlWorkBook.Close(false);
            _xlApp.Quit();

            ReleaseExcel();
        }
        /// <summary>
        /// Calls to release the various excel objects.
        /// </summary>
        public void ReleaseExcel()
        {
            ReleaseObject(_xlWorkSheet);
            ReleaseObject(_xlWorkBook);
            ReleaseObject(_xlApp);
        }
        /// <summary>
        /// releases and destroys the given object.
        /// </summary>
        /// <param name="obj">object: ITC Excel Object</param>
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "WriteExcel: Failed to release Excel.\n" + ex.Message;
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
