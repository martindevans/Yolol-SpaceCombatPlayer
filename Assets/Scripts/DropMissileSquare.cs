using JetBrains.Annotations;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class DropMissileSquare
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public float Thickness = 2.5f;

        [UsedImplicitly] private void Start()
        {
            _parent = transform.parent;
        }

        public override void DrawShapes(Camera cam)
        {
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 45, 0), Vector3.one);
                Draw.Color = Color.red;
                Draw.RectangleThickness = 2.5f;
                Draw.RectangleBorder(Vector3.zero, new Vector2(25, 25), Thickness);

                Draw.LineEndCaps = LineEndCap.Square;
                Draw.LineThickness = 1;
                Draw.LineGeometry = LineGeometry.Flat2D;
                Draw.Line(new Vector3(0, 14, 0), new Vector3(14, 14, 0));
                Draw.Line(new Vector3(14, 14, 0), new Vector3(14, 0, 0));
                Draw.Line(new Vector3(0, -14, 0), new Vector3(-14, -14, 0));
                Draw.Line(new Vector3(-14, -14, 0), new Vector3(-14, 0, 0));
            }
        }
    }
}