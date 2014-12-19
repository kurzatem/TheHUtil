namespace TheHUtil.KingISS
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    internal class KingISSWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        public KingISSWriter() :
            base()
        {
        }

        public void WriteObject<T>(T obj) where T : ISerializable 
        {
            // Check everything to be written. Should compare against the notations and adjust as needed.
            // Might need to do this in another class that contains all the notations.
        }

        public void WriteAll(IEnumerable<StyleGuideEntry> notations)
        {
        }

        public void WriteContents(string contents)
        {
        }

        public void WriteNotationHeader(IEnumerable<StyleGuideEntry> notations)
        {
            
        }
    }
}
