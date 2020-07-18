namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A status returned from a call to the SSPI API.
    /// </summary>
    internal enum SecurityStatus : uint
    {
        /// <summary>
        /// The function succeeded.
        /// </summary>
        Ok = 0x00000000,

        /// <summary>
        /// The function succeeded. The client/server must send the output token to the server/client and wait for a returned token.
        /// For the client, the returned token is passed in another call to InitializeSecurityContext.
        /// For the server, returned token should be passed in pInput for another call to AcceptSecurityContext.
        /// </summary>
        ContinueNeeded = 0x00090312,

        /// <summary>
        /// The function succeeded. The context must finish building the message and then call the CompleteAuthToken function.
        /// </summary>
        CompleteNeeded = 0x00090313,

        /// <summary>
        /// The function succeeded. The client/server must call CompleteAuthToken and pass the output token to the server/client.
        /// The client waits for a returned token and then makes another call to InitializeSecurityContext;
        /// the server waits for a return token from the client and then makes another call to AcceptSecurityContext.
        /// </summary>
        CompleteAndContinue = 0x00090314,

        /// <summary>
        /// The application is referencing a context that has already been closed.
        /// </summary>
        ContextExpired = 0x00090317,

        /// <summary>
        /// The credentials supplied to the security context were not fully initialized.
        /// </summary>
        CredentialsNeeded = 0x00090320,

        /// <summary>
        /// The function failed. There is not enough memory available to complete the requested action.
        /// </summary>
        InsufficientMemory = 0x80090300,

        /// <summary>
        /// The attempted operation is not supported.
        /// </summary>
        Unsupported = 0x80090302,

        /// <summary>
        /// The specified principle is not known in the authentication system.
        /// </summary>
        TargetUnknown = 0x80090303,

        /// <summary>
        /// The function failed. An error occurred that did not map to an SSPI error code.
        /// </summary>
        InternalError = 0x80090304,

        /// <summary>
        /// The requested security package does not exist.
        /// </summary>
        PackageNotFound = 0x80090305,

        /// <summary>
        /// The caller of the function does not have the necessary credentials.
        /// </summary>
        NotOwner = 0x80090306,

        /// <summary>
        /// The function failed. The token passed to the function is not valid.
        /// </summary>
        InvalidToken = 0x80090308,

        /// <summary>
        /// The client could not be impersonated.
        /// </summary>
        NoImpersonation = 0x8009030B,

        /// <summary>
        /// The logon failed.
        /// </summary>
        LogOnDenied = 0x8009030C,

        /// <summary>
        /// The credentials supplied to the package were not recognized.
        /// </summary>
        UnknownCredentials = 0x8009030D,

        /// <summary>
        /// No credentials are available in the security package.
        /// </summary>
        NoCredentials = 0x8009030E,

        /// <summary>
        /// The function failed. No authority could be contacted for authentication.
        /// </summary>
        NoAuthenticatingAuthority = 0x80090311,

        /// <summary>
        /// The function succeeded. The data in the input buffer is incomplete.
        /// The application must read additional data from the client and call AcceptSecurityContext again.
        /// </summary>
        IncompleteMessage = 0x80090318,

        /// <summary>
        /// The function failed. Channel binding policy was not satisfied.
        /// </summary>
        BadBinding = 0x80090346,

        /// <summary>
        /// The function failed. The handle passed to the function is not valid.
        /// </summary>
        InvalidHandle = 0x80090301
    }

    /// <summary>
    /// Provides extension methods for <see cref="SecurityStatus" />.
    /// </summary>
    internal static class SecurityStatusExtension
    {
        /// <summary>
        /// Determines whether this <see cref="SecurityStatus" /> represents an error.
        /// </summary>
        /// <param name="status">The <see cref="SecurityStatus" />.</param>
        /// <returns><c>true</c> if the specified status represents an error; otherwise, <c>false</c>.</returns>
        public static bool IsError(this SecurityStatus status)
        {
            return (uint)status > 0x80000000;
        }
    }
}
