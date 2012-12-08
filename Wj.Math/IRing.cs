using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public interface IRing<T>
    {
        IRing<T> Instance { get; }
        bool NeedsInitialize { get; }
        T Zero { get; }
        T One { get; }

        bool IsZero(T t);
        bool IsOne(T t);
        bool IsApproxZero(T t);
        bool IsUnit(T t);
        bool Divides(T t, T divisor);
        T Add(T t1, T t2);
        T Negate(T t);
        T Subtract(T t1, T t2);
        T Multiply(T t1, T t2);
        T Multiply(T t, int n);
        T Pow(T t, int n);
    }
}
