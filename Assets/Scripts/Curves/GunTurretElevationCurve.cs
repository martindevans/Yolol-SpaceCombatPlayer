using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Curves
{
    public class GunTurretElevationCurve
        : BaseFloatCurve
    {
        public int Index { get; set; }
        public float Elevation => Value;

        public void LoadCurve([NotNull] JToken curve, int index)
        {
            Index = index;

            LoadCurve(curve);
        }
    }
}