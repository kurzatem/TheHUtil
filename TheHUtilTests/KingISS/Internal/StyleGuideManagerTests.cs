using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheHUtilTests.KingISS.Internal
{
    [TestClass]
    public class StyleGuideManagerTests
    {
        [TestMethod]
        public void shouldDecodeHeader()
        {
            // What is needed for reading a KingISS header:
            /*
             * A style guide that holds the default keywords.
             */

            // mock a header

            // give it to the mgr to decode

            // compare the results
        }

        [TestMethod]
        public void shouldEncodeHeader()
        {
            // What is needed for writing a KingISS header:
            /*
             * The style guide data should have all the information needed to write the header.
             * Remember that a typical notation would be: The delimiters for a string are " and "
             * Note: only type names can be inserted other than the default keywords.
             * An exception would be when the header writer is working, then the program could generate headers using the expanded keywords.
             */
            
            // mock a header

            // insert all the pieces in the mgr

            // get an output and compare
        }
    }
}
