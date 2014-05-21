namespace TheHUtilLoggerViewer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using TheHUtil.Extensions;
    using TheHUtil.Logging;

    using TheHUtilLoggerViewer.Interfaces;
    
    public class LogData
    {
        private string logForClassName;

        private string exceptionType;

        private int logNumber;

        private string exceptionMethodName;

        private string exceptionMessage;

        private string exceptionStack;

        private string exceptionOutput;

        private string logComment;

        private ILogData view;

        public string ClassFileName
        {
            get
            {
                return Path.GetFileName(this.ClassFileNameAndPath);
            }
        }

        public string ClassFileNameAndPath
        {
            get
            {
                var allIndexes = this.exceptionOutput.AllIndexesOf(':');
                if (allIndexes.Count > 2)
                {
                    var index = this.exceptionOutput.LastIndexOfBefore(' ', allIndexes[allIndexes.Count - 2]);
                    var rawResult = this.exceptionOutput.Remove(0, index + 1);
                    index = rawResult.LastIndexOf(':');
                    rawResult = rawResult.Remove(index);

                    return rawResult;
                }
                else
                {
                    return null;
                }
            }
        }

        public string LogListing
        {
            get
            {
                return "Log #: " + this.logNumber.ToString();
            }
        }

        public int ClassFileLine
        {
            get
            {
                var colonIndex = this.exceptionOutput.LastIndexOf(':');
                var rawResult = this.exceptionOutput.Remove(0, colonIndex);
                return rawResult.ToInt();
            }
        }

        private LogData(ILogData view)
        {
            this.view = view;
        }

        public LogData(XElement input, ILogData view) :
            this(view)
        {
            this.logForClassName = input.Attribute(Log.ATTRIBUTE_NAME_FOR).Value;
            this.logNumber = input.Attribute(Log.ATTRIBUTE_NAME_NUMBER).Value.ToInt();
            Logger.AddToQueue("LogData", "Attempting to enter log information from xml.", 3);
            var exception = input.Element(Log.NODE_NAME_EXCEPTION);
            if (!exception.IsNull())
            {
                this.exceptionType = CheckAndAssign(exception, Log.ATTRIBUTE_NAME_EXCEPTION_TYPE);
                this.exceptionMethodName = CheckAndAssign(exception, Log.ATTRIBUTE_NAME_METHOD);
                this.exceptionMessage = CheckAndAssign(exception, Log.ATTRIBUTE_NAME_MESSAGE);
                this.exceptionStack = CheckAndAssign(exception, Log.ATTRIBUTE_NAME_STACK);
                this.exceptionOutput = CheckAndAssign(exception, Log.ATTRIBUTE_NAME_TO_STRING_OUTPUT);
            }

            var comment = input.Element(Log.NODE_NAME_COMMENT);
            if (!comment.IsNull())
            {
                this.logComment = CheckAndAssign(comment, Log.ATTRIBUTE_NAME_COMMENT);
            }
        }

        private static string CheckAndAssign(XElement input, string attributeName)
        {
            var attribute = input.Attribute(attributeName);
            if (attribute.IsNull())
            {
                Logger.AddToQueue(null, "No value for " + attributeName, 3);
                return string.Empty;
            }
            else
            {
                Logger.AddToQueue(null, attribute.Value + " is the value for " + attributeName, 3);
                return attribute.Value;
            }
        }

        public void BindUiToData()
        {
            this.view.ExceptionMessage = this.exceptionMessage;
            this.view.ExceptionMethodName = this.exceptionMethodName;
            this.view.ExceptionOutput = this.ClassFileName + " at line: " + this.ClassFileLine;
            this.view.ExceptionStack = this.exceptionStack;
            this.view.ExceptionType = this.exceptionType;
            this.view.LogComment = this.logComment;
            this.view.LogForClassName = this.logForClassName;
            this.view.LogNumber = this.logNumber;
        }
    }
}
