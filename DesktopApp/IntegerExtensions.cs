using System;

namespace DesktopApp
{
    public static class IntegerExtensions
    {
        public static int Mode(this int x, int p)
        {
            var res = x%p;
            return res < 0 ? res + p : res;
        }

        public static int Invert(this int a, int m)
        {
            var x = 0;
            var y = 0;
            var g = Gcd(a, m, ref x, ref y);
            if (g != 1)
            {
                throw new ArgumentException($"Нет решения для a = {a} и m = {m}");
            }
            return (x%m + m)%m;
        }

        private static int Gcd(int a, int b, ref int x, ref int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            var x1 = 0;
            var y1 = 0;
            var d = Gcd(b%a, a, ref x1, ref y1);
            x = y1 - (b/a)*x1;
            y = x1;
            return d;
        }
    }
}