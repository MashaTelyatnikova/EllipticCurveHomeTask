namespace HomeTask.EllipticCurve
{
    public class OrdinaryEllipticCurve : EllipticCurve
    {
        public OrdinaryEllipticCurve(MyBigInteger a, MyBigInteger b, MyBigInteger p) :
            base(a, b, p, (x, y) => y.ModPow(y.Get(2), p) - (x.Pow(x.Get(2)) + x*a + b).Mode(p))
        {
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var numerator = a.Get(3)*first.X*first.X + a;
            var denominator = (a.Get(2) * first.Y).Mode(p);
            
            var invertedToDenominator = denominator.Invert();
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new EllipticCurvePoint(x, y);
        }

        protected override EllipticCurvePoint AddDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            var numerator = second.Y - first.Y; ;
            var denominator = (second.X - first.X).Mode(p);
            var invertedToDenominator = denominator.Invert();
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda - first.X - second.X).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new EllipticCurvePoint(x, y);
        }
    }
}