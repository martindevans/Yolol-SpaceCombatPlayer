using Shapes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class OrientationDisplay
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public float InnerRadius = 80;
        public float OuterRadius = 120;

        public override void DrawShapes(Camera cam)
        {
            _parent ??= transform.parent;
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            var fwd = -math.normalizesafe(((float3)_parent.forward).xz);
            var fwd3 = new Vector3(fwd.x, 0, fwd.y);
            var start = bot + fwd3 * InnerRadius;
            var end = bot + fwd3 * OuterRadius;

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.Color = Color.green;
                Draw.Thickness = 1.5f;
                Draw.Line(start, end);
            }
        }
    }
}
