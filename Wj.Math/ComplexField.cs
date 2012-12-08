using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class ComplexField : IField<Complex>
    {
        public static readonly ComplexField Default = new ComplexField();

        #region IField<Complex> Members

        public IField<Complex> Instance
        {
            get { return Default; }
        }

        public Complex Inverse(Complex t)
        {
            return Complex.Inv(t);
        }

        public Complex Divide(Complex t1, Complex t2)
        {
            return t1 / t2;
        }

        public Complex Divide(Complex t, int n)
        {
            return t / n;
        }

        #endregion

        #region IRing<Complex> Members

        IRing<Complex> IRing<Complex>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return false; }
        }

        public Complex Zero
        {
            get { return Complex.Zero; }
        }

        public Complex One
        {
            get { return Complex.One; }
        }

        public bool IsZero(Complex t)
        {
            return t == Complex.Zero;
        }

        public bool IsApproxZero(Complex t)
        {
            return t.ApproxEquals(Complex.Zero);
        }

        public bool IsOne(Complex t)
        {
            return t == Complex.One;
        }

        public bool IsUnit(Complex t)
        {
            return t != Complex.Zero;
        }

        public bool Divides(Complex t, Complex divisor)
        {
            return true;
        }

        public Complex Add(Complex t1, Complex t2)
        {
            return t1 + t2;
        }

        public Complex Negate(Complex t)
        {
            return -t;
        }

        public Complex Subtract(Complex t1, Complex t2)
        {
            return t1 - t2;
        }

        public Complex Multiply(Complex t1, Complex t2)
        {
            return t1 * t2;
        }

        public Complex Multiply(Complex t, int n)
        {
            return t * n;
        }

        public Complex Pow(Complex t, int n)
        {
            return Complex.Pow(t, n);
        }

        #endregion
    }
}
