namespace TheHUtil.KingISS
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using TheHUtil.Extensions;

    internal class StyleGuideData
    {
        private static int numberOfCategories = Enum.GetNames(typeof(NotationCategories)).Length;

        internal List<StyleGuideEntry> Entries;

        public StyleGuideData()
        {
            this.Entries = new List<StyleGuideEntry>(numberOfCategories);
        }

        public StyleGuideData(string mark, NotationCategories category, bool? isDefinite = null, bool? isPlural = null, bool? isInitiator = null)
        {
            this.Entries = new List<StyleGuideEntry>()
            {
                new StyleGuideEntry(mark, category, isDefinite, isPlural, isInitiator)
            };
        }
    }
}
