namespace TheHUtil.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Linq;

    using TheHUtil.Extensions;

    public static class Logger
    {
        /// <summary>
        /// The list of <see cref="Log"/>s that is contained within this <see cref="Logger"/>.
        /// </summary>
        private static List<Log> logs;

        /// <summary>
        /// The assembly that this <see cref="Logger"/> is logging for.
        /// </summary>
        private static Assembly assembly;

        /// <summary>
        /// The version of the <see cref="Assembly"/> that this <see cref="Logger"/> is logging for.
        /// </summary>
        private static FileVersionInfo version;

        /// <summary>
        /// Determines whether the logger will be dumped to a file or to the debugger.
        /// </summary>
        private static bool dumpToFileOrDebugger;

        /// <summary>
        /// Defines the node name used for naming the root node of the xml file.
        /// </summary>
        public const string NODE_NAME_LOGS = "Logs";

        /// <summary>
        /// Defines the node name used for naming the program's starting executable.
        /// </summary>
        public const string NODE_NAME_ASSEMBLY_NAME = "Name";

        /// <summary>
        /// Defines the node name used for the version of the program being logged.
        /// </summary>
        public const string NODE_NAME_VERSION = "Version";

        /// <summary>
        /// Whether or not to include the stack when logging <see cref="Exception"/>s.
        /// </summary>
        public static bool IncludeStack
        {
            get;
            set;
        }

        /// <summary>
        /// Whether or not to include the output of the "ToString" method of any <see cref="Exception"/>s.
        /// </summary>
        public static bool IncludeToStringOutput
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the logger will dump to a designated file, or to the debugger.
        /// </summary>
        /// <value>True: dumps to file. False: dumps to debugger.</value>
        /// <remarks>The debugger option (false) is only available when there is a debugger attached.</remarks>
        public static bool DumpToFileOrDebugger
        {
            get
            {
                return dumpToFileOrDebugger;
            }

            set
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    dumpToFileOrDebugger = value;
                }
                else
                {
                    dumpToFileOrDebugger = true;
                }
            }
        }

        /// <summary>
        /// The logging granularity level to log. The lower the more important the log.
        /// </summary>
        /// <remarks>The scale that is intended is for <see cref="Exception"/>s to be of a low numeric value and comments would occupy higher values. This is minimize the perfomance impact that the program will have when creating the logs.</remarks>
        public static int LoggingLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new static instance of the <see cref="Logger"/> class.
        /// </summary>
        static Logger()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                logs = new List<Log>(1031);
                IncludeStack = true;
                IncludeToStringOutput = true;
                LoggingLevel = 5;
                DumpToFileOrDebugger = true;
            }
            else
            {
                logs = new List<Log>(41);
                IncludeStack = false;
                IncludeToStringOutput = false;
                LoggingLevel = 2;
                DumpToFileOrDebugger = false;
            }


            assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            version = FileVersionInfo.GetVersionInfo(assembly.Location);
        }

        /// <summary>
        /// Adds a <see cref="Log"/> instance to the <see cref="Logger"/>. This disregards logging levels.
        /// </summary>
        /// <subjectAsParameter name="log">The <see cref="Log"/> instance to add.</subjectAsParameter>
        internal static void AddToQueue(Log log)
        {
            logs.Add(log);
        }

        /// <summary>
        /// Initializes and adds a new instance of the <see cref="Log"/> class to the logger.
        /// </summary>
        /// <subjectAsParameter name="loggingObjectName">The name of the type of object to write the log about. NOTE: if this is not included, there will be a performance hit as the object's type is discovered by tracing the stack.</subjectAsParameter>
        /// <subjectAsParameter name="exceptionEntry">The <see cref="Exception"/> to log.</subjectAsParameter>
        /// <subjectAsParameter name="level">The level of the log.</subjectAsParameter>
        public static void AddToQueue(string loggingObjectName, Exception exceptionEntry, int level = 1)
        {
            if (level <= LoggingLevel)
            {
                AddToQueue(new Log(loggingObjectName, exceptionEntry, level, IncludeStack, IncludeToStringOutput));
            }
        }

        /// <summary>
        /// Initializes and adds a new instance of the <see cref="Log"/> class to the logger.
        /// </summary>
        /// <subjectAsParameter name="loggingObjectName">The name of the type of object to write the log about. NOTE: if this is not included, there will be a performance hit as the object's type is discovered by tracing the stack.</subjectAsParameter>
        /// <subjectAsParameter name="commentEntry">The comment to log.</subjectAsParameter>
        /// <subjectAsParameter name="level">The level of the log.</subjectAsParameter>
        public static void AddToQueue(string loggingObjectName, string commentEntry, int level)
        {
            if (level <= LoggingLevel)
            {
                AddToQueue(new Log(loggingObjectName, commentEntry, level));
            }
        }

        /// <summary>
        /// Initializes and adds a new instance of the <see cref="Log"/> class to the logger.
        /// </summary>
        /// <subjectAsParameter name="loggingObjectName">The name of the type of object to write the log about. NOTE: if this is not included, there will be a performance hit as the object's type is discovered by tracing the stack.</subjectAsParameter>
        /// <subjectAsParameter name="exceptionEntry">The <see cref="Exception"/> to log.</subjectAsParameter>
        /// <subjectAsParameter name="commentEntry">The comment to log.</subjectAsParameter>
        /// <subjectAsParameter name="level">The level of the log.</subjectAsParameter>
        public static void AddToQueue(string loggingObjectName, Exception exceptionEntry, string commentEntry, int level = 1)
        {
            if (level <= LoggingLevel)
            {
                AddToQueue(new Log(loggingObjectName, exceptionEntry, commentEntry, level, IncludeStack, IncludeToStringOutput));
            }
        }

        /// <summary>
        /// Compiles the log queue to a single human readable string.
        /// </summary>
        /// <returns>The logs as a single human readable string.</returns>
        private static string CompileQueueToString()
        {
            var rawResult = new StringBuilder(logs.Count * 4);
            for (var index = 0; index < logs.Count; index++)
            {
                rawResult.Append(logs[index].ToString());
            }

            return rawResult.ToString();
        }

        /// <summary>
        /// Compiles the log queue into a single <see cref="XElement"/> instance.
        /// </summary>
        /// <returns>The logs as a single <see cref="XElement"/> instance.</returns>
        private static XElement CompileQueueToXElement()
        {
            var fullLog = new XElement
                (
                    NODE_NAME_LOGS,
                    new XAttribute(NODE_NAME_ASSEMBLY_NAME, assembly.GetName().Name),
                    new XAttribute(NODE_NAME_VERSION, version.ProductVersion)
                );

            for (var index = 0; index < logs.Count; index++)
            {
                fullLog.Add(logs[index].ToXml(index));
            }

            return fullLog;
        }

        /// <summary>
        /// Compiles and dumps the log queue into a single string.
        /// </summary>
        /// <remarks>The string is in either the form of an xml file, without the declaration, or human readable text.</remarks>
        /// <subjectAsParameter name="asXml">Whether or not to dump the queue to xml or human readable text.</subjectAsParameter>
        /// <returns>A string that contains the log queue output.</returns>
        public static string DumpQueueToString(bool asXml = true)
        {
            return asXml ? CompileQueueToXElement().ToString() : CompileQueueToString();
        }

        /// <summary>
        /// Compiles all the logs into a single xml file and writes it to a designated location.
        /// </summary>
        /// <subjectAsParameter name="pathAndName">The location to write the file to.</subjectAsParameter>
        private static void SaveAsXml(string pathAndName)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                CompileQueueToXElement());

            doc.Save(pathAndName + ".xml");
        }

        /// <summary>
        /// Compiles all the logs into a single human readable text file and writes it to a designated location.
        /// </summary>
        /// <subjectAsParameter name="pathAndName">The location to write the file to.</subjectAsParameter>
        private static void SaveAsText(string pathAndName)
        {
            using (var writer = new StreamWriter(pathAndName, false, Encoding.UTF8))
            {
                writer.Write("Logs for: {0} version: {1}", assembly.FullName, version.ProductVersion);
                writer.Write(CompileQueueToString());
            }            
        }

        /// <summary>
        /// Tests if a log file can be written to a specific location.
        /// </summary>
        /// <remarks>This is a test method that will help ensure that your program will be able to write a file successfully.</remarks>
        /// <subjectAsParameter name="name">The name of the file. This can contain the filePath as well.</subjectAsParameter>
        /// <subjectAsParameter name="filePath">The directory filePath to the file.</subjectAsParameter>
        /// <subjectAsParameter name="asXml">Whether to write the file as xml or human readable text.</subjectAsParameter>
        /// <returns>True: the file and library, as far as can be reasonably expected, work. False: check folder and file permissions or the library has a bug in it.</returns>
        public static bool TestWriteQueueToFile(string name = null, string path = "", bool asXml = true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = GetDateBasedFileName();
            }

            string fullyQualifiedFileName;
            if (name.Contains("\\"))
            {
                fullyQualifiedFileName = Path.GetFullPath(name);
            }
            else
            {
                fullyQualifiedFileName = path + name;

                try
                {
                    fullyQualifiedFileName = Path.GetFullPath(path + name);
                }
                catch
                {
                    return false;
                }
            }

            if (!Directory.Exists(fullyQualifiedFileName))
            {
                return false;
            }
            else
            {
                return TestFileLocation(fullyQualifiedFileName);
            }
        }

        /// <summary>
        /// Tests if a file can be accessed by this library.
        /// </summary>
        /// <subjectAsParameter name="fullyQualifiedFileName">The fully qualified (not relative) file name to test.</subjectAsParameter>
        /// <returns>True: it worked. False: check a few things.</returns>
        private static bool TestFileLocation(string fullyQualifiedFileName)
        {
            var testFileInfo = new FileInfo(fullyQualifiedFileName);
            FileStream testStream;
            try
            {
                testStream = testFileInfo.Exists ? testFileInfo.Open(FileMode.Open) : testFileInfo.Create();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Compiles all the logs into a single file and writes it to a designated location.
        /// </summary>
        /// <remarks>The method <see cref="TestWriteQueueToFile"/> should be called early in development to ensure that your program will be able to utilize this library.</remarks>
        /// <subjectAsParameter name="name">The name of the file to be used. This file will be placed in the same location as the executable that the <see cref="Logger"/> is created within.</subjectAsParameter>
        /// <subjectAsParameter name="asXml">Whether to write the logs as an xml file.</subjectAsParameter>
        public static void WriteQueueToFile(string name = null, bool asXml = true)
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Lo‌cation);
            if (name.IsNull())
            {
                name = GetDateBasedFileName();
            }

            WriteQueueToFile(path, name, asXml);
        }

        /// <summary>
        /// Gets a simple filename based upon the current date.
        /// </summary>
        /// <returns>The date in the following format: Month-Day-Year</returns>
        private static string GetDateBasedFileName()
        {
            return DateTime.Now.ToShortDateString().Replace('/', '-');
        }

        /// <summary>
        /// Compiles all the logs into a single file and writes it to a designated location.
        /// </summary>
        /// <remarks>The method <see cref="TestWriteQueueToFile"/> should be called early in development to ensure that your program will be able to utilize this library.</remarks>
        /// <subjectAsParameter name="filePath">The directory filePath to the log file.</subjectAsParameter>
        /// <subjectAsParameter name="name">The name of the log file.</subjectAsParameter>
        /// <subjectAsParameter name="asXml">Whether to write the file as xml or human readable text.</subjectAsParameter>
        public static void WriteQueueToFile(string path, string name, bool asXml = true)
        {
            if (asXml)
            {
                SaveAsXml(path + "\\" + name);
            }
            else
            {
                SaveAsText(path + "\\" + name);
            }
        }
    }
}
