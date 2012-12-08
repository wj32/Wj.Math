using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public class ModuloField<TPrime> : IField<int> where TPrime : TIntegerType, new()
    {
        public static readonly ModuloField<TPrime> Default = new ModuloField<TPrime>();

        private static int _prime;

        static ModuloField()
        {
            _prime = (new TPrime()).GetValue();

            if (!Discrete.IsPrime(_prime))
                throw new ArgumentException("TPrime must specify a prime number.");
        }

        public static int Element(int n)
        {
            return (_prime + (n % _prime)) % _prime;
        }

        #region IField<int> Members

        public IField<int> Instance
        {
            get { return Default; }
        }

        public int Inverse(int t)
        {
            if (t == 0)
                throw new ArgumentException();

            var pair = IntegerField.Default.ExtendedEuclidean((System.Numerics.BigInteger)t, _prime);

            return Element((int)pair.First);
        }

        public int Divide(int t1, int t2)
        {
            return (t1 * Inverse(t2)) % _prime;
        }

        #endregion

        #region IRing<int> Members

        IRing<int> IRing<int>.Instance
        {
            get { return Default; }
        }

        public bool NeedsInitialize
        {
            get { return false; }
        }

        public int Zero
        {
            get { return 0; }
        }

        public int One
        {
            get { return 1; }
        }

        public bool IsZero(int t)
        {
            return t == 0;
        }

        public bool IsOne(int t)
        {
            return t == 1;
        }

        public bool IsApproxZero(int t)
        {
            return t == 0;
        }

        public bool IsUnit(int t)
        {
            return t != 0;
        }

        public bool Divides(int t, int divisor)
        {
            return true;
        }

        public int Add(int t1, int t2)
        {
            return (t1 + t2) % _prime;
        }

        public int Negate(int t)
        {
            return (_prime - t) % _prime;
        }

        public int Subtract(int t1, int t2)
        {
            return (t1 - t2 + _prime) % _prime;
        }

        public int Multiply(int t1, int t2)
        {
            return (t1 * t2) % _prime;
        }

        public int Pow(int t, int n)
        {
            return this.DefaultPow(t, n);
        }

        #endregion
    }
}
