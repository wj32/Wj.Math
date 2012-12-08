using System;
using System.Collections.Generic;
using System.Text;

namespace Wj.Math
{
    public class RealField : IField<double>
    {
        public static readonly double Epsilon = 1E-14;
        public static readonly int EpsilonDigits = 14;
        public static readonly double E = System.Math.E;
        public static readonly double Phi = (1 + System.Math.Sqrt(5)) / 2;
        public static readonly double Pi = System.Math.PI;
        internal static readonly double DoublePi = System.Math.PI * 2;
        internal static readonly double InvE = 1.0 / System.Math.E;
        internal static readonly double Sqrt2 = System.Math.Sqrt(2);
        internal static readonly double Sqrt5 = System.Math.Sqrt(5);

        public static readonly RealField Default = new RealField();

        #region IField<double> Members

        public IField<double> Instance
        {
            get { return Default; }
        }

        public double Inverse(double t)
        {
            return 1 / t;
        }

        public double Divide(double t1, double t2)
        {
            return t1 / t2;
        }

        public double Divide(double t, int n)
        {
            return t / n;
        }

        #endregion

        #region IRing<double> Members

        IRing<double> IRing<double>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return false; }
        }

        public double Zero
        {
            get { return 0.0; }
        }

        public double One
        {
            get { return 1.0; }
        }

        public bool IsZero(double t)
        {
            return t == 0.0;
        }

        public bool IsApproxZero(double t)
        {
            return System.Math.Abs(t) < Complex.PracticalEpsilon.Re;
        }

        public bool IsOne(double t)
        {
            return t == 1.0;
        }

        public bool IsUnit(double t)
        {
            return t != 0.0;
        }

        public bool Divides(double t, double divisor)
        {
            return true;
        }

        public double Add(double t1, double t2)
        {
            return t1 + t2;
        }

        public double Negate(double t)
        {
            return -t;
        }

        public double Subtract(double t1, double t2)
        {
            return t1 - t2;
        }

        public double Multiply(double t1, double t2)
        {
            return t1 * t2;
        }

        public double Multiply(double t, int n)
        {
            return t * n;
        }

        public double Pow(double t, int n)
        {
            return System.Math.Pow(t, n);
        }

        #endregion
    }
}
