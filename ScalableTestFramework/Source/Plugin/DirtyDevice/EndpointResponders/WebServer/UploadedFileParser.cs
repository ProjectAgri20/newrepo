using System;
using System.Collections.Generic;
using System.IO;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// Class used to get UploadedFiles from the input stream
    /// </summary>
    public static class UploadedFileParser
    {
        /// <summary>
        /// Extracts UploadedFiles from inputStream
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static List<UploadedFile> ParseUploadedFiles(Stream inputStream)
        {
            List<UploadedFile> returnValue = new List<UploadedFile>();
            KeyValuePair<string, int[]> line;

            while (inputStream.CanRead)
            {
                line = ReadNextLine(inputStream);
                if (line.Key.Contains("filename="))
                {
                    string filename = line.Key.Substring(line.Key.IndexOf("filename=")).Replace("filename=", "").Trim(';', '\"', ' ', '\r', '\n');
                    line = ReadNextLine(inputStream);
                    string contentType = line.Key.Substring(line.Key.IndexOf("Content-Type:")).Replace("Content-Type:", "").Trim(';', '\"', ' ', '\r', '\n');
                    line = ReadNextLine(inputStream);

                    List<Byte> contents = new List<byte>(); ;
                    while (line.Value.Length > 0)
                    {
                        line = ReadNextLine(inputStream);
                        if (line.Key.Contains("----------Th3_HttP_M1meBOunDaRY_") || line.Key == "") break;
                        foreach (byte cbyte in line.Value)
                        {
                            contents.Add(cbyte);
                        }
                    }

                    //we sometimes get an extra line term at the end
                    int last = contents.Count - 1;
                    int nextToLast = last - 1;
                    if (contents[nextToLast] == (Byte)13 && // '\r'
                        contents[last] == (Byte)10)         // '\n'
                    {
                        contents.RemoveAt(last);
                        contents.RemoveAt(nextToLast);
                    }

                    returnValue.Add(new UploadedFile(filename, contentType, contents.ToArray()));
                }
                if (line.Key == "") break;
            }

            return returnValue;
        }

        private static KeyValuePair<string, int[]> ReadNextLine(Stream stream)
        {
            string line = "";
            List<int> bytes = new List<int>();

            int nextByte = -1;
            //read next line
            while (!line.EndsWith("\n"))
            {
                nextByte = stream.ReadByte();
                if (nextByte == -1) break;
                char nextChar = (char)nextByte;
                line += nextChar;
                bytes.Add(nextByte);
            }


            KeyValuePair<string, int[]> retval = new KeyValuePair<string, int[]>(line, bytes.ToArray());
            return retval;
        }
    }
}


