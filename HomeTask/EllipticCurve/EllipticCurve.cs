using System;

namespace HomeTask.EllipticCurve
{
    public abstract class EllipticCurve : IEllipticCurve
    {
        protected readonly MyBigInteger a;
        protected readonly MyBigInteger b;
        protected readonly MyBigInteger p;
        public EllipticCurvePoint Zero { get; }
        private readonly Func<MyBigInteger, MyBigInteger, MyBigInteger> equation;

        protected EllipticCurve(MyBigInteger a, MyBigInteger b, MyBigInteger p,
            Func<MyBigInteger, MyBigInteger, MyBigInteger> equation)
        {
            this.a = a;
            this.b = b;
            this.p = p;
            this.Zero = new EllipticCurvePoint(a.Zero(), b.Zero());
            this.equation = equation;
        }

        public bool IsNonSpecial()
        {
            return a.Get(-1)*(a.Get(4)*a*a*a + a.Get(27)*b*b) != a.Zero();
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
            return equation(point.X, point.Y) % p == point.X.Zero();
        }
    }
}