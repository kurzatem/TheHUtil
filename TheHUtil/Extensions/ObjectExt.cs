namespace TheHUtil.Extensions
{
    using System;

    /// <summary>
    /// Defines the <see cref="ObjectExt"/> static class.
    /// </summary>
    /// <remarks>Contains extension methods for designed solely to make your code easier to read.</remarks>
    public static partial class ObjectExt
    {
        /// <summary>
        /// Determines if two references are referring to the exact same instance.
        /// </summary>
        /// <remarks>This simply wraps <seealso cref="object.ReferenceEquals"/> into an extension method.</remarks>
        /// <param name="objA">The first reference to check.</param>
        /// <param name="objB">The second reference to check.</param>
        /// <returns>True: the references refer to the same instance. False: they are separate instances.</returns>
        public new static bool ReferenceEquals(this object objA, object objB)
        {
            return object.ReferenceEquals(objA, objB);
        }

        /// <summary>
        /// Determines if a reference is null or not.
        /// </summary>
        /// <remarks>This simply calls <seealso cref="object.ReferenceEquals"/> with null being the second argument. This is to make the code easier to read.</remarks>
        /// <param name="objA">The reference to check.</param>
        /// <returns>True: the reference is null. False: it is not null.</returns>
        public static bool IsNull(this object objA)
        {
            return object.ReferenceEquals(objA, null);
        }

        public static bool CanConvertTo(this object input, Type outputType)
        {
            if (!(input.IsNull() || outputType.IsNull()))
            {
                var converted = input as IConvertible;
                return !converted.IsNull();
            }
            else
            {
                return false;
            }
        }
    }
}
