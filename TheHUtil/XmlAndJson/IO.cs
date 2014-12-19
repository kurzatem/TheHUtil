namespace TheHUtil.XmlAndJson
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    using Newtonsoft.Json;

    using TheHUtil.IOHelper;

    /// <summary>
    /// Defines the <see cref="IO"/> static class.
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Loads an xml or json file and packs it into an <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        /// <returns>An instance of <see cref="XElement"/> containing the file data.</returns>
        public static XElement Load(string fileName)
        {
            return IO.Parse(IOHelper.ReadFile(fileName, null));
        }

        /// <summary>
        /// Parses either xml or json into an <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <returns>An <see cref="XElement"/> instance that the input string has been parsed from.</returns>
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

        /// <summary>
        /// Writes an <see cref="XElement"/> instance to a file as either xml or json.
        /// </summary>
        /// <param name="input">The <see cref="XElement"/> data to write.</param>
        /// <param name="fileName">The name of the file to write.</param>
        /// <param name="asXml">Whether to write the file as xml or json.</param>
        public static void Save(this XElement input, string fileName, bool asXml = true)
        {
            var fileStream = IOHelper.GetFileStream(fileName, FileMode.Open);
            using (fileStream)
            {
                Save(input, fileStream, asXml);
            }
        }

        /// <summary>
        /// Writes an <see cref="XElement"/> instance to a file as either xml or json.
        /// </summary>
        /// <param name="input">The <see cref="XElement"/> data to write.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="asXml">Whether to write the file as xml or json.</param>
        public static void Save(this XElement input, Stream stream, bool asXml = true)
        {
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
            {
                writer.Write(input.WriteToString(asXml));
            }
        }

        /// <summary>
        /// Writes as <see cref="XElement"/> instance to a string as either xml or json.
        /// </summary>
        /// <param name="input">The <see cref="XElement"/> data to write.</param>
        /// <param name="asXml">Whether to output the data as xml or json.</param>
        /// <returns>A string that contains the given <see cref="XElement"/> data.</returns>
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
