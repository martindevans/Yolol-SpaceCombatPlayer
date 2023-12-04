using Assets.Scripts.Curves;
using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class DebugSphereVisualiser
        : ImmediateModeShapeDrawer
    {
        private DebugSpherePosition _pos;
        private DebugSphereRadius _rad;
        private DebugSphereColor _col;

        public override void OnEnable()
        {
            _pos = GetComponentInParent<DebugSpherePosition>();
            _rad = GetComponentInParent<DebugSphereRadius>();
            _col = GetComponentInParent<DebugSphereColor>();

            base.OnEnable();
        }

        public override void DrawShapes(Camera cam)
        {
            if (_pos == null || _col == null || _rad == null || _rad.Value == 0)
                return;

            var c = _col.Value;
            if (c == Vector3.zero)
                c = new Vector3(1f, 1f, 1f);

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Transparent;

                Draw.Sphere(
                    _pos.Value,
                    _rad.Value,
                    new Color(c.x, c.y, c.z, 0.1f)
                );
            }
        }
    }
}
