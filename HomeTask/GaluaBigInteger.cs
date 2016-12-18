using System.Numerics;
using EllipticCurveUtils;

namespace HomeTask
{
    public class GaluaBigInteger : MyBigInteger
    {
        private BigInteger n;

        public GaluaBigInteger(BigInteger value, BigInteger modular, BigInteger n) : base(value, modular)
        {
            this.modular = modular;
            this.n = n;
        }

        public override MyBigInteger Get(BigInteger val)
        {
            return new GaluaBigInteger(val, modular, n);
        }

        public override MyBigInteger Clone()
        {
            return new GaluaBigInteger(value, modular, n);
        }

        public override MyBigInteger Zero()
        {
            return new GaluaBigInteger(BigInteger.Zero, modular, n);
        }

        public override MyBigInteger One()
        {
            return new GaluaBigInteger(BigInteger.One, modular, n);
        }

        public override MyBigInteger Plus(MyBigInteger y1)
        {
            var y = (GaluaBigInteger)y1;
            return new GaluaBigInteger(value ^ y.value, modular, n);
        }

        public override MyBigInteger Minus(MyBigInteger y1)
        {
            var y = (GaluaBigInteger)y1;
            return new GaluaBigInteger(value ^ y.value, modular, n);
        }

        public override MyBigInteger Mult(MyBigInteger y1)
        {
            var y = (GaluaBigInteger)y1;
            var p = BigInteger.Zero;
            var a = value;
            var b = y.value;

            while (b != 0)
            {
                if ((b & 1) != 0)
                    p ^= a;

                if ((a & n) != 0)
                    a = (a << 1) ^ modular;
                else
                    a <<= 1;
                b >>= 1;
            }

            return new GaluaBigInteger(p, modular, n);
        }

        public override MyBigInteger Divide(MyBigInteger y1)
        {
            var y = (GaluaBigInteger)y1;
            var p = value;
            var q = y.value;
            var res = BigInteger.Zero;

            while (p.Degree() >= q.Degree())
            {
                var i = BigInteger.One;
                while (i <= q)
                {
                    i <<= 1;
                }

                i >>= 1;
                var k = BigInteger.One;
                while (i * k <= p)
                {
                    k <<= 1;
                }

                k >>= 1;
                p = p ^ (q * k);
                res += k;
            }

            return new GaluaBigInteger(res, modular, n);
        }

        public override MyBigInteger ModPow(MyBigInteger exp, MyBigInteger m)
        {
            return this.Pow(exp);
        }
        public override MyBigInteger Mode(MyBigInteger y1)
        {
            var y = (GaluaBigInteger)y1;
            var p = value;
            var q = y.value;
            while (p.Degree() >= q.Degree())
            {
                var i = BigInteger.One;
                while (i <= q)
                {
                    i <<= 1;
                }

                i >>= 1;
                var k = 0;
                while (i <= p)
                {
                    i <<= 1;
                    ++k;
                }

                --k;
                p = p ^ (q << k);
            }

            return new GaluaBigInteger(p, modular, n);
        }
    }
}
