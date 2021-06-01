using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class ElementXRadarDirectionCurve
        : BaseFloatCurve, IRadarDirectionCurve
    {
        private BaseFloatCurve _curveX;
        private BaseFloatCurve _curveY;
        private BaseFloatCurve _curveZ;

        public Vector3 Direction { get; private set; }

        [UsedImplicitly] private void OnEnable()
        {
            _curveX = this;
            _curveY = GetComponent<ElementYRadarDirectionCurve>();
            _curveZ = GetComponent<ElementZRadarDirectionCurve>();
        }

        [UsedImplicitly] protected override void Update()
        {
            base.Update();

            var x = _curveX.Value;
            var y = _curveY.Value;
            var z = _curveZ.Value;

            Direction = Vector3.Normalize(new Vector3(x, y, z));
        }
    }

    public class ElementYRadarDirectionCurve
        : BaseFloatCurve
    {
    }

    public class ElementZRadarDirectionCurve
        : BaseFloatCurve
    {
    }
}
