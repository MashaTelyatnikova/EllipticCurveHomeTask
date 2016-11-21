using System.Drawing;
using System.Numerics;

namespace EllipticCurveUtils
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        private readonly int c;
        public SuperSingularEllipticCurve(int a, int b, int c, int p) : base(a, b, p)
        {
            this.c = c;
        }

        public override bool Contains(Point point)
        {
            BigInteger left = point.Y*point.Y + a*point.Y;
            BigInteger right = point.X*point.X*point.X + b*point.X + c;

            return left.Mode(p) == right.Mode(p);
        }

        public override Point Add(Point first, Point second)
        {
            if (first.Equals(second))
            {
                BigInteger numerator = new BigInteger(first.X).Pow(4) + new BigInteger(b).Pow(2);
                BigInteger denominator = a*a;
                BigInteger invertedDemominator = denominator.Invert(p);

                var x = (numerator*invertedDemominator).Mode(p);
                var y = (new BigInteger(first.Y) + new BigInteger(a) + (new BigInteger(first.X).Pow(2) + b)*(new BigInteger(a).Invert(p))*(first.X + x)).Mode(p);

                return new Point(x, y);
            }
            else
            {
                BigInteger numerator = first.Y + second.Y;
                BigInteger denominator = first.X + second.X;
                BigInteger invertedDenominator = denominator.Invert(p);
                var lambda = (numerator*invertedDenominator).Mode(p);

                var x = new BigInteger(lambda*lambda + first.X + second.X).Mode(p);
                var y = new BigInteger(first.Y + a + lambda*(first.X + x)).Mode(p);

                return new Point(x, y);
            }
        }
    }
}
