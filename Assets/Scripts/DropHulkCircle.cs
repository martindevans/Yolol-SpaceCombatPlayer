using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class DropHulkCircle
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public override void DrawShapes(Camera cam)
        {
            _parent ??= transform.parent;
            var bot = new Vector3(_parent.position.x, 0, _parent.position.z);

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.DiscGeometry = DiscGeometry.Flat2D;
                Draw.Matrix = Matrix4x4.TRS(bot, Quaternion.Euler(90, 0, 0), Vector3.one);
                Draw.Color = Color.white;
                Draw.RingThickness = 3;
                Draw.RingDashed(Vector3.zero, Quaternion.identity, new DashStyle(2), 55);
            }
        }
    }
}
