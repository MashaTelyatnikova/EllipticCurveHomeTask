﻿using System;
using System.Numerics;

namespace EllipticCurveUtils
{
    public class NonSupersSingularEllipticCurve : EllipticCurve
    {
        private readonly BigInteger c;
        public NonSupersSingularEllipticCurve(BigInteger a, BigInteger b, BigInteger c, BigInteger p) : 
            base(a, b, p, (x, y) => x.Pow(2))
        {
            this.c = c;
        }

        public override EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second)
        {
            throw new NotImplementedException();
        }
    }
}