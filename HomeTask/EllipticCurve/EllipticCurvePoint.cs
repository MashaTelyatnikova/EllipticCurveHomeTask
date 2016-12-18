namespace HomeTask.EllipticCurve
{
    public class EllipticCurvePoint
    {
        public MyBigInteger X { get; }
        public MyBigInteger Y { get; }

        public EllipticCurvePoint(MyBigInteger x, MyBigInteger y)
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
            return point.X.Equals(X) && point.Y.Equals(Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }
    }
}