namespace TheHUtil.Logging.Snapshot
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using TheHUtil.Logging.Boilerplate;

    /// <summary>
    /// Defines the generic <see cref="Log"/> class.
    /// </summary>
    /// <typeparam name="T">The type of object given to be logged.</typeparam>
    public class Log<T> : Log
    {
        /// <summary>
        /// The object that has been shallowly copied using <see cref="ObjectExt.Clone"/>.
        /// </summary>
        protected readonly T snapshotObject;

        /// <summary>
        /// Initializes a new instance of the generic <see cref="Log"/> class.
        /// </summary>
        /// <param name="snapshotObject">The object to log data from.</param>
        /// <param name="exceptionEntry">The exception to be logged.</param>
        /// <param name="level">The log's level in terms of importance.</param>
        /// <param name="includeStack">Whether to include the stack in the output.</param>
        public Log(T snapshotObject, Exception exceptionEntry, int level = 1, bool includeStack = true) :
            base("null", exceptionEntry, level)
        {
            this.snapshotObject = (T)snapshotObject.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the generic <see cref="Log"/> class.
        /// </summary>
        /// <param name="snapshotObject">The object to log data from.</param>
        /// <param name="commentEntry">The exception to be logged.</param>
        /// <param name="level">The log's level in terms of importance.</param>
        public Log(T snapshotObject, string commentEntry, int level) :
            base("null", commentEntry, level)
        {
            this.snapshotObject = (T)snapshotObject.Clone();
        }

        /// <summary>
        /// Gets the log's data and returns it in xml format.
        /// </summary>
        /// <param name="number">The log's number as given by the <see cref="Logger"/>.</param>
        /// <returns>The log's data in xml format.</returns>
        public override XElement ToXml(int number = -1)
        {
            var result = base.ToXml(number);

            result.Add(new XAttribute("SnapshotOfData", this.snapshotObject.ToString()));

            return result;
        }
    }
}
