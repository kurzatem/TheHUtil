namespace TheHUtil.XmlAndJson
{
    using System.Xml;
    using System.Xml.Linq;

    internal static class XmlTypeConverters
    {
        internal static XElement ToXElement(this XmlDocument input)
        {
            return input.DocumentElement.ToXElement();
        }

        internal static XElement ToXElement(this XmlElement input)
        {
            return XElement.Parse(input.OuterXml);
        }

        internal static XDocument ToXDocument(this XmlDocument input)
        {
            using (var xmlStream = new XmlNodeReader(input))
            {
                xmlStream.MoveToContent();
                return XDocument.Load(xmlStream);
            }
        }

        internal static XDocument ToXDocument(this XmlElement input)
        {
            return XDocument.Parse(input.OuterXml);
        }

        internal static XmlElement ToXmlElement(this XElement input)
        {
            return input.ToXmlDocument().DocumentElement; 
        }

        internal static XmlDocument ToXmlDocument(this XDocument input)
        {
            var result = input.Root.ToXmlDocument();

            var inputDeclaration = input.Declaration;
            if (!object.ReferenceEquals(inputDeclaration, null))
            {
                var declaration = result.CreateXmlDeclaration
                    (
                        inputDeclaration.Version,
                        inputDeclaration.Encoding,
                        inputDeclaration.Standalone
                    );

                result.InsertBefore(declaration, result.FirstChild);
            }

            return result;
        }

        internal static XmlDocument ToXmlDocument(this XElement input)
        {
            var result = new XmlDocument();
            using (var xmlStream = input.CreateReader())
            {
                result.Load(xmlStream);
            }

            return result;
        }
    }
}
