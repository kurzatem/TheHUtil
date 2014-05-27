namespace TheHUtil.Logging.Boilerplate
{
    using System;

    /// <summary>
    /// Defines the <see cref="MethodFlags"/> bit flags enum.
    /// </summary>
    [Flags]
    public enum MethodFlags
    {
        /// <summary>
        /// Whether to create a ToString method for an object.
        /// </summary>
        ToString = 1, // 0x0001

        /// <summary>
        /// Whether to create an Equals method for an object.
        /// </summary>
        Equals = 2, // 0x0010

        /// <summary>
        /// Whether to create a GetHashCode method for an object.
        /// </summary>
        GetHasCode = 4, // 0x0100

        /// <summary>
        /// Whether to create a Clone method for an object.
        /// </summary>
        Clone = 8 // 0x1000
    }
}
