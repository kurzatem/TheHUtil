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

    public static partial class Logger
    {
        public static void AddToQueue<T>(T objectToLog, Exception exceptionEntry, int level = 1, bool IncludeStack = true, bool includeToStringOutput = true)
        {
            // TODO: implement this using Log<T> and create the other methods that work with this.
            throw new NotImplementedException();
        }
    }
}
