using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace HomeTask
{
    public static class StringExtensions
    {
        public static BigInteger ToBigInteger(this string str)
        {
            return BigInteger.Parse(str);
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
