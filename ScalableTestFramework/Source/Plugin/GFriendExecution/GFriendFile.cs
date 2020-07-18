
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    /// <summary>
    /// Save information for GFriend used file types.
    /// </summary>
    [DataContract]
    public class GFriendFile
    {
        /// <summary>
        /// Name of file without path.
        /// </summary>
        [DisplayName("File Name")]
        [DataMember]
        public string FileName { get; set; }


        /// <summary>
        /// File types. <see cref="GFFileTypes"/>
        /// </summary>
        [DisplayName("File Type")]
        [DataMember]
        public GFFileTypes FileType { get; set; }

        /// <summary>
        /// Contents of files.
        /// </summary>
        [DisplayName("File Preview")]
        [DataMember]
        public string FileContents { get; set; }

        private string _filePath;

        /// <summary>
        /// Constructor. Initialize new GFriendFile.
        /// </summary>
        /// <param name="filePath">Path of file</param>
        /// <param name="fileType">Type of file. <see cref="GFFileTypes"/></param>
        public GFriendFile(string filePath, GFFileTypes fileType)
        {
            _filePath = filePath;
            FileType = fileType;
            FileName = Path.GetFileName(_filePath);
            if(File.Exists(_filePath))
            {
                ReadContents();
            }
            else
            {
                throw new FileNotFoundException("Target file is not found");
            }
        }

        /// <summary>
        /// Read file contents and save.
        /// </summary>
        public void ReadContents()
        {
            using (StreamReader reader = new StreamReader(_filePath))
            {
                FileContents = reader.ReadToEnd();
                reader.Close();
            }
            
        }

    }
}
