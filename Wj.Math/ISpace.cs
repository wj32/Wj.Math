using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    /// <summary>
    /// Support functions for T^n vector space for some field T.
    /// </summary>
    /// <remarks>
    /// This does not represent a generic abstract vector space.
    /// </remarks>
    public interface ISpace<T>
        where T : IEquatable<T>
    {
        ISpace<T> Instance { get; }
        bool HasNorm { get; }
        bool HasInnerProduct { get; }
        bool HasCrossProduct { get; }

        IField<T> Field { get; }
        double Norm<TSpace>(Matrix<T, TSpace> v) where TSpace : ISpace<T>, new();
        T InnerProduct<TSpace>(Matrix<T, TSpace> v1, Matrix<T, TSpace> v2) where TSpace : ISpace<T>, new();
        Matrix<T, TSpace> CrossProduct<TSpace>(Matrix<T, TSpace> v1, Matrix<T, TSpace> v2) where TSpace : ISpace<T>, new();
    }
}
