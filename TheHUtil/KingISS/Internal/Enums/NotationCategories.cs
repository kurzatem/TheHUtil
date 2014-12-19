namespace TheHUtil.KingISS
{
    /// <summary>
    /// Defines the <see cref="NotationCategories"/> enum. This is the core classification for a symbol or keyword.
    /// </summary>
    internal enum NotationCategories
    {
        /// <summary>
        /// The unspecified category. Use this as a last resort.
        /// </summary>
        Notation, // Anything

        /// <summary>
        /// This is either an assigner or a value separator in a collection.
        /// </summary>
        Separator, // Commas, "and", and assigners

        /// <summary>
        /// This is used to bind a value or collection with a name.
        /// </summary>
        Assigner,

        /// <summary>
        /// This defines either the beginning or end of a string, or object.
        /// </summary>
        Delimiter, // strings, objects, and collectors

        /// <summary>
        /// This defines either the beginning or end of any other collection type.
        /// </summary>
        Collector
    }
}
