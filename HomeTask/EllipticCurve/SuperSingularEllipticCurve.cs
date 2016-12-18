namespace HomeTask.EllipticCurve
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        public SuperSingularEllipticCurve(MyBigInteger a, MyBigInteger b, MyBigInteger c, MyBigInteger p) :
            base(a, b, p, (x, y) => y*y + y*a - x*x - x*b - c)
        {
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var lambda = (first.X*first.X + b)*a.Invert();

            var x = lambda*lambda + first.X + first.X;
            var y = lambda*x + lambda*first.X + first.Y;

            return new EllipticCurvePoint(x, a + y);
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = first.Y + second.Y;
            var denominator = first.X + second.X;
            var invertedDenominator = denominator.Invert();
            var lambda = numerator*invertedDenominator;

            var x = lambda*lambda + first.X + second.X;
            var y = lambda*x + lambda*first.X + first.Y;

            return new EllipticCurvePoint(x, a + y);
        }
    }
}