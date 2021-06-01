using Assets.Scripts.Curves;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class RadarIndicator
        : ImmediateModeShapeDrawer
    {
        public float InnerRadius = 130;
        public float OuterRadius = 430;
        public float SlerpConstant = 0.1f;

        private Vector3? _current;

        private IRadarDirectionCurve _directionCurve;

        private RadarRangeCurve _rangeCurve;
        private RadarAngleCurve _angleCurve;

        public float FakeAngle;

        public void FixedUpdate()
        {
            _directionCurve ??= GetComponentInParent<IRadarDirectionCurve>();
            _rangeCurve ??= GetComponentInParent<RadarRangeCurve>();
            _angleCurve ??= GetComponentInParent<RadarAngleCurve>();

            var dir = _directionCurve?.Direction ?? Vector3.one.normalized;

            var last = _current;
            if (last.HasValue)
                _current = Vector3.Normalize(Vector3.Slerp(last.Value, dir, SlerpConstant));
            else
                _current = dir;
        }

        public override void DrawShapes(Camera cam)
        {
            if (!Application.isPlaying)
                _current = Vector3.one.normalized;
            else if (!_current.HasValue)
                return;

            if (_current.Value.sqrMagnitude < 0.1f)
                return;

            var range = Mathf.Clamp(_rangeCurve?.Value ?? 0, InnerRadius, OuterRadius);
            var angle = (_angleCurve?.Value ?? FakeAngle);

            var rot = Quaternion.LookRotation(-_current.Value, Vector3.Dot(-_current.Value, Vector3.up) > 0.9 ? Vector3.left : Vector3.up);

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, 0), Vector3.one);
                Draw.LineThickness = 2;
                Draw.Color = Color.red;

                var radius = Mathf.Tan(Mathf.Deg2Rad * angle / 2) * range;
                Draw.Cone(_current.Value * (range + InnerRadius), rot, radius, range);
            }
        }
    }
}
