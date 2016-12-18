namespace HomeTask.EllipticCurve
{
    public class NonSupersSingularEllipticCurve : EllipticCurve
    {
        public NonSupersSingularEllipticCurve(MyBigInteger a, MyBigInteger b, MyBigInteger c, MyBigInteger p) :
            base(
                a, b, p,
                (x, y) => y*y + a*x*y - x*x*x - b*x*x - c)
        {
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = first.Y + second.Y;
            var denominator = first.X + second.X;
            var invertedDenominator = denominator.Invert();
            var lambda = numerator*invertedDenominator;

            var x = first.X + second.X + b + lambda*lambda + a*lambda;
            var y = lambda*x + lambda*first.X + first.Y;

            return new EllipticCurvePoint(x, a*x + y);
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint point)
        {
            var invertedX = (a*point.X).Invert();
            var lambda = (point.X*point.X + a*point.Y)*invertedX;

            var x = point.X + point.X + b + lambda * lambda + a * lambda;
            var y = lambda * x + lambda * point.X + point.Y;

            return new EllipticCurvePoint(x, a * x + y);
        }
    }
}