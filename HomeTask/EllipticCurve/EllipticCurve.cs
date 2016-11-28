using System;
using System.Drawing;
using System.Numerics;
using EllipticCurveUtils;

namespace HomeTask.EllipticCurve
{
    public abstract class EllipticCurve : IEllipticCurve
    {
        protected readonly BigInteger a;
        protected readonly BigInteger b;
        protected readonly BigInteger p;
        protected static readonly EllipticCurvePoint Zero = new EllipticCurvePoint(0, 0);
        private readonly Func<BigInteger, BigInteger, BigInteger> equation;

        protected EllipticCurve(BigInteger a, BigInteger b, BigInteger p,
            Func<BigInteger, BigInteger, BigInteger> equation)
        {
            this.a = a;
            this.b = b;
            this.p = p;
            this.equation = equation;
        }

        public bool IsNonSpecial()
        {
            return -(4*a*a*a + 27*b*b) != 0;
        }

        public EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            if (Zero.Equals(first))
            {
                return second;
            }

            if (Zero.Equals(second))
            {
                return first;
            }

            return first.Equals(second) ? DoublePoint(first) : AddDifferent(first, second);
        }

        protected abstract EllipticCurvePoint DoublePoint(EllipticCurvePoint point);
        protected abstract EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second);

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