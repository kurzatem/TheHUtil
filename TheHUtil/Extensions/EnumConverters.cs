namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumConverters
    {
        public static Dictionary<TEnum, string> ConvertToDictionary<TEnum>() where TEnum : struct, IConvertible
        {
            return new Dictionary<TEnum, string>(ConvertedEnum<TEnum>.Converter);
        }

        private static class ConvertedEnum<TEnum> where TEnum : struct, IConvertible
        {
            internal static Dictionary<TEnum, string> Converter
            {
                get;
                private set;
            }

            static ConvertedEnum()
            {
                if (typeof(TEnum).BaseType != typeof(Enum))
                {
                    throw new ArgumentException("Input parameter must be an enum", "input");
                }

                Converter = Enum.GetNames(typeof(TEnum)).ToDictionary(key => (TEnum)Enum.Parse(typeof(TEnum), key), val => val);
            }
        }
    }
}
