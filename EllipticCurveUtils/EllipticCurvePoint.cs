using System.Numerics;

namespace EllipticCurveUtils
{
    public class EllipticCurvePoint
    {
        public BigInteger X { get; }
        public BigInteger Y { get; }

        public EllipticCurvePoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            EllipticCurvePoint point = (EllipticCurvePoint) obj;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }
    }
}