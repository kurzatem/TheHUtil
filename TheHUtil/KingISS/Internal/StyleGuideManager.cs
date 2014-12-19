namespace TheHUtil.KingISS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TheHUtil.Extensions;

    internal class StyleGuideManager
    {
        private const string Invalid_Type_Parameter_Exception_Message = "Nothing has been inserted for the type given. Check generic parameter.";

        internal Dictionary<Type, StyleGuideData> styleGuides;

        // What is needed for reading a KingISS file:
        /*
         * The style guide data that will allow for character/keyword lookups to determine the definition for it.
         * That can be accomplished by a simple Contains->Get setup which can be called on each character or group.
         * Might need to create methods that do just that within the style guide data and/or manager.
         * The extra metadata would be useful in reading, but not required.
         *  
         * How it needs to access the data: a string should be passed in character by character.
         * Each time it should be noted if there is a full, partial, or no match.
         * Note: if there are partial matches between strings in the style guide, then the next character must be examined.
         */

        // What is needed for reading a KingISS header:
        /*
         * A style guide that holds the default keywords.
         */

        // What is needed for writing a KingISS file:
        /*
         * Besides serialization data, the style guide data that defines the separators and delimiters.
         * Determine what the type of object (object.GetType will suffice) getting serialized is and pop the correct notation in.
         * The notation needed would need both the context and type.
         * For example: To assign a property to a string, the notation for it would be the assigner for a string.
         * 
         * To find it via type->context->notation:
         * Data would be stored internally using the context as a key and the notation as the value.
         * The data would then be indexed by type of object and the value would be the entry.
         */

        // What is needed for writing a KingISS header:
        /*
         * The style guide data should have all the information needed to write the header.
         * Remember that a typical notation would be: The delimiters for a string are " and "
         * Note: only type names can be inserted other than the default keywords.
         * An exception would be when the header writer is working, then the program could generate headers using the expanded keywords.
         */
        
        public StyleGuideManager()
        {
            this.styleGuides = new Dictionary<Type, StyleGuideData>();
        }

        public static StyleGuideManager GetDefaultManager()
        {
            /*
             * Defaults:
             * Notation, For, Separator, Delimiter, Is, """, & ","
             */ 
            
            var result = new StyleGuideManager();
            result.InsertNotation<object>("Notation", NotationCategories.Notation);
            result.InsertNotation<object>("For", NotationCategories.Assigner, true);
            result.InsertNotation<object>("Separator", NotationCategories.Separator, true);
            result.InsertNotation<object>("Delimiter", NotationCategories.Delimiter, true);
            result.InsertNotation<object>("Is", NotationCategories.Assigner, true);
            result.InsertNotation<string>("\"", NotationCategories.Delimiter, true, false, true);
            result.InsertNotation<string>("\"", NotationCategories.Delimiter, true, false, false);
            result.InsertNotation<object>(",", NotationCategories.Separator, true);
            return result;
        }

        public static StyleGuideManager GetExtendedDefaultManager()
        {
            /* Extended:
             * The, A, All, Are, Of, And
             */
            
            var result = GetDefaultManager();
            result.InsertNotation<object>("The", NotationCategories.Notation, true, false);
            result.InsertNotation<object>("A", NotationCategories.Notation, false, false);
            result.InsertNotation<object>("All", NotationCategories.Notation, true, true);
            result.InsertNotation<object>("Are", NotationCategories.Delimiter, true, true);
            result.InsertNotation<object>("Of", NotationCategories.Assigner, false);
            result.InsertNotation<object>("And", NotationCategories.Separator, true, false, false);
            return result;
        }

        private bool TryToRemoveMark(string mark, Type typeOfT)
        {
            int indexToRemove = this.styleGuides[typeOfT].Entries.FindIndex(sge => sge.Mark == mark);
            if (indexToRemove > -1)
            {
                this.styleGuides[typeOfT].Entries.RemoveAt(indexToRemove);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeMarkForNotation<T>(string newMark, NotationCategories category, bool? isDefinite = null, bool? isPlural = null, bool? isInitiator = null)
        {
            var typeOfT = typeof(T);
            if (this.styleGuides.ContainsKey(typeOfT))
            {
                var definition = new Definition(category, isDefinite, isPlural, isInitiator);
                var entryNumber = this.styleGuides[typeOfT].Entries.FindIndex(sge => sge.Definition == definition);
                if (entryNumber > -1)
                {
                    this.styleGuides[typeOfT].Entries[entryNumber].Mark = newMark;
                }
                else
                {
                    throw new ArgumentException("No entry matches the type and definition given.");
                }
            }
            else
            {
                throw new ArgumentException(Invalid_Type_Parameter_Exception_Message);
            }
        }

        public void ChangeNotationForMark<T>(string mark, NotationCategories newCategory, bool? newIsDefinite = null, bool? newIsPlural = null, bool? newIsInitiator = null)
        {
            var typeOfT = typeof(T);
            if (this.styleGuides.ContainsKey(typeOfT))
            {
                var entry = this.styleGuides[typeOfT].Entries.Find(sge => sge.Mark == mark);
                if (entry.IsNull())
                {
                    throw new ArgumentException("No entries found for give mark.", "mark");
                }
                else
                {
                    entry.Definition = new Definition(newCategory, newIsDefinite, newIsPlural, newIsInitiator);
                }
            }
            else
            {
                throw new ArgumentException(Invalid_Type_Parameter_Exception_Message);
            }
        }

        public IEnumerable<string> GetMarksFor<T>()
        {
            return this.styleGuides[typeof(T)].Entries.Select(sge => sge.Mark);
        }

        public string GetMarkForNotation<T>(string mark, NotationCategories category, bool? isDefinite = null, bool? isPlural = null, bool? isInitiator = null)
        {
            var definition = new Definition(category, isDefinite, isPlural, isInitiator);
            var entry = this.styleGuides[typeof(T)].Entries.Find(sge => sge.Definition == definition);
            if (entry.IsNull())
            {
                throw new ArgumentException("No entry matches the given definition");
            }
            else
            {
                return entry.Mark;
            }
        }

        public void InsertNotation<T>(string mark, NotationCategories category, bool? isDefinite = null, bool? isPlural = null, bool? isInitiator = null)
        {
            var typeOfT = typeof(T);
            // Check by type
            if (this.styleGuides.ContainsKey(typeOfT))
            {
                var newNotation = new StyleGuideEntry(mark, category, isDefinite, isPlural, isInitiator);
                // Check by notation metadata
                if (this.styleGuides[typeOfT].Entries.Contains(newNotation))
                {
                    throw new ArgumentException("Notation desired already exists for the " + typeOfT.Name + " data type");
                }
                else
                {
                    this.styleGuides[typeOfT].Entries.Add(newNotation);
                }
            }
            else
            {
                this.styleGuides.Add(typeOfT, new StyleGuideData(mark, category, isDefinite, isPlural, isInitiator));
            }
        }
        
        public void RemoveNotationFor<T>(string mark)
        {
            var typeOfT = typeof(T);
            this.TryToRemoveMark(mark, typeOfT);
        }

        public void RemoveNotation(string mark)
        {
            foreach (var type in this.styleGuides.Keys)
            {
                if (this.TryToRemoveMark(mark, type))
                {
                    return;
                }
            }
        }
    }
}
