namespace TheHUtilTests.Logging
{
    using System;
    using System.Xml.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TheHUtil.Logging;
    using TheHUtilLoggerViewer;

    [TestClass]
    public class LoadableLogTests
    {
        private const string TEST_FILE_PATH_AND_NAME = @"C:\Users\Bryan\Documents\Visual Studio 2012\Projects\TheHUtil\TheHUtilTests\bin\DebugTestFile.xml";
        
        public LoadableLogTests()
        {
            Logger.LoggingLevel = 2;
            Logger.IncludeStack = true;
            Logger.IncludeToStringOutput = true;
            Logger.AddToQueue("LoadableLogTests", "This is for testing purposes only.", 1);
        }
    }
}
