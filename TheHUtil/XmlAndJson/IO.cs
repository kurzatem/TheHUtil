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

    public static class IO
    {
        public static XElement Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(fileName);
            }

            using (var reader = File.OpenText(fileName))
            {
                return IO.Parse(reader.ReadToEnd());
            }
        }

        public static XElement Parse(string data)
        {
            switch (data[0])
            {
                case '{':
                    return JsonConvert.DeserializeXNode(data, "root").Root;
                case '<':
                    return XElement.Parse(data);
                default:
                    throw new FormatException("Input data is not in xml or json format.");
            }
        }

        public static void Save(this XElement input, string fileName, bool asXml = true)
        {
            var fileStream = IOHelper.GetFileStream(fileName);
            using (fileStream)
            {
                using (var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    writer.Write(input.WriteToString(asXml));
                }
            }
        }

        public static string WriteToString(this XElement input, bool asXml)
        {
            if (asXml)
            {
                var xmlResult = new XDocument
                    (
                        new XDeclaration("1.0", "utf-8", "no"),
                        input
                    );

                return xmlResult.ToString();
            }
            else
            {
                var jsonResult = JsonConvert.SerializeXNode(input, Formatting.Indented);
                return jsonResult;
            }
        }
    }
}
