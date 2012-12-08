using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class ComplexSpace : ISpace<Complex>
    {
        public static readonly ComplexSpace Default = new ComplexSpace();

        public ISpace<Complex> Instance
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

        public IField<Complex> Field
        {
            get { return ComplexField.Default; }
        }

        public double Norm<TSpace>(Matrix<Complex, TSpace> v) where TSpace : ISpace<Complex>, new()
        {
            if (!v.IsVector)
                throw new ArgumentException();

            double sum = 0;

            for (int i = 0; i < v.Rows; i++)
            {
                Complex e = v.M[i, 0];

                sum += e.Re * e.Re + e.Im * e.Im;
            }

            return System.Math.Sqrt(sum);
        }

        public Complex InnerProduct<TSpace>(Matrix<Complex, TSpace> v1, Matrix<Complex, TSpace> v2) where TSpace : ISpace<Complex>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != v2.Rows)
                throw new ArgumentException();

            Complex sum = 0;

            for (int i = 0; i < v1.Rows; i++)
                sum += v1.M[i, 0] * Complex.Conjugate(v2.M[i, 0]);

            return sum;
        }

        public Matrix<Complex, TSpace> CrossProduct<TSpace>(Matrix<Complex, TSpace> v1, Matrix<Complex, TSpace> v2) where TSpace : ISpace<Complex>, new()
        {
            throw new NotSupportedException();
        }
    }
}
