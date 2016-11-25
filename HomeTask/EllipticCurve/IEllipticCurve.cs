using EllipticCurveUtils;

namespace HomeTask.EllipticCurve
{
    public interface IEllipticCurve
    {
        EllipticCurvePoint Add(EllipticCurvePoint first, EllipticCurvePoint second);
        bool Contains(EllipticCurvePoint point);
        bool IsSpecial();
    }
}
