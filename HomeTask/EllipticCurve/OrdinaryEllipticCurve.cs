namespace HomeTask.EllipticCurve
{
    public class OrdinaryEllipticCurve : EllipticCurve
    {
        public OrdinaryEllipticCurve(MyBigInteger a, MyBigInteger b, MyBigInteger p) :
            base(a, b, p, (x, y) => y*y - x*x*x - a*x - b)
        {
        }

        protected override EllipticCurvePoint DoublePoint(EllipticCurvePoint first)
        {
            var numerator = a.Get(3)*first.X*first.X + a;
            var denominator = (a.Get(2) * first.Y)%p;
            
            var invertedToDenominator = denominator.Invert();
            var lambda = (numerator*invertedToDenominator)%p;

            var x = (lambda*lambda - first.X - first.X)%p;
            var y = (lambda*(first.X - x) - first.Y)%p;

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

        protected override bool AreDifferent(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            return first.X != second.X;
        }

        protected override bool AreOpposite(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            return second.Equals(new EllipticCurvePoint(first.X, (first.X.Get(-1)*first.Y)%p));
        }
    }
}