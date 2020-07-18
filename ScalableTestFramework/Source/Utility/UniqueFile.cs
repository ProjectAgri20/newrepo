using System;
using System.IO;
using System.Text.RegularExpressions;
using HP.ScalableTest.Framework;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Supports creation of uniquely named files on a file system.
    /// </summary>
    public sealed class UniqueFile
    {
        /// <summary>
        /// Gets the unique file ID.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets a <see cref="System.IO.FileInfo" /> object representing the unique file.
        /// </summary>
        public FileInfo FileInfo { get; }

        /// <summary>
        /// Gets the name of the unique file.
        /// </summary>
        public string Name => FileInfo.Name;

        /// <summary>
        /// Gets the full path of the unique file.
        /// </summary>
        public string FullName => FileInfo.FullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueFile" /> class.
        /// </summary>
        /// <param name="id">The unique file ID.</param>
        /// <param name="fileInfo">The <see cref="System.IO.FileInfo" />.</param>
        private UniqueFile(Guid id, FileInfo fileInfo)
        {
            Id = id;
            FileInfo = fileInfo;
        }

        /// <summary>
        /// Creates a uniquely named copy of the specified file in a temporary location.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A <see cref="UniqueFile" /> object representing the uniquely named file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public static UniqueFile Create(string fileName)
        {
            return Create(new FileInfo(fileName));
        }

        /// <summary>
        /// Creates a uniquely named copy of the specified file in a temporary location.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileId">The <see cref="Guid" /> to use for the unique file name.</param>
        /// <returns>A <see cref="UniqueFile" /> object representing the uniquely named file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public static UniqueFile Create(string fileName, Guid fileId)
        {
            return Create(new FileInfo(fileName), fileId);
        }

        /// <summary>
        /// Creates a uniquely named copy of the specified file in a temporary location.
        /// </summary>
        /// <param name="file">A <see cref="System.IO.FileInfo" /> representing the file to copy.</param>
        /// <returns>A <see cref="UniqueFile" /> object representing the uniquely named file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        public static UniqueFile Create(FileInfo file)
        {
            return Create(file, SequentialGuid.NewGuid());
        }

        /// <summary>
        /// Creates a uniquely named copy of the specified file in a temporary location.
        /// </summary>
        /// <param name="file">A <see cref="System.IO.FileInfo" /> representing the file to copy.</param>
        /// <param name="fileId">The <see cref="Guid" /> to use for the unique file name.</param>
        /// <returns>A <see cref="UniqueFile" /> object representing the uniquely named file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        public static UniqueFile Create(FileInfo file, Guid fileId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            // Generate the unique file name
            string fileName = Path.GetFileNameWithoutExtension(file.Name);
            string extension = file.Extension;
            string uniqueFileName = $"__{fileName}_-_{fileId}__{extension}";

            // Create the file in a temporary location
            LogTrace($"Creating unique file: {uniqueFileName}");
            string newPath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            FileInfo uniqueFile = null;
            Retry.WhileThrowing<IOException>(() => uniqueFile = file.CopyTo(newPath), 10, TimeSpan.FromSeconds(1));

            return new UniqueFile(fileId, uniqueFile);
        }

        /// <summary>
        /// Extracts the unique identifier encoded in the file name.
        /// </summary>
        /// <param name="uniqueFileName">The unique file name.</param>
        /// <returns>The unique identifier encoded in the file name.</returns>
        /// <exception cref="FormatException"><paramref name="uniqueFileName" /> is not in the correct format.</exception>
        public static Guid ExtractId(string uniqueFileName)
        {
            string guidRegex = @"\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1}";
            Match match = Regex.Match(uniqueFileName, $"_-_({guidRegex})__");
            if (match.Success)
            {
                return new Guid(match.Groups[1].Value);
            }
            else
            {
                throw new FormatException($"Unable to extract ID from file name '{uniqueFileName}'");
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => FileInfo.ToString();

        /// <summary>
        /// Performs an implicit conversion from <see cref="UniqueFile" /> to <see cref="System.IO.FileInfo" />.
        /// </summary>
        /// <param name="uniqueFile">The unique file.</param>
        /// <returns>The <see cref="System.IO.FileInfo" /> from the <see cref="UniqueFile" />.</returns>
        public static implicit operator FileInfo(UniqueFile uniqueFile) => uniqueFile?.FileInfo;
    }
}
