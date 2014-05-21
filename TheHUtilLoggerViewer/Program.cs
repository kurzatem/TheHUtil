namespace TheHUtilLoggerViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using TheHUtil.Logging;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.IncludeStack = true;
            Logger.IncludeToStringOutput = true;
            Logger.LoggingLevel = 2;
            
            try
            {
                Logger.TestWriteQueueToFile();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new WinformViewer());
            }
            finally
            {
                Logger.WriteQueueToFile();
            }
        }
    }
}
