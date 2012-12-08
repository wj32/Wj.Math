using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class FiniteSet
    {
        public static FiniteSet<Pair<T, U>> CartesianProduct<T, U>(FiniteSet<T> st, FiniteSet<U> su) where T : IEquatable<T> where U : IEquatable<U>
        {
            HashSet<Pair<T, U>> set = new HashSet<Pair<T, U>>();

            foreach (T t in st)
            {
                foreach (U u in su)
                {
                    set.Add(new Pair<T, U>(t, u));
                }
            }

            return new FiniteSet<Pair<T, U>>(set);
        }
    }
}
