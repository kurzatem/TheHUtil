namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TheHUtil.HelperConstants;
    
    public static class StringExt
    {
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
        /// Removes the last occurance of a given string from another string.
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
        /// Removes the last occurance of a character from a given string.
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
        /// Converts the given string to a <see cref="Byte"/>.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="clean">Whether to remove the non-numeric characters or not.</param>
        /// <returns>The given string as a byte.</returns>
        public static byte ToByte(this string input, bool clean = true)
        {
            return clean ?
                byte.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                byte.Parse(input);
        }

        /// <summary>
        /// Converts a given string to a <see cref="byte"/> wrapped in a <see cref="Nullable<T>"/> instance.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="clean">Whether to remove the non-numeric characters or not.</param>
        /// <returns>The given string as a nullable byte.</returns>
        public static byte? ToNullableByte(this string input, bool clean = true)
        {
            try
            {
                return input.ToByte(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static sbyte ToSByte(this string input, bool clean = true)
        {
            return clean ?
                sbyte.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                sbyte.Parse(input);
        }

        public static sbyte? ToNullableSByte(this string input, bool clean = true)
        {
            try
            {
                return input.ToSByte(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static short ToShort(this string input, bool clean = true)
        {
            return clean ?
                short.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                short.Parse(input);
        }

        public static short? ToNullableShort(this string input, bool clean = true)
        {
            try
            {
                return input.ToShort(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static ushort ToUShort(this string input, bool clean = true)
        {
            return clean ?
                ushort.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                ushort.Parse(input);
        }

        public static ushort? ToNullableUShort(this string input, bool clean = true)
        {
            try
            {
                return input.ToUShort(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static int ToInt(this string input, bool clean = true)
        {
            return clean ?
                int.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                int.Parse(input);
        }

        public static int? ToNullableInt(this string input, bool clean = true)
        {
            try
            {
                return input.ToInt(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static uint ToUInt(this string input, bool clean = true)
        {
            return clean ?
                uint.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                uint.Parse(input);
        }

        public static uint? ToNullableUInt(this string input, bool clean = true)
        {
            try
            {
                return input.ToUInt(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static long ToLong(this string input, bool clean = true)
        {
            return clean ?
                long.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                long.Parse(input);
        }

        public static long? ToNullableLong(this string input, bool clean = true)
        {
            try
            {
                return input.ToLong(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static ulong ToULong(this string input, bool clean = true)
        {
            return clean ?
                ulong.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                ulong.Parse(input);
        }

        public static ulong? ToNullableULong(this string input, bool clean = true)
        {
            try
            {
                return input.ToULong(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static float ToFloat(this string input, bool clean = true)
        {
            return clean ?
                float.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                float.Parse(input);
        }

        public static float? ToNullableFloat(this string input, bool clean = true)
        {
            try
            {
                return input.ToFloat(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static double ToDouble(this string input, bool clean = true)
        {
            return clean ?
                double.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                double.Parse(input);
        }

        public static double? ToNullableDouble(this string input, bool clean = true)
        {
            try
            {
                return input.ToDouble(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static decimal ToDecimal(this string input, bool clean = true)
        {
            return clean ?
                decimal.Parse(input.RemoveAll(CharConsts.NotNumbers)) :
                decimal.Parse(input);
        }

        public static decimal? ToNullableDecimal(this string input, bool clean = true)
        {
            try
            {
                return input.ToDecimal(clean);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
