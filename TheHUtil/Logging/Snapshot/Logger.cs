namespace TheHUtil.Logging.Snapshot
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

    /// <summary>
    /// Defines the <see cref="Logger"/> class for taking object "snapshots".
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Creates a new log containing the object's data as well as normal logging data and adds it to the queue.
        /// </summary>
        /// <typeparam name="T">The type of the object to be logged.</typeparam>
        /// <param name="objectToLog">The object to log data from.</param>
        /// <param name="exceptionEntry">The exception to log.</param>
        /// <param name="level">The level of the log in terms of importance.</param>
        /// <param name="includeStack">Whether or not to include the stack when outputting the log data.</param>
        public static void AddToQueue<T>(T objectToLog, Exception exceptionEntry, int level = 1, bool includeStack = true)
        {
            if (level <= Logging.Logger.LoggingLevel)
            {
                Logging.Logger.AddToQueue(new Log<T>(objectToLog, exceptionEntry, level, includeStack));
            }
        }

        /// <summary>
        /// Creates a new log containing the object's data as well as normal logging data and adds it to the queue.
        /// </summary>
        /// <typeparam name="T">The type of the object to be logged.</typeparam>
        /// <param name="objectToLog">The object to log data from.</param>
        /// <param name="commentEntry">A comment to log about the object's data.</param>
        /// <param name="level">The level of the log in terms of importance.</param>
        public static void AddToQueue<T>(T objectToLog, string commentEntry, int level)
        {
            if (level <= Logging.Logger.LoggingLevel)
            {
                Logging.Logger.AddToQueue(new Log<T>(objectToLog, commentEntry, level));
            }
        }
    }
}
