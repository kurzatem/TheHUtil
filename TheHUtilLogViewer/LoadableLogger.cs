namespace TheHUtilLogViewer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using TheHUtil.Extensions;
    using TheHUtil.HelperConstants;
    using TheHUtil.Logging;

    internal class LoadableLogger
    {
        private List<LoadableLog> logs;

        private string name;

        private string version;

        private LoadableLogger()
        {
            this.logs = new List<LoadableLog>(1031);
        }

        public static LoadableLogger Open(string pathAndName)
        {
            if (!File.Exists(pathAndName))
            {
                throw new FileNotFoundException("File does not exist at this location: " + pathAndName);
            }

            if (Path.GetExtension(pathAndName).Equals("xml", StringComparison.InvariantCultureIgnoreCase))
            {
                return OpenXml(pathAndName);
            }
            else if (Path.GetExtension(pathAndName).Equals("txt", StringComparison.InvariantCultureIgnoreCase))
            {
                return OpenText(pathAndName);
            }
            else
            {
                throw new FileFormatException("Invalid file type given. Currently only accepts xml and txt files.");
            }
        }

        private static LoadableLogger OpenText(string pathAndName, bool supressWarnings = false)
        {
            var result = new LoadableLogger();
            using (var doc = new StreamReader(pathAndName, Encoding.UTF8))
            {
                int nestCount = 0;
                var inputData = new List<string>(23);
                while (!doc.EndOfStream)
                {
                    var line = doc.ReadLine();
                    inputData.Add(line);
                    if (line.StartsWith("Logs for:"))
                    {
                        GetApplicationInfoFromText(result, line);
                    }
                    else
                    {
                        nestCount = GetLogDataFromText(result, nestCount, inputData, line);
                    }
                }

                if (nestCount != 0 && !supressWarnings)
                {
                    throw new EndOfStreamException("File is incomplete. Possibly corrupted or process was terminated prematurely.");
                }
            }
            
            return result;
        }

        private static int GetLogDataFromText(LoadableLogger result, int nestCount, List<string> inputData, string line)
        {
            switch (line)
            {
                case Log.NODE_NAME_LOG:
                    nestCount++;
                    break;
                case Log.TEXT_END_OF_COMMENT:
                case Log.TEXT_END_OF_EXCEPTION:
                    nestCount--;
                    if (nestCount == 0)
                    {
                        result.logs.Add(new LoadableLog(inputData));
                        inputData.Clear();
                    }

                    break;
            }

            return nestCount;
        }

        private static void GetApplicationInfoFromText(LoadableLogger logger, string line)
        {
            line = line.RemoveFirst("Logs for:");
            line = line.RemoveFirst("version");
            var words = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            logger.name = words[0];
            logger.version = words[1];
        }

        private static LoadableLogger OpenXml(string pathAndName)
        {
            var result = new LoadableLogger();
            var doc = XDocument.Load(pathAndName);
            var logs = doc.Elements(Log.NODE_NAME_LOG);
            foreach (var log in logs)
            {
                result.logs.Add(new LoadableLog(log));
            }

            return result;
        }

        public void Save(string pathAndName)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void BindToUI()
        {
            throw new NotImplementedException();
        }
    }
}
