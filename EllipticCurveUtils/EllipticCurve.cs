using System;
using System.Drawing;
using System.Numerics;

namespace EllipticCurveUtils
{
    public abstract class EllipticCurve : IEllipticCurve
    {
        protected readonly BigInteger a;
        protected readonly BigInteger b;
        protected readonly BigInteger p;
        protected static readonly EllipticCurvePoint Zero = new EllipticCurvePoint(0, 0);
        protected Func<BigInteger, BigInteger, BigInteger> equation;

        public EllipticCurve(BigInteger a, BigInteger b, BigInteger p)
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

        public abstract EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second);

        public bool Contains(EllipticCurvePoint point)
        {
            return equation(point.X, point.Y).Mode(p) == 0;
        }

        public bool IsSpecial()
        {
            return (4*a*a*a + 27*b*b).Mode(p) == 0;
        }
    }
}