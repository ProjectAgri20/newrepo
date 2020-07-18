using System;
using System.Collections.Generic;
using System.Text;
using HP.ScalableTest.PluginSupport.Print;
using System.Printing;
using HP.ScalableTest.Framework.Plugin;
using System.Reflection;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Fax
{
    class FaxConfigPrintingEngine : PrintingEngine
    {
        private StringBuilder _contentToPrint = new StringBuilder();
        /// <summary>
        /// This is an overridden method, from the Print Support PrintEngine class
        /// PrintTag Virtual Method.
        /// </summary>
        /// <param name="printQueue"></param>
        /// <param name="executionData"></param>
        public override StringBuilder PrintTag(PrintQueue printQueue, PluginExecutionData executionData)
        {
            FaxActivityData data = executionData.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            PrintFaxData(data);
            _contentToPrint.AppendLine();
            _contentToPrint.AppendLine(string.Format("UserName: {0}", Environment.UserName));
            _contentToPrint.AppendLine(string.Format("Session ID: {0}", executionData.SessionId));
            _contentToPrint.AppendLine(string.Format("Activity ID:{0}", executionData.ActivityExecutionId));
            _contentToPrint.AppendLine(string.Format("Date: {0}", DateTime.Now.ToShortDateString()));
            _contentToPrint.AppendLine(string.Format("Time: {0}", DateTime.Now.ToShortTimeString()));
            return _contentToPrint;
        }

        private void PrintFaxData(FaxActivityData _faxData)
        {
            if (_faxData != null)
            {
                _contentToPrint.AppendLine(string.Format("Fax Settings Data"));
                _contentToPrint.AppendLine();

                PropertyInfo[] propertiesFaxData = _faxData.GetType().GetProperties();
                foreach (PropertyInfo propFaxData in propertiesFaxData)
                {
                    if (!((propFaxData.Name.Equals("ScanOptions")) || (propFaxData.Name.Equals("AutomationPause"))))
                    {
                        _contentToPrint.AppendLine(string.Format("{0} : {1}", propFaxData.Name, propFaxData.GetValue(_faxData, null)));
                    }
                }

                PropertyInfo[] propertyFaxOption = _faxData.ScanOptions.GetType().GetProperties();
                foreach (PropertyInfo propFaxOption in propertyFaxOption)
                {
                    if (!(propFaxOption.Name.Equals("LockTimeouts") ||
                        propFaxOption.Name.Equals("FileType") ||
                        propFaxOption.Name.Equals("Color") ||
                        propFaxOption.Name.Equals("Cropping") ||
                        propFaxOption.Name.Equals("ScanModes") ||
                        propFaxOption.Name.Equals("BookLetFormat") || propFaxOption.Name.Equals("BorderOnEachPage") ||
                        propFaxOption.Name.Equals("CreateMultiFile") || propFaxOption.Name.Equals("MaxPageperFile") ||
                        propFaxOption.Name.Equals("SetEraseEdges") || propFaxOption.Name.Equals("ApplySameWidth") || propFaxOption.Name.Equals("MirrorFrontSide") || propFaxOption.Name.Equals("UseInches") ||
                        propFaxOption.Name.Equals("SignOrEncrypt") ||
                        propFaxOption.Name.Equals("OriginalOneSided") || propFaxOption.Name.Equals("OriginalPageflip") ||
                        propFaxOption.Name.Equals("OutputOneSided") || propFaxOption.Name.Equals("OutputPageflip") ||
                        propFaxOption.Name.Equals("Collate") ||
                        propFaxOption.Name.Equals("EdgeToEdge") ||
                        propFaxOption.Name.Equals("ZoomSize") || propFaxOption.Name.Equals("ReduceEnlargeOptions") || propFaxOption.Name.Equals("IncludeMargin") ||
                        propFaxOption.Name.Equals("PaperSelectionPaperSize") || propFaxOption.Name.Equals("PaperSelectionPaperType") || propFaxOption.Name.Equals("PaperSelectionPaperTray") ||
                        propFaxOption.Name.Equals("PagesPerSheetElement") || propFaxOption.Name.Equals("PagesPerSheetAddBorder")))
                    {
                        _contentToPrint.AppendLine(string.Format("{0} : {1}", propFaxOption.Name, propFaxOption.GetValue(_faxData.ScanOptions, null)));
                    }
                }

            }

        }
    }
}
