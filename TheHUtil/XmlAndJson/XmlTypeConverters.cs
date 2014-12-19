namespace TheHUtil.XmlAndJson
{
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Defines the <see cref="XmlTypeConverters"/> class.
    /// </summary>
    /// <remarks>This is extremely useful for converting between xml container types, but may not be the fastest method. They rely heavily on outputting the xml to a string and parsing the string.</remarks>
    public static class XmlTypeConverters
    {
        /// <summary>
        /// Converts a <see cref="XmlDocument"/> instance into a <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XmlDocument"/> instance to convert.</param>
        /// <returns>An <see cref="XElement"/> instance containing the data from the input <see cref="XmlDocument"/>.</returns>
        public static XElement ToXElement(this XmlDocument input)
        {
            return input.DocumentElement.ToXElement();
        }

        /// <summary>
        /// Converts a <see cref="XmlElement"/> instance into a <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XmlElement"/> instance to convert.</param>
        /// <returnsAn <see cref="XElement"/> instance containing the data from the input <see cref="XmlElement"/>.</returns>
        public static XElement ToXElement(this XmlElement input)
        {
            return XElement.Parse(input.OuterXml);
        }

        /// <summary>
        /// Converts a <see cref="XmlDocument"/> instance into a <see cref="XDocument"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XmlDocument"/> instance to convert.</param>
        /// <returnsAn <see cref="XDocument"/> instance containing the data from the input <see cref="XmlDocument"/>.</returns>
        public static XDocument ToXDocument(this XmlDocument input)
        {
            using (var xmlStream = new XmlNodeReader(input))
            {
                xmlStream.MoveToContent();
                return XDocument.Load(xmlStream);
            }
        }

        /// <summary>
        /// Converts a <see cref="XmlElement"/> instance into a <see cref="XDocument"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XmlElement"/> instance to convert.</param>
        /// <returnsAn <see cref="XDocument"/> instance containing the data from the input <see cref="XmlElement"/>.</returns>
        public static XDocument ToXDocument(this XmlElement input)
        {
            return XDocument.Parse(input.OuterXml);
        }

        /// <summary>
        /// Converts a <see cref="XElement"/> instance into a <see cref="XmlElement"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XElement"/> instance to convert.</param>
        /// <returnsAn <see cref="XmlElement"/> instance containing the data from the input <see cref="XElement"/>.</returns>
        public static XmlElement ToXmlElement(this XElement input)
        {
            return input.ToXmlDocument().DocumentElement; 
        }

        /// <summary>
        /// Converts a <see cref="XDocument"/> instance into a <see cref="XmlDocument"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XDocument"/> instance to convert.</param>
        /// <returnsAn <see cref="XmlDocument"/> instance containing the data from the input <see cref="XDocument"/>.</returns>
        public static XmlDocument ToXmlDocument(this XDocument input)
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

        /// <summary>
        /// Converts a <see cref="XElement"/> instance into a <see cref="XmlDocument"/> instance.
        /// </summary>
        /// <param name="input">The <see cref="XElement"/> instance to convert.</param>
        /// <returnsAn <see cref="XmlDocument"/> instance containing the data from the input <see cref="XElement"/>.</returns>
        public static XmlDocument ToXmlDocument(this XElement input)
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
