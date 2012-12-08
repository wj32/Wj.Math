using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class RealSpace : ISpace<double>
    {
        public static readonly RealSpace Default = new RealSpace();

        public ISpace<double> Instance
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
            get { return true; }
        }

        public IField<double> Field
        {
            get { return RealField.Default; }
        }

        public double Norm<TSpace>(Matrix<double, TSpace> v) where TSpace : ISpace<double>, new()
        {
            if (!v.IsVector)
                throw new ArgumentException();

            double sum = 0;

            for (int i = 0; i < v.Rows; i++)
                sum += v.M[i, 0] * v.M[i, 0];

            return System.Math.Sqrt(sum);
        }

        public double InnerProduct<TSpace>(Matrix<double, TSpace> v1, Matrix<double, TSpace> v2) where TSpace : ISpace<double>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != v2.Rows)
                throw new ArgumentException();

            double sum = 0;

            for (int i = 0; i < v1.Rows; i++)
                sum += v1.M[i, 0] * v2.M[i, 0];

            return sum;
        }

        public Matrix<double, TSpace> CrossProduct<TSpace>(Matrix<double, TSpace> v1, Matrix<double, TSpace> v2) where TSpace : ISpace<double>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != 3 || v2.Rows != 3)
                throw new ArgumentException();

            Matrix<double, TSpace> v3 = new Matrix<double, TSpace>(3, 1);

            v3.M[0, 0] = v1.M[1, 0] * v2.M[2, 0] - v1.M[2, 0] * v2.M[1, 0];
            v3.M[1, 0] = v1.M[2, 0] * v2.M[0, 0] - v1.M[0, 0] * v2.M[2, 0];
            v3.M[2, 0] = v1.M[0, 0] * v2.M[1, 0] - v1.M[1, 0] * v2.M[0, 0];

            return v3;
        }
    }
}
