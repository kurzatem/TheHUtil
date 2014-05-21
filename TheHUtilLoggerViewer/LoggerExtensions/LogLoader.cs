namespace TheHUtilLoggerViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Windows.Forms;

    using TheHUtil.Extensions;
    using TheHUtil.HelperConstants;
    using TheHUtil.IOHelper;
    using TheHUtil.Logging;

    using TheHUtilLoggerViewer.Interfaces;

    public class LogLoader : ILogLoader
    {
        private List<LogData> logs;

        private Dictionary<string, string> classFiles;

        private IView view;

        private string className = typeof(LogLoader).Name;

        public string ProgramName
        {
            get;
            private set;
        }

        public string ProgramVersion
        {
            get;
            private set;
        }

        public string[] LogCollectionListing
        {
            get
            {
                return this.logs.Select(l => l.LogListing).ToArray();
            }
        }

        public LogLoader(IView view)
        {
            this.logs = new List<LogData>(41);
            this.classFiles = new Dictionary<string, string>(23);
            this.view = view;
        }

        public void OpenFile(string nameAndPath, BackgroundWorker worker)
        {
            worker.ReportProgress(0);
            this.logs.Clear();
            worker.ReportProgress(25);
            var fileData = XElement.Parse(IO.ReadFile(nameAndPath));
            worker.ReportProgress(50);
            foreach (var logData in fileData.Elements("Log"))
            {
                this.logs.Add(new LogData(logData, this.view));
            }

            worker.ReportProgress(90);
            this.ProgramName = fileData.Attribute(Logger.NODE_NAME_ASSEMBLY_NAME).Value;
            this.ProgramVersion = fileData.Attribute(Logger.NODE_NAME_VERSION).Value;
            worker.ReportProgress(100);
        }

        public void ShowLog()
        {
            if (!this.view.SelectedLogListing.IsNullOrEmptyOrWhiteSpace())
            {
                var logNumber = this.view.SelectedLogListing.ToInt();
                var log = this.logs[logNumber];
                log.BindUiToData();
                if (!log.ClassFileNameAndPath.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!this.classFiles.ContainsKey(log.ClassFileName))
                    {
                        this.LoadAndCacheFile(log.ClassFileNameAndPath);
                    }

                    if (this.classFiles.ContainsKey(log.ClassFileName))
                    {
                        this.view.ClassFileView = this.classFiles[log.ClassFileName];
                    }
                }

                this.view.LineToHighlight = this.logs[logNumber].ClassFileLine;
            }
        }

        private void LoadAndCacheFile(string nameAndPath)
        {
            try
            {
                Logger.AddToQueue("LogLoader", "nameAndPath: " + nameAndPath, 1);
                this.classFiles.Add(Path.GetFileName(nameAndPath), IO.ReadFile(nameAndPath));
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("File cannot be access with current permissions.", "Unauthorized file access.");
            }
            catch (IOException)
            {
                MessageBox.Show("An unknown IO exception occured. Try selecting the log again to attempt to load the file.", "IO error occured.");
            }
            catch (Exception e)
            {
                Logger.AddToQueue("LogLoader", e);
            }
        }
    }
}
