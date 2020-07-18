using System;
using System.Collections.Generic;
using System.Text;
using HP.ScalableTest.PluginSupport.Print;
using System.Printing;
using HP.ScalableTest.Framework.Plugin;
using System.Reflection;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Copy
{
    public class CopyConfigPrintingEngine : PrintingEngine
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
            CopyData data = executionData.GetMetadata<CopyData>(ConverterProvider.GetMetadataConverters());
            PrintCopyData(data);
            _contentToPrint.AppendLine();
            _contentToPrint.AppendLine(string.Format("UserName: {0}", Environment.UserName));
            _contentToPrint.AppendLine(string.Format("Session ID: {0}", executionData.SessionId));
            _contentToPrint.AppendLine(string.Format("Activity ID:{0}", executionData.ActivityExecutionId));
            _contentToPrint.AppendLine(string.Format("Date: {0}", DateTime.Now.ToShortDateString()));
            _contentToPrint.AppendLine(string.Format("Time: {0}", DateTime.Now.ToShortTimeString()));
            return _contentToPrint;
        }
        private void PrintCopyData(CopyData _copyData)
        {
            if (_copyData != null)
            {
                _contentToPrint.AppendLine(string.Format("Copy Settings Data"));
                _contentToPrint.AppendLine();

                PropertyInfo[] propertiesCopyData = _copyData.GetType().GetProperties();
                foreach (PropertyInfo propCopyData in propertiesCopyData)
                {
                    if (!((propCopyData.Name.Equals("Options")|| (propCopyData.Name.Equals("_printingActivityData")) || propCopyData.Name.Equals("LockTimeouts") || propCopyData.Name.Equals("Copies") || propCopyData.Name.Equals("AutomationPause"))))
                    {
                        _contentToPrint.AppendLine(
                    string.Format("{0} : {1}", propCopyData.Name, propCopyData.GetValue(_copyData, null)));

                    }
                }

                PropertyInfo[] propertyCopyOption = _copyData.Options.GetType().GetProperties();
                foreach (PropertyInfo propCopyOption in propertyCopyOption)
                {
                    if (propCopyOption.Name.Equals("StampContents"))
                    {
                        List<StampContents> contents = (List<StampContents>)propCopyOption.GetValue(_copyData.Options, null);
                        if (_copyData.Options != null)
                        {
                            if (contents.Count > 0)
                            {
                                foreach (StampContents content in contents)
                                {
                                    _contentToPrint.AppendLine(string.Format("StampType : {0}", content.StampType.ToString()));
                                    _contentToPrint.AppendLine(string.Format("StampContentType : {0}", content.StampContentType.ToString()));
                                }
                            }
                        }
                    }
                    else if(!(propCopyOption.Name.Equals("OptimizeTextPicture")))
                    {
                        _contentToPrint.AppendLine(
                        string.Format("{0} : {1}", propCopyOption.Name, propCopyOption.GetValue(_copyData.Options, null)));
                    }
                }

            }

        }
    }
}
