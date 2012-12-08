using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class RationalField : IField<Rational>
    {
        public static readonly RationalField Default = new RationalField();

        #region IField<Rational> Members

        public IField<Rational> Instance
        {
            get { return Default; }
        }

        public Rational Inverse(Rational t)
        {
            return Rational.Inv(t);
        }

        public Rational Divide(Rational t1, Rational t2)
        {
            return t1 / t2;
        }

        public Rational Divide(Rational t, int n)
        {
            return t / n;
        }

        #endregion

        #region IRing<Rational> Members

        IRing<Rational> IRing<Rational>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return false; }
        }

        public Rational Zero
        {
            get { return Rational.Zero; }
        }

        public Rational One
        {
            get { return Rational.One; }
        }

        public bool IsZero(Rational t)
        {
            return t == Rational.Zero;
        }

        public bool IsOne(Rational t)
        {
            return t == Rational.One;
        }

        public bool IsApproxZero(Rational t)
        {
            return IsZero(t);
        }

        public bool IsUnit(Rational t)
        {
            return !IsZero(t);
        }

        public bool Divides(Rational t, Rational divisor)
        {
            return true;
        }

        public Rational Add(Rational t1, Rational t2)
        {
            return t1 + t2;
        }

        public Rational Negate(Rational t)
        {
            return -t;
        }

        public Rational Subtract(Rational t1, Rational t2)
        {
            return t1 - t2;
        }

        public Rational Multiply(Rational t1, Rational t2)
        {
            return t1 * t2;
        }

        public Rational Multiply(Rational t, int n)
        {
            return t * n;
        }

        public Rational Pow(Rational t, int n)
        {
            return Rational.Pow(t, n);
        }

        #endregion
    }
}
