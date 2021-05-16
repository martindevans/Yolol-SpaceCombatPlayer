namespace Assets.Scripts.Curves
{
    public class GunTurretBearingCurve
        : BaseFloatCurve
    {
        public int Index { get; set; }
        public float Bearing => Value;
    }
}