namespace TheHUtil.Logging.Snapshot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Log<T> : Log where T: ICloneable
    {
        protected readonly T snapshotObject;

        public Log(T snapshotObject, Exception exceptionEntry, int level = 1, bool includeStack = true) :
            base("null", exceptionEntry, level)
        {
            this.snapshotObject = (T)snapshotObject.Clone();
        }

        public Log(T snapshotObject, string commentEntry, int level) :
            base("null", commentEntry, level)
        {
            this.snapshotObject = (T)snapshotObject.Clone();
        }
    }
}
