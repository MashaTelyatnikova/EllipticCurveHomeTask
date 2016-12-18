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
    }
}