using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class RingExtensions
    {
        public static T DefaultPow<T>(this IRing<T> ring, T t, int n)
        {
            T b = t;
            T result = ring.One;

            while (n != 0)
            {
                if (n % 2 != 0)
                    result = ring.Multiply(result, b);

                if (n == 1)
                    break;

                n /= 2;
                b = ring.Multiply(b, b);
            }

            return result;
        }

        public static Pair<T, T> ExtendedEuclidean<T, TRing>(this TRing domain, T x, T y) where T : IEquatable<T> where TRing : IRing<T>, IEuclideanDomain<T>
        {
            if (domain.IsZero(y))
            {
                return new Pair<T, T>(domain.One, domain.Zero);
            }
            else
            {
                T q;
                T r;

                q = domain.Divide(x, y, out r);

                var result = ExtendedEuclidean<T, TRing>(domain, y, r);

                return new Pair<T, T>(result.Second, domain.Subtract(result.First, domain.Multiply(q, result.Second)));
            }
        }
    }
}
