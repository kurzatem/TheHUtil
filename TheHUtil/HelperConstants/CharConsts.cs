namespace TheHUtil.HelperConstants
{
    using System.Collections.Generic;
    using System.Linq;

    using TheHUtil.Extensions;

    /// <summary>
    /// Defines the <see cref="CharConsts"/> static class.
    /// </summary>
    public static class CharConsts
    {
        /// <summary>
        /// Defines all possible characters in the UTF-16 encoding.
        /// </summary>
        public static char[] AllPossibilities
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines every letters in the UTF-16 encoding.
        /// </summary>
        public static char[] Letters
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines every number in the UTF-16 encoding.
        /// </summary>
        public static char[] Numbers
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines everything that is a number and the comma, period and minus sign in the UTF-16 encoding.
        /// </summary>
        public static char[] NumbersAndNotations
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines every punctuation mark in the UTF-16 encoding.
        /// </summary>
        public static char[] Punctuation
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines every symbol in the UTF-16 encoding.
        /// </summary>
        public static char[] Symbols
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the static instance of the <see cref="CharConsts"/> class.
        /// </summary>
        static CharConsts()
        {
            // NOTE: the list sizes are optimised for memory usage as well as speed. They were taken from an older design's array lengths.
            var all = new List<char>(65536); //  All
            var letters = new List<char>(47572); //  Letters
            var numbers = new List<char>(630); //  Numbers
            var numbersPlus = new List<char>(633); //  Numbers and notations
            var punctuation = new List<char>(20); //  Punctuation
            var symbols = new List<char>(3490); //  Symbols
            
            foreach (var rawChar in Enumerable.Range(char.MinValue, char.MaxValue + 1))
            {
                var ch = (char)rawChar;
                // all
                all.Add(ch);
                if (char.IsLetter(ch))
                {
                    // letters
                    letters.Add(ch);
                }
                else if (char.IsNumber(ch))
                {
                    // numbers
                    numbers.Add(ch);
                    // numbers and notations
                    numbersPlus.Add(ch);
                }
                else if (char.IsSeparator(ch))
                {
                    // symbols
                    symbols.Add(ch);

                }
                else if (char.IsPunctuation(ch))
                {
                    // punctuation
                    punctuation.Add(ch);
                }
            }

            numbersPlus.Add(',');
            numbersPlus.Add('.');
            numbersPlus.Add('-');

            AllPossibilities = all.ToArray();
            Letters = letters.ToArray();
            Numbers = numbers.ToArray();
            NumbersAndNotations = numbersPlus.ToArray();
            Punctuation = punctuation.ToArray();
            Symbols = symbols.ToArray();
        }
    }
}
