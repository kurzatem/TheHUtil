namespace TheHUtil.KingISS
{
    using System.IO;
    using System.Xml.Linq;

    using TheHUtil.Pipeline;

    internal class KingISSReader
    {
        /* KingISS is a internal file structure for object notation.
         * It is meant to be simple and readable from a human standpoint.
         * Therefore, the usage of symbols must be limited or defined.
         * There are 7 keywords that are defined for use: "is", "are", "for", "separator", "delimiters", "notation", and "and".
         * The only types that the parser should know by default are "object", "array" and "tuple".
         * All of these words are to be understood by any parser written to deal with KingISS data structures.
         * With the exception of "is" all of these terms are not to appear inside the actual data portion of a KingISS data structure.
         * In order to understand how the words are defined, one must understand how the data is constructed.
         * Some of the following should be pretty well understood, but will included for completeness.
         * 
         * Within a KingISS data structure, there are 3 predefined symbols.
         * These are the quotation marks, for delimiting strings, the backslash, for providing the use of quotation marks within a string, and the comma, for separating items in a collection.
         * Data is understood using English terms that either have a definition that matches its function or are used in certain programming languages.
         * It should be noted that while the parser should only know of 3 data types by default, it can be allowed understand other data types.
         * In order to use symbols, one must define them using a guide for the parser.
         * How this is implemented is up to the developer and/or specification for the implementation.
         * However, here are a few suggestions for how this can accomplished.
         * 1) have the symbols hard-coded (recommended for a single system using the style).
         * 2) have a separate file (recommended for a multiple systems needing to share a common style).
         * 3) have a header within the data file (recommended for multiple systems that may not share a common style).
         * How styles are defined will be considered after the definitions.
         * 
         * The term "object" is meant to be understood as a 'complex' object.
         * Complexity in this case really just means that there is more than 1 value stored within it.
         * Every object therefore must define names for each of the values stored within.
         * This is to give some meaning to the values, otherwise the data is nearly useless.
         * The method that a single piece of data is written follows what is generally accepted: "'name' 'separator' 'value'".
         * Generally speaking, the type of object is given as the 'name' portion and the 'value' would use some form of notation to define where the grouping for that object's data ends.
         * When used in a KingISS style file, it refers to the notations that will be used to show the delimiters of an object's value.
         * This is used on within style guides.
         * 
         * The term "separator" refers to any symbol or word that separates a name for some data from it's value.
         * A typical separator is the equals sign.
         * Although vague, this is to distinguish it from the reserved keyword "is".
         * This is used only within style guides.
         * 
         * The term "is" refers to the 'root separator'.
         * In most object notation styles, there is only 1 separator defined with the data type to be inferred using some form of other notation.
         * An example of this would using quotation marks to delimit a string.
         * While there is nothing wrong with this style it leads to some unreadability due to the notation needing to be created for the parser to understand while it may not necessarily be used in written communication between humans.
         * Although KingISS is not meant to eliminate this practice, it is meant to provide some alternatives as well as provide a basis for ideas to be tried.
         * This is where the root separator comes into play.
         * The root separator is meant to serve the sole purpose of being a separator without giving any clues as to what type of data follows.
         * A couple of recommended separator symbols would using the colon for strings and the equals sign for numbers.
         * It is strongly recommended that the word is be reserved for anything that is a complex object.
         * 
         * The term "are" is synonymous with "is".
         * This should be used instead of "is" if a collection of any sort is used and no special separator is defined.
         * This must be used within a style guide for assigning delimiters.
         * 
         * The term "notation" refers to any styles used.
         * This is used only within style guides.
         * 
         * The term "for" is actually a special separator used specifically when assigning a specific symbol to a notation.
         * This is used only within style guides.
         * 
         * The term "and" is another special separator.
         * This is used with the specific purpose of defining the last item in a collection.
         * This is optional, but it's usage will allow the collection to be readable by a human.
         * From an implementation standpoint, caution must be taken as the error of a human using "and" more than once in a collection.
         * 
         * The term "array" refers to a collection that contains items of the same type.
         * This is used only within style guides.
         * 
         * The term "tuple" refers to a collection that contains items of differing types, but the types are always in the same order.
         * An object's value in this context is technically a tuple.
         * The difference is that an object must define names for all the items.
         * This is used only within style guides.
         * 
         * The term "delimiters" refers to the symbols that define the beginning and end of a particular collection.
         * This is used only within style guides.
         * 
         * To specify a style, there is a specific style that needs to be followed, which should be natural for fluent English speakers.
         * The most exhaustive example would be a header in the data file.
         * This would appear as so:
         * 
         * notation for string separator is :
         * notation for number separator is =
         * notation for object delimiters are { and }
         * 
         * For completeness, this header defines that a string utilizes the colon as a separator, a number uses the equals sign as a separator, and an object's delimiters as being curly braces.
         * 
         * One final note, a future version may allow for terms to be used that will designate singularity.
         * For instance, in the example style guide, the term "the" would be inserted at the beginning of the assignment to show that there is only one assignment of this being made.
         * To go along with that line of thought, the term "a" could then be used to signal that there are other notations that will be assigned the same meaning or something very similarly.
         * An example of that would be the additional assignments to the terms "is" and "are".
         */

        // Load
        // Parse
        // Serialize

        private static KingISSReader headerReader;
        
        private StreamReader streamReader;

        static KingISSReader()
        {

        }

        public KingISSReader(string fileName)
        {
            this.streamReader = new StreamReader(fileName);
        }

        public KingISSReader(Stream stream)
        {
            this.streamReader = new StreamReader(stream);
        }

        private void ReadHeader(string fileName)
        {

        }

        private void ReadBody(string fileName)
        {

        }

        public void Load()
        {
            
        }

        public void Load(string fileName)
        {
        }

        public void Load(Stream stream)
        {
        }

        public void Parse(string content)
        {

        }

        public void Parse(Stream stream)
        {
            this.streamReader = new StreamReader(stream);
            // Read line by line.
        }

        public void Deserialize(string fileName)
        {
            this.streamReader = new StreamReader(fileName);
            this.Deserialize();
        }

        public void Deserialize(Stream stream)
        {
            this.streamReader = new StreamReader(stream);
            this.Deserialize();
        }

        public void Deserialize()
        {
            // TODO: deserialize to a publicly available class
        }
    }
}
