using System;
using System.Collections;
using System.Collections.Generic;
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

            return new BigInteger(str.ToBytes().ToArray());
        }

        public static IEnumerable<byte> ToBytes(this string str)
        {
            
            str = str.Replace(" ", "");
            for (var i = 0; i < str.Length; i += 2)
            {
                var sub = $"{str[i]}{str[i + 1]}";
                yield return Convert.ToByte(sub, 16);
            }
        }

    }
}
