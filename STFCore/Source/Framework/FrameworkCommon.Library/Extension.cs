using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Text;
using System.Threading;

namespace HP.ScalableTest
{
    /// <summary>
    /// This is an extension class created to house common extension methods used across the framework.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Formats the string.
        /// </summary>
        /// <param name="value">Format string</param>
        /// <param name="args">arguments to place into the formatted string.</param>
        /// <returns>Completed string.</returns>
        /// <example>
        /// This is a simple helper, but not much different from string.Format()
        /// <code>
        /// TraceFactory.Logger.Debug("STDOUT: {0}".FormatWith(stdOut));
        /// </code>
        /// </example>
        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, value, args);
        }

        /// <summary>
        /// Randomly shuffles the list of items.
        /// </summary>
        /// <typeparam name="T">Any <see cref="IList"/> based collection</typeparam>
        /// <param name="list">The <see cref="IList"/> to be shuffled</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            Random random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Performs a deep copy clone of the specified source object.
        /// </summary>
        /// <typeparam name="T">The object type to be cloned</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>A new copy of the cloned object</returns>
        /// <remarks>
        /// This method uses a serialized and deserialized <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> to create the clone.
        /// </remarks>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Performs a deep copy of the specified source object using Data Contract serialization
        /// Note that while this clone method can result in a more thorough copy, the trade-off is
        /// that it can also take significantly more time to process than via a binary formatter
        /// </summary>
        /// <typeparam name="T">The object type to be cloned</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>A new copy of the cloned object</returns>
        /// <remarks>
        /// This method uses a serialized and deserialized <see cref="System.Runtime.Serialization.DataContractSerializer"/> to create the clone.
        /// </remarks>
        public static T CloneViaDataContract<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var dcs = new System.Runtime.Serialization
                .DataContractSerializer(typeof(T), null, int.MaxValue, false, true, null);
            using (var ms = new System.IO.MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }

        /// <summary>
        /// Get the <see cref="Exception.Message"/>s of the supplied <paramref name="error"/> and all <see cref="Exception.InnerException"/>s.
        /// </summary>
        /// <param name="error">The <see cref="Exception"/>, or <c>null</c>.</param>
        /// <returns>A string with all exceptions down through each inner exception concatenated together</returns>
        public static string JoinAllErrorMessages(this Exception error)
        {
            if (error != null)
            {
                if (error.InnerException != null)
                {
                    return error.Message + Environment.NewLine + "InnerException: " + error.InnerException.JoinAllErrorMessages();
                }

                var reflectionException = error as ReflectionTypeLoadException;
                if (reflectionException != null)
                {
                    return error.Message + Environment.NewLine + "LoaderException(s): " + string.Join(Environment.NewLine, reflectionException.LoaderExceptions.Select(le => le.Message));
                }

                return error.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get a 22-character, case-sensitive GUID as a string.
        /// </summary>
        /// <remarks>
        /// Refer to http://web.archive.org/web/20100408172352/http://prettycode.org/2009/11/12/short-guid/
        /// </remarks>
        public static string ToShortString(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Substring(0, 22)
                .Replace("/", "_")
                .Replace("+", "-");
        }

        /// <summary>
        /// Compresses the designated string into a Base64 string.
        /// </summary>
        /// <param name="text">The string text to be compressed.</param>
        /// <returns>A System.String representing the compress value.</returns>
        public static string Compress(this string text)
        {
            //http://dotnet-snippets.de/snippet/strings-komprimieren-und-dekomprimieren/1058

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                zipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var data = new byte[memoryStream.Length];
            memoryStream.Read(data, 0, data.Length);

            var zipBuffer = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, zipBuffer, 4, data.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, zipBuffer, 0, 4);
            return Convert.ToBase64String(zipBuffer);
        }

        /// <summary>
        /// Decompresses the designated string from a Base64 string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>A System.String representing the uncompressed text.</returns>
        public static string Decompress(this string compressedText)
        {
            byte[] zipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int length = BitConverter.ToInt32(zipBuffer, 0);
                memoryStream.Write(zipBuffer, 4, zipBuffer.Length - 4);

                var buffer = new byte[length];

                memoryStream.Position = 0;
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    zipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}