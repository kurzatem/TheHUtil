namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ArrayExt"/> static class.
    /// </summary>
    public static class ArrayExt
    {
        /// <summary>
        /// Gets the result value of a given collection.
        /// </summary>
        /// <typeparam name="T">The collection's data type to use.</typeparam>
        /// <subjectAsParameter name="collection">The collection to get the result value of.</subjectAsParameter>
        /// <returns>The result value of a collection.</returns>
        public static T LastValue<T>(this IList<T> collection)
        {
            return collection[collection.Count - 1];
        }

        /// <summary>
        /// Writes all entries of a collection to a single string.
        /// </summary>
        /// <typeparam name="T">The collection's data type to use. It is recommended that the parameterless "ToString" method be overridden for the object used.</typeparam>
        /// <subjectAsParameter name="collection">The collection to write the contents of.</subjectAsParameter>
        /// <subjectAsParameter name="separator">The character to use to separate the output of the items in the collection.</subjectAsParameter>
        /// <returns>All items, converted into strings, in the collection.</returns>
        public static string PrintContentsToString<T>(this IList<T> collection, char separator)
        {
            return collection.PrintContentsToString(separator.ToString());
        }

        /// <summary>
        /// Writes all entries of a collection to a single string.
        /// </summary>
        /// <typeparam name="T">The collection's data type to use. It is recommended that the parameterless "ToString" method be overridden for the object used.</typeparam>
        /// <subjectAsParameter name="collection">The collection to write the contents of.</subjectAsParameter>
        /// <subjectAsParameter name="separator">The string to use to separate the output of the items in the collection.</subjectAsParameter>
        /// <returns>All items, converted into strings, in the collection.</returns>
        public static string PrintContentsToString<T>(this IList<T> collection, string separator)
        {
            if (collection.Count > 0)
            {
                StringBuilder result = new StringBuilder(collection.Count * collection[0].ToString().Length);
                foreach (var entry in collection)
                {
                    result.Append(entry.ToString());
                    if (collection.IndexOf(entry) != collection.Count - 1)
                    {
                        result.Append(separator);
                    }
                }

                return result.ToString();
            }
            else
            {
                return "Array was empty.";
            }
        }
    }
}