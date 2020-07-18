using System;
using Microsoft.Office.Interop.Excel;

namespace WebJetAdminComparisonTool
{
    /// <summary>
    /// Helper class for opening a workbook in Excel and cleaning up afterward.
    /// </summary>
    public class ExcelApp : IDisposable
    {
        private Application _excelApp = null;
        private Workbook _excelBook = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelApp"/> class.
        /// </summary>
        public ExcelApp()
        {
            _excelApp = new Application();
            _excelApp.DisplayAlerts = false;
        }

        /// <summary>
        /// Opens the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public Workbook Open(string filePath)
        {
            _excelBook = _excelApp.Workbooks.Open
                (
                    filePath,
                    UpdateLinks: 0,
                    ReadOnly: true,
                    IgnoreReadOnlyRecommended: true,
                    CorruptLoad: XlCorruptLoad.xlNormalLoad
                );
            return _excelBook;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_excelBook != null)
            {
                _excelBook.Close(XlSaveAction.xlDoNotSaveChanges);
                _excelBook = null;
            }

            if (_excelApp != null)
            {
                _excelApp.Application.Quit();
                _excelApp = null;
            }
        }
    }
}
