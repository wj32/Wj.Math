using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class PolynomialSpace<T, TField> :
        ISpace<Polynomial<T, TField>>
        where T : IEquatable<T>
        where TField : IField<T>, new()
    {
        public static readonly PolynomialSpace<T, TField> Default = new PolynomialSpace<T, TField>();

        public ISpace<Polynomial<T, TField>> Instance
        {
            get { return Default; }
        }

        public bool HasNorm
        {
            get { return false; }
        }

        public bool HasInnerProduct
        {
            get { return false; }
        }

        public bool HasCrossProduct
        {
            get { return false; }
        }

        public IField<Polynomial<T, TField>> Field
        {
            get { return PolynomialField<T, TField>.Default; }
        }

        public double Norm<TSpace>(Matrix<Polynomial<T, TField>, TSpace> v) where TSpace : ISpace<Polynomial<T, TField>>, new()
        {
            throw new NotSupportedException();
        }

        public Polynomial<T, TField> InnerProduct<TSpace>(Matrix<Polynomial<T, TField>, TSpace> v1, Matrix<Polynomial<T, TField>, TSpace> v2) where TSpace : ISpace<Polynomial<T, TField>>, new()
        {
            throw new NotSupportedException();
        }

        public Matrix<Polynomial<T, TField>, TSpace> CrossProduct<TSpace>(Matrix<Polynomial<T, TField>, TSpace> v1, Matrix<Polynomial<T, TField>, TSpace> v2) where TSpace : ISpace<Polynomial<T, TField>>, new()
        {
            throw new NotSupportedException();
        }
    }
}
