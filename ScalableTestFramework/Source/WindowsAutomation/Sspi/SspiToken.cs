using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A token obtained as a result of an SSPI operation.
    /// </summary>
    public sealed class SspiToken
    {
        /// <summary>
        /// Gets the raw bytes that make up the token.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Array is intended to be modified.")]
        public byte[] Token { get; }

        /// <summary>
        /// Gets the token represented as a base 64 string.
        /// </summary>
        public string TokenString
        {
            get { return Token != null ? Convert.ToBase64String(Token) : null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SspiToken" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="ArgumentNullException"><paramref name="token" /> is null.</exception>
        public SspiToken(byte[] token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            Token = new byte[token.Length];
            Array.Copy(token, Token, Token.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SspiToken" /> class.
        /// </summary>
        /// <param name="base64String">The token represented as a base 64 string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="base64String" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public SspiToken(string base64String)
        {
            Token = Convert.FromBase64String(base64String);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SspiToken" /> class.
        /// </summary>
        /// <param name="tokenBuffer">The token buffer.</param>
        internal SspiToken(SecureBuffer tokenBuffer)
        {
            if (tokenBuffer.Length > 0)
            {
                Token = new byte[tokenBuffer.Length];
                Array.Copy(tokenBuffer.Buffer, Token, Token.Length);
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return TokenString;
        }
    }
}
