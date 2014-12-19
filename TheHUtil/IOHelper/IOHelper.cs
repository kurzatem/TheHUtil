namespace TheHUtil.IOHelper
{
    using System;
    using System.IO;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="IOHelper"/> class.
    /// </summary>
    /// <remarks>The methods in this class have logging enabled, but it will not stop any exceptions from being thrown. One major exception is that if a file is not found, there will be no exception thrown.</remarks>
    public static class IOHelper
    {
        /// <summary>
        /// Gets the information about a file that theoretically is located at the given string.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="filePath">The optional path to the file.</param>
        /// <returns>An instance of the <see cref="FileInfo"/> class for the desired file.</returns>
        private static FileInfo GetFileInfo(string fileName, string filePath)
        {
            string pathAndName;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                pathAndName = fileName;
            }
            else
            {
                pathAndName = Path.Combine(filePath, fileName);
            }

            var fileInfo = new FileInfo(pathAndName);
            return fileInfo;
        }

        /// <summary>
        /// Creates a stream for a given file. This does the logging as well.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="filePath">The path of the file to open.</param>
        /// <param name="mode">The style in which to open the file.</param>
        /// <returns>A <see cref="Stream"/> instance for reading or writing a file.</returns>
        private static FileStream GetStreamWithLogging(string fileName, string filePath, FileMode mode)
        {
            var fileInfo = GetFileInfo(fileName, filePath);

            if (fileInfo.Exists)
            {
                return fileInfo.Open(mode);
            }
            else
            {
                if (mode == FileMode.Create || mode == FileMode.CreateNew || mode == FileMode.OpenOrCreate)
                {
                    return fileInfo.Create();
                }
                else
                {
                    throw new UnauthorizedAccessException("File does not exist and creating is not allowed.");
                }
            }
        }

        /// <summary>
        /// Reads the file and outputs it's contents to a string.
        /// </summary>
        /// <param name="name">The name of the file to read.</param>
        /// <param name="path">The path to the file.</param>
        /// <returns>A string containing the contents of the file unless the file does not exist. In that case, null will be returned</returns>
        public static string ReadFile(string fileName, string filePath = null)
        {
            using (var stream = new StreamReader(GetStreamWithLogging(fileName, filePath, FileMode.Open)))
            {
                return stream.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets a <see cref="FileStream"/> of a desired file or creates it if it does not exist.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns>An instance of the <see cref="FileStream"/> class for reading or writing a file.</returns>
        public static FileStream GetFileStream(string fileName, FileMode mode, string filePath = null)
        {
            return GetStreamWithLogging(fileName, filePath, mode);
        }
    }
}
