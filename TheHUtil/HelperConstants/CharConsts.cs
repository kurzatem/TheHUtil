namespace TheHUtil.HelperConstants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CharConsts
    {
        public static char[] AllPossibilities
        {
            get
            {
                return Enumerable.Range(char.MinValue, char.MaxValue + 1).Select(c => (char)c).ToArray();
            }
        }

        public static char[] Numbers
        {
            get
            {
                return AllPossibilities.Where(c => char.IsNumber(c)).ToArray();
            }
        }

        public static char[] NotNumbers
        {
            get
            {
                return AllPossibilities.Where(c => !char.IsNumber(c)).ToArray();
            }
        }

        public static char[] Letters
        {
            get
            {
                return AllPossibilities.Where(c => char.IsLetter(c)).ToArray();
            }
        }

        public static char[] NotLetters
        {
            get
            {
                return AllPossibilities.Where(c => !char.IsLetter(c)).ToArray();
            }
        }

        public static char[] Symbols
        {
            get
            {
                return AllPossibilities.Where(c => char.IsSymbol(c)).ToArray();
            }
        }

        public static char[] NotSymbols
        {
            get
            {
                return AllPossibilities.Where(c => !char.IsSymbol(c)).ToArray();
            }
        }

        public static char[] NumericNotations
        {
            get
            {
                return AllPossibilities.Where(c => char.IsNumber(c) | c == ',' | c == '.').ToArray();
            }
        }

        public static char[] NotNumericNotation
        {
            get
            {
                return AllPossibilities.Where(c => !char.IsNumber(c) | c != ',' | c != '.').ToArray();
            }
        }

        public static char[] Punctuation
        {
            get
            {
                return AllPossibilities.Where(c => char.IsSeparator(c)).ToArray();
            }
        }

        public static char[] NotPunctuation
        {
            get
            {
                return AllPossibilities.Where(c => !char.IsPunctuation(c)).ToArray();
            }
        }
    }
}
