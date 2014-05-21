namespace TheHUtil.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="Log"/> class.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Holds the type information of <see cref="Log"/>. Will contain data after the first call to <see cref="TypeOfLog"/>.
        /// </summary>
        private static Type typeofLog;

        /// <summary>
        /// Holds the type information of <see cref="Logger"/>. Will contain data after the first call to <see cref="TypeOfLogger"/>.
        /// </summary>
        private static Type typeofLogger;

        /// <summary>
        /// Gets the type of <see cref="Log"/>. Assists in getting the proper object is retrieved for having called for a log.
        /// </summary>
        protected static Type TypeOfLog
        {
            get
            {
                if (typeofLog.IsNull())
                {
                    typeofLog = typeof(Log);
                }

                return typeofLog;
            }
        }

        /// <summary>
        /// Gets the type of <see cref="Logger"/>. Assists in getting the proper object is retrieved for having called for a log.
        /// </summary>
        protected static Type TypeOfLogger
        {
            get
            {
                if (typeofLogger.IsNull())
                {
                    typeofLogger = typeof(Logger);
                }

                return typeofLogger;
            }
        }

        /// <summary>
        /// The name of the object writing the log.
        /// </summary>
        protected readonly string loggingObjectName;

        /// <summary>
        /// The <see cref="Exception"/> to be logged.
        /// </summary>
        protected readonly Exception exceptionEntry;

        /// <summary>
        /// The string to be logged.
        /// </summary>
        protected readonly string stringEntry;

        /// <summary>
        /// The stack to be logged.
        /// </summary>
        protected readonly StackTrace stack;

        /// <summary>
        /// The logging level of this log.
        /// </summary>
        /// <remarks>This is the log's relative importance value. The recommended scale is that <see cref="Exception"/>s are to occupy lower numeric value levels and comments occupy higher numeric values. This allows for flexibility before runtime to log only what is desired so that your program is only minimally impacted.</remarks>
        protected readonly int level;

        /// <summary>
        /// Whether to include the stack in the log or not.
        /// </summary>
        protected readonly bool includeStack;

        /// <summary>
        /// Whether or not to include the output from an <see cref="Exception"/>'s "ToString" method.
        /// </summary>
        protected readonly bool includeToStringOutput;
        
        /// <summary>
        /// Defines the xml node name used for the container of the log.
        /// </summary>
        public const string NODE_NAME_LOG = "Log";

        /// <summary>
        /// Defines the xml node name used for the object that called the logging.
        /// </summary>
        public const string ATTRIBUTE_NAME_FOR = "For";

        /// <summary>
        /// Defines the xml node name, or text file tag, used for any comments in the log.
        /// </summary>
        public const string NODE_NAME_COMMENT = "Comment";

        /// <summary>
        /// Defines the xml attribute name used for any details of the comment in the log.
        /// </summary>
        public const string ATTRIBUTE_NAME_COMMENT = "Details";

        /// <summary>
        /// Defines the xml node name, or text file tag, used for the exceptionEntry in the log, if needed.
        /// </summary>
        public const string NODE_NAME_EXCEPTION = "Exception";

        /// <summary>
        /// Defines the xml node name used for the exceptionEntry type in the log, if needed.
        /// </summary>
        public const string ATTRIBUTE_NAME_EXCEPTION_TYPE = "Type";

        /// <summary>
        /// Defines the xml node name used for the log's number.
        /// </summary>
        public const string ATTRIBUTE_NAME_NUMBER = "Number";

        /// <summary>
        /// Defines the xml node name used for the method that the log is for.
        /// </summary>
        public const string ATTRIBUTE_NAME_METHOD = "Method";

        /// <summary>
        /// Defines the xml node name used for the message of the exceptionEntry that the log is for.
        /// </summary>
        public const string ATTRIBUTE_NAME_MESSAGE = "Message";

        /// <summary>
        /// Defines the xml node name used for the inner exceptionEntry of the log, if needed.
        /// </summary>
        public const string NODE_NAME_INNER = "Inner";

        /// <summary>
        /// Defines the xml node name used for the stack tracing of the log, if needed.
        /// </summary>
        public const string ATTRIBUTE_NAME_STACK = "Stack";

        /// <summary>
        /// Defines the xml node name used for the out of the "ToString" method defined in the exceptionEntry.
        /// </summary>
        public const string ATTRIBUTE_NAME_TO_STRING_OUTPUT = "ToStringOutput";
        
        /// <summary>
        /// Defines the tag used within the non-xml version of the log file for the end of a comment log.
        /// </summary>
        public const string TEXT_END_OF_COMMENT = "End of comment.";

        /// <summary>
        /// Defines the tag used within the non-xml version of the log file for the end of an exceptionEntry log.
        /// </summary>
        public const string TEXT_END_OF_EXCEPTION = "End of exception entry.";

        /// <summary>
        /// Gets whether the log contains an stringEntry.
        /// </summary>
        public bool HasEntry
        {
            get
            {
                return (this.exceptionEntry != null || this.stringEntry != string.Empty) ? true : false;
            }
        }

        /// <summary>
        /// Gets the logging level for this log.
        /// </summary>
        public int LogLevel
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="loggingObjectName">The name of the object calling for the log. If this is not defined, then there will be a performance hit as the stack will be traced to find out the type of class called this.</param>
        /// <param name="level">The logging level for this log.</param>
        /// <param name="includeStack">Whether to include the stack in the log.</param>
        protected Log(string loggingObjectName, int level, bool includeStack = false)
        {
            if (string.IsNullOrWhiteSpace(loggingObjectName))
            {
                this.loggingObjectName = this.ResolveCallingType();
            }
            else
            {
                this.loggingObjectName = loggingObjectName;
            }

            this.level = level;
            this.includeStack = includeStack;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class. This is for logging an <see cref="Exception"/>.
        /// </summary>
        /// <param name="loggingObjectName">The name of the object calling for the log. If this is not defined, then there will be a performance hit as the provided exception will give up the data.</param>
        /// <param name="exceptionEntry">The <see cref="Exception"/> to be logged.</param>
        /// <param name="level">The logging level for this log.</param>
        /// <param name="includeStack">Whether to include the stack in the log.</param>
        public Log(string loggingObjectName, Exception exceptionEntry, int level, bool includeStack = true, bool includeToStringOutput = true) :
            this(
            loggingObjectName.IsNullOrEmptyOrWhiteSpace() ? exceptionEntry.TargetSite.ReflectedType.Name : loggingObjectName,
            level,
            includeStack)
        {
            this.exceptionEntry = exceptionEntry;
            this.stringEntry = string.Empty;
            this.includeToStringOutput = includeToStringOutput;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class. This is for logging a comment.
        /// </summary>
        /// <param name="loggingObjectName">The name of the object calling for the log. If this is not defined, then there will be a performance hit as the stack will be traced to find out the type of class called this.</param>
        /// <param name="stringEntry">The comment to be logged.</param>
        /// <param name="level">The logging level for this log.</param>
        /// <param name="includeStack">Whether to include the stack in the log.</param>
        public Log(string loggingObjectName, string stringEntry, int level) :
            this(loggingObjectName, level)
        {
            this.stringEntry = stringEntry;
            this.exceptionEntry = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class. This is for logging both an <see cref="Exception"/> and a comment.
        /// </summary>
        /// <param name="loggingObjectName">The name of the object calling for the log. If this is not defined, then there will be a performance hit as the stack will be traced to find out the type of class called this.</param>
        /// <param name="exceptionEntry">The <see cref="Exception"/> to be logged.</param>
        /// <param name="stringEntry">The exceptionEntry to be logged.</param>
        /// <param name="level">The logging level for this log.</param>
        /// <param name="includeStack">Whether to include the stack in the log.</param>
        public Log(string loggingObjectName, Exception exceptionEntry, string stringEntry, int level, bool includeStack = true, bool includeToStringOutput = true) :
            this(loggingObjectName, exceptionEntry, level, includeStack, includeToStringOutput)
        {
            this.stringEntry = stringEntry;
        }

        /// <summary>
        /// Resolves the type of the object that requested the log. If this is called, it will have a negative effect upon the program's performance.
        /// </summary>
        /// <param name="loggingObjectName">The name of the object to be determined. Defaults to an empty string.</param>
        /// <param name="layers">The number of layers or steps the trace back on the call stack.</param>
        /// <returns>The string representing the object type that called for this log to be created. NOTE: this only traces back to the first object that is not of the <see cref="Log"/> type. Also, if the calling type is of the type <see cref="Log"/>, then the next calling type will be found. It should be noted that it *shouldn't* resolve past the Main method of a program.</returns>
        private string ResolveCallingType(int layers = 2)
        {
            var stack = new StackFrame(layers);
            var method = stack.GetMethod();
            var type = method.DeclaringType;
            if ((type == TypeOfLog || type == TypeOfLogger) && method.Name != "Main")
            {
                layers += 2;
                return this.ResolveCallingType(layers);
            }
            else
            {
                return type.Name;
            }
        }

        /// <summary>
        /// Overrides the ToString method to dump the <see cref="Log"/> to a text output.
        /// </summary>
        /// <returns>The contents of the <see cref="Log"/> for writing as a human readable text file.</returns>
        public override string ToString()
        {
            return this.ToString(-1);
        }

        /// <summary>
        /// Dumps the internal data to a non-xml output.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The internal data of the <see cref="Log"/> in non-xml format.</returns>
        public string ToString(int number)
        {
            var result = new StringBuilder();
            result.AppendLine(NODE_NAME_LOG);
            result.Append(ATTRIBUTE_NAME_FOR);
            result.Append(": ");
            result.AppendLine(this.loggingObjectName.GetType().ToString());
            result.Append(ATTRIBUTE_NAME_NUMBER);
            result.Append(": ");
            result.AppendLine(number.ToString());
            result.AppendLine((!object.ReferenceEquals(this.exceptionEntry, null)) ?
                this.FormatExceptionToString() : this.FormatCommentToString());

            return result.ToString();
        }

        /// <summary>
        /// Dumps the internal data of the <see cref="Log"/> in an xml format.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The internal data of the <see cref="Log"/> as xml.</returns>
        public XElement ToXml(int number = -1)
        {
            var result = new XElement(NODE_NAME_LOG,
                    new XAttribute(ATTRIBUTE_NAME_FOR, this.loggingObjectName),
                    new XAttribute(ATTRIBUTE_NAME_NUMBER, number));

            if (!object.ReferenceEquals(this.exceptionEntry, null))
            {
                result.Add(
                    new XElement(this.FormatExceptionToXml()));
            }
            else
            {
                result.Add(
                    new XElement(this.FormatCommentToXml()));
            }

            return result;
        }

        /// <summary>
        /// Formats the comment given in the <see cref="Log"/> to a string.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The comment data of the <see cref="Log"/> as a string.</returns>
        private string FormatCommentToString()
        {
            var result = new StringBuilder();
            result.AppendLine(NODE_NAME_COMMENT);
            result.AppendLine(this.stringEntry);
            result.AppendLine(TEXT_END_OF_COMMENT);
            return result.ToString();
        }

        /// <summary>
        /// Formats the comment given in the <see cref="Log"/> to xml.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The comment data of the <see cref="Log"/> as xml.</returns>
        private XElement FormatCommentToXml()
        {
            return new XElement(
                NODE_NAME_COMMENT,
                new XAttribute(ATTRIBUTE_NAME_COMMENT, this.stringEntry)
                );
        }

        /// <summary>
        /// Formats the <see cref="Exception"/> given in the <see cref="Log"/> as a string.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The <see cref="Exception"/> data of the <see cref="Log"/> as a string.</returns>
        private string FormatExceptionToString()
        {
            var result = FormatExceptionToString(this.exceptionEntry, this.includeStack, this.includeToStringOutput);
            if (this.stringEntry.Length > 0)
            {
                result = result + this.FormatCommentToString();
            }

            return result;
        }

        /// <summary>
        /// Formats the <see cref="Exception"/> given in the <see cref="Log"/> as xml.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <returns>The <see cref="Exception"/> data of the <see cref="Log"/> as a string.</returns>
        private XElement FormatExceptionToXml()
        {
            var result = FormatExceptionToXml(this.exceptionEntry, this.includeStack, this.includeToStringOutput);
            if (this.stringEntry.Length > 0)
            {
                result.Add(this.FormatCommentToXml());
            }
            
            return result;
        }

        /// <summary>
        /// Formats a given <see cref="Exception"/>, optionally it's stack tracing, as a string.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <param name="e">The <see cref="Exception"/> to be converted.</param>
        /// <param name="includeStack">Whether or not to include the stack.</param>
        /// <param name="includeToStringOutput">Whether of not to include the output of the <see cref="Exception"/> "ToString" method.</param>
        /// <returns>The given <see cref="Exception"/> as a string.</returns>
        private static string FormatExceptionToString(Exception e, bool includeStack, bool includeToStringOutput)
        {
            var result = new StringBuilder();
            result.AppendLine(NODE_NAME_EXCEPTION);

            result.Append("Occured in ");
            result.Append(e.TargetSite);
            result.AppendLine(" method");
            if (!string.IsNullOrWhiteSpace(e.Message))
            {
                result.Append(ATTRIBUTE_NAME_MESSAGE);
                result.Append(": ");
                result.AppendLine(e.Message);
            }

            if (!object.ReferenceEquals(e.InnerException, null))
            {
                result.Append("Inner exception: "); 
                result.AppendLine(FormatExceptionToString(e.InnerException, includeStack, includeToStringOutput));
            }

            if (includeStack)
            {
                result.Append("Stack trace: ");
                result.AppendLine(e.StackTrace);
            }

            if (includeToStringOutput)
            {
                result.Append("Output from ToString method: ");
                result.AppendLine(e.ToString());
            }

            result.AppendLine(TEXT_END_OF_EXCEPTION);
            return result.ToString();
        }

        /// <summary>
        /// Formats a given <see cref="Exception"/>, optionally it's stack tracing, as xml.
        /// </summary>
        /// <param name="number">The number of the <see cref="Log"/>.</param>
        /// <param name="e">The <see cref="Exception"/> to be formatted.</param>
        /// <param name="includeStack">Whether or not to include the stack.</param>
        /// <param name="includeToStringOutput">Whether of not to include the output of the <see cref="Exception"/> "ToString" method.</param>
        /// <returns>The given <see cref="Exception"/> as xml.</returns>
        private static XElement FormatExceptionToXml(Exception e, bool includeStack, bool includeToStringOutput)
        {
            var result = new XElement(NODE_NAME_EXCEPTION,
                new XAttribute(ATTRIBUTE_NAME_EXCEPTION_TYPE, e.GetType().Name));

            result.Add(new XAttribute(ATTRIBUTE_NAME_METHOD, e.TargetSite));

            if (!string.IsNullOrWhiteSpace(e.Message))
            {
                result.Add(new XAttribute(ATTRIBUTE_NAME_MESSAGE, e.Message));
            }

            if (!object.ReferenceEquals(e.InnerException, null))
            {
                result.Add(new XElement(NODE_NAME_INNER, FormatExceptionToXml(e.InnerException, includeStack, includeToStringOutput)));
            }

            if (includeStack)
            {
                result.Add(new XAttribute(ATTRIBUTE_NAME_STACK, e.StackTrace));
            }

            if (includeToStringOutput)
            {
                result.Add(new XAttribute(ATTRIBUTE_NAME_TO_STRING_OUTPUT, e.ToString()));
            }

            return result;
        }
    }
}
