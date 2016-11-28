using System.Numerics;
using EllipticCurveUtils;

namespace HomeTask.EllipticCurve
{
    public class OrdinaryEllipticCurve : EllipticCurve
    {
        public OrdinaryEllipticCurve(BigInteger a, BigInteger b, BigInteger p) :
            base(a, b, p, (x, y) => BigInteger.ModPow(y, 2, p) - (BigInteger.Pow(x, 2) + x*a + b).Mode(p))
        {
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var numerator = 3*first.X*first.X + a;
            var denominator = (2*first.Y).Mode(p);
            
            var invertedToDenominator = denominator.Invert(p);
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new EllipticCurvePoint(x, y);
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = second.Y - first.Y; ;
            var denominator = (second.X - first.X).Mode(p);
            var invertedToDenominator = denominator.Invert(p);
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda - first.X - second.X).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new EllipticCurvePoint(x, y);
        }
    }
}