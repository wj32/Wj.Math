using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public interface IField<T> : IRing<T>
    {
        new IField<T> Instance { get; }
        T Inverse(T t);
        T Divide(T t1, T t2);
        T Divide(T t, int n);
    }
}
