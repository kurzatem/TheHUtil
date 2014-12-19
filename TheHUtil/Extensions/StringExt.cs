namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using TheHUtil.HelperConstants;
    
    public static class StringExt
    {
        /// <summary>
        /// Gets all the locations of a specified character in a given string.
        /// </summary>
        /// <param name="input">The string to check for the character.</param>
        /// <param name="character">The character to check for in the string.</param>
        /// <returns>A collection of all the zero-based locations of the character in the string.</returns>
        public static IList<int> AllIndexesOf(this string input, char character)
        {
            var result = new List<int>(input.Length);
            if (!input.IsNull())
            {
                for (int index = 0; index < input.Length; index++)
                {
                    if (input[index] == character)
                    {
                        result.Add(index);
                    }
                }

                return result;
            }

            return new[] { -1 };
        }

        /// <summary>
        /// Determines if the input string contains all the given characters.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="chars">The characters to check for.</param>
        /// <returns>True: contains all the given characters. False: does not contain all the given characters.</returns>
        public static bool ContainsAll(this string input, IList<char> chars)
        {
            foreach (var ch in chars)
            {
                if (!input.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the input string contains any of the given characters.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="chars">The characters to check for.</param>
        /// <returns>True: contains any of the given characters. False: does not contain any of the given characters.</returns>
        public static bool ContainsAny(this string input, IList<char> chars)
        {
            if (!string.IsNullOrEmpty(input))
            {
                foreach (var ch in chars)
                {
                    if (input.Contains(ch))
                    {
                        return true;
                    }
                }
            }
                
            // If input is empty of null OR doesn't contain any of the given characters. Fewer exit points this way.
            return false;
        }

        /// <summary>
        /// Counts all the ocurrances of a given string within another string.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="substring">The string to check for.</param>
        /// <returns>The number of substrings present within the input string.</returns>
        public static int Count(this string input, string substring)
        {
            return input.Intersect(substring).Count();
        }

        /// <summary>
        /// Counts all the occurances of a given character within a string.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="character">The character to check for.</param>
        /// <returns>The number of characters that match the given character in the input string.</returns>
        public static int Count(this string input, char character)
        {
            return input.Count((ch) => ch == character);
        }

        /// <summary>
        /// Counts all the occurances of a given set of characters within a string.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <param name="chars">The set of characters to check for.</param>
        /// <returns>The number of matches between all the given characters and the input string.</returns>
        public static int Count(this string input, IList<char> chars)
        {
            int result = 0;
            foreach (var ch in chars)
            {
                result += input.Count(ch);
            }

            return result;
        }

        /// <summary>
        /// Wraps <see cref="string.IsNullOrWhiteSpace"/> for clearer code.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>True: the string is null, empty or all white space characters.</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Gets the rawResult index of a given character before a specified index.
        /// </summary>
        /// <param name="input">The string to check for the character.</param>
        /// <param name="character">The character to check for in the string.</param>
        /// <param name="checkBeforeThis">The zero-based index to check before.</param>
        /// <returns>The rawResult index of a character that appears before the given index. -1 is returned if the character is not found in the string.</returns>
        public static int LastIndexOfBefore(this string input, char character, int checkBeforeThis)
        {
            var subject = input.AllIndexesOf(character);
            int result = -1;
            if (subject.Count > 0)
            {
                foreach (var index in subject)
                {
                    if (index < checkBeforeThis)
                    {
                        result = index;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Removes all occurances of a given character from a string after a specified index.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The characters to remove.</param>
        /// <param name="startIndex">The zero-based index from which to start removing.</param>
        /// <returns>The given string with all occurances of the given character after the designated starting index.</returns>
        public static string RemoveAfter(this string input, char removeThis, int startIndex)
        {
            StringBuilder output = new StringBuilder(input.Remove(startIndex));
            output.Append(input.RemoveAll(removeThis));
            return output.ToString();
        }

        /// <summary>
        /// Removes all occurances of a given set of characters from a string after a specified index.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThese">The characters to remove.</param>
        /// <param name="startIndex">The zero-based index from which to start removing.</param>
        /// <returns>The given string with all occurances of the given characters after the designated starting index.</returns>
        public static string RemoveAfter(this string input, IList<char> removeThese, int startIndex)
        {
            StringBuilder output = new StringBuilder(input.Remove(startIndex));
            output.Append(input.RemoveAll(removeThese));
            return output.ToString();
        }

        /// <summary>
        /// Removes all occurances of a given string from another string after a specified index.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="stringToRemove">The string to remove.</param>
        /// <param name="startIndex">The zero-based index from which to start removing.</param>
        /// <returns>The given string with all occurances of the given string to remove after the designated starting index.</returns>
        public static string RemoveAfter(this string input, string removeThis, int startIndex)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(removeThis) && input.Contains(removeThis))
            {
                return input.Remove(startIndex, removeThis.Length);
            }

            return input;
        }

        /// <summary>
        /// Removes all occurances of a character from a string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The character to remove.</param>
        /// <returns>The given string with all the given character removed.</returns>
        public static string RemoveAll(this string input, char removeThis)
        {
            if (!string.IsNullOrEmpty(input) || !input.Contains(removeThis))
            {
                List<char> result = new List<char>(input.Length);
                foreach (var ch in input)
                {
                    if (ch != removeThis)
                    {
                        result.Add(ch);
                    }
                }

                return new string(result.ToArray());
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Removes all occurances of a given set of characters from a string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThese">The characters to remove.</param>
        /// <returns>The given string with all of the given characters removed.</returns>
        public static string RemoveAll(this string input, IList<char> removeThese)
        {
            if (!string.IsNullOrEmpty(input))
            {
                List<char> result = new List<char>(input.Length);
                foreach (var ch in input)
                {
                    if (!removeThese.Contains(ch))
                    {
                        result.Add(ch);
                    }
                }

                return new string(result.ToArray());
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Removes all occurances of a given string from anotehr string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The characters to remove.</param>
        /// <returns>The given string with the given string removed.</returns>
        public static string RemoveAll(this string input, string removeThis)
        {
            if (input.IsNull())
            {
                throw new ArgumentNullException("Input string cannot be null.");
            }

            if (!input.Contains(removeThis) || input.Length < removeThis.Length)
            {
                return input;
            }

            var result = new List<char>(input.Length);
            for (var index = 0; index < input.Length; index++)
            {
                if (input[index] == removeThis[0])
                {
                    var temp = input.Substring(index, removeThis.Length);
                    if (temp != removeThis)
                    {
                        result.AddRange(temp);
                    }

                    index += temp.Length - 1;
                }
                else
                {
                    result.Add(input[index]);
                }
            }

            return new string(result.ToArray());
        }

        /// <summary>
        /// Removes a specified number of character occurances from a given string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The character to remove.</param>
        /// <param name="count">The number of times that the character should be removed.</param>
        /// <param name="fromBeginning">Whether to remove from the beginning or end.</param>
        /// <returns>The given string with a specified number of occurances of a given character.</returns>
        public static string RemoveCount(this string input, char removeThis, int count = 1, bool fromBeginning = true)
        {
            if (count < 0)
            {
                throw new ArgumentException("Cannot remove less than zero occurances.");
            }

            if (string.IsNullOrEmpty(input) || count == 0)
            {
                return input;
            }

            var amount = input.Count(removeThis);
            if (count >= input.Length || count >= amount)
            {
                return input.RemoveAll(removeThis);
            }

            for (var iterations = 0; iterations <= amount; iterations++)
            {
                if (fromBeginning)
                {
                    input.RemoveFirst(removeThis);
                }
                else
                {
                    input.RemoveLast(removeThis);
                }
            }

            return input;
        }

        /// <summary>
        /// Removes a specified number of string occurances from a given string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The string to remove.</param>
        /// <param name="count">The number of times to remove the string to be removed.</param>
        /// <param name="fromBeginning">Whether to remove from the beginning or end.</param>
        /// <returns>The given string with a specified number of occurances of a given character.</returns>
        public static string RemoveCount(this string input, string removeThis, int count = 1, bool fromBeginning = true)
        {
            if (count < 0)
            {
                throw new ArgumentException("Cannot remove less than zero occurances.");
            }

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(removeThis) || count == 0 || !input.Contains(removeThis))
            {
                return input;
            }

            var amount = input.Count(removeThis);
            if (count >= input.Length || count >= amount)
            {
                return input.RemoveAll(removeThis);
            }

            for (var iterations = 0; iterations <= amount; iterations++)
            {
                if (fromBeginning)
                {
                    input = input.RemoveFirst(removeThis);
                }
                else
                {
                    input = input.RemoveLast(removeThis);
                }
            }

            return input;
        }

        /// <summary>
        /// Removes the first occurance of a given character from a string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The character to remove.</param>
        /// <returns>The given string with the first given character removed.</returns>
        public static string RemoveFirst(this string input, char removeThis)
        {
            if (!string.IsNullOrEmpty(input) && input.Contains(removeThis))
            {
                var startIndex = input.IndexOf(removeThis);
                return input.Remove(startIndex, 1);
            }

            return input;
        }

        /// <summary>
        /// Removes the first occurance of a given string from another string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The string to remove.</param>
        /// <returns>The given string with the first occurance of another string removed.</returns>
        public static string RemoveFirst(this string input, string removeThis)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(removeThis) && input.Contains(removeThis))
            {
                var startIndex = input.IndexOf(removeThis);
                return input.Remove(startIndex, removeThis.Length);
            }

            return input;
        }

        /// <summary>
        /// Removes the rawResult occurance of a given string from another string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The string to remove.</param>
        /// <returns>The given string with the last occurance of another string removed.</returns>
        public static string RemoveLast(this string input, string removeThis)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(removeThis) && input.Contains(removeThis))
            {
                var startIndex = input.LastIndexOf(removeThis);
                return input.Remove(startIndex, removeThis.Length);
            }

            return input;
        }

        /// <summary>
        /// Removes the rawResult occurance of a character from a given string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="removeThis">The character to remove.</param>
        /// <returns>The given string with the last occurance of a given character.</returns>
        public static string RemoveLast(this string input, char removeThis)
        {
            if (!string.IsNullOrEmpty(input) && input.Contains(removeThis))
            {
                var startIndex = input.LastIndexOf(removeThis);
                return input.Remove(startIndex, 1);
            }

            return input;
        }

        /// <summary>
        /// Attempts to parse a string to any type that has an existing parser.
        /// </summary>
        /// <remarks>This will only attempt to check if there is a public static method named "Parse" that accepts a string. If one is not found, then an exception is thrown.</remarks>
        /// <typeparam name="T">A generic type to parse the input string to.</typeparam>
        /// <param name="input">The string to parse.</param>
        /// <returns>A new object of type "T" that has parsed the given string.</returns>
        public static T ParseTo<T>(this string input)
        {
            return StringParser<T>.Parse(input);
        }

        /// <summary>
        /// Attempts to parse a string to any .NET numeric type. For example an int.
        /// </summary>
        /// <remarks>This is designed with the byte, sbyte, short, ushort, int, uint, long, ulong, float, double, decimal and bool in mind.</remarks>
        /// <typeparam name="T">A generic type to parse the input string to.</typeparam>
        /// <param name="input">The string to parse.</param>
        /// <param name="clean">Whether to clean the input string of everything that is not a number, decimal point or minus sign.</param>
        /// <returns>An instance of the type "T" which has been parsed from the input string.</returns>
        public static T ParseToNumeric<T>(this string input, bool clean = true)
        {
            return clean ?
                input.Preserve(CharConsts.NumbersAndNotations).ParseTo<T>() :
                input.ParseTo<T>();
        }

        /// <summary>
        /// Attempts to parse a string to and .NET numeric type. If there is an exception thrown, it will return null.
        /// </summary>
        /// <typeparam name="T">A generic type to parse the input string to.</typeparam>
        /// <param name="input">The string to parse.</param>
        /// <param name="clean">Whether to clean the input string of everything that is not a number, decimal point or minus sign.</param>
        /// <returns>An instance of the type "T" which has been parsed from the input string. If the parser throws a <seealso cref="FormatException"/>, then the return will be null.</returns>
        public static T? ParseToNullableNumeric<T>(this string input, bool clean = true) where T : struct
        {
            try
            {
                return input.ParseToNumeric<T>(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Preserves, in the original order, a set of characters in a given string.
        /// </summary>
        /// <param name="input">The string to remove from.</param>
        /// <param name="keepThese">The set of characters to preserve.</param>
        /// <returns>The given string with all but the given set of characters removed. This preserves the given string's order.</returns>
        public static string Preserve(this string input, IList<char> keepThese)
        {
            if (!string.IsNullOrEmpty(input))
            {
                List<char> result = new List<char>(input.Length);
                foreach (var ch in input)
                {
                    if (keepThese.Contains(ch))
                    {
                        result.Add(ch);
                    }
                }

                return new string(result.ToArray());
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Defines the <see cref="StringParser"/> generic static class.
        /// </summary>
        /// <typeparam name="T">The generic type for the class.</typeparam>
        private static class StringParser<T>
        {
            /// <summary>
            /// The cached parsing method compiled into an expression tree.
            /// </summary>
            private static Func<string, T> parser;

            /// <summary>
            /// Initializes the static instance of the <see cref="StringParser"/> generic class.
            /// </summary>
            static StringParser()
            {
                var existingParser = typeof(T).GetMethod
                    (
                        "Parse",
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static,
                        null,
                        new[] { typeof(string) }, new[] { new ParameterModifier(1) }
                    );

                if (existingParser.IsNull())
                {
                    throw new InvalidOperationException("Type does not contain a public static method named \"Parse\" that accepts a single string as it's parameter.");
                }
                else
                {
                    var inputParameter = Expression.Parameter(typeof(string), "value");
                    var methodExpression = Expression.Call(existingParser, inputParameter);
                    parser = Expression.Lambda<Func<string, T>>(methodExpression, inputParameter).Compile();
                }
            }

            /// <summary>
            /// Parses an input string into the generic type.
            /// </summary>
            /// <param name="input">The string to parse.</param>
            /// <returns>An instance of "T" that has been generated from the parse method.</returns>
            internal static T Parse(string input)
            {
                return parser(input);
            }
        }
    }
}
