using System.Numerics;
using EllipticCurveUtils;

namespace HomeTask.EllipticCurve
{
    public class NonSupersSingularEllipticCurve : EllipticCurve
    {
        public NonSupersSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) :
            base(a, b, p, (x, y) => y.Pow(2) + a*x*y - x*y - x.Pow(3) - b*x.Pow(2) - c)
        {
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = first.Y + second.Y;
            var denominator = first.X + second.X;
            var invertedDenominator = denominator.Mode(p).Invert(p);
            var lambda = (numerator*invertedDenominator).Mode(p);

            var x = (lambda.Pow(2) + lambda + denominator + a).Mode(p);
            var y = (lambda*(first.X + x) + x + first.Y).Mode(p);

            return new EllipticCurvePoint(x, y);
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint point)
        {
            var invertedX1 = point.X.Mode(p).Invert(p);
            var lambda = (point.X + point.Y*invertedX1).Mode(p);

            var x = (lambda.Pow(2) + lambda + a).Mode(p);
            var y = (point.X.Pow(2) + (lambda + 1)*x).Mode(p);

            return new EllipticCurvePoint(x, y);
        }
    }
}