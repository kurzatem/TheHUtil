namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="CollectionsExt"/> static class.
    /// </summary>
    /// <remarks>Contains some handy extension methods.</remarks>
    public static class CollectionsExt
    {
        private const string empty_collection_notification = "Collection is empty.";

        /// <summary>
        /// Gets the last value of a given collection.
        /// </summary>
        /// <typeparam name="T">The collection's data type to use.</typeparam>
        /// <param name="collection">The collection to get the last value of.</param>
        /// <returns>The last value of a collection.</returns>
        public static T LastValue<T>(this IList<T> collection)
        {
            return collection[collection.Count - 1];
        }

        /// <summary>
        /// Writes all entries of a collection to a single string.
        /// </summary>
        /// <remarks>It is recommended that the "ToString" method be overridden for the object used. Otherwise you can end up with a bunch of type names.</remarks>
        /// <typeparam name="T">The object type stored in the collection.</typeparam>
        /// <param name="collection">The collection to write the contents of.</param>
        /// <param name="separator">The string to use to separate the output of the items in the collection.</param>
        /// <returns>All items, converted into strings, in the collection.</returns>
        public static string PrintContentsToString<T>(this IEnumerable<T> collection, char separator)
        {
            return collection.PrintContentsToString(separator.ToString());
        }

        /// <summary>
        /// Writes all entries of a collection to a single string.
        /// </summary>
        /// <remarks>It is recommended that the "ToString" method be overridden for the object used. Otherwise you can end up with a bunch of type names.</remarks>
        /// <typeparam name="T">The object type stored in the collection.</typeparam>
        /// <param name="collection">The collection to write the contents of.</param>
        /// <param name="separator">The string to use to separate the output of the items in the collection.</param>
        /// <returns>All items, converted into strings, in the collection.</returns>
        public static string PrintContentsToString<T>(this IEnumerable<T> collection, string separator)
        {
            if (collection is IList<T>)
            {
                var list = collection as IList<T>;
                var result = new StringBuilder(list.Count * list[0].ToString().Length);
                for (var index = 0; index < list.Count; index++)
                {
                    result.Append(list[index].ToString());
                    if (index != list.Count - 1)
                    {
                        result.Append(separator);
                    }
                }

                return result.ToString();
            }
            else
            {
                var count = 0;
                var rawResult = new StringBuilder();
                foreach (var entry in collection)
                {
                    rawResult.Append(entry.ToString() + separator);
                }

                if (count == 0)
                {
                    return empty_collection_notification;
                }
                else
                {
                    var indexOfLastSeparator = rawResult.Length - separator.Length - 1;
                    rawResult.Remove(indexOfLastSeparator, separator.Length);
                    return rawResult.ToString();
                }
            }
        }
    }
}