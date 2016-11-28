using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace HomeTask
{
    public static class StringExtensions
    {
        public static BigInteger ToBigInteger(this string str)
        {
            BigInteger res;
            if (BigInteger.TryParse(str, out res))
            {
                return res;
            }

            return BigInteger.Parse(str.ToLower().Substring(2), NumberStyles.AllowHexSpecifier);
        }
    }
}
