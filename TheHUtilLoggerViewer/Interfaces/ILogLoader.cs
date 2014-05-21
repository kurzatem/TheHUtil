namespace TheHUtilLoggerViewer.Interfaces
{
    using System.ComponentModel;

    public interface ILogLoader
    {
        string ProgramName { get; }

        string ProgramVersion { get; }

        string[] LogCollectionListing { get; }

        void OpenFile(string nameAndPath, BackgroundWorker worker);

        void ShowLog();
    }
}
