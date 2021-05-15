using Assets.Scripts.Curves;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class RadarTargetIndicator
        : ImmediateModeShapeDrawer
    {
        private RadarTargetCurve _targetCurve;
        private Transform _transform;

        public Vector3 FakeTarget;

        public override void DrawShapes(Camera cam)
        {
            _targetCurve ??= GetComponentInParent<RadarTargetCurve>();
            var target = _targetCurve?.Value ?? Vector3.zero;

            if (!Application.isPlaying)
                target = FakeTarget;

            if (target.sqrMagnitude < 10)
                return;

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.LineThickness = 2;
                Draw.Color = Color.red;

                _transform ??= transform;
                var start = _transform.parent.position;
                var dir = target - start;

                Draw.LineDashed(start, target);
            }
        }
    }
}
