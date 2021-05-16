namespace Assets.Scripts.Curves
{
    public class GunTurretElevationCurve
        : BaseFloatCurve
    {
        public int Index { get; set; }
        public float Elevation => Value;
    }
}