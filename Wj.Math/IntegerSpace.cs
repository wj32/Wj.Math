using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public class IntegerSpace : ISpace<BigInteger>
    {
        public static readonly IntegerSpace Default = new IntegerSpace();

        public ISpace<BigInteger> Instance
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

        public IField<BigInteger> Field
        {
            get { return IntegerField.Default; }
        }

        public double Norm<TSpace>(Matrix<BigInteger, TSpace> v) where TSpace : ISpace<BigInteger>, new()
        {
            return System.Math.Sqrt((double)InnerProduct(v, v));
        }

        public BigInteger InnerProduct<TSpace>(Matrix<BigInteger, TSpace> v1, Matrix<BigInteger, TSpace> v2) where TSpace : ISpace<BigInteger>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != v2.Rows)
                throw new ArgumentException();

            BigInteger sum = 0;

            for (int i = 0; i < v1.Rows; i++)
                sum += v1.M[i, 0] * v2.M[i, 0];

            return sum;
        }

        public Matrix<BigInteger, TSpace> CrossProduct<TSpace>(Matrix<BigInteger, TSpace> v1, Matrix<BigInteger, TSpace> v2) where TSpace : ISpace<BigInteger>, new()
        {
            throw new NotSupportedException();
        }
    }
}
