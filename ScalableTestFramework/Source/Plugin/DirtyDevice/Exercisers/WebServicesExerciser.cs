using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class WebServicesExerciser
    {
        public int MaxRetryCount { get; set; } = 3;

        private readonly DirtyDeviceManager _owner;
        private readonly JediDevice _device;
        private readonly JediWebServices _webServices;
        private readonly Snmp _snmp;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServicesExerciser" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal WebServicesExerciser(DirtyDeviceManager owner, JediDevice device)
        {
            _owner = owner;
            _device = device;
            _webServices = _device.WebServices;
            _snmp = _device.Snmp;
        }

        /// <summary>
        /// Gets the device URN.
        /// </summary>
        /// <returns>WebTicket.</returns>
        public WebServiceTicket GetUrn(string endPoint, string uri)
        {
            WebServiceTicket ticket = _webServices.GetDeviceTicket(endPoint, uri);
            return ticket;
        }

        /// <summary>
        /// Sets the device URN.
        /// </summary>
        /// <param name="timeout">Information to put into device settings.</param>
        public void SetUrn(string endPoint, string uri, WebServiceTicket ticket)
        {
            try
            {
                var getResultTicket = _webServices.PutDeviceTicket(endPoint, uri, ticket, getResult: true);
            }
            catch (WebServicesInvalidOperationException ex) when (StringMatcher.IsMatch("not valid", ex.Message, StringMatch.Contains, true))
            {
                throw new ArgumentOutOfRangeException(nameof(ticket), ticket, "Value is not within the allowable range for the device.");
            }
        }

        internal void Exercise(DirtyDeviceActivityData activityData,AssetAttributes deviceAttributes)
        {
            WebServiceTicket webTicket;

            var callList = new[]
                {
                new
                {
                    EndPoint = "device",
                    Uri = "urn:hp:imaging:con:service:device:DeviceService",
                    GetOrSet = "get",
                },
                }.ToList();

            if (deviceAttributes.HasFlag(AssetAttributes.Scanner))
            {
                callList = new[]
                {
                new
                {
                    EndPoint = "device",
                    Uri = "urn:hp:imaging:con:service:device:DeviceService",
                    GetOrSet = "get",
                },
                new
                {
                    EndPoint = "email",
                    Uri = "urn:hp:imaging:con:service:email:EmailService",
                    GetOrSet = "get;set",
                },
                new
                {
                    EndPoint = "copy",
                    Uri = "urn:hp:imaging:con:service:copy:CopyService",
                    GetOrSet = "get;set",
                },
                }.ToList();
            }

            foreach (var call in callList)
            {
                for (var attempt = 1; attempt <= MaxRetryCount; attempt++)
                {
                    try
                    {
                        _owner.OnUpdateStatus(this, $"  Performing web service GET.  (EndPoint: {call.EndPoint}; Uri: {call.Uri})");
                        webTicket = GetUrn(call.EndPoint, call.Uri);
                        _owner.OnUpdateStatus(this, $"    Device response was {webTicket.ToString().Length} characters.");
                    }
                    catch (Exception x)
                    {
                        _owner.OnUpdateStatus(this, $"  Could not perform web service GET.  (Device: {_device.Address}; EndPoint: {call.EndPoint}; Uri: {call.Uri}; Error: {x.Message})");
                        if (attempt >= MaxRetryCount)
                        {
                            throw;
                        }
                        _owner.OnUpdateStatus(this, $"  Will attempt {MaxRetryCount - attempt} more times.");
                        // Cannot proceed to set that which we did not successfully get.
                        continue;
                    }

                    if (call.GetOrSet.Contains("set"))
                    {
                        try
                        {
                            _owner.OnUpdateStatus(this, $"  Performing web service SET.  (EndPoint: {call.EndPoint}; Uri: {call.Uri})");
                            SetUrn(call.EndPoint, call.Uri, webTicket);
                            // If we got this far successfully, we are done.  We do not need to retry.
                            break;
                        }
                        catch (Exception x)
                        {
                            _owner.OnUpdateStatus(this, $"  Could not perform web service SET.  (Device: {_device.Address}; EndPoint: {call.EndPoint}; Uri: {call.Uri}; Error: {x.Message})");
                            if (attempt >= MaxRetryCount)
                            {
                                throw;
                            }
                            _owner.OnUpdateStatus(this, $"  Will attempt {MaxRetryCount - attempt} more times.");
                        }
                    }
                    else
                    {
                        // If we got this far successfully, we are done.  We do not need to retry.
                        break;
                    }
                }
            }
        }
    }
}
