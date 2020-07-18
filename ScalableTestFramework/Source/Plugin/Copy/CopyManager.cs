using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.Copy;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Copy
{
    public class CopyManager : ScanActivityManager
    {

        private readonly CopyData _data;
        private ICopyApp _copyApp;

        protected override string ScanType => "Copy";

        public CopyManager(PluginExecutionData executionData, ScanOptions scanOptions)
            : base(executionData)
        {
            _data = executionData.GetMetadata<CopyData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the copy job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);

            // Load the copy application            
            _copyApp = CopyAppFactory.Create(device);

            _copyApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _copyApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);

            AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            //Checking whether to launch from Quickset or Not.
            if (_data.UseQuickset)
            {
                if (!_data.LaunchQuicksetFromApp)
                {
                    _copyApp.LaunchFromQuickSet(Authenticator, am, _data.QuickSetName);
                }
                else
                {
                    _copyApp.Launch(Authenticator, am);
                    _copyApp.SelectQuickSet(_data.QuickSetName);
                }
            }
            else
            {
                UpdateStatus("Starting execution of Launch()...");
                _copyApp.Launch(Authenticator, am);
                Type optionsType = _copyApp.Options.GetType();
                
                if (optionsType.BaseType.Equals(typeof(HP.ScalableTest.DeviceAutomation.Helpers.JediOmni.JediOmniJobOptionsManager)))
                {
                    SetOptions();
                }
                else
                {
                    SetOptionsWindjammer();
                }
            }

            // Set job build
            _copyApp.Options.SetJobBuildState(this.UseJobBuild);
        }

        private void SetOptions()
        {

            //Apply settings from the configuration
            // Select the appropriate color
            _copyApp.Options.SetColor(_data.Color);            

            // Select the appropriate number of copies    
            _copyApp.Options.SetNumCopies(_data.Copies);
            UpdateStatus("Option SetNumCopies() has been set...");


            string errorLog = string.Empty;
            _copyApp.Options.SetCollate(_data.Options.Collate);
            UpdateStatus("Option SetCollate() has been set...");

            //The below code is to update the log with any error messages we encouter while running the Options . 
            if (!String.IsNullOrEmpty(errorLog))
                UpdateStatus($"Some exception in SetCollate with message - {errorLog}");


            _copyApp.Options.SetEdgeToEdge(_data.Options.EdgeToEdge);
            UpdateStatus("Option SetEdgeToEdge() has been set...");

            //Have added this line of code to smooth the flow of normal job execution at windzammer device, as when user does not select any options
            if (!_data.Options.Orientation.Equals(ContentOrientation.None))
            {
                _copyApp.Options.SetOrientation(_data.Options.Orientation);
                UpdateStatus("Option SetOrientation() has been set...");
            }
            if (!_data.Options.OptimizeTextPicOptions.Equals(OptimizeTextPic.None))
            {
                _copyApp.Options.SelectOptimizeTextOrPicture(_data.Options.OptimizeTextPicOptions);
                UpdateStatus("Option SelectOptimizeTextOrPicture() has been set...");
            }
            try
            {
                _copyApp.Options.SetReduceEnlarge(_data.Options.ReduceEnlargeOptions, _data.Options.IncludeMargin, _data.Options.ZoomSize);
                UpdateStatus("Option SetReduceEnlarge() has been set...");
            }
            catch (NotImplementedException)
            {
                //Ignore - not implemented for WJ
            }

            if (_data.Options.SetSides)
            {
                _copyApp.Options.SetSides(_data.Options.OriginalOneSided, _data.Options.OutputOneSided, _data.Options.OriginalPageflip, _data.Options.OutputPageflip);
                UpdateStatus("Option SetSides() has been set...");
            }

                #region SetStamps
            if (_data.Options.setStamps)
            {                
                Dictionary<StampType, string> stampList = new Dictionary<StampType, string>();
                foreach (StampContents stamp in _data.Options.StampContents)
                {
                    if (!stamp.StampContentType.ToUpper().Equals("NONE"))
                    {
                        string streRmovespace = String.Empty;
                        stampList[stamp.StampType] = stamp.StampContentType;
                    }
                }
                if (stampList.Count > 0) // Execute only when any of the stamp types is set to a value other than NONE
                {
                    _copyApp.Options.SetStamps(stampList);
                    UpdateStatus("Option SetStamps() has been set...");
                }
            }
            #endregion
            //Have added this line of code to smooth the flow of normal job execution at windzammer device, as when user does not select any options

            _copyApp.Options.SetScanMode(_data.Options.ScanModes);
            UpdateStatus("Options SetScanMode() has been set...");

            if (_data.Options.BookLetFormat)
            {
                errorLog = string.Empty;
                _copyApp.Options.SetBooklet(_data.Options.BookLetFormat, _data.Options.BorderOnEachPage);
                UpdateStatus("Option SetBooklet() has been set...");
            }

            //The below code is to update the log with any error messages we encouter while running the Options . 
            if (!String.IsNullOrEmpty(errorLog))
                UpdateStatus($"Some exception in SetBooklet with message - {errorLog}");


            //Execute the below code onlt when Watermark text is set to a value other than empty dtring or NONE
            if (!string.IsNullOrEmpty(_data.Options.WatermarkText))
            {
                _copyApp.Options.SetWaterMark(_data.Options.WatermarkText);
                UpdateStatus("Option SetWaterMark() has been set...");

            }
            #region Erase edges
            if (_data.Options.SetEraseEdges)
            {
                Dictionary<EraseEdgesType, decimal> eraseEdgeList = new Dictionary<EraseEdgesType, decimal>();
                foreach (var prop in _data.Options.EraseEdgesValue.GetType().GetProperties())
                {
                    if(prop.GetValue(_data.Options.EraseEdgesValue, null).ToString() != string.Empty)
                    {
                        decimal propValue = Convert.ToDecimal(prop.GetValue(_data.Options.EraseEdgesValue, null));
                        if (propValue > 0 && !propValue.Equals(0.00))
                        {
                            foreach (EraseEdgesType edge in Enum.GetValues(typeof(EraseEdgesType)))
                            {
                                if (string.Equals(edge.ToString(), prop.Name, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    eraseEdgeList.Add(edge, propValue);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (eraseEdgeList.Count > 0) //Execute the Set Erase edges only when there is a value for any position which is not equal to 0.00
                {
                    UpdateStatus("Option SetEraseEdges() has been set...");
                    _copyApp.Options.SetEraseEdges(eraseEdgeList, _data.Options.ApplySameWdith, _data.Options.MirrorFrontSide, _data.Options.UseInches);
                }
            }
            #endregion
            #region Pages per sheet
            //Execute this Option when the Pages Per Sheet checkbox is checked
            if (_data.Options.SetPagesPerSheet)
            {
                errorLog = String.Empty;
                _copyApp.Options.SetPagesPerSheet(_data.Options.PagesPerSheetElement, _data.Options.PagesPerSheetAddBorder);
                UpdateStatus("Option SetPagesPerSheet() has been set ...");
                //The below code is to update the log with any error messages we encouter while running the Options . 
                if (!string.IsNullOrEmpty(errorLog))
                    UpdateStatus($"Some exception in SetPagesPerSheet with message - {errorLog}");
            }
            #endregion
            #region original Size
            //Execute this options only when there is a user selection on the UI . Do not execute when NULL or empty string or when Any is selected. The default on the device is 'Any'

            _copyApp.Options.SetOriginalSize(_data.Options.OriginalSizeType);
            UpdateStatus("Options SetOriginalSize()is set ...");

            #endregion
            #region Paper Selection
            //Have added this line of code to smooth the flow of normal job execution at windzammer device, as when user does not select any options
            //Execute the Set Paper selection only when something is changed for Paper size, paper type or Tray type . 

            _copyApp.Options.SetPaperSelection(_data.Options.PaperSelectionPaperSize, _data.Options.PaperSelectionPaperType, _data.Options.PaperSelectionPaperTray);
            UpdateStatus("Option SetPaperSelection() has been set...");

            #endregion
            #region Image resolution
            //Below IF condition is a check for the defaults on the device . It is only executed when the user changes these values which is not equal to the default
            if (_data.Options.SetImageAdjustment)
            {
                _copyApp.Options.SetImageAdjustments(_data.Options.ImageAdjustSharpness, _data.Options.ImageAdjustDarkness, _data.Options.ImageAdjustContrast, _data.Options.ImageAdjustbackgroundCleanup);
                UpdateStatus("Option SetImageAdjustments() has been set ...");
            }
            #endregion
        }

        private void SetOptionsWindjammer()
        {
            _copyApp.Options.SetColor(_data.Color);

            if (_data.Copies > 1)
            {
                _copyApp.Options.SetNumCopies(_data.Copies);
                UpdateStatus("Option SetNumCopies() has been set...");
            }

            if (_data.Options.Collate == false) // Execute when user unchecks this options . The default is true
            {
                string errorLog = String.Empty;
                _copyApp.Options.SetCollate(_data.Options.Collate);
                UpdateStatus("Option SetCollate() has been set...");
                
                if (!String.IsNullOrEmpty(errorLog))
                    UpdateStatus($"Some exception in SetCollate with message - {errorLog}");
            }

            if (_data.Options.EdgeToEdge) //Execute when set to false 
            {
                _copyApp.Options.SetEdgeToEdge(_data.Options.EdgeToEdge);
                UpdateStatus("Option SetEdgeToEdge() has been set...");
            }
            
            if (!_data.Options.Orientation.Equals(ContentOrientation.None))
            {
                _copyApp.Options.SetOrientation(_data.Options.Orientation);
                UpdateStatus("Option SetOrientation() has been set...");
            }

            if (!_data.Options.OptimizeTextPicOptions.Equals(OptimizeTextPic.None))
            {
                _copyApp.Options.SelectOptimizeTextOrPicture(_data.Options.OptimizeTextPicOptions);
                UpdateStatus("Option SelectOptimizeTextOrPicture() has been set...");
            }

        }

        /// <summary>
        /// Finish up the copy job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            // Start the job            
            try
            {
                _copyApp.WorkflowLogger = WorkflowLogger;
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (this.UseJobBuild)
                {
                    options.JobBuildSegments = _data.PageCount;
                }

                if (_copyApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }
    }
}
