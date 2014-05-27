namespace TheHUtil.Logging.Boilerplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using TheHUtil.Extensions;

    public static partial class ObjectExt
    {
        public static T New<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public static T Clone<T>(this T subject)
        {
            return ObjectExtCache<T>.Clone(subject);
        }

        public static int GetHashCode<T>(this T subject)
        {
            return ObjectExtCache<T>.GetHashCode(subject);
        }

        public static bool Equals<T>(this T subject, T other)
        {
            return ObjectExtCache<T>.Equals(subject, other);
        }

        static class ObjectExtCache<T>
        {
            private static Func<T, T> cloneTree;

            private static Func<T, T, bool> equalsTree;

            private static Func<T, int> hashCodeTree;
            
            private static Func<T> parameterlessCtor;
            
            private static Func<T, T> MakeCloneMethod()
            {
                var subjectAsParameter = Expression.Parameter(typeof(T), "in");

                var properties = from property in typeof(T).GetProperties()
                                 where property.CanRead && property.CanWrite
                                 select (MemberBinding)Expression.Bind(property,
                                     Expression.Property(subjectAsParameter, property));

                return Expression.Lambda<Func<T, T>>(
                    Expression.MemberInit(
                        Expression.New(typeof(T)), properties), subjectAsParameter).Compile();
            }

            private static Func<T, T, bool> MakeEqualsMethod()
            {
                var typeofT = typeof(T);

                var aAsParameter = Expression.Parameter(typeofT, "a");
                var bAsParameter = Expression.Parameter(typeofT, "b");

                var comparisons = typeofT.GetProperties().
                    Where(p => p.CanRead).
                    Select(p => Expression.Equal(Expression.Property(aAsParameter, p), Expression.Property(bAsParameter, p)));

                Expression result = null;
                foreach (var comparer in comparisons)
                {
                    if (result.IsNull())
                    {
                        result = comparer;
                    }
                    else
                    {
                        result = Expression.AndAlso(result, comparer);
                    }
                }
                
                return Expression.Lambda<Func<T, T, bool>>(result, aAsParameter, bAsParameter).Compile();
            }

            private static Func<T, int> MakeGetHashCodeMethod()
            {
                var typeofT = typeof(T);
                var subjectAsParameter = Expression.Parameter(typeofT, "x");

                var hashes = typeofT.GetProperties().
                    Where(p => p.CanRead).
                    Select(p => Expression.Call(Expression.Property(subjectAsParameter, p), "GetHashCode", Type.EmptyTypes));

                Expression result = null;
                foreach (var call in hashes)
                {
                    if (result.IsNull())
                    {
                        result = call;
                    }
                    else
                    {
                        result = Expression.ExclusiveOr(result, call);
                    }
                }

                return Expression.Lambda<Func<T, int>>(result, subjectAsParameter).Compile();
            }

            public static T Clone(T subject)
            {
                if (cloneTree.IsNull())
                {
                    cloneTree = MakeCloneMethod();
                }

                return cloneTree(subject);
            }

            public static bool Equals(T subject, T other)
            {
                if (equalsTree.IsNull())
                {
                    equalsTree = MakeEqualsMethod();
                }

                return equalsTree(subject, other);
            }

            public static int GetHashCode(T subject)
            {
                if (hashCodeTree.IsNull())
                {
                    hashCodeTree = MakeGetHashCodeMethod();
                }

                return hashCodeTree(subject);
            }
        }
    }
}
