using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public class PolynomialField<T, TField> :
        IField<Polynomial<T, TField>>,
        IEuclideanDomain<Polynomial<T, TField>>
        where T : IEquatable<T>
        where TField : IField<T>, new()
    {
        public static readonly PolynomialField<T, TField> Default = new PolynomialField<T, TField>();

        private IField<T> _field = (new TField()).Instance;

        #region IField<Polynomial<T, TField>> Members

        public IField<Polynomial<T, TField>> Instance
        {
            get { return Default; }
        }

        public Polynomial<T, TField> Inverse(Polynomial<T, TField> t)
        {
            if (t.Degree != 0)
                throw new ArgumentException();

            return new Polynomial<T, TField>((new TField()).Inverse(t[0].Coeff));
        }

        public Polynomial<T, TField> Divide(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            if (t2.Degree != 0)
                throw new ArgumentException();

            return t1 / t2.CoefficientAt(0);
        }

        public Polynomial<T, TField> Divide(Polynomial<T, TField> t, int n)
        {
            return t / _field.Multiply(_field.One, n);
        }

        #endregion

        #region IRing<Polynomial<T, TField>> Members

        IRing<Polynomial<T, TField>> IRing<Polynomial<T, TField>>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return true; }
        }

        public Polynomial<T, TField> Zero
        {
            get { return Polynomial<T, TField>.Zero; }
        }

        public Polynomial<T, TField> One
        {
            get { return Polynomial<T, TField>.One; }
        }

        public bool IsZero(Polynomial<T, TField> t)
        {
            return t.Degree == 0 && _field.IsZero(t.CoefficientAt(0));
        }

        public bool IsApproxZero(Polynomial<T, TField> t)
        {
            return t.Degree == 0 && _field.IsApproxZero(t.CoefficientAt(0));
        }

        public bool IsOne(Polynomial<T, TField> t)
        {
            return t.Degree == 0 && _field.IsOne(t.CoefficientAt(0));
        }

        public bool IsUnit(Polynomial<T, TField> t)
        {
            return t.Degree == 0 && _field.IsUnit(t.CoefficientAt(0));
        }

        public bool Divides(Polynomial<T, TField> t, Polynomial<T, TField> divisor)
        {
            Polynomial<T, TField> remainder;

            Polynomial<T, TField>.Divide(t, divisor, out remainder);

            return IsZero(remainder);
        }

        public Polynomial<T, TField> Add(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            return t1 + t2;
        }

        public Polynomial<T, TField> Negate(Polynomial<T, TField> t)
        {
            return -t;
        }

        public Polynomial<T, TField> Subtract(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            return t1 - t2;
        }

        public Polynomial<T, TField> Multiply(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            return t1 * t2;
        }

        public Polynomial<T, TField> Multiply(Polynomial<T, TField> t, int n)
        {
            return t * _field.Multiply(_field.One, n);
        }

        public Polynomial<T, TField> Pow(Polynomial<T, TField> t, int n)
        {
            return t.Pow(n);
        }

        #endregion

        #region IEuclideanDomain<Polynomial<T, TField>> Members

        public BigInteger Euclidean(Polynomial<T, TField> t)
        {
            return t.Degree;
        }

        public Polynomial<T, TField> Divide(Polynomial<T, TField> t1, Polynomial<T, TField> t2, out Polynomial<T, TField> r)
        {
            return Polynomial<T, TField>.Divide(t1, t2, out r);
        }

        public Polynomial<T, TField> Gcd(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            return Polynomial<T, TField>.Gcd(t1, t2);
        }

        public Polynomial<T, TField> Gcd(Polynomial<T, TField>[] t, Polynomial<T, TField>[] q)
        {
            if (t.Length == 0)
                throw new ArgumentException();

            Polynomial<T, TField> gcd = t[0];

            for (int i = 1; i < t.Length; i++)
                gcd = Polynomial<T, TField>.Gcd(gcd, t[i]);

            if (q != null)
            {
                for (int i = 0; i < t.Length; i++)
                    q[i] = t[i] / gcd;
            }

            return gcd;
        }

        public Polynomial<T, TField> Lcm(Polynomial<T, TField> t1, Polynomial<T, TField> t2)
        {
            return Polynomial<T, TField>.Lcm(t1, t2);
        }

        public Polynomial<T, TField> Lcm(Polynomial<T, TField>[] t, Polynomial<T, TField>[] q)
        {
            if (t.Length == 0)
                throw new ArgumentException();

            Polynomial<T, TField> lcm = t[0];

            for (int i = 1; i < t.Length; i++)
                lcm = Polynomial<T, TField>.Lcm(lcm, t[i]);

            if (q != null)
            {
                for (int i = 0; i < t.Length; i++)
                    q[i] = lcm / t[i];
            }

            return lcm;
        }

        #endregion
    }
}
