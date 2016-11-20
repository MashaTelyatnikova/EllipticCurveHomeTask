using System.Drawing;
using System.Numerics;

namespace EllipticCurveUtils
{
    public class EllipticCurve
    {
        protected readonly int a;
        protected readonly int b;
        protected readonly int p;
        protected static readonly Point Zero = new Point(0, 0);

        public EllipticCurve(int a, int b, int p)
        {
            this.a = a;
            this.b = b;
            this.p = p;
        }

        public bool IsNonSpecial()
        {
            return -(4*a*a*a + 27*b*b) != 0;
        }

        public virtual bool Contains(Point point)
        {
            BigInteger ySq = point.Y*point.Y;
            BigInteger res = point.X*point.X*point.X + a*point.X + b;

            return ySq.Mode(p) == res.Mode(p);
        }

        public virtual Point Add(Point first, Point second)
        {
            if (first == Zero)
            {
                return second;
            }

            if (second == Zero)
            {
                return first;
            }
            BigInteger numerator = 0;
            BigInteger denominator = 0;
            if (first.Equals(second))
            {
                numerator = 3*first.X*first.X + a;
                denominator = 2*first.Y;
            }
            else
            {
                numerator = second.Y - first.Y;
                denominator = second.X - first.X;
            }

            denominator = denominator.Mode(p);
            var invertedToDenominator = denominator.Invert(p);
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda - first.X - second.X).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new Point(x, y);
        }
    }
}