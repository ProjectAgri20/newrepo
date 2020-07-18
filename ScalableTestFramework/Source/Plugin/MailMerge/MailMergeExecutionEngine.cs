using System;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Print;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;

namespace HP.ScalableTest.Plugin.MailMerge
{
    /// <summary>
    /// Mail Merge controller which does the execution part
    /// </summary>
    public class MailMergeExecutionEngine : IPluginExecutionEngine
    {
        private bool _setupDone = false;
        private MailMergeActivityData _mailMergeData;
        private Word.Application _wrdApp;
        private Word._Document _wrdDoc;
        private Object _oMissing = System.Reflection.Missing.Value;
        private Object _oFalse = false;
        private string _tempDataSourceFileName;

        private PrintQueueInfoCollection _printQueueInfoCollection;
        private string _defaultPrintQueue;

        private static string _userName;
        private string _sessionId;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (!_setupDone)
            {
                AddOfficeRegistryEntries();
                _setupDone = true;
            }

            _mailMergeData = executionData.GetMetadata<MailMergeActivityData>();
            _printQueueInfoCollection = executionData.PrintQueues;
            _userName = executionData.Credential.UserName;
            _sessionId = executionData.SessionId;

            _tempDataSourceFileName = Path.Combine(Path.GetTempPath(), $"{_userName}_{Guid.NewGuid()}_tempDataSourceFile.doc");

            //install the local printer or remote print queue
            // Set up the list of print devices
            if (_printQueueInfoCollection.Count == 0)
            {
                return new PluginExecutionResult(PluginResult.Skipped);
            }

            PrintQueueInfo printQueueInfo = _printQueueInfoCollection.First();
            PrintQueue printQueue = PrintQueueController.Connect(printQueueInfo);

            // Log the device/server that was used for this job, if one was specified
            LogDevice(executionData, printQueueInfo);


            if (_mailMergeData.PrintJobSeparator)
            {
                PrintTag(printQueue, executionData);
            }

            _defaultPrintQueue = printQueue.FullName;

