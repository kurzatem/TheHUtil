namespace TheHUtilTests.Logging.Boilerplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Testee : IEquatable<Testee>, ICloneable
    {
        public int Integer
        {
            get;
            set;
        }

        public byte Partial
        {
            get;
            set;
        }

        public Testee()
        {
            this.Integer = 107;
            this.Partial = 9;
        }

        public override bool Equals(object obj)
        {
            return (obj is Testee) ? this.Equals(obj as Testee) : false;
        }

        public bool Equals(Testee other)
        {
            return this.Integer == other.Integer;
        }

        public override int GetHashCode()
        {
            return this.Integer ^ this.Partial;
        }

        public override string ToString()
        {
            return string.Format("Integer: {0}\nPartial: {1}", this.Integer, this.Partial);
        }

        public object Clone()
        {
            var result = new Testee();
            result.Integer = this.Integer;
            result.Partial = this.Partial;
            return result;
        }
    }
}
