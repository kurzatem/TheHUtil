namespace TheHUtil.KingISS
{
    internal enum Keywords
    {
        The,        // Definite Notation
        A,          // Indefinite Notation
        Is,         // Singularity Assigner
        Are,        // Plurality Assigner
        For,        // Definite (Category) Assigner
        Of,         // Indefinite (Category) Assigner
        Separator,  // Category
        Delimiter,  // Category
        Notation,   // Category
        And,        // Definite Terminal Separator
        Object,     //
        Array,      //
        Tuple       //

        // Arrays & Tuples are Collections (The notations for these are collectors)
        // Objects are Tuples

        // Delimiters and Separators are Notations
        // A, For and The are Notations
        // Collectors are Delimiters
        // Assigners are Separators
        // And is a Separator
        // Is and Are are Assigners

        /*
         *                  Delimiters  ->  Collectors
         * Notations    <                   
         *                  Separators  ->  Assigners
        */
    }
}
