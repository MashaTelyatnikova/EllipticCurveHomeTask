using System.Numerics;

namespace EllipticCurveUtils
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        private readonly BigInteger c;

        public SuperSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) : base(a, b, p)
        {
            this.c = c;
            this.equation = (x, y) => y.Pow(2) + y*a - x.Pow(3) - x*b - c;
        }

        public override EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (first.Equals(second))
            {
                BigInteger numerator = first.X.Pow(4) + b.Pow(2);
                BigInteger denominator = a*a;
                BigInteger invertedDemominator = denominator.Invert(p);

                var x = (numerator*invertedDemominator).Mode(p);
                var y =
                    (first.Y + a + (first.X.Pow(2) + b)*(a.Invert(p))*(first.X + x))
                        .Mode(p);

                return new EllipticCurvePoint(x, y);
            }
            else
            {
                BigInteger numerator = first.Y + second.Y;
                BigInteger denominator = first.X + second.X;
                BigInteger invertedDenominator = denominator.Invert(p);
                var lambda = (numerator*invertedDenominator).Mode(p);

                var x = (lambda*lambda + first.X + second.X).Mode(p);
                var y = (first.Y + a + lambda*(first.X + x)).Mode(p);

                return new EllipticCurvePoint(x, y);
            }
        }
    }
}