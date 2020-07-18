
namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Provides methods for communicating with the Embedded Web Server (EWS) on a device.
    /// </summary>
    public interface IEwsCommunicator
    {
        /// <summary>
        /// Creates an <see cref="EwsRequest"/> of the specified type.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <returns>An <see cref="EwsRequest"/>.</returns>
        EwsRequest CreateRequest(string requestType);

        /// <summary>
        /// Creates an <see cref="EwsRequest"/> of the specified type and subtype.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <param name="requestSubtype">The request subtype.</param>
        /// <returns>An <see cref="EwsRequest"/>.</returns>
        EwsRequest CreateRequest(string requestType, string requestSubtype);

        /// <summary>
        /// Submits the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An <see cref="EwsResult" /> containing the result of the HTTP request(s).</returns>
        EwsResult Submit(EwsRequest request);
    }
}
