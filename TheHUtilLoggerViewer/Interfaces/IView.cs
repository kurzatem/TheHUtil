namespace TheHUtilLoggerViewer.Interfaces
{
    public interface IView : ILogData
    {
        string ClassFileView { set; }

        int LineToHighlight { set; }

        string SelectedLogListing { get; }
    }
}
