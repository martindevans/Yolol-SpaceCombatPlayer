using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class DropLine
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public override void DrawShapes(Camera cam)
        {
            _parent ??= transform.parent;
            var top = _parent.position;
            var bot = new Vector3(top.x, 0, top.z);

            if (top.y > 5)
                top.y -= 5;
            else if (top.y < -5)
                top.y += 5;
            else
                return;

            using (Draw.Command(cam))
            {
                Draw.ZTest = CompareFunction.Less;
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.Color = top.y > bot.y ? Color.green : new Color(0.3922f, 0.5843f, 0.9294f, 1);
                Draw.LineThickness = 2;
                Draw.Line(top, bot);
            }
        }
    }
}
