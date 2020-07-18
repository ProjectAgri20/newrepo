using System.IO;
using System.IO.Compression;
using System.Text;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Contains methods for compressing and decompressing data.
    /// </summary>
    public static class CompressionUtil
    {
        /// <summary>
        /// Compresses the specified string into a byte array.
        /// </summary>
        /// <param name="data">The string data to compress.</param>
        /// <returns>A byte array containing the compressed string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static byte[] Compress(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gs);
                    }
                    return mso.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses the specified byte array into a string.
        /// </summary>
        /// <param name="data">The byte array containing the compressed data.</param>
        /// <returns>A string containing the decompressed data.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Decompress(byte[] data)
        {
            using (var msi = new MemoryStream(data))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        gs.CopyTo(mso);
                    }
                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }
    }
}
