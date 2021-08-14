using JetBrains.Annotations;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class DropShellCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        [UsedImplicitly] private void Start()
        {
            _parent = transform.parent;
        }

        public override void DrawShapes(Camera cam)
        {
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 0, 0), Vector3.one);
                Draw.Color = Color.red;

                Draw.Thickness = 2.5f;
                Draw.Ring(10);

                Draw.LineEndCaps = LineEndCap.Square;
                Draw.Thickness = 1;

                //var pi4 = Mathf.PI / 4;
                //Draw.Arc(13, -pi4, pi4);
                //Draw.Arc(13, Mathf.PI - pi4, Mathf.PI + pi4);
            }
        }
    }
}