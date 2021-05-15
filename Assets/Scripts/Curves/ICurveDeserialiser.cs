using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Curves
{
    public interface ICurveDeserialiser
    {
        void LoadCurve([NotNull] JToken curve);
    }
}
