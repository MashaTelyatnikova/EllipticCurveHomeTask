using System;
using System.Numerics;

namespace HomeTask
{
    public class MyBigInteger
    {
        protected BigInteger value;
        protected BigInteger modular;

        public MyBigInteger(BigInteger value, BigInteger modular)
        {
            this.value = value;
            this.modular = modular;
        }

        public virtual MyBigInteger Get(BigInteger val)
        {
            return new MyBigInteger(val, modular);
        }

        public virtual MyBigInteger ModPow(MyBigInteger exp, MyBigInteger m)
        {
            return new MyBigInteger(BigInteger.ModPow(value, exp.value, m.value), modular);
        }
        public virtual MyBigInteger Zero()
        {
            return new MyBigInteger(BigInteger.Zero, modular);
        }

        public virtual MyBigInteger One()
        {
            return new MyBigInteger(BigInteger.One, modular);
        }

        public virtual MyBigInteger Plus(MyBigInteger b)
        {
            return new MyBigInteger(value + b.value, modular);
        }

        public virtual MyBigInteger Mult(MyBigInteger b)
        {
            return new MyBigInteger(value*b.value, modular);
        }

        public virtual MyBigInteger Minus(MyBigInteger b)
        {
            return new MyBigInteger(value - b.value, modular);
        }

        public virtual MyBigInteger Divide(MyBigInteger b)
        {
            return new MyBigInteger(value/b.value, modular);
        }

        public virtual MyBigInteger Mode(MyBigInteger b)
        {
            var x = value%b.value;

            return new MyBigInteger(x < 0 ? x + b.value : x, modular);
        }

        private static MyBigInteger Gcd(MyBigInteger a, MyBigInteger b, ref MyBigInteger x,
            ref MyBigInteger y)
        {
            if (a.value == 0)
            {
                x.value = 0;
                y = a.One();
                return b;
            }

            var x1 = a.Zero();
            var y1 = a.Zero();
            var d = Gcd(b.Mode(a), a, ref x1, ref y1);
            x = y1.Minus(b.Divide(a).Mult(x1));
            y = x1;
            return d;
        }

        public MyBigInteger Pow(MyBigInteger pow)
        {
            var result = this.Clone();
            while (pow.value > 0)
            {
                result = result.Mult(this);
                pow.value--;
            }

            return result;
        }

        public virtual MyBigInteger Clone()
        {
            return new MyBigInteger(value, modular);
        }

        public MyBigInteger Invert()
        {
            var x = this.Zero();
            var y = this.Zero();
            var mod = Get(modular);

            var g = Gcd(this, mod, ref x, ref y);
            if (g.value != 1)
            {
                throw new ArgumentException($"Нет решения");
            }
            return (x.Mode(mod).Plus(mod)).Mode(mod);
        }

        public override bool Equals(object obj)
        {
            var x = (MyBigInteger) obj;
            return x.value == value;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public static MyBigInteger operator +(MyBigInteger a, MyBigInteger b)
        {
            return a.Plus(b);
        }

        public static MyBigInteger operator -(MyBigInteger a, MyBigInteger b)
        {
            return a.Minus(b);
        }

        public static MyBigInteger operator *(MyBigInteger a, MyBigInteger b)
        {
            return a.Mult(b);
        }

        public static MyBigInteger operator /(MyBigInteger a, MyBigInteger b)
        {
            return a.Divide(b);
        }

        public static MyBigInteger operator %(MyBigInteger a, MyBigInteger b)
        {
            return a.Mode(b);
        }

        public static bool operator ==(MyBigInteger a, MyBigInteger b)
        {
            return a.value == b.value;
        }

        public static bool operator !=(MyBigInteger a, MyBigInteger b)
        {
            return a.value != b.value;
        }
    }
}