using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class ComplexSpaceExtensions
    {
        private const int EigenPowThreshold = 1000000;

        #region Basic

        public static Matrix<Complex, ComplexSpace> ConjugateTranspose(this Matrix<Complex, ComplexSpace> m)
        {
            Matrix<Complex, ComplexSpace> newMatrix = new Matrix<Complex, ComplexSpace>(m.Columns, m.Rows);

            for (int i = 0; i < newMatrix.Rows; i++)
            {
                for (int j = 0; j < newMatrix.Columns; j++)
                {
                    newMatrix.M[j, i] = Complex.Conjugate(m.M[i, j]);
                }
            }

            return newMatrix;
        }

        public static Matrix<Complex, ComplexSpace> Normalize(this Matrix<Complex, ComplexSpace> v)
        {
            return v / v.Norm();
        }

        public static Matrix<double, RealSpace> Normalize(this Matrix<double, RealSpace> v)
        {
            return v / v.Norm();
        }

        #endregion

        #region Eigenvectors

        public static Complex[] Eigenvalues(this Matrix<Complex, ComplexSpace> matrix)
        {
            var characteristicPolynomial = matrix.CharacteristicPolynomial<ComplexField>();
            var eigenvalues = characteristicPolynomial.FindRoots();

            return eigenvalues.ToArray();
        }

        public static double[] Eigenvalues(this Matrix<double, RealSpace> matrix)
        {
            Complex[] eigenvalues = Matrix<Complex, ComplexSpace>.FromMatrix(matrix, c => c).Eigenvalues();
            List<double> realValues = new List<double>();

            for (int i = 0; i < eigenvalues.Length; i++)
            {
                if (eigenvalues[i].ApproxEquals(eigenvalues[i].Re))
                    realValues.Add(eigenvalues[i].Re);
            }

            return realValues.ToArray();
        }

        public static bool Eigendecomposition(this Matrix<Complex, ComplexSpace> matrix, out Matrix<Complex, ComplexSpace> p, out Matrix<Complex, ComplexSpace> d)
        {
            var eigenvalues = matrix.Eigenvalues();

            if (eigenvalues.Length != matrix.Rows)
            {
                p = null;
                d = null;
                return false;
            }

            var eigenvectors = new Matrix<Complex, ComplexSpace>[matrix.Rows];

            for (int i = 0; i < eigenvalues.Length; i++)
                eigenvectors[i] = matrix.Eigenspace(eigenvalues[i])[0];

            p = Matrix<Complex, ComplexSpace>.FromColumns(eigenvectors);
            d = Matrix<Complex, ComplexSpace>.Diagonal(eigenvalues);

            return true;
        }

        public static bool Eigendecomposition(this Matrix<double, RealSpace> matrix, out Matrix<double, RealSpace> p, out Matrix<double, RealSpace> d)
        {
            p = null;
            d = null;

            var eigenvalues = matrix.Eigenvalues();

            if (eigenvalues.Length != matrix.Rows)
            {
                return false;
            }

            var eigenvectors = new Matrix<double, RealSpace>[matrix.Rows];

            for (int i = 0; i < eigenvalues.Length; i++)
            {
                var eigenspace = matrix.Eigenspace(eigenvalues[i]);

                if (eigenspace.Length == 0)
                    return false;

                eigenvectors[i] = eigenspace[0];
            }

            p = Matrix<double, RealSpace>.FromColumns(eigenvectors);
            d = Matrix<double, RealSpace>.Diagonal(eigenvalues);

            return true;
        }

        public static Matrix<Complex, ComplexSpace> JordanNormalForm(this Matrix<Complex, ComplexSpace> matrix, out Matrix<Complex, ComplexSpace> p)
        {
            return matrix.JordanNormalForm(matrix.Eigenvalues(), out p, null);
        }

        public static void DiagonalPow(Matrix<Complex, ComplexSpace> matrix, int n)
        {
            for (int i = 0; i < matrix.Rows; i++)
                matrix.M[i, i] = Complex.Pow(matrix.M[i, i], n);
        }

        public static Matrix<Complex, ComplexSpace> Pow(this Matrix<Complex, ComplexSpace> matrix, int n, bool eigendecomposition)
        {
            if (!matrix.IsSquare)
                throw new InvalidOperationException();

            if (n == 0)
                return Matrix<Complex, ComplexSpace>.Identity(matrix.Rows);

            if (eigendecomposition && n > EigenPowThreshold)
            {
                Matrix<Complex, ComplexSpace> p;
                Matrix<Complex, ComplexSpace> d;

                if (matrix.Eigendecomposition(out p, out d))
                {
                    DiagonalPow(d, n);
                    return p * d * p.Inverse();
                }
            }

            return matrix.Pow(n);
        }

        public static Matrix<double, RealSpace> Pow(this Matrix<double, RealSpace> matrix, int n, bool eigendecomposition)
        {
            if (!matrix.IsSquare)
                throw new InvalidOperationException();

            if (n == 0)
                return Matrix<double, RealSpace>.Identity(matrix.Rows);

            if (eigendecomposition && n > EigenPowThreshold)
            {
                Matrix<Complex, ComplexSpace> p;
                Matrix<Complex, ComplexSpace> d;

                if (Matrix<Complex, ComplexSpace>.FromMatrix(matrix, r => r).Eigendecomposition(out p, out d))
                {
                    DiagonalPow(d, n);
                    return Matrix<double, RealSpace>.FromMatrix(p * d * p.Inverse(), c => c.Re);
                }
            }

            return matrix.Pow(n);
        }

        #endregion

        #region Gram-Schmidt Process

        public static Matrix<Complex, ComplexSpace> GramSchmidt(this Matrix<Complex, ComplexSpace> matrix)
        {
            var basis = new Matrix<Complex, ComplexSpace>[matrix.Rows];

            for (int i = 0; i < basis.Length; i++)
            {
                var a = matrix.GetColumn(i);

                for (int j = 0; j < i; j++)
                {
                    a -= Matrix<Complex, ComplexSpace>.Projection(a, basis[j]);
                }

                basis[i] = a;
            }

            for (int i = 0; i < basis.Length; i++)
                basis[i] = basis[i].Normalize();

            return Matrix<Complex, ComplexSpace>.FromColumns(basis);
        }

        public static Matrix<double, RealSpace> GramSchmidt(this Matrix<double, RealSpace> matrix)
        {
            var basis = new Matrix<double, RealSpace>[matrix.Rows];

            for (int i = 0; i < basis.Length; i++)
            {
                var a = matrix.GetColumn(i);

                for (int j = 0; j < i; j++)
                {
                    a -= Matrix<double, RealSpace>.Projection(a, basis[j]);
                }

                basis[i] = a;
            }

            for (int i = 0; i < basis.Length; i++)
                basis[i] = basis[i].Normalize();

            return Matrix<double, RealSpace>.FromColumns(basis);
        }

        public static void QrDecomposition(this Matrix<Complex, ComplexSpace> matrix, out Matrix<Complex, ComplexSpace> q, out Matrix<Complex, ComplexSpace> r)
        {
            q = matrix.GramSchmidt();
            r = q.Transpose() * matrix;
        }

        public static void QrDecomposition(this Matrix<double, RealSpace> matrix, out Matrix<double, RealSpace> q, out Matrix<double, RealSpace> r)
        {
            q = matrix.GramSchmidt();
            r = q.Transpose() * matrix;
        }

        #endregion
    }
}
