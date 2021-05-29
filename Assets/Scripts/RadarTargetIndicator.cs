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

            if (target.sqrMagnitude < 1)
                return;

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.LineThickness = 2;
                Draw.Color = new Color(1, 0, 0, 0.75f);

                _transform ??= transform;
                var start = _transform.parent.position;

                Draw.LineDashed(start, target);
            }
        }
    }
}
