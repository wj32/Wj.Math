using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public static class PolynomialExtensions
    {
        private static Complex _defaultDkStart = new Complex(0.4, 0.9);

        #region Root Finding

        public static Complex FindRoot(this Polynomial<Complex, ComplexField> polynomial, Complex start)
        {
            return polynomial.FindRootLaguerre(start);
        }

        public static Complex FindRootLaguerre(this Polynomial<Complex, ComplexField> polynomial, Complex start)
        {
            Polynomial<Complex, ComplexField> p;
            Polynomial<Complex, ComplexField> pd1;
            Polynomial<Complex, ComplexField> pd2;
            Complex x;

            p = polynomial.RemoveMultipleRoots().MakeMonic();

            if (p.Degree == 0)
                return Complex.Zero;

            pd1 = p.Differentiate();
            pd2 = pd1.Differentiate();

            x = start;

            while (true)
            {
                Complex y;

                y = p.Evaluate(x);

                if (y == 0)
                    break;

                Complex g = pd1.Evaluate(x) / y;
                Complex gSquared = g * g;
                Complex h = gSquared - (pd2.Evaluate(x) / y);
                Complex denomPart = Complex.Sqrt((p.Degree - 1) * (p.Degree * h - gSquared));
                Complex denom = g + denomPart;
                Complex denom2 = g - denomPart;

                if (denom2.Abs > denom.Abs)
                    denom = denom2;

                Complex newX = x - ((double)p.Degree / denom);

                if (x.ApproxEquals(newX))
                    break;

                x = newX;
            }

            return x;
        }

        public static Complex[] FindRoots(this Polynomial<Complex, ComplexField> polynomial)
        {
            return polynomial.FindRootsDk();
        }

        public static Complex[] FindRootsDk(this Polynomial<Complex, ComplexField> polynomial)
        {
            return polynomial.FindRootsDk(_defaultDkStart);
        }

        public static Complex[] FindRootsDk(this Polynomial<Complex, ComplexField> polynomial, Complex start)
        {
            Polynomial<Complex, ComplexField> p;
            Complex[] roots;
            bool[] done;
            int doneCount = 0;

            p = polynomial.RemoveMultipleRoots().MakeMonic();

            if (p.Degree == 0)
                return new Complex[0];

            roots = new Complex[p.Degree];
            done = new bool[p.Degree];

            for (int i = 0; i < roots.Length; i++)
                roots[i] = Complex.Pow(start, i);

            while (true)
            {
                for (int i = 0; i < roots.Length; i++)
                {
                    Complex oldRoot;
                    Complex nom;
                    Complex denom;

                    if (done[i])
                        continue;

                    oldRoot = roots[i];
                    nom = p.Evaluate(oldRoot);
                    denom = 1;

                    for (int j = 0; j < roots.Length; j++)
                    {
                        if (j == i)
                            continue;

                        denom *= oldRoot - roots[j];
                    }

                    roots[i] = oldRoot - nom / denom;

                    if (roots[i].ApproxEquals(oldRoot))
                    {
                        done[i] = true;
                        doneCount++;
                    }
                }

                if (doneCount == roots.Length)
                    break;
            }

            return roots;
        }

        #endregion

        #region Matrices

        public static Matrix<T, TSpace> Evaluate<T, TField, TSpace>(this Polynomial<T, TField> polynomial, Matrix<T, TSpace> x)
            where T : IEquatable<T>
            where TField : IField<T>, new()
            where TSpace : ISpace<T>, new()
        {
            if (!x.IsSquare)
                throw new InvalidOperationException();

            Matrix<T, TSpace> y = Matrix<T, TSpace>.Zero(x.Rows);

            foreach (Term<T> term in polynomial)
            {
                y += term.Coeff * x.Pow(term.Deg);
            }

            return y;
        }

        #endregion
    }
}
