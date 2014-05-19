namespace TheHUtilLogViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using TheHUtil.Extensions;
    using TheHUtil.Logging;

    internal class LoadableLog
    {
        private string name;

        private string comment;

        private string exceptionType;

        private string method;

        private string message;

        private LoadableLog inner;

        private string stack;

        private int logNumber;

        private string extraComments;

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.name = value;
            }
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }

            private set
            {
                this.comment = value;
            }
        }

        public string ExceptionType
        {
            get
            {
                return this.exceptionType;
            }

            private set
            {
                this.exceptionType = value;
            }
        }

        public string MethodName
        {
            get
            {
                return this.method;
            }

            private set
            {
                this.method = value;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }

            private set
            {
                this.message = value;
            }
        }

        public LoadableLog Inner
        {
            get
            {
                return this.inner;
            }

            private set
            {
                this.inner = value;
            }
        }

        public string Stack
        {
            get
            {
                return this.stack;
            }

            private set
            {
                this.stack = value;
            }
        }

        public string ExtraComments
        {
            get
            {
                return this.extraComments;
            }

            set
            {
                this.extraComments = value;
            }
        }

        public int LogNumber
        {
            get
            {
                return this.logNumber;
            }

            private set
            {
                this.logNumber = value;
            }
        }

        public LoadableLog(XElement inputData)
        {
            this.Name = inputData.Element(Log.NODE_NAME_FOR).TrimChildrenValues();
            string number = inputData.Element(Log.NODE_NAME_NUMBER).TrimChildrenValues();
            if (!object.ReferenceEquals(number, null) || number != string.Empty)
            {
                this.LogNumber = number.ToInt();
            }

            this.Comment = inputData.Element(Log.NODE_NAME_COMMENT).TrimChildrenValues();
            this.ExceptionType = inputData.Element(Log.NODE_NAME_EXCEPTION_TYPE).TrimChildrenValues();
            this.MethodName = inputData.Element(Log.NODE_NAME_METHOD).TrimChildrenValues();
            this.Message = inputData.Element(Log.NODE_NAME_MESSAGE).TrimChildrenValues();
            this.Stack = inputData.Element(Log.NODE_NAME_STACK).TrimChildrenValues();
            var inner = inputData.Element(Log.NODE_NAME_INNER);
            if (!object.ReferenceEquals(inner, null))
            {
                this.Inner = new LoadableLog(inner);
            }
        }

        public LoadableLog(IEnumerable<string> inputData)
        {
            foreach (var line in inputData)
            {
                
            }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void BindToUI()
        {
            throw new NotImplementedException();
        }
    }
}
