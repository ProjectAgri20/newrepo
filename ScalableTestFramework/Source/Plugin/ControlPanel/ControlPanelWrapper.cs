using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    public class ControlPanelWrapper
    {
        private readonly string _methodName;
        private readonly Dictionary<string, string> _parameterValues;
        private readonly IDevice _device;
        private readonly NetworkCredential _credential;

        internal event EventHandler<StatusChangedEventArgs> StatusUpdate;
        internal event EventHandler<ScreenCaptureEventArgs> ScreenCapture;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="functionName"></param>
        /// <param name="parameterValues"></param>
        public ControlPanelWrapper(DeviceInfo deviceInfo, string functionName, Dictionary<string, string> parameterValues)
        {
            _methodName = functionName;
            _parameterValues = parameterValues;

            try
            {
                _device = DeviceConstructor.Create(deviceInfo);
            }
            catch (DeviceSecurityException)
            {
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public ControlPanelWrapper(IDevice device, NetworkCredential credential, string functionName, Dictionary<string, string> parameterValues)
        {
            _device = device;
            _methodName = functionName;
            _parameterValues = parameterValues;
            _credential = credential;
        }

        /// <summary>
        /// Execute
        /// </summary>
        public PluginExecutionResult Execute()
        {
            Type deviceType;
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Skipped, "Device is not supported");
            
            if (_device is PhoenixNovaDevice || _device is PhoenixMagicFrameDevice)
            {
                PhoenixWorkflow phoenixWorkFlow = new PhoenixWorkflow(_device, _credential);

                deviceType = phoenixWorkFlow.GetType();
                result = ExecuteMethod(phoenixWorkFlow, deviceType);
            }
            if (_device is SiriusUIv2Device)
            {
                SiriusPentaneWorkflow siriusPentaneWorkFlow = new SiriusPentaneWorkflow((SiriusUIv2Device)_device, _credential);

                deviceType = siriusPentaneWorkFlow.GetType();

                result = ExecuteMethod(siriusPentaneWorkFlow, deviceType);
            }
            if (_device is SiriusUIv3Device)
            {
                SiriusTriptaneWorkFlow siriusTriptaneWorkFlow = new SiriusTriptaneWorkFlow((SiriusUIv3Device)_device, _credential);

                deviceType = siriusTriptaneWorkFlow.GetType();
                result = ExecuteMethod(siriusTriptaneWorkFlow, deviceType);
            }
            if (_device is JediWindjammerDevice)
            {
                JediWorkflow jediWorkFlow = new JediWorkflow((JediWindjammerDevice)_device, _credential);

                deviceType = jediWorkFlow.GetType();
                result = ExecuteMethod(jediWorkFlow, deviceType);
            }
            if (_device is JediOmniDevice)
            {
                OmniWorkflow omniWorkFlow = new OmniWorkflow((JediOmniDevice)_device, _credential);
                omniWorkFlow.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);
                omniWorkFlow.ScreenCapture += (s, e) => UpdateScreenShot(e.ScreenShotImage);
                deviceType = omniWorkFlow.GetType();
                result = ExecuteMethod(omniWorkFlow, deviceType);
            }

            return result;
        }

        private PluginExecutionResult ExecuteMethod(object workFlow, Type deviceType)
        {
            try
            {
                MethodInfo methodInfo = deviceType.GetMethod(_methodName);
                if (methodInfo != null)
                {
                    object[] mparam = GetParameterValues(methodInfo).ToArray();
                    methodInfo.Invoke(workFlow, mparam);
                    return new PluginExecutionResult(PluginResult.Passed);
                }
                return new PluginExecutionResult(PluginResult.Skipped, $"{_methodName} not implemented in {deviceType.Name}");
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, exception.InnerException ?? exception);
            }
        }

        /// <summary>
        /// LoadParamNameValue
        /// </summary>

        private List<object> GetParameterValues(MethodInfo methodInfo)
        {
            List<object> parameterList = new List<object>();
            var parameters = methodInfo.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters.ElementAt(i).ParameterType.IsEnum)
                {
                    parameterList.Add(Enum.Parse(parameters.ElementAt(i).ParameterType.GetTypeInfo(), _parameterValues.Values.ElementAt(i)));
                }
                else if (parameters.ElementAt(i).ParameterType.IsPrimitive)
                {
                    try
                    {
                        parameterList.Add(
                            TypeDescriptor.GetConverter(parameters.ElementAt(i).ParameterType)
                                .ConvertFromInvariantString(_parameterValues.Values.ElementAt(i)));
                    }
                    catch (NotSupportedException)
                    {
                        parameterList.Add(_parameterValues.Values.ElementAt(i));
                    }
                    catch (ArgumentNullException)
                    {
                        parameterList.Add(_parameterValues.Values.ElementAt(i));
                    }
                }
                else
                {
                    parameterList.Add(_parameterValues.Values.ElementAt(i));
                }
            }
            return parameterList;
        }

        private void UpdateStatus(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }

        private void UpdateScreenShot(Image screenImage)
        {
            ScreenCapture?.Invoke(this, new ScreenCaptureEventArgs(screenImage));
        }
    }
}