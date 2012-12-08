using System;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System.Text;

namespace Wj.Math
{
    public class IntegerField : IField<BigInteger>, IEuclideanDomain<BigInteger>
    {
        public static readonly IntegerField Default = new IntegerField();

        #region IField<BigInteger> Members

        public IField<BigInteger> Instance
        {
            get { return Default; }
        }

        public BigInteger Inverse(BigInteger t)
        {
            throw new NotSupportedException();
        }

        public BigInteger Divide(BigInteger t1, BigInteger t2)
        {
            throw new NotSupportedException();
        }

        public BigInteger Divide(BigInteger t, int n)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IRing<BigInteger> Members

        IRing<BigInteger> IRing<BigInteger>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return false; }
        }

        public BigInteger Zero
        {
            get { return BigInteger.Zero; }
        }

        public BigInteger One
        {
            get { return BigInteger.One; }
        }

        public bool IsZero(BigInteger t)
        {
            return t == BigInteger.Zero;
        }

        public bool IsOne(BigInteger t)
        {
            return t == BigInteger.One;
        }

        public bool IsApproxZero(BigInteger t)
        {
            return IsZero(t);
        }

        public bool IsUnit(BigInteger t)
        {
            return t == BigInteger.One;
        }

        public bool Divides(BigInteger t, BigInteger divisor)
        {
            return BigInteger.Remainder(t, divisor) == 0;
        }

        public BigInteger Add(BigInteger t1, BigInteger t2)
        {
            return t1 + t2;
        }

        public BigInteger Negate(BigInteger t)
        {
            return -t;
        }

        public BigInteger Subtract(BigInteger t1, BigInteger t2)
        {
            return t1 - t2;
        }

        public BigInteger Multiply(BigInteger t1, BigInteger t2)
        {
            return t1 * t2;
        }

        public BigInteger Multiply(BigInteger t, int n)
        {
            return t * n;
        }

        public BigInteger Pow(BigInteger t, int n)
        {
            return BigInteger.Pow(t, n);
        }

        #endregion

        #region IEuclideanDomain<BigInteger> Members

        public BigInteger Euclidean(BigInteger t)
        {
            return BigInteger.Abs(t);
        }

        public BigInteger Divide(BigInteger t1, BigInteger t2, out BigInteger r)
        {
            return BigInteger.DivRem(t1, t2, out r);
        }

        public BigInteger Gcd(BigInteger t1, BigInteger t2)
        {
            return BigInteger.GreatestCommonDivisor(t1, t2);
        }

        public BigInteger Gcd(BigInteger[] t, BigInteger[] q)
        {
            if (t.Length == 0)
                throw new ArgumentException();

            BigInteger gcd = t[0];

            for (int i = 1; i < t.Length; i++)
                gcd = BigInteger.GreatestCommonDivisor(gcd, t[i]);

            if (q != null)
            {
                for (int i = 0; i < t.Length; i++)
                    q[i] = t[i] / gcd;
            }

            return gcd;
        }

        public BigInteger Lcm(BigInteger t1, BigInteger t2)
        {
            return t1 * t2 / BigInteger.GreatestCommonDivisor(t1, t2);
        }

        public BigInteger Lcm(BigInteger[] t, BigInteger[] q)
        {
            if (t.Length == 0)
                throw new ArgumentException();

            BigInteger lcm = t[0];

            for (int i = 1; i < t.Length; i++)
                lcm = this.Lcm(lcm, t[i]);

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
