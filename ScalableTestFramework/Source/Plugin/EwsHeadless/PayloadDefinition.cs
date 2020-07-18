using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Payload Definition Class
    /// </summary>
    public class PayloadDefinition
    {
        /// <summary>
        /// Payload string
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Target URL
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// Get, Put, Post etc.
        /// </summary>
        public HttpVerb HttpMethod { get; set; }

        /// <summary>
        /// Indicates whether the operation is Upload
        /// </summary>
        public bool IsUpload { get; set; }

        /// <summary>
        /// Indicates whether the operation is to remove the solution
        /// </summary>
        public bool IsRemoveSolution { get; set; }

        /// <summary>
        /// Indicates if Sign on is required
        /// </summary>
        public bool IsSessionIdRequired { get; set; }

        /// <summary>
        /// Indicates if ViewState needs to be sent
        /// </summary>
        public bool IsViewStateRequired { get; set; }
        /// <summary>
        /// Indicates if this is a wizard operation
        /// </summary>
        public bool IsWizardIdRequired { get; set; }

        /// <summary>
        /// Indicates if Hide Id is required
        /// </summary>
        public bool IsHideRequired { get; set; }

        /// <summary>
        /// The URL to capture the ViewState
        /// </summary>
        public string ViewStateUrl { get; set; }

        /// <summary>
        /// the URL to capture wizard Id
        /// </summary>
        public string WizardIdUrl { get; set; }

        /// <summary>
        /// the URL to capture Hide Id
        /// </summary>
        public string HideUrl { get; set; }

        /// <summary>
        /// Header collection to be sent
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Form data to be sent
        /// </summary>
        public NameValueCollection NameValuePairs { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayloadDefinition()
        {
            Headers = new Dictionary<string, string>();
            NameValuePairs = new NameValueCollection();
        }

        /// <summary>
        /// Prepares the Payload
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string PreparePayload(EwsRequest request)
        {
            var configValues = request?.PayloadValues;
            var allParamsInSchema = request?.SchemaParemeters;

            var filledPayload = Payload;
            if (configValues?.Count <= 0)
                return filledPayload;

            foreach (string item in configValues.Keys)
            {
                filledPayload = configValues[item].Equals("donotsend") ? filledPayload.Replace("" + item + "={" + item + "}&", string.Empty) : filledPayload.Replace("{" + item + "}", HttpUtility.UrlEncode(configValues[item]));

                if (allParamsInSchema.Contains(item))
                {
                    allParamsInSchema.Remove(item);
                }
            }
            if (allParamsInSchema.Count <= 0)
                return filledPayload;

            foreach (string param in allParamsInSchema)
            {
                filledPayload = filledPayload.Replace("" + param + "={" + param + "}&", string.Empty);
                filledPayload = filledPayload.Replace("<dd:" + param + ">{" + param + "}</dd:" + param + ">", string.Empty);
            }
            return filledPayload;
        }
    }
}