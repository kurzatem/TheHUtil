namespace TheHUtil.IOHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TheHUtil.Logging;

    public static class IO
    {
        private static string className;

        public static string ClassName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(className))
                {
                    className = typeof(IO).Name;
                }

                return className;
            }
        }

        private static FileInfo GetFileInfo(string name, string path)
        {
            string pathAndName;
            if (string.IsNullOrWhiteSpace(path))
            {
                pathAndName = name;
            }
            else
            {
                pathAndName = Path.Combine(path, name);
            }

            var fileInfo = new FileInfo(pathAndName);
            return fileInfo;
        }

        public static string ReadFile(string name, string path = null)
        {
            var fileInfo = GetFileInfo(name, path);
            if (fileInfo.Exists)
            {
                try
                {
                    using (var stream = new StreamReader(fileInfo.OpenRead()))
                    {
                        return stream.ReadToEnd();
                    }
                }
                catch (UnauthorizedAccessException uae)
                {
                    Logger.AddToQueue(ClassName, uae);
                    throw;
                }
                catch (IOException ioe)
                {
                    Logger.AddToQueue(ClassName, ioe);
                    throw;
                }
            }
            else
            {
                Logger.AddToQueue(ClassName, "File does not exist.", 2);
                return null;
            }
        }

        public static FileStream GetFileStream(string fileName, string filePath = null)
        {
            var fileInfo = GetFileInfo(fileName, filePath);

            if (fileInfo.Exists)
            {
                return fileInfo.Open(FileMode.Open);
            }
            else
            {
                return fileInfo.Create();
            }
        }
    }
}
