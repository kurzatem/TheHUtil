namespace TheHUtil.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ObjectExt
    {
        public static bool ReferenceEquals(this object objA, object objB)
        {
            return object.ReferenceEquals(objA, objB);
        }

        public static bool IsNull(this object objA)
        {
            return object.ReferenceEquals(objA, null);
        }
    }
}
