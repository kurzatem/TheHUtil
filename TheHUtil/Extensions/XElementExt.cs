namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using TheHUtil.Extensions;
    using TheHUtil.HelperConstants;

    public static class XElementExt
    {
        private static string GetJustNumbers(this XElement element)
        {
            return element.TrimChildrenValues().RemoveAll(CharConsts.NotNumbers);
        }

        public static bool AllAttributesAreEqual(this XElement subject, XElement other)
        {
            if (subject.Name == other.Name && subject.HasAttributes && other.HasAttributes)
            {
                foreach (var attr in subject.Attributes())
                {
                    if (attr.Value != other.Attribute(attr.Name).Value)
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

        public static bool ChildrenHaveContents(this XElement element)
        {
            if (element.HasElements)
            {
                foreach (var child in element.Elements())
                {
                    if (!child.IsEmpty)
                    {
                        return true;
                    }
                }
            }
            else if (element.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    if (!object.ReferenceEquals(attr.Value, null) || attr.Value == string.Empty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

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

        public static IEnumerable<T> ValueToCollection<T>(this XElement element, Func<string, T> parser, string[] separators, bool trimChildren = true)
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
