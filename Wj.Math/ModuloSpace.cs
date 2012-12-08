using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class ModuloSpace<TPrime> : ISpace<int> where TPrime : TIntegerType, new()
    {
        public static readonly ModuloSpace<TPrime> Default = new ModuloSpace<TPrime>();

        public ISpace<int> Instance
        {
            get { return Default; }
        }

        public bool HasNorm
        {
            get { return false; }
        }

        public bool HasInnerProduct
        {
            get { return true; }
        }

        public bool HasCrossProduct
        {
            get { return false; }
        }

        public IField<int> Field
        {
            get { return ModuloField<TPrime>.Default; }
        }

        public double Norm<TSpace>(Matrix<int, TSpace> v) where TSpace : ISpace<int>, new()
        {
            throw new NotSupportedException();
        }

        public int InnerProduct<TSpace>(Matrix<int, TSpace> v1, Matrix<int, TSpace> v2) where TSpace : ISpace<int>, new()
        {
            if (!v1.IsVector || !v2.IsVector || v1.Rows != v2.Rows)
                throw new ArgumentException();

            int sum = 0;

            for (int i = 0; i < v1.Rows; i++)
                sum += v1.M[i, 0] * v2.M[i, 0];

            return sum;
        }

        public Matrix<int, TSpace> CrossProduct<TSpace>(Matrix<int, TSpace> v1, Matrix<int, TSpace> v2) where TSpace : ISpace<int>, new()
        {
            throw new NotSupportedException();
        }
    }
}
