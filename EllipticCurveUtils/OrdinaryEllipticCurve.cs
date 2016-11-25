using System.Numerics;

namespace EllipticCurveUtils
{
    public class OrdinaryEllipticCurve : EllipticCurve
    {
        public OrdinaryEllipticCurve(BigInteger a, BigInteger b, BigInteger p) : 
            base(a, b, p, (x, y) => y.Pow(2) - x.Pow(3) - x * a - b)
        {
        }

        public override EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (Zero.Equals(first))
            {
                return second;
            }

            if (Zero.Equals(second))
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

            return new EllipticCurvePoint(x, y);
        }
    }
}