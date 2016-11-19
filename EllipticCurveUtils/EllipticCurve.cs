using System.Drawing;

namespace EllipticCurveUtils
{
    public class EllipticCurve
    {
        private readonly int a;
        private readonly int b;
        private readonly int p;

        public EllipticCurve(int a, int b, int p)
        {
            this.a = a;
            this.b = b;
            this.p = p;
        }

        public Point Add(Point first, Point second)
        {
            var numerator = 0;
            var denominator = 0;
            if (first.Equals(second))
            {
                numerator = 3*first.X*first.X + a;
                denominator = (2*first.Y).Mode(p);
            }
            else
            {
                numerator = second.Y - first.Y;
                denominator = (second.X - first.X).Mode(p);
            }

            var invertedToDenominator = denominator.Invert(p);
            var lambda = numerator*invertedToDenominator;

            var x = (lambda*lambda - first.X - second.X).Mode(p);
            var y = (lambda*(first.X - x) - first.Y).Mode(p);

            return new Point(x, y);
        }
    }
}