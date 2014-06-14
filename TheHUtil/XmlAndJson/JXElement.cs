namespace TheHUtil.XmlAndJson
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using TheHUtil.IOHelper;

    public class JXElement : XElement
    {
        public JXElement(XName name) :
            base(name)
        {
        }

        public JXElement(XElement other) :
            base(other)
        {
        }

        public JXElement(XName name, object content) :
            base(name, content)
        {
        }

        public JXElement(XName name, params object[] content) :
            base(name, content)
        {
        }

        public static JXElement Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            using (var reader = File.OpenText(fileName))
            {
                return JXElement.Parse(reader.ReadToEnd());
            }
        }

        public static JXElement Parse(string data)
        {
            switch (data[0])
            {
                case '{':
                    return JsonConvert.DeserializeXNode(data, "root").Root as JXElement;
                case '<':
                    return XElement.Parse(data) as JXElement;
                default:
                    throw new FormatException("Input data is not in xml or json format.");
            }
        }

        public void Save(string fileName, bool asXml = true)
        {
            var fileStream = IO.GetFileStream(fileName);
            using (fileStream)
            {
                using (var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    if (asXml)
                    {
                        var xDoc = new XDocument
                            (
                                new XDeclaration("1.0", "utf-8", "no"),
                                this
                            );

                        xDoc.Save(writer);
                    }
                    else
                    {
                        var serializedXml = JsonConvert.SerializeXNode(this, Formatting.Indented);
                        writer.Write(serializedXml);
                    }
                }
            }
        }
    }
}
