using Assets.Scripts.Curves;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class DropFuelDisplay
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        private FuelLitersCurve _curve;

        public float FakeMaxFuel;
        public float FakeFuel;

        public override void DrawShapes(Camera cam)
        {
            _curve ??= GetComponentInParent<FuelLitersCurve>();

            var max = _curve?.MaxValue ?? FakeMaxFuel;
            var now = _curve?.Value ?? FakeFuel;

            _parent ??= transform.parent;
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 0, 0), Vector3.one);
                Draw.Color = Color.green;

                Draw.RingThickness = 2;
                Draw.Arc(60, 0, Mathf.PI);
                Draw.Arc(75, 0, Mathf.PI);

                var perc = Mathf.Clamp01(now / max);

                Draw.RingThickness = 12;
                Draw.Arc(67.5f, 0.0275f, Mathf.Lerp(0.0275f, Mathf.PI - 0.0275f, perc));

                Draw.LineThickness = 2;
                Draw.Line(new Vector3(60, 0, 0), new Vector3(75, 0, 0));
                Draw.Line(new Vector3(-60, 0, 0), new Vector3(-75, 0, 0));
            }
        }
    }
}
