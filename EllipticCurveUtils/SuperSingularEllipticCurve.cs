using System.Drawing;
using System.Numerics;

namespace EllipticCurveUtils
{
    public class SuperSingularEllipticCurve : EllipticCurve
    {
        private readonly int c;
        public SuperSingularEllipticCurve(int a, int b, int c, int p) : base(a, b, p)
        {
            this.c = c;
        }

        public override bool Contains(Point point)
        {
            BigInteger left = point.Y*point.Y + a*point.Y;
            BigInteger right = point.X*point.X*point.X + b*point.X + c;

            return left.Mode(p) == right.Mode(p);
        }

        public override Point Add(Point first, Point second)
        {
            return base.Add(first, second);
        }
    }
}
