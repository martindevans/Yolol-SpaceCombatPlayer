using Shapes;
using UnityEngine;

namespace Assets.Scripts
{
    public class DropLine
        : ImmediateModeShapeDrawer
    {
        private Transform _parent;

        public Color ColorUp = new Color(0, 1, 0, 0.5f);
        public Color ColorDown = new Color(0.1922f, 0.1843f, 0.9994f, 0.5f);

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
                Draw.BlendMode = ShapesBlendMode.Screen;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.Color = top.y > bot.y ? ColorUp : ColorDown;
                Draw.LineThickness = 2;
                Draw.Line(top, bot);
            }
        }
    }
}