            //generate the mail merge doc and print it to default print queue
            try
            {
                ExecutionServices.CriticalSection.Run(new Framework.Synchronization.LocalLockToken(printQueueInfo.AssociatedAssetId, new TimeSpan(0, 0, 30), new TimeSpan(0, 5, 0)), PrintMailMerge);
            }
            catch (Exception ex)
            {
                return new PluginExecutionResult(PluginResult.Failed, ex.Message);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private static void LogDevice(PluginExecutionData executionData, PrintQueueInfo printer)
        {
            if (!string.IsNullOrEmpty(printer.AssociatedAssetId))
            {
                var log = new ActivityExecutionAssetUsageLog(executionData, printer.AssociatedAssetId);
                ExecutionServices.DataLogger.Submit(log);
            }
        }

        /// <summary>
        /// generate the mail merge document and print it to default print queue
        /// </summary>
        private void PrintMailMerge()
        {
            Word.MailMergeFields wrdMergeFields;

            // Create an instance of Word  and make it visible.
            _wrdApp = new Word.Application { Visible = false };

            // Add a new document.
            _wrdDoc = _wrdApp.Documents.Add(ref _oMissing, ref _oMissing, ref _oMissing, ref _oMissing);
            _wrdDoc.Select();

            var wrdSelection = _wrdApp.Selection;
            var wrdMailMerge = _wrdDoc.MailMerge;

            // Perform mail merge. setup the default queue
            _wrdApp.ActivePrinter = _defaultPrintQueue;

            // Create a MailMerge Data file.
            CreateMailMergeDataFile();

            if (_mailMergeData.Format == MailMergeFormat.Letter)
            {
                GenerateLetter(wrdSelection, wrdMailMerge, out wrdMergeFields);
            }
            else
            {
                try
                {
                    _wrdDoc.PageSetup.PaperSize = Word.WdPaperSize.wdPaperEnvelope10;
                    _wrdDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                    _wrdDoc.PageSetup.BottomMargin = 36.28f;
                    _wrdDoc.PageSetup.RightMargin = 36.28f;
                    _wrdDoc.PageSetup.TopMargin = 36.28f;
                    _wrdDoc.PageSetup.LeftMargin = 36.28f;

                    _wrdDoc.Envelope.DefaultOrientation = Word.WdEnvelopeOrientation.wdLeftLandscape;
                    _wrdDoc.Envelope.DefaultFaceUp = true;
                    GenerateEnvelope(wrdSelection, wrdMailMerge, out wrdMergeFields);
                }
                catch (System.Runtime.InteropServices.COMException comException)
                {
                    _wrdDoc.Close(ref _oFalse, ref _oMissing, ref _oMissing);

                    // Release References.
                    _wrdDoc = null;
                    _wrdApp = null;

                    File.Delete(_tempDataSourceFileName);
                    throw new Exception(comException.Message);

                }
            }

            try
            {
                wrdMailMerge.Destination = Word.WdMailMergeDestination.wdSendToPrinter;
                //wrdMailMerge.HighlightMergeFields = true;
                wrdMailMerge.MainDocumentType = _mailMergeData.Format == MailMergeFormat.Letter ? Word.WdMailMergeMainDocType.wdFormLetters : Word.WdMailMergeMainDocType.wdEnvelopes;
                wrdMailMerge.Execute(ref _oFalse);
            }
            catch (System.Runtime.InteropServices.COMException comException)
            {
                _wrdDoc.Close(ref _oFalse, ref _oMissing, ref _oMissing);

                // Release References.
                _wrdDoc = null;
                _wrdApp = null;

                // Clean up temp file.
                File.Delete(_tempDataSourceFileName);
                throw new Exception(comException.Message);
                //throw new ActivityFailedException("Failed to print the envelopes", comException);
            }

            // Close the original form document.
            _wrdDoc.Saved = true;
            _wrdDoc.Close(ref _oFalse, ref _oMissing, ref _oMissing);

            // Release References.
            _wrdDoc = null;
            _wrdApp = null;

            // Clean up temp file.
            File.Delete(_tempDataSourceFileName);
        }

        private void PrintTag(PrintQueue printQueue, PluginExecutionData executionData)
        {
            StringBuilder strFileContent = new StringBuilder();
            strFileContent.AppendLine();
            strFileContent.AppendLine();
            strFileContent.AppendLine($"UserName: {_userName}");
            strFileContent.AppendLine($"Session ID: {_sessionId}");
            strFileContent.AppendLine($"Actvity ID: {executionData.ActivityExecutionId}");
            strFileContent.AppendLine($"Date: {DateTime.Now.ToShortDateString()}");
            strFileContent.AppendLine($"Time: {DateTime.Now.ToShortTimeString()}");

            string tagfileName = Path.Combine(Path.GetTempPath(), $"{executionData.ActivityExecutionId}.txt");
            File.WriteAllText(tagfileName, strFileContent.ToString(), Encoding.ASCII);

            FilePrinter printer = FilePrinterFactory.Create(tagfileName);
            printer.Print(printQueue);

            File.Delete(tagfileName);
        }

        private void GenerateLetter(Word.Selection wrdSelection, Word.MailMerge wrdMailMerge, out Word.MailMergeFields wrdMergeFields)
        {
            // Create a string and insert it into the document.
            wrdSelection.Font.Bold = 1;
            var strToAdd = _mailMergeData.Originator.Company + Environment.NewLine + _mailMergeData.Originator.Department;
            wrdSelection.ParagraphFormat.Alignment =
                Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wrdSelection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wrdSelection.TypeText(strToAdd);

            InsertLines(4);

            // Insert merge data.
            wrdSelection.Font.Bold = 0;
            wrdSelection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            wrdMergeFields = wrdMailMerge.Fields;
            wrdMergeFields.Add(wrdSelection.Range, "Name");
            wrdSelection.TypeParagraph();
            wrdMergeFields.Add(wrdSelection.Range, "Address");

            InsertLines(2);

            // Right justify the line and insert a date field
            // with the current date.
            wrdSelection.ParagraphFormat.Alignment =
                Word.WdParagraphAlignment.wdAlignParagraphRight;

            Object objDate = "dddd, MMMM dd, yyyy";
            wrdSelection.InsertDateTime(ref objDate, ref _oFalse, ref _oMissing,
                ref _oMissing, ref _oMissing);

            InsertLines(2);

            // Justify the rest of the document.
            wrdSelection.ParagraphFormat.Alignment =
                Word.WdParagraphAlignment.wdAlignParagraphJustify;

            wrdSelection.TypeText("Dear ");
            wrdMergeFields.Add(wrdSelection.Range, "Name");
            wrdSelection.TypeText(",");
            InsertLines(2);

            //insert the messagebody
            wrdSelection.TypeText(_mailMergeData.MessageBody);
            InsertLines(2);

            // Go to the end of the document.
            Object oConst1 = Word.WdGoToItem.wdGoToLine;
            Object oConst2 = Word.WdGoToDirection.wdGoToLast;
            _wrdApp.Selection.GoTo(ref oConst1, ref oConst2, ref _oMissing, ref _oMissing);
            InsertLines(2);

            strToAdd = "Thank You," + Environment.NewLine + _mailMergeData.Originator.Name + Environment.NewLine + _mailMergeData.Originator.Designation;
            wrdSelection.TypeText(strToAdd);
        }

        private void GenerateEnvelope(Word.Selection wrdSelection, Word.MailMerge wrdMailMerge, out Word.MailMergeFields wrdMergeFields)
        {
            // Create a string and insert it into the document.
            wrdSelection.Font.Bold = 1;
            var strToAdd = _mailMergeData.Originator.Name + Environment.NewLine + _mailMergeData.Originator.Designation + Environment.NewLine + _mailMergeData.Originator.Department + ", " + _mailMergeData.Originator.Company;
            wrdSelection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            wrdSelection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wrdSelection.TypeText(strToAdd);
            InsertLines(2);

            Object oConst1 = Word.WdGoToItem.wdGoToLine;
            Object oConst2 = Word.WdGoToDirection.wdGoToLast;
            _wrdApp.Selection.GoTo(ref oConst1, ref oConst2, ref _oMissing, ref _oMissing);

            wrdSelection.Font.Bold = 0;
            wrdSelection.ParagraphFormat.Alignment =
                Word.WdParagraphAlignment.wdAlignParagraphRight;
            wrdMergeFields = wrdMailMerge.Fields;
            wrdMergeFields.Add(wrdSelection.Range, "Name");
            wrdSelection.TypeParagraph();
            wrdMergeFields.Add(wrdSelection.Range, "Address");
        }

        /// <summary>
        /// this is the datasource file for the mail merge activity
        /// </summary>
        private void CreateMailMergeDataFile()
        {
            int iCount;

            Object oName = _tempDataSourceFileName;
            Object oHeader = "Name, Address";
            _wrdDoc.MailMerge.CreateDataSource(ref oName, ref _oMissing,
                ref _oMissing, ref oHeader, ref _oMissing, ref _oMissing,
                ref _oMissing, ref _oMissing, ref _oMissing);

            // Open the file to insert data.
            Word._Document oDataDoc = _wrdApp.Documents.Open(ref oName, ref _oMissing,
                ref _oMissing, ref _oMissing, ref _oMissing, ref _oMissing,
                ref _oMissing, ref _oMissing, ref _oMissing, ref _oMissing,
                ref _oMissing, ref _oMissing, ref _oMissing, ref _oMissing,
                ref _oMissing/*, ref oMissing */);

            for (iCount = 1; iCount < _mailMergeData.RecipientCollection.Count; iCount++)
            {
                oDataDoc.Tables[1].Rows.Add(ref _oMissing);
            }
            // Fill in the data.
            for (int i = 0; i < _mailMergeData.RecipientCollection.Count; i++)
            {
                FillRow(oDataDoc, i + 2, _mailMergeData.RecipientCollection[i].Name, _mailMergeData.RecipientCollection[i].Address);
            }

            // Save and close the file.
            oDataDoc.Save();
            oDataDoc.Close(ref _oFalse, ref _oMissing, ref _oMissing);
        }

        /// <summary>
        /// function to insert empty lines in the word document
        /// </summary>
        /// <param name="lineNum"></param>
        private void InsertLines(int lineNum)
        {
            int iCount;

            // Insert "LineNum" blank lines.
            for (iCount = 1; iCount <= lineNum; iCount++)
            {
                _wrdApp.Selection.TypeParagraph();
            }
        }

        /// <summary>
        /// function to fill up the table
        /// </summary>
        /// <param name="oDoc"></param>
        /// <param name="row"></param>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        private static void FillRow(Word._Document oDoc, int row, string text1, string text2)
        {
            // Insert the data into the specific cell.
            oDoc.Tables[1].Cell(row, 1).Range.InsertAfter(text1);
            oDoc.Tables[1].Cell(row, 2).Range.InsertAfter(text2);
        }

        internal static void AddOfficeRegistryEntries()
        {
            // Prevent the "User Name" popup from appearing
            using (RegistryKey userInfoKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Office\Common\UserInfo"))
            {
                // Check for values.
                object userName = userInfoKey.GetValue("UserName");
                if (userName == null)
                {
                    ExecutionServices.SystemTrace.LogInfo($"Adding UserName, UserInitials, and Company registry values to {userInfoKey.Name}");
                    userInfoKey.SetValue("UserName", _userName, RegistryValueKind.String);
                    userInfoKey.SetValue("UserInitials", _userName[0], RegistryValueKind.String);
                    userInfoKey.SetValue("Company", "", RegistryValueKind.String);
                }
            }

            // Prevent the "Welcome to the 2010 MS Office System" dialog box from showing, will ensure that this is extended to office 2013 in coming days
            using (RegistryKey generalKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Office\14.0\Common\General"))
            {
                int shownOptInValue = (int)generalKey.GetValue("ShownFirstRunOptin", 0);
                if (shownOptInValue == 0)
                {
                    ExecutionServices.SystemTrace.LogInfo($"Adding ShownOptIn value to {generalKey.Name}");
                    generalKey.SetValue("ShownFirstRunOptin", 1, RegistryValueKind.DWord);
                }
            }
        }
    }
}