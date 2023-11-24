using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class ElementXRadarTargetCurve
        : BaseFloatCurve, IRadarTargetCurve
    {
        private BaseFloatCurve _curveX;
        private BaseFloatCurve _curveY;
        private BaseFloatCurve _curveZ;

        public Vector3 Position { get; private set; }

        [UsedImplicitly]
        private void OnEnable()
        {
            _curveX = this;
            _curveY = GetComponent<ElementYRadarTargetCurve>();
            _curveZ = GetComponent<ElementZRadarTargetCurve>();
        }

        [UsedImplicitly]
        protected override void Update()
        {
            base.Update();

            var x = _curveX.Value;
            var y = _curveY.Value;
            var z = _curveZ.Value;

            Position = new Vector3(x, y, z);
        }
    }

    public class ElementYRadarTargetCurve
        : BaseFloatCurve
    {
    }

    public class ElementZRadarTargetCurve
        : BaseFloatCurve
    {
    }
}