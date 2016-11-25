using System.Numerics;

namespace HomeTask.EllipticCurve
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

            var point = (EllipticCurvePoint) obj;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }
    }
}