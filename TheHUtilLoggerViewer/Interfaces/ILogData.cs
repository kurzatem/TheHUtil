namespace TheHUtilLoggerViewer.Interfaces
{
    public interface ILogData
    {
        string LogForClassName { set; }

        string ExceptionType { set; }

        int LogNumber { set; }

        string ExceptionMethodName { set; }

        string ExceptionMessage { set; }

        string ExceptionStack { set; }

        string ExceptionOutput { set; }

        string LogComment { set; }
    }
}
