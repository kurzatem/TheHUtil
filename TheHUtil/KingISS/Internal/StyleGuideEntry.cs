namespace TheHUtil.KingISS
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using TheHUtil.Extensions;

    internal class StyleGuideEntry : IEquatable<StyleGuideEntry>, ISerializable
    {
        public Definition Definition
        {
            get;
            set;
        }

        public string Mark
        {
            get;
            set;
        }

        public StyleGuideEntry(string mark, NotationCategories category, bool? isDefinite = null, bool? isPlural = null, bool? isInitiator = null)
        {
            this.Mark = mark;
            this.Definition = new Definition(category, isDefinite, isInitiator, isPlural);
        }

        public override bool Equals(object obj)
        {
            return !obj.IsNull() && obj is StyleGuideEntry && this.Equals(obj as StyleGuideEntry);
        }

        public bool Equals(StyleGuideEntry other)
        {
            return
                this.Mark == other.Mark &&
                this.Definition == other.Definition;
        }

        public override int GetHashCode()
        {
            return this.Definition.GetHashCode() ^ this.Mark.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // NOTE: May not need this.
        }
    }
}
