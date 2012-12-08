using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class RationalSpace : ISpace<Rational>
    {
        public static readonly RationalSpace Default = new RationalSpace();

        public ISpace<Rational> Instance
        {
            get { return Default; }
        }

        public bool HasNorm
        {
            get { return true; }
        }

        public bool HasInnerProduct
        {
            get { return true; }
        }

        public bool HasCrossProduct
        {
            get { return false; }
        }

        public IField<Rational> Field
        {
            get { return RationalField.Default; }
        }

        public double Norm<TSpace>(Matrix<Rational, TSpace> v) where TSpace : ISpace<Rational>, new()
        {
            return System.Math.Sqrt((double)InnerProduct(v, v));
        }

        public Rational InnerProduct<TSpace>(Matrix<Rational, TSpace> v1, Matrix<Rational, TSpace> v2) where TSpace : ISpace<Rational>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != v2.Rows)
                throw new ArgumentException();

            Rational sum = 0;

            for (int i = 0; i < v1.Rows; i++)
                sum += v1.M[i, 0] * v2.M[i, 0];

            return sum;
        }

        public Matrix<Rational, TSpace> CrossProduct<TSpace>(Matrix<Rational, TSpace> v1, Matrix<Rational, TSpace> v2) where TSpace : ISpace<Rational>, new()
        {
            throw new NotImplementedException();
        }
    }
}
