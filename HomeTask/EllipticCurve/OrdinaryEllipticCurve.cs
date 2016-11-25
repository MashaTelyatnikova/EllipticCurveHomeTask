using System.Numerics;

namespace EllipticCurveUtils
{
    public class OrdinaryEllipticCurve : EllipticCurve
    {
        public OrdinaryEllipticCurve(BigInteger a, BigInteger b, BigInteger p) :
            base(a, b, p, (x, y) => y.Pow(2) - x.Pow(3) - x*a - b)
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