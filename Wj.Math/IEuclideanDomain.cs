using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public interface IEuclideanDomain<T>
    {
        BigInteger Euclidean(T t);
        T Divide(T t1, T t2, out T r);
        T Gcd(T t1, T t2);
        T Gcd(T[] t, T[] q);
        T Lcm(T t1, T t2);
        T Lcm(T[] t, T[] q);
    }
}
