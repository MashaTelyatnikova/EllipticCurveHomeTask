using System.Numerics;

namespace EllipticCurveUtils
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        private readonly BigInteger c;

        public SuperSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) :
            base(a, b, p, (x, y) => y.Pow(2) + y*a - x.Pow(3) - x*b - c)
        {
            this.c = c;
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var numerator = first.X.Pow(4) + b.Pow(2);
            var denominator = a*a;
            var invertedDemominator = denominator.Invert(p);

            var x = (numerator*invertedDemominator).Mode(p);
            var y =
                (first.Y + a + (first.X.Pow(2) + b)*(a.Invert(p))*(first.X + x))
                    .Mode(p);

            return new EllipticCurvePoint(x, y);
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = first.Y + second.Y;
            var denominator = first.X + second.X;
            var invertedDenominator = denominator.Invert(p);
            var lambda = (numerator*invertedDenominator).Mode(p);

            var x = (lambda*lambda + first.X + second.X).Mode(p);
            var y = (first.Y + a + lambda*(first.X + x)).Mode(p);

            return new EllipticCurvePoint(x, y);
        }
    }
}