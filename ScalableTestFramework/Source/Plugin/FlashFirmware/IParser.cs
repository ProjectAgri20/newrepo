using System.Text.RegularExpressions;

namespace HP.ScalableTest.Plugin.FlashFirmware
{
    /// <summary>
    /// Interface for interacting with the fax emulator.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// The default EFI shell timeout
        /// </summary>
        int DefaultTimeout { get; }

        /// <summary>
        /// Read/Write timeout for the socket
        /// </summary>
        int SocketTimeout { get; set; }

        /// <summary>
        /// Opens a TcpClient socket with a Networkstream wrapper
        /// </summary>
        /// <param name="ipAddress">Opens connection to this IP address</param>
        /// <param name="port">Opens connection to this port</param>
        void Open(string ipAddress, int port);
        
        /// <summary>
        /// Validate device comes up within 10 minutes
        /// </summary>
        void DeviceCanConnect(string IPAddress, int port);

        /// <summary>
        /// Read from stream
        /// </summary>
        /// <returns>A string of contents read from the stream</returns>
        string ReadStream();

        /// <summary>
        /// Write binary data to a stream
        /// </summary>
        /// <param name="data">binary data to send to the stream</param>
        void WriteStream(byte [] data);

        /// <summary>
        /// Write to a stream
        /// </summary>
        /// <param name="command">the string to write to a stream</param>
        void WriteStream(string command);

        /// <summary>
        /// Write to a stream
        /// </summary>
        /// <param name="value">the byte to write to a stream</param>
        void WriteStream(byte value);

        /// <summary>
        /// Clear the stream
        /// </summary>
        void ClearStream();

        /// <summary>
        /// Closes the socket connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Sends Raw Data across a raw socket connection
        /// </summary>
        void WriteSocket(byte[] buffer);     
        
        /// <summary>
        /// Cleanse EFI shell output by removing all control characters.
        /// </summary>
        /// <param name="message">The message to normalize</param>
        /// <returns>The cleansed output</returns>
        string Normalize(string message);

        /// <summary>
        /// Receive data from connection (EFI/CE) until the pattern is matched or alloted time expires or the default time span expires
        /// </summary>
        /// <param name="pattern">Wait until pattern is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        string WaitFor(string pattern);

        /// <summary>
        /// Receive data from connection (EFI/CE) until the pattern is matched or alloted time expires
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="pattern">Wait until pattern is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        string WaitFor(int milliseconds, string pattern);

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="patterns">Wait until one of the given patterns is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        string WaitFor(int milliseconds, Regex[] patterns);

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires and returns 
        /// the line containing the matched pattern;
        /// Throws TimeOutExpection if pattern is not found and an Exception if the line containing pattern can't be found 
        /// </summary>
        /// <param name="pattern">Wait until one of the given patterns is matched</param>
        /// <returns>Returns a line that contains the matched value or throws timeout exception</returns>
        string WaitForLine(string pattern);

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires and returns 
        /// the line containing the matched pattern;
        /// Throws TimeOutExpection if pattern is not found and an Exception if the line containing pattern can't be found 
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="pattern">Wait until one of the given patterns is matched</param>
        /// <returns>Returns a line that contains the matched value or throws timeout exception</returns>
        string WaitForLine(int milliseconds, string pattern);

        /// <summary>
        /// Find matches in Serial output until pattern is matched or timeout
        /// </summary>
        /// <param name="find">Get all matches to this regex pattern</param>
        /// <param name="until">Stop when this regex is found</param>
        /// <param name="milliseconds">Timeout after period in milliseconds</param>
        /// <returns>Returns match collection or throws timeout exception</returns>
        MatchCollection FindUntil(string find, string until, int milliseconds);

        ///<summary>
        ///Sends a command and collects the output until a pattern match is found
        ///</summary>
        ///<param name = "command">The string command to be sent</param>
        ///<param name="pattern">The string pattern to match</param>
        string SendWait(string command, string pattern);

        ///<summary>
        ///Sends a command and collects the output until a pattern match is found or time expires
        ///</summary>
        ///<param name = "command">The string command to be sent</param>
        ///<param name="pattern">The string pattern to match</param>
        ///<param name="milliseconds">The time limit to collect the output and wait for a match</param>
        string SendWait(string command, string pattern, int milliseconds);

        /// <summary>
        /// Parses a delimited string and returns the requested filetype path.
        /// </summary>
        /// <param name="delimitedFileNames">Delimited string containing one or more file paths</param>
        /// <param name="filePath">The full path to the file to upload</param>
        /// <param name="requiredFileType">Either FileType.CSV or FileType.Bundle</param>
        void ParseFileNames(string delimitedFileNames, out string filePath, string requiredFileType);



    }
}

    