using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Curves
{
    public class ElementWOrientationCurve
        : BaseFloatCurve
    {
        private BaseFloatCurve _curveW;
        private BaseFloatCurve _curveX;
        private BaseFloatCurve _curveY;
        private BaseFloatCurve _curveZ;
        private Transform _transform;

        [UsedImplicitly] private void OnEnable()
        {
            _curveW = this;
            _curveX = GetComponent<ElementXOrientationCurve>();
            _curveY = GetComponent<ElementYOrientationCurve>();
            _curveZ = GetComponent<ElementZOrientationCurve>();
        }

        [UsedImplicitly] protected override void Update()
        {
            base.Update();

            var w = _curveW.Value;
            var x = _curveX.Value;
            var y = _curveY.Value;
            var z = _curveZ.Value;

            _transform ??= transform;
            _transform.rotation = new Quaternion(x, y, z, w).normalized;
        }
    }

    public class ElementXOrientationCurve
        : BaseFloatCurve
    {
    }

    public class ElementYOrientationCurve
        : BaseFloatCurve
    {
    }

    public class ElementZOrientationCurve
        : BaseFloatCurve
    {
    }
}
