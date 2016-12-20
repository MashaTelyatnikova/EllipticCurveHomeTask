using System;
using System.Numerics;

namespace EllipticCurveUtils
{
    public static class IntegerExtensions
    {
        public static int Degree(this BigInteger a)
        {
            var h = 0;

            while (a != 0)
            {
                a >>= 1;
                h++;
            }
            return h - 1;
        }

        public static BigInteger Pow(this BigInteger a, BigInteger n)
        {
            
            var p = n/int.MaxValue;
            while (p != 0)
            {
                
                a = a << int.MaxValue;
                p--;
            }
            var c = (int) n%int.MaxValue;
            a = a << c;
            a = a >> 1;
            return a;
        }
    }
}