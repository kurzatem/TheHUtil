namespace TheHUtil.KingISS
{
    using System;

    using TheHUtil.Extensions;

    internal struct Definition : IEquatable<Definition>
    {
        public readonly NotationCategories Category;

        public readonly bool? IsDefinite;

        public readonly bool? IsPlural;

        public readonly bool? IsInitiator;

        public Definition(NotationCategories category, bool? isDefinite, bool? isPlural, bool? isInitiator)
        {
            this.Category = category;
            if (category == NotationCategories.Delimiter || category == NotationCategories.Collector)
            {
                this.IsInitiator = isInitiator;
            }
            else
            {
                this.IsInitiator = null;
            }

            this.IsDefinite = isDefinite;
            this.IsPlural = isPlural;
        }

        public static bool operator ==(Definition a, Definition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Definition a, Definition b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            return !obj.IsNull() && obj is Definition && this.Equals((Definition)obj);
        }

        public bool Equals(Definition other)
        {
            return
                this.Category == other.Category &&
                this.IsDefinite == other.IsDefinite &&
                this.IsInitiator == other.IsInitiator &&
                this.IsPlural == other.IsPlural;
        }

        public override int GetHashCode()
        {
            return
                (this.IsInitiator.GetHashCode() << 5) +
                (this.IsPlural.GetHashCode() << 4) +
                (this.IsDefinite.GetHashCode() << 3) +
                (int)this.Category;
        }

        public override string ToString()
        {
            var pluralPlug = string.Empty;
            var definitePlug = string.Empty;
            var initiatorPlug = string.Empty;
            if (this.IsPlural.HasValue)
            {
                pluralPlug = this.IsPlural.Value ? "plural" : "singular";
            }

            if (this.IsDefinite.HasValue)
            {
                definitePlug = this.IsDefinite.Value ? "definite" : "indefinite";
            }

            if (this.IsInitiator.HasValue)
            {
                initiatorPlug = this.IsInitiator.Value ? " initiator" : " terminator";
            }

            return string.Format
                (
                    "Notation is {0} {1} {2}{3}.",
                    pluralPlug,
                    definitePlug,
                    this.Category.ToString(),
                    initiatorPlug
                );            
        }
    }
}
