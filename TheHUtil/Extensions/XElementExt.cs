namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TheHUtil.HelperConstants;

    /// <summary>
    /// Defines the <see cref="XElementExt"/> static class.
    /// </summary>
    /// <remarks>Contains some handy extension methods for ensuring that the correct data is pulled from <see cref="XElement"/> instances.</remarks>
    public static class XElementExt
    {
        /// <summary>
        /// Gets just the numbers from a string.
        /// </summary>
        /// <remarks>This gets every number from the string. Not the first contiguous set.</remarks>
        /// <param name="element">The <see cref="XElement"/> instance to retrieve the numbers from.</param>
        /// <returns>Returns just the numbers.</returns>
        private static string GetJustNumbers(this XElement element)
        {
            return element.TrimChildrenValues().Preserve(CharConsts.Numbers);
        }

        /// <summary>
        /// Determines if all attributes of the node contain equivalent data.
        /// </summary>
        /// <remarks>This does not check the children. For that, try using <see cref="XElement.DeepEquals"/>. Also, the attributes to be compared are determined by the first instance.</remarks>
        /// <param name="element">The first <see cref="XElement"/> instance to compare.</param>
        /// <param name="other">The second <see cref="XElement"/> instance to compare.</param>
        /// <returns>True: the instances' attributes contain equivalent data. False: the attributes are not equivalent.</returns>
        public static bool AllAttributesAreEqual(this XElement element, XElement other)
        {
            if (element.Name == other.Name && element.HasAttributes && other.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    try
                    {
                        if (attr.Value != other.Attribute(attr.Name).Value)
                        {
                            return false;
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the <see cref="XElement"/> instance contains any data within it's child nodes.
        /// </summary>
        /// <remarks>This checks for attributes as well as data within the node itself. Note that a node with a closing tag is considered to contain data, an empty string to be precise.</remarks>
        /// <param name="element">The <see cref="XElement"/> instance to check.</param>
        /// <returns>True: the instance contains some data in either an attribute or the node itself. False: contains no data.</returns>
        public static bool ChildrenHaveContents(this XElement element)
        {
            if (element.HasElements)
            {
                foreach (var child in element.Elements())
                {
                    if (child.IsEmpty && !child.HasAttributes)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Trims the child values from an <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/> instance to trim.</param>
        /// <returns>The value of the top level node.</returns>
        public static string TrimChildrenValues(this XElement element)
        {
            string result = element.Value;
            if (element.IsEmpty)
            {
                throw new NullReferenceException("XML element does not contain any data.");
            }
            else if (element.ChildrenHaveContents())
            {
                foreach (var child in element.Elements())
                {
                    if (!child.IsEmpty)
                    {
                        string childValue = child.Value;
                        int end = result.LastIndexOf(childValue, StringComparison.InvariantCulture) + childValue.Length;
                        int begin = result.IndexOf(childValue, StringComparison.InvariantCulture);
                        result = result.Remove(begin, end - begin);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses an xml node value into another object type
        /// </summary>
        /// <remarks>If no parser is designated, one will be attempted to be found using reflection. It will search for a static method named "Parse".</remarks>
        /// <typeparam name="T">The type of object to parse to.</typeparam>
        /// <param name="element">The <see cref="XElement"/> instance to parse the value from.</param>
        /// <param name="parser">The parser to use.</param>
        /// <param name="trimChildren">Whether to trim the child values from the node. This defaults to true.</param>
        /// <returns>The value of an xml node as an instance of an object.</returns>
        public static T ParseValue<T>(this XElement element, Func<string, T> parser = null, bool trimChildren = true)
        {
            if (parser.IsNull())
            {
                return trimChildren ? element.TrimChildrenValues().ParseTo<T>() : element.Value.ParseTo<T>();
            }

            if (trimChildren)
            {
                return parser.Invoke(element.TrimChildrenValues());
            }
            else
            {
                return parser.Invoke(element.Value);
            }
        }

        /// <summary>
        /// Parses the values of an <see cref="XElement"/> into a collection of that object type.
        /// </summary>
        /// <typeparam name="T">The type of object to parse to.</typeparam>
        /// <param name="element">The <see cref="XElement"/> instance to pull the data from.</param>
        /// <param name="parser">The parser to use.</param>
        /// <param name="separators">A collection of strings that are used within the xml to separate the collection of data to parse.</param>
        /// <param name="trimChildren">Whether to trim out the child nodes or not. This defaults to true.</param>
        /// <returns>A collection with the values parsed.</returns>
        public static IEnumerable<T> ParseValueToCollection<T>(this XElement element, Func<string, T> parser, string[] separators, bool trimChildren = true)
        {
            string input;
            if (trimChildren)
            {
                input = element.TrimChildrenValues();
            }
            else
            {
                input = element.Value;
            }

            var rawResult = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return rawResult.Select(parser);
        }
    }
}
