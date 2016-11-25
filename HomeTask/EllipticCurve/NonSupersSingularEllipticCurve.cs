using System;
using System.Numerics;

namespace EllipticCurveUtils
{
    public class NonSupersSingularEllipticCurve : EllipticCurve
    {
        private readonly BigInteger c;
        public NonSupersSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) : 
            base(a, b, p, (x, y) => y.Pow(2) - x*y - x.Pow(3) - a*x.Pow(2) - b)
        {
            this.c = c;
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            throw new NotImplementedException();
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint firstd)
        {
            throw new NotImplementedException();
        }
    }
}
