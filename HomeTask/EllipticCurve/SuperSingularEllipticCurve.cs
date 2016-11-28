﻿using System.Numerics;
using EllipticCurveUtils;

namespace HomeTask.EllipticCurve
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        public SuperSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) :
            base(a, b, p, (x, y) => (BigInteger.Pow(y, 2) + y*a).Mode(p) - (BigInteger.Pow(x, 3) + x*b + c).Mode(p))
        {
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var numerator = BigInteger.Pow(first.X, 4) + BigInteger.Pow(b, 2);
            var denominator = a*a;
            var invertedDemominator = denominator.Invert(p);

            var x = (numerator*invertedDemominator).Mode(p);
            var y =
                (first.Y + a + (BigInteger.Pow(first.X, 2) + b)*(a.Invert(p))*(first.X + x))
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